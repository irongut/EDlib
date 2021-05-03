using EDlib.Platform;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EDlib.Network
{
    /// <summary>Class used to download and cache data.</summary>
    public sealed class CachedDownloadService : IDownloadService
    {
        private static readonly CachedDownloadService instance = new CachedDownloadService();

        private static string agent;

        private static ICacheService cache;

        private static IConnectivityService connectivity;

        private readonly HttpClient client;

        private CachedDownloadService()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Requested-With", agent);
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            client.Timeout = TimeSpan.FromSeconds(40);
        }

        /// <summary>Instantiates the CachedDownloadService class.</summary>
        /// <param name="userAgent">The user agent used for downloads.</param>
        /// <param name="cacheService">The platform specific cache for downloaded data.</param>
        /// <param name="connectivityService">The platform specific connectivity service.</param>
        /// <returns>DownloadService</returns>
        public static CachedDownloadService Instance(string userAgent, ICacheService cacheService, IConnectivityService connectivityService)
        {
            agent = userAgent;
            cache = cacheService;
            connectivity = connectivityService;
            return instance;
        }

        /// <summary>
        /// Gets data from an API, caches the data and when it was last updated with the option to cancel the request.
        /// If a copy of the data exists in the cache and has not expired it will be returned, otherwise the data will be downloaded.
        /// </summary>
        /// <param name="url">The URL for downloading the data.</param>
        /// <param name="options">Options structure for download.</param>
        /// <returns>Task&lt;(string data, DateTime updated)&gt;</returns>
        /// <exception cref="NoNetworkNoCacheException">No Internet available and no data cached.</exception>
        /// <exception cref="APIException">Http errors from the API called.</exception>
        public async Task<(string data, DateTime updated)> GetData(string url, DownloadOptions options)
        {
            string urlHash = Sha256Helper.GenerateHash(url);
            string dataKey = $"{urlHash}-Data";
            string updatedKey = $"{urlHash}-Updated";

            string data;
            DateTime lastUpdated;

            if (!connectivity.IsConnected())
            {
                // no valid connectivity
                if (cache.Exists(dataKey))
                {
                    // use cached data
                    data = cache.Get(dataKey);
                    lastUpdated = DateTime.Parse(cache.Get(updatedKey));
                }
                else
                {
                    throw new NoNetworkNoCacheException("No Internet available and no data cached.");
                }
            }
            else if (!options.IgnoreCache && cache.Exists(dataKey) && !cache.IsExpired(dataKey))
            {
                // use cached data
                data = cache.Get(dataKey);
                lastUpdated = DateTime.Parse(cache.Get(updatedKey));
            }
            else
            {
                // download data
                HttpResponseMessage response;
                if (options.CancelToken == null)
                {
                    response = await client.GetAsync(new Uri(url)).ConfigureAwait(false);
                }
                else
                {
                    response = await client.GetAsync(new Uri(url), options.CancelToken.Token).ConfigureAwait(false);
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new APIException($"{response.StatusCode} - {response.ReasonPhrase}", (int)response.StatusCode);
                }
                else
                {
                    data = await HttpHelper.ReadContentAsync(response).ConfigureAwait(false);
                    lastUpdated = DateTime.Now;
                }

                // cache data
                cache.Add(dataKey, data, options.Expiry);
                cache.Add(updatedKey, lastUpdated.ToString(), options.Expiry);
            }
            return (data, lastUpdated);
        }

        /// <summary>
        /// Posts a request to an API, caches the response and when it was last updated with the option to cancel the request.
        /// If a copy of the response exists in the cache and has not expired it will be returned, otherwise the API will be queried.
        /// </summary>
        /// <param name="url">The URL of the API.</param>
        /// <param name="content">The content of the API request.</param>
        /// <param name="options">Options structure for download.</param>
        /// <returns>Task&lt;(string data, DateTime updated)&gt;</returns>
        /// <exception cref="NoNetworkNoCacheException">No Internet available and no data cached.</exception>
        /// <exception cref="APIException">Http errors from the API called.</exception>
        public async Task<(string data, DateTime updated)> PostData(string url, string content, DownloadOptions options)
        {
            string data;
            DateTime lastUpdated;
            string urlHash = Sha256Helper.GenerateHash(url + content);
            string dataKey = $"{urlHash}-Data";
            string updatedKey = $"{urlHash}-Updated";

            if (!connectivity.IsConnected())
            {
                // no valid connectivity
                if (cache.Exists(dataKey))
                {
                    // use cached data
                    data = cache.Get(dataKey);
                    lastUpdated = DateTime.Parse(cache.Get(updatedKey));
                }
                else
                {
                    throw new NoNetworkNoCacheException("No Internet available and no data cached.");
                }
            }
            else if (!options.IgnoreCache && cache.Exists(dataKey) && !cache.IsExpired(dataKey))
            {
                // use cached data
                data = cache.Get(dataKey);
                lastUpdated = DateTime.Parse(cache.Get(updatedKey));
            }
            else
            {
                // post request
                HttpResponseMessage response;
                StringContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                if (options.CancelToken == null)
                {
                    response = await client.PostAsync(new Uri(url), httpContent).ConfigureAwait(false);
                }
                else
                {
                    response = await client.PostAsync(new Uri(url), httpContent, options.CancelToken.Token).ConfigureAwait(false);
                }

                if (!response.IsSuccessStatusCode)
                {
                    throw new APIException($"{response.StatusCode} - {response.ReasonPhrase}", (int)response.StatusCode);
                }
                else
                {
                    data = await HttpHelper.ReadContentAsync(response).ConfigureAwait(false);
                    lastUpdated = DateTime.Now;
                }

                // cache data
                cache.Add(dataKey, data, options.Expiry);
                cache.Add(updatedKey, lastUpdated.ToString(), options.Expiry);
            }
            return (data, lastUpdated);
        }
    }
}

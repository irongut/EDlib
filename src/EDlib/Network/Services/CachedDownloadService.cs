using EDlib.Platform;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
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
        /// Gets and caches the data and when it was last updated with the option to cancel a download.
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
            return await GetData(url, dataKey, updatedKey, options).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets and caches the data and when it was last updated with the option to cancel a download.
        /// If a copy of the data exists in the cache and has not expired it will be returned, otherwise the data will be downloaded.
        /// </summary>
        /// <param name="url">The URL for downloading the data.</param>
        /// <param name="dataKey">The key for cached data.</param>
        /// <param name="updatedKey">The key for caching when the data was the last updated.</param>
        /// <param name="options">Options structure for download.</param>
        /// <returns>Task&lt;(string data, DateTime updated)&gt;</returns>
        /// <exception cref="NoNetworkNoCacheException">No Internet available and no data cached.</exception>
        /// <exception cref="APIException">Http errors from the API called.</exception>
        public async Task<(string data, DateTime updated)> GetData(string url, string dataKey, string updatedKey, DownloadOptions options)
        {
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
                (data, lastUpdated) = await Download(url, options.CancelToken).ConfigureAwait(false);

                // cache data
                cache.Add(dataKey, data, options.Expiry);
                cache.Add(updatedKey, lastUpdated.ToString(), options.Expiry);
            }
            return (data, lastUpdated);
        }

        private async Task<(string data, DateTime updated)> Download(string url, CancellationTokenSource cancelToken)
        {
            var uri = new Uri(url);
            HttpResponseMessage response;
            if (cancelToken == null)
            {
                response = await client.GetAsync(uri).ConfigureAwait(false);
            }
            else
            {
                response = await client.GetAsync(uri, cancelToken.Token).ConfigureAwait(false);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new APIException($"{response.StatusCode} - {response.ReasonPhrase}", (int)response.StatusCode);
            }
            else
            {
                string data = await HttpHelper.ReadContentAsync(response).ConfigureAwait(false);
                return (data, DateTime.Now);
            }
        }
    }
}

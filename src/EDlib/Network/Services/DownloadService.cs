using EDlib.Platform;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.Network
{
    /// <summary>Internal class used to download and cache data.</summary>
    internal sealed class DownloadService
    {
        private static readonly DownloadService instance = new DownloadService();

        private static string agent;

        private static ICacheService cache;

        private static IConnectivityService connectivity;

        private readonly HttpClient client;

        private DownloadService()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Requested-With", agent);
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            client.Timeout = TimeSpan.FromSeconds(40);
        }

        /// <summary>Instantiates the DownloadService class.</summary>
        /// <param name="userAgent">The user agent used for downloads.</param>
        /// <param name="cacheService">The platform specific cache for downloaded data.</param>
        /// <param name="connectivityService">The platform specific connectivity service.</param>
        /// <returns>DownloadService</returns>
        public static DownloadService Instance(string userAgent, ICacheService cacheService, IConnectivityService connectivityService)
        {
            agent = userAgent;
            cache = cacheService;
            connectivity = connectivityService;
            return instance;
        }

        /// <summary>Gets and caches the data and when it was last updated.
        /// If a copy of the data exists in the cache and has not expired it will be returned, otherwise the data will be downloaded.</summary>
        /// <param name="url">The URL for downloading the data.</param>
        /// <param name="dataKey">The key for cached data.</param>
        /// <param name="lastUpdatedKey">The key for caching when the data was the last updated.</param>
        /// <param name="expiry">How long to cache the data.</param>
        /// <param name="ignoreCache">Ignore any cached data if set to <c>true</c>. (optional)</param>
        /// <returns>Task&lt;(string data, DateTime updated)&gt;</returns>
        /// <exception cref="NoNetworkNoCacheException">No Internet available and no data cached.</exception>
        /// <exception cref="APIException">Http errors from the API called.</exception>
        public async Task<(string data, DateTime updated)> GetData(string url, string dataKey, string lastUpdatedKey, TimeSpan expiry, bool ignoreCache = false)
        {
            return await GetData(url, dataKey, lastUpdatedKey, expiry, null, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets and caches the data and when it was last updated with the option to cancel a download.
        /// If a copy of the data exists in the cache and has not expired it will be returned, otherwise the data will be downloaded.
        /// </summary>
        /// <param name="url">The URL for downloading the data.</param>
        /// <param name="dataKey">The key for cached data.</param>
        /// <param name="lastUpdatedKey">The key for caching when the data was the last updated.</param>
        /// <param name="expiry">How long to cache the data.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="ignoreCache">Ignore any cached data if set to <c>true</c>. (optional)</param>
        /// <returns>Task&lt;(string data, DateTime updated)&gt;</returns>
        /// <exception cref="NoNetworkNoCacheException">No Internet available and no data cached.</exception>
        /// <exception cref="APIException">Http errors from the API called.</exception>
        public async Task<(string data, DateTime updated)> GetData(string url, string dataKey, string lastUpdatedKey, TimeSpan expiry, CancellationTokenSource cancelToken, bool ignoreCache = false)
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
                    lastUpdated = DateTime.Parse(cache.Get(lastUpdatedKey));
                }
                else
                {
                    throw new NoNetworkNoCacheException("No Internet available and no data cached.");
                }
            }
            else if (!ignoreCache && cache.Exists(dataKey) && !cache.IsExpired(dataKey))
            {
                // use cached data
                data = cache.Get(dataKey);
                lastUpdated = DateTime.Parse(cache.Get(lastUpdatedKey));
            }
            else
            {
                // download data
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
                    data = await HttpHelper.ReadContentAsync(response).ConfigureAwait(false);
                    lastUpdated = DateTime.Now;
                    // cache data
                    cache.Add(dataKey, data, expiry);
                    cache.Add(lastUpdatedKey, lastUpdated.ToString(), expiry);
                }
            }
            return (data, lastUpdated);
        }
    }
}

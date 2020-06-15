using EDlib.Platform;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.Network
{
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

        public static DownloadService Instance(string userAgent, ICacheService cacheService, IConnectivityService connectivityService)
        {
            agent = userAgent;
            cache = cacheService;
            connectivity = connectivityService;
            return instance;
        }

        public async Task<(string data, DateTime updated)> GetData(string url, string dataKey, string lastUpdatedKey, TimeSpan expiry, bool ignoreCache = false)
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
                HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
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
                HttpResponseMessage response = await client.GetAsync(uri, cancelToken.Token).ConfigureAwait(false);
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

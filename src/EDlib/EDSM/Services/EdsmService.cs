using EDlib.Network;
using EDlib.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.EDSM
{
    public sealed class EdsmService
    {
        private static readonly EdsmService instance = new EdsmService();

        private const string EdsmURL = "https://www.edsm.net/";

        private static string agent;

        private static ICacheService cache;

        private static IConnectivityService connectivity;

        private EdsmService() { }

        public static EdsmService Instance(string userAgent, ICacheService cacheService, IConnectivityService connectivityService)
        {
            agent = userAgent;
            cache = cacheService;
            connectivity = connectivityService;
            return instance;
        }

        public async Task<(string data, DateTime updated)> GetData(string method, Dictionary<string, string> parameters, TimeSpan expiry, bool ignoreCache = false)
        {
            return await GetData(method, parameters, expiry, null, ignoreCache).ConfigureAwait(false);
        }

        public async Task<(string data, DateTime updated)> GetData(string method, Dictionary<string, string> parameters, TimeSpan expiry, CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            string url = BuildUrl(method, parameters);
            DownloadService downloadService = DownloadService.Instance(agent, cache, connectivity);
            (string json, DateTime lastUpdated) = await downloadService.GetData(url, expiry, cancelToken, ignoreCache).ConfigureAwait(false);
            return (json, lastUpdated);
        }

        private string BuildUrl(string method, Dictionary<string, string> parameters)
        {
            string url = EdsmURL + method;
            if (parameters?.Any() == true)
            {
                if (url.Contains("?"))
                    url += "&";
                else
                    url += "?";
                foreach (var param in parameters)
                {
                    url = $"{url}{param.Key}={param.Value}&";
                }
                url = url.Remove(url.Length - 1);
            }
            return url;
        }
    }
}

using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.EDSM
{
    /// <summary>Gets Elite: Dangerous server status.</summary>
    public sealed class EliteStatusService
    {
        private static readonly EliteStatusService instance = new EliteStatusService();

        private static string agent;

        private static ICacheService cache;

        private static IConnectivityService connectivity;

        private const string edsmMethod = "api-status-v1/elite-server";

        private EliteStatus eliteStatus;
        private DateTime lastUpdated;

        private EliteStatusService() { }

        /// <summary>Instantiates the EliteStatusService class.</summary>
        /// <param name="userAgent">The user agent used for downloads.</param>
        /// <param name="cacheService">The platform specific cache for downloaded data.</param>
        /// <param name="connectivityService">The platform specific connectivity service.</param>
        /// <returns>EliteStatusService</returns>
        public static EliteStatusService Instance(string userAgent, ICacheService cacheService, IConnectivityService connectivityService)
        {
            agent = userAgent;
            cache = cacheService;
            connectivity = connectivityService;
            return instance;
        }

        /// <summary>Gets the status of the Elite: Dangerous server.</summary>
        /// <param name="cacheMinutes">The number of minutes to cache the data.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;NewsArticle&gt;, DateTime)&gt;</returns>
        public async Task<(EliteStatus status, DateTime updated)> GetData(int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetData(null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets the status of the Elite: Dangerous server.</summary>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;NewsArticle&gt;, DateTime)&gt;</returns>
        public async Task<(EliteStatus status, DateTime updated)> GetData(CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (eliteStatus == null || (lastUpdated + expiry < DateTime.Now))
            {
                string json;
                EdsmService edsmService = EdsmService.Instance(agent, cache, connectivity);
                (json, lastUpdated) = await edsmService.GetData(edsmMethod, null, expiry, cancelToken, ignoreCache).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json) || json == "{}")
                {
                    throw new APIException("EDSM method returned no data.");
                }

                eliteStatus = JsonConvert.DeserializeObject<EliteStatus>(json);
            }
            return (eliteStatus, lastUpdated);
        }
    }
}

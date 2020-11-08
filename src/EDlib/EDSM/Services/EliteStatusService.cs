using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EDlib.EDSM
{
    public sealed class EliteStatusService
    {
        private static readonly EliteStatusService instance = new EliteStatusService();

        private static string agent;

        private static ICacheService cache;

        private static IConnectivityService connectivity;

        private const string edsmMethod = "api-status-v1/elite-server";
        private const string dataKey = "EDServerStatus";
        private const string lastUpdatedKey = "EDServerStatusLastUpdated";

        private EliteStatus eliteStatus;
        private DateTime lastUpdated;

        private EliteStatusService() { }

        public static EliteStatusService Instance(string userAgent, ICacheService cacheService, IConnectivityService connectivityService)
        {
            agent = userAgent;
            cache = cacheService;
            connectivity = connectivityService;
            return instance;
        }

        public async Task<(EliteStatus status, DateTime updated)> GetData(int minutes = 5, bool ignoreCache = false)
        {
            if (minutes < 5) minutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(minutes);
            if (eliteStatus == null || (lastUpdated + expiry < DateTime.Now))
            {
                string json;
                EdsmService edsmService = EdsmService.Instance(agent, cache, connectivity);
                (json, lastUpdated) = await edsmService.GetData(edsmMethod, null, dataKey, lastUpdatedKey, expiry, ignoreCache).ConfigureAwait(false);

                eliteStatus = JsonConvert.DeserializeObject<EliteStatus>(json);
            }
            return (eliteStatus, lastUpdated);
        }
    }
}

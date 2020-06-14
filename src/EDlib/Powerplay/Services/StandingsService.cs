using EDlib.Network;
using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.Powerplay
{
    public sealed class StandingsService
    {
        private static readonly StandingsService instance = new StandingsService();

        private static string agent;

        private static ICacheService cache;

        private static IConnectivityService connectivity;

        private const string URL = "https://api.taranissoftware.com/elite-dangerous/galactic-standings.json";
        private const string dataKey = "Standings";
        private const string lastUpdatedKey = "StandingsUpdated";

        private GalacticStandings galacticStandings;
        private DateTime lastUpdated;

        private StandingsService()
        {
            lastUpdated = DateTime.MinValue;
        }

        public static StandingsService Instance(string userAgent, ICacheService cacheService, IConnectivityService connectivityService)
        {
            agent = userAgent;
            cache = cacheService;
            connectivity = connectivityService;
            return instance;
        }

        public async Task<GalacticStandings> GetData(CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            TimeSpan expiry = TimeSpan.FromMinutes(15);
            if ((galacticStandings == null) || (galacticStandings.Cycle != CycleService.CurrentCycle() && (lastUpdated + expiry < DateTime.Now)))
            {
                // download the standings
                string json;
                DownloadService downloadService = DownloadService.Instance(agent, cache, connectivity);
                (json, lastUpdated) = await downloadService.GetData(URL, dataKey, lastUpdatedKey, expiry, cancelToken, ignoreCache).ConfigureAwait(false);

                // parse the standings
                galacticStandings = JsonConvert.DeserializeObject<GalacticStandings>(json);

                // cache till next cycle if updated
                if (galacticStandings.Cycle == CycleService.CurrentCycle())
                {
                    expiry = CycleService.TimeTillTick();
                    cache.Add(dataKey, json, expiry);
                    cache.Add(lastUpdatedKey, lastUpdated.ToString(), expiry);
                }
            }
            return galacticStandings;
        }
    }
}

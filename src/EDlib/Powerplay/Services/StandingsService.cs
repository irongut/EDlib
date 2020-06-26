using EDlib.Network;
using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.Powerplay
{
    /// <summary>Gets the current Powerplay Galactic Standings.</summary>
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

        /// <summary>Instantiates the StandingsService class.</summary>
        /// <param name="userAgent">The user agent used for downloads.</param>
        /// <param name="cacheService">The platform specific cache for downloaded data.</param>
        /// <param name="connectivityService">The platform specific connectivity service.</param>
        /// <returns>StandingsService</returns>
        public static StandingsService Instance(string userAgent, ICacheService cacheService, IConnectivityService connectivityService)
        {
            agent = userAgent;
            cache = cacheService;
            connectivity = connectivityService;
            return instance;
        }

        /// <summary>Gets the current Powerplay Galactic Standings.</summary>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="ignoreCache">Ignore any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;GalacticStandings&gt;</returns>
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

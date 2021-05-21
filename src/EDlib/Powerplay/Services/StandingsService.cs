using EDlib.Network;
using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.Powerplay
{
    /// <summary>
    ///   <para>Gets the current Powerplay Galactic Standings from an API provided by Taranis Software.</para>
    ///   <para>The standings update weekly on a Thursday between 08:00 - 10:00 UTC, the data is cached till 08:00 UTC Thursday.</para>
    /// </summary>
    public sealed class StandingsService
    {
        private static readonly StandingsService instance = new StandingsService();

        private static ICacheService cache;
        private static IDownloadService dService;

        private const string URL = "https://api.taranissoftware.com/elite-dangerous/galactic-standings.json";
        private const string dataKey = "Standings";
        private const string updatedKey = "StandingsUpdated";

        private GalacticStandings galacticStandings;
        private DateTime lastUpdated;

        private StandingsService()
        {
            lastUpdated = DateTime.MinValue;
        }

        /// <summary>Instantiates the StandingsService class.</summary>
        /// <param name="downloadService">IDownloadService instance used to download data.</param>
        /// <param name="cacheService">The platform specific cache for downloaded data.</param>
        /// <returns>StandingsService</returns>
        public static StandingsService Instance(IDownloadService downloadService, ICacheService cacheService)
        {
            dService = downloadService;
            cache = cacheService;
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
                string json;
                if (!ignoreCache && cache.Exists(dataKey) && !cache.IsExpired(dataKey))
                {
                    // use cached data
                    json = cache.Get(dataKey);
                    lastUpdated = DateTime.Parse(cache.Get(updatedKey));
                }
                else
                {
                    try
                    {
                        // download the standings
                        DownloadOptions options = new DownloadOptions(cancelToken, expiry, ignoreCache);
                        (json, lastUpdated) = await dService.GetData(URL, options).ConfigureAwait(false);
                    }
                    catch (NoNetworkNoCacheException) when (cache.Exists(dataKey))
                    {
                        // use cached data
                        json = cache.Get(dataKey);
                        lastUpdated = DateTime.Parse(cache.Get(updatedKey));
                    }
                }

                // parse the standings
                galacticStandings = JsonConvert.DeserializeObject<GalacticStandings>(json);

                // cache till next cycle if updated
                if (galacticStandings.Cycle == CycleService.CurrentCycle())
                {
                    expiry = CycleService.TimeTillTick() + TimeSpan.FromHours(1);
                    cache.Add(dataKey, json, expiry);
                    cache.Add(updatedKey, lastUpdated.ToString(), expiry);
                }
            }
            return galacticStandings;
        }
    }
}

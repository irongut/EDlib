﻿using EDlib.Network;
using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDlib.BGS
{
    /// <summary>Gets the date and time that the Background Simulation (BGS) updates, known as the BGS tick.</summary>
    public sealed class BgsTickService
    {
        private static readonly BgsTickService instance = new BgsTickService();

        private static string agent;

        private static ICacheService cache;

        private static IConnectivityService connectivity;

        private readonly string TickURL = "http://tick.phelbore.com/api/ticks?start={0}&end={1}";

        private List<BgsTick> bgsTicks;
        private DateTime lastUpdated;

        private BgsTickService()
        {
            bgsTicks = new List<BgsTick>();
        }

        /// <summary>Instantiates the BgsTickService class.</summary>
        /// <param name="userAgent">The user agent used for downloads.</param>
        /// <param name="cacheService">The platform specific cache for downloaded data.</param>
        /// <param name="connectivityService">The platform specific connectivity service.</param>
        /// <returns>BgsTickService</returns>
        public static BgsTickService Instance(string userAgent, ICacheService cacheService, IConnectivityService connectivityService)
        {
            agent = userAgent;
            cache = cacheService;
            connectivity = connectivityService;
            return instance;
        }

        /// <summary>Gets the latest BGS tick.</summary>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>BgsTick</returns>
        public async Task<(BgsTick tick, DateTime updated)> GetData(bool ignoreCache = false)
        {
            (List<BgsTick> _, DateTime _) = await GetData(7, ignoreCache).ConfigureAwait(false);

            if (bgsTicks?.Any() == true)
            {
                return (bgsTicks.OrderByDescending(o => o.Time).First(), lastUpdated);
            }
            else
            {
                return (new BgsTick(), lastUpdated);
            }
        }

        /// <summary>Gets multiple recent BGS ticks.</summary>
        /// <param name="days">The required number of days worth of ticks.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;BgsTick&gt;, DateTime)&gt;</returns>
        public async Task<(List<BgsTick> ticks, DateTime updated)> GetData(int days, bool ignoreCache = false)
        {
            TimeSpan expiry = TimeSpan.FromHours(1);
            if (bgsTicks == null || bgsTicks.Count < days || (lastUpdated + expiry < DateTime.Now))
            {
                string json;
                string queryURL = String.Format(TickURL,
                                                DateTime.UtcNow.Date.AddDays(0 - days).ToString("yyyy-MM-dd"),
                                                DateTime.UtcNow.Date.ToString("yyyy-MM-dd"));

                // download the json
                CachedDownloadService downloadService = CachedDownloadService.Instance(agent, cache, connectivity);
                (json, lastUpdated) = await downloadService.GetData(queryURL, expiry, ignoreCache).ConfigureAwait(false);

                // parse the data
                bgsTicks = JsonConvert.DeserializeObject<List<BgsTick>>(json);
            }
            return (bgsTicks.Take(days).ToList(), lastUpdated);
        }
    }
}

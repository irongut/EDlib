﻿using EDlib.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace EDlib.BGS
{
    /// <summary>Gets the date and time that the Background Simulation (BGS) updates, known as the BGS tick.</summary>
    public sealed class BgsTickService
    {
        private static readonly BgsTickService instance = new BgsTickService();

        private static IDownloadService dService;

        private readonly string TickURL = "http://tick.phelbore.com/api/ticks?start={0}&end={1}";

        private List<BgsTick> bgsTicks;
        private DateTime lastUpdated;

        private BgsTickService()
        {
            bgsTicks = new List<BgsTick>();
        }

        /// <summary>Instantiates the BgsTickService class.</summary>
        /// <param name="downloadService">IDownloadService instance used to download data.</param>
        /// <returns>BgsTickService</returns>
        public static BgsTickService Instance(IDownloadService downloadService)
        {
            dService = downloadService;
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
            if (bgsTicks == null || bgsTicks.Count < days || (lastUpdated + expiry < DateTime.Now) || ignoreCache)
            {
                string json;
                CultureInfo culture = new CultureInfo("en-GB");
                string queryURL = string.Format(TickURL,
                                                DateTime.UtcNow.Date.AddDays(0 - days).ToString("yyyy-MM-dd", culture),
                                                DateTime.UtcNow.Date.ToString("yyyy-MM-dd", culture));

                // download the json
                DownloadOptions options = new DownloadOptions(expiry, ignoreCache);
                (json, lastUpdated) = await dService.GetData(queryURL, options).ConfigureAwait(false);

                // parse the data
                bgsTicks = JsonConvert.DeserializeObject<List<BgsTick>>(json);
            }
            return (bgsTicks?.Take(days).ToList(), lastUpdated);
        }
    }
}

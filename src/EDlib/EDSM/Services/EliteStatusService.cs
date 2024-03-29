﻿using EDlib.Network;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.EDSM
{
    /// <summary>
    ///   <para>Gets the status of the Elite: Dangerous game servers.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-status-v1">Status v1</a>.</para>
    /// </summary>
    public sealed class EliteStatusService
    {
        private static readonly EliteStatusService instance = new EliteStatusService();

        private static IEdsmService EdsmService;

        private const string edsmMethod = "api-status-v1/elite-server";

        private EliteStatus eliteStatus;
        private DateTime lastUpdated;

        private EliteStatusService() { }

        /// <summary>Instantiates the EliteStatusService class.</summary>
        /// <param name="edsmService">IEdsmService instance used to download data from EDSM.</param>
        /// <returns>EliteStatusService</returns>
        public static EliteStatusService Instance(IEdsmService edsmService)
        {
            EdsmService = edsmService;
            return instance;
        }

        /// <summary>Gets the status of the Elite: Dangerous server.</summary>
        /// <param name="cacheMinutes">The number of minutes to cache the data.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;NewsArticle&gt;, DateTime)&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        public async Task<(EliteStatus status, DateTime updated)> GetData(int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetData(null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets the status of the Elite: Dangerous server.</summary>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;NewsArticle&gt;, DateTime)&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        public async Task<(EliteStatus status, DateTime updated)> GetData(CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (eliteStatus == null || (lastUpdated + expiry < DateTime.Now) || ignoreCache)
            {
                string json;
                DownloadOptions options = new DownloadOptions(cancelToken, expiry, ignoreCache);
                (json, lastUpdated) = await EdsmService.GetData(edsmMethod, null, options).ConfigureAwait(false);

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

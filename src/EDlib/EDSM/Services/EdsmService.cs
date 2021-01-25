﻿using EDlib.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.EDSM
{
    /// <summary>Utility service used by other services to get data from EDSM APIs.</summary>
    public sealed class EdsmService
    {
        private static readonly EdsmService instance = new EdsmService();

        private const string EdsmURL = "https://www.edsm.net/";

        private static IDownloadService dService;

        private EdsmService() { }

        /// <summary>Instantiates the EdsmService class.</summary>
        /// <param name="downloadService">IDownloadService instance used to download data.</param>
        /// <returns>EdsmService</returns>
        public static EdsmService Instance(IDownloadService downloadService)
        {
            dService = downloadService;
            return instance;
        }

        /// <summary>Gets data from an EDSM API.</summary>
        /// <param name="method">The EDSM API method.</param>
        /// <param name="parameters">The parameters for the API method.</param>
        /// <param name="expiry">The expiry time for cached data.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(string, DateTime)&gt;<br /></returns>
        public async Task<(string data, DateTime updated)> GetData(string method, Dictionary<string, string> parameters, TimeSpan expiry, bool ignoreCache = false)
        {
            return await GetData(method, parameters, expiry, null, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets data from an EDSM API with the option to cancel the download.</summary>
        /// <param name="method">The EDSM API method.</param>
        /// <param name="parameters">The parameters for the API method.</param>
        /// <param name="expiry">The expiry time for cached data.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(string, DateTime)&gt;<br /></returns>
        public async Task<(string data, DateTime updated)> GetData(string method, Dictionary<string, string> parameters, TimeSpan expiry, CancellationTokenSource cancelToken, bool ignoreCache = false)
        {
            string url = BuildUrl(method, parameters);
            DownloadOptions options = new DownloadOptions(cancelToken, expiry, ignoreCache);
            (string json, DateTime lastUpdated) = await dService.GetData(url, options).ConfigureAwait(false);
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

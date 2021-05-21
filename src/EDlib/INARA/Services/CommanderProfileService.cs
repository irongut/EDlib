using EDlib.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.INARA
{
    internal struct SearchNameParameter
    {
        [JsonProperty(PropertyName = "searchName")]
        public string SearchName { get; }

        public SearchNameParameter(string name)
        {
            SearchName = name;
        }
    }

    /// <summary>
    ///   <para>Gets basic information about a Commander from the INARA API like ranks, squadron and allegiance.</para>
    ///   <para>See INARA documentation for <a href="https://inara.cz/inara-api-docs/#event-2">getCommanderProfile</a>.</para>
    ///   <para>Note: The information returned will be determined by the Commander's privacy settings on INARA.</para>
    /// </summary>
    public sealed class CommanderProfileService
    {
        private static readonly CommanderProfileService instance = new CommanderProfileService();

        private static IDownloadService dService;

        private const string eventName = "getCommanderProfile";

        private CommanderProfile commander;
        private string cachedName;

        private CommanderProfileService() { }

        /// <summary>Instantiates the CommanderProfileService class.</summary>
        /// <param name="downloadService">IDownloadService instance used to download data.</param>
        /// <returns>CommanderProfileService</returns>
        public static CommanderProfileService Instance(IDownloadService downloadService)
        {
            dService = downloadService;
            return instance;
        }

        /// <summary>Gets basic information about a Commander from the INARA API.</summary>
        /// <param name="searchName">The Commander's name to search for. If an exact match is not found a number of partial matches may be returned.</param>
        /// <param name="cacheMinutes">How long to cache the data in minutes, minimum 5 minutes.</param>
        /// <param name="identity">The <see cref="InaraIdentity" /> credentials required to access the INARA API.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;CommanderProfile&gt;</returns>
        public async Task<CommanderProfile> GetData(string searchName,
                                                    int cacheMinutes,
                                                    InaraIdentity identity,
                                                    CancellationTokenSource cancelToken,
                                                    bool ignoreCache = false)
        {
            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);

            if (commander == null || !searchName.Equals(cachedName, StringComparison.OrdinalIgnoreCase) || commander.LastUpdated + expiry < DateTime.Now)
            {
                // request data
                DownloadOptions options = new DownloadOptions(cancelToken, expiry, ignoreCache);
                InaraService inaraService = InaraService.Instance(dService);

                List<InaraEvent> input = new List<InaraEvent>
                {
                    new InaraEvent(eventName, new SearchNameParameter(searchName))
                };

                (string json, _) = await inaraService.GetData(new InaraHeader(identity), input, options).ConfigureAwait(false);

                // parse commander profile
                InaraRequest outputData = JsonConvert.DeserializeObject<InaraRequest>(json);
                foreach (InaraEvent item in outputData.Events)
                {
                    if (item.EventData != null)
                    {
                        commander = JsonConvert.DeserializeObject<CommanderProfile>(item.EventData.ToString());
                        cachedName = searchName;
                    }
                }
            }
            return commander;
        }
    }
}

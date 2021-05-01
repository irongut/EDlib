using EDlib.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.INARA
{
    public struct SearchNameParameter
    {
        [JsonProperty(PropertyName = "searchName")]
        public string SearchName { get; }

        public SearchNameParameter(string name)
        {
            SearchName = name;
        }
    }

    public sealed class CommanderProfileService
    {
        private static readonly CommanderProfileService instance = new CommanderProfileService();

        private static IDownloadService dService;

        private const string eventName = "getCommanderProfile";

        private CommanderProfile commander;

        private CommanderProfileService() { }

        public static CommanderProfileService Instance(IDownloadService downloadService)
        {
            dService = downloadService;
            return instance;
        }

        public async Task<CommanderProfile> GetData(string searchName,
                                                    int cacheMinutes,
                                                    InaraIdentity identity,
                                                    CancellationTokenSource cancelToken,
                                                    bool ignoreCache = false)
        {
            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);

            if (commander == null || commander.LastUpdated + expiry < DateTime.Now)
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
                    }
                }
            }
            return commander;
        }
    }
}

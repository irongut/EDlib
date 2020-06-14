using EDlib.Network;
using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EDlib.Powerplay
{
    public sealed class PowerDetailsService
    {
        private static readonly PowerDetailsService instance = new PowerDetailsService();

        private static string agent;

        private static ICacheService cache;

        private static IConnectivityService connectivity;

        private const string URL = "https://api.taranissoftware.com/elite-dangerous/power-comms.json";
        private const string dataKey = "PowerComms";
        private const string lastUpdatedKey = "PowerCommsUpdated";

        private List<PowerDetails> powerList;
        private List<PowerComms> commsList;

        private PowerDetailsService()
        {
            powerList = new List<PowerDetails>();
            commsList = new List<PowerComms>();
        }

        public static PowerDetailsService Instance(string userAgent, ICacheService cacheService, IConnectivityService connectivityService)
        {
            agent = userAgent;
            cache = cacheService;
            connectivity = connectivityService;
            return instance;
        }

        public PowerDetails GetPowerDetails(string shortName)
        {
            if (powerList?.Any() == false)
            {
                GetPowerList();
            }
            return powerList.Find(x => x.ShortName.Equals(shortName));
        }

        public async Task<PowerComms> GetPowerCommsAsync(string shortName)
        {
            if (commsList?.Any() == false)
            {
                await GetPowerCommsListAsync().ConfigureAwait(false);
            }
            return commsList.Find(x => x.ShortName.Equals(shortName));
        }

        private void GetPowerList()
        {
            const string fileName = "EDlib.Powerplay.Resources.PowerDetails.json";

            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(fileName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    powerList = (List<PowerDetails>)serializer.Deserialize(reader, typeof(List<PowerDetails>));
                }
            }
        }

        private async Task GetPowerCommsListAsync()
        {
            string json;
            TimeSpan expiry = TimeSpan.FromDays(1);
            DownloadService downloadService = DownloadService.Instance(agent, cache, connectivity);
            (json, _) = await downloadService.GetData(URL, dataKey, lastUpdatedKey, expiry).ConfigureAwait(false);
            commsList = JsonConvert.DeserializeObject<List<PowerComms>>(json);
        }
    }
}

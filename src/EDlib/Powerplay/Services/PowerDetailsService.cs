using EDlib.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EDlib.Powerplay
{
    /// <summary>Gets a Power's statistics, ethos, benefits and comms data.</summary>
    public sealed class PowerDetailsService
    {
        private static readonly PowerDetailsService instance = new PowerDetailsService();

        private static IDownloadService dService;

        private const string URL = "https://api.taranissoftware.com/elite-dangerous/power-comms.json";

        private List<PowerDetails> powerList;
        private List<PowerComms> commsList;

        private PowerDetailsService()
        {
            powerList = new List<PowerDetails>();
            commsList = new List<PowerComms>();
        }

        /// <summary>Instantiates the PowerDetailsService class.</summary>
        /// <param name="downloadService">IDownloadService instance used to download data.</param>
        /// <returns>PowerDetailsService</returns>
        public static PowerDetailsService Instance(IDownloadService downloadService)
        {
            dService = downloadService;
            return instance;
        }

        /// <summary>Gets a Power's statistics, ethos and benefits data from an embedded json resource.</summary>
        /// <param name="shortName">The required Power's short name.</param>
        /// <returns>PowerDetals</returns>
        public PowerDetails GetPowerDetails(string shortName)
        {
            if (powerList?.Any() == false)
            {
                GetPowerList();
            }
            return powerList.Find(x => x.ShortName.Equals(shortName));
        }

        /// <summary>Gets the comms data for a Power from an online resource - Subreddit and Discord / Slack servers.</summary>
        /// <param name="shortName">The required Power's short name.</param>
        /// <param name="cacheDays">The number of days to cache the Power comms data.</param>
        /// <returns>Task&lt;PowerComms&gt;</returns>
        public async Task<PowerComms> GetPowerCommsAsync(string shortName, int cacheDays)
        {
            if (commsList?.Any() == false)
            {
                await GetPowerCommsListAsync(cacheDays).ConfigureAwait(false);
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

        private async Task GetPowerCommsListAsync(int cacheDays)
        {
            string json;
            if (cacheDays < 1)
            {
                cacheDays = 1;
            }
            TimeSpan expiry = TimeSpan.FromDays(cacheDays);
            DownloadOptions options = new DownloadOptions(null, expiry);
            (json, _) = await dService.GetData(URL, options).ConfigureAwait(false);
            commsList = JsonConvert.DeserializeObject<List<PowerComms>>(json);
        }
    }
}

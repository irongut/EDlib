using EDlib.EDSM;
using EDlib.GalNet;
using EDlib.Mock.Platform;
using EDlib.Network;
using EDlib.Powerplay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EDlibDemo
{
    internal static class Program
    {
        private const string userAgent = "EDlib Demo";
        private static HttpClient client;

        private static async Task Main()
        {
            try
            {
                SetupClient();

                EliteStatusService statusService = EliteStatusService.Instance(DownloadService.Instance(userAgent, new UnmeteredConnection()));
                (EliteStatus eliteStatus, DateTime lastUpdated) = await statusService.GetData().ConfigureAwait(false);
                Console.WriteLine(eliteStatus.ToString());
                Console.WriteLine(string.Empty);

                string json = await GetDataAsync().ConfigureAwait(false);
                GalacticStandings galacticStandings = JsonConvert.DeserializeObject<GalacticStandings>(json);
                Console.WriteLine(galacticStandings.ToString());

                Random rand = new Random();
                string shortName = galacticStandings.Standings[rand.Next(10)].ShortName;
                PowerDetailsService powerService = PowerDetailsService.Instance(DownloadService.Instance(userAgent, new UnmeteredConnection()));
                PowerDetails powerDetails = powerService.GetPowerDetails(shortName);
                Console.WriteLine($"Random Power: {powerDetails}");

                PowerComms powerCommms = await powerService.GetPowerCommsAsync(shortName, 1).ConfigureAwait(false);
                Console.WriteLine($"Comms: {powerCommms}");
                Console.WriteLine(string.Empty);

                GalNetService gnService = GalNetService.Instance(DownloadService.Instance(userAgent, new UnmeteredConnection()));
                (List<NewsArticle> newsList, DateTime updated) = await gnService.GetData(20, 1, null).ConfigureAwait(false);
                Console.WriteLine("### GalNet News ###");
                Console.WriteLine(string.Empty);
                foreach (NewsArticle article in newsList)
                {
                    Console.WriteLine($"{article.Title} ({article.Topic})");
                    foreach (string tag in article.Tags)
                    {
                        Console.Write($"{tag} ");
                    }
                    Console.Write(Environment.NewLine);
                    Console.WriteLine(string.Empty);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }

        private static void SetupClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Requested-With", userAgent);
            client.Timeout = TimeSpan.FromSeconds(40);
        }

        private static async Task<string> GetDataAsync()
        {
            var uri = new Uri("https://api.taranissoftware.com/elite-dangerous/galactic-standings.json");
            HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"{response.StatusCode} - {response.ReasonPhrase}");
            }
            else
            {
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}

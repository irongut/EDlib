using EDlib.Powerplay;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EDlibDemo
{
    internal static class Program
    {
        private static HttpClient client;

        private static async Task Main()
        {
            try
            {
                SetupClient();

                string json = await GetDataAsync().ConfigureAwait(false);
                GalacticStandings galacticStandings = JsonConvert.DeserializeObject<GalacticStandings>(json);

                Console.WriteLine(galacticStandings.ToString());
                Console.WriteLine("");

                Random rand = new Random();
                string shortName = galacticStandings.Standings[rand.Next(10)].ShortName;
                PowerDetailsService powerService = PowerDetailsService.Instance("EDlib Demo", null, null);
                PowerDetails powerDetails = powerService.GetPowerDetails(shortName);

                Console.WriteLine(powerDetails.ToString());
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }

        private static void SetupClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Requested-With", "EDlib Demo");
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

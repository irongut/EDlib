using EDlib.Standings;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("ERROR: {0}", ex.Message));
            }
        }

        private static void SetupClient()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Requested-With", "EDlib");
            client.Timeout = TimeSpan.FromSeconds(40);
        }

        private static async Task<string> GetDataAsync()
        {
            var uri = new Uri("https://api.taranissoftware.com/elite-dangerous/galactic-standings.json");
            HttpResponseMessage response = await client.GetAsync(uri).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(String.Format("{0} - {1}", response.StatusCode, response.ReasonPhrase));
            }
            else
            {
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}

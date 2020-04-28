using EDlib.Standings;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EDlibDemo
{
    internal static class Program
    {
        private static HttpClient client;

        private static async Task Main()
        {
            try
            {
                // download Mahon Reddit rss
                SetupClient();
                string rss = await GetDataAsync().ConfigureAwait(false);

                // get standings article
                var xdoc = XDocument.Parse(rss);
                var entries = from item in xdoc.Descendants("{http://www.w3.org/2005/Atom}entry")
                              where item.Element("{http://www.w3.org/2005/Atom}title").Value.Contains(string.Format("Week {0} Powerplay Standings", CycleService.CurrentCycle()))
                              select item;

                if (entries?.Any() == false)
                {
                    throw new Exception("Current Cycle Standings Not Found");
                }
                else if (entries.Count() > 1)
                {
                    throw new Exception("Multiple Entries Found For Current Cycle");
                }

                // parse standings article
                var article = entries.First();
                string list = StripTags(article.Element("{http://www.w3.org/2005/Atom}content").Value, "ol");
                DateTime updated = DateTime.Now;
                GalacticStandings galacticStandings = new GalacticStandings(CycleService.CurrentCycle(), updated);
                int position = 0;
                while (list.IndexOf("</li>") > -1)
                {
                    position++;
                    galacticStandings.Standings.Add(new PowerStanding(position, StripTags(list, "li"), CycleService.CurrentCycle(), updated));
                    list = list.Substring(list.IndexOf("</li>") + 5);
                }

                // output standings article
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
            var uri = new Uri("https://www.reddit.com/r/EliteMahon.rss");
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

        private static string StripTags(string content, string tag)
        {
            string openTag = string.Format("<{0}>", tag);
            int start = content.IndexOf(openTag) + openTag.Length;
            int end = content.IndexOf(string.Format("</{0}>", tag));
            return content[start..end];
        }
    }
}

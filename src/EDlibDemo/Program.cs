using EDlib;
using EDlib.EDSM;
using EDlib.GalNet;
using EDlib.INARA;
using EDlib.Mock.Platform;
using EDlib.Network;
using EDlib.Powerplay;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EDlibDemo
{
    internal class Program
    {
        private const string userAgent = "EDlib Demo";
        private static HttpClient client;
        private static IConfigurationRoot config;
        private static string appName;
        private static InaraIdentity identity;

        private static async Task Main()
        {
            try
            {
                SetupClient();

                Console.WriteLine("### Server Status ###");
                EliteStatusService statusService = EliteStatusService.Instance(DownloadService.Instance(userAgent, new UnmeteredConnection()));
                (EliteStatus eliteStatus, DateTime lastUpdated) = await statusService.GetData().ConfigureAwait(false);
                Console.WriteLine(eliteStatus.ToString());
                Console.WriteLine();

                Console.WriteLine("### Powerplay Cycle ###");
                Console.WriteLine($"Current Cycle: {CycleService.CurrentCycle()}");
                Console.WriteLine($"Time Remaining: {CycleService.TimeRemaining()}");
                Console.WriteLine($"Timespan Remaining: {CycleService.TimeTillTick()}");
                Console.WriteLine($"Final Day: {CycleService.FinalDay()}");
                Console.WriteLine($"Cycle End Imminent: {CycleService.CycleImminent()}");
                Console.WriteLine();

                Console.WriteLine("### Galactic Standings ###");
                StandingsService standingsService = StandingsService.Instance(DownloadService.Instance(userAgent, new UnmeteredConnection()), new EmptyCache());
                GalacticStandings galacticStandings = await standingsService.GetData(new CancellationTokenSource()).ConfigureAwait(false);
                Console.WriteLine(galacticStandings.ToString());

                Random rand = new();
                string shortName = galacticStandings.Standings[rand.Next(10)].ShortName;
                PowerDetailsService powerService = PowerDetailsService.Instance(DownloadService.Instance(userAgent, new UnmeteredConnection()));
                PowerDetails powerDetails = powerService.GetPowerDetails(shortName);
                Console.WriteLine($"Random Power: {powerDetails}");

                PowerComms powerCommms = await powerService.GetPowerCommsAsync(shortName, 1).ConfigureAwait(false);
                Console.WriteLine($"Comms: {powerCommms}");
                Console.WriteLine();

                Console.WriteLine("### GalNet News ###");
                GalNetService gnService = GalNetService.Instance(DownloadService.Instance(userAgent, new UnmeteredConnection()));
                (List<NewsArticle> newsList, DateTime _) = await gnService.GetData(5, 1, null).ConfigureAwait(false);
                foreach (NewsArticle article in newsList)
                {
                    Console.WriteLine($"{article.Topic}: {article.Title}");
                    foreach (string tag in article.Tags)
                    {
                        Console.Write($"{tag} ");
                    }
                    Console.Write(Environment.NewLine);
                    Console.WriteLine();
                }

                Console.WriteLine("### Community Goals ###");
                InitialiseInara();
                CommunityGoalsService cgService = CommunityGoalsService.Instance(DownloadService.Instance(appName, new UnmeteredConnection()));
                try
                {
                    (List<CommunityGoal> cgList, DateTime _) = await cgService.GetData(2, 60, identity, null).ConfigureAwait(false);
                    foreach (CommunityGoal goal in cgList)
                    {
                        Console.WriteLine($"{goal.CommunityGoalName}");
                        Console.WriteLine($"Location: {goal.StarsystemName} - {goal.StationName}");
                        Console.WriteLine($"Type: {goal.Topic}, Tier: {goal.TierReached} / {goal.TierMax} ");
                        Console.WriteLine($"Expires: {goal.GoalExpiry} {(goal.IsCompleted ? "Completed" : "Active")}");
                        Console.WriteLine();
                    }
                }
                catch (APIException ex)
                {
                    Console.WriteLine($"API Error: {ex.Message}");
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

        private static void InitialiseInara()
        {
            if (config == null)
            {
                config = new ConfigurationBuilder()
                             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                             .AddJsonFile("appsettings.json")
                             .AddUserSecrets<Program>()
                             .Build();

                appName = config["Inara-AppName"];
                identity = new(appName,
                               config["Inara-AppVersion"],
                               config["Inara-ApiKey"],
                               bool.Parse(config["Inara-IsDeveloped"]));
            }
        }
    }
}

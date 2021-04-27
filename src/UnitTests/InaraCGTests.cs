using EDlib.INARA;
using EDlib.Mock.Platform;
using EDlib.Network;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class InaraCGTests
    {
        [TestMethod]
        public async Task CommunityGoalsTest()
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile("appsettings.json")
                            .AddUserSecrets<InaraCGTests>()
                            .Build();

            string appName = config["Inara-AppName"];
            CommunityGoalsService cgService = CommunityGoalsService.Instance(DownloadService.Instance(appName, new UnmeteredConnection()));

            InaraIdentity identity = new(appName,
                                         config["Inara-AppVersion"],
                                         config["Inara-ApiKey"],
                                         bool.Parse(config["Inara-IsDeveloped"]));

            (List<CommunityGoal> cgList, DateTime updated) = await cgService.GetData(60, 28, identity, new CancellationTokenSource()).ConfigureAwait(false);
            Assert.IsTrue(cgList.Count > 1);
            Assert.IsTrue(updated > DateTime.MinValue);

            int max = cgList.Count > 4 ? 4 : cgList.Count;
            for (int i = 0; i < max; i++)
            {
                CommunityGoal cg = cgList[i];
                Assert.IsFalse(string.IsNullOrWhiteSpace(cg.CommunityGoalName));
                Assert.IsTrue(cg.CommunityGoalGameID > 0);
                Assert.IsFalse(string.IsNullOrWhiteSpace(cg.StarsystemName));
                Assert.IsFalse(string.IsNullOrWhiteSpace(cg.StationName));
                Assert.IsTrue(cg.GoalExpiry > DateTime.MinValue);
                Assert.IsTrue(cg.TierReached >= 0);
                Assert.IsTrue(cg.TierMax > 0);
                Assert.IsTrue(cg.ContributorsNum >= 0);
                Assert.IsTrue(cg.ContributionsTotal >= 0);
                if (cg.TierReached == cg.TierMax || cg.TimeRemaining == TimeSpan.Zero)
                {
                    Assert.IsTrue(cg.IsCompleted);
                }
                else
                {
                    Assert.IsFalse(cg.IsCompleted);
                }
                Assert.IsTrue(cg.LastUpdate > DateTime.MinValue);
                Assert.IsFalse(string.IsNullOrWhiteSpace(cg.GoalObjectiveText));
                Assert.IsNotNull(cg.GoalRewardText); // can be empty
                Assert.IsFalse(string.IsNullOrWhiteSpace(cg.GoalDescriptionText));
                Assert.IsFalse(string.IsNullOrWhiteSpace(cg.InaraURL));
                Assert.IsTrue(cg.TimeRemaining >= TimeSpan.Zero);
                Assert.IsTrue(cg.Progress >= 0.0);
                Assert.IsFalse(string.IsNullOrWhiteSpace(cg.ProgressText));
                Assert.IsFalse(string.IsNullOrWhiteSpace(cg.Topic));
            }
        }
    }
}

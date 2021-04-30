using EDlib.INARA;
using EDlib.Mock.Platform;
using EDlib.Network;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class InaraCGTests
    {
        private IConfigurationRoot config;
        private string appName;
        private InaraIdentity identity;

        [TestMethod]
        public async Task CommunityGoalTest()
        {
            InitialiseInaraTests();

            CommunityGoalsService cgService = CommunityGoalsService.Instance(DownloadService.Instance(appName, new UnmeteredConnection()));
            (List<CommunityGoal> cgList, DateTime updated) = await cgService.GetData(1, 60, identity, new CancellationTokenSource()).ConfigureAwait(false);
            Assert.AreEqual(1, cgList.Count);
            Assert.IsTrue(updated > DateTime.MinValue);

            CommunityGoal cg = cgList[0];
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

        [TestMethod]
        public async Task CommunityGoalAltBoWTest()
        {
            InitialiseInaraTests();

            CommunityGoalsService cgService = CommunityGoalsService.Instance(DownloadService.Instance(appName, new UnmeteredConnection()));
            string BoW = LoadBoW("UnitTests.Resources.CGBoW.json");
            (List<CommunityGoal> cgList, DateTime updated) = await cgService.GetData(1, 60, identity, new CancellationTokenSource(), BoW).ConfigureAwait(false);
            Assert.AreEqual(1, cgList.Count);
            Assert.IsTrue(updated > DateTime.MinValue);

            CommunityGoal cg = cgList[0];
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

        [TestMethod]
        public async Task CommunityGoalsTest()
        {
            InitialiseInaraTests();

            CommunityGoalsService cgService = CommunityGoalsService.Instance(DownloadService.Instance(appName, new UnmeteredConnection()));
            (List<CommunityGoal> cgList, DateTime updated) = await cgService.GetData(60, identity, new CancellationTokenSource()).ConfigureAwait(false);
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

        [TestMethod]
        public async Task CommunityGoalsByTimeTest()
        {
            InitialiseInaraTests();

            CommunityGoalsService cgService = CommunityGoalsService.Instance(DownloadService.Instance(appName, new UnmeteredConnection()));
            (List<CommunityGoal> cgList, DateTime updated) = await cgService.GetDataByTime(28, 60, identity, new CancellationTokenSource()).ConfigureAwait(false);
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

        private void InitialiseInaraTests()
        {
            if (config == null)
            {
                config = new ConfigurationBuilder()
                             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                             .AddJsonFile("appsettings.json")
                             .AddUserSecrets<InaraCGTests>()
                             .Build();

                appName = config["Inara-AppName"];
                identity = new(appName,
                               config["Inara-AppVersion"],
                               config["Inara-ApiKey"],
                               bool.Parse(config["Inara-IsDeveloped"]));
            }
        }

        private string LoadBoW(string filename)
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(filename))
            {
                using (StreamReader reader = new(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}

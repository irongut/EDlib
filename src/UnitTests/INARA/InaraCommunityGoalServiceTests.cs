using EDlib.INARA;
using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class InaraCommunityGoalServiceTests
    {
        [TestMethod]
        public async Task CG_SingleGoal_Get_Test()
        {
            DateTime timestamp = DateTime.Now;
            string inaraData = JsonConvert.SerializeObject(GetCGRequestData(timestamp));

            Mock<IInaraService> mockInara = new();
            mockInara.Setup(x => x.GetData(It.IsAny<InaraHeader>(), It.IsAny<IList<InaraEvent>>(), It.IsAny<DownloadOptions>()).Result).Returns((inaraData, timestamp));

            CommunityGoalsService cgService = CommunityGoalsService.Instance(mockInara.Object);
            (List<CommunityGoal> goals, DateTime updated) = await cgService.GetData(
                1,
                60,
                new InaraIdentity("name", "version", "key", true),
                new CancellationTokenSource(),
                ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(goals);
            Assert.AreEqual(1, goals.Count);
            Assert.AreEqual(704, goals[0].CommunityGoalGameID);
            Assert.AreEqual("Caine Massey Mining Initiative", goals[0].CommunityGoalName);
            Assert.AreEqual("Dulos", goals[0].StarsystemName);
            Assert.AreEqual("Smith Port", goals[0].StationName);
            Assert.AreEqual(timestamp - TimeSpan.FromDays(1), goals[0].GoalExpiry);
            Assert.AreEqual(2, goals[0].TierReached);
            Assert.AreEqual(5, goals[0].TierMax);
            Assert.AreEqual(1458, goals[0].ContributorsNum);
            Assert.AreEqual(485271, goals[0].ContributionsTotal);
            Assert.IsTrue(goals[0].IsCompleted);
            Assert.AreEqual(timestamp, goals[0].LastUpdate);
            Assert.AreEqual("Deliver Gallite, Bromellite and Samarium", goals[0].GoalObjectiveText);
            Assert.IsTrue(string.IsNullOrWhiteSpace(goals[0].GoalRewardText));
            Assert.IsFalse(string.IsNullOrWhiteSpace(goals[0].GoalDescriptionText));
            Assert.AreEqual("https://inara.cz/galaxy-communitygoals/", goals[0].InaraURL);
            Assert.AreEqual(TimeSpan.Zero, goals[0].TimeRemaining);
            Assert.AreEqual(0.4, goals[0].Progress);
            Assert.AreEqual("Progress: 2 / 5 Tiers", goals[0].ProgressText);
            Assert.AreEqual("Mining", goals[0].Topic);

            mockInara.Verify(x => x.GetData(It.IsAny<InaraHeader>(), It.IsAny<IList<InaraEvent>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task CG_Get_Test()
        {
            DateTime timestamp = DateTime.Now;
            string inaraData = JsonConvert.SerializeObject(GetCGRequestData(timestamp));

            Mock<IInaraService> mockInara = new();
            mockInara.Setup(x => x.GetData(It.IsAny<InaraHeader>(), It.IsAny<IList<InaraEvent>>(), It.IsAny<DownloadOptions>()).Result).Returns((inaraData, timestamp));

            CommunityGoalsService cgService = CommunityGoalsService.Instance(mockInara.Object);
            (List<CommunityGoal> goals, DateTime updated) = await cgService.GetData(
                60,
                new InaraIdentity("name", "version", "key", true),
                new CancellationTokenSource(),
                ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(goals);
            Assert.AreEqual(3, goals.Count);
            Assert.AreEqual(695, goals[2].CommunityGoalGameID);
            Assert.AreEqual("Fight for the Neo-Marlinist Order of Mudhrid", goals[2].CommunityGoalName);
            Assert.AreEqual("Mudhrid", goals[2].StarsystemName);
            Assert.AreEqual("Fairfax Vision", goals[2].StationName);
            Assert.AreEqual(timestamp - TimeSpan.FromDays(15), goals[2].GoalExpiry);
            Assert.AreEqual(4, goals[2].TierReached);
            Assert.AreEqual(6, goals[2].TierMax);
            Assert.AreEqual(1342, goals[2].ContributorsNum);
            Assert.AreEqual(75546464873, goals[2].ContributionsTotal);
            Assert.IsTrue(goals[2].IsCompleted);
            Assert.AreEqual(timestamp, goals[2].LastUpdate);
            Assert.AreEqual("Hand in Combat bonds", goals[2].GoalObjectiveText);
            Assert.IsTrue(string.IsNullOrWhiteSpace(goals[2].GoalRewardText));
            Assert.IsFalse(string.IsNullOrWhiteSpace(goals[2].GoalDescriptionText));
            Assert.AreEqual("https://inara.cz/galaxy-communitygoals/", goals[2].InaraURL);
            Assert.AreEqual(TimeSpan.Zero, goals[2].TimeRemaining);
            Assert.AreEqual(0.66666666666666663, goals[2].Progress);
            Assert.AreEqual("Progress: 4 / 6 Tiers", goals[2].ProgressText);
            Assert.AreEqual("Combat", goals[2].Topic);

            mockInara.Verify(x => x.GetData(It.IsAny<InaraHeader>(), It.IsAny<IList<InaraEvent>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task CG_ByTime_Get_Test()
        {
            DateTime timestamp = DateTime.Now;
            string inaraData = JsonConvert.SerializeObject(GetCGRequestData(timestamp));

            Mock<IInaraService> mockInara = new();
            mockInara.Setup(x => x.GetData(It.IsAny<InaraHeader>(), It.IsAny<IList<InaraEvent>>(), It.IsAny<DownloadOptions>()).Result).Returns((inaraData, timestamp));

            CommunityGoalsService cgService = CommunityGoalsService.Instance(mockInara.Object);
            (List<CommunityGoal> goals, DateTime updated) = await cgService.GetDataByTime(
                10,
                60,
                new InaraIdentity("name", "version", "key", true),
                new CancellationTokenSource(),
                ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(goals);
            Assert.AreEqual(2, goals.Count);
            Assert.AreEqual(702, goals[1].CommunityGoalGameID);
            Assert.AreEqual("Colonia Bridge Project Third Phase - Colonia", goals[1].CommunityGoalName);
            Assert.AreEqual("Colonia", goals[1].StarsystemName);
            Assert.AreEqual("Jaques Station", goals[1].StationName);
            Assert.AreEqual(timestamp - TimeSpan.FromDays(8), goals[1].GoalExpiry);
            Assert.AreEqual(1, goals[1].TierReached);
            Assert.AreEqual(2, goals[1].TierMax);
            Assert.AreEqual(1406, goals[1].ContributorsNum);
            Assert.AreEqual(5784671, goals[1].ContributionsTotal);
            Assert.IsTrue(goals[1].IsCompleted);
            Assert.AreEqual(timestamp, goals[1].LastUpdate);
            Assert.AreEqual("Deliver Computer Components, Ceramic Composites and Thermal Cooling Units", goals[1].GoalObjectiveText);
            Assert.IsTrue(string.IsNullOrWhiteSpace(goals[1].GoalRewardText));
            Assert.IsFalse(string.IsNullOrWhiteSpace(goals[1].GoalDescriptionText));
            Assert.AreEqual("https://inara.cz/galaxy-communitygoals/", goals[1].InaraURL);
            Assert.AreEqual(TimeSpan.Zero, goals[1].TimeRemaining);
            Assert.AreEqual(0.5, goals[1].Progress);
            Assert.AreEqual("Progress: 1 / 2 Tiers", goals[1].ProgressText);
            Assert.AreEqual("Trade", goals[1].Topic);

            mockInara.Verify(x => x.GetData(It.IsAny<InaraHeader>(), It.IsAny<IList<InaraEvent>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task CG_AltBow_Get_Test()
        {
            DateTime timestamp = DateTime.Now;
            string inaraData = JsonConvert.SerializeObject(GetCGRequestData(timestamp));
            string BoW = LoadBoW("UnitTests.Resources.CGBoW.json");

            Mock<IInaraService> mockInara = new();
            mockInara.Setup(x => x.GetData(It.IsAny<InaraHeader>(), It.IsAny<IList<InaraEvent>>(), It.IsAny<DownloadOptions>()).Result).Returns((inaraData, timestamp));

            CommunityGoalsService cgService = CommunityGoalsService.Instance(mockInara.Object);
            (List<CommunityGoal> goals, DateTime updated) = await cgService.GetData(
                60,
                new InaraIdentity("name", "version", "key", true),
                new CancellationTokenSource(),
                BoW,
                true).ConfigureAwait(false);

            Assert.IsNotNull(goals);
            Assert.AreEqual(3, goals.Count);
            Assert.AreEqual(695, goals[2].CommunityGoalGameID);
            Assert.AreEqual("Fight for the Neo-Marlinist Order of Mudhrid", goals[2].CommunityGoalName);
            Assert.AreEqual("Mudhrid", goals[2].StarsystemName);
            Assert.AreEqual("Fairfax Vision", goals[2].StationName);
            Assert.AreEqual(timestamp - TimeSpan.FromDays(15), goals[2].GoalExpiry);
            Assert.AreEqual(4, goals[2].TierReached);
            Assert.AreEqual(6, goals[2].TierMax);
            Assert.AreEqual(1342, goals[2].ContributorsNum);
            Assert.AreEqual(75546464873, goals[2].ContributionsTotal);
            Assert.IsTrue(goals[2].IsCompleted);
            Assert.AreEqual(timestamp, goals[2].LastUpdate);
            Assert.AreEqual("Hand in Combat bonds", goals[2].GoalObjectiveText);
            Assert.IsTrue(string.IsNullOrWhiteSpace(goals[2].GoalRewardText));
            Assert.IsFalse(string.IsNullOrWhiteSpace(goals[2].GoalDescriptionText));
            Assert.AreEqual("https://inara.cz/galaxy-communitygoals/", goals[2].InaraURL);
            Assert.AreEqual(TimeSpan.Zero, goals[2].TimeRemaining);
            Assert.AreEqual(0.66666666666666663, goals[2].Progress);
            Assert.AreEqual("Progress: 4 / 6 Tiers", goals[2].ProgressText);
            Assert.AreEqual("Combat", goals[2].Topic);

            mockInara.Verify(x => x.GetData(It.IsAny<InaraHeader>(), It.IsAny<IList<InaraEvent>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        private static InaraRequest GetCGRequestData(DateTime updated)
        {
            List<CommunityGoal> goals = new()
            {
                new CommunityGoal()
                {
                    CommunityGoalName = "Caine Massey Mining Initiative",
                    CommunityGoalGameID = 704,
                    StarsystemName = "Dulos",
                    StationName = "Smith Port",
                    GoalExpiry = updated - TimeSpan.FromDays(1),
                    TierReached = 2,
                    TierMax = 5,
                    ContributorsNum = 1458,
                    ContributionsTotal = 485271,
                    IsCompleted = true,
                    LastUpdate = updated,
                    GoalObjectiveText = "Deliver Gallite, Bromellite and Samarium",
                    GoalRewardText = string.Empty,
                    GoalDescriptionText = "Caine Massey and Torval Mining Ltd are running rival campaigns to deliver mined commodities to the Dulos system.\n\nFor the past decade, the megacorp Caine Massey has supplied ore and other raw materials to several subsidiaries in the region.",
                    InaraURL = "https://inara.cz/galaxy-communitygoals/"
                },
                new CommunityGoal()
                {
                    CommunityGoalName = "Colonia Bridge Project Third Phase - Colonia",
                    CommunityGoalGameID = 702,
                    StarsystemName = "Colonia",
                    StationName = "Jaques Station",
                    GoalExpiry = updated - TimeSpan.FromDays(8),
                    TierReached = 1,
                    TierMax = 2,
                    ContributorsNum = 1406,
                    ContributionsTotal = 5784671,
                    IsCompleted = true,
                    LastUpdate = updated,
                    GoalObjectiveText = "Deliver Computer Components, Ceramic Composites and Thermal Cooling Units",
                    GoalRewardText = string.Empty,
                    GoalDescriptionText = "Phase three aims to enhance the Colonia Bridge by building permanent starports in key locations along the route.\n\n\"Pilots are asked to deliver ceramic composites, computer components and thermal cooling units to Jaques Station in the Colonia system.\"",
                    InaraURL = "https://inara.cz/galaxy-communitygoals/"
                },
                new CommunityGoal()
                {
                    CommunityGoalName = "Fight for the Neo-Marlinist Order of Mudhrid",
                    CommunityGoalGameID = 695,
                    StarsystemName = "Mudhrid",
                    StationName = "Fairfax Vision",
                    GoalExpiry = updated - TimeSpan.FromDays(15),
                    TierReached = 4,
                    TierMax = 6,
                    ContributorsNum = 1342,
                    ContributionsTotal = 75546464873,
                    IsCompleted = true,
                    LastUpdate = updated,
                    GoalObjectiveText = "Hand in Combat bonds",
                    GoalRewardText = string.Empty,
                    GoalDescriptionText = "The Neo-Marlinist Order of Mudhrid and the Epsilon Fornacis Empire Group have declared war in the Mudhrid system.\n\nThe Steel Majesty megaship is serving as the base of the Neo-Marlinist forces.",
                    InaraURL = "https://inara.cz/galaxy-communitygoals/"
                }
            };
            return new()
            {
                Header = new InaraHeader { EventStatus = 200 },
                Events = new List<InaraEvent> { new InaraEvent("inaraEvent", goals) { EventStatus = 200 } }
            };
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

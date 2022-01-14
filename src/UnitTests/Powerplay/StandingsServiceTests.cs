using EDlib;
using EDlib.Network;
using EDlib.Platform;
using EDlib.Powerplay;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class StandingsServiceTests
    {
        [TestMethod]
        public async Task StandingsService_Cache_Test()
        {
            DateTime timestamp = DateTime.Now;

            GalacticStandings data = GetTestData(timestamp);
            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((data.ToJson(), DateTime.Now));

            Mock<ICacheService> mockCache = new();
            mockCache.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            mockCache.Setup(x => x.IsExpired(It.IsAny<string>())).Returns(false);
            mockCache.Setup(x => x.Get("Standings")).Returns(data.ToJson());
            mockCache.Setup(x => x.Get("StandingsUpdated")).Returns(timestamp.ToString());
            mockCache.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()));

            StandingsService service = StandingsService.Instance(mockDownloadService.Object, mockCache.Object);
            GalacticStandings standings = await service.GetData(new CancellationTokenSource()).ConfigureAwait(false);

            Assert.AreEqual(CycleService.CurrentCycle(), standings.Cycle);
            Assert.AreEqual(timestamp, standings.LastUpdated);
            Assert.AreEqual(11, standings.Standings.Count);

            PowerStanding ald = standings.Standings.Find(x => x.ShortName.Equals("ALD"));
            Assert.AreEqual(3, ald.Id);
            Assert.AreEqual("Arissa Lavigny-Duval", ald.Name);
            Assert.AreEqual(1, ald.Position);
            Assert.AreEqual(StandingChange.up, ald.Change);
            Assert.IsFalse(ald.Turmoil);
            Assert.AreEqual("Empire", ald.Allegiance);
            Assert.AreEqual(timestamp, ald.LastUpdated);
            Assert.AreEqual(113, ald.CyclesSinceTurmoil);

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Never());
            mockCache.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.IsExpired(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.Get(It.IsAny<string>()), Times.Exactly(2));
            mockCache.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task StandingsService_Get_Test()
        {
            DateTime timestamp = DateTime.Now;

            GalacticStandings data = GetTestData(timestamp);
            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((data.ToJson(), DateTime.Now));

            Mock<ICacheService> mockCache = new();
            mockCache.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            mockCache.Setup(x => x.IsExpired(It.IsAny<string>())).Returns(true);
            mockCache.Setup(x => x.Get(It.IsAny<string>())).Returns(timestamp.ToString());
            mockCache.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()));

            StandingsService service = StandingsService.Instance(mockDownloadService.Object, mockCache.Object);
            GalacticStandings standings = await service.GetData(new CancellationTokenSource()).ConfigureAwait(false);

            Assert.AreEqual(CycleService.CurrentCycle(), standings.Cycle);
            Assert.AreEqual(timestamp, standings.LastUpdated);
            Assert.AreEqual(11, standings.Standings.Count);

            PowerStanding ald = standings.Standings.Find(x => x.ShortName.Equals("ALD"));
            Assert.AreEqual(3, ald.Id);
            Assert.AreEqual("Arissa Lavigny-Duval", ald.Name);
            Assert.AreEqual(1, ald.Position);
            Assert.AreEqual(StandingChange.up, ald.Change);
            Assert.IsFalse(ald.Turmoil);
            Assert.AreEqual("Empire", ald.Allegiance);
            Assert.AreEqual(timestamp, ald.LastUpdated);
            Assert.AreEqual(113, ald.CyclesSinceTurmoil);

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
            mockCache.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.IsExpired(It.IsAny<string>()), Times.Never);
            mockCache.Verify(x => x.Get(It.IsAny<string>()), Times.Never);
            mockCache.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task StandingsService_Get_NoNetwork_Test()
        {
            DateTime timestamp = DateTime.Now;

            GalacticStandings data = GetTestData(timestamp);
            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>())).Throws<NoNetworkNoCacheException>();

            Mock<ICacheService> mockCache = new();
            mockCache.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            mockCache.Setup(x => x.IsExpired(It.IsAny<string>())).Returns(true);
            mockCache.Setup(x => x.Get("Standings")).Returns(data.ToJson());
            mockCache.Setup(x => x.Get("StandingsUpdated")).Returns(timestamp.ToString());
            mockCache.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()));

            StandingsService service = StandingsService.Instance(mockDownloadService.Object, mockCache.Object);
            GalacticStandings standings = await service.GetData(new CancellationTokenSource()).ConfigureAwait(false);

            Assert.AreEqual(CycleService.CurrentCycle(), standings.Cycle);
            Assert.AreEqual(timestamp, standings.LastUpdated);
            Assert.AreEqual(11, standings.Standings.Count);

            PowerStanding ald = standings.Standings.Find(x => x.ShortName.Equals("ALD"));
            Assert.AreEqual(3, ald.Id);
            Assert.AreEqual("Arissa Lavigny-Duval", ald.Name);
            Assert.AreEqual(1, ald.Position);
            Assert.AreEqual(StandingChange.up, ald.Change);
            Assert.IsFalse(ald.Turmoil);
            Assert.AreEqual("Empire", ald.Allegiance);
            Assert.AreEqual(timestamp, ald.LastUpdated);
            Assert.AreEqual(113, ald.CyclesSinceTurmoil);

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
            mockCache.Verify(x => x.Exists(It.IsAny<string>()), Times.Exactly(2));
            mockCache.Verify(x => x.IsExpired(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.Get(It.IsAny<string>()), Times.Exactly(2));
            mockCache.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task StandingsService_Get_NoUpdate_Test()
        {
            DateTime timestamp = DateTime.Now;

            GalacticStandings data = GetTestData(timestamp, 250);
            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((data.ToJson(), DateTime.Now));

            Mock<ICacheService> mockCache = new();
            mockCache.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            mockCache.Setup(x => x.IsExpired(It.IsAny<string>())).Returns(true);
            mockCache.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()));

            StandingsService service = StandingsService.Instance(mockDownloadService.Object, mockCache.Object);
            GalacticStandings standings = await service.GetData(new CancellationTokenSource()).ConfigureAwait(false);

            Assert.AreEqual(250, standings.Cycle);
            Assert.AreEqual(timestamp, standings.LastUpdated);
            Assert.AreEqual(11, standings.Standings.Count);

            PowerStanding ald = standings.Standings.Find(x => x.ShortName.Equals("ALD"));
            Assert.AreEqual(3, ald.Id);
            Assert.AreEqual("Arissa Lavigny-Duval", ald.Name);
            Assert.AreEqual(1, ald.Position);
            Assert.AreEqual(StandingChange.up, ald.Change);
            Assert.IsFalse(ald.Turmoil);
            Assert.AreEqual("Empire", ald.Allegiance);
            Assert.AreEqual(timestamp, ald.LastUpdated);
            Assert.AreEqual(113, ald.CyclesSinceTurmoil);

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
            mockCache.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.IsExpired(It.IsAny<string>()), Times.Never);
            mockCache.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Never);
        }

        private static GalacticStandings GetTestData(DateTime updated, int cycle = 0)
        {
            var standings = new GalacticStandings(cycle == 0 ? CycleService.CurrentCycle() : cycle, updated);
            standings.Standings.Add(new PowerStanding(3, "Arissa Lavigny-Duval", 1, StandingChange.up, false, "Empire", "ALD", updated, 113));
            standings.Standings.Add(new PowerStanding(5, "Edmund Mahon", 2, StandingChange.down, false, "Alliance", "Mahon", updated, 12));
            standings.Standings.Add(new PowerStanding(10, "Zachary Hudson", 3, StandingChange.none, false, "Federation", "Hudson", updated, 148));
            standings.Standings.Add(new PowerStanding(4, "Denton Patreus", 4, StandingChange.up, false, "Empire", "Patreus", updated, 48));
            standings.Standings.Add(new PowerStanding(1, "Aisling Duval", 5, StandingChange.down, false, "Empire", "Aisling", updated, 163));
            standings.Standings.Add(new PowerStanding(7, "Li Yong-Rui", 6, StandingChange.none, false, "Independent", "LYR", updated, 6));
            standings.Standings.Add(new PowerStanding(6, "Felicia Winters", 7, StandingChange.none, true, "Federation", "Winters", updated, 0));
            standings.Standings.Add(new PowerStanding(8, "Pranav Antal", 8, StandingChange.none, false, "Independent", "Antal", updated, 50));
            standings.Standings.Add(new PowerStanding(9, "Yuri Grom", 9, StandingChange.none, false, "Independent", "Grom", updated, 0));
            standings.Standings.Add(new PowerStanding(2, "Archon Delaine", 10, StandingChange.none, false, "Independent", "Delaine", updated, 38));
            standings.Standings.Add(new PowerStanding(11, "Zemina Torval", 11, StandingChange.none, false, "Empire", "Torval", updated, 66));
            return standings;
        }
    }
}

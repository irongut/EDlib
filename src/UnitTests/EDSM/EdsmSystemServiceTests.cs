using EDlib.EDSM;
using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class EdsmSystemServiceTests
    {
        [TestMethod]
        public async Task Stations_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetSolStationData(timestamp));

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            SystemStations stations = await systemService.GetStations("Sol", ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(stations);
            Assert.AreEqual(27, stations.Id);
            Assert.AreEqual(10477373803, stations.Id64);
            Assert.AreEqual("Sol", stations.Name);
            Assert.IsFalse(string.IsNullOrWhiteSpace(stations.Url));
            Assert.AreEqual(3, stations.Stations.Count);
            Assert.IsTrue(stations.LastUpdated > DateTime.MinValue);

            Station gStation = stations.Stations.Find(x => x.Name == "Galileo");
            Assert.IsNotNull(gStation);
            Assert.AreEqual(560, gStation.Id);
            Assert.AreEqual(128016640, gStation.MarketId);
            Assert.AreEqual("Ocellus Starport", gStation.Type);
            Assert.AreEqual("Galileo", gStation.Name);
            Assert.AreEqual(505, gStation.DistanceToArrival);
            Assert.AreEqual("Federation", gStation.Allegiance);
            Assert.AreEqual("Democracy", gStation.Government);
            Assert.AreEqual("Refinery", gStation.Economy);
            Assert.IsTrue(string.IsNullOrWhiteSpace(gStation.SecondEconomy));
            Assert.IsTrue(gStation.HaveMarket);
            Assert.IsTrue(gStation.HaveOutfitting);
            Assert.IsTrue(gStation.HaveShipyard);
            Assert.IsTrue(gStation.OtherServices.Length > 0);
            Assert.IsNotNull(gStation.ControllingFaction);
            Assert.AreEqual(1, gStation.ControllingFaction.Id);
            Assert.AreEqual("Mother Gaia", gStation.ControllingFaction.Name);
            Assert.IsNotNull(gStation.UpdateTime);
            Assert.AreEqual(timestamp, gStation.UpdateTime.Information);
            Assert.AreEqual(timestamp, gStation.UpdateTime.Market);
            Assert.AreEqual(timestamp, gStation.UpdateTime.Shipyard);
            Assert.AreEqual(timestamp, gStation.UpdateTime.Outfitting);
            Assert.IsNotNull(gStation.Body);
            Assert.AreEqual(14780, gStation.Body.Id);
            Assert.AreEqual("Moon", gStation.Body.Name);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Stations_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetSolStationData(timestamp));

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            SystemStations stations = await systemService.GetStations("Sol", new CancellationTokenSource(), ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(stations);
            Assert.AreEqual(27, stations.Id);
            Assert.AreEqual(10477373803, stations.Id64);
            Assert.AreEqual("Sol", stations.Name);
            Assert.IsFalse(string.IsNullOrWhiteSpace(stations.Url));
            Assert.AreEqual(3, stations.Stations.Count);
            Assert.IsTrue(stations.LastUpdated > DateTime.MinValue);

            Station mStation = stations.Stations.Find(x => x.Name == "Mars High");
            Assert.IsNotNull(mStation);
            Assert.AreEqual(1822, mStation.Id);
            Assert.AreEqual(128017664, mStation.MarketId);
            Assert.AreEqual("Orbis Starport", mStation.Type);
            Assert.AreEqual("Mars High", mStation.Name);
            Assert.AreEqual(714, mStation.DistanceToArrival);
            Assert.AreEqual("Federation", mStation.Allegiance);
            Assert.AreEqual("Democracy", mStation.Government);
            Assert.AreEqual("Service", mStation.Economy);
            Assert.IsTrue(string.IsNullOrWhiteSpace(mStation.SecondEconomy));
            Assert.IsTrue(mStation.HaveMarket);
            Assert.IsTrue(mStation.HaveOutfitting);
            Assert.IsTrue(mStation.HaveShipyard);
            Assert.IsTrue(mStation.OtherServices.Length > 0);
            Assert.IsNotNull(mStation.ControllingFaction);
            Assert.AreEqual(223, mStation.ControllingFaction.Id);
            Assert.AreEqual("Sol Workers' Party", mStation.ControllingFaction.Name);
            Assert.IsNotNull(mStation.UpdateTime);
            Assert.AreEqual(timestamp, mStation.UpdateTime.Information);
            Assert.AreEqual(timestamp, mStation.UpdateTime.Market);
            Assert.AreEqual(timestamp, mStation.UpdateTime.Shipyard);
            Assert.AreEqual(timestamp, mStation.UpdateTime.Outfitting);
            Assert.IsNotNull(mStation.Body);
            Assert.AreEqual(1828, mStation.Body.Id);
            Assert.AreEqual("Mars", mStation.Body.Name);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Market_ById_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetMarketData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            Market gMarket = await systemService.GetMarket(128016640, ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(gMarket);
            Assert.AreEqual(27, gMarket.Id);
            Assert.AreEqual(10477373803, gMarket.Id64);
            Assert.AreEqual("Sol", gMarket.Name);
            Assert.AreEqual(128016640, gMarket.MarketId);
            Assert.AreEqual(560, gMarket.SId);
            Assert.AreEqual("Galileo", gMarket.SName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gMarket.Url));
            Assert.AreEqual(3, gMarket.Commodities.Count);
            Assert.IsTrue(gMarket.LastUpdated > DateTime.MinValue);

            Commodity gold = gMarket.Commodities.Find(x => x.Id == "gold");
            Assert.IsNotNull(gold);
            Assert.AreEqual("gold", gold.Id);
            Assert.AreEqual("Gold", gold.Name);
            Assert.AreEqual(49800, gold.BuyPrice);
            Assert.AreEqual(4615, gold.Stock);
            Assert.AreEqual(48746, gold.SellPrice);
            Assert.AreEqual(1, gold.Demand);
            Assert.AreEqual(2, gold.StockBracket);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Market_ById_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetMarketData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            Market gMarket = await systemService.GetMarket(128016640, new CancellationTokenSource(), ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(gMarket);
            Assert.AreEqual(27, gMarket.Id);
            Assert.AreEqual(10477373803, gMarket.Id64);
            Assert.AreEqual("Sol", gMarket.Name);
            Assert.AreEqual(128016640, gMarket.MarketId);
            Assert.AreEqual(560, gMarket.SId);
            Assert.AreEqual("Galileo", gMarket.SName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gMarket.Url));
            Assert.AreEqual(3, gMarket.Commodities.Count);
            Assert.IsTrue(gMarket.LastUpdated > DateTime.MinValue);

            Commodity aluminium = gMarket.Commodities.Find(x => x.Id == "aluminium");
            Assert.IsNotNull(aluminium);
            Assert.AreEqual("aluminium", aluminium.Id);
            Assert.AreEqual("Aluminium", aluminium.Name);
            Assert.AreEqual(267, aluminium.BuyPrice);
            Assert.AreEqual(2676732, aluminium.Stock);
            Assert.AreEqual(245, aluminium.SellPrice);
            Assert.AreEqual(1, aluminium.Demand);
            Assert.AreEqual(3, aluminium.StockBracket);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Market_ByName_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetMarketData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            Market gMarket = await systemService.GetMarket("Sol", "Galileo", ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(gMarket);
            Assert.AreEqual(27, gMarket.Id);
            Assert.AreEqual(10477373803, gMarket.Id64);
            Assert.AreEqual("Sol", gMarket.Name);
            Assert.AreEqual(128016640, gMarket.MarketId);
            Assert.AreEqual(560, gMarket.SId);
            Assert.AreEqual("Galileo", gMarket.SName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gMarket.Url));
            Assert.AreEqual(3, gMarket.Commodities.Count);
            Assert.IsTrue(gMarket.LastUpdated > DateTime.MinValue);

            Commodity catalysers = gMarket.Commodities.Find(x => x.Id == "advancedcatalysers");
            Assert.IsNotNull(catalysers);
            Assert.AreEqual("advancedcatalysers", catalysers.Id);
            Assert.AreEqual("Advanced Catalysers", catalysers.Name);
            Assert.AreEqual(0, catalysers.BuyPrice);
            Assert.AreEqual(0, catalysers.Stock);
            Assert.AreEqual(3435, catalysers.SellPrice);
            Assert.AreEqual(3626984, catalysers.Demand);
            Assert.AreEqual(0, catalysers.StockBracket);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Market_ByName_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetMarketData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            Market gMarket = await systemService.GetMarket("Sol", "Galileo", new CancellationTokenSource(), ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(gMarket);
            Assert.AreEqual(27, gMarket.Id);
            Assert.AreEqual(10477373803, gMarket.Id64);
            Assert.AreEqual("Sol", gMarket.Name);
            Assert.AreEqual(128016640, gMarket.MarketId);
            Assert.AreEqual(560, gMarket.SId);
            Assert.AreEqual("Galileo", gMarket.SName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gMarket.Url));
            Assert.AreEqual(3, gMarket.Commodities.Count);
            Assert.IsTrue(gMarket.LastUpdated > DateTime.MinValue);

            Commodity gold = gMarket.Commodities.Find(x => x.Id == "gold");
            Assert.IsNotNull(gold);
            Assert.AreEqual("gold", gold.Id);
            Assert.AreEqual("Gold", gold.Name);
            Assert.AreEqual(49800, gold.BuyPrice);
            Assert.AreEqual(4615, gold.Stock);
            Assert.AreEqual(48746, gold.SellPrice);
            Assert.AreEqual(1, gold.Demand);
            Assert.AreEqual(2, gold.StockBracket);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Shipyard_ById_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetShipyardData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            Shipyard gShipyard = await systemService.GetShipyard(128016640, ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(gShipyard);
            Assert.AreEqual(27, gShipyard.Id);
            Assert.AreEqual(10477373803, gShipyard.Id64);
            Assert.AreEqual("Sol", gShipyard.Name);
            Assert.AreEqual(128016640, gShipyard.MarketId);
            Assert.AreEqual(560, gShipyard.SId);
            Assert.AreEqual("Galileo", gShipyard.SName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gShipyard.Url));
            Assert.AreEqual(3, gShipyard.Ships.Count);
            Assert.IsTrue(gShipyard.LastUpdated > DateTime.MinValue);

            Ship fas = gShipyard.Ships.Find(x => x.Name == "Federal Assault Ship");
            Assert.IsNotNull(fas);
            Assert.AreEqual(128672145, fas.Id);
            Assert.AreEqual("Federal Assault Ship", fas.Name);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Shipyard_ById_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetShipyardData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            Shipyard gShipyard = await systemService.GetShipyard(128016640, new CancellationTokenSource(), ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(gShipyard);
            Assert.AreEqual(27, gShipyard.Id);
            Assert.AreEqual(10477373803, gShipyard.Id64);
            Assert.AreEqual("Sol", gShipyard.Name);
            Assert.AreEqual(128016640, gShipyard.MarketId);
            Assert.AreEqual(560, gShipyard.SId);
            Assert.AreEqual("Galileo", gShipyard.SName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gShipyard.Url));
            Assert.AreEqual(3, gShipyard.Ships.Count);
            Assert.IsTrue(gShipyard.LastUpdated > DateTime.MinValue);

            Ship eagle = gShipyard.Ships.Find(x => x.Name == "Eagle");
            Assert.IsNotNull(eagle);
            Assert.AreEqual(128049255, eagle.Id);
            Assert.AreEqual("Eagle", eagle.Name);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Shipyard_ByName_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetShipyardData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            Shipyard gShipyard = await systemService.GetShipyard("Sol", "Galileo", ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(gShipyard);
            Assert.AreEqual(27, gShipyard.Id);
            Assert.AreEqual(10477373803, gShipyard.Id64);
            Assert.AreEqual("Sol", gShipyard.Name);
            Assert.AreEqual(128016640, gShipyard.MarketId);
            Assert.AreEqual(560, gShipyard.SId);
            Assert.AreEqual("Galileo", gShipyard.SName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gShipyard.Url));
            Assert.AreEqual(3, gShipyard.Ships.Count);
            Assert.IsTrue(gShipyard.LastUpdated > DateTime.MinValue);

            Ship vulture = gShipyard.Ships.Find(x => x.Name == "Vulture");
            Assert.IsNotNull(vulture);
            Assert.AreEqual(128049309, vulture.Id);
            Assert.AreEqual("Vulture", vulture.Name);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Shipyard_ByName_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetShipyardData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            Shipyard gShipyard = await systemService.GetShipyard("Sol", "Galileo", new CancellationTokenSource(), ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(gShipyard);
            Assert.AreEqual(27, gShipyard.Id);
            Assert.AreEqual(10477373803, gShipyard.Id64);
            Assert.AreEqual("Sol", gShipyard.Name);
            Assert.AreEqual(128016640, gShipyard.MarketId);
            Assert.AreEqual(560, gShipyard.SId);
            Assert.AreEqual("Galileo", gShipyard.SName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gShipyard.Url));
            Assert.AreEqual(3, gShipyard.Ships.Count);
            Assert.IsTrue(gShipyard.LastUpdated > DateTime.MinValue);

            Ship fas = gShipyard.Ships.Find(x => x.Name == "Federal Assault Ship");
            Assert.IsNotNull(fas);
            Assert.AreEqual(128672145, fas.Id);
            Assert.AreEqual("Federal Assault Ship", fas.Name);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Outfitting_ById_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetOutfittingData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            StationOutfitting gOutfit = await systemService.GetOutfitting(128016640, ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(gOutfit);
            Assert.AreEqual(27, gOutfit.Id);
            Assert.AreEqual(10477373803, gOutfit.Id64);
            Assert.AreEqual("Sol", gOutfit.Name);
            Assert.AreEqual(128016640, gOutfit.MarketId);
            Assert.AreEqual(560, gOutfit.SId);
            Assert.AreEqual("Galileo", gOutfit.SName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gOutfit.Url));
            Assert.AreEqual(3, gOutfit.Outfitting.Count);
            Assert.IsTrue(gOutfit.LastUpdated > DateTime.MinValue);

            ShipModule buggybay = gOutfit.Outfitting.Find(x => x.Name == "2G Planetary Vehicle Hangar");
            Assert.IsNotNull(buggybay);
            Assert.AreEqual("int_buggybay_size2_class2", buggybay.Id);
            Assert.AreEqual("2G Planetary Vehicle Hangar", buggybay.Name);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Outfitting_ById_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetOutfittingData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            StationOutfitting gOutfit = await systemService.GetOutfitting(128016640, new CancellationTokenSource(), ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(gOutfit);
            Assert.AreEqual(27, gOutfit.Id);
            Assert.AreEqual(10477373803, gOutfit.Id64);
            Assert.AreEqual("Sol", gOutfit.Name);
            Assert.AreEqual(128016640, gOutfit.MarketId);
            Assert.AreEqual(560, gOutfit.SId);
            Assert.AreEqual("Galileo", gOutfit.SName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gOutfit.Url));
            Assert.AreEqual(3, gOutfit.Outfitting.Count);
            Assert.IsTrue(gOutfit.LastUpdated > DateTime.MinValue);

            ShipModule armour = gOutfit.Outfitting.Find(x => x.Name == "1I Reinforced Alloy");
            Assert.IsNotNull(armour);
            Assert.AreEqual("eagle_armour_grade2", armour.Id);
            Assert.AreEqual("1I Reinforced Alloy", armour.Name);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Outfitting_ByName_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetOutfittingData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            StationOutfitting gOutfit = await systemService.GetOutfitting("Sol", "Galileo", ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(gOutfit);
            Assert.AreEqual(27, gOutfit.Id);
            Assert.AreEqual(10477373803, gOutfit.Id64);
            Assert.AreEqual("Sol", gOutfit.Name);
            Assert.AreEqual(128016640, gOutfit.MarketId);
            Assert.AreEqual(560, gOutfit.SId);
            Assert.AreEqual("Galileo", gOutfit.SName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gOutfit.Url));
            Assert.AreEqual(3, gOutfit.Outfitting.Count);
            Assert.IsTrue(gOutfit.LastUpdated > DateTime.MinValue);

            ShipModule cannon = gOutfit.Outfitting.Find(x => x.Name == "4A Multi-Cannon (Gimbal)");
            Assert.IsNotNull(cannon);
            Assert.AreEqual("hpt_multicannon_gimbal_huge", cannon.Id);
            Assert.AreEqual("4A Multi-Cannon (Gimbal)", cannon.Name);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Outfitting_ByName_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetOutfittingData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            StationOutfitting gOutfit = await systemService.GetOutfitting("Sol", "Galileo", new CancellationTokenSource(), ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(gOutfit);
            Assert.AreEqual(27, gOutfit.Id);
            Assert.AreEqual(10477373803, gOutfit.Id64);
            Assert.AreEqual("Sol", gOutfit.Name);
            Assert.AreEqual(128016640, gOutfit.MarketId);
            Assert.AreEqual(560, gOutfit.SId);
            Assert.AreEqual("Galileo", gOutfit.SName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gOutfit.Url));
            Assert.AreEqual(3, gOutfit.Outfitting.Count);
            Assert.IsTrue(gOutfit.LastUpdated > DateTime.MinValue);

            ShipModule buggybay = gOutfit.Outfitting.Find(x => x.Name == "2G Planetary Vehicle Hangar");
            Assert.IsNotNull(buggybay);
            Assert.AreEqual("int_buggybay_size2_class2", buggybay.Id);
            Assert.AreEqual("2G Planetary Vehicle Hangar", buggybay.Name);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Factions_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetFactionsData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            SystemFactions factions = await systemService.GetFactions("Sol", ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(factions);
            Assert.AreEqual(27, factions.Id);
            Assert.AreEqual(10477373803, factions.Id64);
            Assert.AreEqual("Sol", factions.Name);
            Assert.IsFalse(string.IsNullOrWhiteSpace(factions.Url));
            Assert.IsNotNull(factions.ControllingFaction);
            Assert.AreEqual(1, factions.ControllingFaction.Id);
            Assert.AreEqual("Mother Gaia", factions.ControllingFaction.Name);
            Assert.AreEqual("Federation", factions.ControllingFaction.Allegiance);
            Assert.AreEqual("Democracy", factions.ControllingFaction.Government);

            Faction congress = factions.Factions.Find(x => x.Name == "Federal Congress");
            Assert.IsNotNull(congress);
            Assert.AreEqual(963, congress.Id);
            Assert.AreEqual("Federal Congress", congress.Name);
            Assert.AreEqual("Federation", congress.Allegiance);
            Assert.AreEqual("Democracy", congress.Government);
            Assert.AreEqual(0.174524, congress.Influence);
            Assert.AreEqual("None", congress.State);
            Assert.IsNull(congress.ActiveStates);
            Assert.IsNull(congress.RecoveringStates);
            Assert.IsNull(congress.PendingStates);
            Assert.AreEqual("Happy", congress.Happiness);
            Assert.IsFalse(congress.IsPlayer);
            Assert.AreEqual(1642890937, congress.LastUpdate);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Factions_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetFactionsData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemService systemService = SystemService.Instance(mockEdsm.Object);
            SystemFactions factions = await systemService.GetFactions("Sol", new CancellationTokenSource(), ignoreCache: true).ConfigureAwait(false);

            Assert.IsNotNull(factions);
            Assert.AreEqual(27, factions.Id);
            Assert.AreEqual(10477373803, factions.Id64);
            Assert.AreEqual("Sol", factions.Name);
            Assert.IsFalse(string.IsNullOrWhiteSpace(factions.Url));
            Assert.IsNotNull(factions.ControllingFaction);
            Assert.AreEqual(1, factions.ControllingFaction.Id);
            Assert.AreEqual("Mother Gaia", factions.ControllingFaction.Name);
            Assert.AreEqual("Federation", factions.ControllingFaction.Allegiance);
            Assert.AreEqual("Democracy", factions.ControllingFaction.Government);

            Faction gaia = factions.Factions.Find(x => x.Name == "Mother Gaia");
            Assert.IsNotNull(gaia);
            Assert.AreEqual(1, gaia.Id);
            Assert.AreEqual("Mother Gaia", gaia.Name);
            Assert.AreEqual("Federation", gaia.Allegiance);
            Assert.AreEqual("Democracy", gaia.Government);
            Assert.AreEqual(0.298897, gaia.Influence);
            Assert.AreEqual("Boom", gaia.State);
            Assert.AreEqual("Boom", gaia.ActiveStates[0].State);
            Assert.AreEqual("Civil liberty", gaia.RecoveringStates[0].State);
            Assert.AreEqual(0, gaia.RecoveringStates[0].Trend);
            Assert.IsNull(gaia.PendingStates);
            Assert.AreEqual("Happy", gaia.Happiness);
            Assert.IsFalse(gaia.IsPlayer);
            Assert.AreEqual(1642890937, gaia.LastUpdate);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        private static SystemStations GetSolStationData(DateTime updated)
        {
            UpdateTime updateTime = new()
            {
                Information = updated,
                Market = updated,
                Shipyard = updated,
                Outfitting = updated
            };
            List<Station> stations = new()
            {
                new Station()
                {
                    Id = 375,
                    MarketId = 128016384,
                    Type = "Orbis Starport",
                    Name = "Daedalus",
                    DistanceToArrival = 9168,
                    Allegiance = "Federation",
                    Government = "Democracy",
                    Economy = "Refinery",
                    SecondEconomy = null,
                    HaveMarket = true,
                    HaveShipyard = true,
                    HaveOutfitting = true,
                    OtherServices = (new string[] { "Black Market", "Restock", "Refuel", "Repair", "Contacts", "Universal Cartographics", "Missions", "Crew Lounge", "Tuning", "Search and Rescue" }),
                    ControllingFaction = new ControllingFaction() { Id = 1, Name = "Mother Gaia"},
                    UpdateTime = updateTime,
                    Body = new Body() { Id = 5094, Name = "Ariel" }
                },
                new Station()
                {
                    Id = 560,
                    MarketId = 128016640,
                    Type = "Ocellus Starport",
                    Name = "Galileo",
                    DistanceToArrival = 505,
                    Allegiance = "Federation",
                    Government = "Democracy",
                    Economy = "Refinery",
                    SecondEconomy = null,
                    HaveMarket = true,
                    HaveShipyard = true,
                    HaveOutfitting = true,
                    OtherServices = (new string[] { "Black Market", "Restock", "Refuel", "Repair", "Contacts", "Universal Cartographics", "Missions", "Crew Lounge", "Tuning", "Search and Rescue" }),
                    ControllingFaction = new ControllingFaction() { Id = 1, Name = "Mother Gaia" },
                    UpdateTime = updateTime,
                    Body = new Body() { Id = 14780, Name = "Moon" }
                },
                new Station()
                {
                    Id = 1822,
                    MarketId = 128017664,
                    Type = "Orbis Starport",
                    Name = "Mars High",
                    DistanceToArrival = 714,
                    Allegiance = "Federation",
                    Government = "Democracy",
                    Economy = "Service",
                    SecondEconomy = null,
                    HaveMarket = true,
                    HaveShipyard = true,
                    HaveOutfitting = true,
                    OtherServices = (new string[] { "Black Market", "Restock", "Refuel", "Repair", "Contacts", "Universal Cartographics", "Missions", "Crew Lounge", "Tuning", "Search and Rescue" }),
                    ControllingFaction = new ControllingFaction() { Id = 223, Name = "Sol Workers' Party" },
                    UpdateTime = updateTime,
                    Body = new Body() { Id = 1828, Name = "Mars" }
                }
            };
            return new SystemStations(27, 10477373803, "Sol", "https://www.edsm.net/en/system/stations/id/27/name/Sol", stations);
        }

        private static Market GetMarketData()
        {
            List<Commodity> commodities = new()
            {
                new Commodity()
                {
                    Id = "advancedcatalysers",
                    Name = "Advanced Catalysers",
                    BuyPrice = 0,
                    Stock = 0,
                    SellPrice = 3435,
                    Demand = 3626984,
                    StockBracket = 0
                },
                new Commodity()
                {
                    Id = "aluminium",
                    Name = "Aluminium",
                    BuyPrice = 267,
                    Stock = 2676732,
                    SellPrice = 245,
                    Demand = 1,
                    StockBracket = 3
                },
                new Commodity()
                {
                    Id = "gold",
                    Name = "Gold",
                    BuyPrice = 49800,
                    Stock = 4615,
                    SellPrice = 48746,
                    Demand = 1,
                    StockBracket = 2
                }
            };
            return new Market(27, 10477373803, "Sol", 128016640, 560, "Galileo", "https://www.edsm.net/en/system/stations/id/27/name/Sol/details/facility/market/idS/560/nameS/Galileo", commodities);
        }

        private static Shipyard GetShipyardData()
        {
            List<Ship> ships = new()
            {
                new Ship() { Id = 128049255, Name = "Eagle" },
                new Ship() { Id = 128049309, Name = "Vulture" },
                new Ship() { Id = 128672145, Name = "Federal Assault Ship" }
            };
            return new Shipyard(27, 10477373803, "Sol", 128016640, 560, "Galileo", "https://www.edsm.net/en/system/stations/id/27/name/Sol/details/facility/market/idS/560/nameS/Galileo", ships);
        }

        private static StationOutfitting GetOutfittingData()
        {
            List<ShipModule> modules = new()
            {
                new ShipModule() { Id = "eagle_armour_grade2", Name = "1I Reinforced Alloy" },
                new ShipModule() { Id = "hpt_multicannon_gimbal_huge", Name = "4A Multi-Cannon (Gimbal)" },
                new ShipModule() { Id = "int_buggybay_size2_class2", Name = "2G Planetary Vehicle Hangar" }
            };
            return new StationOutfitting(27, 10477373803, "Sol", 128016640, 560, "Galileo", "https://www.edsm.net/en/system/stations/id/27/name/Sol/details/facility/market/idS/560/nameS/Galileo", modules);
        }

        private static SystemFactions GetFactionsData()
        {
            ControllingFaction controlFaction = new() { Id = 1, Name = "Mother Gaia", Allegiance = "Federation", Government = "Democracy" };
            List<Faction> factions = new()
            {
                new Faction()
                {
                    Id = 1,
                    Name = "Mother Gaia",
                    Allegiance = "Federation",
                    Government = "Democracy",
                    Influence = 0.298897,
                    State = "Boom",
                    ActiveStates = (new FactionState[] { new FactionState() { State = "Boom" } }),
                    RecoveringStates = (new FactionState[] { new FactionState() { State = "Civil liberty", Trend = 0 } }),
                    PendingStates = null,
                    Happiness = "Happy",
                    IsPlayer = false,
                    LastUpdate = 1642890937
                },
                new Faction()
                {
                    Id = 963,
                    Name = "Federal Congress",
                    Allegiance = "Federation",
                    Government = "Democracy",
                    Influence = 0.174524,
                    State = "None",
                    ActiveStates = null,
                    RecoveringStates = null,
                    PendingStates = null,
                    Happiness = "Happy",
                    IsPlayer = false,
                    LastUpdate = 1642890937
                },
                new Faction()
                {
                    Id = 79985,
                    Name = "Aegis Core",
                    Allegiance = "Independent",
                    Government = "Cooperative",
                    Influence = 0.082247,
                    State = "None",
                    ActiveStates = null,
                    RecoveringStates = null,
                    PendingStates = null,
                    Happiness = "Happy",
                    IsPlayer = false,
                    LastUpdate = 1642890937
                },
            };
            return new SystemFactions(27, 10477373803, "Sol", "https://www.edsm.net/en/system/stations/id/27/name/Sol/details/facility/market/idS/560/nameS/Galileo", controlFaction, factions);
        }
    }
}

using EDlib;
using EDlib.EDSM;
using EDlib.Mock.Platform;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class EdsmSystemTests
    {
        [TestMethod]
        public async Task StationsTest()
        {
            SystemService systemService = SystemService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            SystemStations stations;
            try
            {
                stations = await systemService.GetStations("Sol").ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(stations);
            Assert.IsTrue(stations.Id > 0);
            Assert.IsTrue(stations.Id64 > 0);
            Assert.AreEqual(stations.Name, "Sol");
            Assert.IsFalse(string.IsNullOrWhiteSpace(stations.Url));
            Assert.IsTrue(stations.Stations.Count > 0);
            Assert.IsTrue(stations.LastUpdated > DateTime.MinValue);

            Station gStation = stations.Stations.Find(x => x.Name == "Galileo");
            Assert.IsNotNull(gStation);
            Assert.IsTrue(gStation.Id > 0);
            Assert.IsTrue(gStation.MarketId > 0);
            Assert.AreEqual(gStation.Type, "Ocellus Starport");
            Assert.AreEqual(gStation.Name, "Galileo");
            Assert.IsTrue(gStation.DistanceToArrival > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(gStation.Allegiance));
            Assert.IsFalse(string.IsNullOrWhiteSpace(gStation.Government));
            Assert.AreEqual(gStation.Economy, "Refinery");
            Assert.IsTrue(string.IsNullOrWhiteSpace(gStation.SecondEconomy));
            Assert.IsTrue(gStation.HaveMarket);
            Assert.IsTrue(gStation.HaveOutfitting);
            Assert.IsTrue(gStation.HaveShipyard);
            Assert.IsTrue(gStation.OtherServices.Length > 0);
            Assert.IsNotNull(gStation.ControllingFaction);
            Assert.IsNotNull(gStation.UpdateTime);
            Assert.IsTrue(gStation.UpdateTime.Information > DateTime.MinValue);
            Assert.IsTrue(gStation.UpdateTime.Market > DateTime.MinValue);
            Assert.IsTrue(gStation.UpdateTime.Shipyard > DateTime.MinValue);
            Assert.IsTrue(gStation.UpdateTime.Outfitting > DateTime.MinValue);
            Assert.IsNull(gStation.Body);

            Station hStation = stations.Stations.Find(x => x.Name == "Haberlandt Survey");
            Assert.IsNotNull(hStation);
            Assert.IsTrue(hStation.Id > 0);
            Assert.IsTrue(hStation.MarketId > 0);
            Assert.AreEqual(hStation.Type, "Planetary Outpost");
            Assert.AreEqual(hStation.Name, "Haberlandt Survey");
            Assert.IsTrue(hStation.DistanceToArrival > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(hStation.Allegiance));
            Assert.IsFalse(string.IsNullOrWhiteSpace(hStation.Government));
            Assert.AreEqual(hStation.Economy, "Military");
            Assert.IsTrue(string.IsNullOrWhiteSpace(hStation.SecondEconomy));
            Assert.IsTrue(hStation.HaveMarket);
            Assert.IsTrue(hStation.HaveOutfitting);
            Assert.IsFalse(hStation.HaveShipyard);
            Assert.IsTrue(hStation.OtherServices.Length > 0);
            Assert.IsNotNull(hStation.ControllingFaction);
            Assert.IsNotNull(hStation.UpdateTime);
            Assert.IsTrue(hStation.UpdateTime.Information > DateTime.MinValue);
            Assert.IsTrue(hStation.UpdateTime.Market > DateTime.MinValue);
            Assert.IsNull(hStation.UpdateTime.Shipyard);
            Assert.IsTrue(hStation.UpdateTime.Outfitting > DateTime.MinValue);
            Assert.IsNotNull(hStation.Body);
            Assert.IsTrue(hStation.Body.Id > 0);
            Assert.AreEqual(hStation.Body.Name, "Europa");
            Assert.IsNotNull(hStation.Body.Latitude);
            Assert.IsNotNull(hStation.Body.Longitude);
        }

        [TestMethod]
        public async Task MarketTest()
        {
            SystemService systemService = SystemService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            Market gMarket;
            try
            {
                gMarket = await systemService.GetMarket(128016640).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(gMarket);
            Assert.IsTrue(gMarket.Id > 0);
            Assert.IsTrue(gMarket.Id64 > 0);
            Assert.AreEqual(gMarket.Name, "Sol");
            Assert.AreEqual(gMarket.MarketId, 128016640);
            Assert.IsTrue(gMarket.SId > 0);
            Assert.AreEqual(gMarket.SName, "Galileo");
            Assert.IsFalse(string.IsNullOrWhiteSpace(gMarket.Url));
            Assert.IsTrue(gMarket.Commodities.Count > 0);
            Assert.IsTrue(gMarket.LastUpdated > DateTime.MinValue);

            Commodity aluminium = gMarket.Commodities.Find(x => x.Name == "Aluminium");
            Assert.IsNotNull(aluminium);
            Assert.AreEqual(aluminium.Id, "aluminium");
            Assert.AreEqual(aluminium.Name, "Aluminium");
            Assert.IsTrue(aluminium.BuyPrice > 0);
            Assert.IsTrue(aluminium.SellPrice > 0);
            Assert.IsTrue(aluminium.Stock > 0);
            Assert.IsTrue(aluminium.Demand > 0);
            Assert.IsTrue(aluminium.StockBracket > 0);
        }

        [TestMethod]
        public async Task ShipyardTest()
        {
            SystemService systemService = SystemService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            Shipyard gShipyard;
            try
            {
                gShipyard = await systemService.GetShipyard(128016640).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(gShipyard);
            Assert.IsTrue(gShipyard.Id > 0);
            Assert.IsTrue(gShipyard.Id64 > 0);
            Assert.AreEqual(gShipyard.Name, "Sol");
            Assert.AreEqual(gShipyard.MarketId, 128016640);
            Assert.IsTrue(gShipyard.SId > 0);
            Assert.AreEqual(gShipyard.SName, "Galileo");
            Assert.IsFalse(string.IsNullOrWhiteSpace(gShipyard.Url));
            Assert.IsTrue(gShipyard.Ships.Count > 0);
            Assert.IsTrue(gShipyard.LastUpdated > DateTime.MinValue);

            Ship eagle = gShipyard.Ships.Find(x => x.Name == "Eagle");
            Assert.IsNotNull(eagle);
            Assert.IsTrue(eagle.Id > 0);
            Assert.AreEqual(eagle.Name, "Eagle");
        }

        [TestMethod]
        public async Task OutfittingTest()
        {
            SystemService systemService = SystemService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            StationOutfitting gOutfit;
            try
            {
                gOutfit = await systemService.GetOutfitting(128016640).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(gOutfit);
            Assert.IsTrue(gOutfit.Id > 0);
            Assert.IsTrue(gOutfit.Id64 > 0);
            Assert.AreEqual(gOutfit.Name, "Sol");
            Assert.AreEqual(gOutfit.MarketId, 128016640);
            Assert.IsTrue(gOutfit.SId > 0);
            Assert.AreEqual(gOutfit.SName, "Galileo");
            Assert.IsFalse(string.IsNullOrWhiteSpace(gOutfit.Url));
            Assert.IsTrue(gOutfit.Outfitting.Count > 0);
            Assert.IsTrue(gOutfit.LastUpdated > DateTime.MinValue);

            ShipModule eagleArmour = gOutfit.Outfitting.Find(x => x.Id == "eagle_armour_grade1");
            Assert.IsNotNull(eagleArmour);
            Assert.AreEqual(eagleArmour.Id, "eagle_armour_grade1");
            Assert.AreEqual(eagleArmour.Name, "1I Lightweight Alloy");
        }

        [TestMethod]
        public async Task FactionsTest()
        {
            SystemService systemService = SystemService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            SystemFactions factions;
            try
            {
                factions = await systemService.GetFactions("Sol").ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(factions);
            Assert.IsTrue(factions.Id > 0);
            Assert.IsTrue(factions.Id64 > 0);
            Assert.AreEqual(factions.Name, "Sol");
            Assert.IsFalse(string.IsNullOrWhiteSpace(factions.Url));
            Assert.IsNotNull(factions.ControllingFaction);
            Assert.IsTrue(factions.ControllingFaction.Id > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(factions.ControllingFaction.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(factions.ControllingFaction.Allegiance));
            Assert.IsFalse(string.IsNullOrWhiteSpace(factions.ControllingFaction.Government));
            Assert.IsNotNull(factions.Factions);
            Assert.IsTrue(factions.Factions.Count > 0);
            Assert.IsTrue(factions.LastUpdated > DateTime.MinValue);

            Faction mgFaction = factions.Factions.Find(x => x.Name == "Mother Gaia");
            Assert.IsNotNull(mgFaction);
            Assert.IsTrue(mgFaction.Id > 0);
            Assert.AreEqual(mgFaction.Name, "Mother Gaia");
            Assert.AreEqual(mgFaction.Allegiance, "Federation");
            Assert.AreEqual(mgFaction.Government, "Democracy");
            Assert.IsTrue(mgFaction.Influence > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(mgFaction.State));
            foreach (FactionState state in mgFaction.ActiveStates)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(state.State));
                Assert.IsNull(state.Trend);
            }
            foreach (FactionState state in mgFaction.RecoveringStates)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(state.State));
                Assert.IsNotNull(state.Trend);
            }
            foreach (FactionState state in mgFaction.PendingStates)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(state.State));
                Assert.IsNotNull(state.Trend);
            }
            Assert.IsFalse(string.IsNullOrWhiteSpace(mgFaction.Happiness));
            Assert.IsFalse(mgFaction.IsPlayer);
            Assert.IsTrue(mgFaction.LastUpdate > 0);
        }
    }
}

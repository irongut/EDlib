using EDlib.Mock.Platform;
using EDlib.Powerplay;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class PowerplayTests
    {
        [TestMethod]
        public void NewPowerCommsTest()
        {
            PowerComms comms = new PowerComms(3, "ALD", "reddit/ald", "discord/ald");
            Assert.AreEqual(comms.Id, 3);
            Assert.AreEqual(comms.ShortName, "ALD");
            Assert.AreEqual(comms.Reddit, "reddit/ald");
            Assert.AreEqual(comms.Comms, "discord/ald");
        }

        [TestMethod]
        public async Task PowerCommsTest()
        {
            PowerDetailsService pdService = PowerDetailsService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            PowerComms comms = await pdService.GetPowerCommsAsync("ALD").ConfigureAwait(false);
            Assert.AreEqual(comms.Id, 3);
            Assert.AreEqual(comms.ShortName, "ALD");
            Assert.IsTrue(comms.Reddit.Contains("reddit.com", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(comms.Comms.Contains("discord", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(comms.ToString().Contains("ALD", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void PowerDetailsTest()
        {
            PowerDetailsService pdService = PowerDetailsService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            PowerDetails details = pdService.GetPowerDetails("ALD");
            Assert.AreEqual(details.Id, 3);
            Assert.AreEqual(details.HQ, "Kamadhenu");
            Assert.AreEqual(details.YearOfBirth, 3243);
            Assert.AreEqual(details.Allegiance, "Empire");
            Assert.AreEqual(details.PreparationEthos, "Social");
            Assert.AreEqual(details.ExpansionEthos, "Combat");
            Assert.AreEqual(details.ExpansionStrongGovernment, "Feudal & Patronage");
            Assert.AreEqual(details.ExpansionWeakGovernment, "Dictatorship");
            Assert.AreEqual(details.ControlEthos, "Combat");
            Assert.AreEqual(details.ControlStrongGovernment, "Feudal & Patronage");
            Assert.AreEqual(details.ControlWeakGovernment, "Dictatorship");
            Assert.IsNotNull(details.ControlSystemEffect);
            Assert.IsNotNull(details.AllianceExploitedEffect);
            Assert.IsNotNull(details.EmpireExploitedEffect);
            Assert.IsNotNull(details.FederationExploitedEffect);
            Assert.IsNotNull(details.IndependentExploitedEffect);
            Assert.IsNotNull(details.Rating1);
            Assert.IsNotNull(details.Rating2);
            Assert.IsNotNull(details.Rating3);
            Assert.IsNotNull(details.Rating4);
            Assert.IsNotNull(details.Rating5);
            Assert.IsTrue(details.ToString().Contains("ALD", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void NewPowerStandingTest()
        {
            DateTime date = DateTime.Now;
            PowerStanding power = new PowerStanding(3,
                                                    "Arissa",
                                                    1,
                                                    StandingChange.up,
                                                    false,
                                                    "Empire",
                                                    "ALD",
                                                    date);
            Assert.AreEqual(power.Id, 3);
            Assert.AreEqual(power.Name, "Arissa");
            Assert.AreEqual(power.Position, 1);
            Assert.AreEqual(power.Change, StandingChange.up);
            Assert.AreEqual(power.ChangeString, "Up");
            Assert.IsFalse(power.Turmoil);
            Assert.AreEqual(power.Allegiance, "Empire");
            Assert.AreEqual(power.ShortName, "ALD");
            Assert.AreEqual(power.LastUpdated, date);
            Assert.IsTrue(power.ToString().Contains("Arissa", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void NewPowerStandingTestToo()
        {
            DateTime date = DateTime.Now;
            PowerStanding power = new PowerStanding(1, "Arissa Lavigny-Duval (+1)", 260, date);
            Assert.AreEqual(power.Id, 3);
            Assert.AreEqual(power.Name, "Arissa Lavigny-Duval");
            Assert.AreEqual(power.Position, 1);
            Assert.AreEqual(power.Change, StandingChange.up);
            Assert.AreEqual(power.ChangeString, "Up");
            Assert.IsFalse(power.Turmoil);
            Assert.AreEqual(power.Allegiance, "Empire");
            Assert.AreEqual(power.ShortName, "ALD");
            Assert.AreEqual(power.LastUpdated, date);
            Assert.IsTrue(power.ToString().Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task StandingsTest()
        {
            StandingsService service = StandingsService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            GalacticStandings standings = await service.GetData(new CancellationTokenSource()).ConfigureAwait(false);
            Assert.IsTrue(standings.Cycle > 250);
            Assert.IsTrue(standings.LastUpdated > DateTime.MinValue);
            Assert.IsTrue(standings.Standings.Count == 11);
            Assert.IsTrue(standings.ToString().Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(standings.ToJson().Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(standings.ToCSV().Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            PowerStanding ald = standings.Standings.Find(x => x.ShortName.Equals("ALD"));
            Assert.AreEqual(ald.Id, 3);
            Assert.AreEqual(ald.Name, "Arissa Lavigny-Duval");
            Assert.AreEqual(ald.Allegiance, "Empire");
            Assert.AreEqual(ald.LastUpdated, standings.LastUpdated);
            Assert.AreEqual(ald.Cycle, $"Cycle {standings.Cycle}");
        }
    }
}

using EDlib.Mock.Platform;
using EDlib.Network;
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
            Assert.AreEqual(3, comms.Id);
            Assert.AreEqual("ALD", comms.ShortName);
            Assert.AreEqual("reddit/ald", comms.Reddit);
            Assert.AreEqual("discord/ald", comms.Comms);
        }

        [TestMethod]
        public async Task PowerCommsTest()
        {
            PowerDetailsService pdService = PowerDetailsService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            PowerComms comms = await pdService.GetPowerCommsAsync("ALD", 1).ConfigureAwait(false);
            Assert.AreEqual(3, comms.Id);
            Assert.AreEqual("ALD", comms.ShortName);
            Assert.IsTrue(comms.Reddit.Contains("reddit.com", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(comms.Comms.Contains("discord", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(comms.ToString().Contains("ALD", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void PowerDetailsTest()
        {
            PowerDetailsService pdService = PowerDetailsService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            PowerDetails details = pdService.GetPowerDetails("ALD");
            Assert.AreEqual(3, details.Id);
            Assert.AreEqual("Kamadhenu", details.HQ);
            Assert.AreEqual(3243, details.YearOfBirth);
            Assert.AreEqual("Empire", details.Allegiance);
            Assert.AreEqual("Social", details.PreparationEthos);
            Assert.AreEqual("Combat", details.ExpansionEthos);
            Assert.AreEqual("Feudal & Patronage", details.ExpansionStrongGovernment);
            Assert.AreEqual("Dictatorship", details.ExpansionWeakGovernment);
            Assert.AreEqual("Combat", details.ControlEthos);
            Assert.AreEqual("Feudal & Patronage", details.ControlStrongGovernment);
            Assert.AreEqual("Dictatorship", details.ControlWeakGovernment);
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
            PowerStanding power = new PowerStanding(3, "Arissa", 1, StandingChange.up, false, "Empire", "ALD", date);
            Assert.AreEqual(3, power.Id);
            Assert.AreEqual("Arissa", power.Name);
            Assert.AreEqual(1, power.Position);
            Assert.AreEqual(StandingChange.up, power.Change);
            Assert.AreEqual("Up", power.ChangeString);
            Assert.IsFalse(power.Turmoil);
            Assert.AreEqual("Empire", power.Allegiance);
            Assert.AreEqual("ALD", power.ShortName);
            Assert.AreEqual(date, power.LastUpdated);
            Assert.AreEqual(-1, power.CyclesSinceTurmoil);
            Assert.IsTrue(power.ToString().Contains("Arissa", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void NewPowerStandingSinceTurmoilTest()
        {
            DateTime date = DateTime.Now;
            PowerStanding power = new PowerStanding(3, "Arissa", 1, StandingChange.up, false, "Empire", "ALD", date, 46);
            Assert.AreEqual(3, power.Id);
            Assert.AreEqual("Arissa", power.Name);
            Assert.AreEqual(1, power.Position);
            Assert.AreEqual(StandingChange.up, power.Change);
            Assert.AreEqual("Up", power.ChangeString);
            Assert.IsFalse(power.Turmoil);
            Assert.AreEqual("Empire", power.Allegiance);
            Assert.AreEqual("ALD", power.ShortName);
            Assert.AreEqual(date, power.LastUpdated);
            Assert.AreEqual(46, power.CyclesSinceTurmoil);
            Assert.IsTrue(power.ToString().Contains("Arissa", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void NewPowerStandingBackendTest()
        {
            DateTime date = DateTime.Now;
            PowerStanding power = new PowerStanding(1, "Arissa Lavigny-Duval (+1)", 260, date);
            Assert.AreEqual(3, power.Id);
            Assert.AreEqual("Arissa Lavigny-Duval", power.Name);
            Assert.AreEqual(1, power.Position);
            Assert.AreEqual(StandingChange.up, power.Change);
            Assert.AreEqual("Up", power.ChangeString);
            Assert.IsFalse(power.Turmoil);
            Assert.AreEqual("Empire", power.Allegiance);
            Assert.AreEqual("ALD", power.ShortName);
            Assert.AreEqual(date, power.LastUpdated);
            Assert.AreEqual(-1, power.CyclesSinceTurmoil);
            Assert.IsTrue(power.ToString().Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task StandingsTest()
        {
            StandingsService service = StandingsService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()), new EmptyCache());
            GalacticStandings standings = await service.GetData(new CancellationTokenSource()).ConfigureAwait(false);
            Assert.IsTrue(standings.Cycle > 250);
            Assert.IsTrue(standings.LastUpdated > DateTime.MinValue);
            Assert.IsTrue(standings.Standings.Count == 11);
            Assert.IsTrue(standings.ToString().Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(standings.ToJson().Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(standings.ToCSV().Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            PowerStanding ald = standings.Standings.Find(x => x.ShortName.Equals("ALD"));
            Assert.AreEqual(3, ald.Id);
            Assert.AreEqual("Arissa Lavigny-Duval", ald.Name);
            Assert.AreEqual("Empire", ald.Allegiance);
            Assert.AreEqual(standings.LastUpdated, ald.LastUpdated);
            Assert.AreEqual($"Cycle {standings.Cycle}", ald.Cycle);
            Assert.AreEqual(-1, ald.CyclesSinceTurmoil); // will need to change once backend includes the data
        }
    }
}

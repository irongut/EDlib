using EDlib.Powerplay;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class PowerplayTests
    {
        [TestMethod]
        public void New_PowerComms_Test()
        {
            PowerComms comms = new PowerComms(3, "ALD", "reddit-ALD", "comms-ALD");
            Assert.AreEqual(3, comms.Id);
            Assert.AreEqual("ALD", comms.ShortName);
            Assert.AreEqual("reddit-ALD", comms.Reddit);
            Assert.AreEqual("comms-ALD", comms.Comms);
        }

        [TestMethod]
        public void New_PowerStanding_Test()
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
        }

        [TestMethod]
        public void New_PowerStanding_CyclesSinceTurmoil_Test()
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
        }

        [TestMethod]
        public void New_PowerStanding_Backend_Test()
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
        }
    }
}

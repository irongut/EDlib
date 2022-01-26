using EDlib.Network;
using EDlib.Powerplay;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class PowerDetailsServiceTests
    {
        [TestMethod]
        public async Task PowerComms_Get_Test()
        {
            string data = JsonConvert.SerializeObject(GetTestData());
            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((data, DateTime.Now));

            PowerDetailsService pdService = PowerDetailsService.Instance(mockDownloadService.Object);
            PowerComms comms = await pdService.GetPowerCommsAsync("ALD", 1).ConfigureAwait(false);

            Assert.AreEqual(3, comms.Id);
            Assert.AreEqual("ALD", comms.ShortName);
            Assert.AreEqual("reddit-ALD", comms.Reddit);
            Assert.AreEqual("comms-ALD", comms.Comms);

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public void PowerDetails_Test()
        {
            string data = string.Empty;
            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((data, DateTime.Now));

            PowerDetailsService pdService = PowerDetailsService.Instance(mockDownloadService.Object);
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
            Assert.IsFalse(string.IsNullOrWhiteSpace(details.ControlSystemEffect));
            Assert.IsFalse(string.IsNullOrWhiteSpace(details.AllianceExploitedEffect));
            Assert.IsFalse(string.IsNullOrWhiteSpace(details.EmpireExploitedEffect));
            Assert.IsFalse(string.IsNullOrWhiteSpace(details.FederationExploitedEffect));
            Assert.IsFalse(string.IsNullOrWhiteSpace(details.IndependentExploitedEffect));
            Assert.IsFalse(string.IsNullOrWhiteSpace(details.Rating1));
            Assert.IsFalse(string.IsNullOrWhiteSpace(details.Rating2));
            Assert.IsFalse(string.IsNullOrWhiteSpace(details.Rating3));
            Assert.IsFalse(string.IsNullOrWhiteSpace(details.Rating4));
            Assert.IsFalse(string.IsNullOrWhiteSpace(details.Rating5));

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Never());
        }

        private static List<PowerComms> GetTestData()
        {
            return new()
            {
                new PowerComms(1, "Aisling", "reddit-Aisling", "comms-Aisling"),
                new PowerComms(2, "Delaine", "reddit-Delaine", "comms-Delaine"),
                new PowerComms(3, "ALD", "reddit-ALD", "comms-ALD"),
                new PowerComms(4, "Patreus", "reddit-Patreus", "comms-Patreus"),
                new PowerComms(5, "Mahon", "reddit-Mahon", "comms-Mahon"),
                new PowerComms(6, "Winters", "reddit-Winters", "comms-Winters"),
                new PowerComms(7, "LYR", "reddit-LYR", "comms-LYR"),
                new PowerComms(8, "Antal", "reddit-Antal", "comms-Antal"),
                new PowerComms(9, "Grom", "reddit-Grom", "comms-Grom"),
                new PowerComms(10, "Hudson", "reddit-Hudson", "comms-Hudson"),
                new PowerComms(11, "Torval", "reddit-Torval", "comms-Torval")
            };
        }
    }
}

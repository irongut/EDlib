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
    public class EdsmSystemsServiceTests
    {
        [TestMethod]
        public void Options_All_Test()
        {
            SystemsOptions allOptions = GetAllOptions();

            Assert.IsTrue(allOptions.ShowId);
            Assert.IsTrue(allOptions.ShowCoordinates);
            Assert.IsTrue(allOptions.ShowPermit);
            Assert.IsTrue(allOptions.ShowInformation);
            Assert.IsTrue(allOptions.ShowPrimaryStar);
        }

        [TestMethod]
        public void Options_IdOnly_Test()
        {
            SystemsOptions idOptions = new()
            {
                ShowId = true
            };

            Assert.IsTrue(idOptions.ShowId);
            Assert.IsFalse(idOptions.ShowCoordinates);
            Assert.IsFalse(idOptions.ShowPermit);
            Assert.IsFalse(idOptions.ShowInformation);
            Assert.IsFalse(idOptions.ShowPrimaryStar);
        }

        [TestMethod]
        public void Options_Comparison_Test()
        {
            SystemsOptions allOptions = GetAllOptions();

            SystemsOptions idOptions = new()
            {
                ShowId = true
            };

            Assert.IsTrue(allOptions.Equals(allOptions));
            Assert.IsTrue(allOptions.Equals((object)allOptions));
            Assert.IsFalse(allOptions.Equals(idOptions));

            Assert.AreNotEqual(allOptions.GetHashCode(), idOptions.GetHashCode());
        }

        [TestMethod]
        public async Task System_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetSolSystemData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemsService systemsService = SystemsService.Instance(mockEdsm.Object);
            SolarSystem solSystem = await systemsService.GetSystem("Sol", GetAllOptions()).ConfigureAwait(false);

            Assert.AreEqual("Sol", solSystem.Name);
            Assert.AreEqual(0, solSystem.Distance);
            Assert.AreEqual(27, solSystem.Id);
            Assert.AreEqual(10477373803, solSystem.Id64);
            Assert.AreEqual(0, solSystem.Coords.X);
            Assert.AreEqual(0, solSystem.Coords.Y);
            Assert.AreEqual(0, solSystem.Coords.Z);
            Assert.IsTrue(solSystem.CoordsLocked);
            Assert.IsTrue(solSystem.RequiresPermit);
            Assert.AreEqual("Sol", solSystem.PermitName);
            Assert.AreEqual("Federation", solSystem.Information.Allegiance);
            Assert.AreEqual("Democracy", solSystem.Information.Government);
            Assert.AreEqual("Mother Gaia", solSystem.Information.Faction);
            Assert.AreEqual("Boom", solSystem.Information.FactionState);
            Assert.AreEqual(22780919531, solSystem.Information.Population);
            Assert.AreEqual("High", solSystem.Information.Security);
            Assert.AreEqual("Refinery", solSystem.Information.Economy);
            Assert.AreEqual("Service", solSystem.Information.SecondEconomy);
            Assert.AreEqual("Common", solSystem.Information.Reserve);
            Assert.AreEqual("Sol", solSystem.PrimaryStar.Name);
            Assert.AreEqual("G (White-Yellow) Star", solSystem.PrimaryStar.Type);
            Assert.IsTrue(solSystem.PrimaryStar.IsScoopable);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task System_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(GetAchenarSystemData());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemsService systemsService = SystemsService.Instance(mockEdsm.Object);
            SolarSystem achenarSystem = await systemsService.GetSystem("Achenar", GetAllOptions(), new CancellationTokenSource()).ConfigureAwait(false);

            Assert.AreEqual("Achenar", achenarSystem.Name);
            Assert.AreEqual(0, achenarSystem.Distance);
            Assert.AreEqual(12523, achenarSystem.Id);
            Assert.AreEqual(164098653, achenarSystem.Id64);
            Assert.AreEqual(67.5, achenarSystem.Coords.X);
            Assert.AreEqual(-119.46875, achenarSystem.Coords.Y);
            Assert.AreEqual(24.84375, achenarSystem.Coords.Z);
            Assert.IsTrue(achenarSystem.CoordsLocked);
            Assert.IsTrue(achenarSystem.RequiresPermit);
            Assert.AreEqual("Achenar", achenarSystem.PermitName);
            Assert.AreEqual("Empire", achenarSystem.Information.Allegiance);
            Assert.AreEqual("Patronage", achenarSystem.Information.Government);
            Assert.AreEqual("Achenar Empire League", achenarSystem.Information.Faction);
            Assert.AreEqual("None", achenarSystem.Information.FactionState);
            Assert.AreEqual(16380054761, achenarSystem.Information.Population);
            Assert.AreEqual("High", achenarSystem.Information.Security);
            Assert.AreEqual("Refinery", achenarSystem.Information.Economy);
            Assert.AreEqual("Service", achenarSystem.Information.SecondEconomy);
            Assert.AreEqual("null", achenarSystem.Information.Reserve);
            Assert.AreEqual("Achenar", achenarSystem.PrimaryStar.Name);
            Assert.AreEqual("B (Blue-White) Star", achenarSystem.PrimaryStar.Type);
            Assert.IsTrue(achenarSystem.PrimaryStar.IsScoopable);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Systems_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(new List<SolarSystem>()
            {
                GetSolSystemData(),
                GetAchenarSystemData(),
                GetGatewaySystemData()
            });
            string[] systemNames = { "Sol", "Achenar", "Gateway" };

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemsService systemsService = SystemsService.Instance(mockEdsm.Object);
            (List<SolarSystem> systems, DateTime lastUpdated) = await systemsService.GetSystems(systemNames, GetAllOptions()).ConfigureAwait(false);

            Assert.AreEqual(3, systems.Count);
            Assert.AreEqual("Sol", systems[0].Name);
            Assert.AreEqual("Achenar", systems[1].Name);
            Assert.AreEqual("Gateway", systems[2].Name);
            Assert.AreEqual(timestamp, lastUpdated);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task Systems_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(new List<SolarSystem>()
            {
                GetSolSystemData(),
                GetAchenarSystemData(),
                GetGatewaySystemData()
            });
            string[] systemNames = { "Sol", "Achenar", "Gateway" };

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemsService systemsService = SystemsService.Instance(mockEdsm.Object);
            (List<SolarSystem> systems, DateTime lastUpdated) = await systemsService.GetSystems(systemNames, GetAllOptions(), new CancellationTokenSource()).ConfigureAwait(false);

            Assert.AreEqual(3, systems.Count);
            Assert.AreEqual("Sol", systems[0].Name);
            Assert.AreEqual("Achenar", systems[1].Name);
            Assert.AreEqual("Gateway", systems[2].Name);
            Assert.AreEqual(timestamp, lastUpdated);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task SystemsCube_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(new List<SolarSystem>()
            {
                GetAchenarSystemData(),
                GetGatewaySystemData()
            });

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemsService systemsService = SystemsService.Instance(mockEdsm.Object);
            (List<SolarSystem> systems, DateTime lastUpdated) = await systemsService.GetSystemsInCube("Sol", 10, GetAllOptions(), new CancellationTokenSource()).ConfigureAwait(false);

            Assert.AreEqual(2, systems.Count);
            Assert.AreEqual("Achenar", systems[0].Name);
            Assert.AreEqual("Gateway", systems[1].Name);
            Assert.AreEqual(timestamp, lastUpdated);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task SystemsCube_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(new List<SolarSystem>()
            {
                GetAchenarSystemData(),
                GetGatewaySystemData()
            });

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemsService systemsService = SystemsService.Instance(mockEdsm.Object);
            (List<SolarSystem> systems, DateTime lastUpdated) = await systemsService.GetSystemsInCube("Sol", 10, GetAllOptions()).ConfigureAwait(false);

            Assert.AreEqual(2, systems.Count);
            Assert.AreEqual("Achenar", systems[0].Name);
            Assert.AreEqual("Gateway", systems[1].Name);
            Assert.AreEqual(timestamp, lastUpdated);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task SystemsSphere_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(new List<SolarSystem>()
            {
                GetSolSystemData(),
                GetGatewaySystemData()
            });

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemsService systemsService = SystemsService.Instance(mockEdsm.Object);
            (List<SolarSystem> systems, DateTime lastUpdated) = await systemsService.GetSystemsInSphere("Achenar", 8, 4, GetAllOptions()).ConfigureAwait(false);

            Assert.AreEqual(2, systems.Count);
            Assert.AreEqual("Sol", systems[0].Name);
            Assert.AreEqual("Gateway", systems[1].Name);
            Assert.AreEqual(timestamp, lastUpdated);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task SystemsSphere_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(new List<SolarSystem>()
            {
                GetSolSystemData(),
                GetGatewaySystemData()
            });

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            SystemsService systemsService = SystemsService.Instance(mockEdsm.Object);
            (List<SolarSystem> systems, DateTime lastUpdated) = await systemsService.GetSystemsInSphere("Achenar", 8, 4, GetAllOptions(), new CancellationTokenSource()).ConfigureAwait(false);

            Assert.AreEqual(2, systems.Count);
            Assert.AreEqual("Sol", systems[0].Name);
            Assert.AreEqual("Gateway", systems[1].Name);
            Assert.AreEqual(timestamp, lastUpdated);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        private static SystemsOptions GetAllOptions()
        {
            return new SystemsOptions()
            {
                ShowId = true,
                ShowCoordinates = true,
                ShowPermit = true,
                ShowInformation = true,
                ShowPrimaryStar = true
            };
        }

        private static SolarSystem GetSolSystemData()
        {
            Coords solCoords = new()
            {
                X = 0,
                Y = 0,
                Z = 0
            };
            PrimaryStar solStar = new()
            {
                Name = "Sol",
                Type = "G (White-Yellow) Star",
                IsScoopable = true
            };
            SystemInfo solInfo = new()
            {
                Allegiance = "Federation",
                Government = "Democracy",
                Faction = "Mother Gaia",
                FactionState = "Boom",
                Population = 22780919531,
                Security = "High",
                Economy = "Refinery",
                SecondEconomy = "Service",
                Reserve = "Common"
            };
            return new SolarSystem("Sol", 0, 27, 10477373803, solCoords, true, true, "Sol", solInfo, solStar);
        }

        private static SolarSystem GetAchenarSystemData()
        {
            Coords achenarCoords = new()
            {
                X = 67.5,
                Y = -119.46875,
                Z = 24.84375
            };
            PrimaryStar achenarStar = new()
            {
                Name = "Achenar",
                Type = "B (Blue-White) Star",
                IsScoopable = true
            };
            SystemInfo achenarInfo = new()
            {
                Allegiance = "Empire",
                Government = "Patronage",
                Faction = "Achenar Empire League",
                FactionState = "None",
                Population = 16380054761,
                Security = "High",
                Economy = "Refinery",
                SecondEconomy = "Service",
                Reserve = "null"
            };
            return new SolarSystem("Achenar", 0, 12523, 164098653, achenarCoords, true, true, "Achenar", achenarInfo, achenarStar);
        }

        private static SolarSystem GetGatewaySystemData()
        {
            Coords gatewayCoords = new()
            {
                X = -11,
                Y = 77.84375,
                Z = -0.875
            };
            PrimaryStar gatewayStar = new()
            {
                Name = "Gateway",
                Type = "K (Yellow-Orange giant) Star",
                IsScoopable = true
            };
            SystemInfo gatewayInfo = new()
            {
                Allegiance = "Alliance",
                Government = "Corporate",
                Faction = "Alliance Office of Statistics",
                FactionState = "Boom",
                Population = 400027502,
                Security = "Medium",
                Economy = "Refinery",
                SecondEconomy = "Agriculture",
                Reserve = "null"
            };
            return new SolarSystem("Gateway", 0, 19886, 2832631665362, gatewayCoords, true, false, string.Empty, gatewayInfo, gatewayStar);
        }
    }
}

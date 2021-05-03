using EDlib;
using EDlib.EDSM;
using EDlib.Mock.Platform;
using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class EdsmSystemsTests
    {
        [TestMethod]
        public async Task SolSystemTest()
        {
            SystemsOptions options = new()
            {
                ShowId = true,
                ShowCoordinates = true,
                ShowPermit = true,
                ShowInformation = true,
                ShowPrimaryStar = true
            };
            SystemsService systemsService = SystemsService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            SolarSystem solSystem;
            try
            {
                solSystem = await systemsService.GetSystem("Sol", options).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(solSystem);
            Assert.AreEqual("Sol", solSystem.Name);
            Assert.AreEqual(0, solSystem.Distance);
            Assert.IsTrue(solSystem.Id > 0);
            Assert.IsTrue(solSystem.Id64 > 0);
            Assert.AreEqual(solSystem.Coords.X, 0);
            Assert.AreEqual(solSystem.Coords.Y, 0);
            Assert.AreEqual(solSystem.Coords.Z, 0);
            Assert.IsTrue(solSystem.CoordsLocked);
            Assert.IsTrue(solSystem.RequiresPermit);
            Assert.AreEqual("Sol", solSystem.PermitName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.Allegiance));
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.Government));
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.Faction));
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.FactionState));
            Assert.IsTrue(solSystem.Information.Population > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.Security));
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.Economy));
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.SecondEconomy));
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.Reserve));
            Assert.AreEqual(solSystem.PrimaryStar.Name, "Sol");
            Assert.AreEqual(solSystem.PrimaryStar.Type, "G (White-Yellow) Star");
            Assert.IsTrue(solSystem.PrimaryStar.IsScoopable);
            Assert.IsTrue(solSystem.LastUpdated > DateTime.MinValue);
        }

        [TestMethod]
        public async Task SolSystemWithCancelTest()
        {
            SystemsOptions options = new()
            {
                ShowId = true,
                ShowCoordinates = true,
                ShowPermit = true,
                ShowInformation = true,
                ShowPrimaryStar = true
            };
            SystemsService systemsService = SystemsService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            SolarSystem solSystem;
            try
            {
                solSystem = await systemsService.GetSystem("Sol", options, new CancellationTokenSource()).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(solSystem);
            Assert.AreEqual("Sol", solSystem.Name);
            Assert.AreEqual(0, solSystem.Distance);
            Assert.IsTrue(solSystem.Id > 0);
            Assert.IsTrue(solSystem.Id64 > 0);
            Assert.AreEqual(solSystem.Coords.X, 0);
            Assert.AreEqual(solSystem.Coords.Y, 0);
            Assert.AreEqual(solSystem.Coords.Z, 0);
            Assert.IsTrue(solSystem.CoordsLocked);
            Assert.IsTrue(solSystem.RequiresPermit);
            Assert.AreEqual("Sol", solSystem.PermitName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.Allegiance));
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.Government));
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.Faction));
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.FactionState));
            Assert.IsTrue(solSystem.Information.Population > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.Security));
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.Economy));
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.SecondEconomy));
            Assert.IsFalse(string.IsNullOrWhiteSpace(solSystem.Information.Reserve));
            Assert.AreEqual(solSystem.PrimaryStar.Name, "Sol");
            Assert.AreEqual(solSystem.PrimaryStar.Type, "G (White-Yellow) Star");
            Assert.IsTrue(solSystem.PrimaryStar.IsScoopable);
            Assert.IsTrue(solSystem.LastUpdated > DateTime.MinValue);
        }

        [TestMethod]
        public async Task NgaraweSystemTest()
        {
            SystemsOptions options = new()
            {
                ShowPermit = true
            };
            SystemsService systemsService = SystemsService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            SolarSystem ngSystem;
            try
            {
                ngSystem = await systemsService.GetSystem("Ngarawe", options).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(ngSystem);
            Assert.AreEqual("Ngarawe", ngSystem.Name);
            Assert.AreEqual(0, ngSystem.Distance);
            Assert.IsTrue(ngSystem.Id == 0);
            Assert.IsTrue(ngSystem.Id64 == 0);
            Assert.IsNull(ngSystem.Coords);
            Assert.IsFalse(ngSystem.CoordsLocked);
            Assert.IsFalse(ngSystem.RequiresPermit);
            Assert.IsNull(ngSystem.PermitName);
            Assert.IsNull(ngSystem.Information);
            Assert.IsNull(ngSystem.PrimaryStar);
            Assert.IsTrue(ngSystem.LastUpdated > DateTime.MinValue);
        }

        [TestMethod]
        public async Task SystemsTest()
        {
            SystemsOptions options = new()
            {
                ShowId = true,
                ShowCoordinates = true
            };
            string[] systemNames = { "Achenar", "Ngarawe", "Sol" };
            SystemsService systemsService = SystemsService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            List<SolarSystem> systems;
            try
            {
                (systems, _) = await systemsService.GetSystems(systemNames, options).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(systems);
            Assert.IsTrue(systems.Count > 0);

            SolarSystem solSystem = systems.Find(x => x.Name == "Sol");
            Assert.IsNotNull(solSystem);
            Assert.AreEqual("Sol", solSystem.Name);
            Assert.AreEqual(0, solSystem.Distance);
            Assert.IsTrue(solSystem.Id > 0);
            Assert.IsTrue(solSystem.Id64 > 0);
            Assert.AreEqual(solSystem.Coords.X, 0);
            Assert.AreEqual(solSystem.Coords.Y, 0);
            Assert.AreEqual(solSystem.Coords.Z, 0);
            Assert.IsTrue(solSystem.CoordsLocked);

            SolarSystem acSystem = systems.Find(x => x.Name == "Achenar");
            Assert.IsNotNull(acSystem);
            Assert.AreEqual("Achenar", acSystem.Name);
            Assert.AreEqual(0, solSystem.Distance);
            Assert.IsTrue(acSystem.Id > 0);
            Assert.IsTrue(acSystem.Id64 > 0);
            Assert.IsTrue(acSystem.Coords.X != 0);
            Assert.IsTrue(acSystem.Coords.Y != 0);
            Assert.IsTrue(acSystem.Coords.Z != 0);
            Assert.IsTrue(acSystem.CoordsLocked);

            SolarSystem wSystem = systems.Find(x => x.Name == "Wolf 359");
            Assert.IsNull(wSystem);
        }

        [TestMethod]
        public async Task SystemsWithCancelTest()
        {
            SystemsOptions options = new()
            {
                ShowId = true,
                ShowCoordinates = true
            };
            string[] systemNames = { "Achenar", "Ngarawe", "Sol" };
            SystemsService systemsService = SystemsService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            List<SolarSystem> systems;
            try
            {
                (systems, _) = await systemsService.GetSystems(systemNames, options, new CancellationTokenSource()).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(systems);
            Assert.IsTrue(systems.Count > 0);

            SolarSystem solSystem = systems.Find(x => x.Name == "Sol");
            Assert.IsNotNull(solSystem);
            Assert.AreEqual("Sol", solSystem.Name);
            Assert.AreEqual(0, solSystem.Distance);
            Assert.IsTrue(solSystem.Id > 0);
            Assert.IsTrue(solSystem.Id64 > 0);
            Assert.AreEqual(solSystem.Coords.X, 0);
            Assert.AreEqual(solSystem.Coords.Y, 0);
            Assert.AreEqual(solSystem.Coords.Z, 0);
            Assert.IsTrue(solSystem.CoordsLocked);

            SolarSystem acSystem = systems.Find(x => x.Name == "Achenar");
            Assert.IsNotNull(acSystem);
            Assert.AreEqual("Achenar", acSystem.Name);
            Assert.AreEqual(0, solSystem.Distance);
            Assert.IsTrue(acSystem.Id > 0);
            Assert.IsTrue(acSystem.Id64 > 0);
            Assert.IsTrue(acSystem.Coords.X != 0);
            Assert.IsTrue(acSystem.Coords.Y != 0);
            Assert.IsTrue(acSystem.Coords.Z != 0);
            Assert.IsTrue(acSystem.CoordsLocked);

            SolarSystem wSystem = systems.Find(x => x.Name == "Wolf 359");
            Assert.IsNull(wSystem);
        }

        [TestMethod]
        public async Task SolCubeTest()
        {
            SystemsOptions options = new()
            {
                ShowId = true,
                ShowCoordinates = true
            };
            SystemsService systemsService = SystemsService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            List<SolarSystem> solCube;
            try
            {
                (solCube, _) = await systemsService.GetSystemsInCube("Sol", 10, options).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(solCube);
            Assert.IsTrue(solCube.Count > 0);

            SolarSystem solSystem = solCube.Find(x => x.Name == "Sol");
            Assert.IsNotNull(solSystem);
            Assert.AreEqual("Sol", solSystem.Name);
            Assert.AreEqual(0, solSystem.Distance);
            Assert.IsTrue(solSystem.Id > 0);
            Assert.IsTrue(solSystem.Id64 > 0);
            Assert.AreEqual(solSystem.Coords.X, 0);
            Assert.AreEqual(solSystem.Coords.Y, 0);
            Assert.AreEqual(solSystem.Coords.Z, 0);
            Assert.IsTrue(solSystem.CoordsLocked);

            SolarSystem acSystem = solCube.Find(x => x.Name == "Alpha Centauri");
            Assert.IsNotNull(acSystem);
            Assert.AreEqual("Alpha Centauri", acSystem.Name);
            Assert.IsTrue(acSystem.Distance > 0);
            Assert.IsTrue(acSystem.Id > 0);
            Assert.IsTrue(acSystem.Id64 > 0);
            Assert.IsTrue(acSystem.Coords.X != 0);
            Assert.IsTrue(acSystem.Coords.Y != 0);
            Assert.IsTrue(acSystem.Coords.Z != 0);
            Assert.IsTrue(acSystem.CoordsLocked);

            SolarSystem wSystem = solCube.Find(x => x.Name == "Wolf 359");
            Assert.IsNull(wSystem);
        }

        [TestMethod]
        public async Task SolCubeWithCancelTest()
        {
            SystemsOptions options = new()
            {
                ShowId = true,
                ShowCoordinates = true
            };
            SystemsService systemsService = SystemsService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            List<SolarSystem> solCube;
            try
            {
                (solCube, _) = await systemsService.GetSystemsInCube("Sol", 10, options, new CancellationTokenSource()).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(solCube);
            Assert.IsTrue(solCube.Count > 0);

            SolarSystem solSystem = solCube.Find(x => x.Name == "Sol");
            Assert.IsNotNull(solSystem);
            Assert.AreEqual("Sol", solSystem.Name);
            Assert.AreEqual(0, solSystem.Distance);
            Assert.IsTrue(solSystem.Id > 0);
            Assert.IsTrue(solSystem.Id64 > 0);
            Assert.AreEqual(solSystem.Coords.X, 0);
            Assert.AreEqual(solSystem.Coords.Y, 0);
            Assert.AreEqual(solSystem.Coords.Z, 0);
            Assert.IsTrue(solSystem.CoordsLocked);

            SolarSystem acSystem = solCube.Find(x => x.Name == "Alpha Centauri");
            Assert.IsNotNull(acSystem);
            Assert.AreEqual("Alpha Centauri", acSystem.Name);
            Assert.IsTrue(acSystem.Distance > 0);
            Assert.IsTrue(acSystem.Id > 0);
            Assert.IsTrue(acSystem.Id64 > 0);
            Assert.IsTrue(acSystem.Coords.X != 0);
            Assert.IsTrue(acSystem.Coords.Y != 0);
            Assert.IsTrue(acSystem.Coords.Z != 0);
            Assert.IsTrue(acSystem.CoordsLocked);

            SolarSystem wSystem = solCube.Find(x => x.Name == "Wolf 359");
            Assert.IsNull(wSystem);
        }

        [TestMethod]
        public async Task SolSphereTest()
        {
            SystemsOptions options = new()
            {
                ShowId = true,
                ShowCoordinates = true
            };
            SystemsService systemsService = SystemsService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            List<SolarSystem> solSphere;
            try
            {
                (solSphere, _) = await systemsService.GetSystemsInSphere("Sol", 8, 4, options).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(solSphere);
            Assert.IsTrue(solSphere.Count > 0);

            SolarSystem solSystem = solSphere.Find(x => x.Name == "Sol");
            Assert.IsNull(solSystem);

            SolarSystem acSystem = solSphere.Find(x => x.Name == "Alpha Centauri");
            Assert.IsNotNull(acSystem);
            Assert.AreEqual("Alpha Centauri", acSystem.Name);
            Assert.IsTrue(acSystem.Distance > 0);
            Assert.IsTrue(acSystem.Id > 0);
            Assert.IsTrue(acSystem.Id64 > 0);
            Assert.IsTrue(acSystem.Coords.X != 0);
            Assert.IsTrue(acSystem.Coords.Y != 0);
            Assert.IsTrue(acSystem.Coords.Z != 0);
            Assert.IsTrue(acSystem.CoordsLocked);

            SolarSystem wSystem = solSphere.Find(x => x.Name == "Wolf 359");
            Assert.IsNotNull(wSystem);
            Assert.IsTrue(wSystem.Distance > 0);
            Assert.IsTrue(wSystem.Id > 0);
            Assert.IsTrue(wSystem.Id64 > 0);
            Assert.IsTrue(wSystem.Coords.X != 0);
            Assert.IsTrue(wSystem.Coords.Y != 0);
            Assert.IsTrue(wSystem.Coords.Z != 0);
            Assert.IsTrue(wSystem.CoordsLocked);
        }

        [TestMethod]
        public async Task SolSphereWithCancelTest()
        {
            SystemsOptions options = new()
            {
                ShowId = true,
                ShowCoordinates = true
            };
            SystemsService systemsService = SystemsService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            List<SolarSystem> solSphere;
            try
            {
                (solSphere, _) = await systemsService.GetSystemsInSphere("Sol", 8, 4, options, new CancellationTokenSource()).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(solSphere);
            Assert.IsTrue(solSphere.Count > 0);

            SolarSystem solSystem = solSphere.Find(x => x.Name == "Sol");
            Assert.IsNull(solSystem);

            SolarSystem acSystem = solSphere.Find(x => x.Name == "Alpha Centauri");
            Assert.IsNotNull(acSystem);
            Assert.AreEqual("Alpha Centauri", acSystem.Name);
            Assert.IsTrue(acSystem.Distance > 0);
            Assert.IsTrue(acSystem.Id > 0);
            Assert.IsTrue(acSystem.Id64 > 0);
            Assert.IsTrue(acSystem.Coords.X != 0);
            Assert.IsTrue(acSystem.Coords.Y != 0);
            Assert.IsTrue(acSystem.Coords.Z != 0);
            Assert.IsTrue(acSystem.CoordsLocked);

            SolarSystem wSystem = solSphere.Find(x => x.Name == "Wolf 359");
            Assert.IsNotNull(wSystem);
            Assert.IsTrue(wSystem.Distance > 0);
            Assert.IsTrue(wSystem.Id > 0);
            Assert.IsTrue(wSystem.Id64 > 0);
            Assert.IsTrue(wSystem.Coords.X != 0);
            Assert.IsTrue(wSystem.Coords.Y != 0);
            Assert.IsTrue(wSystem.Coords.Z != 0);
            Assert.IsTrue(wSystem.CoordsLocked);
        }

        [TestMethod]
        public void OptionsTest()
        {
            SystemsOptions allOptions = new SystemsOptions()
            {
                ShowId = true,
                ShowCoordinates = true,
                ShowPermit = true,
                ShowInformation = true,
                ShowPrimaryStar = true
            };

            Assert.IsTrue(allOptions.ShowId);
            Assert.IsTrue(allOptions.ShowCoordinates);
            Assert.IsTrue(allOptions.ShowPermit);
            Assert.IsTrue(allOptions.ShowInformation);
            Assert.IsTrue(allOptions.ShowPrimaryStar);

            SystemsOptions idOptions = new SystemsOptions()
            {
                ShowId = true
            };

            Assert.IsTrue(idOptions.ShowId);
            Assert.IsFalse(idOptions.ShowCoordinates);
            Assert.IsFalse(idOptions.ShowPermit);
            Assert.IsFalse(idOptions.ShowInformation);
            Assert.IsFalse(idOptions.ShowPrimaryStar);

            Assert.IsTrue(allOptions.Equals(allOptions));
            Assert.IsTrue(allOptions.Equals((object)allOptions));
            Assert.IsFalse(allOptions.Equals(idOptions));

            Assert.AreNotEqual(allOptions.GetHashCode(), idOptions.GetHashCode());
        }
    }
}

﻿using EDlib.EDSM;
using EDlib.Mock.Platform;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class EdsmSystemsTests
    {
        [TestMethod]
        public async Task SolSystemTest()
        {
            SystemsOptions options = new SystemsOptions
            {
                ShowId = true,
                ShowCoordinates = true,
                ShowPermit = true,
                ShowInformation = true,
                ShowPrimaryStar = true
            };
            SystemsService systemsService = SystemsService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            SolarSystem solSystem = await systemsService.GetSystem("Sol", options).ConfigureAwait(false);
            Assert.IsNotNull(solSystem);
            Assert.AreEqual(solSystem.Name, "Sol");
            Assert.AreEqual(solSystem.Distance, 0);
            Assert.IsTrue(solSystem.Id > 0);
            Assert.IsTrue(solSystem.Id64 > 0);
            Assert.AreEqual(solSystem.Coords.X, 0);
            Assert.AreEqual(solSystem.Coords.Y, 0);
            Assert.AreEqual(solSystem.Coords.Z, 0);
            Assert.IsTrue(solSystem.CoordsLocked);
            Assert.IsTrue(solSystem.RequiresPermit);
            Assert.AreEqual(solSystem.PermitName, "Sol");
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
            SystemsOptions options = new SystemsOptions
            {
                ShowPermit = true
            };
            SystemsService systemsService = SystemsService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            SolarSystem ngSystem = await systemsService.GetSystem("Ngarawe", options).ConfigureAwait(false);
            Assert.IsNotNull(ngSystem);
            Assert.AreEqual(ngSystem.Name, "Ngarawe");
            Assert.AreEqual(ngSystem.Distance, 0);
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
            SystemsOptions options = new SystemsOptions
            {
                ShowId = true,
                ShowCoordinates = true
            };
            string[] systemNames = { "Achenar", "Ngarawe", "Sol" };
            SystemsService systemsService = SystemsService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            (List<SolarSystem> systems, DateTime lastUpdated) = await systemsService.GetSystems(systemNames, options).ConfigureAwait(false);
            Assert.IsNotNull(systems);
            Assert.IsTrue(systems.Count > 0);

            SolarSystem solSystem = systems.Find(x => x.Name == "Sol");
            Assert.IsNotNull(solSystem);
            Assert.AreEqual(solSystem.Name, "Sol");
            Assert.AreEqual(solSystem.Distance, 0);
            Assert.IsTrue(solSystem.Id > 0);
            Assert.IsTrue(solSystem.Id64 > 0);
            Assert.AreEqual(solSystem.Coords.X, 0);
            Assert.AreEqual(solSystem.Coords.Y, 0);
            Assert.AreEqual(solSystem.Coords.Z, 0);
            Assert.IsTrue(solSystem.CoordsLocked);

            SolarSystem acSystem = systems.Find(x => x.Name == "Achenar");
            Assert.IsNotNull(acSystem);
            Assert.AreEqual(acSystem.Name, "Achenar");
            Assert.AreEqual(solSystem.Distance, 0);
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
            SystemsOptions options = new SystemsOptions
            {
                ShowId = true,
                ShowCoordinates = true
            };
            SystemsService systemsService = SystemsService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            (List<SolarSystem> solCube, DateTime lastUpdated) = await systemsService.GetSystemsInCube("Sol", 10, options).ConfigureAwait(false);
            Assert.IsNotNull(solCube);
            Assert.IsTrue(solCube.Count > 0);

            SolarSystem solSystem = solCube.Find(x => x.Name == "Sol");
            Assert.IsNotNull(solSystem);
            Assert.AreEqual(solSystem.Name, "Sol");
            Assert.AreEqual(solSystem.Distance, 0);
            Assert.IsTrue(solSystem.Id > 0);
            Assert.IsTrue(solSystem.Id64 > 0);
            Assert.AreEqual(solSystem.Coords.X, 0);
            Assert.AreEqual(solSystem.Coords.Y, 0);
            Assert.AreEqual(solSystem.Coords.Z, 0);
            Assert.IsTrue(solSystem.CoordsLocked);

            SolarSystem acSystem = solCube.Find(x => x.Name == "Alpha Centauri");
            Assert.IsNotNull(acSystem);
            Assert.AreEqual(acSystem.Name, "Alpha Centauri");
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
            SystemsOptions options = new SystemsOptions
            {
                ShowId = true,
                ShowCoordinates = true
            };
            SystemsService systemsService = SystemsService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            (List<SolarSystem> solSphere, DateTime lastUpdated) = await systemsService.GetSystemsInSphere("Sol", 8, 4, options).ConfigureAwait(false);
            Assert.IsNotNull(solSphere);
            Assert.IsTrue(solSphere.Count > 0);

            SolarSystem solSystem = solSphere.Find(x => x.Name == "Sol");
            Assert.IsNull(solSystem);

            SolarSystem acSystem = solSphere.Find(x => x.Name == "Alpha Centauri");
            Assert.IsNotNull(acSystem);
            Assert.AreEqual(acSystem.Name, "Alpha Centauri");
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
    }
}

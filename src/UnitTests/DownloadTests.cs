using EDlib.Mock.Platform;
using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class DownloadTests
    {
        private const string url = "https://api.taranissoftware.com/elite-dangerous/galactic-standings.json";
        
        [TestMethod]
        public void DownloadOptionsTest()
        {
            DownloadOptions optionsZero = new DownloadOptions();
            Assert.IsNull(optionsZero.CancelToken);
            Assert.AreEqual(optionsZero.Expiry, TimeSpan.FromTicks(0));
            Assert.IsFalse(optionsZero.IgnoreCache);

            DownloadOptions optionsOne = new DownloadOptions(new CancellationTokenSource());
            Assert.IsNotNull(optionsOne.CancelToken);
            Assert.AreEqual(optionsOne.Expiry, TimeSpan.FromTicks(0));
            Assert.IsFalse(optionsOne.IgnoreCache);

            DownloadOptions optionsTwo = new DownloadOptions(TimeSpan.FromMinutes(5));
            Assert.IsNull(optionsTwo.CancelToken);
            Assert.AreEqual(optionsTwo.Expiry, TimeSpan.FromMinutes(5));
            Assert.IsFalse(optionsTwo.IgnoreCache);

            DownloadOptions optionsThree = new DownloadOptions(new CancellationTokenSource(), TimeSpan.FromMinutes(5));
            Assert.IsNotNull(optionsThree.CancelToken);
            Assert.AreEqual(optionsThree.Expiry, TimeSpan.FromMinutes(5));
            Assert.IsFalse(optionsThree.IgnoreCache);

            DownloadOptions optionsFour = new DownloadOptions(TimeSpan.FromMinutes(5), true);
            Assert.IsNull(optionsFour.CancelToken);
            Assert.AreEqual(optionsFour.Expiry, TimeSpan.FromMinutes(5));
            Assert.IsTrue(optionsFour.IgnoreCache);

            DownloadOptions optionsFive = new DownloadOptions(new CancellationTokenSource(), TimeSpan.FromMinutes(5), true);
            Assert.IsNotNull(optionsFive.CancelToken);
            Assert.AreEqual(optionsFive.Expiry, TimeSpan.FromMinutes(5));
            Assert.IsTrue(optionsFive.IgnoreCache);
        }

        [TestMethod]
        public async Task DownloadServiceTest()
        {
            DownloadService dService = DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection());
            DownloadOptions options = new DownloadOptions();
            (string data, DateTime lastUpdated) = await dService.GetData(url, options);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.IsTrue(data.Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);
        }

        [TestMethod]
        public async Task CachedDownloadServiceTest()
        {
            CachedDownloadService dService = CachedDownloadService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            DownloadOptions options = new DownloadOptions(TimeSpan.FromHours(1));
            (string data, DateTime lastUpdated) = await dService.GetData(url, options);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.IsTrue(data.Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);
        }

        [TestMethod]
        public async Task CachedDownloadServiceTwoTest()
        {
            CachedDownloadService dService = CachedDownloadService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            DownloadOptions options = new DownloadOptions(TimeSpan.FromHours(1));
            (string data, DateTime lastUpdated) = await dService.GetData(url, "testData", "testUpdated", options);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.IsTrue(data.Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);
        }
    }
}

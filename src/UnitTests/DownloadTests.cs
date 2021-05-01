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
            DownloadOptions optionsZero = new();
            Assert.IsNull(optionsZero.CancelToken);
            Assert.AreEqual(TimeSpan.FromTicks(0), optionsZero.Expiry);
            Assert.IsFalse(optionsZero.IgnoreCache);

            DownloadOptions optionsOne = new(new CancellationTokenSource());
            Assert.IsNotNull(optionsOne.CancelToken);
            Assert.AreEqual(TimeSpan.FromTicks(0), optionsOne.Expiry);
            Assert.IsFalse(optionsOne.IgnoreCache);

            DownloadOptions optionsTwo = new(TimeSpan.FromMinutes(5));
            Assert.IsNull(optionsTwo.CancelToken);
            Assert.AreEqual(TimeSpan.FromMinutes(5), optionsTwo.Expiry);
            Assert.IsFalse(optionsTwo.IgnoreCache);

            DownloadOptions optionsThree = new(new CancellationTokenSource(), TimeSpan.FromMinutes(5));
            Assert.IsNotNull(optionsThree.CancelToken);
            Assert.AreEqual(TimeSpan.FromMinutes(5), optionsThree.Expiry);
            Assert.IsFalse(optionsThree.IgnoreCache);

            DownloadOptions optionsFour = new(TimeSpan.FromMinutes(5), true);
            Assert.IsNull(optionsFour.CancelToken);
            Assert.AreEqual(TimeSpan.FromMinutes(5), optionsFour.Expiry);
            Assert.IsTrue(optionsFour.IgnoreCache);

            DownloadOptions optionsFive = new(new CancellationTokenSource(), TimeSpan.FromMinutes(5), true);
            Assert.IsNotNull(optionsFive.CancelToken);
            Assert.AreEqual(TimeSpan.FromMinutes(5), optionsFive.Expiry);
            Assert.IsTrue(optionsFive.IgnoreCache);
        }

        [TestMethod]
        public async Task DownloadServiceTest()
        {
            DownloadService dService = DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection());
            DownloadOptions options = new();
            (string data, DateTime lastUpdated) = await dService.GetData(url, options).ConfigureAwait(false);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.IsTrue(data.Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);
        }

        [TestMethod]
        public async Task CachedDownloadServiceTest()
        {
            CachedDownloadService dService = CachedDownloadService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            DownloadOptions options = new(TimeSpan.FromHours(1));
            (string data, DateTime lastUpdated) = await dService.GetData(url, options).ConfigureAwait(false);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.IsTrue(data.Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);
        }

        [TestMethod]
        public async Task CachedDownloadServiceTwoTest()
        {
            CachedDownloadService dService = CachedDownloadService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            DownloadOptions options = new(TimeSpan.FromHours(1));
            (string data, DateTime lastUpdated) = await dService.GetData(url, "testData", "testUpdated", options).ConfigureAwait(false);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.IsTrue(data.Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);
        }
    }
}

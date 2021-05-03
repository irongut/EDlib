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
            Assert.AreEqual(TimeSpan.FromTicks(0), optionsZero.Expiry);
            Assert.IsFalse(optionsZero.IgnoreCache);

            DownloadOptions optionsOne = new DownloadOptions(new CancellationTokenSource());
            Assert.IsNotNull(optionsOne.CancelToken);
            Assert.AreEqual(TimeSpan.FromTicks(0), optionsOne.Expiry);
            Assert.IsFalse(optionsOne.IgnoreCache);

            DownloadOptions optionsTwo = new DownloadOptions(TimeSpan.FromMinutes(5));
            Assert.IsNull(optionsTwo.CancelToken);
            Assert.AreEqual(TimeSpan.FromMinutes(5), optionsTwo.Expiry);
            Assert.IsFalse(optionsTwo.IgnoreCache);

            DownloadOptions optionsThree = new DownloadOptions(new CancellationTokenSource(), TimeSpan.FromMinutes(5));
            Assert.IsNotNull(optionsThree.CancelToken);
            Assert.AreEqual(TimeSpan.FromMinutes(5), optionsThree.Expiry);
            Assert.IsFalse(optionsThree.IgnoreCache);

            DownloadOptions optionsFour = new DownloadOptions(TimeSpan.FromMinutes(5), true);
            Assert.IsNull(optionsFour.CancelToken);
            Assert.AreEqual(TimeSpan.FromMinutes(5), optionsFour.Expiry);
            Assert.IsTrue(optionsFour.IgnoreCache);

            DownloadOptions optionsFive = new DownloadOptions(new CancellationTokenSource(), TimeSpan.FromMinutes(5), true);
            Assert.IsNotNull(optionsFive.CancelToken);
            Assert.AreEqual(TimeSpan.FromMinutes(5), optionsFive.Expiry);
            Assert.IsTrue(optionsFive.IgnoreCache);
        }

        [TestMethod]
        public async Task DownloadServiceTest()
        {
            DownloadService dService = DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection());
            (string data, DateTime lastUpdated) = await dService.GetData(url, new DownloadOptions()).ConfigureAwait(false);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.IsTrue(data.Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);
        }

        [TestMethod]
        public async Task CachedDownloadServiceTest()
        {
            CachedDownloadService dService = CachedDownloadService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            (string data, DateTime lastUpdated) = await dService.GetData(url, new DownloadOptions(TimeSpan.FromHours(1))).ConfigureAwait(false);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.IsTrue(data.Contains("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);
        }
    }
}

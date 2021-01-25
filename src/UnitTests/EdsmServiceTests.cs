using EDlib.EDSM;
using EDlib.Mock.Platform;
using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class EdsmServiceTests
    {
        [TestMethod]
        public async Task GetDataTest()
        {
            EdsmService edsmService = EdsmService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            (string json, DateTime lastUpdated) = await edsmService.GetData("api-status-v1/elite-server", null, TimeSpan.FromMinutes(5)).ConfigureAwait(false);
            Assert.IsFalse(string.IsNullOrWhiteSpace(json));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);
        }

        [TestMethod]
        public async Task GetDataWithCancelTest()
        {
            EdsmService edsmService = EdsmService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            (string json, DateTime lastUpdated) = await edsmService.GetData("api-status-v1/elite-server", null, TimeSpan.FromMinutes(5), new CancellationTokenSource()).ConfigureAwait(false);
            Assert.IsFalse(string.IsNullOrWhiteSpace(json));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);
        }
    }
}

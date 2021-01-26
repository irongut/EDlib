using EDlib;
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
    public class EdsmStatusTests
    {
        [TestMethod]
        public void UnknownStatusTest()
        {
            EliteStatus eliteStatus = new EliteStatus();
            Assert.IsTrue(eliteStatus.Status == -1);
            Assert.IsTrue(eliteStatus.Type.Contains("unknown", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(eliteStatus.Message.Contains("unknown", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(eliteStatus.LastUpdated > DateTime.MinValue);
        }

        [TestMethod]
        public async Task EliteStatusTest()
        {
            EliteStatusService statusService = EliteStatusService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            EliteStatus eliteStatus;
            DateTime lastUpdated;
            try
            {
                (eliteStatus, lastUpdated) = await statusService.GetData().ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(eliteStatus);
            Assert.IsTrue(lastUpdated > DateTime.Now.AddMinutes(-1));
            Assert.IsTrue(eliteStatus.Status != -1);
            Assert.IsTrue(eliteStatus.Type.Contains("success", StringComparison.OrdinalIgnoreCase)
                       || eliteStatus.Type.Contains("warning", StringComparison.OrdinalIgnoreCase)
                       || eliteStatus.Type.Contains("danger", StringComparison.OrdinalIgnoreCase));
            Assert.IsFalse(string.IsNullOrWhiteSpace(eliteStatus.Message));
            Assert.IsTrue(eliteStatus.LastUpdated > DateTime.MinValue);
        }

        [TestMethod]
        public async Task EliteStatusWithCancelTest()
        {
            EliteStatusService statusService = EliteStatusService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            EliteStatus eliteStatus;
            DateTime lastUpdated;
            try
            {
                (eliteStatus, lastUpdated) = await statusService.GetData(new CancellationTokenSource()).ConfigureAwait(false);
            }
            catch (APIException)
            {
                Assert.Inconclusive("Skipping test due to issue with EDSM API.");
                return;
            }

            Assert.IsNotNull(eliteStatus);
            Assert.IsTrue(lastUpdated > DateTime.Now.AddMinutes(-1));
            Assert.IsTrue(eliteStatus.Status != -1);
            Assert.IsTrue(eliteStatus.Type.Contains("success", StringComparison.OrdinalIgnoreCase)
                       || eliteStatus.Type.Contains("warning", StringComparison.OrdinalIgnoreCase)
                       || eliteStatus.Type.Contains("danger", StringComparison.OrdinalIgnoreCase));
            Assert.IsFalse(string.IsNullOrWhiteSpace(eliteStatus.Message));
            Assert.IsTrue(eliteStatus.LastUpdated > DateTime.MinValue);
        }
    }
}

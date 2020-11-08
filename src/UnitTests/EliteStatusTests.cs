using EDlib.EDSM;
using EDlib.Mock.Platform;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class EliteStatusTests
    {
        [TestMethod]
        public void UnknownStatusTest()
        {
            EliteStatus eliteStatus = new EliteStatus();
            Assert.IsTrue(eliteStatus.Status == -1);
            Assert.IsTrue(eliteStatus.Type.Contains("unknown", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(eliteStatus.Message.Contains("unknown", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public async Task EliteStatusTest()
        {
            EliteStatusService statusService = EliteStatusService.Instance("EDlib UnitTests", new EmptyCache(), new UnmeteredConnection());
            (EliteStatus eliteStatus, DateTime lastUpdated) = await statusService.GetData().ConfigureAwait(false);
            Assert.IsNotNull(eliteStatus);
            Assert.IsTrue(lastUpdated > DateTime.Now.AddMinutes(-1));
            Assert.IsTrue(eliteStatus.Status != -1);
            Assert.IsTrue(eliteStatus.Type.Contains("success", StringComparison.OrdinalIgnoreCase)
                       || eliteStatus.Type.Contains("warning", StringComparison.OrdinalIgnoreCase)
                       || eliteStatus.Type.Contains("danger", StringComparison.OrdinalIgnoreCase));
        }
    }
}

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
    public class EdsmEliteStatusServiceTests
    {
        [TestMethod]
        public void Status_Unknown_Test()
        {
            EliteStatus eliteStatus = new EliteStatus();
            Assert.AreEqual(-1, eliteStatus.Status);
            Assert.AreEqual("unknown", eliteStatus.Type);
            Assert.AreEqual("Status Unknown", eliteStatus.Message);
            Assert.IsTrue(eliteStatus.LastUpdated > DateTime.MinValue);
        }

        [TestMethod]
        public async Task EliteStatus_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(new EliteStatus(2, "success", "OK", timestamp));

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), null, It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            EliteStatusService statusService = EliteStatusService.Instance(mockEdsm.Object);
            (EliteStatus eliteStatus, DateTime lastUpdated) = await statusService.GetData().ConfigureAwait(false);

            Assert.AreEqual(2, eliteStatus.Status);
            Assert.AreEqual("success", eliteStatus.Type);
            Assert.AreEqual("OK", eliteStatus.Message);
            Assert.AreEqual(timestamp, eliteStatus.LastUpdated);
            Assert.AreEqual(timestamp, lastUpdated);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), null, It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task EliteStatus_Get_Unknown_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(new EliteStatus());

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), null, It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            EliteStatusService statusService = EliteStatusService.Instance(mockEdsm.Object);
            (EliteStatus eliteStatus, DateTime lastUpdated) = await statusService.GetData().ConfigureAwait(false);

            Assert.AreEqual(-1, eliteStatus.Status);
            Assert.AreEqual("unknown", eliteStatus.Type);
            Assert.AreEqual("Status Unknown", eliteStatus.Message);
            Assert.IsTrue(eliteStatus.LastUpdated > DateTime.MinValue);
            Assert.AreEqual(timestamp, lastUpdated);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), null, It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task EliteStatus_Get_WithCancel_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromMinutes(10);
            string edsmData = JsonConvert.SerializeObject(new EliteStatus(2, "success", "OK", timestamp));

            Mock<IEdsmService> mockEdsm = new();
            mockEdsm.Setup(x => x.GetData(It.IsAny<string>(), null, It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            EliteStatusService statusService = EliteStatusService.Instance(mockEdsm.Object);
            (EliteStatus eliteStatus, DateTime lastUpdated) = await statusService.GetData(new CancellationTokenSource()).ConfigureAwait(false);

            Assert.AreEqual(2, eliteStatus.Status);
            Assert.AreEqual("success", eliteStatus.Type);
            Assert.AreEqual("OK", eliteStatus.Message);
            Assert.AreEqual(timestamp, eliteStatus.LastUpdated);
            Assert.AreEqual(timestamp, lastUpdated);

            mockEdsm.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }
    }
}

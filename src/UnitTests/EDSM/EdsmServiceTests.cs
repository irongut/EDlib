using EDlib.EDSM;
using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class EdsmServiceTests
    {
        [TestMethod]
        public async Task EdsmService_Get_NoParameters_Test()
        {
            DateTime timestamp = DateTime.Now;
            const string edsmData = "edsm-data";

            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            EdsmService edsmService = EdsmService.Instance(mockDownloadService.Object);
            (string data, DateTime lastUpdated) = await edsmService.GetData("method", null, new DownloadOptions()).ConfigureAwait(false);

            Assert.AreEqual(edsmData, data);
            Assert.AreEqual(timestamp, lastUpdated);

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task EdsmService_Get_Test()
        {
            DateTime timestamp = DateTime.Now;
            const string edsmData = "edsm-data";
            Dictionary<string, string> edsmParams = new();
            edsmParams.Add("key1", "value1");
            edsmParams.Add("key2", "value2");

            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((edsmData, timestamp));

            EdsmService edsmService = EdsmService.Instance(mockDownloadService.Object);
            (string data, DateTime lastUpdated) = await edsmService.GetData("method", edsmParams, new DownloadOptions()).ConfigureAwait(false);

            Assert.AreEqual(edsmData, data);
            Assert.AreEqual(timestamp, lastUpdated);

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
        }
    }
}

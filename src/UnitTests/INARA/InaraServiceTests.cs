using EDlib;
using EDlib.INARA;
using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class InaraServiceTests
    {
        [TestMethod]
        public async Task InaraService_Get_Test()
        {
            DateTime timestamp = DateTime.Now;
            string inaraData = JsonConvert.SerializeObject(GetInaraRequestData());

            DownloadOptions options = new();
            InaraHeader header = new InaraHeader(new InaraIdentity("name", "version", "key", true));
            List<InaraEvent> input = new() { new InaraEvent("inaraMethod", new List<object>()) };

            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.PostData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((inaraData, timestamp));

            InaraService inaraService = InaraService.Instance(mockDownloadService.Object);
            (string data, DateTime lastUpdated) = await inaraService.GetData(header, input, options).ConfigureAwait(false);

            Assert.AreEqual(inaraData, data);
            Assert.AreEqual(timestamp, lastUpdated);

            mockDownloadService.Verify(x => x.PostData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public void InaraService_Get_HeaderError_Test()
        {
            DateTime timestamp = DateTime.Now;
            string inaraData = JsonConvert.SerializeObject(GetInaraRequestData(headerStatus: 204));

            DownloadOptions options = new();
            InaraHeader header = new InaraHeader(new InaraIdentity("name", "version", "key", true));
            List<InaraEvent> input = new() { new InaraEvent("inaraMethod", new List<object>()) };

            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.PostData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((inaraData, timestamp));

            InaraService inaraService = InaraService.Instance(mockDownloadService.Object);
            APIException ex = Assert.ThrowsExceptionAsync<APIException>(async () => await inaraService.GetData(header, input, options)).Result;

            Assert.AreEqual(204, ex.StatusCode);
            Assert.IsTrue(ex.Message.Contains("Error message", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void InaraService_Get_EventError_Test()
        {
            DateTime timestamp = DateTime.Now;
            string inaraData = JsonConvert.SerializeObject(GetInaraRequestData(eventStatus: 204));

            DownloadOptions options = new();
            InaraHeader header = new InaraHeader(new InaraIdentity("name", "version", "key", true));
            List<InaraEvent> input = new() { new InaraEvent("inaraMethod", new List<object>()) };

            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.PostData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((inaraData, timestamp));

            InaraService inaraService = InaraService.Instance(mockDownloadService.Object);
            APIException ex = Assert.ThrowsExceptionAsync<APIException>(async () => await inaraService.GetData(header, input, options)).Result;

            Assert.AreEqual(204, ex.StatusCode);
            Assert.IsTrue(ex.Message.Contains("Error message", StringComparison.OrdinalIgnoreCase));
        }

        private static InaraRequest GetInaraRequestData(int headerStatus = 200, int eventStatus = 200)
        {
            return new()
            {
                Header = new InaraHeader { EventStatus = headerStatus, EventStatusText = headerStatus > 203 ? "Error message" : string.Empty },
                Events = new List<InaraEvent> { new InaraEvent("inaraEvent", new List<object>()) { EventStatus = eventStatus, EventStatusText = eventStatus > 203 ? "Error message" : string.Empty } }
            };
        }
    }
}

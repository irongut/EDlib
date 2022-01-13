using EDlib;
using EDlib.Network;
using EDlib.Platform;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class DownloadServiceTests
    {
        private const string url = "https://httpbin.org/user-agent";
        private const string postUrl = "https://httpbin.org/post";

        [TestMethod]
        public async Task DownloadService_Get_NoNetwork_Test()
        {
            Mock<IConnectivityService> mockConnectivity = new();
            mockConnectivity.Setup(x => x.IsConnected()).Returns(false);

            DownloadService dService = DownloadService.Instance("EDlib UnitTests", mockConnectivity.Object);

            await Assert.ThrowsExceptionAsync<NoNetworkNoCacheException>(() => dService.GetData(url, new DownloadOptions()));
        }

        [TestMethod]
        public async Task DownloadService_Get_Test()
        {
            Mock<IConnectivityService> mockConnectivity = new();
            mockConnectivity.Setup(x => x.IsConnected()).Returns(true);

            DownloadService dService = DownloadService.Instance("EDlib UnitTests", mockConnectivity.Object);
            (string data, DateTime lastUpdated) = await dService.GetData(url, new DownloadOptions()).ConfigureAwait(false);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.IsTrue(data.Contains("user-agent", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);

            mockConnectivity.Verify(x => x.IsConnected(), Times.Once);
        }

        [TestMethod]
        public async Task DownloadService_Post_NoNetwork_Test()
        {
            Mock<IConnectivityService> mockConnectivity = new();
            mockConnectivity.Setup(x => x.IsConnected()).Returns(false);

            DownloadService dService = DownloadService.Instance("EDlib UnitTests", mockConnectivity.Object);

            await Assert.ThrowsExceptionAsync<NoNetworkNoCacheException>(() => dService.PostData(postUrl, "EDlib UnitTests", new DownloadOptions()));
        }

        [TestMethod]
        public async Task DownloadService_Post_Test()
        {
            Mock<IConnectivityService> mockConnectivity = new();
            mockConnectivity.Setup(x => x.IsConnected()).Returns(true);

            DownloadService dService = DownloadService.Instance("EDlib UnitTests", mockConnectivity.Object);
            (string data, DateTime lastUpdated) = await dService.PostData(postUrl, "EDlib UnitTests", new DownloadOptions()).ConfigureAwait(false);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.IsTrue(data.Contains("EDlib UnitTests", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);

            mockConnectivity.Verify(x => x.IsConnected(), Times.Once);
        }
    }
}

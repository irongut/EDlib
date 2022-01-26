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
    public class CachedDownloadServiceTests
    {
        private const string url = "https://httpbin.org/user-agent";
        private const string postUrl = "https://httpbin.org/post";

        [TestMethod]
        public async Task CachedDownloadService_Get_NoNetwork_NoCache_Test()
        {
            Mock<IConnectivityService> mockConnectivity = new();
            mockConnectivity.Setup(x => x.IsConnected()).Returns(false);

            Mock<ICacheService> mockCache = new();
            mockCache.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);

            CachedDownloadService cdService = CachedDownloadService.Instance("EDlib UnitTests", mockCache.Object, mockConnectivity.Object);

            await Assert.ThrowsExceptionAsync<NoNetworkNoCacheException>(() => cdService.GetData(url, new DownloadOptions()));
        }

        [TestMethod]
        public async Task CachedDownloadService_Get_NoNetwork_Cache_Test()
        {
            DateTime timestamp = DateTime.Now;

            Mock<IConnectivityService> mockConnectivity = new();
            mockConnectivity.Setup(x => x.IsConnected()).Returns(false);

            Mock<ICacheService> mockCache = new();
            mockCache.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            mockCache.Setup(x => x.IsExpired(It.IsAny<string>())).Returns(false);
            mockCache.Setup(x => x.Get(It.IsAny<string>())).Returns(timestamp.ToString());

            CachedDownloadService cdService = CachedDownloadService.Instance("EDlib UnitTests", mockCache.Object, mockConnectivity.Object);
            (string data, DateTime lastUpdated) = await cdService.GetData(url, new DownloadOptions(TimeSpan.FromHours(1))).ConfigureAwait(false);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.AreEqual(timestamp.ToString(), data);
            Assert.IsTrue(lastUpdated > DateTime.MinValue);

            mockConnectivity.Verify(x => x.IsConnected(), Times.Once);
            mockCache.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.IsExpired(It.IsAny<string>()), Times.Never);
            mockCache.Verify(x => x.Get(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task CachedDownloadService_Get_Cache_Test()
        {
            DateTime timestamp = DateTime.Now;

            Mock<IConnectivityService> mockConnectivity = new();
            mockConnectivity.Setup(x => x.IsConnected()).Returns(true);

            Mock<ICacheService> mockCache = new();
            mockCache.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            mockCache.Setup(x => x.IsExpired(It.IsAny<string>())).Returns(false);
            mockCache.Setup(x => x.Get(It.IsAny<string>())).Returns(timestamp.ToString());

            CachedDownloadService cdService = CachedDownloadService.Instance("EDlib UnitTests", mockCache.Object, mockConnectivity.Object);
            (string data, DateTime lastUpdated) = await cdService.GetData(url, new DownloadOptions(TimeSpan.FromHours(1))).ConfigureAwait(false);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.AreEqual(timestamp.ToString(), data);
            Assert.IsTrue(lastUpdated > DateTime.MinValue);

            mockConnectivity.Verify(x => x.IsConnected(), Times.Once);
            mockCache.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.IsExpired(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.Get(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task CachedDownloadService_Get_Test()
        {
            Mock<IConnectivityService> mockConnectivity = new();
            mockConnectivity.Setup(x => x.IsConnected()).Returns(true);

            Mock<ICacheService> mockCache = new();
            mockCache.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            mockCache.Setup(x => x.IsExpired(It.IsAny<string>())).Returns(true);
            mockCache.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()));

            CachedDownloadService cdService = CachedDownloadService.Instance("EDlib UnitTests", mockCache.Object, mockConnectivity.Object);
            (string data, DateTime lastUpdated) = await cdService.GetData(url, new DownloadOptions(TimeSpan.FromHours(1))).ConfigureAwait(false);

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.IsTrue(data.Contains("user-agent", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);

            mockConnectivity.Verify(x => x.IsConnected(), Times.Once);
            mockCache.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.IsExpired(It.IsAny<string>()), Times.Never);
            mockCache.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task CachedDownloadService_Post_NoNetwork_NoCache_Test()
        {
            Mock<IConnectivityService> mockConnectivity = new();
            mockConnectivity.Setup(x => x.IsConnected()).Returns(false);

            Mock<ICacheService> mockCache = new();
            mockCache.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);

            CachedDownloadService cdService = CachedDownloadService.Instance("EDlib UnitTests", mockCache.Object, mockConnectivity.Object);

            await Assert.ThrowsExceptionAsync<NoNetworkNoCacheException>(() => cdService.PostData(postUrl, "EDlib UnitTests", new DownloadOptions()));
        }

        [TestMethod]
        public async Task CachedDownloadService_Post_NoNetwork_Cache_Test()
        {
            DateTime timestamp = DateTime.Now;

            Mock<IConnectivityService> mockConnectivity = new();
            mockConnectivity.Setup(x => x.IsConnected()).Returns(true);

            Mock<ICacheService> mockCache = new();
            mockCache.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            mockCache.Setup(x => x.IsExpired(It.IsAny<string>())).Returns(false);
            mockCache.Setup(x => x.Get(It.IsAny<string>())).Returns(timestamp.ToString());

            CachedDownloadService cdService = CachedDownloadService.Instance("EDlib UnitTests", mockCache.Object, mockConnectivity.Object);
            (string data, DateTime lastUpdated) = await cdService.PostData(postUrl, "EDlib UnitTests", new DownloadOptions());

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.AreEqual(timestamp.ToString(), data);
            Assert.IsTrue(lastUpdated > DateTime.MinValue);

            mockConnectivity.Verify(x => x.IsConnected(), Times.Once);
            mockCache.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.IsExpired(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.Get(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task CachedDownloadService_Post_Cache_Test()
        {
            DateTime timestamp = DateTime.Now;

            Mock<IConnectivityService> mockConnectivity = new();
            mockConnectivity.Setup(x => x.IsConnected()).Returns(true);

            Mock<ICacheService> mockCache = new();
            mockCache.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            mockCache.Setup(x => x.IsExpired(It.IsAny<string>())).Returns(false);
            mockCache.Setup(x => x.Get(It.IsAny<string>())).Returns(timestamp.ToString());

            CachedDownloadService cdService = CachedDownloadService.Instance("EDlib UnitTests", mockCache.Object, mockConnectivity.Object);
            (string data, DateTime lastUpdated) = await cdService.PostData(postUrl, "EDlib UnitTests", new DownloadOptions());

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.AreEqual(timestamp.ToString(), data);
            Assert.IsTrue(lastUpdated > DateTime.MinValue);

            mockConnectivity.Verify(x => x.IsConnected(), Times.Once);
            mockCache.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.IsExpired(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.Get(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task CachedDownloadService_Post_Test()
        {
            Mock<IConnectivityService> mockConnectivity = new();
            mockConnectivity.Setup(x => x.IsConnected()).Returns(true);

            Mock<ICacheService> mockCache = new();
            mockCache.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);
            mockCache.Setup(x => x.IsExpired(It.IsAny<string>())).Returns(true);
            mockCache.Setup(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()));

            CachedDownloadService cdService = CachedDownloadService.Instance("EDlib UnitTests", mockCache.Object, mockConnectivity.Object);
            (string data, DateTime lastUpdated) = await cdService.PostData(postUrl, "EDlib UnitTests", new DownloadOptions());

            Assert.IsFalse(string.IsNullOrWhiteSpace(data));
            Assert.IsTrue(data.Contains("EDlib UnitTests", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);

            mockConnectivity.Verify(x => x.IsConnected(), Times.Once);
            mockCache.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
            mockCache.Verify(x => x.IsExpired(It.IsAny<string>()), Times.Never);
            mockCache.Verify(x => x.Add(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Exactly(2));
        }
    }
}

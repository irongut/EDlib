using EDlib.BGS;
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
    public class BGSTests
    {
        [TestMethod]
        public void UnknownTick_Test()
        {
            BgsTick tick = new BgsTick();
            Assert.AreEqual("Unknown", tick.TimeString);
        }

        [TestMethod]
        public void NewTick_Test()
        {
            DateTime date = DateTime.Now;
            BgsTick tick = new BgsTick(date);
            Assert.AreEqual(date.ToString("g"), tick.TimeString);
        }

        [TestMethod]
        public async Task LatestTick_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromHours(24);
            string tickData = JsonConvert.SerializeObject(new List<BgsTick>() { new BgsTick(timestamp) });

            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((tickData, DateTime.Now));

            BgsTickService bgsService = BgsTickService.Instance(mockDownloadService.Object);
            (BgsTick tick, DateTime lastUpdated) = await bgsService.GetData().ConfigureAwait(false);

            Assert.IsNotNull(tick);
            Assert.AreEqual(timestamp, tick.Time);
            Assert.AreEqual(timestamp.ToString("g"), tick.TimeString);
            Assert.IsTrue(lastUpdated > DateTime.Now.AddMinutes(-1));

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task LatestTick_Unknown_Test()
        {
            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((string.Empty, DateTime.Now));

            BgsTickService bgsService = BgsTickService.Instance(mockDownloadService.Object);
            (BgsTick tick, DateTime lastUpdated) = await bgsService.GetData().ConfigureAwait(false);

            Assert.IsNotNull(tick);
            Assert.AreEqual(DateTime.MinValue, tick.Time);
            Assert.AreEqual("Unknown", tick.TimeString);
            Assert.IsTrue(lastUpdated > DateTime.Now.AddMinutes(-1));

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task MultipleTick_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromHours(24);
            string tickData = JsonConvert.SerializeObject(new List<BgsTick>()
            {
                new BgsTick(timestamp),
                new BgsTick(timestamp - TimeSpan.FromDays(1)),
                new BgsTick(timestamp - TimeSpan.FromDays(2)),
                new BgsTick(timestamp - TimeSpan.FromDays(3)),
                new BgsTick(timestamp - TimeSpan.FromDays(4))
            });

            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((tickData, DateTime.Now));

            BgsTickService bgsService = BgsTickService.Instance(mockDownloadService.Object);
            (List<BgsTick> ticks, DateTime lastUpdated) = await bgsService.GetData(5).ConfigureAwait(false);

            Assert.IsNotNull(ticks);
            Assert.IsTrue(ticks.Count == 5);
            Assert.AreEqual(timestamp, ticks[0].Time);
            Assert.AreEqual(timestamp.ToString("g"), ticks[0].TimeString);
            Assert.IsTrue(lastUpdated > DateTime.Now.AddMinutes(-1));

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
        }
    }
}

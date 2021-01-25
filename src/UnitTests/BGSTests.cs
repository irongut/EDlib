using EDlib.BGS;
using EDlib.Network;
using EDlib.Mock.Platform;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class BGSTests
    {
        [TestMethod]
        public void UnknownTickTest()
        {
            BgsTick tick = new BgsTick();
            Assert.AreEqual(tick.TimeString, "Unknown");
        }

        [TestMethod]
        public void NewTickTest()
        {
            DateTime date = DateTime.Now;
            BgsTick tick = new BgsTick(date);
            Assert.AreEqual(tick.TimeString, date.ToString("g"));
        }

        [TestMethod]
        public async Task LatestTickTest()
        {
            BgsTickService bgsService = BgsTickService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            (BgsTick tick, DateTime lastUpdated) = await bgsService.GetData().ConfigureAwait(false);
            Assert.IsNotNull(tick);
            Assert.IsTrue(lastUpdated > DateTime.Now.AddMinutes(-1));
        }

        [TestMethod]
        public async Task MultipleTickTest()
        {
            BgsTickService bgsService = BgsTickService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            (List<BgsTick> ticks, DateTime lastUpdated) = await bgsService.GetData(5).ConfigureAwait(false);
            Assert.IsNotNull(ticks);
            Assert.IsTrue(lastUpdated > DateTime.Now.AddMinutes(-1));
            Assert.IsTrue(ticks.Count == 5);
        }
    }
}

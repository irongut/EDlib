using EDlib.Powerplay;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class CycleServiceTests
    {
        [TestMethod]
        public void TimeRemainingTest()
        {
            string timeRemaining = CycleService.TimeRemaining();
            TimeSpan time = CycleService.TimeTillTick();
            Assert.IsTrue(timeRemaining.IndexOf("hour") > 0);
            if (time.Days == 0)
            {
                Assert.IsTrue(timeRemaining.IndexOf("minute") > 0);
            }
            else
            {
                Assert.IsTrue(timeRemaining.IndexOf("day") > 0);
            }
        }

        [TestMethod]
        public void CycleImminentTest()
        {
            int day = (int)DateTime.UtcNow.DayOfWeek;
            int hour = DateTime.UtcNow.Hour;
            bool imminent = (day == 3 && hour >= 19) || (day == 4 && hour < 7);
            Assert.AreEqual(imminent, CycleService.CycleImminent());
        }

        [TestMethod]
        public void FinalDayTest()
        {
            DateTime currentUTC = DateTime.UtcNow;
            int day = (int)currentUTC.DayOfWeek;
            int hour = currentUTC.Hour;
            if ((day == 3 && hour >= 7) || (day == 4 && hour < 7))
            {
                Assert.IsTrue(CycleService.FinalDay());
            }
            else
            {
                Assert.IsFalse(CycleService.FinalDay());
            }
        }

    [TestMethod]
        public void TimeTillTickTest()
        {
            TimeSpan time = CycleService.TimeTillTick();
            Assert.IsTrue(time != TimeSpan.MaxValue);
            Assert.IsTrue(time > TimeSpan.Zero);
            Assert.IsTrue(time.Days < 7);
        }

        [TestMethod]
        public void CurrentCycleTest()
        {
            int cycle = CycleService.CurrentCycle();
            Assert.IsTrue(cycle > 200);
            Assert.IsTrue(cycle < 500);
        }
    }
}

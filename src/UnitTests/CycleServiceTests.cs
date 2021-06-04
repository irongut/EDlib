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
        public void CycleImminent12hTest()
        {
            int day = (int)DateTime.UtcNow.DayOfWeek;
            int hour = DateTime.UtcNow.Hour;
            if ((day == 3 && hour > 18) || (day == 4 && hour < 7))
            {
                Assert.IsTrue(CycleService.CycleImminent());
            }
            else
            {
                Assert.IsFalse(CycleService.CycleImminent());
            }
        }

        [TestMethod]
        public void CycleImminent5hTest()
        {
            int day = (int)DateTime.UtcNow.DayOfWeek;
            int hour = DateTime.UtcNow.Hour;
            if (day == 4 && hour > 1 && hour < 7)
            {
                Assert.IsTrue(CycleService.CycleImminent(5));
            }
            else
            {
                Assert.IsFalse(CycleService.CycleImminent(5));
            }
        }

        [TestMethod]
        public void CycleImminent24hTest()
        {
            int day = (int)DateTime.UtcNow.DayOfWeek;
            int hour = DateTime.UtcNow.Hour;
            if ((day == 3 && hour > 6) || (day == 4 && hour < 7))
            {
                Assert.IsTrue(CycleService.CycleImminent(24));
            }
            else
            {
                Assert.IsFalse(CycleService.CycleImminent(24));
            }
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

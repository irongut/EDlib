using EDlib.Powerplay;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace UnitTests
{
    [TestClass]
    public class CycleServiceTests
    {
        [TestMethod]
        public void PowerplayStartDate_Test()
        {
            Assert.AreEqual(DateTime.Parse("04-06-2015 07:00:00", new CultureInfo("en-GB")), CycleService.PowerplayStartDate);
        }

        [TestMethod]
        public void TimeTillTick_Current_Test()
        {
            TimeSpan time = CycleService.TimeTillTick();

            Assert.IsTrue(time < TimeSpan.MaxValue);
            Assert.IsTrue(time > TimeSpan.Zero);
            Assert.IsTrue(time.Days < 7);
        }

        [TestMethod]
        public void TimeTillTick_Mon_Test()
        {
            CycleService.FixedDate = DateTime.Parse("10-01-2022 01:00:00", new CultureInfo("en-GB"));
            TimeSpan time = CycleService.TimeTillTick();

            Assert.AreEqual(3, time.Days);
            Assert.AreEqual(6, time.Hours);
            Assert.AreEqual(0, time.Minutes);

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void TimeTillTick_Tues_Test()
        {
            CycleService.FixedDate = DateTime.Parse("11-01-2022 01:00:00", new CultureInfo("en-GB"));
            TimeSpan time = CycleService.TimeTillTick();

            Assert.AreEqual(2, time.Days);
            Assert.AreEqual(6, time.Hours);
            Assert.AreEqual(0, time.Minutes);

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void TimeTillTick_Wed_Test()
        {
            CycleService.FixedDate = DateTime.Parse("12-01-2022 01:00:00", new CultureInfo("en-GB"));
            TimeSpan time = CycleService.TimeTillTick();

            Assert.AreEqual(1, time.Days);
            Assert.AreEqual(6, time.Hours);
            Assert.AreEqual(0, time.Minutes);

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void TimeTillTick_Thurs_PreTick_Test()
        {
            CycleService.FixedDate = DateTime.Parse("13-01-2022 01:00:00", new CultureInfo("en-GB"));
            TimeSpan time = CycleService.TimeTillTick();

            Assert.AreEqual(0, time.Days);
            Assert.AreEqual(6, time.Hours);
            Assert.AreEqual(0, time.Minutes);

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void TimeTillTick_Thurs_PostTick_Test()
        {
            CycleService.FixedDate = DateTime.Parse("13-01-2022 13:00:00", new CultureInfo("en-GB"));
            TimeSpan time = CycleService.TimeTillTick();

            Assert.AreEqual(6, time.Days);
            Assert.AreEqual(18, time.Hours);
            Assert.AreEqual(0, time.Minutes);

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void TimeTillTick_Fri_Test()
        {
            CycleService.FixedDate = DateTime.Parse("14-01-2022 01:00:00", new CultureInfo("en-GB"));
            TimeSpan time = CycleService.TimeTillTick();

            Assert.AreEqual(6, time.Days);
            Assert.AreEqual(6, time.Hours);
            Assert.AreEqual(0, time.Minutes);

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void TimeTillTick_Sat_Test()
        {
            CycleService.FixedDate = DateTime.Parse("15-01-2022 01:00:00", new CultureInfo("en-GB"));
            TimeSpan time = CycleService.TimeTillTick();

            Assert.AreEqual(5, time.Days);
            Assert.AreEqual(6, time.Hours);
            Assert.AreEqual(0, time.Minutes);

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void TimeTillTick_Sun_Test()
        {
            CycleService.FixedDate = DateTime.Parse("16-01-2022 01:00:00", new CultureInfo("en-GB"));
            TimeSpan time = CycleService.TimeTillTick();

            Assert.AreEqual(4, time.Days);
            Assert.AreEqual(6, time.Hours);
            Assert.AreEqual(0, time.Minutes);

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void TimeRemaining_Current_Test()
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
        public void TimeRemaining_NoDays_Test()
        {
            CycleService.FixedDate = DateTime.Parse("13-01-2022 01:00:00", new CultureInfo("en-GB"));

            string timeRemaining = CycleService.TimeRemaining();

            Assert.IsFalse(timeRemaining.IndexOf("day") > -1);
            Assert.IsTrue(timeRemaining.IndexOf("6 hours") > -1);
            Assert.IsTrue(timeRemaining.IndexOf("0 minutes") > -1);

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void TimeRemaining_SingleDay_Test()
        {
            CycleService.FixedDate = DateTime.Parse("12-01-2022 01:00:00", new CultureInfo("en-GB"));

            string timeRemaining = CycleService.TimeRemaining();

            Assert.IsTrue(timeRemaining.IndexOf("1 day") > -1);
            Assert.IsFalse(timeRemaining.IndexOf("days") > -1);
            Assert.IsTrue(timeRemaining.IndexOf("6 hours") > -1);
            Assert.IsTrue(timeRemaining.IndexOf("0 minutes") > -1);

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void TimeRemaining_SingleHour_Test()
        {
            CycleService.FixedDate = DateTime.Parse("11-01-2022 06:00:00", new CultureInfo("en-GB"));

            string timeRemaining = CycleService.TimeRemaining();

            Assert.IsTrue(timeRemaining.IndexOf("2 days") > -1);
            Assert.IsTrue(timeRemaining.IndexOf("1 hour") > -1);
            Assert.IsFalse(timeRemaining.IndexOf("hours") > -1);
            Assert.IsTrue(timeRemaining.IndexOf("0 minutes") > -1);

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void TimeRemaining_SingleMinute_Test()
        {
            CycleService.FixedDate = DateTime.Parse("11-01-2022 01:59:00", new CultureInfo("en-GB"));

            string timeRemaining = CycleService.TimeRemaining();

            Assert.IsTrue(timeRemaining.IndexOf("2 days") > -1);
            Assert.IsTrue(timeRemaining.IndexOf("5 hours") > -1);
            Assert.IsTrue(timeRemaining.IndexOf("1 minute") > -1);
            Assert.IsFalse(timeRemaining.IndexOf("minutes") > -1);

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void CycleImminent_Current_Test()
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
        public void CycleImminent_True_Test()
        {
            CycleService.FixedDate = DateTime.Parse("13-01-2022 01:00:00", new CultureInfo("en-GB"));

            Assert.IsTrue(CycleService.CycleImminent());

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void CycleImminent_False_Test()
        {
            CycleService.FixedDate = DateTime.Parse("12-01-2022 01:00:00", new CultureInfo("en-GB"));

            Assert.IsFalse(CycleService.CycleImminent());

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void FinalDay_Current_Test()
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
        public void FinalDay_True_Test()
        {
            CycleService.FixedDate = DateTime.Parse("13-01-2022 01:00:00", new CultureInfo("en-GB"));

            Assert.IsTrue(CycleService.FinalDay());

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void FinalDay_False_Test()
        {
            CycleService.FixedDate = DateTime.Parse("12-01-2022 01:00:00", new CultureInfo("en-GB"));

            Assert.IsFalse(CycleService.FinalDay());

            CycleService.FixedDate = null;
        }

        [TestMethod]
        public void CurrentCycle_Current_Test()
        {
            int cycle = CycleService.CurrentCycle();
            Assert.IsTrue(cycle > 300);
        }

        [TestMethod]
        public void CurrentCycle_Fixed_Test()
        {
            CycleService.FixedDate = DateTime.Parse("12-01-2022 01:00:00", new CultureInfo("en-GB"));

            Assert.AreEqual(345, CycleService.CurrentCycle());

            CycleService.FixedDate = null;
        }
    }
}

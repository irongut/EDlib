using System;
using System.Globalization;

namespace EDlib.Powerplay
{
    /// <summary>A static class that deals with the Powerplay cycle and when it changes (ticks).</summary>
    public static class CycleService
    {
        /// <summary>Gets the time remaining till the end of the current Powerplay cycle.</summary>
        /// <returns>Days and hours until the last 24 hours, then hours and minutes.</returns>
        public static string TimeRemaining()
        {
            TimeSpan time = TimeTillTick();

            string hours;
            if (time.Hours == 1)
            {
                hours = "1 hour";
            }
            else
            {
                hours = String.Format("{0} hours", time.Hours);
            }

            if (time.Days == 0)
            {
                string minutes;
                if (time.Minutes == 1)
                {
                    minutes = "1 minute";
                }
                else
                {
                    minutes = String.Format("{0} minutes", time.Minutes);
                }
                return String.Format("{0} {1}", hours, minutes);
            }
            else if (time.Days == 1)
            {
                return String.Format("1 day {0}", hours);
            }
            else
            {
                return String.Format("{0} days {1}", time.Days, hours);
            }
        }

        /// <summary>Determines if the end of the current Powerplay cycle is imminent - less than 12 hours remaining.</summary>
        /// <returns><c>true</c> if imminent, else <c>false</c>.</returns>
        public static bool CycleImminent()
        {
            // last 12 hours
            DateTime currentUTC = DateTime.UtcNow;
            int day = (int)currentUTC.DayOfWeek;
            int hour = currentUTC.Hour;
            return (day == 3 && hour >= 19) || (day == 4 && hour < 7);
        }

        /// <summary>Determines if the current Powerplay cycle ends within 24 hours.</summary>
        /// <returns><c>true</c> if it is the final 24 hours of the cycle, else <c>false</c>.</returns>
        public static bool FinalDay()
        {
            // last 24 hours
            DateTime currentUTC = DateTime.UtcNow;
            int day = (int)currentUTC.DayOfWeek;
            int hour = currentUTC.Hour;
            return (day == 3 && hour >= 7) || (day == 4 && hour < 7);
        }

        /// <summary>Gets the time remaining till the end of the current Powerplay cycle.</summary>
        /// <returns>The time till the end of the cycle.</returns>
        public static TimeSpan TimeTillTick()
        {
            DateTime currentUTC = DateTime.UtcNow;
            int day = (int)currentUTC.DayOfWeek;
            TimeSpan timeDiff = TimeSpan.FromHours(7) - currentUTC.TimeOfDay;
            switch (day)
            {
                case 0: // Sunday
                    return TimeSpan.FromDays(4) + timeDiff;
                case 1:
                    return TimeSpan.FromDays(3) + timeDiff;
                case 2:
                    return TimeSpan.FromDays(2) + timeDiff;
                case 3:
                    return TimeSpan.FromDays(1) + timeDiff;
                case 4:
                    if (currentUTC.Hour < 7)
                    {
                        return TimeSpan.FromDays(0) + timeDiff;
                    }
                    else
                    {
                        return TimeSpan.FromDays(7) + timeDiff;
                    }
                case 5:
                    return TimeSpan.FromDays(6) + timeDiff;
                case 6:
                    return TimeSpan.FromDays(5) + timeDiff;
                default:
                    return TimeSpan.MaxValue;
            }
        }

        /// <summary>Gets the current Powerplay cycle number.</summary>
        /// <returns>The current Powerplay cycle number.</returns>
        public static int CurrentCycle()
        {
            DateTime ppStart = DateTime.Parse("04-06-2015 07:00:00", new CultureInfo("en-GB"));
            TimeSpan diff = DateTime.UtcNow - ppStart;
            return (diff.Days / 7) + 1;
        }
    }
}

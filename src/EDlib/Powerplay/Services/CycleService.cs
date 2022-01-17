using System;
using System.Globalization;

namespace EDlib.Powerplay
{
    /// <summary>A static class that represents the Powerplay cycle and when it changes (ticks).</summary>
    public static class CycleService
    {
        /// <summary>The date and time the first Powerplay cycle started.</summary>
        public static DateTime PowerplayStartDate { get; } = DateTime.Parse("04-06-2015 07:00:00", new CultureInfo("en-GB"));

        /// <summary>The time remaining till the end of the current Powerplay cycle.</summary>
        public static TimeSpan TimeTillTick()
        {
            TimeSpan timeDiff = TimeSpan.FromHours(7) - DateTime.UtcNow.TimeOfDay;
            switch ((int)DateTime.UtcNow.DayOfWeek)
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
                    if (DateTime.UtcNow.Hour < 7)
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

        /// <summary>
        ///   The time remaining till the end of the current Powerplay cycle.<br/>
        ///   Days, hours and minutes.
        /// </summary>
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
                hours = $"{time.Hours} hours";
            }

            string minutes;
            if (time.Minutes == 1)
            {
                minutes = "1 minute";
            }
            else
            {
                minutes = $"{time.Minutes} minutes";
            }

            if (time.Days == 0)
            {
                return $"{hours} {minutes}";
            }
            else if (time.Days == 1)
            {
                return $"1 day {hours} {minutes}";
            }
            else
            {
                return $"{time.Days} days {hours} {minutes}";
            }
        }

        /// <summary>Returns <c>true</c> if the end of the current Powerplay cycle is imminent - less than <c>imminentHours</c> hours remaining.</summary>
        /// <param name="imminentHours">The number of hours during which the end of the cycle is imminent (1 - 24).</param>
        public static bool CycleImminent(int imminentHours = 12)
        {
            return TimeTillTick() < TimeSpan.FromHours(imminentHours);
        }

        /// <summary>Returns <c>true</c> if the current Powerplay cycle ends within 24 hours.</summary>
        public static bool FinalDay() => CycleImminent(24);

        /// <summary>The current Powerplay cycle number.</summary>
        public static int CurrentCycle()
        {
            TimeSpan diff = DateTime.UtcNow - PowerplayStartDate;
            return (diff.Days / 7) + 1;
        }
    }
}

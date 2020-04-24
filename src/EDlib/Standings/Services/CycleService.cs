using System;

namespace EDlib.Standings
{
    public static class CycleService
    {
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

        public static bool CycleImminent()
        {
            // last 12 hours
            DateTime currentUTC = DateTime.UtcNow;
            int day = (int)(currentUTC.DayOfWeek);
            int hour = currentUTC.Hour;
            return (day == 3 && hour >= 19) || (day == 4 && hour < 7);
        }

        public static bool FinalDay()
        {
            // last 24 hours
            DateTime currentUTC = DateTime.UtcNow;
            int day = (int)(currentUTC.DayOfWeek);
            int hour = currentUTC.Hour;
            return (day == 3 && hour >= 7) || (day == 4 && hour < 7);
        }

        public static TimeSpan TimeTillTick()
        {
            DateTime currentUTC = DateTime.UtcNow;
            int day = (int)(currentUTC.DayOfWeek);
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

        public static int CurrentCycle()
        {
            DateTime ppStart = DateTime.Parse("2015-06-04 07:00:00");
            TimeSpan diff = DateTime.UtcNow - ppStart;
            return (diff.Days / 7) + 1;
        }
    }
}

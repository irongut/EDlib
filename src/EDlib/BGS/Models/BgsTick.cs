using Newtonsoft.Json;
using System;

namespace EDlib.BGS
{
    public class BgsTick
    {
        private DateTime _time;
        [JsonProperty(PropertyName = "TIME")]
        public DateTime Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    TimeString = value.ToString("g");
                }
            }
        }

        public string TimeString { get; internal set; }

        public BgsTick()
        {
            TimeString = "Unknown";
        }

        public BgsTick(DateTime time)
        {
            Time = time;
            TimeString = time.ToString("g");
        }
    }
}

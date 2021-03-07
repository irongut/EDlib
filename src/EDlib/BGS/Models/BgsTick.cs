using EDlib.Platform;
using Newtonsoft.Json;
using System;

namespace EDlib.BGS
{
    /// <summary>Data for the BGS Tick.</summary>
    public class BgsTick
    {
        private DateTime _time;
        /// <summary>Returns the date and time of the BGS tick.</summary>
        /// <value>Date and time of the BGS Tick.</value>
        [Preserve (Conditional=true)]
        [JsonProperty(PropertyName = "TIME")]
        public DateTime Time
        {
            get { return _time; }
            internal set
            {
                if (_time != value)
                {
                    _time = value;
                    TimeString = value.ToString("g");
                }
            }
        }

        /// <summary>Returns the date and time of the BGS tick as a string.</summary>
        /// <value>Date and time of the BGS Tick.</value>
        [Preserve(Conditional = true)]
        public string TimeString { get; internal set; }

        /// <summary>Initializes a new instance of the <see cref="BgsTick" /> class, with the value Unknown for the time string.</summary>
        [Preserve(Conditional = true)]
        public BgsTick()
        {
            TimeString = "Unknown";
        }

        /// <summary>Initializes a new instance of the <see cref="BgsTick" /> class with the specified date and time.</summary>
        /// <param name="time">Date and time of the BGS Tick.</param>
        [Preserve(Conditional = true)]
        public BgsTick(DateTime time)
        {
            Time = time;
            TimeString = time.ToString("g");
        }
    }
}

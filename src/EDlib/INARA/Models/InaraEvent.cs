using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EDlib.INARA
{
    public class InaraEvent
    {
        [JsonProperty(PropertyName = "eventName")]
        public string EventName { get; }

        [JsonProperty(PropertyName = "eventTimestamp")]
        public DateTime EventTimestamp { get; }

        [JsonProperty(PropertyName = "eventData")]
        public List<object> EventData { get; internal set; }

        [JsonProperty("eventStatus")]
        public int? EventStatus { get; internal set; }

        [JsonProperty("eventStatusText")]
        public string EventStatusText { get; internal set; }

        public InaraEvent(string name, List<object> data)
        {
            EventName = name;
            EventTimestamp = DateTime.Now;
            EventData = data;
        }
    }
}

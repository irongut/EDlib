using EDlib.Platform;
using Newtonsoft.Json;
using System;

namespace EDlib.INARA
{
    public class InaraEvent
    {
        [JsonProperty(PropertyName = "eventName")]
        public string EventName { get; }

        [JsonProperty(PropertyName = "eventTimestamp")]
        public DateTime EventTimestamp { get; }

        [JsonProperty(PropertyName = "eventData")]
        public dynamic EventData { get; internal set; }

        [JsonProperty("eventStatus")]
        public int? EventStatus { get; internal set; }

        [JsonProperty("eventStatusText")]
        public string EventStatusText { get; internal set; }

        [Preserve(Conditional = true)]
        public InaraEvent(string name, object data)
        {
            EventName = name;
            EventTimestamp = DateTime.Now;
            EventData = data;
        }
    }
}

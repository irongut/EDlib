using EDlib.Platform;
using Newtonsoft.Json;
using System;

namespace EDlib.INARA
{
    /// <summary>
    ///   <para>Represents the data block in an INARA API request and response.</para>
    ///   <para>See INARA documentation for <a href="https://inara.cz/inara-api-docs/#eventdataeventsprops">Events properties</a>.</para>
    /// </summary>
    public class InaraEvent
    {
        /// <summary>The name of the event / method.</summary>
        [JsonProperty(PropertyName = "eventName")]
        public string EventName { get; }

        /// <summary>Date and time in ISO 8601 format (like: 2017-05-02T17:30:49Z)</summary>
        [JsonProperty(PropertyName = "eventTimestamp")]
        public DateTime EventTimestamp { get; }

        /// <summary>Required properties for a request / data returned by the API. Individual event properties are detailed in the event's documentation.</summary>
        [JsonProperty(PropertyName = "eventData")]
        public dynamic EventData { get; internal set; }

        /// <summary>Event status code, see <a href="https://inara.cz/inara-api-docs/#eventdatacodes">INARA eventStatus codes</a>.</summary>
        [JsonProperty("eventStatus")]
        public int? EventStatus { get; internal set; }

        /// <summary>Explanation of the status code, only returned on errors and warnings.</summary>
        [JsonProperty("eventStatusText")]
        public string EventStatusText { get; internal set; }

        /// <summary>Initializes a new instance of the <see cref="InaraEvent" /> class.</summary>
        /// <param name="name">The name of the event / method.</param>
        /// <param name="data">The event data.</param>
        [Preserve(Conditional = true)]
        public InaraEvent(string name, object data)
        {
            EventName = name;
            EventTimestamp = DateTime.Now;
            EventData = data;
        }
    }
}

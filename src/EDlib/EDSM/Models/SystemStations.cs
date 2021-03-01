using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EDlib.EDSM
{
    /// <summary>Lists the stations in a system returned by EDSM System API methods.</summary>
    public class SystemStations
    {
        /// <summary>The EDSM internal ID of the system.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The EDSM internal ID of the system. (64 bit)</summary>
        [JsonProperty("id64")]
        public long Id64 { get; set; }

        /// <summary>The name of the system.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The EDSM URL for the system.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>List of <see cref="Station" /> in the system.</summary>
        [JsonProperty("stations")]
        public List<Station> Stations { get; set; }

        /// <summary>The date and time when the information was requested.</summary>
        public DateTime LastUpdated { get; }

        /// <summary>Initializes a new instance of the <see cref="SystemStations" /> class.</summary>
        /// <param name="id">The EDSM system ID.</param>
        /// <param name="id64">The EDSM 64 bit system ID.</param>
        /// <param name="name">The system name.</param>
        /// <param name="url">The EDSM URL for the system.</param>
        /// <param name="stations">Array of <see cref="Station" /> in the system.</param>
        [Preserve(Conditional = true)]
        public SystemStations(long id, long id64, string name, string url, List<Station> stations)
        {
            Id = id;
            Id64 = id64;
            Name = name;
            Url = url;
            Stations = stations;
            LastUpdated = DateTime.Now;
        }
    }
}

using EDlib.Platform;
using Newtonsoft.Json;
using System;

namespace EDlib.EDSM
{
    /// <summary>Elite: Dangerous server status.</summary>
    public class EliteStatus
    {
        /// <summary>The status code returned by the Elite: Dangerous server.</summary>
        [JsonProperty(PropertyName = "status")]
        public int Status { get; internal set; }

        /// <summary>A Bootstrap style status string - success / warning / danger.</summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; internal set; }

        /// <summary>The message returned by the Elite: Dangerous server.</summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; internal set; }

        /// <summary>When EDSM last checked the status from the Elite: Dangerous server.</summary>
        [JsonProperty(PropertyName = "lastUpdate")]
        public DateTime LastUpdated { get; internal set; }

        /// <summary>Initializes a new instance of the <see cref="EliteStatus" /> class with unknown status.</summary>
        [Preserve(Conditional = true)]
        public EliteStatus()
        {
            Status = -1;
            Type = "unknown";
            Message = "Status Unknown";
            LastUpdated = DateTime.Now;
        }

        /// <summary>Initializes a new instance of the <see cref="EliteStatus" /> class.</summary>
        /// <param name="status">The server status code.</param>
        /// <param name="type">The status string.</param>
        /// <param name="message">The server status message.</param>
        /// <param name="lastupdate">When the server status was last checked.</param>
        [Preserve(Conditional = true)]
        public EliteStatus(int status, string type, string message, DateTime lastupdate)
        {
            Status = status;
            Type = type;
            Message = message;
            LastUpdated = lastupdate;
        }

        /// <summary>Returns the server status message and last updated as a string.</summary>
        /// <returns>A <see cref="System.String" /> that represents the server status.</returns>
        public override string ToString()
        {
            return $"Elite Server Status: {Message} ({LastUpdated})";
        }
    }
}

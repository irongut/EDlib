using Newtonsoft.Json;
using System;

namespace EDlib.EDSM
{
    public class EliteStatus
    {
        [JsonProperty(PropertyName = "status")]
        public int Status { get; internal set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; internal set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; internal set; }

        [JsonProperty(PropertyName = "lastUpdate")]
        public DateTime LastUpdated { get; internal set; }

        public EliteStatus()
        {
            Status = -1;
            Type = "unknown";
            Message = "Status Unknown";
            LastUpdated = DateTime.Now;
        }

        public EliteStatus(int status, string type, string message, DateTime lastupdate)
        {
            Status = status;
            Type = type;
            Message = message;
            LastUpdated = lastupdate;
        }

        public override string ToString()
        {
            return $"Elite Server Status: {Message} ({LastUpdated})";
        }
    }
}

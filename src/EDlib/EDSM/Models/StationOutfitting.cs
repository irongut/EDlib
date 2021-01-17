using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EDlib.EDSM
{
    /// <summary>Represents an outfitting service returned by EDSM System API methods.</summary>
    public class StationOutfitting
    {
        /// <summary>The EDSM internal ID of the outfitting service.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The EDSM internal ID of the outfitting service (64 bit).</summary>
        [JsonProperty("id64")]
        public long Id64 { get; set; }

        /// <summary>The name of the system.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The outfitting service ID (use this ID for EDSM queries).</summary>
        [JsonProperty("marketId")]
        public long MarketId { get; set; }

        /// <summary>The EDSM internal ID of the station.</summary>
        [JsonProperty("sId")]
        public long SId { get; set; }

        /// <summary>The name of the station.</summary>
        [JsonProperty("sName")]
        public string SName { get; set; }

        /// <summary>The EDSM URL for the outfitting service.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>List of <see cref="ShipModule" /> available from this outfitting service.</summary>
        [JsonProperty("outfitting")]
        public List<ShipModule> Outfitting { get; set; }

        /// <summary>The date and time when the information was requested.</summary>
        public DateTime LastUpdated { get; }

        /// <summary>Initializes a new instance of the <see cref="StationOutfitting" /> class.</summary>
        /// <param name="id">The EDSM outfitting service ID.</param>
        /// <param name="id64">The EDSM 64 bit outfitting service ID.</param>
        /// <param name="name">The system name.</param>
        /// <param name="marketId">The outfitting service ID.</param>
        /// <param name="sId">The station ID.</param>
        /// <param name="sName">The station name.</param>
        /// <param name="url">The EDSM URL for the outfitting service.</param>
        /// <param name="outfitting">List of <see cref="Outfitting" /> available from this outfitting service.</param>
        public StationOutfitting(long id, long id64, string name, long marketId, long sId, string sName, string url, List<ShipModule> outfitting)
        {
            Id = id;
            Id64 = id64;
            Name = name;
            MarketId = marketId;
            SId = sId;
            SName = sName;
            Url = url;
            Outfitting = outfitting;
            LastUpdated = DateTime.Now;
        }
    }

    /// <summary>Represents a ship module returned by EDSM System API methods.</summary>
    public class ShipModule
    {
        /// <summary>The module ID.</summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>The name of the module.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

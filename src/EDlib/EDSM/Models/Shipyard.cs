using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EDlib.EDSM
{
    /// <summary>
    ///   <para>Represents a station shipyard returned by EDSM System API methods.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-system-v1">System v1</a>.</para>
    /// </summary>
    public class Shipyard
    {
        /// <summary>The EDSM internal ID of the shipyard.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The EDSM internal ID of the shipyard (64 bit).</summary>
        [JsonProperty("id64")]
        public long Id64 { get; set; }

        /// <summary>The name of the system.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The shipyard ID (use this ID for EDSM queries).</summary>
        [JsonProperty("marketId")]
        public long MarketId { get; set; }

        /// <summary>The EDSM internal ID of the station.</summary>
        [JsonProperty("sId")]
        public long SId { get; set; }

        /// <summary>The name of the station.</summary>
        [JsonProperty("sName")]
        public string SName { get; set; }

        /// <summary>The EDSM URL for the shipyard.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>List of <see cref="Ship" /> available from this shipyard.</summary>
        [JsonProperty("ships")]
        public List<Ship> Ships { get; set; }

        /// <summary>The date and time when the information was requested.</summary>
        public DateTime LastUpdated { get; }

        /// <summary>Initializes a new instance of the <see cref="Market" /> class.</summary>
        /// <param name="id">The EDSM shipyard ID.</param>
        /// <param name="id64">The EDSM 64 bit shipyard ID.</param>
        /// <param name="name">The system name.</param>
        /// <param name="marketId">The shipyard ID.</param>
        /// <param name="sId">The station ID.</param>
        /// <param name="sName">The station name.</param>
        /// <param name="url">The EDSM URL for the shipyard.</param>
        /// <param name="ships">List of <see cref="Ship" /> available from this shipyard.</param>
        [Preserve(Conditional = true)]
        public Shipyard(long id, long id64, string name, long marketId, long sId, string sName, string url, List<Ship> ships)
        {
            Id = id;
            Id64 = id64;
            Name = name;
            MarketId = marketId;
            SId = sId;
            SName = sName;
            Url = url;
            Ships = ships;
            LastUpdated = DateTime.Now;
        }
    }

    /// <summary>
    ///   <para>Represents a ship returned by EDSM System API methods.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-system-v1">System v1</a>.</para>
    /// </summary>
    public class Ship
    {
        /// <summary>The ship ID.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The name of the ship.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

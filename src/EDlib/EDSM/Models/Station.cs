using EDlib.Platform;
using Newtonsoft.Json;
using System;

namespace EDlib.EDSM
{
    /// <summary>
    ///   <para>Represents a station returned by EDSM System API methods.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-system-v1">System v1</a>.</para>
    /// </summary>
    public class Station
    {
        /// <summary>The EDSM internal ID of the station.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The market ID (use this ID for EDSM queries).</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("marketId")]
        public long MarketId { get; set; }

        /// <summary>The type of station.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>The name of station.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Distance from the primary star to the station in light seconds.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("distanceToArrival")]
        public double DistanceToArrival { get; set; }

        /// <summary>The superpower allegiance of the faction.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("allegiance")]
        public string Allegiance { get; set; }

        /// <summary>The government type of the faction.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("government")]
        public string Government { get; set; }

        /// <summary>The primary economy type of the station.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("economy")]
        public string Economy { get; set; }

        /// <summary>The secondary economy type of the station.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("secondEconomy")]
        public string SecondEconomy { get; set; }

        /// <summary><c>true</c> if the station has a market.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("haveMarket")]
        public bool HaveMarket { get; set; }

        /// <summary><c>true</c> if the station has a shipyard.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("haveShipyard")]
        public bool HaveShipyard { get; set; }

        /// <summary><c>true</c> if the station has an outfitting service.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("haveOutfitting")]
        public bool HaveOutfitting { get; set; }

        /// <summary>Array of other services available in the station.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("otherServices")]
        public string[] OtherServices { get; set; }

        /// <summary>The current faction that controls the station.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("controllingFaction")]
        public ControllingFaction ControllingFaction { get; set; }

        /// <summary>When the station details were last updated on ESDM.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("updateTime")]
        public UpdateTime UpdateTime { get; set; }

        /// <summary>Location of a planetary outpost.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public Body Body { get; set; }
    }

    /// <summary>
    ///   <para>Represents when the station details were last updated on ESDM.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-system-v1">System v1</a>.</para>
    /// </summary>
    public class UpdateTime
    {
        /// <summary>The date and time when the station data was last updated on EDSM.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("information")]
        public DateTime Information { get; set; }

        /// <summary>The date and time when the market data was last updated on EDSM.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("market")]
        public DateTime? Market { get; set; }

        /// <summary>The date and time when the shipyard data was last updated on EDSM.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("shipyard")]
        public DateTime? Shipyard { get; set; }

        /// <summary>The date and time when the outfitting data was last updated on EDSM.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("outfitting")]
        public DateTime? Outfitting { get; set; }
    }

    /// <summary>
    ///   <para>Represents the location of a planetary outpost returned by EDSM System API methods.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-system-v1">System v1</a>.</para>
    /// </summary>
    public class Body
    {
        /// <summary>The ID of the body.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The name of the body.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The latitude of the planetary outpost if known.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public double? Latitude { get; set; }

        /// <summary>The longitude of the planetary outpost if known.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public double? Longitude { get; set; }
    }
}

using Newtonsoft.Json;
using System;

namespace EDlib.EDSM
{
    /// <summary>Represents a station returned by EDSM System API methods.</summary>
    public class Station
    {
        /// <summary>The EDSM internal ID of the station.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The market ID (use this ID for EDSM queries).</summary>
        [JsonProperty("marketId")]
        public long MarketId { get; set; }

        /// <summary>The type of station.</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>The name of station.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Distance from the primary star to the station in light seconds.</summary>
        [JsonProperty("distanceToArrival")]
        public double DistanceToArrival { get; set; }

        /// <summary>The superpower allegiance of the faction.</summary>
        [JsonProperty("allegiance")]
        public string Allegiance { get; set; }

        /// <summary>The government type of the faction.</summary>
        [JsonProperty("government")]
        public string Government { get; set; }

        /// <summary>The primary economy type of the station.</summary>
        [JsonProperty("economy")]
        public string Economy { get; set; }

        /// <summary>The secondary economy type of the station.</summary>
        [JsonProperty("secondEconomy")]
        public string SecondEconomy { get; set; }

        /// <summary><c>true</c> if the station has a market.</summary>
        [JsonProperty("haveMarket")]
        public bool HaveMarket { get; set; }

        /// <summary><c>true</c> if the station has a shipyard.</summary>
        [JsonProperty("haveShipyard")]
        public bool HaveShipyard { get; set; }

        /// <summary><c>true</c> if the station has an outfitting service.</summary>
        [JsonProperty("haveOutfitting")]
        public bool HaveOutfitting { get; set; }

        /// <summary>Array of other services available in the station.</summary>
        [JsonProperty("otherServices")]
        public string[] OtherServices { get; set; }

        /// <summary>The current faction that controls the station.</summary>
        [JsonProperty("controllingFaction")]
        public ControllingFaction ControllingFaction { get; set; }

        /// <summary>When the station details were last updated on ESDM.</summary>
        [JsonProperty("updateTime")]
        public UpdateTime UpdateTime { get; set; }

        /// <summary>Location of a planetary outpost.</summary>
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public Body Body { get; set; }
    }

    /// <summary>Represents when the station details were last updated on ESDM.</summary>
    public class UpdateTime
    {
        /// <summary>The date and time when the station data was last updated on EDSM.</summary>
        [JsonProperty("information")]
        public DateTime Information { get; set; }

        /// <summary>The date and time when the market data was last updated on EDSM.</summary>
        [JsonProperty("market")]
        public DateTime? Market { get; set; }

        /// <summary>The date and time when the shipyard data was last updated on EDSM.</summary>
        [JsonProperty("shipyard")]
        public DateTime? Shipyard { get; set; }

        /// <summary>The date and time when the outfitting data was last updated on EDSM.</summary>
        [JsonProperty("outfitting")]
        public DateTime? Outfitting { get; set; }
    }

    /// <summary>Represents the location of a planetary outpost returned by EDSM System API methods.</summary>
    public class Body
    {
        /// <summary>The ID of the body.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The name of the body.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The latitude of the planetary outpost if known.</summary>
        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public double? Latitude { get; set; }

        /// <summary>The longitude of the planetary outpost if known.</summary>
        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public double? Longitude { get; set; }
    }
}

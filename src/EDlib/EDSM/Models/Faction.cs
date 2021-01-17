using Newtonsoft.Json;

namespace EDlib.EDSM
{
    /// <summary>Represents a BGS faction returned by EDSM System API methods.</summary>
    public class Faction
    {
        /// <summary>The ID of the faction.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The name of the faction.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The superpower allegiance of the faction.</summary>
        [JsonProperty("allegiance")]
        public string Allegiance { get; set; }

        /// <summary>The government type of the faction.</summary>
        [JsonProperty("government")]
        public string Government { get; set; }

        /// <summary>The faction's influence level.</summary>
        [JsonProperty("influence")]
        public double Influence { get; set; }

        /// <summary>The faction's current state.</summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>The faction's active states.</summary>
        [JsonProperty("activeStates")]
        public FactionState[] ActiveStates { get; set; }

        /// <summary>The faction's recovering states.</summary>
        [JsonProperty("recoveringStates")]
        public FactionState[] RecoveringStates { get; set; }

        /// <summary>The faction's pending states.</summary>
        [JsonProperty("pendingStates")]
        public FactionState[] PendingStates { get; set; }

        /// <summary>The faction's current happiness.</summary>
        [JsonProperty("happiness")]
        public string Happiness { get; set; }

        /// <summary><c>true</c> if the faction is a player faction.</summary>
        [JsonProperty("isPlayer")]
        public bool IsPlayer { get; set; }

        /// <summary>The date and time when the information was requested.</summary>
        [JsonProperty("lastUpdate")]
        public long LastUpdate { get; set; }
    }

    /// <summary>Represents a faction's BGS state returned by EDSM System API methods.</summary>
    public class FactionState
    {
        /// <summary>The BGS state.</summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>The BGS state trend.</summary>
        [JsonProperty("trend")]
        public long? Trend { get; set; }
    }
}

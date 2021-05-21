using EDlib.Platform;
using Newtonsoft.Json;

namespace EDlib.EDSM
{
    /// <summary>
    ///   <para>Represents a BGS faction returned by EDSM System API methods.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-system-v1">System v1</a>.</para>
    /// </summary>
    public class Faction
    {
        /// <summary>The ID of the faction.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The name of the faction.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The superpower allegiance of the faction.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("allegiance")]
        public string Allegiance { get; set; }

        /// <summary>The government type of the faction.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("government")]
        public string Government { get; set; }

        /// <summary>The faction's influence level.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("influence")]
        public double Influence { get; set; }

        /// <summary>The faction's current state.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>The faction's active states.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("activeStates")]
        public FactionState[] ActiveStates { get; set; }

        /// <summary>The faction's recovering states.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("recoveringStates")]
        public FactionState[] RecoveringStates { get; set; }

        /// <summary>The faction's pending states.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("pendingStates")]
        public FactionState[] PendingStates { get; set; }

        /// <summary>The faction's current happiness.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("happiness")]
        public string Happiness { get; set; }

        /// <summary><c>true</c> if the faction is a player faction.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("isPlayer")]
        public bool IsPlayer { get; set; }

        /// <summary>The date and time when the information was requested.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("lastUpdate")]
        public long LastUpdate { get; set; }
    }

    /// <summary>
    ///   <para>Represents a faction's BGS state returned by EDSM System API methods.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-system-v1">System v1</a>.</para>
    /// </summary>
    public class FactionState
    {
        /// <summary>The BGS state.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>The BGS state trend.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("trend")]
        public long? Trend { get; set; }
    }
}

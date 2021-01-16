using Newtonsoft.Json;

namespace EDlib.EDSM
{
    /// <summary>Represents the BGS faction that controls a system or station returned by EDSM API methods.</summary>
    public class ControllingFaction
    {
        /// <summary>
        ///   <para>The ID of the faction.</para>
        ///   <para>Engineer factions do not have an Id so this property is nullable.</para>
        /// </summary>
        [JsonProperty("id")]
        public long? Id { get; set; }

        /// <summary>The name of the faction.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The superpower allegiance of the faction when returned by the EDSM method.</summary>
        [JsonProperty("allegiance")]
        public string Allegiance { get; set; }

        /// <summary>The government type of the faction when returned by the EDSM method.</summary>
        [JsonProperty("government")]
        public string Government { get; set; }
    }
}

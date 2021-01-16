using Newtonsoft.Json;
using System;

namespace EDlib.EDSM
{
    /// <summary>Represents a solar system returned by EDSM Systems API methods.</summary>
    public class SolarSystem
    {
        /// <summary>The name of the system.</summary>
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>Distance in light years from another system specified in the API request.</summary>
        [JsonProperty("distance")]
        public double Distance { get; set; }

        /// <summary>The EDSM internal ID of the system.</summary>
        [JsonProperty("id")]
        public long Id { get; }

        /// <summary>The EDSM internal ID of the system (64 bit).</summary>
        [JsonProperty("id64")]
        public long Id64 { get; }

        /// <summary>Coordinates of the system.</summary>
        [JsonProperty("coords")]
        public Coords Coords { get; }

        /// <summary>Whether EDSM has locked the coordinates of the system.</summary>
        [JsonProperty("coordsLocked")]
        public bool CoordsLocked { get; }

        /// <summary>Whether the system requires a permit.</summary>
        [JsonProperty("requirePermit")]
        public bool RequiresPermit { get; }

        /// <summary>The name of the system permit if required.</summary>
        [JsonProperty("permitName")]
        public string PermitName { get; set; }

        /// <summary>
        ///   <para>A summary of information about the system.</para>
        ///   <para>Includes Allegiance, Government, Faction, Population, Security and Economic information.</para>
        /// </summary>
        [JsonProperty("information")]
        public SystemInfo Information { get; }

        /// <summary>
        ///   <para>Information concerning the primary star of the system.</para>
        ///   <para>Includes Type, Name and if the star can be scooped for fuel.</para>
        /// </summary>
        [JsonProperty("primaryStar")]
        public PrimaryStar PrimaryStar { get; }

        /// <summary>The date and time when the information was requested.</summary>
        public DateTime LastUpdated { get; }

        /// <summary>Initializes a new instance of the <see cref="SolarSystem" /> class.</summary>
        /// <param name="name">The system name.</param>
        /// <param name="distance">Distance from a specified system.</param>
        /// <param name="id">The EDSM system ID.</param>
        /// <param name="id64">The EDSM 64 bit system ID.</param>
        /// <param name="coords">The system coords.</param>
        /// <param name="coordsLocked">if set to <c>true</c> [coords locked].</param>
        /// <param name="requirePermit">If the system requires a permit.</param>
        /// <param name="permitName">Name of the system permit.</param>
        /// <param name="information">The system information.</param>
        /// <param name="primaryStar">The primary star information.</param>
        public SolarSystem(string name, double distance, long id, long id64, Coords coords, bool coordsLocked, bool requirePermit, string permitName, SystemInfo information, PrimaryStar primaryStar)
        {
            Name = name;
            Distance = distance;
            Id = id;
            Id64 = id64;
            Coords = coords;
            CoordsLocked = coordsLocked;
            RequiresPermit = requirePermit;
            PermitName = permitName;
            Information = information;
            PrimaryStar = primaryStar;
            LastUpdated = DateTime.Now;
        }
    }

    /// <summary>Galactic coordinates for the system.</summary>
    public class Coords
    {
        /// <summary>X coordinate.</summary>
        [JsonProperty("x")]
        public double X { get; set; }

        /// <summary>Y coordinate.</summary>
        [JsonProperty("y")]
        public double Y { get; set; }

        /// <summary>Z coordinate.</summary>
        [JsonProperty("z")]
        public double Z { get; set; }
    }

    /// <summary>A summary of information about a system.</summary>
    public class SystemInfo
    {
        /// <summary>The current superpower allegiance of the system.</summary>
        [JsonProperty("allegiance")]
        public string Allegiance { get; set; }

        /// <summary>The current government type of the system.</summary>
        [JsonProperty("government")]
        public string Government { get; set; }

        /// <summary>The current faction who controls the system.</summary>
        [JsonProperty("faction")]
        public string Faction { get; set; }

        /// <summary>The current active state of the controlling faction.</summary>
        [JsonProperty("factionState")]
        public string FactionState { get; set; }

        /// <summary>The population of the system.</summary>
        [JsonProperty("population")]
        public long Population { get; set; }

        /// <summary>The current security level of the system.</summary>
        [JsonProperty("security")]
        public string Security { get; set; }

        /// <summary>The primary economy type of the system.</summary>
        [JsonProperty("economy")]
        public string Economy { get; set; }

        /// <summary>The secondary economy type of the system.</summary>
        [JsonProperty("secondEconomy")]
        public string SecondEconomy { get; set; }

        /// <summary>The mining reserves present in system.</summary>
        [JsonProperty("reserve")]
        public string Reserve { get; set; }
    }

    /// <summary>Details of the primary star in a system.</summary>
    public class PrimaryStar
    {
        /// <summary>The type of the primary star. See the EDSM Celestial Bodies FAQ for more info.</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>The name of the primary star.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Whether the primary star can be scooped for fuel.</summary>
        [JsonProperty("isScoopable")]
        public bool IsScoopable { get; set; }
    }
}

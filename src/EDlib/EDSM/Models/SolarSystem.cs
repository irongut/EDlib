using Newtonsoft.Json;
using System;

namespace EDlib.EDSM
{
    public class SolarSystem
    {
        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("id")]
        public long Id { get; }

        [JsonProperty("id64")]
        public long Id64 { get; }

        [JsonProperty("coords")]
        public Coords Coords { get; }

        [JsonProperty("coordsLocked")]
        public bool CoordsLocked { get; }

        [JsonProperty("requirePermit")]
        public bool RequiresPermit { get; }

        [JsonProperty("permitName")]
        public string PermitName { get; set; }

        [JsonProperty("information")]
        public SystemInfo Information { get; }

        [JsonProperty("primaryStar")]
        public PrimaryStar PrimaryStar { get; }

        public DateTime LastUpdated { get; }

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

    public class Coords
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("z")]
        public double Z { get; set; }
    }

    public class SystemInfo
    {
        [JsonProperty("allegiance")]
        public string Allegiance { get; set; }

        [JsonProperty("government")]
        public string Government { get; set; }

        [JsonProperty("faction")]
        public string Faction { get; set; }

        [JsonProperty("factionState")]
        public string FactionState { get; set; }

        [JsonProperty("population")]
        public long Population { get; set; }

        [JsonProperty("security")]
        public string Security { get; set; }

        [JsonProperty("economy")]
        public string Economy { get; set; }

        [JsonProperty("secondEconomy")]
        public string SecondEconomy { get; set; }

        [JsonProperty("reserve")]
        public string Reserve { get; set; }
    }

    public class PrimaryStar
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isScoopable")]
        public bool IsScoopable { get; set; }
    }
}

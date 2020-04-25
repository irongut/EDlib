using System;

namespace EDlib.Standings
{
    public enum StandingChange
    {
        up,
        down,
        none
    }

    public class PowerStanding
    {
        public int Id { get; }

        public string Name { get; }

        public int Position { get; }

        public StandingChange Change { get; }

        public bool Turmoil { get; }

        public string Allegiance { get; }

        public string ShortName { get; }

        public string Cycle { get; set; }   // made set public until StandingsService included in EDlib or changed

        public DateTime LastUpdated { get; }

        public string ChangeString
        {
            get
            {
                switch (Change)
                {
                    case StandingChange.up:
                        return "Up";
                    case StandingChange.down:
                        return "Down";
                    case StandingChange.none:
                        return "No Change";
                    default:
                        return String.Empty;
                }
            }
        }

        public PowerStanding(int id, string name, int position, StandingChange change, bool turmoil, string allegiance, string shortname, DateTime updated)
        {
            Id = id;
            Name = name;
            Position = position;
            Change = change;
            Turmoil = turmoil;
            Allegiance = allegiance;
            ShortName = shortname;
            LastUpdated = updated;
        }

        public PowerStanding(int position, string standing, int cycle, DateTime updated)
        {
            if (standing.IndexOf("Aisling Duval", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Id = 1;
                Name = "Aisling Duval";
                Allegiance = "Empire";
                ShortName = "Aisling";
            }
            else if (standing.IndexOf("Archon Delaine", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Id = 2;
                Name = "Archon Delaine";
                Allegiance = "Independent";
                ShortName = "Delaine";
            }
            else if (standing.IndexOf("Arissa Lavigny-Duval", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Id = 3;
                Name = "Arissa Lavigny-Duval";
                Allegiance = "Empire";
                ShortName = "ALD";
            }
            else if (standing.IndexOf("Denton Patreus", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Id = 4;
                Name = "Denton Patreus";
                Allegiance = "Empire";
                ShortName = "Patreus";
            }
            else if (standing.IndexOf("Edmund Mahon", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Id = 5;
                Name = "Edmund Mahon";
                Allegiance = "Alliance";
                ShortName = "Mahon";
            }
            else if (standing.IndexOf("Felicia Winters", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Id = 6;
                Name = "Felicia Winters";
                Allegiance = "Federation";
                ShortName = "Winters";
            }
            else if (standing.IndexOf("Li Yong-Rui", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Id = 7;
                Name = "Li Yong-Rui";
                Allegiance = "Independent";
                ShortName = "LYR";
            }
            else if (standing.IndexOf("Pranav Antal", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Id = 8;
                Name = "Pranav Antal";
                Allegiance = "Independent";
                ShortName = "Antal";
            }
            else if (standing.IndexOf("Yuri Grom", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Id = 9;
                Name = "Yuri Grom";
                Allegiance = "Independent";
                ShortName = "Grom";
            }
            else if (standing.IndexOf("Zachary Hudson", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Id = 10;
                Name = "Zachary Hudson";
                Allegiance = "Federation";
                ShortName = "Hudson";
            }
            else if (standing.IndexOf("Zemina Torval", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Id = 11;
                Name = "Zemina Torval";
                Allegiance = "Empire";
                ShortName = "Torval";
            }
            else
            {
                Id = 0;
                Name = "Standings Error";
                Allegiance = "Error";
                ShortName = "Error";
            }

            if (standing.IndexOf("(+", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Change = StandingChange.up;
            }
            else if (standing.IndexOf("(-", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Change = StandingChange.down;
            }
            else
            {
                Change = StandingChange.none;
            }

            Position = position;
            Turmoil = standing.IndexOf("Turmoil", StringComparison.OrdinalIgnoreCase) >= 0;
            Cycle = string.Format("Cycle {0}", cycle);
            LastUpdated = updated;
        }

        public override string ToString()
        {
            return String.Format("{0:00} {1} - {2} ({3})", Position, ChangeString, Name, Allegiance);
        }
    }
}

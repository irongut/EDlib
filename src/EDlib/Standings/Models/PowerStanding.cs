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
        public string Cycle { get; internal set; }
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

        public override string ToString()
        {
            return String.Format("{0:00} {1} - {2} ({3})", Position, ChangeString, Name, Allegiance);
        }
    }
}

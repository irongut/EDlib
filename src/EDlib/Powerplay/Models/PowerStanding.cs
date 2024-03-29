﻿using EDlib.Platform;
using System;

namespace EDlib.Powerplay
{
    /// <summary>The Power's change in standings since the previous cycle.</summary>
    public enum StandingChange
    {
        /// <summary>The Power's standing has improved.</summary>
        up,
        /// <summary>The Power's standing has decayed.</summary>
        down,
        /// <summary>The Power's standing has not changed.</summary>
        none
    }

    /// <summary>Represents the Galactic Standings data for a Power.</summary>
    public class PowerStanding
    {
        /// <summary>The unique identifier for the Power.</summary>
        public int Id { get; set; }

        /// <summary>The Power's name.</summary>
        public string Name { get; set; }

        /// <summary>The Power's position in the Galactic Standings.</summary>
        public int Position { get; set; }

        /// <summary>The Power's change in the Galactic Standings since the previous cycle.</summary>
        public StandingChange Change { get; set; }

        /// <summary>Indicates whether this Power is turmoil.</summary>
        public bool Turmoil { get; set; }

        /// <summary>The Power's allegiance - Alliance, Empire, Federation or Independent.</summary>
        public string Allegiance { get; set; }

        /// <summary>The short name for the Power.</summary>
        public string ShortName { get; set; }

        /// <summary>The Powerplay cycle.</summary>
        public string Cycle { get; set; }

        /// <summary>The date and time the Power's standing was last updated.</summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>The Power's change in the Galactic Standings since the previous cycle as a string.</summary>
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
                        return "None";
                    default:
                        return String.Empty;
                }
            }
        }

        /// <summary>
        ///   The number of cycles since the Power was last in Turmoil.<br/>
        ///   A value of -1 indicates the number of cycles since Turmoil is unknown.
        /// </summary>
        public int CyclesSinceTurmoil { get; set; }

        /// <summary>Initializes a new instance of the <see cref="PowerStanding" /> class with no data.</summary>
        [Preserve(Conditional = true)]
        public PowerStanding()
        {
            CyclesSinceTurmoil = -1;
        }

        /// <summary>Initializes a new instance of the <see cref="PowerStanding" /> class with the given data.</summary>
        /// <param name="id">The Power's unique identifier.</param>
        /// <param name="name">The Power's name.</param>
        /// <param name="position">The Power's position in the Galactic Standings.</param>
        /// <param name="change">The Power's change in the Galactic Standings.</param>
        /// <param name="turmoil">Whether the Power is in Turmoil.</param>
        /// <param name="allegiance">The Power's allegiance.</param>
        /// <param name="shortname">The Power's short name.</param>
        /// <param name="updated">When the Power's standing was last updated.</param>
        [Preserve(Conditional = true)]
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
            CyclesSinceTurmoil = -1;
        }

        /// <summary>Initializes a new instance of the <see cref="PowerStanding" /> class with the given data.</summary>
        /// <param name="id">The Power's unique identifier.</param>
        /// <param name="name">The Power's name.</param>
        /// <param name="position">The Power's position in the Galactic Standings.</param>
        /// <param name="change">The Power's change in the Galactic Standings.</param>
        /// <param name="turmoil">Whether the Power is in Turmoil.</param>
        /// <param name="allegiance">The Power's allegiance.</param>
        /// <param name="shortname">The Power's short name.</param>
        /// <param name="updated">When the Power's standing was last updated.</param>
        /// <param name="cyclesSinceTurmoil">The number of cycles since the Power was last in Turmoil.</param>
        [Preserve(Conditional = true)]
        public PowerStanding(int id, string name, int position, StandingChange change, bool turmoil, string allegiance, string shortname, DateTime updated, int cyclesSinceTurmoil)
        {
            Id = id;
            Name = name;
            Position = position;
            Change = change;
            Turmoil = turmoil;
            Allegiance = allegiance;
            ShortName = shortname;
            LastUpdated = updated;
            CyclesSinceTurmoil = cyclesSinceTurmoil;
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="PowerStanding" /> class with the given data.</para>
        ///   <para>Used by the Galactic Standings API back-end.</para>
        /// </summary>
        /// <param name="position">The Power's position in the Galactic Standings.</param>
        /// <param name="standing">The Power's name, standings change and turmoil status.</param>
        /// <param name="cycle">The Powerplay cycle.</param>
        /// <param name="updated">When the Power's standing was last updated.</param>
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
            Cycle = $"Cycle {cycle}";
            LastUpdated = updated;
            CyclesSinceTurmoil = -1;
        }

        /// <summary>Returns the Power's standing as a string: position + change - Power name (Power allegiance)</summary>
        /// <returns>A <see cref="System.String" /> that represents the Power's standing.</returns>
        public override string ToString()
        {
            return Turmoil
                ? $"{Position:00} {ChangeString} - {Name} ({Allegiance}) TURMOIL"
                : $"{Position:00} {ChangeString} - {Name} ({Allegiance})";
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDlib.Powerplay
{
    /// <summary>Represents the Powerplay Galactic Standings.</summary>
    public class GalacticStandings
    {
        /// <summary>Gets the Powerplay cycle number.</summary>
        /// <value>The Powerplay cycle.</value>
        public int Cycle { get; }

        /// <summary>Gets the date and time the Galactic Standings were last updated.</summary>
        /// <value>When the Standings were last updated.</value>
        public DateTime LastUpdated { get; }

        /// <summary>Gets the Galactic Standings.</summary>
        /// <value>A list of PowerStanding objects.</value>
        public List<PowerStanding> Standings { get; }

        /// <summary>Initializes a new instance of the <see cref="GalacticStandings" /> class.</summary>
        /// <param name="cycle">The Powerplay cycle.</param>
        /// <param name="lastUpdated">When the Standings were last updated.</param>
        public GalacticStandings(int cycle, DateTime lastUpdated)
        {
            Cycle = cycle;
            LastUpdated = lastUpdated;
            Standings = new List<PowerStanding>();
        }

        /// <summary>Returns the Powerplay Galactic Standings as a multi-line string including the cycle and when the Standings were last updated.</summary>
        /// <returns>A <see cref="System.String" /> that represents the Galactic Standings.</returns>
        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            text.AppendFormat("Cycle {0} Galactic Standings{1}", Cycle, Environment.NewLine);
            foreach (PowerStanding power in Standings.OrderBy(x => x.Position))
            {
                text.AppendLine(power.ToString());
            }
            text.AppendFormat("Last Updated: {0}{1}", LastUpdated, Environment.NewLine);
            return text.ToString();
        }

        /// <summary>Returns the Powerplay Galactic Standings as a comma separated string including the cycle and when the Standings were last updated.</summary>
        /// <returns>A <see cref="System.String" /> in CSV format that represents the Galactic Standings.</returns>
        public string ToCSV()
        {
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("Id,Name,Position,Change,Turmoil,Allegiance,Short Name");
            foreach (PowerStanding power in Standings.OrderBy(x => x.Id))
            {
                csv.AppendFormat("{0},{1},{2},{3},{4},{5},{6}{7}",
                    power.Id,
                    power.Name,
                    power.Position,
                    power.ChangeString,
                    power.Turmoil,
                    power.Allegiance,
                    power.ShortName,
                    Environment.NewLine);
            }
            csv.AppendFormat(",Cycle {0},,,,,{1}", Cycle, Environment.NewLine);
            return csv.ToString();
        }

        /// <summary>Returns the Powerplay Galactic Standings as a JSON string including the cycle and when the Standings were last updated.</summary>
        /// <returns>A <see cref="System.String" /> in JSON format that represents the Galactic Standings.</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

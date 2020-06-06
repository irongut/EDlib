using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDlib.Standings
{
    public class GalacticStandings
    {
        public int Cycle { get; }

        public DateTime LastUpdated { get; }

        public List<PowerStanding> Standings { get; }

        public GalacticStandings(int cycle, DateTime lastUpdated)
        {
            Cycle = cycle;
            LastUpdated = lastUpdated;
            Standings = new List<PowerStanding>();
        }

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

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

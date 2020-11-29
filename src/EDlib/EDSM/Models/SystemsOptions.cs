using System;

namespace EDlib.EDSM
{
    public class SystemsOptions : IEquatable<SystemsOptions>
    {
        public bool ShowId { get; set; }

        public bool ShowCoordinates { get; set; }

        public bool ShowPermit { get; set; }

        public bool ShowInformation { get; set; }

        public bool ShowPrimaryStar { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as SystemsOptions);
        }

        public bool Equals(SystemsOptions other)
        {
            if (other == null)
            {
                return false;
            }

            return this.ShowId == other.ShowId
                && this.ShowCoordinates == other.ShowCoordinates
                && this.ShowPermit == other.ShowPermit
                && this.ShowInformation == other.ShowInformation
                && this.ShowPrimaryStar == other.ShowPrimaryStar;
        }

        public override int GetHashCode()
        {
            int hashCode = -1040031009;
            hashCode = (hashCode * -1521134295) + ShowId.GetHashCode();
            hashCode = (hashCode * -1521134295) + ShowCoordinates.GetHashCode();
            hashCode = (hashCode * -1521134295) + ShowPermit.GetHashCode();
            hashCode = (hashCode * -1521134295) + ShowInformation.GetHashCode();
            hashCode = (hashCode * -1521134295) + ShowPrimaryStar.GetHashCode();
            return hashCode;
        }
    }
}

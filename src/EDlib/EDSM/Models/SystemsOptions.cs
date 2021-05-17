using System;

namespace EDlib.EDSM
{
    /// <summary>
    ///   <para>Request options for EDSM Systems API methods.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-v1">Systems v1</a>.</para>
    /// </summary>
    public class SystemsOptions : IEquatable<SystemsOptions>
    {
        /// <summary>Include the EDSM system ID in the results.</summary>
        public bool ShowId { get; set; }

        /// <summary>Include the stellar coordinates of the system in the results.</summary>
        public bool ShowCoordinates { get; set; }

        /// <summary>Include system permit details in the results.</summary>
        public bool ShowPermit { get; set; }

        /// <summary>Include a summary of system information in the results.</summary>
        public bool ShowInformation { get; set; }

        /// <summary>Include primary star details in the results.</summary>
        public bool ShowPrimaryStar { get; set; }

        /// <summary>Determines whether the specified <see cref="System.Object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as SystemsOptions);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
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

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
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

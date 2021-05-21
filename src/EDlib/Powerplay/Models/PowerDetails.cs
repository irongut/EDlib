using EDlib.Platform;

namespace EDlib.Powerplay
{
    /// <summary>Represents a Power's statistics, ethos and benefits data.</summary>
    public class PowerDetails
    {
        /// <summary>The unique identifier for the Power.</summary>
        public int Id { get; }

        /// <summary>The short name for the Power.</summary>
        public string ShortName { get; }

        /// <summary>The name of the Power's headquarters system.</summary>
        public string HQ { get; }

        /// <summary>The year the Power was born.</summary>
        public int YearOfBirth { get; }

        /// <summary>The Power's allegiance.</summary>
        public string Allegiance { get; }

        /// <summary>The Power's preparation ethos.</summary>
        public string PreparationEthos { get; }

        /// <summary>Description of the Power's preparation ethos.</summary>
        public string PreparationText { get; }

        /// <summary>The Power's expansion ethos.</summary>
        public string ExpansionEthos { get; }

        /// <summary>Description of the Power's expansion ethos.</summary>
        public string ExpansionText { get; }

        /// <summary>The government types that help the Power's expansion.</summary>
        public string ExpansionStrongGovernment { get; }

        /// <summary>The government types that hinder the Power's expansion.</summary>
        public string ExpansionWeakGovernment { get; }

        /// <summary>The Power's control ethos.</summary>
        public string ControlEthos { get; }

        /// <summary>Description of the Power's control ethos.</summary>
        public string ControlText { get; }

        /// <summary>The government types that help the Power control their domain.</summary>
        public string ControlStrongGovernment { get; }

        /// <summary>The government types that hinder the Power controlling their domain.</summary>
        public string ControlWeakGovernment { get; }

        /// <summary>Any effects the Power has on their headquarters system.</summary>
        public string HQSystemEffect { get; }

        /// <summary>The effects a Power has on systems they control.</summary>
        public string ControlSystemEffect { get; }

        /// <summary>The effects a Power has on Alliance systems they exploit.</summary>
        public string AllianceExploitedEffect { get; }

        /// <summary>The effects a Power has on Empire systems they exploit.</summary>
        public string EmpireExploitedEffect { get; }

        /// <summary>The effects a Power has on Federation systems they exploit.</summary>
        public string FederationExploitedEffect { get; }

        /// <summary>The effects a Power has on Independent systems they exploit.</summary>
        public string IndependentExploitedEffect { get; }

        /// <summary>The benefits of pledging for a Commander at rank 1.</summary>
        public string Rating1 { get; }

        /// <summary>The benefits of pledging for a Commander at rank 2.</summary>
        public string Rating2 { get; }

        /// <summary>The benefits of pledging for a Commander at rank 3.</summary>
        public string Rating3 { get; }

        /// <summary>The benefits of pledging for a Commander at rank 4.</summary>
        public string Rating4 { get; }

        /// <summary>The benefits of pledging for a Commander at rank 5.</summary>
        public string Rating5 { get; }

        /// <summary>Initializes a new instance of the <see cref="PowerDetails" /> class.</summary>
        /// <param name="id">The unique identifier for the Power.</param>
        /// <param name="shortName">The Power's short name.</param>
        /// <param name="hq">The Power's headquarters.</param>
        /// <param name="yearOfBirth">The Power's year of birth.</param>
        /// <param name="allegiance">The Power's allegiance.</param>
        /// <param name="preparationEthos">The Power's preparation ethos.</param>
        /// <param name="preparationText">The description of the preparation ethos.</param>
        /// <param name="expansionEthos">The Power's expansion ethos.</param>
        /// <param name="expansionText">The description of the expansion ethos.</param>
        /// <param name="expansionStrongGovernment">The government types that help expansion.</param>
        /// <param name="expansionWeakGovernment">The government types that hinder expansion.</param>
        /// <param name="controlEthos">The Power's control ethos.</param>
        /// <param name="controlText">The description of the control ethos.</param>
        /// <param name="controlStrongGovernment">The government types that help with control.</param>
        /// <param name="controlWeakGovernment">The government types that hinder with control.</param>
        /// <param name="hqSystemEffect">Any effects in the Power's headquarters.</param>
        /// <param name="controlSystemEffect">The effects on systems the Power controls.</param>
        /// <param name="allianceExploitedEffect">The effects on Alliance systems the Power exploits.</param>
        /// <param name="empireExploitedEffect">The effects on Empire systems the Power exploits.</param>
        /// <param name="federationExploitedEffect">The effects on Federation systems the Power exploits.</param>
        /// <param name="independentExploitedEffect">The effects on Independent systems the Power exploits.</param>
        /// <param name="rating1">Commander benefits at rank 1.</param>
        /// <param name="rating2">Commander benefits at rank 2.</param>
        /// <param name="rating3">Commander benefits at rank 3.</param>
        /// <param name="rating4">Commander benefits at rank 4.</param>
        /// <param name="rating5">Commander benefits at rank 5.</param>
        [Preserve(Conditional = true)]
        public PowerDetails(int id,
                            string shortName,
                            string hq,
                            int yearOfBirth,
                            string allegiance,
                            string preparationEthos,
                            string preparationText,
                            string expansionEthos,
                            string expansionText,
                            string expansionStrongGovernment,
                            string expansionWeakGovernment,
                            string controlEthos,
                            string controlText,
                            string controlStrongGovernment,
                            string controlWeakGovernment,
                            string hqSystemEffect,
                            string controlSystemEffect,
                            string allianceExploitedEffect,
                            string empireExploitedEffect,
                            string federationExploitedEffect,
                            string independentExploitedEffect,
                            string rating1,
                            string rating2,
                            string rating3,
                            string rating4,
                            string rating5)
        {
            Id = id;
            ShortName = shortName;
            HQ = hq;
            YearOfBirth = yearOfBirth;
            Allegiance = allegiance;
            PreparationEthos = preparationEthos;
            PreparationText = preparationText;
            ExpansionEthos = expansionEthos;
            ExpansionText = expansionText;
            ExpansionStrongGovernment = expansionStrongGovernment;
            ExpansionWeakGovernment = expansionWeakGovernment;
            ControlEthos = controlEthos;
            ControlText = controlText;
            ControlStrongGovernment = controlStrongGovernment;
            ControlWeakGovernment = controlWeakGovernment;
            HQSystemEffect = hqSystemEffect;
            ControlSystemEffect = controlSystemEffect;
            AllianceExploitedEffect = allianceExploitedEffect;
            EmpireExploitedEffect = empireExploitedEffect;
            FederationExploitedEffect = federationExploitedEffect;
            IndependentExploitedEffect = independentExploitedEffect;
            Rating1 = rating1;
            Rating2 = rating2;
            Rating3 = rating3;
            Rating4 = rating4;
            Rating5 = rating5;
        }

        /// <summary>Returns a string containing some Power details: short name (allegiance) - headquarters</summary>
        /// <returns>A <see cref="System.String" /> that represents the Power details.</returns>
        public override string ToString()
        {
            return $"{ShortName} ({Allegiance}) - {HQ}";
        }
    }
}

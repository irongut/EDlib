using EDlib.Platform;

namespace EDlib.Powerplay
{
    /// <summary>Contains a Power's statistics, ethos and benefits data.</summary>
    public class PowerDetails
    {
        /// <summary>Gets or sets a unique identifier for the Power.</summary>
        /// <value>The unique identifier for the Power.</value>
        public int Id { get; }

        /// <summary>Gets or sets a short name for the Power.</summary>
        /// <value>The Power's short name.</value>
        public string ShortName { get; }

        /// <summary>Gets the name of the Power's headquarters system.</summary>
        /// <value>The name of the Power's headquarters.</value>
        public string HQ { get; }

        /// <summary>Gets the year the Power was born.</summary>
        /// <value>The year the Power was born..</value>
        public int YearOfBirth { get; }

        /// <summary>Gets the Power's allegiance.</summary>
        /// <value>The Power's allegiance.</value>
        public string Allegiance { get; }

        /// <summary>Gets the Power's preparation ethos.</summary>
        /// <value>The Power's preparation ethos.</value>
        public string PreparationEthos { get; }

        /// <summary>Gets the description of the Power's preparation ethos.</summary>
        /// <value>The description of the preparation ethos.</value>
        public string PreparationText { get; }

        /// <summary>Gets the Power's expansion ethos.</summary>
        /// <value>The Power's expansion ethos.</value>
        public string ExpansionEthos { get; }

        /// <summary>Gets the description of the Power's expansion ethos.</summary>
        /// <value>The description of the expansion ethos.</value>
        public string ExpansionText { get; }

        /// <summary>Gets the government types that help the Power's expansion.</summary>
        /// <value>The government types that help expansion.</value>
        public string ExpansionStrongGovernment { get; }

        /// <summary>Gets the government types that hinder the Power's expansion.</summary>
        /// <value>The government types that hinder expansion.</value>
        public string ExpansionWeakGovernment { get; }

        /// <summary>Gets the Power's control ethos.</summary>
        /// <value>The Power's control ethos.</value>
        public string ControlEthos { get; }

        /// <summary>Gets the description of the Power's control ethos.</summary>
        /// <value>The description of the control ethos.</value>
        public string ControlText { get; }

        /// <summary>Gets the government types that help the Power control their domain.</summary>
        /// <value>The government types that help with control.</value>
        public string ControlStrongGovernment { get; }

        /// <summary>Gets the government types that hinder the Power control their domain.</summary>
        /// <value>The government types that hinder with control.</value>
        public string ControlWeakGovernment { get; }

        /// <summary>Gets a description of any effects in the Power's headquarters system.</summary>
        /// <value>A description of any effects in the Power's headquarters.</value>
        public string HQSystemEffect { get; }

        /// <summary>Gets a description of the effects a Power has on systems they control.</summary>
        /// <value>A description of the effects on systems the Power controls.</value>
        public string ControlSystemEffect { get; }

        /// <summary>Gets a description of the effects a Power has on Alliance systems they exploit.</summary>
        /// <value>A description of the effects on Alliance systems the Power exploits.</value>
        public string AllianceExploitedEffect { get; }

        /// <summary>Gets a description of the effects a Power has on Empire systems they exploit.</summary>
        /// <value>A description of the effects on Empire systems the Power exploits.</value>
        public string EmpireExploitedEffect { get; }

        /// <summary>Gets a description of the effects a Power has on Federation systems they exploit.</summary>
        /// <value>A description of the effects on Federation systems the Power exploits.</value>
        public string FederationExploitedEffect { get; }

        /// <summary>Gets a description of the effects a Power has on Independent systems they exploit.</summary>
        /// <value>A description of the effects on Independent systems the Power exploits.</value>
        public string IndependentExploitedEffect { get; }

        /// <summary>Gets a description of the benefits to a Commander at rank 1.</summary>
        /// <value>A description of the rank 1 benefits.</value>
        public string Rating1 { get; }

        /// <summary>Gets a description of the benefits to a Commander at rank 2.</summary>
        /// <value>A description of the rank 2 benefits.</value>
        public string Rating2 { get; }

        /// <summary>Gets a description of the benefits to a Commander at rank 3.</summary>
        /// <value>A description of the rank 3 benefits.</value>
        public string Rating3 { get; }

        /// <summary>Gets a description of the benefits to a Commander at rank 4.</summary>
        /// <value>A description of the rank 4 benefits.</value>
        public string Rating4 { get; }

        /// <summary>Gets a description of the benefits to a Commander at rank 5.</summary>
        /// <value>A description of the rank 5 benefits.</value>
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

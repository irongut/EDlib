namespace EDlib.Powerplay
{
    public class PowerDetails
    {
        public int Id { get; }
        public string ShortName { get; }
        public string HQ { get; }
        public int YearOfBirth { get; }
        public string Allegiance { get; }
        public string PreparationEthos { get; }
        public string PreparationText { get; }
        public string ExpansionEthos { get; }
        public string ExpansionText { get; }
        public string ExpansionStrongGovernment { get; }
        public string ExpansionWeakGovernment { get; }
        public string ControlEthos { get; }
        public string ControlText { get; }
        public string ControlStrongGovernment { get; }
        public string ControlWeakGovernment { get; }
        public string HQSystemEffect { get; }
        public string ControlSystemEffect { get; }
        public string AllianceExploitedEffect { get; }
        public string EmpireExploitedEffect { get; }
        public string FederationExploitedEffect { get; }
        public string IndependentExploitedEffect { get; }
        public string Rating1 { get; }
        public string Rating2 { get; }
        public string Rating3 { get; }
        public string Rating4 { get; }
        public string Rating5 { get; }

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

        public override string ToString()
        {
            return $"{ShortName} ({Allegiance}) - {HQ}";
        }
    }
}

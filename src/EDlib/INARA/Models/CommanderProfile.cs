using EDlib.Common;
using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EDlib.INARA
{
    public class CommanderProfile
    {
        #region Properties

        [JsonProperty("userID")]
        public long UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("commanderRanksPilot")]
        public List<CommanderRank> Ranks { get; set; }

        [JsonProperty("preferredAllegianceName")]
        public string PreferredAllegiance { get; set; }

        [JsonProperty("preferredPowerName")]
        public string PreferredPower { get; set; }

        [JsonProperty("commanderMainShip")]
        public CommanderShip MainShip { get; set; }

        [JsonProperty("commanderSquadron")]
        public CommanderSquadron Squadron { get; set; }

        [JsonProperty("commanderWing")]
        public CommanderWing Wing { get; set; }

        [JsonProperty("preferredGameRole")]
        public string PreferredGameRole { get; set; }

        [JsonProperty("avatarImageURL")]
        public string AvatarImageUrl { get; set; }

        [JsonProperty("inaraURL")]
        public string InaraUrl { get; set; }

        [JsonProperty("otherNamesFound")]
        public List<string> OtherNamesFound { get; set; }

        /// <summary>The date and time when the information was requested.</summary>
        public DateTime LastUpdated { get; }

        #endregion

        /// <summary>Initializes a new instance of the <see cref="CommanderProfile" /> class.</summary>
        [Preserve(Conditional = true)]
        public CommanderProfile()
        {
            LastUpdated = DateTime.Now;
        }
    }

    public class CommanderShip
    {
        [JsonProperty("shipType")]
        public string Type { get; set; }

        [JsonProperty("shipName")]
        public string Name { get; set; }

        [JsonProperty("shipIdent")]
        public string Ident { get; set; }

        [JsonProperty("shipRole")]
        public string Role { get; set; }
    }

    public class CommanderRank
    {
        [JsonProperty("rankName")]
        public string Name { get; set; }

        [JsonProperty("rankValue")]
        [JsonConverter(typeof(ParseStringIntConverter))]
        public int Value { get; set; }

        [JsonProperty("rankProgress")]
        public double Progress { get; set; }
    }

    public class CommanderSquadron
    {
        [JsonProperty("squadronID")]
        public long Id { get; set; }

        [JsonProperty("squadronName")]
        public string Name { get; set; }

        [JsonProperty("squadronMembersCount")]
        public long MembersCount { get; set; }

        [JsonProperty("squadronMemberRank")]
        public string Rank { get; set; }

        [JsonProperty("inaraURL")]
        public string InaraUrl { get; set; }
    }

    public class CommanderWing
    {
        [JsonProperty("wingID")]
        public long Id { get; set; }

        [JsonProperty("wingName")]
        public string Name { get; set; }

        [JsonProperty("wingMembersCount")]
        public long MembersCount { get; set; }

        [JsonProperty("wingMemberRank")]
        public string Rank { get; set; }

        [JsonProperty("inaraURL")]
        public string InaraUrl { get; set; }
    }
}

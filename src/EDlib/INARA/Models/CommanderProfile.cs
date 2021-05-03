using EDlib.Common;
using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EDlib.INARA
{
    /// <summary>
    ///   <para>Represents basic information about a Commander from the INARA API.</para>
    ///   <para>See INARA documentation for <a href="https://inara.cz/inara-api-docs/#event-2">getCommanderProfile</a>.</para>
    /// </summary>
    public class CommanderProfile
    {
        #region Properties

        /// <summary>The Commander's user ID on INARA.</summary>
        [JsonProperty("userID")]
        public long UserId { get; set; }

        /// <summary>The Commander's user name on INARA.</summary>
        [JsonProperty("userName")]
        public string UserName { get; set; }

        /// <summary>The Commander's ranks in combat, trade, exploration, etc.</summary>
        [JsonProperty("commanderRanksPilot")]
        public List<CommanderRank> Ranks { get; set; }

        /// <summary>The Commander's superpower allegiance.</summary>
        [JsonProperty("preferredAllegianceName")]
        public string PreferredAllegiance { get; set; }

        /// <summary>The Galactic Power supported by the Commander.</summary>
        [JsonProperty("preferredPowerName")]
        public string PreferredPower { get; set; }

        /// <summary>The Commander's main ship as set on INARA.</summary>
        [JsonProperty("commanderMainShip")]
        public CommanderShip MainShip { get; set; }

        /// <summary>The Commander's Squadron.</summary>
        [JsonProperty("commanderSquadron")]
        public CommanderSquadron Squadron { get; set; }

        /// <summary>The Commander's Wing.</summary>
        [JsonProperty("commanderWing")]
        public CommanderWing Wing { get; set; }

        /// <summary>The Commander's preferred role in game.</summary>
        [JsonProperty("preferredGameRole")]
        public string PreferredGameRole { get; set; }

        /// <summary>URL for the Commander's avatar image on INARA. (jpeg)</summary>
        [JsonProperty("avatarImageURL")]
        public string AvatarImageUrl { get; set; }

        /// <summary>URL for the Commander's profile on INARA.</summary>
        [JsonProperty("inaraURL")]
        public string InaraUrl { get; set; }

        /// <summary>List of possible Commander names returned when a partial match is found.</summary>
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

    /// <summary>Basic information about a Commander's ship returned by the INARA API.</summary>
    public class CommanderShip
    {
        /// <summary>The ship type.</summary>
        [JsonProperty("shipType")]
        public string Type { get; set; }

        /// <summary>The ship's name.</summary>
        [JsonProperty("shipName")]
        public string Name { get; set; }

        /// <summary>The ship's ID tag.</summary>
        [JsonProperty("shipIdent")]
        public string Ident { get; set; }

        /// <summary>The ship's role.</summary>
        [JsonProperty("shipRole")]
        public string Role { get; set; }
    }

    /// <summary>Information about a Commander's rank returned by the INARA API.</summary>
    public class CommanderRank
    {
        /// <summary>The name of the rank.</summary>
        [JsonProperty("rankName")]
        public string Name { get; set; }

        /// <summary>The rank reached by the Commander.</summary>
        [JsonProperty("rankValue")]
        [JsonConverter(typeof(ParseStringIntConverter))]
        public int Value { get; set; }

        /// <summary>The Commander's progress in their current rank.</summary>
        [JsonProperty("rankProgress")]
        public double Progress { get; set; }
    }

    /// <summary>Information about a Commander's squadron returned by the INARA API.</summary>
    public class CommanderSquadron
    {
        /// <summary>The squadron's ID on INARA.</summary>
        [JsonProperty("squadronID")]
        public long Id { get; set; }

        /// <summary>The squadron's name.</summary>
        [JsonProperty("squadronName")]
        public string Name { get; set; }

        /// <summary>The number of Commanders in the squadron.</summary>
        [JsonProperty("squadronMembersCount")]
        public long MembersCount { get; set; }

        /// <summary>The Commander's rank in the squadron.</summary>
        [JsonProperty("squadronMemberRank")]
        public string Rank { get; set; }

        /// <summary>URL for the squadron's profile on INARA.</summary>
        [JsonProperty("inaraURL")]
        public string InaraUrl { get; set; }
    }

    /// <summary>Information about a Commander's wing returned by the INARA API.</summary>
    public class CommanderWing
    {
        /// <summary>The wing's ID on INARA.</summary>
        [JsonProperty("wingID")]
        public long Id { get; set; }

        /// <summary>The wing's name.</summary>
        [JsonProperty("wingName")]
        public string Name { get; set; }

        /// <summary>The number of Commanders in the wing.</summary>
        [JsonProperty("wingMembersCount")]
        public long MembersCount { get; set; }

        /// <summary>The Commander's rank in the wing.</summary>
        [JsonProperty("wingMemberRank")]
        public string Rank { get; set; }

        /// <summary>URL for the wing's profile on INARA.</summary>
        [JsonProperty("inaraURL")]
        public string InaraUrl { get; set; }
    }
}

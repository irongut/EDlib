using EDlib.Platform;

namespace EDlib.Powerplay
{
    /// <summary>Contains the comms data for a Power - Reddit Sub and Discord / Slack server.</summary>
    public class PowerComms
    {
        /// <summary>The unique identifier for the Power.</summary>
        public int Id { get; set; }

        /// <summary>The short name for the Power.</summary>
        public string ShortName { get; set; }

        /// <summary>The URL for the Power's subreddit.</summary>
        public string Reddit { get; set; }

        /// <summary>The join link for the Power's Discord or Slack server.</summary>
        public string Comms { get; set; }

        /// <summary>Initializes a new instance of the <see cref="PowerComms" /> class.</summary>
        /// <param name="id">The unique identifier for the Power.</param>
        /// <param name="shortName">The Power's short name.</param>
        /// <param name="reddit">The Power's subreddit.</param>
        /// <param name="comms">The Power's Discord or Slack server.</param>
        [Preserve(Conditional = true)]
        public PowerComms(int id, string shortName, string reddit, string comms)
        {
            Id = id;
            ShortName = shortName;
            Reddit = reddit;
            Comms = comms;
        }

        /// <summary>Returns the Power's comms details as a string.</summary>
        /// <returns>A <see cref="System.String" /> that represents the Power's comms details.</returns>
        public override string ToString()
        {
            return $"({Id}) {ShortName}, Reddit: {Reddit}, Comms: {Comms}";
        }
    }
}

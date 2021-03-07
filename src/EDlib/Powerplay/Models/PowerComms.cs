using EDlib.Platform;

namespace EDlib.Powerplay
{
    /// <summary>Contains the comms data for a Power - Reddit Sub and Discord / Slack servers.</summary>
    public class PowerComms
    {
        /// <summary>Gets or sets a unique identifier for the Power.</summary>
        /// <value>The unique identifier for the Power.</value>
        public int Id { get; set; }

        /// <summary>Gets or sets a short name for the Power.</summary>
        /// <value>The Power's short name.</value>
        public string ShortName { get; set; }

        /// <summary>Gets or sets the URL for the Power's subreddit.</summary>
        /// <value>The Power's subreddit URL.</value>
        public string Reddit { get; set; }

        /// <summary>Gets or sets the join link for the Power's Discord or Slack server.</summary>
        /// <value>The Power's Discord or Slack server join link.</value>
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

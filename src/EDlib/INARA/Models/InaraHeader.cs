using EDlib.Platform;
using Newtonsoft.Json;

namespace EDlib.INARA
{
    /// <summary>
    ///   <para>Represents the header block in an INARA API request and response.</para>
    ///   <para>See INARA documentation for <a href="https://inara.cz/inara-api-docs/#eventdataheaderprops">Header properties</a>.</para>
    /// </summary>
    public class InaraHeader
    {
        #region input fields

        /// <summary>The name of the application.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("appName")]
        public string AppName { get; }

        /// <summary>The version of the application.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("appVersion")]
        public string AppVersion { get; }

        /// <summary>A user's personal API key or a generic application API key (for read-only events).</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("APIkey")]
        public string ApiKey { get; }

        /// <summary>Set to <c>true</c> to indicate this version is in development.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("isDeveloped")]
        public bool IsDeveloped { get; }

        #endregion

        #region output fields

        /// <summary>Event status code, see <a href="https://inara.cz/inara-api-docs/#eventdatacodes">INARA eventStatus codes</a>.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("eventStatus")]
        public int? EventStatus { get; internal set; }

        /// <summary>Explanation of the status code, only returned on errors and warnings.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("eventStatusText")]
        public string EventStatusText { get; internal set; }

        #endregion

        /// <summary>Initializes a new empty instance of the <see cref="InaraHeader" /> class.</summary>
        [Preserve(Conditional = true)]
        public InaraHeader() { }

        /// <summary>Initializes a new instance of the <see cref="InaraHeader" /> class with the provided credentials.</summary>
        /// <param name="identity">The <see cref="InaraIdentity" /> credentials required to access the INARA API.</param>
        [Preserve(Conditional = true)]
        public InaraHeader(InaraIdentity identity)
        {
            AppName = identity.AppName;
            AppVersion = identity.AppVersion;
            ApiKey = identity.ApiKey;
            IsDeveloped = identity.IsDeveloped;
        }
    }
}

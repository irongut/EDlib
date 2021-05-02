using EDlib.Platform;

namespace EDlib.INARA
{
    /// <summary>
    ///   <para>Represents the credentials required to access the INARA API.</para>
    ///   <para>See INARA documentation for <a href="https://inara.cz/inara-api-docs/#eventdataheaderprops">Header properties</a>.</para>
    /// </summary>
    public class InaraIdentity
    {
        /// <summary>The name of the application.</summary>
        public string AppName { get; }

        /// <summary>The version of the application.</summary>
        public string AppVersion { get; }

        /// <summary>A user's personal API key or a generic application API key (for read-only events).</summary>
        public string ApiKey { get; }

        /// <summary>Set to <c>true</c> to indicate this version is in development.</summary>
        public bool IsDeveloped { get; }

        /// <summary>Initializes a new instance of the <see cref="InaraIdentity" /> class with the provided credentials.</summary>
        /// <param name="appName">The name of the application.</param>
        /// <param name="appVersion">The version of the application.</param>
        /// <param name="apiKey">A user's personal API key or a generic application API key.</param>
        /// <param name="isDeveloped">Set <c>true</c> if in development.</param>
        [Preserve(Conditional = true)]
        public InaraIdentity(string appName, string appVersion, string apiKey, bool isDeveloped)
        {
            AppName = appName;
            AppVersion = appVersion;
            ApiKey = apiKey;
            IsDeveloped = isDeveloped;
        }
    }
}

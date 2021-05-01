using EDlib.Platform;

namespace EDlib.INARA
{
    public class InaraIdentity
    {
        public string AppName { get; }

        public string AppVersion { get; }

        public string ApiKey { get; }

        public bool IsDeveloped { get; }

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

using EDlib.Platform;
using Newtonsoft.Json;

namespace EDlib.INARA
{
    public class InaraHeader
    {
        #region input fields

        [JsonProperty("appName")]
        public string AppName { get; }

        [JsonProperty("appVersion")]
        public string AppVersion { get; }

        [JsonProperty("APIkey")]
        public string ApiKey { get; }

        [JsonProperty("isDeveloped")]
        public bool IsDeveloped { get; }

        #endregion

        #region output fields

        [JsonProperty("eventStatus")]
        public int? EventStatus { get; internal set; }

        [JsonProperty("eventStatusText")]
        public string EventStatusText { get; internal set; }

        #endregion

        [Preserve(Conditional = true)]
        public InaraHeader() { }

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

using Newtonsoft.Json;
using System.Collections.Generic;

namespace EDlib.INARA
{
    public struct InaraRequest
    {
        [JsonProperty("header")]
        public InaraHeader Header;

        [JsonProperty("events")]
        public IList<InaraEvent> Events;
    }
}

using EDlib.Platform;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EDlib.INARA
{
    /// <summary>
    ///   <para>Represents an INARA API request and response.</para>
    ///   <para>See INARA <a href="https://inara.cz/inara-api-docs/">API documentation.</a>.</para>
    /// </summary>
    public struct InaraRequest
    {
        /// <summary>The header block in an INARA API request and response.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("header")]
        public InaraHeader Header;

        /// <summary>The data block in an INARA API request and response.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("events")]
        public IList<InaraEvent> Events;
    }
}

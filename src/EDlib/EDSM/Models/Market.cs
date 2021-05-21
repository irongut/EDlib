using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EDlib.EDSM
{
    /// <summary>
    ///   <para>Represents a station market returned by EDSM System API methods.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-system-v1">System v1</a>.</para>
    /// </summary>
    public class Market
    {
        /// <summary>The EDSM internal ID of the market.</summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>The EDSM internal ID of the market (64 bit).</summary>
        [JsonProperty("id64")]
        public long Id64 { get; set; }

        /// <summary>The name of the system.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The market ID (use this ID for EDSM queries).</summary>
        [JsonProperty("marketId")]
        public long MarketId { get; set; }

        /// <summary>The EDSM internal ID of the station.</summary>
        [JsonProperty("sId")]
        public long SId { get; set; }

        /// <summary>The name of the station.</summary>
        [JsonProperty("sName")]
        public string SName { get; set; }

        /// <summary>The EDSM URL for the market.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>List of <see cref="Commodity" /> available from this market.</summary>
        [JsonProperty("commodities")]
        public List<Commodity> Commodities { get; set; }

        /// <summary>The date and time when the information was requested.</summary>
        public DateTime LastUpdated { get; }

        /// <summary>Initializes a new instance of the <see cref="Market" /> class.</summary>
        /// <param name="id">The EDSM market ID.</param>
        /// <param name="id64">The EDSM 64 bit market ID.</param>
        /// <param name="name">The system name.</param>
        /// <param name="marketId">The market ID.</param>
        /// <param name="sId">The station ID.</param>
        /// <param name="sName">The station name.</param>
        /// <param name="url">The EDSM URL for the market.</param>
        /// <param name="commodities">List of <see cref="Commodity" /> available from this market.</param>
        [Preserve(Conditional = true)]
        public Market(long id, long id64, string name, long marketId, long sId, string sName, string url, List<Commodity> commodities)
        {
            Id = id;
            Id64 = id64;
            Name = name;
            MarketId = marketId;
            SId = sId;
            SName = sName;
            Url = url;
            Commodities = commodities;
            LastUpdated = DateTime.Now;
        }
    }

    /// <summary>
    ///   <para>Represents a market commodity returned by EDSM System API methods.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-system-v1">System v1</a>.</para>
    /// </summary>
    public class Commodity
    {
        /// <summary>The ID of the commodity.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>The name of the commodity.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>The price when buying the commodity.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("buyPrice")]
        public long BuyPrice { get; set; }

        /// <summary>The quantity of the commodity available to buy.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("stock")]
        public long Stock { get; set; }

        /// <summary>The price when selling the commodity.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("sellPrice")]
        public long SellPrice { get; set; }

        /// <summary>Demand for the commodity when selling.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("demand")]
        public long Demand { get; set; }

        /// <summary>Stock bracket for the commodity.</summary>
        [Preserve(Conditional = true)]
        [JsonProperty("stockBracket")]
        public long StockBracket { get; set; }
    }
}

using EDlib.Network;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDlib.INARA
{
    /// <summary>Interface for InaraService, a utility service used by other services to get data from INARA APIs.</summary>
    public interface IInaraService
    {
        /// <summary>Gets data from an INARA API.</summary>
        /// <param name="header">The header for an INARA API request.</param>
        /// <param name="input">The data for an INARA API request.</param>
        /// <param name="options">Options for download.</param>
        /// <returns>Task&lt;(string, DateTime)&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        Task<(string data, DateTime updated)> GetData(InaraHeader header, IList<InaraEvent> input, DownloadOptions options);
    }
}
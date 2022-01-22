using EDlib.Network;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDlib.EDSM
{
    /// <summary>
    ///   <para>Interface for EdsmService, a utility service used by other services to get data from EDSM APIs.</para>
    /// </summary>
    public interface IEdsmService
    {
        /// <summary>Gets data from an EDSM API.</summary>
        /// <param name="method">The EDSM API method.</param>
        /// <param name="parameters">The parameters for the API method.</param>
        /// <param name="options">Options for download.</param>
        /// <returns>Task&lt;(string, DateTime)&gt;<br /></returns>
        Task<(string data, DateTime updated)> GetData(string method, Dictionary<string, string> parameters, DownloadOptions options);
    }
}
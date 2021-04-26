using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EDlib.Network
{
    /// <summary>Interface for services used to download data.</summary>
    public interface IDownloadService
    {
        /// <summary>Gets data from an API and returns the response with the option to cancel the request.</summary>
        /// <param name="url">The URL of the API.</param>
        /// <param name="options">Options structure for download.</param>
        /// <returns>Task&lt;(string data, DateTime updated)&gt;</returns>
        Task<(string data, DateTime updated)> GetData(string url, DownloadOptions options);

        /// <summary>Posts a request to an API and returns the response with the option to cancel the request.</summary>
        /// <param name="url">The URL of the API.</param>
        /// <param name="content">The content of the API request.</param>
        /// <param name="options">Options structure for download.</param>
        /// <returns>Task&lt;(string data, DateTime updated)&gt;</returns>
        Task<(string data, DateTime updated)> PostData(string url, StringContent content, DownloadOptions options);
    }
}
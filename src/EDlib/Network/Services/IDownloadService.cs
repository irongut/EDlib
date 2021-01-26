using System;
using System.Threading.Tasks;

namespace EDlib.Network
{
    /// <summary>Interface for services used to download data.</summary>
    public interface IDownloadService
    {
        /// <summary>Gets the data.</summary>
        /// <param name="url">The URL for downloading the data.</param>
        /// <param name="options">Options structure for download.</param>
        /// <returns>Task&lt;(string data, DateTime updated)&gt;</returns>
        Task<(string data, DateTime updated)> GetData(string url, DownloadOptions options);
    }
}
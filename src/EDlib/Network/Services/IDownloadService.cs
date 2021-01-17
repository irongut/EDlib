using System;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.Network
{
    internal interface IDownloadService
    {
        Task<(string data, DateTime updated)> GetData(string url, TimeSpan expiry, bool ignoreCache = false);

        Task<(string data, DateTime updated)> GetData(string url, TimeSpan expiry, CancellationTokenSource cancelToken, bool ignoreCache = false);
    }
}
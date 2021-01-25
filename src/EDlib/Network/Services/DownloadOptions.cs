using System;
using System.Threading;

namespace EDlib.Network
{
    /// <summary>Options structure for download services.</summary>
    public struct DownloadOptions
    {
        /// <summary>A cancellation token for the download method.</summary>
        public CancellationTokenSource CancelToken { get; }

        /// <summary>How long to cache the data.</summary>
        public TimeSpan Expiry { get; }

        /// <summary>Download method will ignore any cached data if set to <c>true</c>.</summary>
        public bool IgnoreCache { get; }

        /// <summary>DownloadOptions constructor.</summary>
        /// <param name="cancelToken">A cancellation token for the download method.</param>
        public DownloadOptions(CancellationTokenSource cancelToken)
        {
            CancelToken = cancelToken;
            IgnoreCache = false;
        }

        /// <summary>DownloadOptions constructor.</summary>
        /// <param name="expiry">How long to cache the data.</param>
        public DownloadOptions(TimeSpan expiry)
        {
            CancelToken = null;
            Expiry = expiry;
            IgnoreCache = false;
        }

        /// <summary>DownloadOptions constructor.</summary>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="expiry">How long to cache the data.</param>
        public DownloadOptions(CancellationTokenSource cancelToken, TimeSpan expiry)
        {
            CancelToken = cancelToken;
            Expiry = expiry;
            IgnoreCache = false;
        }

        /// <summary>DownloadOptions constructor.</summary>
        /// <param name="expiry">How long to cache the data.</param>
        /// <param name="ignoreCache">Ignore any cached data if set to <c>true</c>.</param>
        public DownloadOptions(TimeSpan expiry, bool ignoreCache)
        {
            CancelToken = null;
            Expiry = expiry;
            IgnoreCache = ignoreCache;
        }

        /// <summary>DownloadOptions constructor.</summary>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="expiry">How long to cache the data.</param>
        /// <param name="ignoreCache">Ignore any cached data if set to <c>true</c>.</param>
        public DownloadOptions(CancellationTokenSource cancelToken, TimeSpan expiry, bool ignoreCache)
        {
            CancelToken = cancelToken;
            Expiry = expiry;
            IgnoreCache = ignoreCache;
        }
    }
}

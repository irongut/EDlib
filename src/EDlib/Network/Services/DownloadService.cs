using EDlib.Platform;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.Network
{
    /// <summary>Class used to download without caching.</summary>
    public class DownloadService : IDownloadService
    {
        private static readonly DownloadService instance = new DownloadService();

        private static string agent;

        private static IConnectivityService connectivity;

        private readonly HttpClient client;

        private DownloadService()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Requested-With", agent);
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            client.Timeout = TimeSpan.FromSeconds(40);
        }

        /// <summary>Instantiates the DownloadService class.</summary>
        /// <param name="userAgent">The user agent used for downloads.</param>
        /// <param name="connectivityService">The platform specific connectivity service.</param>
        /// <returns>DownloadService</returns>
        public static DownloadService Instance(string userAgent, IConnectivityService connectivityService)
        {
            agent = userAgent;
            connectivity = connectivityService;
            return instance;
        }

        /// <summary>Gets the data.</summary>
        /// <param name="url">The URL for downloading the data.</param>
        /// <param name="options">Options structure for download.</param>
        /// <returns>Task&lt;(string data, DateTime updated)&gt;</returns>
        /// <exception cref="NoNetworkNoCacheException">No Internet available and no data cached.</exception>
        /// <exception cref="APIException">Http errors from the API called.</exception>
        public async Task<(string data, DateTime updated)> GetData(string url, DownloadOptions options)
        {
            string data;
            DateTime lastUpdated;

            if (!connectivity.IsConnected())
            {
                // no valid connectivity
                throw new NoNetworkNoCacheException("No Internet available.");
            }
            else
            {
                // download data
                (data, lastUpdated) = await Download(url, options.CancelToken).ConfigureAwait(false);
            }
            return (data, lastUpdated);
        }

        private async Task<(string data, DateTime updated)> Download(string url, CancellationTokenSource cancelToken)
        {
            var uri = new Uri(url);
            HttpResponseMessage response;
            if (cancelToken == null)
            {
                response = await client.GetAsync(uri).ConfigureAwait(false);
            }
            else
            {
                response = await client.GetAsync(uri, cancelToken.Token).ConfigureAwait(false);
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new APIException($"{response.StatusCode} - {response.ReasonPhrase}", (int)response.StatusCode);
            }
            else
            {
                string data = await HttpHelper.ReadContentAsync(response).ConfigureAwait(false);
                return (data, DateTime.Now);
            }
        }
    }
}

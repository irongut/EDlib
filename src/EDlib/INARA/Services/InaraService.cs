using EDlib.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDlib.INARA
{
    /// <summary>
    ///   <para>Utility service used by other services to get data from INARA APIs.</para>
    ///   <para>See INARA <a href="https://inara.cz/inara-api-docs/">API documentation.</a>.</para>
    /// </summary>
    public sealed class InaraService
    {
        private static readonly InaraService instance = new InaraService();

        private const string InaraURL = "https://inara.cz/inapi/v1/";

        private static IDownloadService dService;

        private InaraService() { }

        /// <summary>Instantiates the InaraService class.</summary>
        /// <param name="downloadService">IDownloadService instance used to download data.</param>
        /// <returns>InaraService</returns>
        public static InaraService Instance(IDownloadService downloadService)
        {
            dService = downloadService;
            return instance;
        }

        /// <summary>Gets data from an INARA API.</summary>
        /// <param name="header">The header for an INARA API request.</param>
        /// <param name="input">The data for an INARA API request.</param>
        /// <param name="options">Options for download.</param>
        /// <returns>Task&lt;(string, DateTime)&gt;<br /></returns>
        public async Task<(string data, DateTime updated)> GetData(InaraHeader header, IList<InaraEvent> input, DownloadOptions options)
        {
            // download data
            InaraRequest inputData = new InaraRequest()
            {
                Header = header,
                Events = input
            };

            (string data, DateTime lastUpdated) = await dService.PostData(InaraURL, JsonConvert.SerializeObject(inputData), options).ConfigureAwait(false);

            InaraRequest outputData = JsonConvert.DeserializeObject<InaraRequest>(data);

            // check for request errors
            if (outputData.Header.EventStatus > 203)
            {
                throw new APIException($"{outputData.Header.EventStatus} - {outputData.Header.EventStatusText}", outputData.Header.EventStatus);
            }
            else
            {
                // check for data errors
                string errorText = String.Empty;
                foreach (InaraEvent item in outputData.Events)
                {
                    if (item.EventStatus > 203)
                    {
                        errorText += $"{item.EventStatus} - {item.EventStatusText}, ";
                    }
                }
                if (errorText.Length > 0)
                {
                    int.TryParse(errorText.Substring(0, errorText.IndexOf(" ")), out int errorCode); // need an error code so first wins
                    throw new APIException(errorText.Remove(errorText.Length - 2), errorCode);
                }
            }
            return (data, lastUpdated);
        }
    }
}

using EDlib.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDlib.INARA
{
    public sealed class InaraService
    {
        private static readonly InaraService instance = new InaraService();

        private const string InaraURL = "https://inara.cz/inapi/v1/";

        private static IDownloadService dService;

        private InaraService() { }

        public static InaraService Instance(IDownloadService downloadService)
        {
            dService = downloadService;
            return instance;
        }

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
            if (outputData.Header.EventStatus != 200)
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

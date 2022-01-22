using EDlib.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EDlib.EDSM
{
    /// <summary>
    ///   <para>Gets data from the EDSM System API including stations, markets, shipyards, outfitting and factions.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-system-v1">System v1</a>.</para>
    /// </summary>
    public sealed class SystemService
    {
        private static readonly SystemService instance = new SystemService();

        private static IEdsmService EdsmService;

        private const string stationsMethod = "api-system-v1/stations";
        private const string marketMethod = "api-system-v1/stations/market";
        private const string shipyardMethod = "api-system-v1/stations/shipyard";
        private const string outfittingMethod = "api-system-v1/stations/outfitting";
        private const string factionsMethod = "api-system-v1/factions";

        private SystemStations stations;

        private Market market;
        private Dictionary<string, string> marketParams;

        private Shipyard shipyard;
        private Dictionary<string, string> shipyardParams;

        private StationOutfitting outfitting;
        private Dictionary<string, string> outfittingParams;

        private SystemFactions factions;

        private SystemService() {  }

        /// <summary>Instantiates the SystemService class.</summary>
        /// <param name="edsmService">IEdsmService instance used to download data from EDSM.</param>
        /// <returns>SystemService</returns>
        public static SystemService Instance(IEdsmService edsmService)
        {
            EdsmService = edsmService;
            return instance;
        }

        /// <summary>Gets information about stations in a system.</summary>
        /// <param name="systemName">The system name.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;SystemStations&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<SystemStations> GetStations(string systemName, int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetStations(systemName, null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about stations in a system.</summary>
        /// <param name="systemName">The system name.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;SystemStations&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<SystemStations> GetStations(string systemName, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            if (string.IsNullOrWhiteSpace(systemName))
            {
                throw new ArgumentNullException(nameof(systemName));
            }

            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (stations == null || stations.Name != systemName || (stations.LastUpdated + expiry < DateTime.Now))
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "systemName", systemName }
                };

                string json;
                DownloadOptions options = new DownloadOptions(cancelToken, expiry, ignoreCache);
                (json, _) = await EdsmService.GetData(stationsMethod, parameters, options).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json) || json == "{}")
                {
                    throw new APIException("EDSM method returned no data.");
                }

                stations = JsonConvert.DeserializeObject<SystemStations>(json);
            }
            return stations;
        }

        /// <summary>Gets information about a market in a station.</summary>
        /// <param name="marketId">The in-game marketId of the market.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;Market&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<Market> GetMarket(long marketId, int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetMarket(marketId, null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about a market in a station.</summary>
        /// <param name="marketId">The in-game marketId of the market.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;Market&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<Market> GetMarket(long marketId, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "marketId", marketId.ToString() }
            };
            return await GetMarket(parameters, cancelToken, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about a market in a station.</summary>
        /// <param name="systemName">The name of the system where the market is located.</param>
        /// <param name="stationName">The name of the station where the market is located.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;Market&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<Market> GetMarket(string systemName, string stationName, int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetMarket(systemName, stationName, null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about a market in a station.</summary>
        /// <param name="systemName">The name of the system where the market is located.</param>
        /// <param name="stationName">The name of the station where the market is located.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;Market&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<Market> GetMarket(string systemName, string stationName, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "systemName", systemName },
                { "stationName", stationName }
            };
            return await GetMarket(parameters, cancelToken, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        private async Task<Market> GetMarket(Dictionary<string, string> parameters, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            if (parameters?.Any() == false)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (market == null || (market.LastUpdated + expiry < DateTime.Now) || !marketParams.Equals(parameters))
            {
                string json;
                DownloadOptions options = new DownloadOptions(cancelToken, expiry, ignoreCache);
                (json, _) = await EdsmService.GetData(marketMethod, parameters, options).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json) || json == "{}")
                {
                    throw new APIException("EDSM method returned no data.");
                }

                market = JsonConvert.DeserializeObject<Market>(json);
                marketParams = parameters;
            }
            return market;
        }

        /// <summary>Gets information about a shipyard in a station.</summary>
        /// <param name="marketId">The in-game marketId of the station where the shipyard is located.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;Shipyard&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<Shipyard> GetShipyard(long marketId, int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetShipyard(marketId, null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about a shipyard in a station.</summary>
        /// <param name="marketId">The in-game marketId of the station where the shipyard is located.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;Shipyard&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<Shipyard> GetShipyard(long marketId, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "marketId", marketId.ToString() }
            };
            return await GetShipyard(parameters, cancelToken, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about a shipyard in a station.</summary>
        /// <param name="systemName">The name of the system where the shipyard is located.</param>
        /// <param name="stationName">The name of the station where the shipyard is located.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;Shipyard&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<Shipyard> GetShipyard(string systemName, string stationName, int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetShipyard(systemName, stationName, null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about a shipyard in a station.</summary>
        /// <param name="systemName">The name of the system where the shipyard is located.</param>
        /// <param name="stationName">The name of the station where the shipyard is located.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;Shipyard&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<Shipyard> GetShipyard(string systemName, string stationName, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "systemName", systemName },
                { "stationName", stationName }
            };
            return await GetShipyard(parameters, cancelToken, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        private async Task<Shipyard> GetShipyard(Dictionary<string, string> parameters, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            if (parameters?.Any() == false)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (shipyard == null || (shipyard.LastUpdated + expiry < DateTime.Now) || !shipyardParams.Equals(parameters))
            {
                string json;
                DownloadOptions options = new DownloadOptions(cancelToken, expiry, ignoreCache);
                (json, _) = await EdsmService.GetData(shipyardMethod, parameters, options).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json) || json == "{}")
                {
                    throw new APIException("EDSM method returned no data.");
                }

                shipyard = JsonConvert.DeserializeObject<Shipyard>(json);
                shipyardParams = parameters;
            }
            return shipyard;
        }

        /// <summary>Gets information about a outfitting in a station.</summary>
        /// <param name="marketId">The in-game marketId of the station where the outfitting is located.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;StationOutfitting&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<StationOutfitting> GetOutfitting(long marketId, int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetOutfitting(marketId, null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about a outfitting in a station.</summary>
        /// <param name="marketId">The in-game marketId of the station where the outfitting is located.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;StationOutfitting&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<StationOutfitting> GetOutfitting(long marketId, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "marketId", marketId.ToString() }
            };
            return await GetOutfitting(parameters, cancelToken, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about a outfitting in a station.</summary>
        /// <param name="systemName">The name of the system where the outfitting is located.</param>
        /// <param name="stationName">The name of the station where the outfitting is located.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;StationOutfitting&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<StationOutfitting> GetOutfitting(string systemName, string stationName, int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetOutfitting(systemName, stationName, null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about a outfitting in a station.</summary>
        /// <param name="systemName">The name of the system where the outfitting is located.</param>
        /// <param name="stationName">The name of the station where the outfitting is located.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;StationOutfitting&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<StationOutfitting> GetOutfitting(string systemName, string stationName, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "systemName", systemName },
                { "stationName", stationName }
            };
            return await GetOutfitting(parameters, cancelToken, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        private async Task<StationOutfitting> GetOutfitting(Dictionary<string, string> parameters, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            if (parameters?.Any() == false)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (outfitting == null || (outfitting.LastUpdated + expiry < DateTime.Now) || !outfittingParams.Equals(parameters))
            {
                string json;
                DownloadOptions options = new DownloadOptions(cancelToken, expiry, ignoreCache);
                (json, _) = await EdsmService.GetData(outfittingMethod, parameters, options).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json) || json == "{}")
                {
                    throw new APIException("EDSM method returned no data.");
                }

                outfitting = JsonConvert.DeserializeObject<StationOutfitting>(json);
                outfittingParams = parameters;
            }
            return outfitting;
        }

        /// <summary>Gets information about factions in a system.</summary>
        /// <param name="systemName">The system name.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;SystemFactions&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<SystemFactions> GetFactions(string systemName, int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetFactions(systemName, null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about factions in a system.</summary>
        /// <param name="systemName">The system name.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;SystemFactions&gt;</returns>
        /// <exception cref="APIException">Errors from the API called.</exception>
        /// <exception cref="ArgumentNullException">Required argument is missing.</exception>
        public async Task<SystemFactions> GetFactions(string systemName, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            if (string.IsNullOrWhiteSpace(systemName))
            {
                throw new ArgumentNullException(nameof(systemName));
            }

            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (factions == null || factions.Name != systemName || (factions.LastUpdated + expiry < DateTime.Now))
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "systemName", systemName }
                };

                string json;
                DownloadOptions options = new DownloadOptions(cancelToken, expiry, ignoreCache);
                (json, _) = await EdsmService.GetData(factionsMethod, parameters, options).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json) || json == "{}")
                {
                    throw new APIException("EDSM method returned no data.");
                }

                factions = JsonConvert.DeserializeObject<SystemFactions>(json);
            }
            return factions;
        }
    }
}

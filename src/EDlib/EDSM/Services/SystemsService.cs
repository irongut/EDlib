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
    ///   <para>Gets data from the EDSM Systems API including allegiance, government, controlling faction, population, security and economic information.</para>
    ///   <para>See EDSM API documentation for <a href="https://www.edsm.net/en/api-v1">Systems v1</a>.</para>
    /// </summary>
    public sealed class SystemsService
    {
        private static readonly SystemsService instance = new SystemsService();

        private static IDownloadService dService;

        private const string infoMethod = "api-v1/system";
        private const string systemsMethod = "api-v1/systems?";
        private const string cubeMethod = "api-v1/cube-systems";
        private const string sphereMethod = "api-v1/sphere-systems";

        private SolarSystem solarSystem;
        private SystemsOptions solarSystemOptions;

        private List<SolarSystem> systems;
        private string[] systemsNameList;
        private SystemsOptions systemsOptions;
        private DateTime systemsUpdated;

        private List<SolarSystem> cubeSystems;
        private string cubeSystem;
        private int cubeSize;
        private SystemsOptions cubeOptions;
        private DateTime cubeUpdated;

        private List<SolarSystem> sphereSystems;
        private string sphereSystem;
        private int sphereRadius;
        private int sphereMinRadius;
        private SystemsOptions sphereOptions;
        private DateTime sphereUpdated;

        private SystemsService() { }

        /// <summary>Instantiates the SystemsService class.</summary>
        /// <param name="downloadService">IDownloadService instance used to download data.</param>
        /// <returns>SystemsService</returns>
        public static SystemsService Instance(IDownloadService downloadService)
        {
            dService = downloadService;
            return instance;
        }

        /// <summary>Gets information about a solar system.</summary>
        /// <param name="systemName">The system name.</param>
        /// <param name="options">The Systems API request options.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;SolarSystem&gt;</returns>
        public async Task<SolarSystem> GetSystem(string systemName, SystemsOptions options, int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetSystem(systemName, options, null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about a solar system.</summary>
        /// <param name="systemName">The system name.</param>
        /// <param name="options">The Systems API request options.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;SolarSystem&gt;</returns>
        public async Task<SolarSystem> GetSystem(string systemName, SystemsOptions options, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            if (string.IsNullOrWhiteSpace(systemName))
            {
                throw new ArgumentNullException(nameof(systemName));
            }

            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (solarSystem == null || solarSystem.Name != systemName || (solarSystem.LastUpdated + expiry < DateTime.Now) || !solarSystemOptions.Equals(options))
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "systemName", systemName }
                };
                parameters = AddOptions(parameters, options);

                string json;
                DownloadOptions downloadOptions = new DownloadOptions(cancelToken, expiry, ignoreCache);
                EdsmService edsmService = EdsmService.Instance(dService);
                (json, _) = await edsmService.GetData(infoMethod, parameters, downloadOptions).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json) || json == "{}")
                {
                    throw new APIException("EDSM method returned no data.");
                }

                solarSystem = JsonConvert.DeserializeObject<SolarSystem>(json);
                solarSystemOptions = options;
            }
            return solarSystem;
        }

        /// <summary>Gets information about an array of solar systems.</summary>
        /// <param name="systemNames">An array of system names.</param>
        /// <param name="options">The Systems API request options.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;SolarSystem&gt;, DateTime)&gt;</returns>
        public async Task<(List<SolarSystem> systems, DateTime updated)> GetSystems(string[] systemNames, SystemsOptions options, int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetSystems(systemNames, options, null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about an array of solar systems.</summary>
        /// <param name="systemNames">An array of system names.</param>
        /// <param name="options">The Systems API request options.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;SolarSystem&gt;, DateTime)&gt;</returns>
        public async Task<(List<SolarSystem> systems, DateTime updated)> GetSystems(string[] systemNames, SystemsOptions options, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            if (systemNames?.Any() == false)
            {
                throw new ArgumentNullException(nameof(systemNames));
            }

            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (systems?.Any() == false || (systemsUpdated + expiry < DateTime.Now) || systemsNameList.Equals(systemNames) || !systemsOptions.Equals(options))
            {
                // Dictionary doesn't allow multiple identical keys so add system names to method
                string method = systemsMethod;
                foreach (string systemName in systemNames)
                {
                    method = $"{method}systemName[]={systemName}&";
                }
                method = method.Remove(method.Length - 1);

                Dictionary<string, string> parameters = AddOptions(new Dictionary<string, string>(), options);

                string json;
                DownloadOptions downloadOptions = new DownloadOptions(cancelToken, expiry, ignoreCache);
                EdsmService edsmService = EdsmService.Instance(dService);
                (json, systemsUpdated) = await edsmService.GetData(method, parameters, downloadOptions).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json) || json == "{}")
                {
                    throw new APIException("EDSM method returned no data.");
                }

                systems = JsonConvert.DeserializeObject<List<SolarSystem>>(json);
                systemsNameList = systemNames;
                systemsOptions = options;
            }
            return (systems, systemsUpdated);
        }

        /// <summary>Gets information about systems within a cube.</summary>
        /// <param name="systemName">The name of the system at the centre of the cube.</param>
        /// <param name="size">The size of the cube in light years; max 200 ly.</param>
        /// <param name="options">The Systems API request options.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;SolarSystem&gt;, DateTime)&gt;</returns>
        public async Task<(List<SolarSystem> systems, DateTime updated)> GetSystemsInCube(string systemName, int size, SystemsOptions options, int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetSystemsInCube(systemName, size, options, null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about systems within a cube.</summary>
        /// <param name="systemName">The name of the system at the centre of the cube.</param>
        /// <param name="size">The size of the cube in light years; max 200 ly.</param>
        /// <param name="options">The Systems API request options.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;SolarSystem&gt;, DateTime)&gt;</returns>
        public async Task<(List<SolarSystem> systems, DateTime updated)> GetSystemsInCube(string systemName, int size, SystemsOptions options, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            if (string.IsNullOrWhiteSpace(systemName))
            {
                throw new ArgumentNullException(nameof(systemName));
            }

            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (cubeSystems?.Any() == false || (cubeUpdated + expiry < DateTime.Now) || cubeSystem != systemName || cubeSize != size || !cubeOptions.Equals(options))
            {
                if (size < 1) size = 1;
                else if (size > 200) size = 200;
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "systemName", systemName },
                    { "size", size.ToString() }
                };
                parameters = AddOptions(parameters, options);

                string json;
                DownloadOptions downloadOptions = new DownloadOptions(cancelToken, expiry, ignoreCache);
                EdsmService edsmService = EdsmService.Instance(dService);
                (json, cubeUpdated) = await edsmService.GetData(cubeMethod, parameters, downloadOptions).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json) || json == "{}")
                {
                    throw new APIException("EDSM method returned no data.");
                }

                cubeSystems = JsonConvert.DeserializeObject<List<SolarSystem>>(json);
                cubeSystem = systemName;
                cubeSize = size;
                cubeOptions = options;
            }
            return (cubeSystems, cubeUpdated);
        }

        /// <summary>Gets information about systems within a sphere.</summary>
        /// <param name="systemName">The name of the system at the centre of the sphere.</param>
        /// <param name="radius">The radius of the sphere in light years; max 100 ly.</param>
        /// <param name="minRadius">Set to a value between 0 and <c>radius</c> to reduce the returned results, in light years.</param>
        /// <param name="options">The Systems API request options.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;SolarSystem&gt;, DateTime)&gt;</returns>
        public async Task<(List<SolarSystem> systems, DateTime updated)> GetSystemsInSphere(string systemName, int radius, int minRadius, SystemsOptions options, int cacheMinutes = 5, bool ignoreCache = false)
        {
            return await GetSystemsInSphere(systemName, radius, minRadius, options, null, cacheMinutes, ignoreCache).ConfigureAwait(false);
        }

        /// <summary>Gets information about systems within a sphere.</summary>
        /// <param name="systemName">The name of the system at the centre of the sphere.</param>
        /// <param name="radius">The radius of the sphere in light years; max 100 ly.</param>
        /// <param name="minRadius">Set to a value between 0 and <c>radius</c> to reduce the returned results, in light years.</param>
        /// <param name="options">The Systems API request options.</param>
        /// <param name="cancelToken">A cancellation token.</param>
        /// <param name="cacheMinutes">The number of minutes to cache the data, minimum 5 minutes.</param>
        /// <param name="ignoreCache">Ignores any cached data if set to <c>true</c>.</param>
        /// <returns>Task&lt;(List&lt;SolarSystem&gt;, DateTime)&gt;</returns>
        public async Task<(List<SolarSystem> systems, DateTime updated)> GetSystemsInSphere(string systemName, int radius, int minRadius, SystemsOptions options, CancellationTokenSource cancelToken, int cacheMinutes = 5, bool ignoreCache = false)
        {
            if (string.IsNullOrWhiteSpace(systemName))
            {
                throw new ArgumentNullException(nameof(systemName));
            }

            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (sphereSystems == null || (sphereUpdated + expiry < DateTime.Now) || sphereSystem != systemName || sphereRadius != radius || sphereMinRadius != minRadius || !sphereOptions.Equals(options))
            {
                if (radius < 1) radius = 1;
                else if (radius > 100) radius = 100;
                if (minRadius < 0) minRadius = 0;
                else if (minRadius > radius) minRadius = radius;
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "systemName", systemName },
                    { "minRadius", minRadius.ToString() },
                    { "radius", radius.ToString() }
                };
                parameters = AddOptions(parameters, options);

                string json;
                DownloadOptions downloadOptions = new DownloadOptions(cancelToken, expiry, ignoreCache);
                EdsmService edsmService = EdsmService.Instance(dService);
                (json, sphereUpdated) = await edsmService.GetData(sphereMethod, parameters, downloadOptions).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(json) || json == "{}")
                {
                    throw new APIException("EDSM method returned no data.");
                }

                sphereSystems = JsonConvert.DeserializeObject<List<SolarSystem>>(json);
                sphereSystem = systemName;
                sphereRadius = radius;
                sphereMinRadius = minRadius;
                sphereOptions = options;
            }
            return (sphereSystems, sphereUpdated);
        }

        private Dictionary<string, string> AddOptions(Dictionary<string, string> parameters, SystemsOptions options)
        {
            parameters.Add("showId", options.ShowId ? "1" : "0");
            parameters.Add("showCoordinates", options.ShowCoordinates ? "1" : "0");
            parameters.Add("showPermit", options.ShowPermit ? "1" : "0");
            parameters.Add("showInformation", options.ShowInformation ? "1" : "0");
            parameters.Add("showPrimaryStar", options.ShowPrimaryStar ? "1" : "0");
            return parameters;
        }
    }
}

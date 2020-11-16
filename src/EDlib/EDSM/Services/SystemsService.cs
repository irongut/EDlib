using EDlib.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDlib.EDSM
{
    public sealed class SystemsService
    {
        private static readonly SystemsService instance = new SystemsService();

        private static string agent;

        private static ICacheService cache;

        private static IConnectivityService connectivity;

        private const string infoMethod = "api-v1/system";
        private const string infoDataKey = "Systems-Info";
        private const string infoLastUpdatedKey = "Systems-Info-LastUpdated";

        private const string systemsMethod = "api-v1/systems?";
        private const string systemsDataKey = "Systems";
        private const string systemsLastUpdatedKey = "Systems-LastUpdated";

        private const string cubeMethod = "api-v1/cube-systems";
        private const string cubeDataKey = "Systems-Cube";
        private const string cubeLastUpdatedKey = "Systems-Cube-LastUpdated";

        private const string sphereMethod = "api-v1/sphere-systems";
        private const string sphereDataKey = "Systems-Sphere";
        private const string sphereLastUpdatedKey = "Systems-Sphere-LastUpdated";

        private SolarSystem solarSystem;

        private List<SolarSystem> systems;
        private DateTime systemsUpdated;

        private List<SolarSystem> cubeSystems;
        private DateTime cubeUpdated;

        private List<SolarSystem> sphereSystems;
        private DateTime sphereUpdated;

        private SystemsService() { }

        public static SystemsService Instance(string userAgent, ICacheService cacheService, IConnectivityService connectivityService)
        {
            agent = userAgent;
            cache = cacheService;
            connectivity = connectivityService;
            return instance;
        }

        public async Task<SolarSystem> GetSystem(string systemName, SystemsOptions options, int cacheMinutes = 5, bool ignoreCache = false)
        {
            if (string.IsNullOrWhiteSpace(systemName))
            {
                throw new ArgumentNullException(nameof(systemName));
            }

            if (cacheMinutes < 5) cacheMinutes = 5;
            TimeSpan expiry = TimeSpan.FromMinutes(cacheMinutes);
            if (solarSystem == null || solarSystem.Name != systemName || (solarSystem.LastUpdated + expiry < DateTime.Now))
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>
                {
                    { "systemName", systemName }
                };
                parameters = AddOptions(parameters, options);

                string json;
                EdsmService edsmService = EdsmService.Instance(agent, cache, connectivity);
                (json, _) = await edsmService.GetData(infoMethod, parameters, infoDataKey, infoLastUpdatedKey, expiry, ignoreCache).ConfigureAwait(false);

                solarSystem = JsonConvert.DeserializeObject<SolarSystem>(json);
            }
            return solarSystem;
        }

        public async Task<(List<SolarSystem> systems, DateTime updated)> GetSystems(string[] systemNames, SystemsOptions options, bool ignoreCache = false)
        {
            if (systemNames?.Any() == false)
            {
                throw new ArgumentNullException(nameof(systemNames));
            }

            // caching needs a rethink, for now use 60s
            TimeSpan expiry = TimeSpan.FromSeconds(60);
            if (systems?.Any() == false || (systemsUpdated + expiry < DateTime.Now))
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
                EdsmService edsmService = EdsmService.Instance(agent, cache, connectivity);
                (json, systemsUpdated) = await edsmService.GetData(method, parameters, systemsDataKey, systemsLastUpdatedKey, expiry, ignoreCache).ConfigureAwait(false);

                systems = JsonConvert.DeserializeObject<List<SolarSystem>>(json);
            }
            return (systems, systemsUpdated);
        }

        public async Task<(List<SolarSystem> systems, DateTime updated)> GetSystemsInCube(string systemName, int size, SystemsOptions options, bool ignoreCache = false)
        {
            if (string.IsNullOrWhiteSpace(systemName))
            {
                throw new ArgumentNullException(nameof(systemName));
            }

            // caching needs a rethink, for now use 60s
            TimeSpan expiry = TimeSpan.FromSeconds(60);
            if (cubeSystems?.Any() == false || (cubeUpdated + expiry < DateTime.Now))
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
                EdsmService edsmService = EdsmService.Instance(agent, cache, connectivity);
                (json, cubeUpdated) = await edsmService.GetData(cubeMethod, parameters, cubeDataKey, cubeLastUpdatedKey, expiry, ignoreCache).ConfigureAwait(false);

                cubeSystems = JsonConvert.DeserializeObject<List<SolarSystem>>(json);
            }
            return (cubeSystems, cubeUpdated);
        }

        public async Task<(List<SolarSystem> systems, DateTime updated)> GetSystemsInSphere(string systemName, int radius, int minRadius, SystemsOptions options, bool ignoreCache = false)
        {
            if (string.IsNullOrWhiteSpace(systemName))
            {
                throw new ArgumentNullException(nameof(systemName));
            }

            // caching needs a rethink, for now use 60s
            TimeSpan expiry = TimeSpan.FromSeconds(60);
            if (sphereSystems == null || (sphereUpdated + expiry < DateTime.Now))
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
                EdsmService edsmService = EdsmService.Instance(agent, cache, connectivity);
                (json, sphereUpdated) = await edsmService.GetData(sphereMethod, parameters, sphereDataKey, sphereLastUpdatedKey, expiry, ignoreCache).ConfigureAwait(false);

                sphereSystems = JsonConvert.DeserializeObject<List<SolarSystem>>(json);
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

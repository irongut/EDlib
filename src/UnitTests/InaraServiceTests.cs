using EDlib;
using EDlib.INARA;
using EDlib.Mock.Platform;
using EDlib.Network;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class InaraServiceTests
    {
        private IConfigurationRoot config;
        private string appName;
        private InaraIdentity identity;

        [TestMethod]
        public void InaraEventTest()
        {
            DateTime beforeTime = DateTime.Now;
            InaraEvent inara = new InaraEvent("event-name", new List<string>()
            {
                "result1",
                "result2",
                "result3"
            });

            Assert.AreEqual("event-name", inara.EventName);
            Assert.IsTrue(inara.EventTimestamp > beforeTime);
            Assert.IsTrue(inara.EventTimestamp < DateTime.Now);
            Assert.IsNotNull(inara.EventData);
            foreach (string item in inara.EventData)
            {
                Assert.IsTrue(item.Contains("result", StringComparison.OrdinalIgnoreCase));
            }
            Assert.IsNull(inara.EventStatus);
            Assert.IsTrue(string.IsNullOrWhiteSpace(inara.EventStatusText));
        }

        [TestMethod]
        public void InaraHeaderTest()
        {
            InaraHeader header = new InaraHeader(new InaraIdentity("name", "version", "key", true));

            Assert.AreEqual("name", header.AppName);
            Assert.AreEqual("version", header.AppVersion);
            Assert.AreEqual("key", header.ApiKey);
            Assert.IsTrue(header.IsDeveloped);
            Assert.IsNull(header.EventStatus);
            Assert.IsTrue(string.IsNullOrWhiteSpace(header.EventStatusText));
        }

        [TestMethod]
        public void InaraIdentityTest()
        {
            InaraIdentity id = new InaraIdentity("name", "version", "key", true);

            Assert.AreEqual("name", id.AppName);
            Assert.AreEqual("version", id.AppVersion);
            Assert.AreEqual("key", id.ApiKey);
            Assert.IsTrue(id.IsDeveloped);
        }

        [TestMethod]
        public void InaraRequestTest()
        {
            DateTime beforeTime = DateTime.Now;
            List<InaraEvent> events = new List<InaraEvent>
            {
                new InaraEvent("event-name", new List<string>()
                {
                    "result1",
                    "result2",
                    "result3"
                })
            };

            InaraRequest request = new InaraRequest
            {
                Header = new InaraHeader(new InaraIdentity("name", "version", "key", true)),
                Events = events
            };

            Assert.IsNotNull(request.Header);
            Assert.AreEqual("name", request.Header.AppName);
            Assert.AreEqual("version", request.Header.AppVersion);
            Assert.AreEqual("key", request.Header.ApiKey);
            Assert.IsTrue(request.Header.IsDeveloped);
            Assert.IsNull(request.Header.EventStatus);
            Assert.IsTrue(string.IsNullOrWhiteSpace(request.Header.EventStatusText));

            Assert.IsNotNull(request.Events);
            Assert.AreEqual("event-name", request.Events[0].EventName);
            Assert.IsTrue(request.Events[0].EventTimestamp > beforeTime);
            Assert.IsTrue(request.Events[0].EventTimestamp < DateTime.Now);
            Assert.IsNotNull(request.Events[0].EventData);
            foreach (string item in request.Events[0].EventData)
            {
                Assert.IsTrue(item.Contains("result", StringComparison.OrdinalIgnoreCase));
            }
            Assert.IsNull(request.Events[0].EventStatus);
            Assert.IsTrue(string.IsNullOrWhiteSpace(request.Events[0].EventStatusText));
        }

        [TestMethod]
        public async Task GetDataTest()
        {
            InitialiseInaraTests();
            DownloadOptions options = new();
            InaraService inaraService = InaraService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            List<InaraEvent> input = new()
            {
                new InaraEvent("getCommunityGoalsRecent", new List<object>())
            };

            string json;
            DateTime lastUpdated;
            try
            {
                (json, lastUpdated) = await inaraService.GetData(new InaraHeader(identity), input, options).ConfigureAwait(false);
            }
            catch (APIException ex)
            {
                Assert.Inconclusive($"Skipping test due to INARA API issue: {ex.Message}");
                return;
            }

            Assert.IsFalse(string.IsNullOrWhiteSpace(json));
            Assert.IsTrue(lastUpdated > DateTime.MinValue);
        }

        private void InitialiseInaraTests()
        {
            if (config == null)
            {
                config = new ConfigurationBuilder()
                             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                             .AddJsonFile("appsettings.json")
                             .AddUserSecrets<InaraCGTests>()
                             .Build();

                appName = config["Inara-AppName"];
                identity = new(appName,
                               config["Inara-AppVersion"],
                               config["Inara-ApiKey"],
                               bool.Parse(config["Inara-IsDeveloped"]));
            }
        }
    }
}

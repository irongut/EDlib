using EDlib.INARA;
using EDlib.Mock.Platform;
using EDlib.Network;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class InaraCommanderTests
    {
        private IConfigurationRoot config;
        private string appName;
        private InaraIdentity identity;

        [TestMethod]
        public async Task ExactMatchAppKeyTest()
        {
            InitialiseInaraTests(false);

            CommanderProfileService cmdrService = CommanderProfileService.Instance(DownloadService.Instance(appName, new UnmeteredConnection()));
            CommanderProfile cmdr = await cmdrService.GetData("Irongut", 5, identity, new CancellationTokenSource()).ConfigureAwait(false);
            Assert.IsNotNull(cmdr);
            Assert.IsTrue(cmdr.LastUpdated > DateTime.MinValue);

            Assert.AreEqual(151725, cmdr.UserId);
            Assert.AreEqual("Irongut", cmdr.UserName);
            Assert.AreEqual("Empire", cmdr.PreferredAllegiance);
            Assert.AreEqual("Arissa Lavigny-Duval", cmdr.PreferredPower);
            Assert.AreEqual("Special agent/Shield of justice", cmdr.PreferredGameRole);
            Assert.AreEqual("https://inara.cz/data/users/151/151725x1547.jpg", cmdr.AvatarImageUrl);
            Assert.AreEqual("https://inara.cz/cmdr/151725/", cmdr.InaraUrl);

            Assert.IsTrue(cmdr.Ranks.Count > 0);
            foreach (var rank in cmdr.Ranks)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(rank.Name));
                Assert.IsTrue(rank.Value > 0);
                Assert.AreEqual(0, rank.Progress);
            }

            Assert.IsNotNull(cmdr.MainShip);
            Assert.AreEqual("cutter", cmdr.MainShip.Type);
            Assert.AreEqual("Liberator", cmdr.MainShip.Name);
            Assert.AreEqual("IR-24C", cmdr.MainShip.Ident);
            Assert.AreEqual(string.Empty, cmdr.MainShip.Role);

            Assert.IsNotNull(cmdr.Squadron);
            Assert.AreEqual(446, cmdr.Squadron.Id);
            Assert.AreEqual("The Chapterhouse of Inquisition", cmdr.Squadron.Name);
            Assert.IsTrue(cmdr.Squadron.MembersCount > 0);
            Assert.AreEqual("Senior wingman", cmdr.Squadron.Rank);
            Assert.AreEqual("https://inara.cz/squadron/446/", cmdr.Squadron.InaraUrl);

            Assert.IsNotNull(cmdr.Wing);
            Assert.AreEqual(446, cmdr.Wing.Id);
            Assert.AreEqual("The Chapterhouse of Inquisition", cmdr.Wing.Name);
            Assert.IsTrue(cmdr.Wing.MembersCount > 0);
            Assert.AreEqual("Senior wingman", cmdr.Wing.Rank);
            Assert.AreEqual("https://inara.cz/squadron/446/", cmdr.Wing.InaraUrl);

            Assert.IsNull(cmdr.OtherNamesFound);
        }

        [TestMethod]
        public async Task ExactMatchUserKeyTest()
        {
            InitialiseInaraTests(true);

            CommanderProfileService cmdrService = CommanderProfileService.Instance(DownloadService.Instance(appName, new UnmeteredConnection()));
            CommanderProfile cmdr = await cmdrService.GetData("Jubei Himura", 5, identity, new CancellationTokenSource()).ConfigureAwait(false);
            Assert.IsNotNull(cmdr);
            Assert.IsTrue(cmdr.LastUpdated > DateTime.MinValue);

            Assert.AreEqual(11668, cmdr.UserId);
            Assert.AreEqual("Jubei Himura", cmdr.UserName);
            Assert.AreEqual("Empire", cmdr.PreferredAllegiance);
            Assert.AreEqual("Arissa Lavigny-Duval", cmdr.PreferredPower);
            Assert.IsFalse(string.IsNullOrWhiteSpace(cmdr.PreferredGameRole));
            Assert.IsFalse(string.IsNullOrWhiteSpace(cmdr.AvatarImageUrl));
            Assert.AreEqual("https://inara.cz/cmdr/11668/", cmdr.InaraUrl);

            Assert.IsTrue(cmdr.Ranks.Count > 0);
            foreach (var rank in cmdr.Ranks)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(rank.Name));
                Assert.IsTrue(rank.Value >= 0);
                Assert.IsTrue(rank.Progress >= 0);
            }

            Assert.IsNotNull(cmdr.MainShip);
            Assert.IsFalse(string.IsNullOrWhiteSpace(cmdr.MainShip.Type));
            Assert.IsFalse(string.IsNullOrWhiteSpace(cmdr.MainShip.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(cmdr.MainShip.Ident));

            Assert.IsNotNull(cmdr.Squadron);
            Assert.AreEqual(446, cmdr.Squadron.Id);
            Assert.AreEqual("The Chapterhouse of Inquisition", cmdr.Squadron.Name);
            Assert.IsTrue(cmdr.Squadron.MembersCount > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(cmdr.Squadron.Rank));
            Assert.AreEqual("https://inara.cz/squadron/446/", cmdr.Squadron.InaraUrl);

            Assert.IsNotNull(cmdr.Wing);
            Assert.AreEqual(446, cmdr.Wing.Id);
            Assert.AreEqual("The Chapterhouse of Inquisition", cmdr.Wing.Name);
            Assert.IsTrue(cmdr.Wing.MembersCount > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(cmdr.Wing.Rank));
            Assert.AreEqual("https://inara.cz/squadron/446/", cmdr.Wing.InaraUrl);

            Assert.IsNull(cmdr.OtherNamesFound);
        }

        [TestMethod]
        public async Task PartialMatchUserKeyTest()
        {
            InitialiseInaraTests(true);

            CommanderProfileService cmdrService = CommanderProfileService.Instance(DownloadService.Instance(appName, new UnmeteredConnection()));
            CommanderProfile cmdr = await cmdrService.GetData("Seraphina", 5, identity, new CancellationTokenSource()).ConfigureAwait(false);
            Assert.IsNotNull(cmdr);
            Assert.IsTrue(cmdr.LastUpdated > DateTime.MinValue);

            Assert.IsTrue(cmdr.UserId > 0);
            Assert.IsTrue(cmdr.UserName.Contains("Seraphina", StringComparison.OrdinalIgnoreCase));
            Assert.IsFalse(string.IsNullOrWhiteSpace(cmdr.PreferredAllegiance));
            Assert.IsFalse(string.IsNullOrWhiteSpace(cmdr.InaraUrl));

            Assert.IsTrue(cmdr.Ranks.Count > 0);
            foreach (var rank in cmdr.Ranks)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(rank.Name));
                Assert.IsTrue(rank.Value >= 0);
                Assert.IsTrue(rank.Progress >= 0);
            }

            Assert.IsNotNull(cmdr.OtherNamesFound);
            foreach (string name in cmdr.OtherNamesFound)
            {
                Assert.IsTrue(name.Contains("Seraphina", StringComparison.OrdinalIgnoreCase));
            }
        }

        private void InitialiseInaraTests(bool userApiKey)
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
                               userApiKey ? config["Inara-UserApiKey"] : config["Inara-ApiKey"],
                               bool.Parse(config["Inara-IsDeveloped"]));
            }
        }
    }
}

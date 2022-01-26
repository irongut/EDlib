using EDlib.INARA;
using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class InaraCommanderProfileServiceTests
    {
        [TestMethod]
        public void EmptyProfileTest()
        {
            DateTime beforeTime = DateTime.Now;
            CommanderProfile cmdr = new CommanderProfile();

            Assert.IsTrue(cmdr.LastUpdated > beforeTime);
            Assert.IsTrue(cmdr.LastUpdated < DateTime.Now);
        }

        [TestMethod]
        public async Task CommanderProfile_Get_Test()
        {
            DateTime timestamp = DateTime.Now;
            string inaraData = JsonConvert.SerializeObject(GetCommanderRequestData());

            Mock<IInaraService> mockInara = new();
            mockInara.Setup(x => x.GetData(It.IsAny<InaraHeader>(), It.IsAny<IList<InaraEvent>>(), It.IsAny<DownloadOptions>()).Result).Returns((inaraData, timestamp));

            CommanderProfileService cmdrService = CommanderProfileService.Instance(mockInara.Object);
            CommanderProfile cmdr = await cmdrService.GetData(
                "Irongut",
                5,
                new InaraIdentity("name", "version", "key", true),
                new CancellationTokenSource(),
                true).ConfigureAwait(false);

            Assert.IsNotNull(cmdr);
            Assert.AreEqual(46, cmdr.UserId);
            Assert.AreEqual("Irongut", cmdr.UserName);
            Assert.AreEqual("Empire", cmdr.PreferredAllegiance);
            Assert.AreEqual("Arissa Lavigny-Duval", cmdr.PreferredPower);
            Assert.AreEqual("Shield of Justice", cmdr.PreferredGameRole);
            Assert.AreEqual("https://inara.cz/data/users/151/151725x1547.jpg", cmdr.AvatarImageUrl);
            Assert.AreEqual("https://inara.cz/cmdr/151725/", cmdr.InaraUrl);
            Assert.AreEqual(4, cmdr.Ranks.Count);
            Assert.AreEqual("combat", cmdr.Ranks[0].Name);
            Assert.AreEqual(8, cmdr.Ranks[0].Value);
            Assert.AreEqual(0, cmdr.Ranks[0].Progress);
            Assert.AreEqual(446, cmdr.Squadron.Id);
            Assert.AreEqual("The Chapterhouse of Inquisition", cmdr.Squadron.Name);
            Assert.AreEqual(24, cmdr.Squadron.MembersCount);
            Assert.AreEqual("Senior wingman", cmdr.Squadron.Rank);
            Assert.AreEqual("https://inara.cz/squadron/446/", cmdr.Squadron.InaraUrl);
            Assert.AreEqual(446, cmdr.Wing.Id);
            Assert.AreEqual("The Chapterhouse of Inquisition", cmdr.Wing.Name);
            Assert.AreEqual(24, cmdr.Wing.MembersCount);
            Assert.AreEqual("Senior wingman", cmdr.Wing.Rank);
            Assert.AreEqual("https://inara.cz/squadron/446/", cmdr.Wing.InaraUrl);

            mockInara.Verify(x => x.GetData(It.IsAny<InaraHeader>(), It.IsAny<IList<InaraEvent>>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        private static InaraRequest GetCommanderRequestData()
        {
            List<CommanderRank> ranks = new()
            {
                new CommanderRank() { Name = "combat", Value = 8, Progress = 0 },
                new CommanderRank() { Name = "trade", Value = 8, Progress = 0 },
                new CommanderRank() { Name = "exploration", Value = 8, Progress = 0 },
                new CommanderRank() { Name = "empire", Value = 14, Progress = 0 }
            };
            CommanderSquadron squad = new()
            {
                Id = 446,
                Name = "The Chapterhouse of Inquisition",
                MembersCount = 24,
                Rank = "Senior wingman",
                InaraUrl = "https://inara.cz/squadron/446/"
            };
            CommanderWing wing = new()
            {
                Id = 446,
                Name = "The Chapterhouse of Inquisition",
                MembersCount = 24,
                Rank = "Senior wingman",
                InaraUrl = "https://inara.cz/squadron/446/"
            };
            CommanderProfile profile = new()
            {
                UserId = 46,
                UserName = "Irongut",
                Ranks = ranks,
                PreferredAllegiance = "Empire",
                PreferredPower = "Arissa Lavigny-Duval",
                MainShip = new CommanderShip() { Type = "cutter", Name = "Liberator", Ident = "IR-24C" },
                Squadron = squad,
                Wing = wing,
                PreferredGameRole = "Shield of Justice",
                AvatarImageUrl = "https://inara.cz/data/users/151/151725x1547.jpg",
                InaraUrl = "https://inara.cz/cmdr/151725/"
            };
            return new()
            {
                Header = new InaraHeader { EventStatus = 200 },
                Events = new List<InaraEvent> { new InaraEvent("inaraEvent", profile) { EventStatus = 200 } }
            };
        }
    }
}

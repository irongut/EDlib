using EDlib.GalNet;
using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class GalNetServiceTests
    {
        [TestMethod]
        public async Task GalNet_SingleArticle_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromHours(2);
            string galnetData = JsonConvert.SerializeObject(GetGalNetData());

            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((galnetData, timestamp));

            GalNetService gnService = GalNetService.Instance(mockDownloadService.Object);
            (List<NewsArticle> newsList, DateTime updated) = await gnService.GetData(1, 1, new CancellationTokenSource()).ConfigureAwait(false);

            Assert.AreEqual(1, newsList.Count);
            Assert.AreEqual(timestamp, updated);
            Assert.AreEqual("61eaaf193da577017e06bf44", newsList[0].Id);
            Assert.AreEqual("The Return of the Alexandria", newsList[0].Title);
            Assert.IsFalse(string.IsNullOrWhiteSpace(newsList[0].Body));
            Assert.IsTrue(newsList[0].PublishDateTime > DateTime.Parse("3308/01/01 00:00"));
            Assert.AreEqual("/galnet/uid/61eaaf193da577017e06bf44", newsList[0].Link);
            Assert.AreEqual("Science", newsList[0].Topic);
            Assert.AreEqual(4, newsList[0].Tags.Count);
            Assert.AreEqual("22/01/3308", newsList[0].PublishDate);

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task GalNet_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromHours(2);
            string galnetData = JsonConvert.SerializeObject(GetGalNetData());

            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((galnetData, timestamp));

            GalNetService gnService = GalNetService.Instance(mockDownloadService.Object);
            (List<NewsArticle> newsList, DateTime updated) = await gnService.GetData(1, new CancellationTokenSource()).ConfigureAwait(false);

            Assert.AreEqual(3, newsList.Count);
            Assert.AreEqual(timestamp, updated);
            NewsArticle article = newsList.Find(x => x.Id == "61eaae9fc1b79e7e5c5e7091");
            Assert.AreEqual("61eaae9fc1b79e7e5c5e7091", article.Id);
            Assert.AreEqual("Federal Government Plans for the Future", article.Title);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Body));
            Assert.IsTrue(article.PublishDateTime > DateTime.Parse("3308/01/01 00:00"));
            Assert.AreEqual("/galnet/uid/61eaae9fc1b79e7e5c5e7091", article.Link);
            Assert.AreEqual("Federation", article.Topic);
            Assert.AreEqual(4, article.Tags.Count);
            Assert.AreEqual("21/01/3308", article.PublishDate);

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        [TestMethod]
        public async Task GalNet_AltBoW_Get_Test()
        {
            DateTime timestamp = DateTime.Now - TimeSpan.FromHours(2);
            string galnetData = JsonConvert.SerializeObject(GetGalNetData());

            Mock<IDownloadService> mockDownloadService = new();
            mockDownloadService.Setup(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()).Result).Returns((galnetData, timestamp));

            GalNetService gnService = GalNetService.Instance(mockDownloadService.Object);
            string BoW = LoadBoW("UnitTests.Resources.NewsBoW.json");
            string ignoreBoW = LoadBoW("UnitTests.Resources.NewsFalseBoW.json");
            (List<NewsArticle> newsList, DateTime updated) = await gnService.GetData(1, new CancellationTokenSource(), BoW, ignoreBoW).ConfigureAwait(false);

            Assert.AreEqual(3, newsList.Count);
            Assert.AreEqual(timestamp, updated);
            NewsArticle article = newsList.Find(x => x.Id == "61e95e03eae3b426364c4064");
            Assert.AreEqual("61e95e03eae3b426364c4064", article.Id);
            Assert.AreEqual("Caine-Massey Wins Mining Contract", article.Title);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Body));
            Assert.IsTrue(article.PublishDateTime > DateTime.Parse("3308/01/01 00:00"));
            Assert.AreEqual("/galnet/uid/61e95e03eae3b426364c4064", article.Link);
            Assert.AreEqual("Mining", article.Topic);
            Assert.AreEqual(3, article.Tags.Count);
            Assert.AreEqual("20/01/3308", article.PublishDate);

            mockDownloadService.Verify(x => x.GetData(It.IsAny<string>(), It.IsAny<DownloadOptions>()), Times.Once());
        }

        private static List<NewsArticle> GetGalNetData()
        {
            return new List<NewsArticle>()
            {
                new NewsArticle()
                {
                    Id = "61eaaf193da577017e06bf44",
                    Title = "The Return of the Alexandria",
                    Body = "*Pilots’ Federation ALERT*\r\n\r\nThe Aegis megaship Alexandria, which vanished in hyperspace seven months ago, has been found in the Wregoe TC-X B29-0 system.\r\n\r\nA select group of independent pilots located the Alexandria following a search instigated by Professor Alba Tesreau, former head of research at Aegis.",
                    PublishDateTime = DateTime.Parse("3308/01/22 00:00"),
                    Link = "/galnet/uid/61eaaf193da577017e06bf44"
                },
                new NewsArticle()
                {
                    Id = "61eaae9fc1b79e7e5c5e7091",
                    Title = "Federal Government Plans for the Future",
                    Body = "President Zachary Hudson has delivered a speech to Congress on maintaining security for the Federation in the coming years.\r\n\r\nExcerpts from his address include the following:\r\n\r\n“In the last year, we have excelled at protecting Federal citizens from harm.",
                    PublishDateTime = DateTime.Parse("3308/01/21 00:00"),
                    Link = "/galnet/uid/61eaae9fc1b79e7e5c5e7091"
                },
                new NewsArticle()
                {
                    Id = "61e95e03eae3b426364c4064",
                    Title = "Caine-Massey Wins Mining Contract",
                    Body = "*Pilots’ Federation ALERT*\r\n\r\nThe Caine-Massey corporation has beaten Torval Mining Ltd to retain a mining contract in the Dulos system.\r\n\r\nCompeting campaigns were waged by the two rivals for the rights to supply ore and mined materials to many local subsidiaries.",
                    PublishDateTime = DateTime.Parse("3308/01/20 00:00"),
                    Link = "/galnet/uid/61e95e03eae3b426364c4064"
                },
            };
        }

        private string LoadBoW(string filename)
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(filename))
            {
                using (StreamReader reader = new(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}

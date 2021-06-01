using EDlib.GalNet;
using EDlib.Mock.Platform;
using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class GalNetTests
    {
        [TestMethod]
        public async Task GalNetSingleArticleTest()
        {
            GalNetService gnService = GalNetService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            (List<NewsArticle> newsList, DateTime updated) = await gnService.GetData(1, 1, new CancellationTokenSource()).ConfigureAwait(false);
            Assert.AreEqual(1, newsList.Count);
            Assert.IsTrue(updated > DateTime.MinValue);
            NewsArticle article = newsList[0];
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Id));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Title));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Body));
            Assert.IsTrue(article.PublishDateTime > DateTime.MinValue);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.PublishDate));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Link));
            Assert.IsTrue(article.Link.Contains(article.Id, StringComparison.OrdinalIgnoreCase));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Topic));
            Assert.IsTrue(article.Tags.Count > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Tags[0]));
            Assert.IsTrue(article.ToString().Contains(article.Title));
            Assert.IsTrue(article.ToString().Contains(article.Body));
        }

        [TestMethod]
        public async Task GalNetNewsTest()
        {
            GalNetService gnService = GalNetService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            (List<NewsArticle> newsList, DateTime updated) = await gnService.GetData(1, new CancellationTokenSource()).ConfigureAwait(false);
            Assert.IsTrue(newsList.Count > 1);
            Assert.IsTrue(updated > DateTime.MinValue);
            NewsArticle article = newsList[0];
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Id));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Title));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Body));
            Assert.IsTrue(article.PublishDateTime > DateTime.MinValue);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.PublishDate));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Link));
            Assert.IsTrue(article.Link.Contains(article.Id, StringComparison.OrdinalIgnoreCase));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Topic));
            Assert.IsTrue(article.Tags.Count > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Tags[0]));
            Assert.IsTrue(article.ToString().Contains(article.Title));
            Assert.IsTrue(article.ToString().Contains(article.Body));
        }

        [TestMethod]
        public async Task AlternateBoWTest()
        {
            GalNetService gnService = GalNetService.Instance(DownloadService.Instance("EDlib UnitTests", new UnmeteredConnection()));
            string BoW = LoadBoW("UnitTests.Resources.NewsBoW.json");
            string ignoreBoW = LoadBoW("UnitTests.Resources.NewsFalseBoW.json");
            (List<NewsArticle> newsList, DateTime updated) = await gnService.GetData(1, 1, new CancellationTokenSource(), BoW, ignoreBoW).ConfigureAwait(false);
            Assert.AreEqual(1, newsList.Count);
            Assert.IsTrue(updated > DateTime.MinValue);
            NewsArticle article = newsList[0];
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Id));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Title));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Body));
            Assert.IsTrue(article.PublishDateTime > DateTime.MinValue);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.PublishDate));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Link));
            Assert.IsTrue(article.Link.Contains(article.Id, StringComparison.OrdinalIgnoreCase));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Topic));
            Assert.IsTrue(article.Tags.Count > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Tags[0]));
            Assert.IsTrue(article.ToString().Contains(article.Title));
            Assert.IsTrue(article.ToString().Contains(article.Body));
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

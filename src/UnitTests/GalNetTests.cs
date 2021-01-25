using EDlib.GalNet;
using EDlib.Mock.Platform;
using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
            Assert.IsTrue(newsList.Count == 1);
            Assert.IsTrue(updated > DateTime.MinValue);
            NewsArticle article = newsList[0];
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Title));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Body));
            Assert.IsTrue(article.PublishDateTime > DateTime.MinValue);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.PublishDate));
            Assert.IsTrue(article.Id > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.FDImageName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Slug));
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
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Title));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Body));
            Assert.IsTrue(article.PublishDateTime > DateTime.MinValue);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.PublishDate));
            Assert.IsTrue(article.Id > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.FDImageName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Slug));
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Topic));
            Assert.IsTrue(article.Tags.Count > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(article.Tags[0]));
            Assert.IsTrue(article.ToString().Contains(article.Title));
            Assert.IsTrue(article.ToString().Contains(article.Body));
        }
    }
}

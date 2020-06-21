using EDlib.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TopicsTests
    {
        [TestMethod]
        public void NewsBoWTest()
        {
            TopicsList list = new TopicsList("EDlib.GalNet.Resources.NewsBoW.json");
            Assert.IsTrue(list.Topics.Count > 0);
            Assert.IsTrue(list.Topics[0].Terms.Count > 0);
        }

        [TestMethod]
        public void NewsFalseBoWTest()
        {
            TopicsList list = new TopicsList("EDlib.GalNet.Resources.NewsFalseBoW.json");
            Assert.IsTrue(list.Topics.Count > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(list.Topics[0].Name));
            Assert.IsTrue(list.Topics[0].Terms.Count > 0);
            Assert.AreEqual(list.Topics[0].Count, 0);
        }
    }
}

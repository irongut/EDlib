using EDlib.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class TopicTests
    {
        [TestMethod]
        public void TopicTest()
        {
            Topic topic = new("test", new List<string> { "test", "demo", "sample", "trial"});
            Assert.IsTrue(topic.Name.Equals("test", StringComparison.OrdinalIgnoreCase));
            Assert.IsTrue(topic.Terms.Contains("test"));
            Assert.IsTrue(topic.Terms.Contains("demo"));
            Assert.IsTrue(topic.Terms.Contains("sample"));
            Assert.IsTrue(topic.Terms.Contains("trial"));
        }
    }
}

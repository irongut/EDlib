using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class HashTests
    {
        [TestMethod]
        public void SHA256Test()
        {
            string edHash = Sha256Helper.GenerateHash("Elite-Dangerous");
            Assert.AreEqual("4d3a789483ba9d14595680953fd85e100515c09adec88100393e80099bec2fd7", edHash);
            string tsHash = Sha256Helper.GenerateHash("Taranis&Software");
            Assert.AreEqual("ee287aa1d52fc1d3e8ed566fec4e75f6781312a68992a818dfbcf26fcbf54ed6", tsHash);
        }
    }
}

using EDlib.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace UnitTests
{
    [TestClass]
    public class DownloadOptionsTests
    {
        [TestMethod]
        public void DefaultOptions_Test()
        {
            DownloadOptions options = new DownloadOptions();
            Assert.IsNull(options.CancelToken);
            Assert.AreEqual(TimeSpan.FromTicks(0), options.Expiry);
            Assert.IsFalse(options.IgnoreCache);
        }

        [TestMethod]
        public void CancelToken_Test()
        {
            DownloadOptions options = new DownloadOptions(new CancellationTokenSource());
            Assert.IsNotNull(options.CancelToken);
            Assert.AreEqual(TimeSpan.FromTicks(0), options.Expiry);
            Assert.IsFalse(options.IgnoreCache);
        }

        [TestMethod]
        public void Expiry_Test()
        {
            DownloadOptions options = new DownloadOptions(TimeSpan.FromMinutes(5));
            Assert.IsNull(options.CancelToken);
            Assert.AreEqual(TimeSpan.FromMinutes(5), options.Expiry);
            Assert.IsFalse(options.IgnoreCache);
        }

        [TestMethod]
        public void CancelToken_Expiry_Test()
        {
            DownloadOptions options = new DownloadOptions(new CancellationTokenSource(), TimeSpan.FromMinutes(5));
            Assert.IsNotNull(options.CancelToken);
            Assert.AreEqual(TimeSpan.FromMinutes(5), options.Expiry);
            Assert.IsFalse(options.IgnoreCache);
        }

        [TestMethod]
        public void Expiry_IgnoreCache_Test()
        {
            DownloadOptions options = new DownloadOptions(TimeSpan.FromMinutes(5), true);
            Assert.IsNull(options.CancelToken);
            Assert.AreEqual(TimeSpan.FromMinutes(5), options.Expiry);
            Assert.IsTrue(options.IgnoreCache);
        }

        [TestMethod]
        public void CancelToken_Expiry_IgnoreCache_Test()
        {
            DownloadOptions options = new DownloadOptions(new CancellationTokenSource(), TimeSpan.FromMinutes(5), true);
            Assert.IsNotNull(options.CancelToken);
            Assert.AreEqual(TimeSpan.FromMinutes(5), options.Expiry);
            Assert.IsTrue(options.IgnoreCache);
        }
    }
}

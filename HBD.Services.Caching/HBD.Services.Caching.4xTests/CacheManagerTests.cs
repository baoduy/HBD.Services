#region using

using System;
using System.Threading;
using HBD.Services.Caching.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace HBD.Services.Caching._4xTests
{
    [TestClass]
    public class CacheManagerTests
    {
        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void TestDefault_Instance()
        {
            Assert.IsNotNull(CacheManager.Default);
            Assert.IsInstanceOfType(CacheManager.Default, typeof(MemoryCacheProvider));
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void TestDefault_Region_Instance()
        {
            CacheManager.Default.Set("123", new object());
            Assert.IsNotNull(CacheManager.Default.Get("123"));
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void TestDefault_Expiry_Instance()
        {
            //Cache 5 secs
            CacheManager.Default.Set("123", new object(), new TimeSpan(0, 0, 2));
            Assert.IsNotNull(CacheManager.Default.Get("123"));

            //Delay 6 secs Cache item should be null.
            Thread.Sleep(new TimeSpan(0, 0, 3));
            Assert.IsNull(CacheManager.Default.Get("123"));
        }

        [TestMethod]
        public void SetProvider1_Test()
        {
            var p = new MemoryCacheProvider();
            CacheManager.SetProvider(p);

            Assert.AreEqual(p, CacheManager.Default);
            Assert.IsTrue(CacheManager.IsProviderSet);
        }

        [TestMethod]
        public void SetProvider2_Test()
        {
            var p = new MemoryCacheProvider();
            CacheManager.SetProvider(() => p);

            Assert.AreEqual(p, CacheManager.Default);
            Assert.IsTrue(CacheManager.IsProviderSet);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetProvider_NullException_Test()
        {
            CacheManager.SetProvider((ICacheProvider)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetProvider_NullException2_Test()
        {
            CacheManager.SetProvider((Func<ICacheProvider>)null);
        }

    }
}
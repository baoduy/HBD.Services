#region using

using System;
using HBD.Services.Caching.StTests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HBD.Services.Caching;
using HBD.Services.Caching.Providers;
using HBD.Framework;
using FluentAssertions;

#endregion

namespace HBD.Services.Caching.StTests.Providers
{
    [TestClass]
    public class MemoryCacheProviderTests
    {
        private MemoryCacheProvider _service;

        [TestInitialize]
        public void Initializer()
        {
            _service = new MemoryCacheProvider(new TimeSpan(0, 0, 10));
        }

        [TestCleanup]
        public void CleanUp()
        {
            _service.Dispose();
        }

        private HasAttributeTestClass3 AddToCache(string key)
        {
            var obj = new HasAttributeTestClass3 { Prop3 = "Testing Adding" };
            _service.Set(key, obj);
            return obj;
        }

        private HasAttributeTestClass3 UpdateToCache(string key)
        {
            var obj = new HasAttributeTestClass3 { Prop3 = "Testing Updating" };
            _service.Set(key, obj);
            return obj;
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void Check_ExpiredTime()
        {
            Assert.AreEqual(CacheManager.Default.PropertyValue("DefaultExpiration") , new TimeSpan(4,0,0));
            Assert.AreEqual(_service.PropertyValue("DefaultExpiration"), new TimeSpan(0, 0, 10));
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void Verify_CacheManager()
        {
            Assert.IsNotNull(CacheManager.Default);
            Assert.IsInstanceOfType(CacheManager.Default, typeof(MemoryCacheProvider));
        }


        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void AddOrUpdateTest()
        {
            //Add
            const string key = "test";
            var obj = AddToCache(key);

            var cached = _service.Get(key);
            Assert.IsNotNull(cached);
            Assert.IsTrue(_service.Get<HasAttributeTestClass3>(key).Prop3 == obj.Prop3);
            Assert.IsNull(_service.Get<HasAttributeTestClass2>(key));

            //Update
            obj = UpdateToCache(key);
            _service.Set(key, obj);

            cached = _service.Get(key);
            Assert.IsNotNull(cached);
            Assert.IsTrue(_service.Get<HasAttributeTestClass3>(key).Prop3 == obj.Prop3);
            Assert.IsNull(_service.Get<HasAttributeTestClass2>(key));
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void DisposeTest()
        {
            _service.Dispose();
            var obj = AddToCache("test");
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void RemoveTest()
        {
            const string key = "test";
            var obj = AddToCache(key);
            Assert.IsTrue(_service.Contains(key));
            _service.Remove(key);
            Assert.IsFalse(_service.Contains(key));
        }

        [TestMethod]
        [TestCategory("Fw.Cache.Services")]
        public void GetAllKey_Test()
        {
            _service.Set("1", "1");
            _service.Set("1", "2");
            _service.Set("1", "3");

            _service.Keys.Count.Should().Be(1);

            _service.Set("2", "2");
            _service.Set("3", "3");

            _service.Keys.Count.Should().Be(3);
        }
    }
}
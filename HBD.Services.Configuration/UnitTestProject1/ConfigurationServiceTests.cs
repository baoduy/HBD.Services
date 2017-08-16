using System;
using System.Threading;
using FluentAssertions;
using HBD.Services.Configuration.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using HBD.Services.Caching.Providers;
using System.Threading.Tasks;

namespace HBD.Services.Configuration.StTests
{
    [TestClass]
    public class ConfigurationServiceTests
    {
        [TestInitialize]
        public void Setup()
        { var b = BootStrapper.Default; }

        [TestMethod]
        public void Load_Adapters_From_Mef()
        {
            BootStrapper.Default.Container.GetExports<IConfigAdapter>()
                .Should().NotBeEmpty();

            var config = new ConfigurationService();
            config.Adapters.Count.Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        public void Load_Config_From_Adapter()
        {
            var config = new ConfigurationService();
            var val = config.Get<TestItem>();
            val.Should().NotBeNull();

            var a = BootStrapper.Default.Container.GetExport<TestConfigAdapter>();
            a.IsChangedCalled.Should().Be(0);//Because the val is null
            a.LoadCalled.Should().Be(1);

            val = config.Get<TestItem>();//Get again.
            val.Should().NotBeNull();
            a.IsChangedCalled.Should().Be(1);
            a.LoadCalled.Should().Be(1);
        }

        [TestMethod]
        public void ReLoad_When_Cache_Expired()
        {
            var config = new ConfigurationService();
            var val = config.Get<TestItem>();
            val.Should().NotBeNull();

            Thread.Sleep(new TimeSpan(0, 0, 3));

            val = config.Get<TestItem>();
            val.Should().NotBeNull();

            var a = BootStrapper.Default.Container.GetExport<TestConfigAdapter>();
            a.LoadCalled.Should().BeGreaterOrEqualTo(2);
        }

        [TestMethod]
        public void ReLoad_When_HasChanged()
        {
            var config = new ConfigurationService();
            var val = config.Get<TestItem>();
            val.Should().NotBeNull();

            var a = BootStrapper.Default.Container.GetExport<TestConfigAdapter>();
            a.HasChanged = true;

            val = config.Get<TestItem>();
            val.Should().NotBeNull();

            a.LoadCalled.Should().BeGreaterOrEqualTo(2);
        }

        [TestMethod]
        public void Load_Json_File()
        {
            var config = new ConfigurationService();

            var t = config.Get<TestItem>(adapters => adapters.FirstOrDefault(a => a is TestJsonConfigAdapter));

            t.Should().NotBeNull();
            t.Name.Should().NotBeNullOrEmpty();
            t.Email.Should().Be("drunkcoding@outlook.com");
        }

        [TestMethod]
        public void CreateByConfigurationBuilder()
        {
            var service = new ConfigurationServiceBuilder()
                .WithAdapters(new TestConfigAdapter())
                .WithCacheProvider(new MemoryCacheProvider())
                .WithServiceLocator(HBD.ServiceLocator.Current)
                .Build();

            var t = service.Get<TestItem>(adapters => adapters.FirstOrDefault(a => a is TestJsonConfigAdapter));
            t.Should().NotBeNull();
            t.Name.Should().NotBeNullOrEmpty();
            t.Email.Should().Be("drunkcoding@outlook.com");
        }

        [TestMethod]
        public async Task LoadAsync()
        {
            var config = new ConfigurationService();

            var t = await config.GetAsync<TestItem>(adapters => adapters.FirstOrDefault(a => a is TestJsonConfigAdapter));

            t.Should().NotBeNull();
            t.Name.Should().NotBeNullOrEmpty();
            t.Email.Should().Be("drunkcoding@outlook.com");
        }

        [TestMethod]
        public async Task SaveAsync()
        {
            var config = new ConfigurationService();
            var t = config.Get<TestItem>();

            t.Email = "baoduy2412@outlook.com";

            await config.SaveAsync(t, adapters => adapters.FirstOrDefault(a => a is TestJsonConfigAdapter));

            var ta = BootStrapper.Default.Container.GetExport<TestJsonConfigAdapter>();

            var t1 = await ta.LoadAsync();

            t1.Email.Should().Be(t.Email);
        }

        [TestMethod]
        public void Ignore_Service_Locator()
        {
            var service = new ConfigurationServiceBuilder()
                .IgnoreServiceLocator()
                .Build();

            var t = service.Get<TestItem>();
            t.Should().BeNull();
        }

        [TestMethod]
        public void Ignore_Cache()
        {
            var a = BootStrapper.Default.Container.GetExport<TestConfigAdapter>();
            a.ResetCount();

            var service = new ConfigurationServiceBuilder()
                .WithAdapters(a)
                .IgnoreCaching()
                .IgnoreServiceLocator()
                .Build();

            service.Get<TestItem>();
            service.Get<TestItem>();

            a.LoadCalled.Should().BeGreaterOrEqualTo(2);
        }

        [TestMethod]
        public void Register_Json_File()
        {
            var service = new ConfigurationServiceBuilder()
                 .RegisterFile<TestItem>("TestData\\json1.json")
                 .IgnoreCaching()
                 .IgnoreServiceLocator()
                 .Build();

            var t = service.Get<TestItem>();
            t.Should().NotBeNull();
        }

        [TestMethod]
        public void Register_Xml_File()
        {
            var service = new ConfigurationServiceBuilder()
                 .RegisterFile<TestItem>(new FileFinder().Find("XMLFile1.xml"))
                 .IgnoreCaching()
                 .IgnoreServiceLocator()
                 .Build();

            var t = service.Get<TestItem>();
            t.Should().NotBeNull();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Register_Exception_1()
        {
            new ConfigurationServiceBuilder()
                 .RegisterFile<TestItem>(new FileFinder().Find("XMLFile1.abc"))
                 .IgnoreCaching()
                 .IgnoreServiceLocator()
                 .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Register_Exception_2()
        {
           new ConfigurationServiceBuilder()
                 .RegisterFile<TestItem>("XMLFile1.abc")
                 .IgnoreCaching()
                 .IgnoreServiceLocator()
                 .Build();
        }
    }
}

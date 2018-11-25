using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace HBD.Services.Configuration.StTests
{
    [TestClass]
    public class ConfigurationServiceTests
    {
        [TestMethod]
        public async Task Load_Config_From_Adapter()
        {
            var config = new ConfigurationService(
                new ConfigurationOptions().WithAdapters(new TestConfigAdapter())
                );

            var val = await config.GetAsync<TestItem>();
            val.Should().NotBeNull();

            var a = config.Adapters.First() as TestConfigAdapter;
            a.IsChangedCalled.Should().Be(0);//Because the val is null
            a.LoadCalled.Should().Be(1);

            a.HasChanged();
            val = await config.GetAsync<TestItem>();//Get again.
            val.Should().NotBeNull();
            a.IsChangedCalled.Should().Be(1);
            a.LoadCalled.Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        public async Task ReLoad_When_Cache_Expired()
        {
            var config = new ConfigurationService(new ConfigurationOptions()
                .WithAdapters(new TestConfigAdapter())
                .WithExpiration(new TimeSpan(0, 0, 1))
            );
            var val = await config.GetAsync<TestItem>();
            val.Should().NotBeNull();

            Thread.Sleep(new TimeSpan(0, 0, 3));

            val = await config.GetAsync<TestItem>();
            val.Should().NotBeNull();

            var a = config.Adapters.First() as TestConfigAdapter;
            a.LoadCalled.Should().BeGreaterOrEqualTo(2);
        }

        [TestMethod]
        public async Task ReLoad_When_HasChanged()
        {
            var config = new ConfigurationService(new ConfigurationOptions()
                .WithAdapters(new TestConfigAdapter())
            );
            var val = await config.GetAsync<TestItem>();
            val.Should().NotBeNull();

            var a = config.Adapters.First() as TestConfigAdapter;
            a.IsChanged = true;

            val = await config.GetAsync<TestItem>();
            val.Should().NotBeNull();

            a.LoadCalled.Should().BeGreaterOrEqualTo(2);
        }

        [TestMethod]
        public async Task Load_Json_File()
        {
            var config = new ConfigurationService(new ConfigurationOptions()
                .WithAdapters(new TestJsonConfigAdapter())
            );

            var t = await config.GetAsync<TestItem>();

            t.Should().NotBeNull();
            t.Name.Should().NotBeNullOrEmpty();
            t.Email.Should().Be("drunkcoding@outlook.com");
        }

        [TestMethod]
        public async Task CreateByConfigurationBuilder()
        {
            var service = new ConfigurationService(
                new ConfigurationOptions()
                .WithAdapters(new TestConfigAdapter())
                );

            var t = await service.GetAsync<TestItem>();
            t.Should().NotBeNull();
            t.Name.Should().NotBeNullOrEmpty();
            t.Email.Should().Be("drunkcoding@outlook.com");
        }

        [TestMethod]
        public async Task LoadAsync()
        {
            var config = new ConfigurationService(  new ConfigurationOptions()
                .WithAdapters(new TestConfigAdapter())
                );

            var t = await config.GetAsync<TestItem>();

            t.Should().NotBeNull();
            t.Name.Should().NotBeNullOrEmpty();
            t.Email.Should().Be("drunkcoding@outlook.com");
        }

        //[TestMethod]
        //public async Task SaveAsync()
        //{
        //    var config = new ConfigurationService();
        //    var t = config.Get<TestItem>();

        //    t.Email = "baoduy2412@outlook.com";

        //    await config.SaveAsync(t, adapters => adapters.FirstOrDefault(a => a is TestJsonConfigAdapter));

        //    var ta = BootStrapper.Default.Container.GetExport<TestJsonConfigAdapter>();

        //    var t1 = await ta.LoadAsync();

        //    t1.Email.Should().Be(t.Email);
        //}

        //[TestMethod]
        //public void Ignore_Service_Locator()
        //{
        //    var service = new ConfigurationServiceBuilder()
        //        .IgnoreServiceLocator()
        //        .Build();

        //    var t = service.Get<TestItem>();
        //    t.Should().BeNull();
        //}

        //[TestMethod]
        //public void Ignore_Cache()
        //{
        //    var a = BootStrapper.Default.Container.GetExport<TestConfigAdapter>();
        //    a.ResetCount();

        //    var service = new ConfigurationServiceBuilder()
        //        .WithAdapters(a)
        //        .IgnoreCaching()
        //        .IgnoreServiceLocator()
        //        .Build();

        //    service.Get<TestItem>();
        //    service.Get<TestItem>();

        //    a.LoadCalled.Should().BeGreaterOrEqualTo(2);
        //}

        //[TestMethod]
        //public void Register_Json_File()
        //{
        //    var service = new ConfigurationServiceBuilder()
        //         .RegisterFile<TestItem>("TestData\\json1.json")
        //         .IgnoreCaching()
        //         .IgnoreServiceLocator()
        //         .Build();

        //    var t = service.Get<TestItem>();
        //    t.Should().NotBeNull();
        //}

        //[TestMethod]
        //public void Register_Xml_File()
        //{
        //    var service = new ConfigurationServiceBuilder()
        //         .RegisterFile<TestItem>(new FileFinder().Find("XMLFile1.xml"))
        //         .IgnoreCaching()
        //         .IgnoreServiceLocator()
        //         .Build();

        //    var t = service.Get<TestItem>();
        //    t.Should().NotBeNull();
        //}

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Register_Exception_1()
        {
            new ConfigurationOptions()
                .RegisterFile<TestItem>(new FileFinder().Find("XMLFile1.abc"))
                .IgnoreCaching();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Register_Exception_2()
        {
            new ConfigurationOptions()
                .RegisterFile<TestItem>("XMLFile1.abc")
                .IgnoreCaching();
        }
    }
}

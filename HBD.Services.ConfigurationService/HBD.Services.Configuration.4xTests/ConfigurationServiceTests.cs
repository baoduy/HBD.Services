using System;
using System.Threading;
using FluentAssertions;
using HBD.Services.Configuration.Adapters;
using HBD.Services.Configuration.StTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Configuration._4xTests
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

            var a = BootStrapper.Default.Container.GetExportedValue<TestConfigAdapter>();
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

            var a = BootStrapper.Default.Container.GetExportedValue<TestConfigAdapter>();
            a.LoadCalled.Should().BeGreaterOrEqualTo(2);
        }

        [TestMethod]
        public void ReLoad_When_HasChanged()
        {
            var config = new ConfigurationService();
            var val = config.Get<TestItem>();
            val.Should().NotBeNull();

            var a = BootStrapper.Default.Container.GetExportedValue<TestConfigAdapter>();
            a.HasChanged = true;

            val = config.Get<TestItem>();
            val.Should().NotBeNull();

            a.LoadCalled.Should().BeGreaterOrEqualTo(2);
        }
    }
}

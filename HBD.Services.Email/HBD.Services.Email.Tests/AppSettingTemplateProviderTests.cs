using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using HBD.Services.Email.Providers;
using HBD.Services.Email.Templates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace HBD.Services.Email.Tests
{
    [TestClass]
    public class AppSettingTemplateProviderTests
    {
        [TestMethod]
        public async Task ConfigTemplateAsync()
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("TestData/appsettings.json")
               .Build();

            var section = config.GetSection("Smtp");

            var s = new ServiceCollection()
                .Configure<EmailTemplateSection>(section)
                .AddEmailService(op => op.EmailTemplateFromConfiguration())
                .BuildServiceProvider();

            var p = s.GetService<IEmailTemplateProvider>();
            p.Should().NotBeNull();
            p.Should().BeOfType<AppSettingTemplateProvider>();
            (await p.GetTemplate("Duy2")).Should().NotBeNull();
        }
    }
}

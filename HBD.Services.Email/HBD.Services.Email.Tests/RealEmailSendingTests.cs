using System.IO;
using HBD.Services.Email.Templates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Email.Tests
{
    [TestClass]
    public class RealEmailSendingTests
    {
        [TestMethod]
        public async System.Threading.Tasks.Task SendEmailToHBDAsync()
        {
            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("TestData/appsettings.json")
                 .Build();

            var s = new ServiceCollection()
                .Configure<EmailTemplateSection>(config.GetSection("Smtp"))
                .AddEmailService(op => op.EmailTemplateFromConfiguration())
                .BuildServiceProvider();

            var sender = s.GetService<IEmailService>();

            await sender.SendAsync("Duy1", new object[] { new { Name = "Duy" } });
        }
    }
}

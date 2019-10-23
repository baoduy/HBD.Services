using HBD.Services.Email.Templates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HBD.Services.Email.Tests
{
    [TestClass]
    public class RealEmailSendingTests
    {
        #region Methods

        [TestMethod]
        public async System.Threading.Tasks.Task SendEmailToHBDAsync()
        {
            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("TestData/appsettings.json")
                 .Build();

            var s = new ServiceCollection()
                .AddEmailService(op => op.FromConfiguration(config.GetSection("Smtp")))
                .BuildServiceProvider();

            var sender = s.GetService<IEmailService>();

            await sender.SendAsync("Duy1", new object[] { new { Name = "Duy" } });
        }

        #endregion Methods
    }
}
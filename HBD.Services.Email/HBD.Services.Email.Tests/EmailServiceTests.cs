using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FluentAssertions;
using HBD.Services.Email.Configurations;
using HBD.Services.Email.Providers;
using HBD.Services.Transformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using netDumbster.smtp;

namespace HBD.Services.Email.Tests
{
    [TestClass]
    public class EmailServiceTests
    {
        private static SimpleSmtpServer _smtpServer;
        [AssemblyInitialize]
        public static void AppDomainSetup(TestContext context)
            => _smtpServer = SimpleSmtpServer.Start(25);

        [AssemblyCleanup]
        public static void Cleanup() => _smtpServer.Stop();

        [TestMethod]
        public async Task Test_MailMessageProvider_ByTemplate()
        {
            var emailTemplateProviderMoq = new Mock<IEmailTemplateProvider>();
            emailTemplateProviderMoq.Setup(e => e.GetTemplate(It.IsAny<string>()))
                .ReturnsAsync(new EmailTemplate("Duy")
                {
                    ToEmails = "[DuyEmail],{HBDEmail};hoang@hbd.com",
                    Body = "Hello [Name]",
                    Subject = "Hi, [Name]"
                }).Verifiable();

            using (var mailProvider = new MailMessageProvider(emailTemplateProviderMoq.Object, new Transformer()))
            {

                var mail = await mailProvider.GetMailMessageAsync("Duy",
                    new object[]
                    {
                        new
                        {
                            DuyEmail = "drunkcoding@outlook.net",
                            HBDEmail = "duy@hbd.net",
                            Name = "Duy Hoang"
                        }
                    });

                mail.Should().NotBeNull();
                mail.To.Count.Should().Be(3);
                mail.To.First().Address.Should().Be("drunkcoding@outlook.net");
                mail.To.Last().Address.Should().Be("hoang@hbd.com");
                mail.Subject.Should().Contain("Duy Hoang");
                mail.Body.Should().Contain("Duy Hoang");
            }


            emailTemplateProviderMoq.VerifyAll();
        }

        [TestMethod]
        public async Task Test_SendEmail_ByTemplate()
        {
            _smtpServer.ClearReceivedEmail();

            var emailTemplateProviderMoq = new Mock<IEmailTemplateProvider>();
            emailTemplateProviderMoq.Setup(e => e.GetTemplate(It.IsAny<string>())).ReturnsAsync(new EmailTemplate("Duy")
            {
                ToEmails = "[DuyEmail],{HBDEmail},hoang@hbd.com",
                Body = "Hello [Name]",
            });

            using (var mailService = new EmailService(
                new MailMessageProvider(emailTemplateProviderMoq.Object, new Transformer()), new EmailOptions
                {
                    FromEmailAddress = new MailAddress("drunkcoding@outlook.net"),
                    SmtpClientFactory = () => new SmtpClient("localhost", 25)
                }))
            {



                await mailService.SendAsync("Duy", new object[]
                {
                    new
                    {
                        DuyEmail = "drunkcoding@outlook.net",
                        HBDEmail = "duy@hbd.net",
                        Name = "Duy Hoang"
                    }
                });
            }

            _smtpServer.ReceivedEmailCount.Should().BeGreaterOrEqualTo(1);
        }
    }
}

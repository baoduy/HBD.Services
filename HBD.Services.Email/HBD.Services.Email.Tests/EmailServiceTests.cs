using FluentAssertions;
using HBD.Services.Email.Providers;
using HBD.Services.Email.Templates;
using HBD.Services.Transformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using netDumbster.smtp;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HBD.Services.Email.Tests
{
    [TestClass]
    public class EmailServiceTests
    {
        #region Fields

        private static SimpleSmtpServer _smtpServer;

        #endregion Fields

        #region Methods

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

            using (var mailProvider = new MailMessageProvider(emailTemplateProviderMoq.Object, new TransformerService()))
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
        public async Task Test_EmptyAddress_ShouldBe_Ignore()
        {
            var emailTemplateProviderMoq = new Mock<IEmailTemplateProvider>();
            emailTemplateProviderMoq.Setup(e => e.GetTemplate(It.IsAny<string>()))
                .ReturnsAsync(new EmailTemplate("Duy")
                {
                    ToEmails = "[DuyEmail],{HBDEmail};hoang@hbd.com",
                    Body = "Hello [Name]",
                    Subject = "Hi, [Name]"
                }).Verifiable();

            using (var mailProvider = new MailMessageProvider(emailTemplateProviderMoq.Object, new TransformerService()))
            {
                var mail = await mailProvider.GetMailMessageAsync("Duy",
                    new object[]
                    {
                        new
                        {
                            DuyEmail = "",
                            HBDEmail = "duy@hbd.net",
                            Name = "Duy Hoang"
                        }
                    });

                mail.Should().NotBeNull();
                mail.To.Count.Should().Be(2);
            }

            emailTemplateProviderMoq.VerifyAll();
        }

        [TestMethod]
        public async Task Test_TransformTheSameTemplate_ButDifferentData_EmailDifference()
        {
            var emailTemplateProviderMoq = new Mock<IEmailTemplateProvider>();
            emailTemplateProviderMoq.Setup(e => e.GetTemplate(It.IsAny<string>()))
                .ReturnsAsync(new EmailTemplate("Duy")
                {
                    ToEmails = "[DuyEmail],{HBDEmail};hoang@hbd.com",
                    Body = "Hello [Name]",
                    Subject = "Hi, [Name]"
                }).Verifiable();

            using (var mailProvider = new MailMessageProvider(emailTemplateProviderMoq.Object, new TransformerService()))
            {
                var mail1 = await mailProvider.GetMailMessageAsync("Duy",
                    new object[]
                    {
                        new
                        {
                            DuyEmail = "a@outlook.net",
                            HBDEmail = "h@hbd.net",
                            Name = "Duy"
                        }
                    });

                var mail2 = await mailProvider.GetMailMessageAsync("Duy",
                  new object[]
                  {
                        new
                        {
                            DuyEmail = "b@outlook.net",
                            HBDEmail = "b@hbd.net",
                            Name = "Hoang"
                        }
                  });

                mail1.To.First().Address.Should().NotBe(mail2.To.First().Address);
                mail1.Subject.Should().NotBe(mail2.Subject);
                mail1.Body.Should().NotBe(mail2.Body);
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

            using (var mailService = new SmtpEmailService(
                new MailMessageProvider(emailTemplateProviderMoq.Object, new TransformerService()), new SmtpEmailOptions
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

        [TestMethod]
        public async Task Test_SendEmail_ByTemplate_WithDictionnary()
        {
            _smtpServer.ClearReceivedEmail();

            var emailTemplateProviderMoq = new Mock<IEmailTemplateProvider>();
            emailTemplateProviderMoq.Setup(e => e.GetTemplate(It.IsAny<string>())).ReturnsAsync(new EmailTemplate("Duy")
            {
                ToEmails = "[DuyEmail],{HBDEmail},hoang@hbd.com",
                Body = "Hello [Name]",
            });

            using (var mailService = new SmtpEmailService(
                new MailMessageProvider(emailTemplateProviderMoq.Object, new TransformerService()), new SmtpEmailOptions
                {
                    FromEmailAddress = new MailAddress("drunkcoding@outlook.net"),
                    SmtpClientFactory = () => new SmtpClient("localhost", 25)
                }))
            {
                await mailService.SendAsync("Duy", new object[]
                {
                    new Dictionary<string,object>
                    {
                        ["DuyEmail"] = "drunkcoding@outlook.net",
                        ["HBDEmail"] = "duy@hbd.net",
                        ["Name"] = "Duy Hoang"
                    }
                });
            }

            _smtpServer.ReceivedEmailCount.Should().BeGreaterOrEqualTo(1);
        }

        #endregion Methods
    }
}
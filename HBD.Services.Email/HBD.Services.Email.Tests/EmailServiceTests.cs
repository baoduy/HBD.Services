using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FluentAssertions;
using HBD.Framework.IO;
using HBD.Services.Email.Configurations;
using HBD.Services.Email.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Email.Tests
{
    [TestClass]
    public class EmailServiceTests
    {
        private static readonly string Location = Path.GetFullPath("TestEmail");

        [TestInitialize]
        public void Setup()
        {
            Directory.CreateDirectory(Location);
            DirectoryEx.DeleteFiles(Location);
        }

        [TestMethod]
        public void EmailService_Duplicated_Test()
        {
            using (var t = new EmailService(op =>
            {
                op.FromEmailAddress = new MailAddress("system@hbd.com");

                op.Templates.Add(new EmailTemplate("Duy")
                {
                    ToEmails = "baoduy2412@yahoo.com",
                    Subject = "S",
                    Body = "123"
                });

                op.Templates.Add(new EmailTemplate("Duy")
                {
                    ToEmails = "baoduy2412@yahoo.com",
                    Subject = "S",
                    Body = "123"
                });
            }))
            {
                Action a = () =>
                 {
                     var c = t.EmailInfos.Count;
                 };

                a.Should().Throw<TemplateDuplicatedException>();
            }
        }

        [TestMethod]
        public void EmailService_EmailInfos_Test()
        {
            using (var t = new EmailService(op =>
            {
                op.FromEmailAddress = new MailAddress("system@hbd.com");

                op.Templates.Add(new EmailTemplate("Duy")
                {
                    ToEmails = "baoduy2412@yahoo.com",
                    Subject = "S",
                    Body = "123"
                });
            }))
                t.EmailInfos.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void EmailService_JsonFile_Test()
        {
            using (var t = new EmailService(op => { op.FromEmailAddress = new MailAddress("system@hbd.com");
                op.TemplateJsonFile = "TestData\\Emails.json"; }))
                t.EmailInfos.Should()
                .NotBeNullOrEmpty();
        }

        [TestMethod]
        public void EmailService_Sent_Test()
        {
            using (var smtp = new EmailService(op =>
            {
                op.FromEmailAddress = new MailAddress("system@hbd.com");

                op.TemplateJsonFile = "TestData\\Emails.json";
                op.SmtpClientFactory = () => new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    DeliveryFormat = SmtpDeliveryFormat.International,
                    PickupDirectoryLocation =Path.GetFullPath( Location)
                };
            }))
            {
                smtp.Send("Duy1", new object[]
                {
                    new
                    {
                        Duy="hoangbaoduy@gmail.com",
                        OtherEmail="abc@123.com",
                        Name="Duy"
                    }
                });

                Directory.GetFiles(Location).Any().Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task EmailService_SentAync_Test()
        {
            using (var smtp = new EmailService(op =>
            {
                op.FromEmailAddress = new MailAddress("system@hbd.com");

                op.TemplateJsonFile = "TestData\\Emails.json";
                op.SmtpClientFactory = () => new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    DeliveryFormat = SmtpDeliveryFormat.International,
                    PickupDirectoryLocation =Path.GetFullPath( Location)
                };
            }))
            {
                await smtp.SendAsync("Duy1", new object[]
                {
                    new
                    {
                        Duy="hoangbaoduy@gmail.com",
                        OtherEmail="abc@123.com",
                        Name="Duy"
                    }
                });

                Directory.GetFiles(Location).Any().Should().BeTrue();
            }
        }

        [TestMethod]
        public void EmailService_Sent_MailMessage_Test()
        {
            using (var smtp = new EmailService(op =>
            {
                op.FromEmailAddress = new MailAddress("system@hbd.com");

                op.TemplateJsonFile = "TestData\\Emails.json";
                op.SmtpClientFactory = () => new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    DeliveryFormat = SmtpDeliveryFormat.International,
                    PickupDirectoryLocation =Path.GetFullPath( Location)
                };
            }))
            {
                smtp.Send(new MailMessage{To = { new MailAddress("duy@abc.com")}});

                Directory.GetFiles(Location).Any().Should().BeTrue();
            }
        }

        [TestMethod]
        public async Task EmailService_SentAync_MailMessage_Test()
        {
            using (var smtp = new EmailService(op =>
            {
                op.FromEmailAddress = new MailAddress("system@hbd.com");

                op.TemplateJsonFile = "TestData\\Emails.json";
                op.SmtpClientFactory = () => new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    DeliveryFormat = SmtpDeliveryFormat.International,
                    PickupDirectoryLocation =Path.GetFullPath( Location)
                };
            }))
            {
                await smtp.SendAsync(new MailMessage{To = { new MailAddress("duy@abc.com")}});

                Directory.GetFiles(Location).Any().Should().BeTrue();
            }
        }
    }
}

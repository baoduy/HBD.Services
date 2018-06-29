using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HBD.Framework.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Email._4xTests
{
    [TestClass]
    public class SendMailTests
    {
        private static readonly string Location = "C:\\temp\\TestEmail";

        [TestInitialize]
        public void Setup()
        {
            Directory.CreateDirectory(Location);
            DirectoryEx.DeleteFiles(Location);
        }

         [TestMethod]
        public void EmailService_Sent_Test()
        {
            using (var smtp = new EmailService(op =>
            {
                op.TemplateJsonFile = "TestData\\Emails.json";
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
                op.TemplateJsonFile = "TestData\\Emails.json";
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
    }
}

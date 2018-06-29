using FluentAssertions;
using HBD.Services.Email.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Email.Tests.Configurations
{
    [TestClass]
    public class EmailTemplateTests
    {
        [TestMethod]
        public void Invalid_EmailTemplate_Test()
        {
            new EmailTemplate("Duy").IsValid.Should().BeFalse();

            new EmailTemplate("Duy")
            {
                CcEmails = "Duy",
                Subject = "S",
                Body = "123"
            }.IsValid.Should().BeTrue();

            new EmailTemplate("Duy")
            {
                BccEmails = "Duy",
                Subject = "S",
                Body = "123"
            }.IsValid.Should().BeTrue();

            new EmailTemplate("Duy")
            {
                ToEmails = "Duy",
                Subject = "S",
                Body = "123"
            }.IsValid.Should().BeTrue();

            new EmailTemplate("Duy")
            {
                ToEmails = "Duy",
                Subject = "S",
                BodyFile = "123"
            }.IsValid.Should().BeFalse();

            new EmailTemplate("Duy")
            {
                ToEmails = "Duy",
                Subject = "S",
                BodyFile = "TestData\\Emails.json"
            }.IsValid.Should().BeTrue();
        }

        [TestMethod]
        public void EmailTemplate_Test()
        {
            new EmailTemplate("Duy").Name.Should().Be("Duy");
        }
    }
}

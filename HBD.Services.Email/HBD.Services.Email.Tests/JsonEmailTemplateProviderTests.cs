using System.Threading.Tasks;
using FluentAssertions;
using HBD.Services.Email.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HBD.Services.Email.Tests
{
    [TestClass]
    public class JsonEmailTemplateProviderTests
    {
        [TestMethod]
        public async Task TestJsonProvider()
        {
            using (var p = new JsonEmailTemplateProvider("TestData//Emails.json"))
            {
                var template = await p.GetTemplate("Duy1");

                template.Should().NotBeNull();
                template.Name.Should().NotBeNull();
                template.BccEmails.Should().NotBeNull();
                template.CcEmails.Should().NotBeNull();
                template.Subject.Should().NotBeNull();
                template.ToEmails.Should().NotBeNull();


                template = await p.GetTemplate("Duy2");

                template.Should().NotBeNull();
                template.Name.Should().NotBeNull();
                template.BccEmails.Should().NotBeNull();
                template.CcEmails.Should().NotBeNull();
                template.Subject.Should().NotBeNull();
                template.ToEmails.Should().NotBeNull();
            }
        }
    }
}

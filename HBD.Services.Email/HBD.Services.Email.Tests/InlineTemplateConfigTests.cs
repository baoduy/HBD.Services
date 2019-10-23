using FluentAssertions;
using HBD.Services.Email.Builders;
using HBD.Services.Email.Exceptions;
using HBD.Services.Email.Providers;
using HBD.Services.Email.Templates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace HBD.Services.Email.Tests
{
    [TestClass]
    public class InlineTemplateConfigTests
    {
        #region Methods

        [TestMethod]
        public void BuildAnEmail()
        {
            var b = EmailTemplateBuilder.New("Template 1")
                .To("duy@hbd.com")
                .Cc("hoang@hbd.com")
                .Bcc("steven@hbd.com")
                .With()
                .Subject("Email Subject")
                .Body("Email body")
                .Build();

            b.Should().HaveCount(1);

            var t = b.First();
            t.Name.Should().Be("Template 1");
            t.ToEmails.Should().Be("duy@hbd.com");
            t.CcEmails.Should().Be("hoang@hbd.com");
            t.BccEmails.Should().Be("steven@hbd.com");
            t.Subject.Should().Be("Email Subject");
            t.Body.Should().Be("Email body");
            ((EmailTemplate)t).IsValid.Should().BeTrue();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuildDuplicateEmailName()
        {
            _ = EmailTemplateBuilder.New("Template 1")
                    .To("duy@hbd.com")
                    .With()
                     .Subject("Email Subject")
                     .Body("Email body")
                 .Add("Template 1")
                     .To("duy@hbd.com")
                    .With()
                     .Subject("Email Subject")
                     .BodyFrom("TestData/Body.html")
                 .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTemplateException))]
        public void BuildInvalidEmail()
        {
            _ = EmailTemplateBuilder.New("Template 1")
                .To("duy@hbd.com")
                .With()
                .Subject("AAA")
                .Build();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTemplateException))]
        public void BuildOtherInvalidEmail()
        {
            _ = EmailTemplateBuilder.New("Template 1")
               .To("duy@hbd.com")
                    .With()
                .Subject("AAA")
                .Add("Template 2");
        }

        [TestMethod]
        public void BuildTwoEmail()
        {
            var b = EmailTemplateBuilder.New("Template 1")
                    .To("duy@hbd.com")
                    .Cc("hoang@hbd.com")
                    .Bcc("steven@hbd.com")
                    .With()
                    .Subject("Email Subject")
                    .Body("Email body")
                .Add("Template 2")
                    .To("duy@hbd.com")
                    .With()
                    .Subject("Email Subject")
                    .BodyFrom("TestData/Body.html")
                .Build();

            b.Should().HaveCount(2);
            b.OfType<EmailTemplate>().All(b => b.IsValid).Should().BeTrue();
        }

        [TestMethod]
        public async System.Threading.Tasks.Task ConfigTemplateAsync()
        {
            var s = new ServiceCollection()
                .AddEmailService(op =>
                {
                    op.EmailTemplateFrom(builder => builder.Add("Template 1")
                    .To("duy@hbd.com")
                    .With()
                    .Subject("AA")
                    .Body("BBB"));
                })
                .BuildServiceProvider();

            var p = s.GetService<IEmailTemplateProvider>();
            p.Should().NotBeNull();
            p.Should().BeOfType<InlineEmailTemplateProvider>();
            (await p.GetTemplate("Template 1")).Should().NotBeNull();
        }

        #endregion Methods
    }
}
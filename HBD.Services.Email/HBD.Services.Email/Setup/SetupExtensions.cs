﻿using System;
using System.Net;
using HBD.Services.Email;
using HBD.Services.Email.Providers;
using HBD.Services.Email.Templates;
using HBD.Services.Transformation;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SetupExtensions
    {
        #region Methods

        public static IServiceCollection AddEmailService(this IServiceCollection services, Action<EmailSetupOptions> options)
        {
            var op = new EmailSetupOptions();
            options?.Invoke(op);

            if (op.SmtpClientFactory != null)
                services.AddSingleton(new SmtpEmailOptions
                {
                    FromEmailAddress = string.IsNullOrWhiteSpace(op.FromEmail) ? null : new System.Net.Mail.MailAddress(op.FromEmail),
                    SmtpClientFactory = op.SmtpClientFactory
                });

            if (op.SmtpClientFromConfig)
                services.AddSingleton(p =>
                {
                    var config = p.GetRequiredService<IOptions<EmailTemplateSection>>().Value;

                    return new SmtpEmailOptions
                    {
                        FromEmailAddress = string.IsNullOrWhiteSpace(config.FromEmail) ? null : new System.Net.Mail.MailAddress(config.FromEmail),
                        SmtpClientFactory = () => new System.Net.Mail.SmtpClient(config.Host, config.Port)
                        {
                            Credentials = string.IsNullOrWhiteSpace(config.UserName) ? null : new NetworkCredential(config.UserName, config.Password),
                            EnableSsl = config.EnableSsl
                        }
                    };
                });

            return services.AddSingleton(op)
                .AddTemplateProvider(op)
                .AddEmailServiceOnly();
        }


        private static IServiceCollection AddTemplateProvider(this IServiceCollection services, EmailSetupOptions options)
        {
            if (options.TemplateBuilder != null)
                services.AddSingleton<IEmailTemplateProvider>(new InlineEmailTemplateProvider(options.TemplateBuilder));

            if (!string.IsNullOrWhiteSpace(options.JsonFile))
                services.AddSingleton<IEmailTemplateProvider>(new JsonEmailTemplateProvider(options.JsonFile));

            if (options.SectionName)
                services.AddSingleton<IEmailTemplateProvider, AppSettingTemplateProvider>();

            return services;
        }

        private static IServiceCollection AddEmailServiceOnly(this IServiceCollection services)
            => services.AddSingleton<IMailMessageProvider, MailMessageProvider>()
                        .AddSingleton<ITransformer, Transformer>()
                        .AddSingleton<IEmailService>(p =>
                        {
                            var mail = p.GetRequiredService<IMailMessageProvider>();
                            var options = p.GetService<SmtpEmailOptions>();

                            return new SmtpEmailService(mail, options);
                        });

        #endregion Methods
    }
}
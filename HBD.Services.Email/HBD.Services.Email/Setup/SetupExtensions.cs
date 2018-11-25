using HBD.Services.Email.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HBD.Services.Email.Setup
{
    public static class SetupExtensions
    {
        private static IServiceCollection AddEmailServiceOnly(this IServiceCollection services)
            => services.AddSingleton<IMailMessageProvider, MailMessageProvider>()
                .AddSingleton<IEmailService, EmailService>();

        public static IServiceCollection AddEmailService(this IServiceCollection services, string configFile)
            => services.AddSingleton<IEmailTemplateProvider>(p => new JsonEmailTemplateProvider(configFile))
                .AddEmailServiceOnly();

        public static IServiceCollection AddEmailService(this IServiceCollection services, Func<IServiceProvider, IEmailTemplateProvider> implementationFactory)
            => services.AddSingleton(implementationFactory)
            .AddEmailServiceOnly();
    }
}

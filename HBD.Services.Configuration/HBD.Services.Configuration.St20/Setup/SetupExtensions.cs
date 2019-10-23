using Microsoft.Extensions.DependencyInjection;

namespace HBD.Services.Configuration.Setup
{
    public static class SetupExtensions
    {
        #region Methods

        public static IServiceCollection AddConfigurationService(this IServiceCollection services,
            ConfigurationOptions options)
            => services.AddSingleton<IConfigurationService>(p => new ConfigurationService(options));

        #endregion Methods
    }
}
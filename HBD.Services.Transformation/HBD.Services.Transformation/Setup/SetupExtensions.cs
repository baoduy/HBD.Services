using HBD.Services.Transformation;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SetupExtensions
    {
        #region Methods

        public static IServiceCollection AddTransformerService(this IServiceCollection services, Action<TransformOptions> optionFactory)
           => services.AddSingleton<ITransformerService>(p => new TransformerService(optionFactory));

        #endregion Methods
    }
}
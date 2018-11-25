using Microsoft.Extensions.DependencyInjection;
using System;

namespace HBD.Services.Transformation.Setup
{
   public static class SetupExtensions
   {
       public static IServiceCollection AddTransformerService(this IServiceCollection services, Action<TransformOptions> optionFactory)
           => services.AddSingleton<ITransformerService>(p => new TransformerService(optionFactory));
   }
}

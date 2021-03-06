﻿using HBD.Services.Transformation;
using System;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SetupExtensions
    {
        #region Methods

        public static IServiceCollection AddTransformerService(this IServiceCollection services, Action<TransformOptions> optionFactory) 
            => services.AddTransient<ITransformerService>(p => new TransformerService(optionFactory));

        #endregion Methods
    }
}
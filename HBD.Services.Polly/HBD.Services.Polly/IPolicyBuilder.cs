using System;
using System.Linq.Expressions;
using Polly;

namespace HBD.Services.Polly
{
    public interface IPolicyBuilder<T> : IInterceptorBuilder
    {
        T Build(params object[] constructorArguments);
        T Build<TClass>(params object[] constructorArguments) where TClass : class, T;
        PolicyBuilder<T> For(Expression<Action<T>> methodSelector, Policy policy);
        PolicyBuilder<T> ForAllOthers(Policy policy);
    }
}
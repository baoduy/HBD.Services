using System;
using System.Linq;
using System.Linq.Expressions;
using Castle.DynamicProxy;

namespace HBD.Services.Polly
{
    internal static class Extensions
    {
        public static string GetMethodNameAndParameters<TItem>(this Expression<Action<TItem>> expression)
        {
            if (expression.Body is MethodCallExpression m)
                return $"{m.Method.Name}_{string.Join("_", m.Arguments.Select(a => a.Type.Name))}";
            throw new ArgumentException("Expression is not an MethodCallExpression");
        }

        public static string GetMethodNameAndParameters(this IInvocation invocation) 
            => $"{invocation.Method.Name}_{string.Join("_", invocation.Arguments.Select(a => a.GetType().Name))}";
    }
}

using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Polly;

namespace HBD.Services.Polly
{
    public class PolicyProxy<T> : IInterceptor
    {
        private readonly IDictionary<string, Policy> _policies;
        private readonly Policy _classPolicy;

        internal PolicyProxy(Policy classPolicy, IDictionary<string, Policy> policies)
        {
            if (classPolicy == null && policies.Count <= 0)
                throw new ArgumentNullException("There is no policies provided.");

            _classPolicy = classPolicy;
            _policies = policies;
        }

        public void Intercept(IInvocation invocation)
        {
            if(!typeof(T).IsAssignableFrom(invocation.TargetType))
                throw new NotSupportedException($"This {nameof(PolicyProxy<T>)} is not support {invocation.TargetType.FullName}");

            var name = invocation.GetMethodNameAndParameters();
            var p = TryGetPolicy(name);
           
            if (p != null)
                p.Execute(invocation.Proceed);
            else invocation.Proceed();
        }

        private Policy TryGetPolicy(string methodName) 
            => _policies.ContainsKey(methodName) ? _policies[methodName] : _classPolicy;
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Castle.DynamicProxy;
using Polly;

namespace HBD.Services.Polly
{
    public sealed class PolicyBuilder<T> : IPolicyBuilder<T>
    {
        private readonly IDictionary<string, Policy> _policies = new Dictionary<string, Policy>();
        private Policy _classPolicy;

        private readonly IProxyGenerator _proxyGenerator;

        public PolicyBuilder():this(new ProxyGenerator()){}

        // ReSharper disable once MemberCanBePrivate.Global
        public PolicyBuilder(IProxyGenerator proxyGenerator) => this._proxyGenerator = proxyGenerator;
        
        private void AddPolicy(string methodNameAndParameters, Policy policy)
        {
            if (string.IsNullOrEmpty(methodNameAndParameters))
                throw new ArgumentNullException(nameof(methodNameAndParameters));
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            _policies.Add(methodNameAndParameters, policy);
        }

        #region Apply Policy
        /// <summary>
        /// Apply Policy for a Method.
        /// </summary>
        /// <param name="methodSelector"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public PolicyBuilder<T> For(Expression<Action<T>> methodSelector, Policy policy)
        {
            this.AddPolicy(methodSelector.GetMethodNameAndParameters(), policy);
            return this;
        }

        /// <summary>
        /// Apply the policy for all other virtual methods.
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public PolicyBuilder<T> ForAllOthers(Policy policy)
        {
            if(_classPolicy!=null)
                throw new ArgumentException($"The policy for all other methods had been provided.");
            _classPolicy = policy;
            return this;
        }
        #endregion

        #region Build Methods
        /// <summary>
        /// Create a proxy for TInstance.
        /// </summary>
        /// <returns></returns>
        public T Build(params object[] constructorArguments)
        {
            var p = CreateInterceptor();
            return (T)_proxyGenerator.CreateClassProxy(typeof(T), constructorArguments, p);
        }

        /// <summary>
        /// Create a proxy for an instance of T.
        /// </summary>
        /// <returns></returns>
        public T Build<TClass>(params object[] constructorArguments) where TClass : class, T
        {
            var p = CreateInterceptor();
            return (T)_proxyGenerator.CreateClassProxy(typeof(TClass), constructorArguments, p);
        }
        #endregion

        /// <summary>
        /// Create IInterceptor
        /// </summary>
        /// <returns></returns>
        public IInterceptor CreateInterceptor() => new PolicyProxy<T>(_classPolicy,_policies);
    }
}

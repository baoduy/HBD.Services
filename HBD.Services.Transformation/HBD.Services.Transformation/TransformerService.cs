using HBD.Services.Transformation.Convertors;
using HBD.Services.Transformation.Exceptions;
using HBD.Services.Transformation.TokenExtractors;
using HBD.Services.Transformation.TokenResolvers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBD.Services.Transformation
{
    public class TransformerService : ITransformerService
    {
        #region Fields

        public static readonly IValueFormatter DefaultConvertor = new ValueFormatter();

        public static readonly IReadOnlyCollection<ITokenExtractor> DefaultTokenExtractors = new ITokenExtractor[]
                {
            new AngledBracketTokenExtractor(),
            new SquareBracketExtractor(),
            new CurlyBracketExtractor(),
        };

        public static readonly ITokenResolver DefaultTokenResolver = new TokenResolver();
        private readonly ConcurrentDictionary<string, object> _cacheService;
        private readonly object _locker = new object();
        private readonly Action<TransformOptions> _optionFactory;
        private bool _disabledLocalCache = false;
        private IValueFormatter _formatter;
        private bool _initialized;

        private ITokenResolver _tokenResolver;
        private IReadOnlyCollection<ITokenExtractor> _tokens;
        private object[] _transformData;

        #endregion Fields

        #region Constructors

        public TransformerService() : this(null)
        {
        }

        public TransformerService(Action<TransformOptions> optionFactory)
        {
            this._optionFactory = optionFactory;
            _cacheService = new ConcurrentDictionary<string, object>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Disable the local cache. If there are 2 or move IToken with the same key the value will resolve 1 time only.
        /// Disable cache the TokenResolver will be call for every IToken regarless to the Key.
        /// </summary>
        public bool DisabledLocalCache
        {
            get
            {
                EnsureInitialized();
                return _disabledLocalCache;
            }
        }

        /// <summary>
        /// The convertor will be used to convert obj to string.
        /// Apply the data format in this object.
        /// </summary>
        public IValueFormatter Formatter
        {
            get
            {
                EnsureInitialized();
                return _formatter;
            }
        }

        /// <summary>
        /// The Token Resolver that will use to discover the value by IToken in provided Data.
        /// </summary>
        public ITokenResolver TokenResolver
        {
            get
            {
                EnsureInitialized();
                return _tokenResolver;
            }
        }

        /// <summary>
        /// The Token Extractors that will be used to extract the IToken from Template
        /// </summary>
        public IReadOnlyCollection<ITokenExtractor> Tokens
        {
            get
            {
                EnsureInitialized();
                return _tokens;
            }
        }

        public object[] TransformData
        {
            get
            {
                EnsureInitialized();
                return _transformData;
            }
        }

        #endregion Properties

        #region Methods

        public void Dispose() => Dispose(true);

        public async Task<string> TransformAsync(string template, params object[] additionalData)
        {
            EnsureInitialized();

            var tokens = await Task.WhenAll(Tokens.Select(t => t.ExtractAsync(template))).ConfigureAwait(false);
            return await this.InternalTransformAsync(template, tokens.SelectMany(i => i), additionalData, null).ConfigureAwait(false);
        }

        public async Task<string> TransformAsync(string template, Func<IToken, Task<object>> dataProvider)
        {
            EnsureInitialized();

            var tokens = await Task.WhenAll(Tokens.Select(t => t.ExtractAsync(template))).ConfigureAwait(false);
            return await this.InternalTransformAsync(template, tokens.SelectMany(i => i), null, dataProvider).ConfigureAwait(false);
        }

        protected virtual void Dispose(bool disposing) => _cacheService.Clear();

        protected virtual async Task<string> InternalTransformAsync(string template, IEnumerable<IToken> tokens, object[] additionalData, Func<IToken, Task<object>> dataProvider)
        {
            var builder = new StringBuilder(template);
            var adjustment = 0;

            foreach (var token in tokens.OrderBy(t => t.Index))
            {
                var val = await TryGetAndCacheValueAsync(token, dataProvider).ConfigureAwait(false) ?? TryGetAndCacheValue(token, additionalData);

                if (val == null)
                    throw new UnResolvedTokenException(token.Token);

                var strVal = Formatter.Convert(token, val);

                builder = builder.Replace(token.Token, strVal, token.Index + adjustment, token.Token.Length);
                adjustment += strVal.Length - token.Token.Length;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Try Get data for <see cref="IToken"/> from additionalData and then TransformData and Cache for later use.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="additionalData"></param>
        /// <returns></returns>
        protected virtual object TryGetAndCacheValue(IToken token, object[] additionalData)
        {
            return DisabledLocalCache
                ? TryGetValue(token, additionalData)
                : _cacheService.GetOrAdd(token.Token.ToUpper(), t => TryGetValue(token, additionalData));
        }

        /// <summary>
        /// Try Get data for <see cref="IToken"/> from dataProvider and Cache for later use.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dataProvider"></param>
        /// <returns></returns>
        protected virtual async Task<object> TryGetAndCacheValueAsync(IToken token, Func<IToken, Task<object>> dataProvider)
        {
            if (dataProvider == null) return null;
            var val = await dataProvider(token).ConfigureAwait(false);
            return DisabledLocalCache ? val : _cacheService.GetOrAdd(token.Token.ToUpper(), t => val);
        }

        /// <summary>
        /// Try Get data for <see cref="IToken"/> from additionalData and then TransformData
        /// </summary>
        /// <param name="token"></param>
        /// <param name="additionalData"></param>
        /// <returns></returns>
        protected virtual object TryGetValue(IToken token, object[] additionalData)
        {
            object val = null;

            if (additionalData?.Any() == true)
                val = TokenResolver.Resolve(token, additionalData);

            if (val == null && TransformData?.Any() == true)
                val = TokenResolver.Resolve(token, TransformData);

            return val;
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;

            lock (_locker)
            {
                if (_optionFactory != null)
                {
                    var op = new TransformOptions();
                    _optionFactory.Invoke(op);

                    if (op.TokenExtractors.Any())
                        _tokens = new ReadOnlyCollection<ITokenExtractor>(op.TokenExtractors);

                    _tokenResolver = op.TokenResolver;
                    _formatter = op.Formatter;
                    _disabledLocalCache = op.DisabledLocalCache;
                    _transformData = op.TransformData;
                }

                if (_tokens == null || _tokens.Count <= 0)
                    _tokens = DefaultTokenExtractors;

                if (_tokenResolver == null)
                    _tokenResolver = DefaultTokenResolver;

                if (_formatter == null)
                    _formatter = DefaultConvertor;

                _initialized = true;
            }
        }

        #endregion Methods
    }
}
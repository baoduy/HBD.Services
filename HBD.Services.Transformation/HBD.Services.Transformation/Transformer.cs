using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HBD.Services.Transformation.Convertors;
using HBD.Services.Transformation.Exceptions;
using HBD.Services.Transformation.TokenExtractors;
using HBD.Services.Transformation.TokenResolvers;

namespace HBD.Services.Transformation
{
    public class Transformer : ITransformer
    {
        public static readonly IReadOnlyCollection<ITokenExtractor> DefaultTokenExtractors = new ITokenExtractor[]
        {
            new AngledBracketTokenExtractor(),
            new SquareBracketExtractor(),
            new CurlyBracketExtractor(),
        };
        public static readonly ITokenResolver DefaultTokenResolver = new TokenResolver();
        public static readonly IValueFormatter DefaultConvertor = new ValueFormatter();

        private readonly object _locker = new object();
        private readonly ConcurrentDictionary<string, object> _cacher;
        private readonly Action<TransformOptions> _optionFactory;
        private bool _initialized;

        private IReadOnlyCollection<ITokenExtractor> _tokens;
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

        private ITokenResolver _tokenResolver;
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

        private IValueFormatter _formatter;
        /// <summary>
        /// The convertor will be used to convert obj to string.
        /// Appy the data format in this object.
        /// </summary>
        public IValueFormatter Formatter
        {
            get
            {
                EnsureInitialized();
                return _formatter;
            }
        }

        private bool _disabledLocalCache = false;
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

        private object[] _transformData;
        public object[] TransformData
        {
            get
            {
                EnsureInitialized();
                return _transformData;
            }
        }

        public Transformer() : this(null)
        {
        }

        public Transformer(Action<TransformOptions> optionFactory)
        {
            this._optionFactory = optionFactory;
            _cacher = new ConcurrentDictionary<string, object>();
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

        protected virtual object TryGetValue(IToken token, object[] additionalData)
        {
            object val = null;

            if (additionalData?.Any() == true)
                val = TokenResolver.Resolve(token, additionalData);

            if (val == null && TransformData?.Any() == true)
                val = TokenResolver.Resolve(token, TransformData);

            return val;
        }

        protected virtual object TryGetValueAndCache(IToken token, object[] additionalData)
        {
            return DisabledLocalCache
                ? TryGetValue(token, additionalData)
                : _cacher.GetOrAdd(token.Token.ToUpper(), t => TryGetValue(token, additionalData));
        }

        protected virtual string InternalTransform(string template, IEnumerable<IToken> tokens, object[] additionalData)
        {
            var builder = new StringBuilder(template);
            var adjustment = 0;

            foreach (var token in tokens.OrderBy(t => t.Index))
            {
                var val = TryGetValueAndCache(token, additionalData);

                if (val == null)
                    throw new UnResolvedTokenException(token.Token);

                var strVal = Formatter.Convert(token, val);

                builder = builder.Replace(token.Token, strVal, token.Index + adjustment, token.Token.Length);
                adjustment += strVal.Length - token.Token.Length;
            }

            return builder.ToString();
        }


        public string Transform(string template, params object[] additionalData)
        {
            EnsureInitialized();

            var copyTemplate = template;
            var tokens = Tokens.SelectMany(t => t.Extract(copyTemplate));

            return InternalTransform(template, tokens, additionalData);
        }

        public async Task<string> TransformAsync(string template, params object[] additionalData)
        {
            EnsureInitialized();

            var copyTemplate = template;
            var tokens = await Task.WhenAll(Tokens.Select(t => t.ExtractAsync(copyTemplate)));

            return await Task.Run(() => this.InternalTransform(template, tokens.SelectMany(i => i), additionalData));
        }


        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing) => _cacher.Clear();
    }
}

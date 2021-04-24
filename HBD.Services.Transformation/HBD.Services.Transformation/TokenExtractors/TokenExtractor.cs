using HBD.Services.Transformation.TokenDefinitions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBD.Services.Transformation.TokenExtractors
{
    /// <summary>
    /// The extractor of &lt;token&gt;
    /// </summary>
    public class AngledBracketTokenExtractor : TokenExtractor
    {
        #region Constructors

        public AngledBracketTokenExtractor() : base(new AngledBracketDefinition())
        {
        }

        #endregion Constructors
    }

    /// <summary>
    /// The extractor of {token}
    /// </summary>
    public class CurlyBracketExtractor : TokenExtractor
    {
        #region Constructors

        public CurlyBracketExtractor() : base(new CurlyBracketDefinition())
        {
        }

        #endregion Constructors
    }

    /// <summary>
    /// The extractor of [token]
    /// </summary>
    public class SquareBracketExtractor : TokenExtractor
    {
        #region Constructors

        public SquareBracketExtractor() : base(new SquareBracketDefinition())
        {
        }

        #endregion Constructors
    }

    public class TokenExtractor : ITokenExtractor
    {
        #region Constructors

        public TokenExtractor(ITokenDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        #endregion Constructors

        #region Properties

        protected ITokenDefinition Definition { get; }

        #endregion Properties

        #region Methods

        public IEnumerable<IToken> Extract(string template)
        {
            return ExtractCore(template);
        }

        public Task<IEnumerable<IToken>> ExtractAsync(string template)
        {
            return Task.Run(() => ExtractCore(template));
        }

        protected virtual IEnumerable<IToken> ExtractCore(string template)
        {
            if (string.IsNullOrWhiteSpace(template)) yield break;

            var length = template.Length;

            var si = template.IndexOf(Definition.Begin);

            while (si >= 0 && si < length)
            {
                //If next char is begin of Token then move to next
                if (si < length - 1 && template[si + 1] == Definition.Begin)
                {
                    si += 1;
                    continue;
                }

                var li = template.IndexOf(Definition.End, si);

                if (li <= si) break;

                yield return new TokenResult(Definition, template.Substring(si, li - si + 1), template, si);
                si = template.IndexOf(Definition.Begin, li);
            }
        }

        #endregion Methods
    }
}
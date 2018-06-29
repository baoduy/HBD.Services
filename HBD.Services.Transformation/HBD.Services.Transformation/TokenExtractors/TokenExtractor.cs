using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HBD.Services.Transformation.TokenDefinitions;

namespace HBD.Services.Transformation.TokenExtractors
{
    public class TokenExtractor : ITokenExtractor
    {
        protected ITokenDefinition Definition { get; }

        public TokenExtractor(ITokenDefinition definition)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
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

        public IEnumerable<IToken> Extract(string template)
            => this.ExtractCore(template);

        public Task<IEnumerable<IToken>> ExtractAsync(string template) 
            => Task.Run(() => this.ExtractCore(template));
    }

    /// <summary>
    /// The extractor of {token}
    /// </summary>
    public class CurlyBracketExtractor : TokenExtractor
    {
        public CurlyBracketExtractor() : base(new CurlyBracketDefinition())
        {
        }
    }

    /// <summary>
    /// The extractor of <token>
    /// </summary>
    public class AngledBracketTokenExtractor : TokenExtractor
    {
        public AngledBracketTokenExtractor() : base(new AngledBracketDefinition())
        {
        }
    }

    /// <summary>
    /// The extractor of [token]
    /// </summary>
    public class SquareBracketExtractor : TokenExtractor
    {
        public SquareBracketExtractor() : base(new SquareBracketDefinition())
        {
        }
    }
}

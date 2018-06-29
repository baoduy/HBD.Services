namespace HBD.Services.Transformation.TokenDefinitions
{
    /// <summary>
    /// Defination of [token]
    /// </summary>
    public sealed class SquareBracketDefinition: TokenDefinitionBase
    {
        public override char Begin => '[';
        public override char End => ']';
    }
}

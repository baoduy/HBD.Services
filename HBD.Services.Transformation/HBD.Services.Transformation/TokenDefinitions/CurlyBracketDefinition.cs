namespace HBD.Services.Transformation.TokenDefinitions
{
    /// <summary>
    /// Defination of Curly Brackets {token}
    /// </summary>
    public sealed class CurlyBracketDefinition: TokenDefinitionBase
    {
        public override char Begin => '{';
        public override char End => '}';
    }
}

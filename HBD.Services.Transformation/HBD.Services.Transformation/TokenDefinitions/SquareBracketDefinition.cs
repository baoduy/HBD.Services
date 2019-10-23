namespace HBD.Services.Transformation.TokenDefinitions
{
    /// <summary>
    /// Defination of [token]
    /// </summary>
    public sealed class SquareBracketDefinition : TokenDefinitionBase
    {
        #region Properties

        public override char Begin => '[';

        public override char End => ']';

        #endregion Properties
    }
}
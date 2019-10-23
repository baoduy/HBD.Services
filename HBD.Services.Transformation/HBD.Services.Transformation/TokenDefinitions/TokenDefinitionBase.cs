namespace HBD.Services.Transformation.TokenDefinitions
{
    public abstract class TokenDefinitionBase : ITokenDefinition
    {
        #region Properties

        public abstract char Begin { get; }

        public abstract char End { get; }

        #endregion Properties

        #region Methods

        public bool IsToken(string value)
            => !string.IsNullOrWhiteSpace(value) && value[0] == Begin && value[value.Length - 1] == End;

        #endregion Methods
    }
}
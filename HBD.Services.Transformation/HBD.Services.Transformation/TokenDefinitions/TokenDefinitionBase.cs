namespace HBD.Services.Transformation.TokenDefinitions
{
    public abstract class TokenDefinitionBase : ITokenDefinition
    {
        public abstract char Begin { get; }
        public abstract char End { get; }
        public bool IsToken(string value)
            => !string.IsNullOrWhiteSpace(value) && value[0] == Begin && value[value.Length - 1] == End;
    }
}

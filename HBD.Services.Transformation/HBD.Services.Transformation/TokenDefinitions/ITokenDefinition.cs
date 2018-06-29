namespace HBD.Services.Transformation.TokenDefinitions
{
    public interface ITokenDefinition
    {
        /// <summary>
        /// Begin character of token. Ex '[';
        /// </summary>
        char Begin { get; }

        /// <summary>
        /// End character of token. Ex: ']';
        /// </summary>
        char End { get; }

        /// <summary>
        /// Check whether value is token or not?
        /// Ex: return true if value is [key] and false if [key[.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IsToken(string value);
    }
}

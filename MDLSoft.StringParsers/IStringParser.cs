namespace MDLSoft.StringParsers
{
    /// <summary>
    /// Defines a contract for parsing strings to objects and writing objects to strings.
    /// </summary>
    /// <typeparam name="T">The type of object to parse to and from.</typeparam>
    public interface IStringParser<T> where T : class,new()
    {
        /// <summary>
        /// Parses a string into an object of type T.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>An object of type T parsed from the text.</returns>
        /// <exception cref="StringParserException">Thrown when parsing fails.</exception>
        T Parse(string text);
        
        /// <summary>
        /// Converts an object of type T to its string representation.
        /// </summary>
        /// <param name="data">The object to convert to string.</param>
        /// <returns>A string representation of the object.</returns>
        /// <exception cref="StringParserException">Thrown when writing fails.</exception>
        string Write(T data);
    }
}
namespace Bam.Console
{
    /// <summary>
    /// Exception thrown when command line arguments fail to parse.
    /// </summary>
    public class ArgumentParsingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentParsingException"/> class with the parsed arguments that caused the error.
        /// </summary>
        /// <param name="parsedArguments">The parsed arguments containing the error details.</param>
        public ArgumentParsingException(ParsedArguments parsedArguments) : base(parsedArguments.Message)
        {
            this.ParsedArguments = parsedArguments;
        }

        /// <summary>
        /// Gets the parsed arguments that caused the parsing failure.
        /// </summary>
        public ParsedArguments ParsedArguments { get; private set; }
    }
}

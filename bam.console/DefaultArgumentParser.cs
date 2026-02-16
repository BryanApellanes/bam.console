namespace Bam.Console
{
    /// <summary>
    /// Default implementation of <see cref="IArgumentParser"/> that parses command line arguments using <see cref="ArgumentFormatOptions"/>.
    /// </summary>
    public class DefaultArgumentParser : IArgumentParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultArgumentParser"/> class using default format options.
        /// </summary>
        public DefaultArgumentParser() : this(ArgumentFormatOptions.Default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultArgumentParser"/> class with the specified format options.
        /// </summary>
        /// <param name="options">The argument format options to use for parsing.</param>
        public DefaultArgumentParser(ArgumentFormatOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// Gets the argument format options used by this parser.
        /// </summary>
        public ArgumentFormatOptions Options { get; }

        /// <summary>
        /// Parses the specified command line arguments into an <see cref="IParsedArguments"/> instance.
        /// </summary>
        /// <param name="arguments">The command line arguments to parse.</param>
        /// <returns>The parsed arguments.</returns>
        public IParsedArguments ParseArguments(string[] arguments)
        {
            return new ParsedArguments(Options, arguments, ArgumentInfo.FromArgs(Options, arguments));
        }
    }
}

namespace Bam.Console
{
    public class DefaultArgumentParser : IArgumentParser
    {
        public DefaultArgumentParser() : this(ArgumentFormatOptions.Default) { }

        public DefaultArgumentParser(ArgumentFormatOptions options)
        {
            Options = options;
        }

        public ArgumentFormatOptions Options { get; }

        public IParsedArguments ParseArguments(string[] arguments)
        {
            return new ParsedArguments(Options, arguments, ArgumentInfo.FromArgs(Options, arguments));
        }
    }
}

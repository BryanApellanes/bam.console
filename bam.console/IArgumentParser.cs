namespace Bam.Console
{
    /// <summary>
    /// An interface defining a method for parsing command line arguments.
    /// </summary>
    public interface IArgumentParser
    {
        /// <summary>
        /// Parses the specified command line arguments into an <see cref="IParsedArguments"/> instance.
        /// </summary>
        /// <param name="arguments">The command line arguments to parse.</param>
        /// <returns>The parsed arguments.</returns>
        IParsedArguments ParseArguments(string[] arguments);
    }
}

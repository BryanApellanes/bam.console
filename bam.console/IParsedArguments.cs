/*
	Copyright © Bryan Apellanes 2015  
*/
namespace Bam.Console
{
    /// <summary>
    /// Defines the contract for parsed command line arguments, providing access to argument values, keys, status, and lookup methods.
    /// </summary>
    public interface IParsedArguments
    {
        /// <summary>
        /// Gets or sets the value of the argument with the specified name.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument value, or null if not found.</returns>
        string this[string name] { get; set; }

        /// <summary>
        /// Gets all argument names that were parsed.
        /// </summary>
        string[] Keys { get; }

        /// <summary>
        /// Gets the number of parsed arguments.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Gets or sets a message describing the parse result, such as an error message.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Gets the original string array that was parsed.
        /// </summary>
        string[] OriginalStrings { get; }

        /// <summary>
        /// Gets or sets the status of the argument parsing operation.
        /// </summary>
        ArgumentParseStatus Status { get; set; }

        /// <summary>
        /// Determines whether the specified argument name is present in the parsed arguments.
        /// </summary>
        /// <param name="argumentToLookFor">The argument name to search for.</param>
        /// <returns>True if the argument is present; otherwise, false.</returns>
        bool Contains(string argumentToLookFor);

        /// <summary>
        /// Determines whether the specified argument name is present in the parsed arguments and outputs its value.
        /// </summary>
        /// <param name="argumentToLookFor">The argument name to search for.</param>
        /// <param name="argument">When this method returns, contains the argument value if found; otherwise, null.</param>
        /// <returns>True if the argument is present; otherwise, false.</returns>
        bool Contains(string argumentToLookFor, out string? argument);
    }
}
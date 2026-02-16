namespace Bam.Console
{
    /// <summary>
    /// Represents the result status of parsing command line arguments.
    /// </summary>
    public enum ArgumentParseStatus
    {
        /// <summary>
        /// The arguments are in an invalid format.
        /// </summary>
        Invalid,
        /// <summary>
        /// The arguments were parsed successfully.
        /// </summary>
        Success,
        /// <summary>
        /// An error occurred while parsing the arguments.
        /// </summary>
        Error
    }

}

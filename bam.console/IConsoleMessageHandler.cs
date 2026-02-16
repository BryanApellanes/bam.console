namespace Bam.Console
{
    /// <summary>
    /// Defines methods for logging and printing console messages.
    /// </summary>
    public interface IConsoleMessageHandler
    {
        /// <summary>
        /// Logs the specified console messages.
        /// </summary>
        /// <param name="consoleMessages">The messages to log.</param>
        void Log(params ConsoleMessage[] consoleMessages);
        /// <summary>
        /// Prints the specified console messages to the console output.
        /// </summary>
        /// <param name="messages">The messages to print.</param>
        void Print(params ConsoleMessage[] messages);
    }
}
namespace Bam.Console
{
    /// <summary>
    /// Reports success messages to the console in dark green.
    /// </summary>
    public class ConsoleSuccessReporter : ISuccessReporter
    {
        /// <summary>
        /// Prints a success message to the console in dark green.
        /// </summary>
        /// <param name="message">The success message to display.</param>
        public void ReportSuccess(string message)
        {
            Message.PrintLine(message, ConsoleColor.DarkGreen);
        }
    }
}

namespace Bam.Console
{
    /// <summary>
    /// Reports exceptions to the console using colored output for the message and stack trace.
    /// </summary>
    public class ConsoleExceptionReporter : IExceptionReporter
    {
        /// <summary>
        /// Prints the specified message and exception details to the console in error colors.
        /// </summary>
        /// <param name="message">A descriptive message about the exception context.</param>
        /// <param name="exception">The exception to report.</param>
        public void ReportException(string message, Exception exception)
        {
            Message.PrintLine(message, ConsoleColor.DarkRed);
            ReportException(exception);
        }

        /// <summary>
        /// Prints the exception message and stack trace to the console in error colors.
        /// </summary>
        /// <param name="exception">The exception to report.</param>
        public void ReportException(Exception exception)
        {
            Message.PrintLine(exception.Message, ConsoleColor.DarkRed);
            Message.PrintLine(exception.GetStackTrace(), ConsoleColor.Magenta);
        }
    }
}

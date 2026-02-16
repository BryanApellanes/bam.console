namespace Bam.Console
{
    /// <summary>
    /// Defines a method for reporting success messages.
    /// </summary>
    public interface ISuccessReporter
    {
        /// <summary>
        /// Reports the specified success message.
        /// </summary>
        /// <param name="message">The success message to report.</param>
        void ReportSuccess(string message);
    }
}

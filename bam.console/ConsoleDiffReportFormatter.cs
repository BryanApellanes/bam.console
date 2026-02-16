/*
	Copyright © Bryan Apellanes 2015  
*/

using System.Text;
using Bam.Analytics;

namespace Bam.Console
{
    /// <summary>
    /// Formats diff reports for console output, using color-coded text to indicate unchanged, deleted, and inserted lines.
    /// </summary>
    public class ConsoleDiffReportFormatter : DiffReportFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleDiffReportFormatter"/> class.
        /// </summary>
        public ConsoleDiffReportFormatter() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleDiffReportFormatter"/> class with the specified diff report.
        /// </summary>
        /// <param name="report">The diff report to format.</param>
        public ConsoleDiffReportFormatter(DiffReport report) : base(report) { }
        /// <summary>
        /// Writes an unchanged line to the console in cyan.
        /// </summary>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="text">The line text.</param>
        /// <param name="output">The string builder (unused; output goes to console).</param>
        public override void WriteLine(int lineNumber, string text, StringBuilder output)
        {
            Message.PrintLine("{0}{1}", ConsoleColor.Cyan, NumberLines ? "{0} ".Format(lineNumber) : "", text);
        }

        /// <summary>
        /// Writes a deleted line to the console in red, prefixed with '-'.
        /// </summary>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="text">The deleted line text.</param>
        /// <param name="output">The string builder (unused; output goes to console).</param>
        public override void WriteDeletedLine(int lineNumber, string text, StringBuilder output)
        {
            Message.PrintLine("-{0}{1}", ConsoleColor.Red, NumberLines ? "{0} ".Format(lineNumber) : "", text);
        }

        /// <summary>
        /// Writes an inserted line to the console in green, prefixed with '+'.
        /// </summary>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="text">The inserted line text.</param>
        /// <param name="output">The string builder (unused; output goes to console).</param>
        public override void WriteInsertedLine(int lineNumber, string text, StringBuilder output)
        {
            Message.PrintLine("+{0}{1}", ConsoleColor.Green, NumberLines ? "{0} ".Format(lineNumber) : "", text);
        }
    }
}

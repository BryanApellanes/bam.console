using Bam.Analytics;
using Bam.Logging;

namespace Bam.Console
{
    /// <summary>
    /// Provides static methods for printing and logging colored messages to the console, and for displaying text diffs.
    /// </summary>
    public class Message
    {
        static Message()
        {
            GetDiffReportFormatter = (diffReport) => new ConsoleDiffReportFormatter(diffReport);
        }

        /// <summary>
        /// Logs the specified console messages using the default logger.
        /// </summary>
        /// <param name="messages">The messages to log.</param>
        public static void Log(params ConsoleMessage[] messages)
        {
            ConsoleMessage.Log(messages);
        }

        /// <summary>
        /// Logs a formatted message with a trailing newline using the default logger.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void LogLine(string messageSignature, params object[] messageSignatureArgs)
        {
            Log($"{messageSignature}\r\n", messageSignatureArgs);
        }

        /// <summary>
        /// Logs a formatted message using the default logger.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void Log(string messageSignature, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Log(messageSignature, messageSignatureArgs);
        }

        /// <summary>
        /// Logs a formatted message with a trailing newline using the specified logger.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void LogLine(ILogger logger, string messageSignature, params object[] messageSignatureArgs)
        {
            Log(logger, $"{messageSignature}\r\n", messageSignatureArgs);
        }

        /// <summary>
        /// Logs a formatted message using the specified logger.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void Log(ILogger logger, string messageSignature, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Log(logger, messageSignature, messageSignatureArgs);
        }

        /// <summary>
        /// Logs a formatted message with the specified text color.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="textColor">The foreground color.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void LogLine(string messageSignature, ConsoleColor textColor, params object[] messageSignatureArgs)
        {
            Log($"{messageSignature}", textColor, messageSignatureArgs);
        }

        /// <summary>
        /// Logs a formatted message with the specified text color using the default logger.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="textColor">The foreground color.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void Log(string messageSignature, ConsoleColor textColor, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Log(Bam.Logging.Log.Default, messageSignature, textColor, messageSignatureArgs);
        }

        /// <summary>
        /// Logs a formatted message with the specified color combination using the default logger.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="colors">The foreground and background color combination.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void Log(string messageSignature, ConsoleColorCombo colors, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Log(messageSignature, colors, messageSignatureArgs);
        }

        /// <summary>
        /// Logs a formatted message with a trailing newline and the specified text color using the specified logger.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="textColor">The foreground color.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void LogLine(ILogger logger, string messageSignature, ConsoleColor textColor, params object[] messageSignatureArgs)
        {
            Log(logger, $"{messageSignature}\r\n", textColor, messageSignatureArgs);
        }

        /// <summary>
        /// Logs a formatted message with the specified text color using the specified logger.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="textColor">The foreground color.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void Log(ILogger logger, string messageSignature, ConsoleColor textColor, params object[] messageSignatureArgs)
        {
            ConsoleMessage.Log(logger, messageSignature, textColor, messageSignatureArgs);
        }

        /// <summary>
        /// Prints an empty line to the console.
        /// </summary>
        public static void PrintLine()
        {
            PrintLine("");
        }

        /// <summary>
        /// Prints the JSON representation of the specified object to the console.
        /// </summary>
        /// <param name="instance">The object to serialize and print.</param>
        public static void Print(object instance)
        {
            PrintLine(instance.ToJson(true));
        }

        /// <summary>
        /// Prints a formatted message with a trailing newline to the console.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void PrintLine(string messageSignature, params object?[] messageSignatureArgs)
        {
            Print($"{messageSignature}\r\n", messageSignatureArgs);
        }

        /// <summary>
        /// Prints a formatted message to the console.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void Print(string messageSignature, params object?[] messageSignatureArgs)
        {
            ConsoleMessage.Print(messageSignature, messageSignatureArgs);
        }

        /// <summary>
        /// Prints a formatted message with a trailing newline in the specified text color.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="textColor">The foreground color.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void PrintLine(string messageSignature, ConsoleColor textColor, params object?[] messageSignatureArgs)
        {
            Print($"{messageSignature}\r\n", textColor, messageSignatureArgs);
        }

        /// <summary>
        /// Prints a formatted message with a trailing newline in the specified foreground and background colors.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        /// <param name="backgroundColor">The background color.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void PrintLine(string messageSignature, ConsoleColor foregroundColor, ConsoleColor backgroundColor, params object?[] messageSignatureArgs)
        {
            PrintLine(messageSignature, new ConsoleColorCombo(foregroundColor, backgroundColor), messageSignatureArgs);
        }

        /// <summary>
        /// Prints a formatted message with a trailing newline using the specified color combination.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="colors">The foreground and background color combination.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void PrintLine(string messageSignature, ConsoleColorCombo colors, params object?[] messageSignatureArgs)
        {
            Print($"{messageSignature}\r\n", colors, messageSignatureArgs);
        }

        /// <summary>
        /// Prints a formatted message using the specified color combination.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="colors">The foreground and background color combination.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void Print(string messageSignature, ConsoleColorCombo colors, params object?[] messageSignatureArgs)
        {
            ConsoleMessage.Print(messageSignature, colors, messageSignatureArgs);
        }

        /// <summary>
        /// Prints a formatted message in the specified text color.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="textColor">The foreground color.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void Print(string messageSignature, ConsoleColor textColor, params object?[] messageSignatureArgs)
        {
            ConsoleMessage.Print(messageSignature, textColor, messageSignatureArgs);
        }

        /// <summary>
        /// Prints a color-coded diff of two strings to the console.
        /// </summary>
        /// <param name="first">The first string to compare.</param>
        /// <param name="second">The second string to compare.</param>
        public static void PrintDiff(string first, string second)
        {
            DiffReport report = DiffReport.Create(first, second);
            DiffReportFormatter formatter = GetDiffReportFormatter(report);
            formatter.Format();
        }

        /// <summary>
        /// Gets or sets the factory function that creates a <see cref="DiffReportFormatter"/> for a given <see cref="DiffReport"/>. Defaults to creating a <see cref="ConsoleDiffReportFormatter"/>.
        /// </summary>
        public static Func<DiffReport, DiffReportFormatter> GetDiffReportFormatter
        {
            get;
            set;
        }
    }
}
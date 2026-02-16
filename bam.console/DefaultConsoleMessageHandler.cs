using Bam.Console;

namespace Bam
{
    /// <summary>
    /// Default implementation of <see cref="IConsoleMessageHandler"/> that writes messages directly to <see cref="System.Console"/> with color support.
    /// </summary>
    public class DefaultConsoleMessageHandler : IConsoleMessageHandler
    {
        /// <summary>
        /// Logs the specified console messages using <see cref="ConsoleMessage.Log(ConsoleMessage[])"/>.
        /// </summary>
        /// <param name="consoleMessages">The messages to log.</param>
        public void Log(params Console.ConsoleMessage[] consoleMessages)
        {
            ConsoleMessage.Log(consoleMessages);
        }

        /// <summary>
        /// Prints the specified console messages to the console with their configured colors.
        /// </summary>
        /// <param name="messages">The messages to print.</param>
        public void Print(params ConsoleMessage[] messages)
        {
            if (messages != null)
            {
                foreach (ConsoleMessage message in messages)
                {
                    PrintMessage(message);
                }
            }
        }
        
        /// <summary>
        /// Writes a text string to the console with the specified foreground and background colors, then resets colors.
        /// </summary>
        /// <param name="message">The text to write.</param>
        /// <param name="foregroundColor">The foreground color.</param>
        /// <param name="backgroundColor">The background color (defaults to Black).</param>
        public static void PrintMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            System.Console.ForegroundColor = foregroundColor;
            System.Console.BackgroundColor = backgroundColor;
            System.Console.Write(message);
            System.Console.ResetColor();
        }
        
        /// <summary>
        /// Writes a <see cref="ConsoleMessage"/> to the console using its configured colors, then resets colors.
        /// </summary>
        /// <param name="message">The console message to write.</param>
        public static void PrintMessage(ConsoleMessage message)
        {
            System.Console.ForegroundColor = message.Colors.ForegroundColor;
            System.Console.BackgroundColor = message.Colors.BackgroundColor;
            System.Console.Write(message.Text);
            System.Console.ResetColor();
        }
    }
}
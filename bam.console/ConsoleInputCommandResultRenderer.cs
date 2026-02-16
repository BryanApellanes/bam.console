using Bam.Shell;

namespace Bam.Console
{
    /// <summary>
    /// Renders input command results to the console, displaying success or failure with color-coded output.
    /// </summary>
    public class ConsoleInputCommandResultRenderer : IInputCommandResultRenderer
    {
        /// <summary>
        /// Renders the specified input command result to the console, showing the command name and whether it succeeded or failed.
        /// </summary>
        /// <param name="inputCommandResult">The input command result to render.</param>
        public void RenderInputCommandResult(IInputCommandResult inputCommandResult)
        {
            Args.ThrowIfNull(inputCommandResult, nameof(inputCommandResult));

            ConsoleColorCombo colors = new ConsoleColorCombo(ConsoleColor.Green, ConsoleColor.Black);
            string result = "succeeded";
            string commandName = "command";
            string extended = string.Empty;
            if (inputCommandResult != null)
            {
                commandName = inputCommandResult.InputName ?? commandName;
                if (!inputCommandResult.Success)
                {
                    colors = new ConsoleColorCombo(ConsoleColor.DarkRed, ConsoleColor.DarkYellow);
                    result = "failed";
                    extended = inputCommandResult.Exception.GetMessageAndStackTrace();
                }
            }
            Message.PrintLine();
            Message.Print(" > command ");
            Message.Print("'{0}'", new ConsoleColorCombo(ConsoleColor.White, ConsoleColor.DarkYellow), commandName);
            Message.Print(" --> ");
            Message.Print(" {0}", colors, result);
            Message.PrintLine();
            
            if(inputCommandResult?.Message != null)            
            {
                Message.PrintLine(inputCommandResult.Message, colors);
            }

            if (extended != string.Empty)
            {
                Message.PrintLine(extended, ConsoleColor.DarkMagenta);
            }
        }
    }
}

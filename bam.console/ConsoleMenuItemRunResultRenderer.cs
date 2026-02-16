using Bam.Shell;

namespace Bam.Console
{
    /// <summary>
    /// Renders menu item run results to the console, showing success or failure messages with color-coded output.
    /// </summary>
    public class ConsoleMenuItemRunResultRenderer : IMenuItemRunResultRenderer
    {
        /// <summary>
        /// Gets or sets the action to invoke when a menu item run succeeds. Defaults to printing a green success message.
        /// </summary>
        public Action<IMenuItemRunResult> ItemRunSucceeded { get; set; } = (menuItemRunResult) => Message.PrintLine("{0} succeeded", ConsoleColor.Green, menuItemRunResult?.MenuItem?.DisplayName ?? "run");

        /// <summary>
        /// Gets or sets the action to invoke when a menu item run fails. Defaults to printing a magenta failure message.
        /// </summary>
        public Action<IMenuItemRunResult> ItemRunFailed { get; set; } = (menuItemRunResult) => Message.PrintLine("{0} failed", ConsoleColor.Magenta, menuItemRunResult?.MenuItem?.DisplayName ?? "run");

        /// <summary>
        /// Gets or sets the action to invoke when a menu item run throws an exception. Defaults to printing the stack trace in dark magenta.
        /// </summary>
        public Action<IMenuItemRunResult> ItemRunException { get; set; } = (menuItemRunResult) => Message.PrintLine(menuItemRunResult?.Exception?.GetMessageAndStackTrace() ?? "Stacktrace unavailable", ConsoleColor.DarkMagenta);

        /// <summary>
        /// Renders the specified menu item run result, calling the appropriate success, failure, or exception handler.
        /// </summary>
        /// <param name="menuItemRunResult">The run result to render.</param>
        public void RenderMenuItemRunResult(IMenuItemRunResult menuItemRunResult)
        {
            if (menuItemRunResult != null)
            {
                if (menuItemRunResult.Success)
                {
                    ItemRunSucceeded(menuItemRunResult);
                }
                else
                {
                    if (menuItemRunResult.MenuItem != null && !string.IsNullOrEmpty(menuItemRunResult.MenuItem.DisplayName))
                    {
                        ItemRunFailed(menuItemRunResult);
                    }

                    if (menuItemRunResult.Exception != null && !string.IsNullOrEmpty(menuItemRunResult.Exception.StackTrace))
                    {
                        ItemRunException(menuItemRunResult);
                    }
                }
            }
        }
    }
}

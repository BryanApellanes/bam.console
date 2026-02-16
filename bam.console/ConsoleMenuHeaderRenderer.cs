using Bam.Shell;

namespace Bam.Console
{
    /// <summary>
    /// Renders the menu header to the console, displaying the menu's display name and header text.
    /// </summary>
    public class ConsoleMenuHeaderRenderer : IMenuHeaderRenderer
    {
        /// <summary>
        /// Renders the header for the specified menu, including its display name and header text.
        /// </summary>
        /// <param name="menu">The menu whose header should be rendered.</param>
        public void RenderMenuHeader(IMenu menu)
        {
            Message.PrintLine(menu.DisplayName);
            Message.PrintLine();
            Message.PrintLine(menu.HeaderText);
        }
    }
}

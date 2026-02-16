using Bam.Shell;

namespace Bam.Console
{
    /// <summary>
    /// Renders menu footer text and a tab-separated list of all menus to the console, highlighting the selected menu.
    /// </summary>
    public class ConsoleMenuFooterRenderer : IMenuFooterRenderer
    {
        /// <summary>
        /// Renders the footer for the selected menu and displays all available menus with their selectors.
        /// </summary>
        /// <param name="selectedMenu">The currently selected menu.</param>
        /// <param name="allMenus">All available menus to display in the footer.</param>
        public void RenderMenuFooter(IMenu selectedMenu, params IMenu[] allMenus)
        {
            Message.PrintLine(selectedMenu.FooterText);

            foreach (IMenu m in allMenus)
            {
                ConsoleColorCombo colors = new ConsoleColorCombo(ConsoleColor.Cyan, ConsoleColor.Black);
                if (m == selectedMenu)
                {
                    colors = new ConsoleColorCombo(ConsoleColor.Black, ConsoleColor.White);
                }
                Message.Print("[{0}{1}] {2}\t", colors, ConsoleMenuInput.SelectorPrefix, m.Selector, m.Name);
            }
            Message.PrintLine();
        }
    }
}

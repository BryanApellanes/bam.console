using Bam.Shell;

namespace Bam.Console
{
    /// <summary>
    /// Renders menus to the console, including numbered items, selectors, header, footer, input commands, and a prompt.
    /// </summary>
    public class ConsoleMenuRenderer : MenuRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenuRenderer"/> class with the specified sub-renderers.
        /// </summary>
        /// <param name="headerRenderer">The header renderer.</param>
        /// <param name="footerRenderer">The footer renderer.</param>
        /// <param name="inputReader">The input reader.</param>
        /// <param name="inputCommandRenderer">The input command renderer.</param>
        public ConsoleMenuRenderer(IMenuHeaderRenderer headerRenderer, IMenuFooterRenderer footerRenderer, IMenuInputReader inputReader, IMenuInputCommandRenderer inputCommandRenderer) : base(headerRenderer, footerRenderer, inputReader, inputCommandRenderer)
        {
            Divider = "*".Times(30);
        }

        protected override void RenderItems(IMenu menu)
        {
            int number = 0;
            foreach (IMenuItem item in menu.Items)
            {
                string pointer = item.Selected ? ">" : " ";
                string selector = !string.IsNullOrEmpty(item.Selector) ? $"[{ConsoleMenuInput.SelectorPrefix}{item.Selector}] " : "";
                Message.PrintLine($"{pointer} {++number}. {selector}{item.DisplayName}");
            }
        }

        /// <summary>
        /// Clears the console and re-renders the menu, displaying the current input value and a divider if Enter was pressed.
        /// </summary>
        /// <param name="menu">The menu to re-render.</param>
        /// <param name="menuInput">The current input state.</param>
        /// <param name="otherMenus">Other available menus to include in the footer.</param>
        public override void RerenderMenu(IMenu menu, IMenuInput menuInput, params IMenu[] otherMenus)
        {
            System.Console.Clear();
            RenderMenu(menu, otherMenus);
            Message.Print(menuInput.Value, ConsoleColor.Green);
            if (menuInput.Enter)
            {
                Message.PrintLine();
                RenderDivider();
            }
        }

        /// <summary>
        /// Renders the full menu to the console including header, items, footer, input commands, and prompt.
        /// </summary>
        /// <param name="selectedMenu">The currently selected menu to render.</param>
        /// <param name="menus">All available menus to include in the footer.</param>
        public override void RenderMenu(IMenu selectedMenu, params IMenu[] menus)
        {
            HeaderRenderer.RenderMenuHeader(selectedMenu);
            RenderItems(selectedMenu);
            FooterRenderer.RenderMenuFooter(selectedMenu, menus);
            RenderInputCommands(selectedMenu);
            RenderPrompt(selectedMenu);
        }

        /// <summary>
        /// Renders a visual divider line to the console in dark yellow.
        /// </summary>
        public override void RenderDivider()
        {
            Message.PrintLine();
            Message.PrintLine(Divider, ConsoleColor.DarkYellow);
            Message.PrintLine();
        }

        /// <summary>
        /// Renders the available input commands for the specified menu using the input command renderer.
        /// </summary>
        /// <param name="menu">The menu whose input commands should be rendered.</param>
        public override void RenderInputCommands(IMenu menu)
        {
            InputCommandRenderer.RenderMenuInputCommands(menu);
        }

        /// <summary>
        /// Renders the input prompt for the specified menu, displaying the menu's selector followed by " > ".
        /// </summary>
        /// <param name="menu">The menu to render the prompt for.</param>
        public virtual void RenderPrompt(IMenu menu)
        {
            Message.Print($" {menu.Selector} > ");
        }
    }
}

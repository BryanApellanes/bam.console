using Bam.Shell;
using Bam.Test.Menu;

namespace Bam.Console
{
    /// <summary>
    /// Renders available input commands for a menu to the console, listing each command name and description.
    /// </summary>
    public class ConsoleMenuInputCommandRenderer : IMenuInputCommandRenderer
    {
        /// <summary>
        /// Renders the list of input commands available on the specified menu to the console.
        /// </summary>
        /// <param name="menu">The menu whose input commands should be displayed.</param>
        public void RenderMenuInputCommands(IMenu menu)
        {            
            InputCommands commands = new InputCommands(menu);
            if(commands.Commands.Count > 0)
            {
                Message.PrintLine(Menu.DefaultFooterText);
                foreach (string name in commands.Names)
                {
                    Message.PrintLine($"\"{name}\" -- {commands.Commands[name].Description}");
                }
            }
            Message.PrintLine(Menu.DefaultFooterText);
        }
    }
}

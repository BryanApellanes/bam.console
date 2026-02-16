using Bam.Shell;

namespace Bam.Console
{
    /// <summary>
    /// Provides menus whose items are defined by the <see cref="ConsoleCommandAttribute"/> attribute.
    /// </summary>
    public class ConsoleMenuProvider : MenuProvider<ConsoleCommandAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenuProvider"/> class.
        /// </summary>
        /// <param name="menuItemProvider">The provider for menu items.</param>
        /// <param name="menuItemSelector">The selector for menu item navigation.</param>
        /// <param name="menuItemRunner">The runner for executing menu items.</param>
        public ConsoleMenuProvider(IMenuItemProvider menuItemProvider, IMenuItemSelector menuItemSelector, IMenuItemRunner menuItemRunner) : base(menuItemProvider, menuItemSelector, menuItemRunner)
        {
        }

        /// <summary>
        /// Gets a menu for the specified container type using <see cref="ConsoleCommandAttribute"/> as the item attribute.
        /// </summary>
        /// <param name="type">The container type to build a menu from.</param>
        /// <returns>An <see cref="IMenu"/> for the specified type.</returns>
        public override IMenu GetMenu(Type type)
        {
            return GetMenu<ConsoleCommandAttribute>(type);
        }
    }
}

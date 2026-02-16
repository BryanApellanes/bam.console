using Bam.DependencyInjection;
using Bam.Services;
using Bam.Shell;

namespace Bam.Console
{
    /// <summary>
    /// Runs menu item methods using a dependency provider to resolve instances and a method argument provider to supply parameters.
    /// </summary>
    public class ConsoleMenuItemRunner : IMenuItemRunner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenuItemRunner"/> class with the specified dependency provider and method argument provider.
        /// </summary>
        /// <param name="dependencyProvider">The dependency provider used to resolve instances for non-static methods.</param>
        /// <param name="menuInputParser">The provider that supplies method arguments from menu input.</param>
        public ConsoleMenuItemRunner(IDependencyProvider dependencyProvider, IMenuInputMethodArgumentProvider menuInputParser)
        {
            DependencyProvider = dependencyProvider;
            MethodArgumentProvider = menuInputParser;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenuItemRunner"/> class with the specified dependency provider and a default <see cref="StringArgumentProvider"/>.
        /// </summary>
        /// <param name="dependencyProvider">The dependency provider used to resolve instances for non-static methods.</param>
        public ConsoleMenuItemRunner(IDependencyProvider dependencyProvider) : this(dependencyProvider, new MenuInputMethodArgumentProvider(new StringArgumentProvider()))
        {
        }

        /// <summary>
        /// Gets or sets the provider that supplies method arguments from menu input.
        /// </summary>
        public IMenuInputMethodArgumentProvider MethodArgumentProvider { get; set; }

        /// <summary>
        /// Gets the component that provides instances for non-static menu item methods.
        /// </summary>
        public IDependencyProvider DependencyProvider { get; private set; }

        /// <summary>
        /// Invokes the method associated with the specified menu item, resolving the instance from the dependency provider if needed.
        /// </summary>
        /// <param name="menuItem">The menu item to run.</param>
        /// <param name="menuInput">The current menu input providing arguments.</param>
        /// <returns>A <see cref="IMenuItemRunResult"/> indicating success or failure of the invocation.</returns>
        public IMenuItemRunResult RunMenuItem(IMenuItem menuItem, IMenuInput menuInput)
        {
            try
            {
                if (!menuItem.MethodInfo.IsStatic && menuItem.Instance == null)
                {
                    if (menuItem.MethodInfo.DeclaringType != null)
                    {
                        menuItem.Instance = DependencyProvider.Get(menuItem.MethodInfo.DeclaringType);
                    }
                }

                object? result = menuItem.MethodInfo.Invoke(menuItem.Instance, MethodArgumentProvider.GetMethodArguments(menuItem, menuInput));

                if (result is Task taskResult)
                {
                    taskResult.Wait();
                }
                
                return new MenuItemRunResult()
                {
                    MenuItem = menuItem,
                    Success = true,
                    MenuInput = menuInput,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                return new MenuItemRunResult
                {
                    MenuItem = menuItem,
                    Success = false,
                    MenuInput = menuInput,
                    Exception = ex.GetInnerException(),
                };
            }
        }
    }
}

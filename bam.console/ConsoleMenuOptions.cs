using Bam.DependencyInjection;
using Bam.Services;
using Bam.Shell;

namespace Bam.Console;

/// <summary>
/// Provides console-specific menu options with a factory method to build a fully configured <see cref="ServiceRegistry"/> for console menu rendering.
/// </summary>
public class ConsoleMenuOptions : MenuOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleMenuOptions"/> class with all required menu rendering components.
    /// </summary>
    /// <param name="menuRenderer">The menu renderer.</param>
    /// <param name="menuHeaderRenderer">The menu header renderer.</param>
    /// <param name="menuFooterRenderer">The menu footer renderer.</param>
    /// <param name="menuProvider">The menu provider.</param>
    /// <param name="menuInputReader">The menu input reader.</param>
    /// <param name="menuInputCommandInterpreter">The input command interpreter.</param>
    /// <param name="menuItemRunner">The menu item runner.</param>
    /// <param name="menuItemRunResultRenderer">The menu item run result renderer.</param>
    /// <param name="inputCommandResultRenderer">The input command result renderer.</param>
    public ConsoleMenuOptions(IMenuRenderer menuRenderer, IMenuHeaderRenderer menuHeaderRenderer, IMenuFooterRenderer menuFooterRenderer, IMenuProvider menuProvider, IMenuInputReader menuInputReader, IMenuInputCommandInterpreter menuInputCommandInterpreter, IMenuItemRunner menuItemRunner, IMenuItemRunResultRenderer menuItemRunResultRenderer, IInputCommandResultRenderer inputCommandResultRenderer) : base(menuRenderer, menuHeaderRenderer, menuFooterRenderer, menuProvider, menuInputReader, menuInputCommandInterpreter, menuItemRunner, menuItemRunResultRenderer, inputCommandResultRenderer)
    {
    }

    /// <summary>
    /// Gets a <see cref="ConsoleMenuOptions"/> instance resolved from a configured service registry.
    /// </summary>
    /// <param name="menuItemRunResultRenderer">Optional custom renderer for menu item run results.</param>
    /// <typeparam name="TypedArgumentProviderType">The type of <see cref="ITypedArgumentProvider"/> to use for resolving menu method parameters.</typeparam>
    /// <returns>A configured <see cref="ConsoleMenuOptions"/> instance.</returns>
    public static ConsoleMenuOptions Get<TypedArgumentProviderType>(IMenuItemRunResultRenderer? menuItemRunResultRenderer = null)
        where TypedArgumentProviderType : ITypedArgumentProvider
    {
        return GetServiceRegistry<TypedArgumentProviderType>(menuItemRunResultRenderer).Get<ConsoleMenuOptions>();
    }
    
    /// <summary>
    /// Get a ServiceRegistry configured to provide console menu rendering.
    /// </summary>
    /// <param name="menuItemRunResultRenderer">The renderer used to render run results.</param>
    /// <typeparam name="TypedArgumentProviderType">The type of TypedArgumentProvider to use.  This component provides arguments to menu methods that have parameters.</typeparam>
    /// <returns></returns>
    public static ServiceRegistry GetServiceRegistry<TypedArgumentProviderType>(IMenuItemRunResultRenderer? menuItemRunResultRenderer = null) where TypedArgumentProviderType: ITypedArgumentProvider
    {
        ServiceRegistry registry = new ServiceRegistry();
        registry
            .For<IDependencyProvider>().UseSingleton(registry)
            .For<ServiceRegistry>().UseSingleton(registry)
            .For<IMenuRenderer>().Use<ConsoleMenuRenderer>()
            .For<IMenuHeaderRenderer>().UseSingleton<ConsoleMenuHeaderRenderer>()
            .For<IMenuFooterRenderer>().UseSingleton<ConsoleMenuFooterRenderer>()
            .For<IMenuInputCommandRenderer>().Use<ConsoleMenuInputCommandRenderer>()
            .For<IMenuItemProvider>().Use<MenuItemProvider>()
            .For<IMenuProvider>().Use<ConsoleMenuProvider>()
            .For<IMenuInputReader>().UseSingleton<ConsoleMenuInputReader>()
            .For<IMenuInputCommandInterpreter>().Use<DefaultMenuInputCommandInterpreter>()
            .For<IMenuInputMethodArgumentProvider>().Use<MenuInputMethodArgumentProvider>()
            .For<ITypedArgumentProvider>().Use<TypedArgumentProviderType>()
            .For<IMenuItemRunResultRenderer>().UseSingleton(menuItemRunResultRenderer ?? new ConsoleMenuItemRunResultRenderer())
            .For<IInputCommandResultRenderer>().Use<ConsoleInputCommandResultRenderer>()
            .For<IMenuItemSelector>().Use<MenuItemSelector>()
            .For<IMenuItemRunner>().Use<ConsoleMenuItemRunner>()
            .For<ISuccessReporter>().Use<ConsoleSuccessReporter>()
            .For<IConsoleMethodParameterProvider>().Use<CommandLineArgumentsConsoleMethodParameterProvider>()
            .For<IExceptionReporter>().Use<ConsoleExceptionReporter>()
            .For<IMenuManager>().UseSingleton<MenuManager>();
        return registry;
    } 
}
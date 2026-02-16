using Bam.DependencyInjection;
using Bam.Services;

namespace Bam.Console
{
    /// <summary>
    /// A menu container that configures its dependency provider from a <see cref="ServiceRegistry"/> and is decorated with <see cref="ConsoleMenu"/>.
    /// </summary>
    [ConsoleMenu()]
    public class ConsoleMenuContainer : MenuContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenuContainer"/> class, configuring the provided service registry as the dependency provider.
        /// </summary>
        /// <param name="serviceRegistry">The service registry to configure and use as the dependency provider.</param>
        public ConsoleMenuContainer(ServiceRegistry serviceRegistry)
            :base()
        {
            this.SetDependencyProvider(this.Configure(serviceRegistry));
        }

        /// <summary>
        /// Configures the specified service registry before setting as the DependencyProvider property.
        /// </summary>
        /// <param name="serviceRegistry">The service registry.</param>
        /// <returns>ServiceRegistry</returns>
        public virtual ServiceRegistry Configure(ServiceRegistry serviceRegistry)
        {
            return serviceRegistry;
        }
    }
}

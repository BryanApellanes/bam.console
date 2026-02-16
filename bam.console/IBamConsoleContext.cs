using Bam.Shell;

namespace Bam.Console
{
    /// <summary>
    /// Defines the contract for a Bam console application context, providing access to a menu manager and argument registration.
    /// </summary>
    public interface IBamConsoleContext : IBamContext
    {
        /// <summary>
        /// Gets the menu manager for the console application.
        /// </summary>
        IMenuManager MenuManager { get; }

        /// <summary>
        /// Registers a valid command line argument by name.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="description">An optional description of the argument.</param>
        void AddValidArgument(string name, string? description = null);
        /// <summary>
        /// Registers a valid command line argument with additional options.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="allowNull">Whether the argument may have no value.</param>
        /// <param name="addAcronym">Whether to also register a case-acronym of the name.</param>
        /// <param name="description">An optional description of the argument.</param>
        /// <param name="valueExample">An optional example value for usage display.</param>
        void AddValidArgument(string name, bool allowNull, bool addAcronym = false, string? description = null, string? valueExample = null);
    }
}

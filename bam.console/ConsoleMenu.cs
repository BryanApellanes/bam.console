using Bam.Shell;

namespace Bam.Console
{
    /// <summary>
    /// A menu attribute that identifies a class as a console menu container with items defined by <see cref="ConsoleCommandAttribute"/>.
    /// </summary>
    public class ConsoleMenu : MenuAttribute<ConsoleCommandAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenu"/> class with the default selector "cm".
        /// </summary>
        public ConsoleMenu()
        {
            this.Selector = "cm";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenu"/> class with a selector derived from the specified name.
        /// </summary>
        /// <param name="name">The name of the menu, used to derive the selector.</param>
        public ConsoleMenu(string name): base(name)
        {
            this.Selector = name.CamelCase().CaseAcronym();
        }
    }
}

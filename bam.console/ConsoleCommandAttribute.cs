namespace Bam.Console
{
    /// <summary>
    /// Used to adorn methods that may be executed by a command line switch.
    /// </summary>
    public class ConsoleCommandAttribute : Attribute
    {
        /// <summary>
        /// Instantiates an instance of the ConsoleCommandAttribute class.
        /// </summary>
        public ConsoleCommandAttribute()
        {
        }
        
        /// <summary>
        /// Instantiates an instance of the ConsoleCommandAttribute class.
        /// </summary>
        /// <param name="name">A human-readable name for the command.</param>
        /// <remarks>
        /// The OptionName is derived from the provided name by camel-casing the provided value.
        /// The OptionShortName is derived from OptionName by creating an acronym from the capital letters.
        /// </remarks>
        public ConsoleCommandAttribute(string name) 
        {
            this.OptionName = name.CamelCase(true, new string[] { " " });
            this.OptionShortName = this.OptionName.CaseAcronym(true);
        }

        /// <summary>
        /// Instantiates an instance of the ConsoleCommandAttribute class.
        /// </summary>
        /// <param name="name">A human-readable name for the command.</param>
        /// <param name="description">A brief description of the command.</param>
        public ConsoleCommandAttribute(string name, string description) 
        {
            this.OptionName = name.CamelCase(true, new string[] { " " });
            this.OptionShortName = this.OptionName.CaseAcronym(true);
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets an example value to display in usage help for this command.
        /// </summary>
        public string? ValueExample
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the command line option to specify in order to execute the method.
        /// </summary>
        public string? OptionName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the abbreviated form of the option name, derived as an acronym from the capital letters of <see cref="OptionName"/>.
        /// </summary>
        public string? OptionShortName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a brief description of what this console command does.
        /// </summary>
        public string? Description
        {
            get;
            private set;
        }
    }
}

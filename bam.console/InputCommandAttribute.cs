namespace Bam.Console
{
    /// <summary>
    /// Used to adorn methods that may be executed by name at a bam menu prompt.
    /// </summary>
    public class InputCommandAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputCommandAttribute"/> class.
        /// </summary>
        public InputCommandAttribute() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InputCommandAttribute"/> class with the specified name and optional description.
        /// </summary>
        /// <param name="name">The command name used to invoke the method from menu input.</param>
        /// <param name="description">An optional description of the command.</param>
        public InputCommandAttribute(string name, string description = null)
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the command.
        /// </summary>
        public string Description { get; set; }
    }
}

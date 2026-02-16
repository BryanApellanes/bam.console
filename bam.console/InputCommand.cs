using System.Reflection;

namespace Bam.Console
{
    /// <summary>
    /// Represents a named command that can be invoked from menu input, defined by an <see cref="InputCommandAttribute"/> on a method.
    /// </summary>
    public class InputCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputCommand"/> class.
        /// </summary>
        /// <param name="containerType">The type that declares the command method.</param>
        /// <param name="optionMethod">The method to invoke when the command is executed.</param>
        /// <param name="attribute">The attribute that defines the command name and description.</param>
        public InputCommand(Type containerType, MethodInfo optionMethod, InputCommandAttribute attribute)
        {
            this.ContainerType = containerType;
            this.OptionMethod = optionMethod;
            this.Attribute = attribute;
            if (Attribute == null)
            {
                throw new InvalidOperationException("Option name not specified");
            }
        }

        protected InputCommandAttribute Attribute { get; set; }

        /// <summary>
        /// Gets the name of this command as defined by the attribute.
        /// </summary>
        public string Name
        {
            get
            {
                return Attribute.Name;
            }
        }

        /// <summary>
        /// Gets the description of this command as defined by the attribute.
        /// </summary>
        public string Description
        {
            get
            {
                return Attribute.Description;
            }
        }

        /// <summary>
        /// Gets or sets the type that declares the command method.
        /// </summary>
        public Type ContainerType { get; set; }

        /// <summary>
        /// Gets or sets the method to invoke when this command is executed.
        /// </summary>
        public MethodInfo OptionMethod { get; set; }
    }
}

using Bam.Shell;
using System.Reflection;

namespace Bam.Console
{
    /// <summary>
    /// Discovers and stores all <see cref="InputCommand"/> instances defined on a container type via <see cref="InputCommandAttribute"/>.
    /// </summary>
    public class InputCommands
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputCommands"/> class using the container type from the specified menu.
        /// </summary>
        /// <param name="menu">The menu whose container type to scan for input commands.</param>
        public InputCommands(IMenu menu)
            : this(menu.ContainerType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputCommands"/> class, scanning the specified type for methods decorated with <see cref="InputCommandAttribute"/>.
        /// </summary>
        /// <param name="type">The type to scan for input commands.</param>
        public InputCommands(Type type)
        {
            this.Commands = new Dictionary<string, InputCommand>();
            this.ContainerType = type;
            this.LoadCommands();
        }

        protected void LoadCommands()
        {
            foreach(MethodInfo methodInfo in ContainerType.GetMethods())
            {
                if(methodInfo.HasCustomAttributeOfType(out InputCommandAttribute attribute))
                {
                    InputCommand option = new InputCommand(this.ContainerType, methodInfo, attribute);
                    if (this.Commands.ContainsKey(option.Name))
                    {
                        throw new InvalidOperationException($"Duplicate option name specified: {option.Name}");
                    }
                    Commands.Add(option.Name, option);
                }
            }
        }

        /// <summary>
        /// Gets the type that was scanned for input commands.
        /// </summary>
        public Type ContainerType { get; private set; }

        /// <summary>
        /// Gets the dictionary of input commands keyed by command name.
        /// </summary>
        public Dictionary<string, InputCommand> Commands { get; private set; }

        /// <summary>
        /// Gets all command names.
        /// </summary>
        public IEnumerable<string> Names
        {
            get
            {
                return Commands.Keys;
            }
        }
    }
}

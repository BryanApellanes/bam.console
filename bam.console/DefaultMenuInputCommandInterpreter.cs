using Bam.Services;
using Bam.Shell;
using System.Reflection;
using Bam.DependencyInjection;

namespace Bam.Console
{
    /// <summary>
    /// Interprets menu input by matching typed command names against methods decorated with <see cref="InputCommandAttribute"/> on the current menu's container type.
    /// </summary>
    public class DefaultMenuInputCommandInterpreter : IMenuInputCommandInterpreter
    {
        private readonly Dictionary<Type, InputCommands> _inputOptions = new Dictionary<Type, InputCommands>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMenuInputCommandInterpreter"/> class with the specified dependency provider.
        /// </summary>
        /// <param name="dependencyProvider">The dependency provider used to resolve instances and method arguments.</param>
        public DefaultMenuInputCommandInterpreter(IDependencyProvider dependencyProvider)
        {
            this.DependencyProvider = dependencyProvider;
        }

        /// <summary>
        /// Attempts to interpret the menu input as a named command, looking up and invoking matching <see cref="InputCommandAttribute"/> methods.
        /// </summary>
        /// <param name="menuManager">The current menu manager.</param>
        /// <param name="menuInput">The input to interpret.</param>
        /// <param name="result">The results of any commands that were invoked.</param>
        /// <returns>True if the input was interpreted as a command; otherwise, false.</returns>
        public bool InterpretInput(IMenuManager menuManager, IMenuInput menuInput, out IInputCommandResults result)
        {
            InputCommandResults results = new InputCommandResults();

            result = results;

            if (menuManager == null)
            {
                return false;
            }

            if (menuManager.CurrentMenu == null)
            {
                return false;
            }

            string[] commands = menuInput.Value.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (commands.Length == 0)
            {
                return false;
            }
            string optionName = commands[0];
            string[] arguments = new string[] { };
            if (commands.Length > 1)
            {
                List<string> argList = new List<string>();
                for (int i = 1; i < commands.Length; i++)
                {
                    argList.Add(commands[i]);
                }

                arguments = argList.ToArray();
            }

            MenuSpec menuSpec = menuManager.CurrentMenu.GetSpec();
            InputCommands inputOptions = GetInputOptions(menuSpec.ContainerType);
            if (inputOptions.Commands.TryGetValue(optionName, out var command))
            {
                InputCommandResult optionResult = InvokeOption(command, arguments);
                results.AddResult(optionResult);
                return true;
            }
            else if (inputOptions.Commands.TryGetValue(menuInput.Value, out var commandOption))
            {
                InputCommandResult optionResult = InvokeOption(commandOption, arguments);
                results.AddResult(optionResult);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the dependency provider used to resolve instances and method arguments.
        /// </summary>
        public IDependencyProvider DependencyProvider
        {
            get;
            private set;
        }

        protected InputCommands GetInputOptions(Type containerType)
        {
            if (!_inputOptions.ContainsKey(containerType))
            {
                _inputOptions.Add(containerType, new InputCommands(containerType));
            }
            return _inputOptions[containerType];
        }

        protected virtual object?[] GetMethodArguments(MethodInfo methodInfo)
        {
            DependencyProviderMethodArgumentProvider argumentProvider =
                new DependencyProviderMethodArgumentProvider(DependencyProvider);
            object?[] arguments = argumentProvider.GetMethodArguments(methodInfo);
            return arguments;
        }

        protected virtual InputCommandResult InvokeOption(InputCommand command, string[] inputStrings)
        {
            try
            {
                object? instance = null;
                if (!command.OptionMethod.IsStatic)
                {
                    instance = DependencyProvider.Get(command.ContainerType);
                }

                return new InputCommandResult
                {
                    InputName = command.Name,
                    InvocationResult = command.OptionMethod.Invoke(instance, GetMethodArguments(command.OptionMethod))
                };
            }
            catch (Exception ex)
            {
                return new InputCommandResult()
                {
                    InputName = command.Name,
                    Exception = ex.GetInnerException()
                };
            }
        }
    }
}

using Bam.Configuration;
using Bam.Logging;
using Bam.Shell;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Bam.DependencyInjection;
using Bam.Services;

namespace Bam.Console
{
    /// <summary>
    /// Provides the main entry point and context for a Bam console application, including argument parsing, menu management, and command switch execution.
    /// </summary>
    public class BamConsoleContext : BamContext, IBamConsoleContext
    {
        static BamConsoleContext()
        {
            Current = new BamConsoleContext();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BamConsoleContext"/> class with an empty list of valid argument info.
        /// </summary>
        public BamConsoleContext()
        {
            ValidArgumentInfo = new List<ArgumentInfo>();
        }

        /// <summary>
        /// Initializes and starts the console application using the default menu specs.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public void Main(string[] args)
        {
            Main(args, MenuSpecs.LoadList.ToArray());
        }

        /// <summary>
        /// Initializes and starts the console application by scanning the specified assemblies for menu specs.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="assemblies">The assemblies to scan for menu specifications.</param>
        public void Main(string[] args, params Assembly[] assemblies)
        {
            Main(args, MenuSpecs.Scan(assemblies).ToArray());
        }

        /// <summary>
        /// Initializes and starts the console application with the specified menu specs.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="menuSpecs">The menu specifications to load.</param>
        public void Main(string[] args, params MenuSpecs[] menuSpecs)
        {
            AddSwitches();
            AddConfigurationSwitches();
            MenuSpecs.LoadList = menuSpecs;
            Main(args, () => { });
        }

        /// <summary>
        /// Static entry point that initializes and starts the console application using the default menu specs and the singleton <see cref="Current"/> context.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void StaticMain(string[] args)
        {
            StaticMain(args, MenuSpecs.LoadList.ToArray());
        }

        /// <summary>
        /// Static entry point that initializes and starts the console application by scanning the specified assemblies for menu specs.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="assemblies">The assemblies to scan for menu specifications.</param>
        public static void StaticMain(string[] args, params Assembly[] assemblies)
        {
            StaticMain(args, MenuSpecs.Scan(assemblies).ToArray());
        }

        /// <summary>
        /// Static entry point that initializes and starts the console application with the specified menu specs.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="menuSpecs">The menu specifications to load.</param>
        public static void StaticMain(string[] args, params MenuSpecs[] menuSpecs)
        {
            Current.AddSwitches();
            Current.AddConfigurationSwitches();
            MenuSpecs.LoadList = menuSpecs;
            StaticMain(args, () => { });
        }

        /// <summary>
        /// Static entry point that initializes the console application with a pre-initialization action, then starts the menu input/output loop.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="preInit">An action to execute before initialization.</param>
        /// <param name="parseErrorHandler">An optional delegate invoked when argument parsing fails.</param>
        public static void StaticMain(string[] args, Action preInit, ConsoleArgsParsedDelegate? parseErrorHandler = null)
        {
            if (parseErrorHandler == null)
            {
                parseErrorHandler = (a) => throw new ArgumentException(a.Message);
            }

            StaticInit(args, preInit, parseErrorHandler);

            Current.MenuManager.StartInputOutputLoop();
            System.Console.ReadLine();
        }

        /// <summary>
        /// Instance entry point that initializes the console application with a pre-initialization action, then starts the menu input/output loop.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <param name="preInit">An action to execute before initialization.</param>
        /// <param name="parseErrorHandler">An optional delegate invoked when argument parsing fails.</param>
        public void Main(string[] args, Action preInit, ConsoleArgsParsedDelegate? parseErrorHandler = null)
        {
            if (parseErrorHandler == null)
            {
                parseErrorHandler = (a) => throw new ArgumentException(a.Message);
            }

            Init(args, preInit, parseErrorHandler);

            this.MenuManager.StartInputOutputLoop();
            System.Console.ReadLine();
        }

        protected void Init(string[] args, Action preInit, ConsoleArgsParsedDelegate? parseErrorHandler)
        {
            this.ArgsParsedError += parseErrorHandler;

            preInit();

            this.AddValidArgument("?", true, description: "Show usage");
            this.AddValidArgument("v", true, description: "Show version information");
            this.AddValidArgument("i", true, description: "Run interactively");

            this.AddValidArgument("ut", true, description: "Run all unit tests");
            this.AddValidArgument("it", true, description: "Run all integration tests");
            this.AddValidArgument("spec", true, description: "Run all specification tests");

            this.AddValidArgument("coverage", true, description: "Enable code coverage collection via dotnet-coverage");
            this.AddValidArgument("coverage-output", false, description: "Coverage output file path", valueExample: "coverage.cobertura.xml");
            this.AddValidArgument("coverage-format", false, description: "Coverage output format", valueExample: "cobertura");
            this.AddValidArgument("coverage-include", false, description: "Semicolon-separated module include patterns for coverage; use * for no filtering", valueExample: ".*[/\\\\]bam[.].*[.]dll$");

            this.ParseArgs(args);

            if (this.Arguments.Contains("?"))
            {
                this.Usage(Assembly.GetExecutingAssembly());
                Exit();
            }
            else if (this.Arguments.Contains("v"))
            {
                Version(Assembly.GetEntryAssembly());
                Exit();
            }

            // If command line arguments were specified but not the interactive switch then
            // execute the associated command switches
            if (this.Arguments.Length > 0 && !this.Arguments.Contains("i"))
            {
                if (ExecuteSwitches(this.Logger, this.Arguments))
                {
                    Exit(0);
                }
                if (ExecuteTestSwitches(this.Logger, this.Arguments))
                {
                    Exit(0);
                }
            }
        }

        protected static void StaticInit(string[] args, Action preInit, ConsoleArgsParsedDelegate? parseErrorHandler)
        {
            Current.ArgsParsedError += parseErrorHandler;

            preInit();

            Current.AddValidArgument("?", true, description: "Show usage");
            Current.AddValidArgument("v", true, description: "Show version information");
            Current.AddValidArgument("i", true, description: "Run interactively");

            Current.AddValidArgument("ut", true, description: "Run all unit tests");
            Current.AddValidArgument("it", true, description: "Run all integration tests");
            Current.AddValidArgument("spec", true, description: "Run all specification tests");

            Current.AddValidArgument("coverage", true, description: "Enable code coverage collection via dotnet-coverage");
            Current.AddValidArgument("coverage-output", false, description: "Coverage output file path", valueExample: "coverage.cobertura.xml");
            Current.AddValidArgument("coverage-format", false, description: "Coverage output format", valueExample: "cobertura");
            Current.AddValidArgument("coverage-include", false, description: "Semicolon-separated module include patterns for coverage; use * for no filtering", valueExample: ".*[/\\\\]bam[.].*[.]dll$");

            Current.ParseArgs(args);

            if (Current.Arguments.Contains("?"))
            {
                Current.Usage(Assembly.GetExecutingAssembly());
                Exit();
            }
            else if (Current.Arguments.Contains("v"))
            {
                Version(Assembly.GetEntryAssembly());
                Exit();
            }

            // If command line arguments were specified but not the interactive switch then
            // execute the associated command switches
            if (Current.Arguments.Length > 0 && !Current.Arguments.Contains("i"))
            {
                if (ExecuteSwitches(Current.Logger, Current.Arguments))
                {
                    Exit(0);
                }
                if (ExecuteTestSwitches(Current.Logger, Current.Arguments))
                {
                    Exit(0);
                }
            }
        }

        /// <summary>
        /// Resets console colors, fires exit events, and terminates the process with the specified exit code.
        /// </summary>
        /// <param name="code">The exit code to return to the operating system.</param>
        public static void Exit(int code = 0)
        {
            System.Console.ResetColor();
            Exiting?.Invoke(code);
            Thread.Sleep(1000);
            Environment.Exit(code);
            Exited?.Invoke(code);
        }

        /// <summary>
        /// Gets or sets the singleton instance of the <see cref="BamConsoleContext"/>.
        /// </summary>
        public new static BamConsoleContext Current
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when command line arguments have been successfully parsed.
        /// </summary>
        public event ConsoleArgsParsedDelegate ArgsParsed;

        /// <summary>
        /// Occurs when command line argument parsing results in an error or invalid status.
        /// </summary>
        public event ConsoleArgsParsedDelegate ArgsParsedError;

        /// <summary>
        /// Occurs when the console application is about to exit.
        /// </summary>
        public static event ExitDelegate Exiting;

        /// <summary>
        /// Occurs after the console application has exited.
        /// </summary>
        public static event ExitDelegate Exited;

        private ServiceRegistry? _serviceRegistry;

        /// <summary>
        /// Gets or sets the service registry used for dependency resolution in this context.
        /// </summary>
        public override ServiceRegistry ServiceRegistry
        {
            get => _serviceRegistry ??= GetDefaultContextServiceRegistry();
            set => _serviceRegistry = value;
        }

        /// <summary>
        /// Gets the argument parser resolved from the service registry.
        /// </summary>
        public IArgumentParser ArgumentParser => ServiceRegistry.Get<IArgumentParser>();

        /// <summary>
        /// Gets the configuration provider resolved from the service registry.
        /// </summary>
        public override IConfigurationProvider ConfigurationProvider => ServiceRegistry.Get<IConfigurationProvider>();

        /// <summary>
        /// Gets the application name provider resolved from the service registry.
        /// </summary>
        public override IApplicationNameProvider ApplicationNameProvider => ServiceRegistry.Get<IApplicationNameProvider>();

        /// <summary>
        /// Gets the logger resolved from the service registry.
        /// </summary>
        public override ILogger Logger => ServiceRegistry.Get<ILogger>();

        /// <summary>
        /// Gets or sets the menu manager that drives the console menu input/output loop.
        /// </summary>
        public IMenuManager MenuManager
        {
            get => ServiceRegistry.Get<IMenuManager>();
            set => ServiceRegistry.Set(value);
        }

        /// <summary>
        /// Registers a valid command line argument by name with an optional description.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="description">An optional description of the argument.</param>
        public void AddValidArgument(string name, string? description = null)
        {
            AddValidArgument(name, false, description: description);
        }

        /// <summary>
        /// Registers a valid command line argument with additional options.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="allowNull">Whether the argument may have no value.</param>
        /// <param name="addAcronym">Whether to also register a case-acronym of the name.</param>
        /// <param name="description">An optional description of the argument.</param>
        /// <param name="valueExample">An optional example value for usage display.</param>
        public void AddValidArgument(string name, bool allowNull, bool addAcronym = false, string? description = null, string? valueExample = null)
        {
            ValidArgumentInfo.Add(new ArgumentInfo(name, allowNull, description, valueExample));
            if (addAcronym)
            {
                ValidArgumentInfo.Add(new ArgumentInfo(name.CaseAcronym().ToLowerInvariant(), allowNull, $"{description}; same as {name}", valueExample));
            }
        }

        /// <summary>
        /// Gets or sets the parsed arguments passed on the command line.
        /// </summary>
        public IParsedArguments Arguments
        {
            get;
            set;
        }

        protected List<ArgumentInfo> ValidArgumentInfo
        {
            get;
            set;
        }

        protected void Usage(Assembly assembly)
        {
            string assemblyVersion = assembly.GetName().Version.ToString();
            string fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
            string usageFormat = @"Assembly Version: {0}
File Version: {1}

{2} [arguments]";
            FileInfo info = new FileInfo(assembly.Location);
            Message.PrintLine(usageFormat, assemblyVersion, fileVersion, info.Name);
            Thread.Sleep(3);
            foreach (ArgumentInfo argInfo in ValidArgumentInfo)
            {
                string valueExample = string.IsNullOrEmpty(argInfo.ValueExample) ? string.Empty : string.Format(":{0}\r\n", argInfo.ValueExample);
                Message.PrintLine("/{0}{1}\r\n    {2}", argInfo.Name, valueExample, argInfo.Description);
            }
            Thread.Sleep(30);
        }

        /// <summary>
        /// Prints version information for the specified assembly, including assembly version, file version, and commit hash.
        /// </summary>
        /// <param name="assembly">The assembly to display version information for.</param>
        public static void Version(Assembly? assembly)
        {
            FileVersionInfo fv = FileVersionInfo.GetVersionInfo(assembly?.Location);
            AssemblyCommitAttribute? commitAttribute = assembly.GetCustomAttribute<AssemblyCommitAttribute>();
            StringBuilder versionInfo = new StringBuilder();
            versionInfo.AppendFormat("AssemblyVersion: {0}\r\n", assembly.GetName().Version?.ToString());
            versionInfo.AppendFormat("AssemblyFileVersion: {0}\r\n", fv.FileVersion?.ToString());
            if (commitAttribute != null)
            {
                versionInfo.AppendFormat("Commit: {0}\r\n", commitAttribute.Commit);
            }
            else
            {
                versionInfo.AppendFormat("Commit: AssemblyCommitAttribute not found on specified assembly: {0}\r\n",
                    assembly.Location);
            }

            Message.PrintLine(versionInfo.ToString(), ConsoleColor.Cyan);
        }

        /// <summary>
        /// Creates a new <see cref="BamConsoleContext"/> and returns its default service registry.
        /// </summary>
        /// <returns>A new default <see cref="ServiceRegistry"/>.</returns>
        public static ServiceRegistry GetDefaultServiceRegistry()
        {
            return new BamConsoleContext().ServiceRegistry;
        }

        /// <summary>
        /// Builds and returns the default service registry for this console context, including argument parsing, configuration, logging, and menu rendering services.
        /// </summary>
        /// <returns>A configured <see cref="ServiceRegistry"/>.</returns>
        public override ServiceRegistry GetDefaultContextServiceRegistry()
        {
            ServiceRegistry serviceRegistry = new ServiceRegistry()
                .For<IBamContext>().Use(this)
                .For<ArgumentFormatOptions>().Use(ArgumentFormatOptions.Default)
                .For<IArgumentParser>().Use<DefaultArgumentParser>()
                .For<IConfigurationProvider>().Use(new DefaultConfigurationProvider())
                .For<IApplicationNameProvider>().Use(new ProcessApplicationNameProvider())
                .For<ILogger>().Use(new ConsoleLogger());
            
            serviceRegistry.CombineWith(ConsoleMenuOptions.GetServiceRegistry<StringArgumentProvider>());
            
            return serviceRegistry;
        }

        protected void AddSwitches()
        {
            Assembly? entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                foreach (Type type in entryAssembly.GetTypes())
                {
                    AddSwitches(type);
                }
            }
        }

        /// <summary>
        /// Reads the methods of the specified type and adds those adorned with <see cref="ConsoleCommandAttribute" /> as valid command line switches.
        /// </summary>
        /// <param name="type"></param>
        protected void AddSwitches(Type type)
        {
            foreach (MethodInfo method in type.GetMethods())
            {
                if (method.HasCustomAttributeOfType(out ConsoleCommandAttribute attribute))
                {
                    if (!string.IsNullOrEmpty(attribute.OptionName))
                    {
                        AddValidArgument(attribute.OptionName, true, true, attribute.Description, attribute.ValueExample);
                    }

                    if (!string.IsNullOrEmpty(attribute.OptionShortName))
                    {
                        AddValidArgument(attribute.OptionShortName.ToLowerInvariant(), true, description: $"{attribute.Description}; short for {attribute.OptionName}", valueExample: attribute.ValueExample);
                    }

                    AddValidArgument(method.Name.CamelCase(), true, true, attribute.Description, attribute.ValueExample);
                }
            }
        }

        /// <summary>
        /// Reads the configuration settings and adds them as valid command line switches.
        /// </summary>
        protected void AddConfigurationSwitches()
        {
            Dictionary<string, string> configuration = ConfigurationProvider.GetApplicationConfiguration(ApplicationNameProvider.GetApplicationName());

            configuration.Keys.Each(key => AddValidArgument(key, $"Override value from config: {configuration[key]}"));
        }

        protected void ParseArgs(string[] args)
        {
            Arguments = this.ArgumentParser.ParseArguments(args);
            if (Arguments.Status == ArgumentParseStatus.Error || Arguments.Status == ArgumentParseStatus.Invalid)
            {
                ArgsParsedError?.Invoke(Arguments);
            }
            else if (Arguments.Status == ArgumentParseStatus.Success)
            {
                ArgsParsed?.Invoke(Arguments);
            }
        }

        protected static bool ExecuteSwitches(ILogger logger, IParsedArguments arguments)
        {
            Assembly? entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                logger.Info("Entry assembly is null for ({0})", nameof(BamConsoleContext));
                return false;
            }
            bool executed = false;
            foreach (Type type in entryAssembly.GetTypes())
            {
                foreach (string key in arguments.Keys)
                {
                    ConsoleMethod? methodToInvoke = GetConsoleMethod(arguments, type, key);
                    if (methodToInvoke != null)
                    {
                        CheckDebug(arguments);
                        methodToInvoke.TryInvoke((ex) => logger.Error("Exception executing switch ({0})", ex, key));
                        logger.Info("Executed {0}: {1}", key, methodToInvoke.Information);
                        executed = true;
                    }
                }
            }
            return executed;
        }

        private static ConsoleMethod? GetConsoleMethod(IParsedArguments arguments, Type type, string key, object instance = null)
        {
            string commandLineSwitch = key;
            string switchValue = arguments[key];
            MethodInfo[] methods = type.GetMethods();
            List<ConsoleMethod> toExecute = new List<ConsoleMethod>();
            foreach (MethodInfo method in methods)
            {
                if (method.HasCustomAttributeOfType(out ConsoleCommandAttribute consoleAction))
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
                    if (
                        consoleAction.OptionName.Or("").Equals(commandLineSwitch) ||
                        consoleAction.OptionName.CaseAcronym().ToLowerInvariant().Or("").Equals(commandLineSwitch) ||
                        consoleAction.OptionShortName.Or("").Equals(commandLineSwitch, StringComparison.OrdinalIgnoreCase) ||
                        method.Name.CamelCase().Equals(commandLineSwitch) ||
                        method.Name.CamelCase().CaseAcronym().ToLowerInvariant().Equals(commandLineSwitch)
                        )
                    {
                        toExecute.Add(new ConsoleMethod(method, consoleAction, instance, switchValue));
                    }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
            }

            (toExecute.Count > 1).IsFalse("Multiple ConsoleActions found with the specified command line switch: {0}".Format(commandLineSwitch));

            if (toExecute.Count == 0)
            {
                return null;
            }

            return toExecute[0];
        }

        protected static bool ExecuteTestSwitches(ILogger logger, IParsedArguments arguments)
        {
            Assembly? entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                return false;
            }

            if (Current.ServiceRegistry.TryGet<ITestSwitchExecutor>(out var executor))
            {
                return executor.ExecuteTestSwitches(entryAssembly, logger, arguments);
            }

            logger.Warning("No ITestSwitchExecutor registered; ensure bam.test is referenced");
            return false;
        }

        private static void CheckDebug(IParsedArguments arguments)
        {
            if (arguments.Contains("debug"))
            {
                System.Console.WriteLine($"Attach Debugger: ProcessId={Process.GetCurrentProcess().Id}");
                System.Console.ReadLine();
            }
        }
    }
}

/*
	Copyright © Bryan Apellanes 2015
*/

namespace Bam.Console
{

    /// <summary>
    /// Class used to parse command line arguments.  All arguments are
    /// assumed to be in the format --&lt;name&gt;:&lt;value&gt; or an ArgumentException is thrown
    /// during parsing.
    /// </summary>
    public class ParsedArguments : IParsedArguments
    {
        /// <summary>
        /// Gets the default argument prefix string from the default format options.
        /// </summary>
        public static string DefaultArgPrefix => ArgumentFormatOptions.Default.Prefix;

        /// <summary>
        /// Gets the default value separator character from the default format options.
        /// </summary>
        public static char ValueDivider => ArgumentFormatOptions.Default.ValueSeparator;

        /// <summary>
        /// Gets the default argument format options.
        /// </summary>
        public static ArgumentFormatOptions DefaultOptions => ArgumentFormatOptions.Default;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedArguments"/> class using argument name strings as valid arguments.
        /// </summary>
        /// <param name="args">The command line arguments to parse.</param>
        /// <param name="validArgNames">An array of valid argument names.</param>
        public ParsedArguments(string[] args, string[] validArgNames)
            : this(args, ArgumentInfo.FromStringArray(validArgNames))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedArguments"/> class with the specified valid argument infos and default options.
        /// </summary>
        /// <param name="args">The command line arguments to parse.</param>
        /// <param name="validArgumentInfos">The valid argument info entries.</param>
        public ParsedArguments(string[] args, ArgumentInfo[] validArgumentInfos) : this(DefaultOptions, args, validArgumentInfos)
        {
        }

        /// <summary>
        /// Instantiate a ParsedArguments from the command line args specified to the current process.
        /// </summary>
        public ParsedArguments() : this(Environment.GetCommandLineArgs())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedArguments"/> class, treating all arguments in the array as valid.
        /// </summary>
        /// <param name="args">The command line arguments to parse.</param>
        public ParsedArguments(string[] args) : this(DefaultOptions, args, ArgumentInfo.FromArgs(DefaultOptions, args))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedArguments"/> class with a custom argument prefix.
        /// </summary>
        /// <param name="argPrefix">The argument prefix string to use (e.g., "--" or "/").</param>
        /// <param name="args">The command line arguments to parse.</param>
        /// <param name="validArgumentInfos">The valid argument info entries.</param>
        public ParsedArguments(string argPrefix, string[] args, ArgumentInfo[] validArgumentInfos)
            : this(new ArgumentFormatOptions(argPrefix, ValueDivider), args, validArgumentInfos)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsedArguments"/> class with the specified format options and valid argument infos.
        /// </summary>
        /// <param name="options">The argument format options specifying prefix and separator.</param>
        /// <param name="args">The command line arguments to parse.</param>
        /// <param name="validArgumentInfos">The valid argument info entries.</param>
        public ParsedArguments(ArgumentFormatOptions options, string[] args, ArgumentInfo[] validArgumentInfos)
        {
            OriginalStrings = args;
            parsedArguments = new Dictionary<string, string>();
            if (args.Length == 0)
            {
                Status = ArgumentParseStatus.Success;
                Message = "No arguments";
                return;
            }
            ArgumentInfoHash validArguments = new ArgumentInfoHash(validArgumentInfos);

            // Accept all allowed prefixes plus the short prefix, longest first
            // so "--foo" matches "--" before "-"
            string[] prefixes = new HashSet<string>(ArgumentFormatOptions.AllowedPrefixes) { options.ShortPrefix }
                .OrderByDescending(p => p.Length)
                .ToArray();

            foreach (string argument in args)
            {
                string arg = argument.Trim();

                string? matchedPrefix = null;
                foreach (string p in prefixes)
                {
                    if (arg.StartsWith(p) && arg.Length > p.Length)
                    {
                        matchedPrefix = p;
                        break;
                    }
                }

                if (matchedPrefix == null)
                {
                    Message = $"Unrecognized argument format: {arg}\r\n\r\nAll Args:\r\n{string.Join("\r\n", args)}";
                    Status = ArgumentParseStatus.Error;
                }
                else
                {
                    string[] nameValue = arg.Substring(matchedPrefix.Length, arg.Length - matchedPrefix.Length).Split(new string[] { options.ValueSeparator.ToString() }, StringSplitOptions.RemoveEmptyEntries);
                    string name = string.Empty;
                    if (nameValue.Length > 0)
                    {
                        name = nameValue[0];
                    }

                    // allow {ValueSeparator} in arg value
                    if (nameValue.Length > 2)
                    {
                        int startIndex = arg.IndexOf(options.ValueSeparator, StringComparison.Ordinal) + 1;
                        nameValue = new string[] { name, arg.Substring(startIndex, arg.Length - startIndex) };
                    }

                    if (nameValue.Length == 1 && validArguments[name] != null)
                    {
                        if (validArguments[name]!.AllowNullValue)
                        {
                            parsedArguments.Add(name, "");
                        }
                        else
                        {
                            Message = "No value specified for " + name;
                            Status = ArgumentParseStatus.Error;
                        }
                    }

                    if (nameValue.Length == 2)
                    {
                        if (validArguments[name] == null)
                        {
                            Message = "Invalid argument name specified: " + name;
                            Status = ArgumentParseStatus.Error;
                        }
                        else
                        {
                            if (parsedArguments.ContainsKey(name))
                            {
                                parsedArguments[name] = nameValue[1];
                            }
                            else
                            {
                                parsedArguments.Add(name, nameValue[1]);
                            }
                        }
                    }
                }
            }

            if (Status != ArgumentParseStatus.Error)
            {
                Status = ArgumentParseStatus.Success;
            }
        }

        /// <summary>
        /// Ensures the specified argument is present and has a non-empty value; throws if not.
        /// </summary>
        /// <param name="argument">The argument name to check.</param>
        /// <param name="message">The exception message if the argument value is missing.</param>
        public void EnsureArgumentValue(string argument, string message = "Required argument value not specified")
        {
            EnsureArgument(argument);
            Args.ThrowIf(string.IsNullOrEmpty(this[argument]), message);
        }

        /// <summary>
        /// Ensures the specified argument is present; throws if not.
        /// </summary>
        /// <param name="argument">The argument name to check.</param>
        /// <param name="message">The exception message if the argument is missing.</param>
        public void EnsureArgument(string argument, string message = "Required argument not specified")
        {
            Args.ThrowIf(!Contains(argument), message);
        }

        /// <summary>
        /// Determines whether the specified argument name is present in the parsed arguments.
        /// </summary>
        /// <param name="argumentToLookFor">The argument name to search for.</param>
        /// <returns>True if the argument is present; otherwise, false.</returns>
        public bool Contains(string argumentToLookFor)
        {
            return parsedArguments.ContainsKey(argumentToLookFor);
        }

        /// <summary>
        /// Determines whether the specified argument name is present and outputs its value.
        /// </summary>
        /// <param name="argumentToLookFor">The argument name to search for.</param>
        /// <param name="argument">When this method returns, contains the argument value if found; otherwise, null.</param>
        /// <returns>True if the argument is present; otherwise, false.</returns>
        public bool Contains(string argumentToLookFor, out string? argument)
        {
            bool result = parsedArguments.ContainsKey(argumentToLookFor);
            argument = result ? parsedArguments[argumentToLookFor] : null;
            return result;
        }

        /// <summary>
        /// Gets or sets the message describing the parse result (e.g., error details).
        /// </summary>
        public string Message { get; set; } = null!;

        /// <summary>
        /// Gets or sets the status of the argument parsing operation.
        /// </summary>
        public ArgumentParseStatus Status { get; set; }

        /// <summary>
        /// Gets the original string array that was parsed.
        /// </summary>
        public string[] OriginalStrings { get; private set; }

        readonly Dictionary<string, string> parsedArguments;

        /// <summary>
        /// Gets or sets the value of the argument with the specified name.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <returns>The argument value, or null if not found.</returns>
        public string this[string name]
        {
            get => parsedArguments.ContainsKey(name) ? parsedArguments[name] : null!;
            set => parsedArguments[name] = value;
        }

        /// <summary>
        /// Gets all parsed argument names.
        /// </summary>
        public string[] Keys => parsedArguments.Keys.ToArray();

        /// <summary>
        /// Gets the number of parsed arguments.
        /// </summary>
        public int Length => parsedArguments.Count;

        private static ParsedArguments _current = null!;
        static readonly object _currentLock = new object();

        /// <summary>
        /// Gets the singleton instance of <see cref="ParsedArguments"/>, lazily initialized from the current process command line arguments.
        /// </summary>
        public static ParsedArguments Current => _currentLock.DoubleCheckLock(ref _current, () => new ParsedArguments());
    }
}

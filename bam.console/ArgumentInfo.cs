/*
	Copyright © Bryan Apellanes 2015
*/

namespace Bam.Console
{
    /// <summary>
    /// Describes a single command line argument, including its name, whether it allows null values, a description, and an example value.
    /// </summary>
    public class ArgumentInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentInfo"/> class with the specified name and options.
        /// </summary>
        /// <param name="name">The argument name.</param>
        /// <param name="allowNull">Whether the argument may have no value.</param>
        /// <param name="description">An optional description of the argument.</param>
        /// <param name="valueExample">An optional example value for display in usage help.</param>
        public ArgumentInfo(string name, bool allowNull, string? description = null, string? valueExample = null)
        {
            Name = name;
            AllowNullValue = allowNull;
            Description = description;
            ValueExample = valueExample;
        }

        /// <summary>
        /// Gets or sets an example value for this argument, displayed in usage help.
        /// </summary>
        public string? ValueExample { get; set; }

        /// <summary>
        /// Gets or sets the description of this argument.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the name of this argument.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this argument may have a null (empty) value.
        /// </summary>
        public bool AllowNullValue { get; set; }

        /// <summary>
        /// Returns a string representation of this argument info in the format "Name(Description)".
        /// </summary>
        /// <returns>A string representation of this argument info.</returns>
        public override string ToString()
        {
            return $"{Name}({Description})";
        }

        /// <summary>
        /// Creates an array of <see cref="ArgumentInfo"/> instances from an array of argument names, with null values disallowed.
        /// </summary>
        /// <param name="argumentNames">The argument names.</param>
        /// <returns>An array of <see cref="ArgumentInfo"/> instances.</returns>
        public static ArgumentInfo[] FromStringArray(string[] argumentNames)
        {
            return FromStringArray(argumentNames, false);
        }

        /// <summary>
        /// Creates an array of <see cref="ArgumentInfo"/> instances from an array of argument names.
        /// </summary>
        /// <param name="argumentNames">The argument names.</param>
        /// <param name="allowNulls">Whether null values are allowed for all arguments.</param>
        /// <returns>An array of <see cref="ArgumentInfo"/> instances.</returns>
        public static ArgumentInfo[] FromStringArray(string[] argumentNames, bool allowNulls)
        {
            List<ArgumentInfo> retVal = new List<ArgumentInfo>(argumentNames.Length);
            foreach (string name in argumentNames)
            {
                retVal.Add(new ArgumentInfo(name, allowNulls));
            }

            return retVal.ToArray();
        }

        /// <summary>
        /// Creates a default set of <see cref="ArgumentInfo"/> instances from the specified args array using the default format options.
        /// </summary>
        /// <param name="args">The command line arguments to derive argument info from.</param>
        /// <param name="allowNulls">Whether null values are allowed for all arguments.</param>
        /// <returns>An array of <see cref="ArgumentInfo"/> instances.</returns>
        public static ArgumentInfo[] FromArgs(string[] args, bool allowNulls = true)
        {
            return FromArgs(ArgumentFormatOptions.Default, args, allowNulls);
        }

        /// <summary>
        /// Creates an array of <see cref="ArgumentInfo"/> instances from the specified args array using a custom prefix.
        /// </summary>
        /// <param name="prefix">The argument prefix string (e.g., "--" or "/").</param>
        /// <param name="args">The command line arguments to derive argument info from.</param>
        /// <param name="allowNulls">Whether null values are allowed for all arguments.</param>
        /// <returns>An array of <see cref="ArgumentInfo"/> instances.</returns>
        public static ArgumentInfo[] FromArgs(string prefix, string[] args, bool allowNulls)
        {
            return FromArgs(new ArgumentFormatOptions(prefix, ParsedArguments.ValueDivider), args, allowNulls);
        }

        /// <summary>
        /// Creates an array of <see cref="ArgumentInfo"/> instances from the specified args array using the specified format options.
        /// </summary>
        /// <param name="options">The argument format options specifying prefix and separator.</param>
        /// <param name="args">The command line arguments to derive argument info from.</param>
        /// <param name="allowNulls">Whether null values are allowed for all arguments.</param>
        /// <returns>An array of <see cref="ArgumentInfo"/> instances.</returns>
        public static ArgumentInfo[] FromArgs(ArgumentFormatOptions options, string[] args, bool allowNulls = true)
        {
            // Accept all allowed prefixes plus the short prefix, longest first
            // so "--foo" matches "--" before "-"
            string[] prefixes = new HashSet<string>(ArgumentFormatOptions.AllowedPrefixes) { options.ShortPrefix }
                .OrderByDescending(p => p.Length)
                .ToArray();

            List<ArgumentInfo> results = new List<ArgumentInfo>();
            foreach (string arg in args)
            {
                string? matchedPrefix = null;
                foreach (string p in prefixes)
                {
                    if (arg.StartsWith(p))
                    {
                        matchedPrefix = p;
                        break;
                    }
                }

                if (matchedPrefix == null)
                {
                    Message.PrintLine("Unrecognized argument: {0}", ConsoleColor.Yellow, arg);
                    continue;
                }

                string name = arg.TruncateFront(matchedPrefix.Length).ReadUntil(options.ValueSeparator);
                results.Add(new ArgumentInfo(name, allowNulls));
            }

            return results.ToArray();
        }
    }
}

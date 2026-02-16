namespace Bam.Console
{
    /// <summary>
    /// Defines the formatting options for parsing command line arguments, including the argument prefix and value separator characters.
    /// </summary>
    public class ArgumentFormatOptions
    {
        /// <summary>
        /// The set of allowed argument prefixes.
        /// </summary>
        public static readonly string[] AllowedPrefixes = { "--", "-", "/" };

        /// <summary>
        /// The set of allowed value separator characters.
        /// </summary>
        public static readonly char[] AllowedSeparators = { '=', ':' };

        /// <summary>
        /// Gets the default argument format options, which use "/" and "=" on Windows, or "--" and ":" on other platforms.
        /// </summary>
        public static ArgumentFormatOptions Default { get; } = RuntimeSettings.IsWindows
            ? new ArgumentFormatOptions("/", '=')
            : new ArgumentFormatOptions("--", ':');

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentFormatOptions"/> class using the default prefix and value separator.
        /// </summary>
        public ArgumentFormatOptions() : this(Default.Prefix, Default.ValueSeparator) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentFormatOptions"/> class with the specified prefix and value separator.
        /// </summary>
        /// <param name="prefix">The argument prefix string (e.g., "--", "-", or "/").</param>
        /// <param name="valueSeparator">The character that separates argument names from values (e.g., '=' or ':').</param>
        public ArgumentFormatOptions(string prefix, char valueSeparator)
        {
            Args.ThrowIf(!AllowedPrefixes.Contains(prefix),
                "Invalid prefix '{0}'. Allowed: {1}", prefix, string.Join(", ", AllowedPrefixes));
            Args.ThrowIf(!AllowedSeparators.Contains(valueSeparator),
                "Invalid separator '{0}'. Allowed: {1}", valueSeparator, string.Join(", ", AllowedSeparators));

            Prefix = prefix;
            ValueSeparator = valueSeparator;
        }

        /// <summary>
        /// Gets or sets the primary argument prefix string.
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the short prefix string used for abbreviated argument names.
        /// </summary>
        public string ShortPrefix { get; set; } = "-";

        /// <summary>
        /// Gets or sets the character that separates an argument name from its value.
        /// </summary>
        public char ValueSeparator { get; set; }
    }
}

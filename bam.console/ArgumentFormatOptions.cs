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
        /// GNU/POSIX style: --name=value (long), -n (short).
        /// </summary>
        public static ArgumentFormatOptions Posix { get; } = new("--", '=');

        /// <summary>
        /// Microsoft/Windows style: /name:value.
        /// </summary>
        public static ArgumentFormatOptions Windows { get; } = new("/", ':');

        /// <summary>
        /// Auto-detected platform style. Uses Windows style on Windows, Posix style on Unix/macOS.
        /// Can be overridden via the BAM_ARG_STYLE environment variable (values: "Posix", "Windows").
        /// </summary>
        public static ArgumentFormatOptions Platform { get; } = ResolvePlatformStyle();

        /// <summary>
        /// Gets the default argument format options, resolved from the current platform (or BAM_ARG_STYLE override).
        /// </summary>
        public static ArgumentFormatOptions Default { get; } = Platform;

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
        /// Creates an <see cref="ArgumentFormatOptions"/> instance for the specified <see cref="ArgumentStyle"/>.
        /// </summary>
        /// <param name="style">The argument style to use.</param>
        /// <returns>The corresponding <see cref="ArgumentFormatOptions"/>.</returns>
        public static ArgumentFormatOptions ForStyle(ArgumentStyle style) => style switch
        {
            ArgumentStyle.Posix => Posix,
            ArgumentStyle.Windows => Windows,
            ArgumentStyle.Platform => Platform,
            _ => Platform
        };

        /// <summary>
        /// Gets the primary argument prefix string.
        /// </summary>
        public string Prefix { get; init; }

        /// <summary>
        /// Gets the short prefix string used for abbreviated argument names.
        /// </summary>
        public string ShortPrefix { get; init; } = "-";

        /// <summary>
        /// Gets the character that separates an argument name from its value.
        /// </summary>
        public char ValueSeparator { get; init; }

        private static ArgumentFormatOptions ResolvePlatformStyle()
        {
            string? envOverride = Environment.GetEnvironmentVariable("BAM_ARG_STYLE");
            if (!string.IsNullOrEmpty(envOverride))
            {
                if (envOverride.Equals("Posix", StringComparison.OrdinalIgnoreCase))
                {
                    return new ArgumentFormatOptions("--", '=');
                }
                if (envOverride.Equals("Windows", StringComparison.OrdinalIgnoreCase))
                {
                    return new ArgumentFormatOptions("/", ':');
                }
            }

            return RuntimeSettings.IsWindows
                ? new ArgumentFormatOptions("/", ':')
                : new ArgumentFormatOptions("--", '=');
        }
    }
}

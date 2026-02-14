namespace Bam.Console
{
    public class ArgumentFormatOptions
    {
        public static readonly string[] AllowedPrefixes = { "--", "-", "/" };
        public static readonly char[] AllowedSeparators = { '=', ':' };

        public static ArgumentFormatOptions Default { get; } = RuntimeSettings.IsWindows
            ? new ArgumentFormatOptions("/", '=')
            : new ArgumentFormatOptions("--", ':');

        public ArgumentFormatOptions() : this(Default.Prefix, Default.ValueSeparator) { }

        public ArgumentFormatOptions(string prefix, char valueSeparator)
        {
            Args.ThrowIf(!AllowedPrefixes.Contains(prefix),
                "Invalid prefix '{0}'. Allowed: {1}", prefix, string.Join(", ", AllowedPrefixes));
            Args.ThrowIf(!AllowedSeparators.Contains(valueSeparator),
                "Invalid separator '{0}'. Allowed: {1}", valueSeparator, string.Join(", ", AllowedSeparators));

            Prefix = prefix;
            ValueSeparator = valueSeparator;
        }

        public string Prefix { get; set; }
        public string ShortPrefix { get; set; } = "-";
        public char ValueSeparator { get; set; }
    }
}

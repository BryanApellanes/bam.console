/*
	Copyright © Bryan Apellanes 2015
*/

namespace Bam.Console
{
    public class ArgumentInfo
    {
        public ArgumentInfo(string name, bool allowNull, string? description = null, string? valueExample = null)
        {
            Name = name;
            AllowNullValue = allowNull;
            Description = description;
            ValueExample = valueExample;
        }

        public string? ValueExample { get; set; }
        public string? Description { get; set; }
        public string Name { get; set; }
        public bool AllowNullValue { get; set; }

        public override string ToString()
        {
            return $"{Name}({Description})";
        }

        public static ArgumentInfo[] FromStringArray(string[] argumentNames)
        {
            return FromStringArray(argumentNames, false);
        }

        public static ArgumentInfo[] FromStringArray(string[] argumentNames, bool allowNulls)
        {
            List<ArgumentInfo> retVal = new List<ArgumentInfo>(argumentNames.Length);
            foreach (string name in argumentNames)
            {
                retVal.Add(new ArgumentInfo(name, allowNulls));
            }

            return retVal.ToArray();
        }

        public static ArgumentInfo[] FromArgs(string[] args, bool allowNulls = true)
        {
            return FromArgs(ArgumentFormatOptions.Default, args, allowNulls);
        }

        /// <summary>
        /// Creates a default set of ArgumentInfo instances from the specified args array.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static ArgumentInfo[] FromArgs(string prefix, string[] args, bool allowNulls)
        {
            return FromArgs(new ArgumentFormatOptions(prefix, ParsedArguments.ValueDivider), args, allowNulls);
        }

        public static ArgumentInfo[] FromArgs(ArgumentFormatOptions options, string[] args, bool allowNulls = true)
        {
            List<ArgumentInfo> results = new List<ArgumentInfo>();
            foreach (string arg in args)
            {
                if (!arg.StartsWith(options.Prefix))
                {
                    Message.PrintLine("Unrecognized argument: {0}", ConsoleColor.Yellow, arg);
                    continue;
                }

                string name = arg.TruncateFront(options.Prefix.Length).ReadUntil(options.ValueSeparator);
                results.Add(new ArgumentInfo(name, allowNulls));
            }

            return results.ToArray();
        }
    }
}

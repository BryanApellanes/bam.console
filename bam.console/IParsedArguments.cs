/*
	Copyright © Bryan Apellanes 2015  
*/
namespace Bam.Console
{
    public interface IParsedArguments
    {
        string this[string name] { get; set; }

        string[] Keys { get; }
        int Length { get; }
        string Message { get; set; }
        string[] OriginalStrings { get; }
        ArgumentParseStatus Status { get; set; }

        bool Contains(string argumentToLookFor);

        bool Contains(string argumentToLookFor, out string? argument);
    }
}
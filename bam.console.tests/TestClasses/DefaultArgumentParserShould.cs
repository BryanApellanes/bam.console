using Bam.Test;

namespace Bam.Console.Tests.TestClasses;

[UnitTestMenu("DefaultArgumentParserShould", "daps")]
public class DefaultArgumentParserShould : UnitTestMenuContainer
{
    [UnitTest]
    public void ParseWithPosixOptions()
    {
        When.A<DefaultArgumentParser>("parses with Posix options",
            () => new DefaultArgumentParser(ArgumentFormatOptions.Posix),
            (parser) =>
            {
                return parser.ParseArguments(new[] { "--name=value" });
            })
        .TheTest
        .ShouldPass(because =>
        {
            IParsedArguments result = because.ResultAs<IParsedArguments>();
            because.ItsTrue("status is Success", result.Status == ArgumentParseStatus.Success);
            because.ItsTrue("name has correct value", result["name"] == "value");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ParseWithWindowsOptions()
    {
        When.A<DefaultArgumentParser>("parses with Windows options",
            () => new DefaultArgumentParser(ArgumentFormatOptions.Windows),
            (parser) =>
            {
                return parser.ParseArguments(new[] { "/name:value" });
            })
        .TheTest
        .ShouldPass(because =>
        {
            IParsedArguments result = because.ResultAs<IParsedArguments>();
            because.ItsTrue("status is Success", result.Status == ArgumentParseStatus.Success);
            because.ItsTrue("name has correct value", result["name"] == "value");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ParseWithCustomOptions()
    {
        When.A<DefaultArgumentParser>("parses with custom slash-colon options",
            () => new DefaultArgumentParser(new ArgumentFormatOptions("/", ':')),
            (parser) =>
            {
                return parser.ParseArguments(new[] { "/name:value" });
            })
        .TheTest
        .ShouldPass(because =>
        {
            IParsedArguments result = because.ResultAs<IParsedArguments>();
            because.ItsTrue("status is Success", result.Status == ArgumentParseStatus.Success);
            because.ItsTrue("name has correct value", result["name"] == "value");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ReturnSuccessForEmptyArgs()
    {
        When.A<DefaultArgumentParser>("returns Success for empty args", (parser) =>
        {
            return parser.ParseArguments(Array.Empty<string>());
        })
        .TheTest
        .ShouldPass(because =>
        {
            IParsedArguments result = because.ResultAs<IParsedArguments>();
            because.ItsTrue("status is Success", result.Status == ArgumentParseStatus.Success);
            because.ItsTrue("length is 0", result.Length == 0);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }
}

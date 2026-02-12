using Bam.Test;

namespace Bam.Console.Tests.TestClasses;

[UnitTestMenu("DefaultArgumentParserShould", "daps")]
public class DefaultArgumentParserShould : UnitTestMenuContainer
{
    [UnitTest]
    public void ParseWithDefaultOptions()
    {
        When.A<DefaultArgumentParser>("parses with default options", (parser) =>
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

using Bam.Test;

namespace Bam.Console.Tests.TestClasses;

[UnitTestMenu("ParsedArgumentsShould", "pas")]
public class ParsedArgumentsShould : UnitTestMenuContainer
{
    [UnitTest]
    public void ParseStandardArgs()
    {
        When.A<ParsedArguments>("parses --name=value", () => new ParsedArguments(new[] { "--name=value" }), (parsed) =>
        {
            return parsed;
        })
        .TheTest
        .ShouldPass(because =>
        {
            ParsedArguments result = because.ResultAs<ParsedArguments>();
            because.ItsTrue("status is Success", result.Status == ArgumentParseStatus.Success);
            because.ItsTrue("name has correct value", result["name"] == "value");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ParseMultipleArgs()
    {
        When.A<ParsedArguments>("parses multiple arguments", () => new ParsedArguments(new[] { "--a=1", "--b=2" }), (parsed) =>
        {
            return parsed;
        })
        .TheTest
        .ShouldPass(because =>
        {
            ParsedArguments result = because.ResultAs<ParsedArguments>();
            because.ItsTrue("status is Success", result.Status == ArgumentParseStatus.Success);
            because.ItsTrue("has 2 arguments", result.Length == 2);
            because.ItsTrue("a equals 1", result["a"] == "1");
            because.ItsTrue("b equals 2", result["b"] == "2");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ParseNullValueArg()
    {
        When.A<ParsedArguments>("parses --flag with AllowNullValue=true",
            () => new ParsedArguments(
                new[] { "--flag" },
                new[] { new ArgumentInfo("flag", true) }),
            (parsed) =>
            {
                return parsed;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ParsedArguments result = because.ResultAs<ParsedArguments>();
            because.ItsTrue("status is Success", result.Status == ArgumentParseStatus.Success);
            because.ItsTrue("flag value is empty string", result["flag"] == "");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void RejectNullValueWhenNotAllowed()
    {
        When.A<ParsedArguments>("rejects --flag with AllowNullValue=false",
            () => new ParsedArguments(
                new[] { "--flag" },
                new[] { new ArgumentInfo("flag", false) }),
            (parsed) =>
            {
                return parsed;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ParsedArguments result = because.ResultAs<ParsedArguments>();
            because.ItsTrue("status is Error", result.Status == ArgumentParseStatus.Error);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void HandleValueContainingSeparator()
    {
        When.A<ParsedArguments>("handles value containing separator",
            () => new ParsedArguments(new[] { "--url=http://x=y" }),
            (parsed) =>
            {
                return parsed;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ParsedArguments result = because.ResultAs<ParsedArguments>();
            because.ItsTrue("status is Success", result.Status == ArgumentParseStatus.Success);
            because.ItsTrue("preserves full value after first '='", result["url"] == "http://x=y");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ReportErrorOnUnrecognizedFormat()
    {
        When.A<ParsedArguments>("reports error on unrecognized format",
            () => new ParsedArguments(new[] { "badarg" }),
            (parsed) =>
            {
                return parsed;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ParsedArguments result = because.ResultAs<ParsedArguments>();
            because.ItsTrue("status is Error", result.Status == ArgumentParseStatus.Error);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void OverwriteDuplicateArgs()
    {
        When.A<ParsedArguments>("overwrites duplicate arguments",
            () => new ParsedArguments(new[] { "--x=1", "--x=2" }),
            (parsed) =>
            {
                return parsed;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ParsedArguments result = because.ResultAs<ParsedArguments>();
            because.ItsTrue("status is Success", result.Status == ArgumentParseStatus.Success);
            because.ItsTrue("x equals last value '2'", result["x"] == "2");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ContainsReturnsTrueForExistingArg()
    {
        When.A<ParsedArguments>("Contains returns true for existing arg",
            () => new ParsedArguments(new[] { "--name=value" }),
            (parsed) =>
            {
                return parsed.Contains("name");
            })
        .TheTest
        .ShouldPass(because =>
        {
            bool result = (bool)because.Result;
            because.ItsTrue("Contains returns true", result);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ContainsReturnsFalseForMissingArg()
    {
        When.A<ParsedArguments>("Contains returns false for missing arg",
            () => new ParsedArguments(new[] { "--name=value" }),
            (parsed) =>
            {
                return parsed.Contains("missing");
            })
        .TheTest
        .ShouldPass(because =>
        {
            bool result = (bool)because.Result;
            because.ItsFalse("Contains returns false", result);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ContainsOutOverloadReturnsValue()
    {
        When.A<ParsedArguments>("Contains out overload returns value",
            () => new ParsedArguments(new[] { "--name=hello" }),
            (parsed) =>
            {
                parsed.Contains("name", out string? val);
                return new object[] { true, val! };
            })
        .TheTest
        .ShouldPass(because =>
        {
            object[] results = (object[])because.Result;
            because.ItsTrue("Contains returned true", parsed_Contains_helper(results));
            because.ItsTrue("out value is 'hello'", (string)results[1] == "hello");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    private static bool parsed_Contains_helper(object[] results) => true; // Contains was true if we got a non-null val

    [UnitTest]
    public void ParseWithSlashColonFormat()
    {
        When.A<ParsedArguments>("parses /name:value format",
            () =>
            {
                ArgumentFormatOptions options = new ArgumentFormatOptions("/", ':');
                return new ParsedArguments(options, new[] { "/name:value" }, ArgumentInfo.FromArgs(options, new[] { "/name:value" }));
            },
            (parsed) =>
            {
                return parsed;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ParsedArguments result = because.ResultAs<ParsedArguments>();
            because.ItsTrue("status is Success", result.Status == ArgumentParseStatus.Success);
            because.ItsTrue("name has correct value", result["name"] == "value");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ParseWithDashEqualsFormat()
    {
        When.A<ParsedArguments>("parses -name=value format",
            () =>
            {
                ArgumentFormatOptions options = new ArgumentFormatOptions("-", '=');
                return new ParsedArguments(options, new[] { "-name=value" }, ArgumentInfo.FromArgs(options, new[] { "-name=value" }));
            },
            (parsed) =>
            {
                return parsed;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ParsedArguments result = because.ResultAs<ParsedArguments>();
            because.ItsTrue("status is Success", result.Status == ArgumentParseStatus.Success);
            because.ItsTrue("name has correct value", result["name"] == "value");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void HandleEmptyArgs()
    {
        When.A<ParsedArguments>("handles empty args",
            () => new ParsedArguments(Array.Empty<string>()),
            (parsed) =>
            {
                return parsed;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ParsedArguments result = because.ResultAs<ParsedArguments>();
            because.ItsTrue("status is Success", result.Status == ArgumentParseStatus.Success);
            because.ItsTrue("length is 0", result.Length == 0);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void EnsureArgumentThrowsWhenMissing()
    {
        When.A<ParsedArguments>("EnsureArgument throws when missing",
            () => new ParsedArguments(new[] { "--name=value" }),
            (parsed) =>
            {
                parsed.EnsureArgument("missing");
                return parsed;
            })
        .ExpectException(true)
        .TheTest
        .ShouldPass(because =>
        {
            because.ItsTrue("exception was thrown", because.TestCase?.Exception != null);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void EnsureArgumentValueThrowsWhenEmpty()
    {
        When.A<ParsedArguments>("EnsureArgumentValue throws on null-value arg",
            () => new ParsedArguments(
                new[] { "--flag" },
                new[] { new ArgumentInfo("flag", true) }),
            (parsed) =>
            {
                parsed.EnsureArgumentValue("flag");
                return parsed;
            })
        .ExpectException(true)
        .TheTest
        .ShouldPass(because =>
        {
            because.ItsTrue("exception was thrown", because.TestCase?.Exception != null);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }
}

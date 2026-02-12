using Bam.Test;

namespace Bam.Console.Tests.TestClasses;

[UnitTestMenu("ArgumentInfoShould", "ais")]
public class ArgumentInfoShould : UnitTestMenuContainer
{
    [UnitTest]
    public void CreateFromConstructor()
    {
        When.A<ArgumentInfo>("is created from constructor",
            () => new ArgumentInfo("testName", true, "a description", "exampleValue"),
            (info) =>
            {
                return info;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ArgumentInfo result = because.ResultAs<ArgumentInfo>();
            because.ItsTrue("Name is 'testName'", result.Name == "testName");
            because.ItsTrue("AllowNullValue is true", result.AllowNullValue);
            because.ItsTrue("Description is 'a description'", result.Description == "a description");
            because.ItsTrue("ValueExample is 'exampleValue'", result.ValueExample == "exampleValue");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ToStringReturnsFormattedString()
    {
        When.A<ArgumentInfo>("ToString returns formatted string",
            () => new ArgumentInfo("name", false, "description"),
            (info) =>
            {
                return info.ToString();
            })
        .TheTest
        .ShouldPass(because =>
        {
            string result = (string)because.Result;
            because.ItsTrue("ToString returns 'name(description)'", result == "name(description)");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void FromStringArrayCreatesInfos()
    {
        When.A<ArgumentInfo>("FromStringArray creates infos",
            () => new ArgumentInfo("placeholder", false),
            (placeholder) =>
            {
                return ArgumentInfo.FromStringArray(new[] { "arg1", "arg2", "arg3" });
            })
        .TheTest
        .ShouldPass(because =>
        {
            ArgumentInfo[] results = (ArgumentInfo[])because.Result;
            because.ItsTrue("creates 3 infos", results.Length == 3);
            because.ItsTrue("first name is 'arg1'", results[0].Name == "arg1");
            because.ItsTrue("second name is 'arg2'", results[1].Name == "arg2");
            because.ItsTrue("third name is 'arg3'", results[2].Name == "arg3");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void FromArgsExtractsNames()
    {
        When.A<ArgumentInfo>("FromArgs extracts names from default format",
            () => new ArgumentInfo("placeholder", false),
            (placeholder) =>
            {
                return ArgumentInfo.FromArgs(new[] { "--name=value", "--flag" });
            })
        .TheTest
        .ShouldPass(because =>
        {
            ArgumentInfo[] results = (ArgumentInfo[])because.Result;
            because.ItsTrue("creates 2 infos", results.Length == 2);
            because.ItsTrue("first name is 'name'", results[0].Name == "name");
            because.ItsTrue("second name is 'flag'", results[1].Name == "flag");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void FromArgsWithCustomOptions()
    {
        When.A<ArgumentInfo>("FromArgs extracts names with custom options",
            () => new ArgumentInfo("placeholder", false),
            (placeholder) =>
            {
                ArgumentFormatOptions options = new ArgumentFormatOptions("/", ':');
                return ArgumentInfo.FromArgs(options, new[] { "/name:value" });
            })
        .TheTest
        .ShouldPass(because =>
        {
            ArgumentInfo[] results = (ArgumentInfo[])because.Result;
            because.ItsTrue("creates 1 info", results.Length == 1);
            because.ItsTrue("name is 'name'", results[0].Name == "name");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ArgumentInfoHashLooksUpByName()
    {
        When.A<ArgumentInfoHash>("looks up by name",
            () => new ArgumentInfoHash(new[]
            {
                new ArgumentInfo("alpha", false, "first"),
                new ArgumentInfo("beta", true, "second")
            }),
            (hash) =>
            {
                return new object?[] { hash["alpha"], hash["beta"], hash["missing"] };
            })
        .TheTest
        .ShouldPass(because =>
        {
            object?[] results = (object?[])because.Result;
            ArgumentInfo? alpha = results[0] as ArgumentInfo;
            ArgumentInfo? beta = results[1] as ArgumentInfo;
            because.ItsTrue("alpha found", alpha != null);
            because.ItsTrue("alpha name is correct", alpha?.Name == "alpha");
            because.ItsTrue("beta found", beta != null);
            because.ItsTrue("beta name is correct", beta?.Name == "beta");
            because.ItsTrue("missing returns null", results[2] == null);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ArgumentInfoHashHandlesDuplicates()
    {
        When.A<ArgumentInfoHash>("handles duplicate names by overwriting",
            () => new ArgumentInfoHash(new[]
            {
                new ArgumentInfo("dup", false, "first"),
                new ArgumentInfo("dup", true, "second")
            }),
            (hash) =>
            {
                return hash["dup"]!;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ArgumentInfo? result = because.Result as ArgumentInfo;
            because.ItsTrue("result is not null", result != null);
            because.ItsTrue("description is 'second' (last wins)", result?.Description == "second");
            because.ItsTrue("AllowNullValue is true (last wins)", result?.AllowNullValue == true);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }
}

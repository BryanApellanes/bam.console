using Bam.Test;

namespace Bam.Console.Tests.TestClasses;

[UnitTestMenu("ArgumentFormatOptionsShould", "afos")]
public class ArgumentFormatOptionsShould : UnitTestMenuContainer
{
    [UnitTest]
    public void DefaultHasExpectedValues()
    {
        When.A<ArgumentFormatOptions>("has expected default values", () => ArgumentFormatOptions.Default, (options) =>
        {
            return options;
        })
        .TheTest
        .ShouldPass(because =>
        {
            ArgumentFormatOptions result = because.ResultAs<ArgumentFormatOptions>();
            because.ItsTrue("default prefix is '--'", result.Prefix == "--");
            because.ItsTrue("default value separator is '='", result.ValueSeparator == '=');
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void AcceptAllValidPrefixes()
    {
        When.A<ArgumentFormatOptions>("accepts all valid prefixes", (defaultOptions) =>
        {
            ArgumentFormatOptions singleDash = new ArgumentFormatOptions("-", '=');
            ArgumentFormatOptions slash = new ArgumentFormatOptions("/", '=');
            return new object[] { defaultOptions, singleDash, slash };
        })
        .TheTest
        .ShouldPass(because =>
        {
            object[] results = (object[])because.Result;
            ArgumentFormatOptions doubleDash = (ArgumentFormatOptions)results[0];
            ArgumentFormatOptions singleDash = (ArgumentFormatOptions)results[1];
            ArgumentFormatOptions slash = (ArgumentFormatOptions)results[2];
            because.ItsTrue("'--' prefix accepted", doubleDash.Prefix == "--");
            because.ItsTrue("'-' prefix accepted", singleDash.Prefix == "-");
            because.ItsTrue("'/' prefix accepted", slash.Prefix == "/");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void AcceptAllValidSeparators()
    {
        When.A<ArgumentFormatOptions>("accepts all valid separators", (defaultOptions) =>
        {
            ArgumentFormatOptions colon = new ArgumentFormatOptions("--", ':');
            return new object[] { defaultOptions, colon };
        })
        .TheTest
        .ShouldPass(because =>
        {
            object[] results = (object[])because.Result;
            ArgumentFormatOptions equals = (ArgumentFormatOptions)results[0];
            ArgumentFormatOptions colon = (ArgumentFormatOptions)results[1];
            because.ItsTrue("'=' separator accepted", equals.ValueSeparator == '=');
            because.ItsTrue("':' separator accepted", colon.ValueSeparator == ':');
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ThrowOnInvalidPrefix()
    {
        When.A<ArgumentFormatOptions>("throws on invalid prefix '##'", (defaultOptions) =>
        {
            new ArgumentFormatOptions("##", '=');
            return defaultOptions;
        })
        .ExpectException(true)
        .TheTest
        .ShouldPass(because =>
        {
            because.ItsTrue("exception was thrown for invalid prefix", because.TestCase?.Exception != null);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ThrowOnInvalidSeparator()
    {
        When.A<ArgumentFormatOptions>("throws on invalid separator '|'", (defaultOptions) =>
        {
            new ArgumentFormatOptions("--", '|');
            return defaultOptions;
        })
        .ExpectException(true)
        .TheTest
        .ShouldPass(because =>
        {
            because.ItsTrue("exception was thrown for invalid separator", because.TestCase?.Exception != null);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ParameterlessConstructorUsesDefaults()
    {
        When.A<ArgumentFormatOptions>("created with parameterless constructor uses defaults", (options) =>
        {
            return options;
        })
        .TheTest
        .ShouldPass(because =>
        {
            ArgumentFormatOptions result = because.ResultAs<ArgumentFormatOptions>();
            because.ItsTrue("prefix matches default '--'", result.Prefix == ArgumentFormatOptions.Default.Prefix);
            because.ItsTrue("separator matches default '='", result.ValueSeparator == ArgumentFormatOptions.Default.ValueSeparator);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }
}

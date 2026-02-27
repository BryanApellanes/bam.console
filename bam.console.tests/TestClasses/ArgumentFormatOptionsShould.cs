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
            if (RuntimeSettings.IsWindows)
            {
                because.ItsTrue("default prefix is '/' on Windows", result.Prefix == "/");
                because.ItsTrue("default value separator is ':' on Windows", result.ValueSeparator == ':');
            }
            else
            {
                because.ItsTrue("default prefix is '--' on non-Windows", result.Prefix == "--");
                because.ItsTrue("default value separator is '=' on non-Windows", result.ValueSeparator == '=');
            }
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ShortPrefixIsAlwaysDash()
    {
        When.A<ArgumentFormatOptions>("ShortPrefix is always '-'", () => ArgumentFormatOptions.Default, (options) =>
        {
            return options;
        })
        .TheTest
        .ShouldPass(because =>
        {
            ArgumentFormatOptions result = because.ResultAs<ArgumentFormatOptions>();
            because.ItsTrue("ShortPrefix is '-'", result.ShortPrefix == "-");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void AcceptAllValidPrefixes()
    {
        When.A<ArgumentFormatOptions>("accepts all valid prefixes", (defaultOptions) =>
        {
            ArgumentFormatOptions doubleDash = new ArgumentFormatOptions("--", '=');
            ArgumentFormatOptions singleDash = new ArgumentFormatOptions("-", '=');
            ArgumentFormatOptions slash = new ArgumentFormatOptions("/", '=');
            return new object[] { doubleDash, singleDash, slash };
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
            ArgumentFormatOptions equals = new ArgumentFormatOptions("--", '=');
            ArgumentFormatOptions colon = new ArgumentFormatOptions("--", ':');
            return new object[] { equals, colon };
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
            because.ItsTrue("prefix matches default", result.Prefix == ArgumentFormatOptions.Default.Prefix);
            because.ItsTrue("separator matches default", result.ValueSeparator == ArgumentFormatOptions.Default.ValueSeparator);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void PosixStyleHasDoubleDashAndEquals()
    {
        When.A<ArgumentFormatOptions>("Posix preset has '--' and '='", () => ArgumentFormatOptions.Posix, (options) =>
        {
            return options;
        })
        .TheTest
        .ShouldPass(because =>
        {
            ArgumentFormatOptions result = because.ResultAs<ArgumentFormatOptions>();
            because.ItsTrue("Posix prefix is '--'", result.Prefix == "--");
            because.ItsTrue("Posix separator is '='", result.ValueSeparator == '=');
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void WindowsStyleHasSlashAndColon()
    {
        When.A<ArgumentFormatOptions>("Windows preset has '/' and ':'", () => ArgumentFormatOptions.Windows, (options) =>
        {
            return options;
        })
        .TheTest
        .ShouldPass(because =>
        {
            ArgumentFormatOptions result = because.ResultAs<ArgumentFormatOptions>();
            because.ItsTrue("Windows prefix is '/'", result.Prefix == "/");
            because.ItsTrue("Windows separator is ':'", result.ValueSeparator == ':');
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void PlatformStyleMatchesRuntimePlatform()
    {
        When.A<ArgumentFormatOptions>("Platform preset matches runtime", () => ArgumentFormatOptions.Platform, (options) =>
        {
            return options;
        })
        .TheTest
        .ShouldPass(because =>
        {
            ArgumentFormatOptions result = because.ResultAs<ArgumentFormatOptions>();
            if (RuntimeSettings.IsWindows)
            {
                because.ItsTrue("Platform prefix is '/' on Windows", result.Prefix == "/");
                because.ItsTrue("Platform separator is ':' on Windows", result.ValueSeparator == ':');
            }
            else
            {
                because.ItsTrue("Platform prefix is '--' on non-Windows", result.Prefix == "--");
                because.ItsTrue("Platform separator is '=' on non-Windows", result.ValueSeparator == '=');
            }
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void ForStyleReturnCorrectOptions()
    {
        When.A<ArgumentFormatOptions>("ForStyle returns correct options for each style", (defaultOptions) =>
        {
            ArgumentFormatOptions posix = ArgumentFormatOptions.ForStyle(ArgumentStyle.Posix);
            ArgumentFormatOptions windows = ArgumentFormatOptions.ForStyle(ArgumentStyle.Windows);
            ArgumentFormatOptions platform = ArgumentFormatOptions.ForStyle(ArgumentStyle.Platform);
            return new object[] { posix, windows, platform };
        })
        .TheTest
        .ShouldPass(because =>
        {
            object[] results = (object[])because.Result;
            ArgumentFormatOptions posix = (ArgumentFormatOptions)results[0];
            ArgumentFormatOptions windows = (ArgumentFormatOptions)results[1];
            ArgumentFormatOptions platform = (ArgumentFormatOptions)results[2];

            because.ItsTrue("ForStyle(Posix) prefix is '--'", posix.Prefix == "--");
            because.ItsTrue("ForStyle(Posix) separator is '='", posix.ValueSeparator == '=');
            because.ItsTrue("ForStyle(Windows) prefix is '/'", windows.Prefix == "/");
            because.ItsTrue("ForStyle(Windows) separator is ':'", windows.ValueSeparator == ':');
            because.ItsTrue("ForStyle(Platform) prefix matches Platform preset", platform.Prefix == ArgumentFormatOptions.Platform.Prefix);
            because.ItsTrue("ForStyle(Platform) separator matches Platform preset", platform.ValueSeparator == ArgumentFormatOptions.Platform.ValueSeparator);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }
}

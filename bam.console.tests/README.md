# bam.console.tests

Unit tests for the bam.console project, validating argument parsing, console commands, input commands, and menu integration.

## Overview

bam.console.tests is an executable test project that uses the Bam Framework's own menu-driven test runner (not xUnit or NUnit). The entry point calls `BamConsoleContext.StaticMain(args)`, and tests are organized into `[UnitTestMenu]`-adorned classes containing `[UnitTest]`-attributed methods. Each test uses the `When.A<T>()` fluent API to set up a test case, execute actions, and make assertions via the `Because` object.

The test suite covers the core argument parsing pipeline (`ParsedArguments`, `DefaultArgumentParser`, `ArgumentFormatOptions`, `ArgumentInfo`), the `ConsoleCommandAttribute` name derivation logic, and the `InputCommands` discovery mechanism. Tests validate cross-platform argument formats, edge cases like duplicate arguments, values containing separators, null-value arguments, and error conditions for invalid formats.

The project also includes example menu and command classes (`ConsoleTestMenuClass`, `MenuExample`, `InputCommandExample`) that serve as test fixtures demonstrating how `[ConsoleCommand]`, `[MenuItem]`, and `[InputCommand]` attributes are discovered and used at runtime.

## Key Classes

| Class | Description |
|---|---|
| `ParsedArgumentsShould` | Tests for `ParsedArguments`: standard parsing, multiple args, null values, duplicate handling, separator-in-value, format variants (`/name:value`, `-name=value`), `Contains`, `EnsureArgument`, short prefix, mixed prefix. |
| `DefaultArgumentParserShould` | Tests for `DefaultArgumentParser`: parsing with default options, custom slash-colon options, empty args. |
| `ArgumentFormatOptionsShould` | Tests for `ArgumentFormatOptions`: platform defaults, valid/invalid prefixes and separators, parameterless constructor. |
| `ArgumentInfoShould` | Tests for `ArgumentInfo` and `ArgumentInfoHash`: constructor, `ToString`, `FromStringArray`, `FromArgs`, hash lookup, duplicate handling. |
| `ConsoleCommandAttributeShould` | Tests for `ConsoleCommandAttribute`: `OptionName` derivation, `OptionShortName` acronym generation, description storage, default null properties. |
| `InputCommandsShould` | Tests for `InputCommands`: discovery from type, correct name, correct method association. |
| `ConsoleTestMenuClass` | Example menu container with `[ConsoleCommand]`, `[MenuItem]`, and `[InputCommand]` methods used as test fixture. |
| `UnitTests` | Simple smoke test class using `[Test(TestType.Unit)]`. |

## Dependencies

**Project References:**
- `bam.base` -- Core framework primitives
- `bam.test` -- Test framework (`UnitTestMenuContainer`, `When`, `Because`, etc.)
- `bam.console` -- The project under test

**Target Framework:** net10.0
**Output Type:** Exe

## Usage Examples

### Run all unit tests
```bash
dotnet run --project bam.console.tests.csproj -- --ut
```

### Run interactively (menu-driven)
```bash
dotnet run --project bam.console.tests.csproj -- --i
```

### Test structure example
```csharp
[UnitTestMenu("ParsedArgumentsShould", "pas")]
public class ParsedArgumentsShould : UnitTestMenuContainer
{
    [UnitTest]
    public void ParseStandardArgs()
    {
        When.A<ParsedArguments>("parses --name=value",
            () => Parse(new[] { "--name=value" }),
            (parsed) => parsed)
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
}
```

## Known Gaps / Not Yet Implemented

None identified. The test suite covers the core parsing, attribute, and discovery functionality comprehensively.

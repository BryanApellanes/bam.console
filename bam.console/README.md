# bam.console

Console application framework providing argument parsing, command-line switches, and menu-driven interactive shell integration.

## Overview

bam.console is the primary entry point for building Bam Framework console applications. It provides `BamConsoleContext`, a singleton context that handles command-line argument parsing, switch execution, test runner integration, and interactive menu management. Applications call `BamConsoleContext.StaticMain(args)` to bootstrap an interactive menu-driven console application or to execute command-line switches non-interactively.

The argument parsing system supports multiple format styles (e.g., `--name=value`, `/name:value`, `-flag`) with configurable `ArgumentFormatOptions`. The `ConsoleCommandAttribute` marks methods as executable from the command line, automatically deriving switch names and short-name acronyms from the provided human-readable name.

The project also provides console I/O utilities including `Message` (colored output), `Prompt` (user input prompts, selection lists, password input), and process execution extension methods via the `Extensions` class. Console menu rendering is wired up through a `ServiceRegistry` configured by `ConsoleMenuOptions`, which assembles renderers, input readers, and menu item runners for the interactive shell experience.

## Key Classes

| Class | Description |
|---|---|
| `BamConsoleContext` | Singleton application context. Parses args, registers switches, manages menu lifecycle, runs test switches. Entry point via `StaticMain(args)`. |
| `ConsoleCommandAttribute` | Attribute adorning methods that can be invoked as command-line switches. Derives `OptionName` and `OptionShortName` from a human-readable name. |
| `ConsoleMethod` | Wraps a `MethodInfo` and its attribute for reflective invocation. Handles instance resolution, parameter provision, and async Task unwrapping. |
| `ParsedArguments` | Parses `string[] args` into a key-value dictionary. Supports configurable prefix/separator, duplicate handling, and value-in-separator scenarios. |
| `DefaultArgumentParser` | Default `IArgumentParser` implementation that delegates to `ParsedArguments`. |
| `ArgumentFormatOptions` | Configures prefix (`--`, `-`, `/`) and value separator (`=`, `:`) for argument parsing. Platform-aware defaults. |
| `ArgumentInfo` | Metadata about a valid argument: name, whether null values are allowed, description, and value example. |
| `ConsoleMenuOptions` | Configures and returns a `ServiceRegistry` wired for console menu rendering, input reading, and item execution. |
| `ConsoleMenuProvider` | `MenuProvider` specialized for `ConsoleCommandAttribute`-adorned methods. |
| `ConsoleMenuContainer` | Base class for menu container classes adorned with `[ConsoleMenu]`. Provides DI configuration hook. |
| `MenuContainer` | Abstract base for menu containers. Provides `RunAllItems`, `RunEveryItemFromAllMenus` input commands, and method invocation with DI. |
| `InputCommands` | Discovers `[InputCommand]`-attributed methods on a container type and indexes them by name. |
| `Message` | Static helper for colored console output (`Print`, `PrintLine`, `Log`, `PrintDiff`). |
| `Prompt` | Static helper for user input: `Show`, `Confirm`, `SelectFrom`, `ForPassword`, `ForNumber`. |
| `Extensions` | Extension methods for running external processes (`string.Run(...)`, `string.Start(...)`, `string.RunAndWait(...)`). |

## Dependencies

**Project References:**
- `bam.base` -- Core framework primitives, DI, logging, string extensions
- `bam.configuration` -- Configuration provider abstractions
- `bam.shell` -- Menu system abstractions (`IMenu`, `IMenuManager`, `MenuSpecs`, etc.)

**Package References:**
- `Newtonsoft.Json` 13.0.4

**Target Framework:** net10.0
**Output Type:** Library (NuGet package, v3.0.0)

## Usage Examples

### Bootstrap a console application
```csharp
using Bam.Console;

class Program
{
    static void Main(string[] args)
    {
        BamConsoleContext.StaticMain(args);
    }
}
```

### Define command-line switches
```csharp
using Bam.Console;

[ConsoleMenu("My Commands")]
public class MyCommands : ConsoleMenuContainer
{
    public MyCommands(ServiceRegistry sr) : base(sr) { }

    [ConsoleCommand("Do some stuff", "Performs the stuff operation")]
    public void DoSomeStuff()
    {
        Message.PrintLine("Doing stuff...", ConsoleColor.Green);
    }
}
```
Run with: `--doSomeStuff` or `-dss` (auto-generated acronym).

### Parse arguments manually
```csharp
var parser = new DefaultArgumentParser(new ArgumentFormatOptions("--", ':'));
IParsedArguments args = parser.ParseArguments(new[] { "--name:value", "--flag" });
string name = args["name"]; // "value"
bool hasFlag = args.Contains("flag"); // true
```

### Prompt the user
```csharp
string answer = Prompt.Show("Enter your name");
int choice = Prompt.SelectFrom(new[] { "Option A", "Option B", "Option C" });
bool confirmed = Prompt.Confirm("Are you sure? [y/n]", ConsoleColor.Yellow, false);
string password = Prompt.ForPassword("Enter password");
```

## Known Gaps / Not Yet Implemented

- `Extensions.GetExeAndArguments` is marked with `// TODO: obsolete this method` -- the method splits commands by space which does not handle quoted paths correctly.
- `ITestSwitchExecutor` is defined here but has no default implementation in this project; it must be provided by a downstream project (e.g., `bam.test`).

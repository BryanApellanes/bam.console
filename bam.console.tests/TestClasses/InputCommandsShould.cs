using Bam.Test;

namespace Bam.Console.Tests.TestClasses;

[UnitTestMenu("InputCommandsShould", "ics")]
public class InputCommandsShould : UnitTestMenuContainer
{
    [UnitTest]
    public void DiscoverInputCommandsFromType()
    {
        When.A<InputCommands>("discovers input commands from type",
            () => new InputCommands(typeof(ConsoleTestMenuClass)),
            (commands) =>
            {
                return commands;
            })
        .TheTest
        .ShouldPass(because =>
        {
            InputCommands result = because.ResultAs<InputCommands>();
            because.ItsTrue("has commands", result.Commands.Count > 0);
            because.ItsTrue("contains 'inputCommandTest'", result.Commands.ContainsKey("inputCommandTest"));
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void InputCommandHasCorrectName()
    {
        When.A<InputCommands>("input command has correct name",
            () => new InputCommands(typeof(ConsoleTestMenuClass)),
            (commands) =>
            {
                return commands.Commands["inputCommandTest"];
            })
        .TheTest
        .ShouldPass(because =>
        {
            InputCommand result = because.ResultAs<InputCommand>();
            because.ItsTrue("name is 'inputCommandTest'", result.Name == "inputCommandTest");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void InputCommandHasCorrectMethod()
    {
        When.A<InputCommands>("input command has correct method",
            () => new InputCommands(typeof(ConsoleTestMenuClass)),
            (commands) =>
            {
                return commands.Commands["inputCommandTest"];
            })
        .TheTest
        .ShouldPass(because =>
        {
            InputCommand result = because.ResultAs<InputCommand>();
            because.ItsTrue("OptionMethod name is 'InputCommandTest'", result.OptionMethod.Name == "InputCommandTest");
            because.ItsTrue("ContainerType is ConsoleTestMenuClass", result.ContainerType == typeof(ConsoleTestMenuClass));
        })
        .SoBeHappy()
        .UnlessItFailed();
    }
}

using Bam.Test;

namespace Bam.Console.Tests.TestClasses;

[UnitTestMenu("ConsoleCommandAttributeShould", "ccas")]
public class ConsoleCommandAttributeShould : UnitTestMenuContainer
{
    [UnitTest]
    public void DeriveOptionNameFromName()
    {
        When.A<ConsoleCommandAttribute>("derives OptionName from name",
            () => new ConsoleCommandAttribute("Do some stuff"),
            (attr) =>
            {
                return attr;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ConsoleCommandAttribute result = because.ResultAs<ConsoleCommandAttribute>();
            because.ItsTrue("OptionName is 'doSomeStuff'", result.OptionName == "doSomeStuff");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void DeriveOptionShortNameFromOptionName()
    {
        When.A<ConsoleCommandAttribute>("derives OptionShortName as acronym",
            () => new ConsoleCommandAttribute("Do some stuff"),
            (attr) =>
            {
                return attr;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ConsoleCommandAttribute result = because.ResultAs<ConsoleCommandAttribute>();
            // "doSomeStuff".CaseAcronym(true) -> first char always + uppercase chars: D + S + S = "DSS"
            because.ItsTrue("OptionShortName is 'DSS'", result.OptionShortName == "DSS");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void StoreDescription()
    {
        When.A<ConsoleCommandAttribute>("stores description",
            () => new ConsoleCommandAttribute("name", "desc"),
            (attr) =>
            {
                return attr;
            })
        .TheTest
        .ShouldPass(because =>
        {
            ConsoleCommandAttribute result = because.ResultAs<ConsoleCommandAttribute>();
            because.ItsTrue("Description is 'desc'", result.Description == "desc");
        })
        .SoBeHappy()
        .UnlessItFailed();
    }

    [UnitTest]
    public void HaveNullPropertiesWhenDefault()
    {
        When.A<ConsoleCommandAttribute>("has null properties when default", (attr) =>
        {
            return attr;
        })
        .TheTest
        .ShouldPass(because =>
        {
            ConsoleCommandAttribute result = because.ResultAs<ConsoleCommandAttribute>();
            because.ItsTrue("OptionName is null", result.OptionName == null);
            because.ItsTrue("OptionShortName is null", result.OptionShortName == null);
            because.ItsTrue("Description is null", result.Description == null);
        })
        .SoBeHappy()
        .UnlessItFailed();
    }
}

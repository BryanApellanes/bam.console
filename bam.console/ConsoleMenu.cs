using Bam.Shell;

namespace Bam.Console
{
    public class ConsoleMenu : MenuAttribute<ConsoleCommandAttribute>
    {
        public ConsoleMenu() 
        {
            this.Selector = "cm";
        }

        public ConsoleMenu(string name): base(name)
        {
            this.Selector = name.CamelCase().CaseAcronym();
        }
    }
}

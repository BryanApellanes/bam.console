using Bam.Logging;
using System.Reflection;

namespace Bam.Console
{
    public interface ITestSwitchExecutor
    {
        bool ExecuteTestSwitches(Assembly assembly, ILogger logger, IParsedArguments arguments);
    }
}

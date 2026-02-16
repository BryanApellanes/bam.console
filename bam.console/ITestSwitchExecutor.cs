using Bam.Logging;
using System.Reflection;

namespace Bam.Console
{
    /// <summary>
    /// Defines a contract for executing test-related command line switches (e.g., --ut, --it, --spec).
    /// </summary>
    public interface ITestSwitchExecutor
    {
        /// <summary>
        /// Executes test switches found in the parsed arguments against the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly containing test types to execute.</param>
        /// <param name="logger">The logger for reporting test execution status.</param>
        /// <param name="arguments">The parsed command line arguments containing test switches.</param>
        /// <returns>True if any test switches were executed; otherwise, false.</returns>
        bool ExecuteTestSwitches(Assembly assembly, ILogger logger, IParsedArguments arguments);
    }
}

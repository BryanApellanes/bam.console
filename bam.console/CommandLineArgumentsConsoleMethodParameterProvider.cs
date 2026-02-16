using System.Reflection;

namespace Bam.Console;

/// <summary>
/// Provides method parameters by matching method parameter names to command line arguments from the current <see cref="BamConsoleContext"/>.
/// </summary>
public class CommandLineArgumentsConsoleMethodParameterProvider : IConsoleMethodParameterProvider
{
    /// <summary>
    /// Gets method parameters by looking up each parameter name in the current console context's parsed arguments.
    /// </summary>
    /// <param name="methodInfo">The method whose parameters should be resolved from command line arguments.</param>
    /// <returns>An array of argument values matching the method's parameter names.</returns>
    public object[]? GetMethodParameters(MethodInfo methodInfo)
    {
        List<object> results = new List<object>();
        foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
        {
            if (parameterInfo.Name != null && BamConsoleContext.Current.Arguments.Contains(parameterInfo.Name))
            {
                results.Add(BamConsoleContext.Current.Arguments[parameterInfo.Name]);
            }
        }

        return results.ToArray();
    }
}
using System.Reflection;

namespace Bam.Console;

/// <summary>
/// Provides parameter values for console methods to be invoked.
/// </summary>
public interface IConsoleMethodParameterProvider
{
    /// <summary>
    /// Gets the parameter values for the specified method.
    /// </summary>
    /// <param name="methodInfo">The method whose parameters should be resolved.</param>
    /// <returns>An array of parameter values, or null if none could be resolved.</returns>
    object[]? GetMethodParameters(MethodInfo methodInfo);
}
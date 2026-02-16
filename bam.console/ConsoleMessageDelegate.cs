namespace Bam.Console
{
    /// <summary>
    /// Delegate for printing one or more console messages.
    /// </summary>
    /// <param name="messages">The console messages to print.</param>
    public delegate void ConsoleMessageDelegate(params ConsoleMessage[] messages);
}
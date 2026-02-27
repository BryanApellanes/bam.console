namespace Bam.Console
{
    /// <summary>
    /// Defines well-known argument formatting styles for command line parsing.
    /// </summary>
    public enum ArgumentStyle
    {
        /// <summary>
        /// GNU/POSIX convention: --name=value (long), -n (short).
        /// </summary>
        Posix,

        /// <summary>
        /// Microsoft convention: /name:value.
        /// </summary>
        Windows,

        /// <summary>
        /// Auto-detect: Windows style on Windows, Posix style on Unix/macOS.
        /// </summary>
        Platform
    }
}

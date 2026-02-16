/*
	Copyright © Bryan Apellanes 2015  
*/

namespace Bam.Console
{
    /// <summary>
    /// Represents a foreground and background color pair for console output.
    /// </summary>
    public class ConsoleColorCombo
    {
        /// <summary>
        /// Initializes a new instance with the specified foreground color and default background.
        /// </summary>
        /// <param name="foreground">The foreground console color.</param>
        public ConsoleColorCombo(ConsoleColor foreground)
        {
            ForegroundColor = foreground;
        }

        /// <summary>
        /// Initializes a new instance with the specified foreground and background colors.
        /// </summary>
        /// <param name="foreground">The foreground console color.</param>
        /// <param name="background">The background console color.</param>
        public ConsoleColorCombo(ConsoleColor foreground, ConsoleColor background) : this(foreground)
        {
            BackgroundColor = background;
        }

        /// <summary>
        /// Gets the foreground console color.
        /// </summary>
        public ConsoleColor ForegroundColor { get; private set; }
        /// <summary>
        /// Gets the background console color.
        /// </summary>
        public ConsoleColor BackgroundColor { get; private set; }
    }
}

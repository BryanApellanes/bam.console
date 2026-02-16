using Bam.Shell;
using System.Text;

namespace Bam.Console
{
    /// <summary>
    /// Console-based implementation of <see cref="IMenuInput"/> that captures keyboard input including navigation keys, selectors, and item numbers.
    /// </summary>
    public class ConsoleMenuInput : IMenuInput
    {
        /// <summary>
        /// The prefix character used to indicate a selector input (e.g., ":menuName").
        /// </summary>
        public const string SelectorPrefix = ":";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenuInput"/> class with a default exit code of 0.
        /// </summary>
        public ConsoleMenuInput()
        {
            ExitCode = 0;
        }

        /// <summary>
        /// Gets or sets the accumulated input text.
        /// </summary>
        public StringBuilder Input { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the exit key (Escape) was pressed.
        /// </summary>
        public bool Exit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exit code to use when exiting.
        /// </summary>
        public int ExitCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Enter key was pressed.
        /// </summary>
        public bool Enter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether a navigation key (arrow key) was pressed.
        /// </summary>
        public bool IsNavigation
        {
            get
            {
                return IsNavigationKey(ConsoleKey);
            }
        }

        /// <summary>
        /// Gets a value indicating whether a menu item navigation key (up/down arrow) was pressed.
        /// </summary>
        public bool IsMenuItemNavigation
        {
            get
            {
                return IsMenuItemNavigationKey(ConsoleKey);
            }
        }

        /// <summary>
        /// Gets a value indicating whether a menu navigation key (left/right arrow) was pressed.
        /// </summary>
        public bool IsMenuNavigation
        {
            get
            {
                return IsMenuNavigationKey(ConsoleKey);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current input value starts with the selector prefix.
        /// </summary>
        public bool IsSelector
        {
            get
            {
                if (!string.IsNullOrEmpty(Value))
                {
                    return Value.StartsWith(SelectorPrefix);
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the current input value as a string.
        /// </summary>
        public string Value
        {
            get
            {
                return Input.ToString();
            }
        }

        /// <summary>
        /// Gets the selector portion of the input (the value after the selector prefix), or empty if not a selector.
        /// </summary>
        public string Selector
        {
            get
            {
                if (IsSelector)
                {
                    return Value.Trim().TruncateFront(1);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the console key from the last key press.
        /// </summary>
        public ConsoleKey ConsoleKey
        {
            get
            {
                return ConsoleKeyInfo.Key;
            }
        }

        /// <summary>
        /// Gets or sets the full console key info from the last key press.
        /// </summary>
        public ConsoleKeyInfo ConsoleKeyInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the input value parsed as a menu item number, or -1 if the input is not a valid integer.
        /// </summary>
        public int ItemNumber
        {
            get
            {
                if (int.TryParse(Value, out int itemNumber))
                {
                    return itemNumber;
                }
                return -1;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the down arrow key was pressed to move to the next item.
        /// </summary>
        public bool NextItem
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the up arrow key was pressed to move to the previous item.
        /// </summary>
        public bool PreviousItem
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the right arrow key was pressed to move to the next menu.
        /// </summary>
        public bool NextMenu
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the left arrow key was pressed to move to the previous menu.
        /// </summary>
        public bool PreviousMenu
        {
            get;
            set;
        }

        /// <summary>
        /// Determines whether the specified key is a menu item navigation key (up or down arrow).
        /// </summary>
        /// <param name="key">The console key to check.</param>
        /// <returns>True if the key is an up or down arrow; otherwise, false.</returns>
        public virtual bool IsMenuItemNavigationKey(ConsoleKey key)
        {
            return key == ConsoleKey.UpArrow ||
                key == ConsoleKey.DownArrow;
        }

        /// <summary>
        /// Determines whether the specified key is a menu navigation key (left or right arrow).
        /// </summary>
        /// <param name="key">The console key to check.</param>
        /// <returns>True if the key is a left or right arrow; otherwise, false.</returns>
        public virtual bool IsMenuNavigationKey(ConsoleKey key)
        {
            return key == ConsoleKey.RightArrow ||
                key == ConsoleKey.LeftArrow;
        }

        /// <summary>
        /// Determines whether the specified key is any navigation key (up, down, left, or right arrow).
        /// </summary>
        /// <param name="key">The console key to check.</param>
        /// <returns>True if the key is an arrow key; otherwise, false.</returns>
        public static bool IsNavigationKey(ConsoleKey key)
        {
            return key == ConsoleKey.UpArrow ||
                    key == ConsoleKey.DownArrow ||
                    key == ConsoleKey.LeftArrow ||
                    key == ConsoleKey.RightArrow;
        }
    }
}

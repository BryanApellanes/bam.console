using Bam.Shell;
using System.Text;

namespace Bam.Console
{
    /// <summary>
    /// Reads menu input from the console, handling key presses including navigation, backspace, and text entry.
    /// </summary>
    public class ConsoleMenuInputReader : IMenuInputReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMenuInputReader"/> class with the default exit key.
        /// </summary>
        public ConsoleMenuInputReader()
        {
            //BamContext = bamContext;
            ExitKey = MenuManager.ExitKey;
            Input = new StringBuilder();
        }

        /// <summary>
        /// Occurs when input is being read from the console.
        /// </summary>
        public event EventHandler<MenuInputOutputLoopEventArgs> ReadingInput = null!;

        /*protected IBamContext BamContext
        {
            get;
            private set;
        }*/

        protected StringBuilder Input
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the console key that triggers an exit from the menu loop.
        /// </summary>
        public ConsoleKey ExitKey
        {
            get;
            private set;
        }

        /// <summary>
        /// Reads a single key press from the console and returns it as a <see cref="ConsoleMenuInput"/>, handling navigation keys, backspace, and text accumulation.
        /// </summary>
        /// <returns>An <see cref="IMenuInput"/> representing the captured key press and accumulated input.</returns>
        public IMenuInput ReadMenuInput()
        {
            ConsoleKeyInfo consoleKeyInfo = System.Console.ReadKey();
            if (!ConsoleMenuInput.IsNavigationKey(consoleKeyInfo.Key))
            {
                Input.Append(consoleKeyInfo.KeyChar);

                if (consoleKeyInfo.Key == ConsoleKey.Backspace)
                {
                    string currentInput = Input.ToString();
                    int newLength = currentInput.Length - 2;
                    if (newLength >= 0)
                    {
                        StringBuilder newInput = new StringBuilder(Input.ToString().Substring(0, newLength));
                        Input = newInput;
                    }
                }
                // clean up backspaces
                string input = Input.ToString().Trim();
                if (consoleKeyInfo.Key == ConsoleKey.Spacebar)
                {
                    input += " ";
                }
                Input.Clear();
                Input.Append(input);
            }

            ConsoleMenuInput menuInput = new ConsoleMenuInput
            {
                Input = Input,
                Exit = consoleKeyInfo.Key == ConsoleKey.Escape,
                ConsoleKeyInfo = consoleKeyInfo,
                NextItem = consoleKeyInfo.Key == ConsoleKey.DownArrow,
                PreviousItem = consoleKeyInfo.Key == ConsoleKey.UpArrow,
                NextMenu = consoleKeyInfo.Key == ConsoleKey.RightArrow,
                PreviousMenu = consoleKeyInfo.Key == ConsoleKey.LeftArrow,
                Enter = consoleKeyInfo.Key == ConsoleKey.Enter
            };
            ReadingInput?.Invoke(this, new MenuInputOutputLoopEventArgs
            {
                MenuInputReader = this,
                MenuInput = menuInput,
            });
            return menuInput;
        }
    }
}

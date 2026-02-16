using System.Text;

namespace Bam.Console;

/// <summary>
/// Provides static methods for prompting the user for input, selections, confirmations, and passwords via the console.
/// </summary>
public class Prompt
{
    /// <summary>
    /// Prompt for a selection from the specified list of values
    /// </summary>
    /// <param name="options"></param>
    /// <param name="prompt"></param>
    /// <param name="color"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T SelectFrom<T>(IEnumerable<T> options, string prompt = "Select an option from the list", ConsoleColor color = ConsoleColor.DarkCyan)
    {
        return SelectFrom(options, (t) => t.ToString(), prompt, color);
    }

    /// <summary>
    /// Prompt for a selection from the specified list of values, using the specified optionTextSelector to extract option text from the options.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="optionTextSelector"></param>
    /// <param name="prompt"></param>
    /// <param name="color"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T SelectFrom<T>(IEnumerable<T> options, Func<T, string> optionTextSelector, string prompt = "Select an option from the list", ConsoleColor color = ConsoleColor.DarkCyan)
    {
        T[] optionsArray = options.ToArray();
        string[] optionStrings = options.Select(optionTextSelector).ToArray();
        return optionsArray[SelectFrom(optionStrings, prompt, color)];
    }

    /// <summary>
    /// Prompts the user to select from a numbered list of string options.
    /// </summary>
    /// <param name="options">The options to display.</param>
    /// <param name="prompt">The prompt text to display.</param>
    /// <param name="color">The color for the prompt text.</param>
    /// <returns>The zero-based index of the selected option.</returns>
    public static int SelectFrom(string[] options, string prompt = "Select an option from the list", ConsoleColor color = ConsoleColor.Cyan)
    {
        StringBuilder list = new StringBuilder();
        for (int i = 0; i < options.Length; i++)
        {
            list.AppendFormat("{0}. {1}\r\n", (i + 1).ToString(), options[i]);
        }
        list.AppendLine();
        list.Append(prompt);
        int value = ForNumber(list.ToString(), color) - 1;
        Args.ThrowIf(value < 0, "Invalid selection");
        Args.ThrowIf(value > options.Length - 1, "Invalid selection");
        return value;
    }
    
    /// <summary>
    /// Prompts the user for [y]es or [n]o returning true for [y] and false for [n].
    /// </summary>
    /// <param name="message">Optional message for the user.</param>
    /// <param name="allowQuit">If true provides an additional [q]uit option which if selected
    /// will call Environment.Exit(0).</param>
    /// <returns>boolean</returns>
    public static bool Confirm(string message, ConsoleColor color, bool allowQuit)
    {
        Message.Print(message, color);
        if (allowQuit)
        {
            Message.PrintLine(" [q] ");
        }
        else
        {
            Message.PrintLine();
        }

        string answer = System.Console.ReadLine().Trim().ToLower();
        if (answer.IsAffirmative())
        {
            return true;
        }

        if (answer.IsNegative())
        {
            return false;
        }

        if (allowQuit && answer.IsExitRequest())
        {
            Environment.Exit(0);
        }

        return false;
    }

    /// <summary>
    /// Prompts the user for a number. Alias for <see cref="ForInt"/>.
    /// </summary>
    /// <param name="message">The prompt message.</param>
    /// <param name="color">The prompt text color.</param>
    /// <returns>The entered number, or -1 if parsing fails.</returns>
    public static int ForNumber(string message, ConsoleColor color = ConsoleColor.Cyan)
    {
        return ForInt(message, color);
    }

    /// <summary>
    /// Prompts the user for a long integer.
    /// </summary>
    /// <param name="message">The prompt message.</param>
    /// <param name="color">The prompt text color.</param>
    /// <returns>The entered long value, or -1 if parsing fails.</returns>
    public static long ForLong(string message, ConsoleColor color = ConsoleColor.Cyan)
    {
        string value = Show(message, color);
        long result = -1;
        long.TryParse(value, out result);
        return result;
    }

    /// <summary>
    /// Prompts the user for an integer.
    /// </summary>
    /// <param name="message">The prompt message.</param>
    /// <param name="color">The prompt text color.</param>
    /// <returns>The entered integer, or -1 if parsing fails.</returns>
    public static int ForInt(string message, ConsoleColor color = ConsoleColor.Cyan)
    {
        string value = Show(message, color);
        int result = -1;
        int.TryParse(value, out result);
        return result;
    }

        /// <summary>
    /// Prompts the user for input.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>string</returns>
    public static string Show(string message, ConsoleColor textColor = ConsoleColor.Cyan)
    {
        return Show(message, textColor, false);
    }

    /// <summary>
    /// Prompts the user for input with an optional quit option.
    /// </summary>
    /// <param name="message">The prompt message.</param>
    /// <param name="textColor">The text color.</param>
    /// <param name="allowQuit">Whether to allow quitting by entering "q".</param>
    /// <returns>The user's input.</returns>
    public static string Show(string message, ConsoleColor textColor, bool allowQuit)
    {
        return Show(message, ">>", textColor, allowQuit);
    }

    /// <summary>
    /// Prompts the user for input with the specified prompt text and color.
    /// </summary>
    /// <param name="message">The prompt message.</param>
    /// <param name="promptTxt">The prompt indicator text (e.g., ">>").</param>
    /// <param name="textColor">The text color.</param>
    /// <returns>The user's input.</returns>
    public static string Show(string message, string promptTxt, ConsoleColor textColor)
    {
        return Show(message, promptTxt, textColor, false);
    }

    /// <summary>
    /// Prompts the user for input with the specified prompt text, color, and quit option.
    /// </summary>
    /// <param name="message">The prompt message.</param>
    /// <param name="promptTxt">The prompt indicator text.</param>
    /// <param name="textColor">The text color.</param>
    /// <param name="allowQuit">Whether to allow quitting by entering "q".</param>
    /// <returns>The user's input.</returns>
    public static string Show(string message, string promptTxt, ConsoleColor textColor, bool allowQuit)
    {
        return Show(message, promptTxt, new ConsoleColorCombo(textColor), allowQuit);
    }

    public static string Show(string message, string promptTxt, ConsoleColor textColor, ConsoleColor backgroundColor)
    {
        return Show(message, promptTxt, new ConsoleColorCombo(textColor, backgroundColor), false);
    }

    public static string Show(string message, string promptTxt, ConsoleColor textColor, ConsoleColor backgroundColor, bool allowQuit)
    {
        return Show(message, promptTxt, new ConsoleColorCombo(textColor, backgroundColor), allowQuit);
    }

    public static string Show(string message, string promptTxt, ConsoleColorCombo colors, bool allowQuit)
    {
        return Provider(message, promptTxt, colors, allowQuit);
    }

    
    static Func<string, string, ConsoleColorCombo, bool, string> _provider;
    public static Func<string, string, ConsoleColorCombo, bool, string> Provider
    {
        get
        {
            return _provider ?? (_provider = (message, promptTxt, colors, allowQuit) =>
            {
                Message.Print($"{message} {promptTxt} ", colors);
                Thread.Sleep(200);
                string answer = System.Console.ReadLine();

                if (allowQuit && answer.ToLowerInvariant().Equals("q"))
                {
                    Environment.Exit(0);
                }

                return answer.Trim();
            });
        }
        set => _provider = value;
    }

    public static string ForPassword(string message = "Please enter password", ConsoleColor textColor = ConsoleColor.Cyan)
    {
        return ForPassword(message, ">>", new ConsoleColorCombo(textColor));
    }
    
    public static string ForPassword(string message, string promptText, ConsoleColorCombo colors)
    {
        Message.Print($"{message} {promptText} ", colors);
        string pass = string.Empty;
        ConsoleKey key;
        do
        {
            var keyInfo = System.Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && pass.Length > 0)
            {
                System.Console.Write("\b \b");
                pass = pass[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                System.Console.Write("*");
                pass += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);

        return pass;
    }
}
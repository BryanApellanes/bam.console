using Bam.Logging;

namespace Bam.Console
{
    /// <summary>
    /// Represents a console message with text, foreground and background colors, and logging capabilities.
    /// </summary>
    public class ConsoleMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMessage"/> class.
        /// </summary>
        public ConsoleMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMessage"/> class with the specified text.
        /// </summary>
        /// <param name="message">The message text.</param>
        public ConsoleMessage(string message)
        {
            Text = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMessage"/> class with colors and format arguments.
        /// </summary>
        /// <param name="message">The message format string.</param>
        /// <param name="colors">The foreground and background color combination.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public ConsoleMessage(string message, ConsoleColorCombo colors, params object[] messageSignatureArgs)
        {
            Colors = colors;
            SetText(message, messageSignatureArgs);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMessage"/> class with the specified text and colors.
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="textColor">The foreground color.</param>
        /// <param name="background">The background color (defaults to Black).</param>
        public ConsoleMessage(string message, ConsoleColor textColor, ConsoleColor background = ConsoleColor.Black) : this(message, new ConsoleColorCombo(textColor, background))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMessage"/> class with a format string, color, and arguments.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="textColor">The foreground color.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public ConsoleMessage(string messageSignature, ConsoleColor textColor, params object?[] messageSignatureArgs) : this(messageSignature, new ConsoleColorCombo(textColor, ConsoleColor.Black))
        {
            SetText(messageSignature, messageSignatureArgs);
        }

        private string? _text;

        /// <summary>
        /// Gets the formatted message text. If a message signature and arguments were provided, the text is lazily formatted on first access.
        /// </summary>
        public string Text
        {
            get => GetText();
            private set => _text = value;
        }

        /// <summary>
        /// Sets the message text from a format string and arguments, clearing any previously cached text.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public void SetText(string messageSignature, params object?[] messageSignatureArgs)
        {
            _text = null;
            MessageSignature = messageSignature;
            MessageArgs = messageSignatureArgs;
        }

        /// <summary>
        /// Gets or sets the foreground color for this message.
        /// </summary>
        public ConsoleColor ForegroundColor
        {
            get => Colors.ForegroundColor;
            set => Colors = new ConsoleColorCombo(value);
        }

        /// <summary>
        /// Gets or sets the background color for this message.
        /// </summary>
        public ConsoleColor BackgroundColor
        {
            get => Colors.BackgroundColor;
            set => Colors = new ConsoleColorCombo(ForegroundColor, value);
        }

        /// <summary>
        /// Gets or sets the foreground and background color combination for this message.
        /// </summary>
        public ConsoleColorCombo Colors { get; set; }

        protected string MessageSignature { get; set; }

        protected object?[] MessageArgs { get; set; }

        protected string GetText()
        {
            if (!string.IsNullOrEmpty(_text))
            {
                return _text;
            }

            try
            {
                if (string.IsNullOrEmpty(MessageSignature))
                {
                    return string.Empty;
                }
                _text = string.Format(MessageSignature, MessageArgs);
            }
            catch (Exception ex)
            {
                _text = ex.Message;
            }

            return _text;
        }

        /// <summary>
        /// Logs this message using the default logger.
        /// </summary>
        public void Log()
        {
            Log(Bam.Logging.Log.Default, this);
        }

        /// <summary>
        /// Logs this message using the specified logger.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        public void Log(ILogger logger)
        {
            Log(logger, this);
        }

        /// <summary>
        /// Logs a message to the default logger with the specified format arguments.
        /// </summary>
        /// <param name="message">The message format string.</param>
        /// <param name="messageArgs">Arguments to format into the message.</param>
        public static void Log(string message, params object[] messageArgs)
        {
            Log(Bam.Logging.Log.Default, message, messageArgs);
        }

        /// <summary>
        /// Logs a message to the specified logger with the default cyan color.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="message">The message format string.</param>
        /// <param name="messageArgs">Arguments to format into the message.</param>
        public static void Log(ILogger logger, string message, params object[] messageArgs)
        {
            Log(logger, message, ConsoleColor.Cyan, messageArgs);
        }

        /// <summary>
        /// Logs a message to the specified logger with the specified text color.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="textColor">The foreground color for the message.</param>
        /// <param name="messageArgs">Arguments to format into the message.</param>
        public static void Log(ILogger logger, string messageSignature, ConsoleColor textColor, params object[] messageArgs)
        {
            Log(logger, new ConsoleMessage(messageSignature, textColor, messageArgs));
        }

        /// <summary>
        /// Logs a message to the default logger with the specified color combination.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="colors">The foreground and background color combination.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void Log(string messageSignature, ConsoleColorCombo colors, params object[] messageSignatureArgs)
        {
            Log(Bam.Logging.Log.Default, messageSignature, colors, messageSignatureArgs);
        }

        /// <summary>
        /// Logs a message to the specified logger with the specified color combination.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="colors">The foreground and background color combination.</param>
        /// <param name="messageSignatureArgs">Arguments to format into the message.</param>
        public static void Log(ILogger logger, string messageSignature, ConsoleColorCombo colors, params object[] messageSignatureArgs)
        {
            Log(logger, new ConsoleMessage(messageSignature, colors, messageSignatureArgs));
        }

        /// <summary>
        /// Logs multiple console messages using the default logger.
        /// </summary>
        /// <param name="consoleMessages">The messages to log.</param>
        public static void Log(params ConsoleMessage[] consoleMessages)
        {
            foreach (ConsoleMessage consoleMessage in consoleMessages)
            {
                Log(Bam.Logging.Log.Default, consoleMessage);
            }
        }

        /// <summary>
        /// Logs multiple console messages using the specified logger.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="consoleMessages">The messages to log.</param>
        public static void Log(ILogger logger, params ConsoleMessage[] consoleMessages)
        {
            foreach (ConsoleMessage consoleMessage in consoleMessages)
            {
                Log(logger, consoleMessage);
            }
        }

        /// <summary>
        /// Logs a single console message using the default logger.
        /// </summary>
        /// <param name="consoleMessage">The message to log.</param>
        public static void Log(ConsoleMessage consoleMessage)
        {
            Log(Bam.Logging.Log.Default, consoleMessage);
        }

        static readonly HashSet<ConsoleColor> _errorBackgrounds = new HashSet<ConsoleColor>(new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.Magenta, ConsoleColor.DarkMagenta });
        static readonly HashSet<ConsoleColor> _warningBackgrounds = new HashSet<ConsoleColor>(new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.DarkYellow });
        /// <summary>
        /// Prints and logs a console message, routing to error, warning, or info based on the message's color.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="consoleMessage">The message to print and log.</param>
        public static void Log(ILogger logger, ConsoleMessage consoleMessage)
        {
            Print(consoleMessage);
            Task.Run(() =>
            {
                if (_errorBackgrounds.Contains(consoleMessage.Colors.BackgroundColor))
                {
                    logger.Error(consoleMessage.MessageSignature ?? consoleMessage.Text, consoleMessage.MessageArgs ?? new object[] { });
                    return;
                }

                if (_warningBackgrounds.Contains(consoleMessage.Colors.BackgroundColor))
                {
                    logger.Warning(consoleMessage.MessageSignature ?? consoleMessage.Text, consoleMessage.MessageArgs ?? new object[] { });
                    return;
                }

                switch (consoleMessage.Colors.ForegroundColor)
                {
                    case ConsoleColor.Red:
                    case ConsoleColor.DarkRed:
                    case ConsoleColor.Magenta:
                    case ConsoleColor.DarkMagenta:
                        logger.Error(consoleMessage.MessageSignature ?? consoleMessage.Text, consoleMessage.MessageArgs ?? new object[] { });
                        break;
                    case ConsoleColor.Yellow:
                    case ConsoleColor.DarkYellow:
                        logger.Warning(consoleMessage.MessageSignature ?? consoleMessage.Text, consoleMessage.MessageArgs ?? new object[] { });
                        break;
                    case ConsoleColor.Black:
                    case ConsoleColor.White:
                    case ConsoleColor.DarkBlue:
                    case ConsoleColor.DarkGreen:
                    case ConsoleColor.Gray:
                    case ConsoleColor.DarkGray:
                    case ConsoleColor.Blue:
                    case ConsoleColor.Green:
                    case ConsoleColor.Cyan:
                    case ConsoleColor.DarkCyan:
                    default:
                        logger.Info(consoleMessage.MessageSignature ?? consoleMessage.Text, consoleMessage.MessageArgs ?? new object[] { });
                        break;
                }
            });
        }

        /// <summary>
        /// Prints this message to the console.
        /// </summary>
        public void Print()
        {
            PrintMessage(this);
        }

        /// <summary>
        /// Prints a formatted message to the console with the default cyan color.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="messageArgs">Arguments to format into the message.</param>
        public static void Print(string messageSignature, params object?[] messageArgs)
        {
            if (messageArgs != null && messageArgs.Length == 0)
            {
                PrintMessage(messageSignature, ConsoleColor.Cyan);
            }
            else
            {
                Print(messageSignature, ConsoleColor.Cyan, messageArgs);
            }
        }

        /// <summary>
        /// Prints a formatted message to the console with the specified color combination.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="colors">The foreground and background color combination.</param>
        /// <param name="messageArgs">Arguments to format into the message.</param>
        public static void Print(string messageSignature, ConsoleColorCombo colors, params object[] messageArgs)
        {
            Print(new ConsoleMessage(messageSignature, colors, messageArgs));
        }

        /// <summary>
        /// Prints a formatted message to the console with the specified text color.
        /// </summary>
        /// <param name="messageSignature">The message format string.</param>
        /// <param name="textColor">The foreground color.</param>
        /// <param name="messageArgs">Arguments to format into the message.</param>
        public static void Print(string messageSignature, ConsoleColor textColor, params object?[] messageArgs)
        {
            if (messageArgs == null || messageArgs.Length == 0)
            {
                PrintMessage(new ConsoleMessage(messageSignature) { ForegroundColor = textColor });
            }
            else
            {
                Print(new ConsoleMessage(messageSignature, textColor, messageArgs));
            }
        }

        /// <summary>
        /// Prints a list of console messages.
        /// </summary>
        /// <param name="messages">The messages to print.</param>
        public static void Print(List<ConsoleMessage> messages)
        {
            Print(messages.ToArray());
        }

        private static IConsoleMessageHandler _consoleMessageHandler;
        private static readonly object _consoleMessageHandlerLock = new object();
        /// <summary>
        /// Gets or sets the handler used for printing and logging console messages. Defaults to <see cref="DefaultConsoleMessageHandler"/>.
        /// </summary>
        public static IConsoleMessageHandler ConsoleMessageHandler
        {
            get
            {
                return _consoleMessageHandlerLock.DoubleCheckLock(ref _consoleMessageHandler, () => new DefaultConsoleMessageHandler());
            }
            set => _consoleMessageHandler = value;
        }

        private static ConsoleMessageDelegate _printProvider;
        private static readonly object _printProviderLock = new object();
        /// <summary>
        /// Gets or sets the delegate used to print console messages. Defaults to the <see cref="ConsoleMessageHandler"/>'s Print method.
        /// </summary>
        public static ConsoleMessageDelegate PrintProvider
        {
            get
            {
                return _printProviderLock.DoubleCheckLock(ref _printProvider, () => ConsoleMessageHandler.Print);
            }
            set => _printProvider = value;
        }

        /// <summary>
        /// Prints the specified console messages using the current <see cref="PrintProvider"/>.
        /// </summary>
        /// <param name="messages">The messages to print.</param>
        public static void Print(params ConsoleMessage[] messages)
        {
            PrintProvider(messages);
        }

        private static void PrintMessage(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            DefaultConsoleMessageHandler.PrintMessage(message, foregroundColor, backgroundColor);
        }

        private static void PrintMessage(ConsoleMessage message)
        {
            DefaultConsoleMessageHandler.PrintMessage(message);
        }
    }
}

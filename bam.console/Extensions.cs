/*
	Copyright © Bryan Apellanes 2015  
*/

using System.Diagnostics;
using Bam.CommandLine;

namespace Bam.Console
{
    /// <summary>
    /// Extension methods for running external processes from command strings and file paths.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Runs the specified command string in a separate process with default settings.
        /// </summary>
        /// <param name="command">The command to run.</param>
        /// <returns>The process output.</returns>
        public static ProcessOutput Run(this string command)
        {
            return Run(command, (s) => { });
        }
        
        /// <summary>
        /// Run the specified command in a separate process capturing the output
        /// and error streams if any. This method will block if a timeout is specified,
        /// which it is by default, it will
        /// not block if timeout is null.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="onStandardOutput">The on standard output.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static ProcessOutput Run(this string command, Action<string> onStandardOutput, int? timeout = 600000)
        {
            return command.Run(null!, onStandardOutput, onStandardOutput, false, timeout);
        }

        /// <summary>
        /// Runs the specified command in a separate process with separate standard output and error output handlers.
        /// </summary>
        /// <param name="command">The command to run.</param>
        /// <param name="onStandardOut">Handler for standard output lines.</param>
        /// <param name="onErrorOut">Handler for error output lines.</param>
        /// <param name="timeout">Optional timeout in milliseconds; null means non-blocking.</param>
        /// <returns>The process output.</returns>
        public static ProcessOutput Run(this string command, Action<string> onStandardOut, Action<string> onErrorOut, int? timeout = null)
        {
            return command.Run(null!, onStandardOut, onErrorOut, false, timeout);
        }

        /// <summary>
        /// Runs the specified command in a separate process, optionally prompting for administrator privileges.
        /// </summary>
        /// <param name="command">The command to run.</param>
        /// <param name="onStandardOut">Handler for standard output lines.</param>
        /// <param name="onErrorOut">Handler for error output lines.</param>
        /// <param name="promptForAdmin">Whether to prompt for administrator privileges.</param>
        /// <returns>The process output.</returns>
        public static ProcessOutput Run(this string command, Action<string> onStandardOut, Action<string> onErrorOut, bool promptForAdmin = false)
        {
            return command.Run(null!, onStandardOut, onErrorOut, promptForAdmin);
        }
        /// <summary>
        /// Run the specified command in a separate process
        /// </summary>
        /// <param name="command">The command to run</param>
        /// <param name="onExit">EventHandler to execute when the process exits</param>
        /// <param name="timeout">The number of milliseconds to block before returning, specify 0 not to block</param>
        /// <returns></returns>
        public static ProcessOutput Run(this string command, EventHandler onExit, int? timeout)
        {
            return command.Run(onExit, null!, null!, false, timeout);
        }

        /// <summary>
        /// Run the specified command in a separate process capturing the output
        /// and error streams if any. This method will block if a timeout is specified, it will
        /// not block if timeout is null.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="onExit"></param>
        /// <param name="onStandardOut"></param>
        /// <param name="onErrorOut"></param>
        /// <param name="promptForAdmin"></param>
        /// <param name="timeout">The number of milliseconds to block before returning, specify 0 not to block</param>
        /// <returns></returns>
        public static ProcessOutput Run(this string command, EventHandler onExit, Action<string>? onStandardOut = null, Action<string>? onErrorOut = null, bool promptForAdmin = false, int? timeout = null)
        {
            GetExeAndArguments(command, out string exe, out string arguments);

            return exe.Run(arguments, onExit, onStandardOut, onErrorOut, promptForAdmin, timeout);
        }

        /// <summary>
        /// Execute the specified exe with the specified arguments.  This method will not block.
        /// </summary>
        /// <param name="exe"></param>
        /// <param name="arguments"></param>
        /// <param name="onExit"></param>
        public static ProcessOutput Run(this string exe, string arguments, EventHandler onExit)
        {
            return exe.Run(arguments, onExit, null!);
        }

        /// <summary>
        /// Runs the process described by the specified <see cref="ProcessStartInfo"/> and waits for it to complete.
        /// </summary>
        /// <param name="info">The process start info.</param>
        /// <param name="standardOut">Handler for standard output lines.</param>
        /// <param name="errorOut">Handler for error output lines.</param>
        /// <param name="timeOut">Timeout in milliseconds.</param>
        /// <returns>The process output.</returns>
        public static ProcessOutput RunAndWait(this ProcessStartInfo info, Action<string>? standardOut = null, Action<string>? errorOut = null, int timeOut = 60000)
        {
            ProcessOutputCollector output = new ProcessOutputCollector(standardOut, errorOut);
            return info.Run(output, timeOut);
        }

        /// <summary>
        /// Start the specified command in a separate process blocking until it exits.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="arguments"></param>
        /// <param name="standardOut"></param>
        /// <param name="errorOut"></param>
        /// <returns></returns>
        public static ProcessOutput Start(this string filePath, string? arguments = null, Action<string>? standardOut = null, Action<string>? errorOut = null)
        {
            ProcessStartInfo startInfo = filePath.ToStartInfo(arguments);
            return startInfo.Run(standardOut!, errorOut);
        }

        /// <summary>
        /// Creates a <see cref="ProcessStartInfo"/> for the specified file path with the current directory as the working directory.
        /// </summary>
        /// <param name="filePath">The path to the executable.</param>
        /// <param name="arguments">Optional command line arguments.</param>
        /// <returns>A configured <see cref="ProcessStartInfo"/>.</returns>
        public static ProcessStartInfo ToStartInfo(this string filePath, string? arguments = null)
        {
            return filePath.ToStartInfo(new DirectoryInfo("."), arguments);
        }

        /// <summary>
        /// Creates a <see cref="ProcessStartInfo"/> for the specified file path with the specified working directory.
        /// </summary>
        /// <param name="filePath">The path to the executable.</param>
        /// <param name="workingDirectory">The working directory for the process.</param>
        /// <param name="arguments">Optional command line arguments.</param>
        /// <returns>A configured <see cref="ProcessStartInfo"/>.</returns>
        public static ProcessStartInfo ToStartInfo(this string filePath, DirectoryInfo workingDirectory, string? arguments = null)
        {
            return new FileInfo(filePath).ToStartInfo(workingDirectory, arguments);
        }

        /// <summary>
        /// Creates a <see cref="ProcessStartInfo"/> for the specified file with redirected output, no shell execute, and no window.
        /// </summary>
        /// <param name="fileInfo">The file info for the executable.</param>
        /// <param name="workingDirectory">The working directory for the process.</param>
        /// <param name="arguments">Optional command line arguments.</param>
        /// <returns>A configured <see cref="ProcessStartInfo"/>.</returns>
        public static ProcessStartInfo ToStartInfo(this FileInfo fileInfo, DirectoryInfo workingDirectory, string? arguments = null)
        {
            return new ProcessStartInfo
            {
                FileName = fileInfo.FullName,
                Arguments = arguments,
                WorkingDirectory = workingDirectory.FullName,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                ErrorDialog = false,
                CreateNoWindow = true
            };
        }

        /// <summary>
        /// Creates a <see cref="ProcessStartInfo"/> that runs the specified file via cmd.exe /c.
        /// </summary>
        /// <param name="cmdFilePath">The path to the command file.</param>
        /// <param name="arguments">The arguments to pass to the command file.</param>
        /// <param name="workingDir">Optional working directory; defaults to the current directory.</param>
        /// <returns>A configured <see cref="ProcessStartInfo"/>.</returns>
        public static ProcessStartInfo ToCmdStartInfo(this string cmdFilePath, string arguments, DirectoryInfo? workingDir = null)
        {
            FileInfo cmdFile = new FileInfo(cmdFilePath);
            return cmdFile.ToCmdStartInfo(arguments, workingDir ?? new DirectoryInfo("."));
        }

        /// <summary>
        /// Creates a <see cref="ProcessStartInfo"/> that runs the specified command file via cmd.exe /c.
        /// </summary>
        /// <param name="cmdFileInfo">The file info for the command file.</param>
        /// <param name="arguments">The arguments to pass to the command file.</param>
        /// <param name="workingDirectory">The working directory for the process.</param>
        /// <returns>A configured <see cref="ProcessStartInfo"/>.</returns>
        public static ProcessStartInfo ToCmdStartInfo(this FileInfo cmdFileInfo, string arguments, DirectoryInfo workingDirectory)
        {
            return OSInfo.GetPath("cmd.exe").ToStartInfo(workingDirectory, $"/c \"{cmdFileInfo.FullName}\" {arguments}");
        }


        /// <summary>
        /// Runs the command and waits for it to complete.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="standardOut">The standard out.</param>
        /// <param name="errorOut">The error out.</param>
        /// <param name="timeOut">The time out.</param>
        /// <returns></returns>
        public static ProcessOutput RunAndWait(this string command, Action<string>? standardOut = null, Action<string>? errorOut = null, int timeOut = 60000)
        {
            GetExeAndArguments(command, out string exe, out string arguments);
            return exe.Run(arguments, (o, a) => { }, standardOut, errorOut, false, timeOut);
        }

        /// <summary>
        /// Runs the command and waits for it to complete.
        /// </summary>
        /// <param name="exe">The executable.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="onExit">The on exit.</param>
        /// <param name="timeOut">The time out.</param>
        public static ProcessOutput RunAndWait(this string exe, string arguments, EventHandler? onExit = null, int timeOut = 60000)
        {
            return exe.Run(arguments, onExit!, timeOut);
        }

        /// <summary>
        /// Run the specified exe with the specified arguments, executing the specified onExit
        /// when the process completes.  This method will block if a timeout is specified, it will
        /// not block if timeout is null.
        /// </summary>
        /// <param name="exe"></param>
        /// <param name="arguments"></param>
        /// <param name="onExit"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static ProcessOutput Run(this string exe, string arguments, EventHandler onExit, int? timeout)
        {
            return exe.Run(arguments, onExit, null!, null!, false, timeout);
        }

        /// <summary>
        /// Run the specified exe with the specified arguments, executing the specified onExit
        /// when the process completes.  This method will block if a timeout is specified, it will
        /// not block if timeout is null.
        /// </summary>
        /// <param name="exe"></param>
        /// <param name="arguments"></param>
        /// <param name="onExit"></param>
        /// <param name="onStandardOut"></param>
        /// <param name="onErrorOut"></param>
        /// <param name="promptForAdmin"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static ProcessOutput Run(this string exe, string arguments, EventHandler onExit, Action<string>? onStandardOut = null, Action<string>? onErrorOut = null, bool promptForAdmin = false, int? timeout = null)
        {
            ProcessStartInfo startInfo = ProcessExtensions.CreateStartInfo(promptForAdmin);
            startInfo.FileName = exe;
            startInfo.Arguments = arguments;
            ProcessOutputCollector receiver = new ProcessOutputCollector(onStandardOut, onErrorOut);
            return startInfo.Run(onExit, receiver, timeout);
        }



        // TODO: obsolete this method
        private static void GetExeAndArguments(string command, out string exe, out string arguments)
        {
            exe = command;
            arguments = string.Empty;
            string[] split = command.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length > 1)
            {
                exe = split[0];
                for (int i = 1; i < split.Length; i++)
                {
                    arguments += split[i];
                    if (i != split.Length - 1)
                        arguments += " ";
                }
            }
        }
    }
}

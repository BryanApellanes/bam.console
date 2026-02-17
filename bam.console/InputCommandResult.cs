using Bam.Shell;

namespace Bam.Console
{
    /// <summary>
    /// Represents the result of executing an input command, including success status, invocation result, and any exception.
    /// </summary>
    public class InputCommandResult : IInputCommandResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputCommandResult"/> class.
        /// </summary>
        public InputCommandResult()
        {
        }

        /// <summary>
        /// Gets or sets the name of the input command that was executed.
        /// </summary>
        public string InputName { get; set; } = null!;

        object? _invocationResult;
        /// <summary>
        /// Gets or sets the return value of the invoked command method. Setting this also checks for nested exceptions in <see cref="InputCommandResults"/> or <see cref="InputCommandResult"/> values.
        /// </summary>
        public object? InvocationResult
        {
            get
            {
                return _invocationResult;
            }
            set
            {
                _invocationResult = value;
                if(_invocationResult is InputCommandResults results)
                {
                    this.CheckResultsExceptions(results);
                }else if (_invocationResult is InputCommandResult result)
                {
                    this.CheckResultException(result);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the command executed without throwing an exception.
        /// </summary>
        public bool Success
        {
            get
            {
                return Exception == null;
            }
        }

        /// <summary>
        /// Gets the exception message, or null if no exception occurred.
        /// </summary>
        public string? Message
        {
            get
            {
                return Exception?.Message;
            }
        }

        /// <summary>
        /// Gets or sets the exception that occurred during command execution, or null if successful.
        /// </summary>
        public Exception Exception
        {
            get;
            set;
        } = null!;

        private void CheckResultsExceptions(InputCommandResults results)
        {
            List<Exception> exceptions = new List<Exception>();
            foreach(IInputCommandResult result in results.Results)
            {
                if(!result.Success)
                {
                    exceptions.Add(result.Exception);
                }
            }
            if (exceptions.Count > 0)
            {
                this.Exception = new AggregateException(exceptions);
            }
        }

        private void CheckResultException(InputCommandResult result)
        {
            if (!result.Success)
            {
                this.Exception = result.Exception;
            }
        }
    }
}

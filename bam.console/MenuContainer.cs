using Bam.Shell;
using Bam.Services;
using System.Reflection;
using Bam.DependencyInjection;

namespace Bam.Console
{
    public abstract class MenuContainer
    {
        public MenuContainer() { }

        /// <summary>
        /// Create a new MenuContainer using the specified dependency provider.
        /// </summary>
        /// <param name="dependencyProvider"></param>
        public MenuContainer(IDependencyProvider dependencyProvider)
        // Note that this uses service locator specifically to empower a test writer
        // to manipulate the state of the container.
        {
            this.SetDependencyProvider(dependencyProvider);
        }

        protected void SetDependencyProvider(IDependencyProvider dependencyProvider)
        {
            DependencyProvider = dependencyProvider;
            MethodArgumentProvider = new DependencyProviderMethodArgumentProvider(dependencyProvider);
        }

        /// <summary>
        /// Gets or set the DependencyProvider
        /// </summary>
        protected IDependencyProvider? DependencyProvider
        {
            get;
            set;
        }

        protected IMethodArgumentProvider? MethodArgumentProvider
        {
            get;
            private set;
        }

        protected IExceptionReporter? ExceptionReporter => DependencyProvider?.Get<IExceptionReporter>();

        protected ISuccessReporter? SuccessReporter => DependencyProvider?.Get<ISuccessReporter>();

        /// <summary>
        /// Get a configured instance of the specified generic type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            if (DependencyProvider == null)
            {
                throw new InvalidOperationException($"{nameof(DependencyProvider)} not set");
            }

            return DependencyProvider.Get<T>();
        }

        [InputCommand("all", "run all listed commands")]
        public InputCommandResults RunAllItems(IMenuManager menuManager)
        {
            if (DependencyProvider == null)
            {
                throw new InvalidOperationException($"{nameof(DependencyProvider)} not set");
            }

            if (menuManager == null)
            {
                throw new InvalidOperationException($"{nameof(menuManager)} not specified.");
            }

            if (menuManager.CurrentMenu == null)
            {
                throw new InvalidOperationException($"Current menu is not set.");
            }

            InputCommandResults results = new InputCommandResults();
            foreach (IMenuItem item in menuManager.CurrentMenu.Items)
            {
                if (item != null && item.MethodInfo != null)
                {
                    object? instance = null;
                    if (!item.MethodInfo.IsStatic && item.MethodInfo.DeclaringType != null)
                    {
                        instance = DependencyProvider.Get(item.MethodInfo.DeclaringType);
                    }
                    if (instance == null && item.Instance != null)
                    {
                        instance = item.Instance;
                    }

                    results.AddResult(TryInvoke(item.DisplayName, item.MethodInfo, instance));
                }
            }

            List<Task> tasks = new List<Task>();
            foreach (IInputCommandResult commandResult in results.Results)
            {
                if (commandResult.InvocationResult is Task task)
                {
                    tasks.Add(task);
                }
            }

            if (tasks.Count > 0)
            {
                Task.WaitAll(tasks.ToArray());
            }
            
            return results;
        }

        private InputCommandResult TryInvoke(string itemDisplayName, MethodInfo method, object? instance)
        {
            try
            {
                if (MethodArgumentProvider == null)
                {
                    throw new InvalidOperationException($"{nameof(MethodArgumentProvider)} not set");
                }

                if (SuccessReporter == null)
                {
                    throw new InvalidOperationException($"{nameof(SuccessReporter)} not set");
                }

                object? result = method.Invoke(instance, MethodArgumentProvider.GetMethodArguments(method));
                if (result is Task task)
                {
                    task.Wait();
                }
                SuccessReporter.ReportSuccess($"{itemDisplayName} completed successfully.");
                return new InputCommandResult()
                {
                    InputName = itemDisplayName,
                    InvocationResult = result
                };
            }
            catch (Exception ex)
            {
                Exception e = ex.GetInnerException();
                ExceptionReporter?.ReportException($"{itemDisplayName} failed.", e);
                return new InputCommandResult()
                { 
                    InputName = itemDisplayName, 
                    Exception = e 
                };
            }
        }
    }
}

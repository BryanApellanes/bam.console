/*
	Copyright © Bryan Apellanes 2015  
*/

using System.Reflection;
using System.Diagnostics;

namespace Bam.Console
{
    /// <summary>
    /// Represents a method that can be invoked from the console, typically associated with a <see cref="ConsoleCommandAttribute"/>.
    /// </summary>
    [Serializable]
    public class ConsoleMethod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMethod"/> class.
        /// </summary>
        public ConsoleMethod()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMethod"/> class with the specified method.
        /// </summary>
        /// <param name="method">The method info to wrap.</param>
        public ConsoleMethod(MethodInfo method)
            : this(method, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMethod"/> class with the specified method and attribute.
        /// </summary>
        /// <param name="method">The method info to wrap.</param>
        /// <param name="actionInfo">The attribute adorning the method.</param>
        public ConsoleMethod(MethodInfo method, Attribute actionInfo)
        {
            Method = method;
            Attribute = actionInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleMethod"/> class with the specified method, attribute, provider instance, and switch value.
        /// </summary>
        /// <param name="method">The method info to wrap.</param>
        /// <param name="actionInfo">The attribute adorning the method.</param>
        /// <param name="provider">The instance on which to invoke the method, or null for static methods.</param>
        /// <param name="switchValue">The command line switch value associated with this method.</param>
        public ConsoleMethod(MethodInfo method, Attribute actionInfo, object provider, string switchValue = "")
            : this(method, actionInfo)
        {
            Provider = provider;
            SwitchValue = switchValue;
        }

        /// <summary>
        /// Used to help build usage examples for /? 
        /// </summary>
        public string SwitchValue { get; set; }
        /// <summary>
        /// Gets or sets the method info for the method to invoke.
        /// </summary>
        public MethodInfo Method { get; set; }

        /// <summary>
        /// Gets or sets the parameters to pass when invoking the method.
        /// </summary>
        public object[]? Parameters { get; set; }

        object? _provider;

        /// <summary>
        /// Gets or sets the object instance on which to invoke the method. If not set and the method is not static, an instance is constructed or resolved from the service registry.
        /// </summary>
        public object? Provider
        {
            get
            {
                if (_provider == null && !Method.IsStatic && Method.DeclaringType != null)
                {
                    _provider = Method.DeclaringType.Construct();
                    if(_provider == null)
                    {
                        _provider = BamConsoleContext.Current.ServiceRegistry.Get(Method.DeclaringType);
                    }
                }
                return _provider;
            }
            set => _provider = value;
        }

        private IConsoleMethodParameterProvider? _parameterProvider;

        /// <summary>
        /// Gets the parameter provider used to resolve method parameters. Lazily resolved from the service registry if the method has parameters.
        /// </summary>
        public IConsoleMethodParameterProvider? ParameterProvider
        {
            get
            {
                if (_parameterProvider == null && Method.GetParameters().Any())
                {
                    _parameterProvider = BamConsoleContext.Current.ServiceRegistry.Get<IConsoleMethodParameterProvider>();
                }

                return _parameterProvider;
            }
        }
        
        /// <summary>
        /// Gets a human-readable description of the method, derived from the attribute's information or the method name.
        /// </summary>
        public string Information
        {
            get
            {
                string info = Method.Name.PascalSplit(" ");
                if (Attribute != null)
                {
                    if (Attribute is IInfoAttribute consoleAction && !string.IsNullOrEmpty(consoleAction.Information))
                    {
                        info = consoleAction.Information;
                    }
                }

                return info;
            }
        }

        /// <summary>
        /// Returns a string representation including the fully qualified method name and information.
        /// </summary>
        /// <returns>A string representation of this console method.</returns>
        public override string ToString()
        {
            return $"{Method.DeclaringType?.Namespace}.{Method.DeclaringType?.Name}.{Method.Name}: ({Information})";
        }

        /// <summary>
        /// Gets or sets the attribute that decorates the method.
        /// </summary>
        public Attribute Attribute { get; set; }

        /// <summary>
        /// Attempts to invoke the method, catching exceptions and passing them to the specified handler.
        /// </summary>
        /// <param name="exceptionHandler">An optional handler for exceptions thrown during invocation.</param>
        /// <returns>True if the method invoked successfully; otherwise, false.</returns>
        public bool TryInvoke(Action<Exception> exceptionHandler = null)
        {
            try
            {
                Invoke();
                return true;
            }
            catch (Exception ex)
            {
                Action<Exception> handler = exceptionHandler ?? ((e) => { });
                handler(ex.GetInnerException());
                return false;
            }
        }

        /// <summary>
        /// Invokes the method, resolving the provider and parameters if not already set.
        /// </summary>
        /// <returns>The return value of the invoked method, or null.</returns>
        [DebuggerStepThrough]
        public object? Invoke()
        {
            object? result = null;
            try
            {
                if (!Method.IsStatic && Provider == null)
                {
                    if (Method.DeclaringType != null) Provider = Method.DeclaringType.Construct();
                }

                if (Parameters == null || Parameters.Length == 0)
                {
                    if (Method.GetParameters().Any())
                    {
                        Parameters = ParameterProvider?.GetMethodParameters(Method);
                    }
                }
                result = Method.Invoke(Provider, Parameters);
                if (result is Task task)
                {
                    task.GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Exception inner = ex.GetInnerException();
#if DEBUG
                System.Console.Error.WriteLine(inner.ToString());
#endif
                throw inner;
            }

            return result;
        }

        /// <summary>
        /// Discovers all methods on the specified type that are decorated with the specified attribute type and returns them as <see cref="ConsoleMethod"/> instances.
        /// </summary>
        /// <param name="typeToAnalyze">The type to scan for decorated methods.</param>
        /// <param name="attributeAddorningMethod">The attribute type to look for on methods.</param>
        /// <returns>A list of <see cref="ConsoleMethod"/> instances.</returns>
        public static List<ConsoleMethod> FromType(Type typeToAnalyze, Type attributeAddorningMethod)
        {
            return FromType<ConsoleMethod>(typeToAnalyze, attributeAddorningMethod);
        }

        /// <summary>
        /// Discovers all methods on the specified type that are decorated with the specified attribute type and returns them as instances of <typeparamref name="TConsoleMethod"/>.
        /// </summary>
        /// <typeparam name="TConsoleMethod">The type of console method to create.</typeparam>
        /// <param name="typeToAnalyze">The type to scan for decorated methods.</param>
        /// <param name="attributeAddorningMethod">The attribute type to look for on methods.</param>
        /// <returns>A list of <typeparamref name="TConsoleMethod"/> instances.</returns>
        public static List<TConsoleMethod> FromType<TConsoleMethod>(Type typeToAnalyze, Type attributeAddorningMethod) where TConsoleMethod : ConsoleMethod, new()
        {
            List<TConsoleMethod> actions = new List<TConsoleMethod>();
            MethodInfo[] methods = typeToAnalyze.GetMethods();
            foreach (MethodInfo method in methods)
            {
                if (method.HasCustomAttributeOfType(attributeAddorningMethod, false, out object action))
                {
                    actions.Add(new TConsoleMethod { Method = method, Attribute = (Attribute)action });
                }
            }

            return actions;
        }

        /// <summary>
        /// Discovers all methods decorated with the specified attribute type across all types in the assembly.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute type to look for on methods.</typeparam>
        /// <param name="assembly">The assembly to scan.</param>
        /// <returns>A list of <see cref="ConsoleMethod"/> instances.</returns>
        public static List<ConsoleMethod> FromAssembly<TAttribute>(Assembly assembly)
        {
            return FromAssembly(assembly, typeof(TAttribute));
        }

        /// <summary>
        /// Discovers all methods decorated with the specified attribute type across all types in the assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        /// <param name="attrType">The attribute type to look for on methods.</param>
        /// <returns>A list of <see cref="ConsoleMethod"/> instances.</returns>
        public static List<ConsoleMethod> FromAssembly(Assembly assembly, Type attrType)
        {
            return FromAssembly<ConsoleMethod>(assembly, attrType);
        }

        /// <summary>
        /// Discovers all methods decorated with the specified attribute type across all types in the assembly, returning them as instances of <typeparamref name="TConsoleMethod"/>.
        /// </summary>
        /// <typeparam name="TConsoleMethod">The type of console method to create.</typeparam>
        /// <param name="assembly">The assembly to scan.</param>
        /// <param name="attrType">The attribute type to look for on methods.</param>
        /// <returns>A list of <typeparamref name="TConsoleMethod"/> instances.</returns>
        public static List<TConsoleMethod> FromAssembly<TConsoleMethod>(Assembly assembly, Type attrType) where TConsoleMethod : ConsoleMethod, new()
        {
            List<TConsoleMethod> actions = new List<TConsoleMethod>();
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                actions.AddRange(FromType<TConsoleMethod>(type, attrType));
            }
            return actions;
        }
    }
}

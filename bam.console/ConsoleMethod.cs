/*
	Copyright © Bryan Apellanes 2015  
*/

using System.Reflection;
using System.Diagnostics;

namespace Bam.Console
{
    [Serializable]
    public class ConsoleMethod
    {
        public ConsoleMethod()
        {
        }
        public ConsoleMethod(MethodInfo method)
            : this(method, null)
        {
        }

        public ConsoleMethod(MethodInfo method, Attribute actionInfo)
        {
            Method = method;
            Attribute = actionInfo;
        }

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
        public MethodInfo Method { get; set; }
        public object[]? Parameters { get; set; }

        object? _provider;
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

        public override string ToString()
        {
            return $"{Method.DeclaringType?.Namespace}.{Method.DeclaringType?.Name}.{Method.Name}: ({Information})";
        }

        public Attribute Attribute { get; set; }

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
                throw ex.GetInnerException();
            }

            return result;
        }

        public static List<ConsoleMethod> FromType(Type typeToAnalyze, Type attributeAddorningMethod)
        {
            return FromType<ConsoleMethod>(typeToAnalyze, attributeAddorningMethod);
        }

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

        public static List<ConsoleMethod> FromAssembly<TAttribute>(Assembly assembly)
        {
            return FromAssembly(assembly, typeof(TAttribute));
        }

        public static List<ConsoleMethod> FromAssembly(Assembly assembly, Type attrType)
        {
            return FromAssembly<ConsoleMethod>(assembly, attrType);
        }

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

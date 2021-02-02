using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace APIDaemonClient.CommandObject
{
    public class CLICommandObject
    {
        public string MethodName { get; }
        public string MethodNameDisplay { get; }
        public string MethodDescription { get; } //implement this soon.
        public string[] MethodParameters { get; }
        public List<Type> MethodParameterTypes { get; } = new List<Type>();

        public CLICommandObject([NotNull] string methodName, [NotNull] string methodNameDisplay, string[] methodParameters , List<Type> methodParameterTypes)
        {
            MethodName = methodName;
            MethodNameDisplay = methodNameDisplay;
            MethodParameters = methodParameters;
            MethodParameterTypes = methodParameterTypes;
        }
    }
}

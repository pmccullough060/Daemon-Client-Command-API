using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace APIDaemonClient.CommandObject
{
    public class CLICommandObject
    {
        public string MethodName { get; }
        public string MethodNameDisplay { get; }
        public string MethodDescription { get; } 
        public string[] MethodParameters { get; }
        public List<Type> MethodParameterTypes { get; } = new List<Type>();

        public CLICommandObject([NotNull] string methodName, [NotNull] string methodNameDisplay, [NotNull] string methodDescription, string[] methodParameters , List<Type> methodParameterTypes)
        {
            MethodName = methodName;
            MethodNameDisplay = methodNameDisplay;
            MethodDescription = methodDescription;
            MethodParameters = methodParameters;
            MethodParameterTypes = methodParameterTypes;
        }
    }
}

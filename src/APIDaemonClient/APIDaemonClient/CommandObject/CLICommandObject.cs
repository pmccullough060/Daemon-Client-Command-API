using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace APIDaemonClient.CommandObject
{
    public class CLICommandObject
    {
        public string MethodName { get; }
        public string MethodNameDisplay { get; }
        public string MethodDescription { get; } 
        public string[] MethodParameters { get; private set; }
        public Type[] MethodParameterTypes { get; private set; }

        public MethodInfo MethodInfo { get; private set; }


        public CLICommandObject(string methodName, string methodNameDisplay, string methodDescription, string methodParameters, ParameterInfo[] methodParameterTypes, MethodInfo methodInfo)
        {
            MethodName = methodName;
            MethodNameDisplay = methodNameDisplay;
            MethodDescription = methodDescription;
            MethodInfo = methodInfo;

            setMethodParameters(methodParameters);
            setParameterTypeArray(methodParameterTypes);
        }

        private void setMethodParameters(string methodParameters)
        {
            if (string.IsNullOrEmpty(methodParameters))
            {
                MethodParameters = new string[0]; //avoiding null reference exceptions.
            }
            else
            { 
                MethodParameters = methodParameters.Split(" ");
            }
        }

        private void setParameterTypeArray(ParameterInfo[] parameterInfo)
        {
            MethodParameterTypes = new Type[parameterInfo.Length];

            for(int i = 0; i < parameterInfo.Length; i++)
            {
                MethodParameterTypes[i] = parameterInfo[i].ParameterType;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace APIDaemonClient
{
    public class CLICommandObject
    {
        public string MethodName { get; }
        public string MethodNameDisplay { get; }
        public string MethodDescription { get; } 
        public string[] MethodParameters { get; private set; }
        public Type[] MethodParameterTypes { get; private set; }
        public MethodInfo MethodInfo { get; private set; }

        public CLICommandObject(CLIMethodAttribute attribute, MethodInfo methodInfo)
        {
            MethodName = methodInfo.Name;
            MethodNameDisplay = attribute.CommandName;
            MethodDescription = attribute.CommandDescription;
            MethodInfo = methodInfo;

            setMethodParameters(attribute.CommandArguments);
            setParameterTypeArray(methodInfo.GetParameters());
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

        public override string ToString()
        {
            string methodParameters = "";

            for (int i = 0; i < MethodParameters.Length; i++)
                methodParameters += " " + MethodParameters[i];

            return " =>Command: " + MethodName + " =>Description: " + MethodDescription + " =>Input Parameters:" + methodParameters;
        }
    }
}

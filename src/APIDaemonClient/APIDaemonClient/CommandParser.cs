using System;
using System.Linq;
using System.Collections.Generic;
using APIDaemonClient.Attributes;
using APIDaemonClient.CommandObject;
using APIDaemonClient.ExtendedConsole;
using System.Reflection;

namespace APIDaemonClient
{
    /// <summary>
    /// The command parser is used to run methods based on arguments supplied by the user.
    /// All placed here to make things cleaner.
    /// A system is implemented where methods are decorated to allow them to be called from the command line.
    /// by creating a custom attribute.
    /// </summary>
    public class CommandParser : ICommandParser
    {
        private Dictionary<CLICommandObject, dynamic> CLIMethods = new Dictionary<CLICommandObject, dynamic>();

        /// <summary>
        /// Building the Dictionary<CLICommandObject, dynamic> CLIMethods.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public void ConfigureForCLI<T>(dynamic instance)
        {
            var methodInfoList = typeof(T).GetMethods().Where(x => x.GetCustomAttributes(typeof(CLIMethodAttribute), false).Any()).ToList(); //getting the custom attributes

            foreach(var item in methodInfoList)
            {
                var methodParameterTypes = item.GetParameters().Length > 0 ? BuildParameterTypeArray(item.GetParameters()).ToList() : null; //Returns a List<Type> for all of the method parameters.

                var attribute = (CLIMethodAttribute)item.GetCustomAttributes(typeof(CLIMethodAttribute), false).First(); //only consider first custom attribute

                var cliCommandObject = new CLICommandObject(item.Name, attribute.CommandName, attribute.CommandDescription, attribute.CommandArguments?.Split(" "), methodParameterTypes);

                CLIMethods.Add(cliCommandObject, instance);

                //could we just store the methods here instead as we know the type?? instead of the CLICommandObject
            }
        }

        /// <summary>
        /// returns an array of the different parameter types for each method.
        /// </summary>
        /// <param name="parameterInfo"></param>
        /// <returns></returns>
        private Type[] BuildParameterTypeArray(ParameterInfo[] parameterInfo)
        {
            Type[] parameterTypes = new Type[parameterInfo.Length]; //creating an array to hold the parameter types.

            for (int i = 0; i < parameterInfo.Length; i++)
                parameterTypes[i] = parameterInfo[i].ParameterType;

            return parameterTypes;
        }

        /// <summary>
        /// The input to the console from the user.
        /// </summary>
        /// <param name="command"></param>
        public void CallMethod(string command)
        {
            var commandList = StringListFromCommand(command);

            var argumentList = commandList.Skip(1).ToList();

            var cliKVP = CLIMethods.Where(x => x.Key.MethodName == commandList[0]).FirstOrDefault();

            var methodArguments = MethodParameterObjectArray(cliKVP.Key, argumentList);

            //we use the cliKVP to retrieve the correct method....

            getMethod(cliKVP);

            cliKVP.Value.GetType().GetMethod(cliKVP.Key.MethodName).Invoke(cliKVP.Value, methodArguments);
        }

        //here is where we make sure we are getting the right method
        private void getMethod(KeyValuePair<CLICommandObject, dynamic> cliKVP)
        {
            var method = typeof(UpdateSetting).GetMethods().Single(
                m =>
                    m.Name == cliKVP.Key.MethodName &&
                    m.GetParameters().Length == 1 && //this will be the length of the parameters specified.
                    m.GetParameters().Select((s) => s.ParameterType).ToArray().SequenceEqual(cliKVP.Key.MethodParameterTypes.ToArray()) //really hacky atm, just pinning logic down atm
                );
        }

        private List<string> StringListFromCommand(string command) => command.Split(":", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

        private object[] MethodParameterObjectArray(CLICommandObject cliCommandObject, List<string> arguments)
        {
            object[] methodArguments = new object[arguments.Count];

            for(int i = 0; i < arguments.Count; i++)
            {
                var currentType = cliCommandObject.MethodParameterTypes[i]; //using the method type to parse the input....

                if(currentType == typeof(string))
                {
                    string argumentValue = arguments[i];
                    methodArguments[i] = argumentValue;
                }
                else if(currentType == typeof(int))
                {
                    int argumentValue = int.Parse(arguments[i]);
                    methodArguments[i] = argumentValue;
                }
                else if(currentType == typeof(double))
                {
                    double argumentValue = double.Parse(arguments[i]);
                    methodArguments[i] = argumentValue;
                }
                else if(currentType == typeof(long))
                {
                    double argumentValue = long.Parse(arguments[i]);
                    methodArguments[i] = argumentValue;
                }
                else if (currentType == typeof(decimal))
                {
                    decimal argumentValue = decimal.Parse(arguments[i]);
                    methodArguments[i] = argumentValue;
                }
                else
                {
                    throw new ArgumentException("Value parsing exception");
                }
            }

            return methodArguments;
        }

        public void Parse() 
        {
            ConsoleEx.WriteLineDarkBlue("Next Command:");

            var command = Console.ReadLine();

            try
            {
                CallMethod(command);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Command: {command} is invalid");
                Parse();
            }
        }

        public void DisplayAllRegisteredCommands()
        {
            foreach(var item in CLIMethods)
            {
                string methodParameters = "";

                if(item.Key.MethodParameters != null)
                    foreach (var inputParameter in item.Key.MethodParameters)
                        methodParameters += inputParameter;

                ConsoleEx.WriteLineDarkBlue($"Command: {item.Key.MethodName}, Description: {item.Key.MethodDescription}, Input Parameters: {methodParameters}");
            }
        }
    }
}

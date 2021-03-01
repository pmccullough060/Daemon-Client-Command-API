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

            foreach (var method in methodInfoList)
            {
                var attribute = (CLIMethodAttribute)method.GetCustomAttributes(typeof(CLIMethodAttribute), false).First(); //Retrieves the first CLIMethodAttribute decorating the method.

                var cliCommandObject = new CLICommandObject(method.Name, 
                                                            attribute.CommandName, 
                                                            attribute.CommandDescription, 
                                                            attribute.CommandArguments, 
                                                            method.GetParameters(),
                                                            method); 

                //could we just add the method info the the cliCommandObject instead? Then when we get the correct CLICommandObject we simply invoke the method from inside this object?

                CLIMethods.Add(cliCommandObject, instance);

                // only here for testing.
                getMethod<T>(CLIMethods.Last());
            }
        }

        private void getMethod<T>(KeyValuePair<CLICommandObject, dynamic> cliKVP) //here is where we make sure we are getting the right method when using both static and dynamic polymorphism. ..this may be redundant.
        {
            var method = typeof(T).GetMethods().Single(
                m =>
                    m.Name == cliKVP.Key.MethodName &&
                    m.GetParameters().Length == cliKVP.Key.MethodParameters.Length &&
                    m.GetParameters().Select((s) => s.ParameterType).ToArray().SequenceEqual(cliKVP.Key.MethodParameterTypes)
                );
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

            //gets all the matching names from the CLICommand objects, then attempts to parse each one to the specified type and if it succeeds invokes that method.

            cliKVP.Value.GetType().GetMethod(cliKVP.Key.MethodName).Invoke(cliKVP.Value, methodArguments);
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
            //move most of this functionality into the CLICommandObject by adding a ToString() method.

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

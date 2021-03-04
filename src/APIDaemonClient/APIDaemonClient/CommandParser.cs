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

                var cliCommandObject = new CLICommandObject(attribute, method);

                CLIMethods.Add(cliCommandObject, instance);
            }
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

        /// <summary>
        /// The input to the console from the user.
        /// </summary>
        /// <param name="command"></param>
        public void CallMethod(string command)
        {
            var commandList = StringListFromCommand(command);

            var argumentList = commandList.Skip(1).ToList();

            var listCliKVP = CLIMethods.Where(x => x.Key.MethodName == commandList[0] & x.Key.MethodParameterTypes.Length == argumentList.Count).ToList();

            InvokeMethod(argumentList, listCliKVP);
        }

        private void InvokeMethod(List<string> argumentList, List<KeyValuePair<CLICommandObject,dynamic>> listCliKVP)
        {
            foreach(var kvp in listCliKVP)
            {
                var typesArray = kvp.Key.MethodParameterTypes;

                var methodArguments = new object[typesArray.Length];

                for (int i = 0; i < typesArray.Length; i++)
                {
                    try
                    {
                        var methodArgument = Convert.ChangeType(argumentList[i], typesArray[i]);

                        methodArguments[i] = methodArgument;
                    }
                    catch
                    {
                        break;
                    }
                }

                var invokingClass = kvp.Value;

                var methodToInvoke = kvp.Key.MethodInfo;

                methodToInvoke.Invoke(invokingClass, methodArguments);

                return;
            }
        }

        private List<string> StringListFromCommand(string command)
        {
            return command.Split(":", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
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

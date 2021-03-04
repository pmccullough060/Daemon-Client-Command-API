using System;
using System.Linq;
using System.Collections.Generic;

namespace APIDaemonClient
{
    /// <summary>
    /// The class receives a string input from the user and can use that to invoke a method with arguments.
    /// Simply decorate a public methdod with the CLIMethodAttribute to allow is to be called from the CLI.
    /// Supports static polymorphism.
    /// </summary>
    public class CommandParser : ICommandParser
    {
        private Dictionary<CLICommandObject, dynamic> CLIMethods = new Dictionary<CLICommandObject, dynamic>();

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
            catch
            {
                Console.WriteLine($"Command: {command} is invalid");
                Parse();
            }
        }

        public void CallMethod(string command)
        {
            var commandList = StringListFromCommand(command);

            var argumentList = commandList.Skip(1).ToList();

            var listCliKVP = CLIMethods.Where(x => x.Key.MethodName == commandList[0] & x.Key.MethodParameterTypes.Length == argumentList.Count).ToList();

            InvokeMethod(argumentList, listCliKVP);
        }

        private void InvokeMethod(List<string> argumentList, List<KeyValuePair<CLICommandObject, dynamic>> listCliKVP)
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
                    catch { break; }
                }

                kvp.Key.MethodInfo.Invoke(kvp.Value, methodArguments);

                return;
            }
        }

        private List<string> StringListFromCommand(string command) => command.Split(":", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

        public void DisplayAllRegisteredCommands()
        {
            foreach(var item in CLIMethods)
                ConsoleEx.WriteLineDarkBlue(item.Key.ToString());
        }
    }
}

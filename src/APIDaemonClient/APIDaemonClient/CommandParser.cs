using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using APIDaemonClient.Attributes;
using Microsoft.Extensions.Configuration;
using APIDaemonClient.CommandObject;
using APIDaemonClient.ExtendedConsole;
using APIDaemonClient.Views;
using System.Linq.Expressions;
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
        private Dictionary<CLICommandObject, dynamic> CLIMethods;

        private readonly IDaemonHttpClient _daemon;
        private readonly ILogger<CommandParser> _logger;
        private readonly IUpdateSetting _updateSetting;

        public CommandParser(IDaemonHttpClient daemon, ILogger<CommandParser> logger, IUpdateSetting updateSetting)
        {
            _daemon = daemon;
            _logger = logger;
            _updateSetting = updateSetting;

            CLIMethods = new Dictionary<CLICommandObject, dynamic>();

            CLICommandConfigContainer(); //method being ran more than once.
        }

        private void CLICommandConfigContainer() //this is where you configure the all the interfaces and DI instances required.
        {
            ConfigureForCLI<IUpdateSetting>(_updateSetting);
            ConfigureForCLI<IDaemonHttpClient>(_daemon);
        }

        private void ConfigureForCLI<T>(dynamic instance)
        {
            var methodInfoList = typeof(T).GetMethods().Where(x => x.GetCustomAttributes(typeof(CLIMethodAttribute), false).Any()).ToList(); //getting the custom attributes

            foreach(var item in methodInfoList)
            {
                var methodParameterTypes = item.GetParameters().Length > 0 ? BuildParameterTypeArray(item.GetParameters()).ToList() : null;

                var attribute = (CLIMethodAttribute)item.GetCustomAttributes(typeof(CLIMethodAttribute), false).First(); //only consider first custom attribute

                var cliCommandObject = new CLICommandObject()
                {
                    MethodNameDisplay = item.Name,
                    MethodName = attribute.CommandName,
                    MethodParameters = attribute.CommandArguments?.Split(" "),
                    MethodParameterTypes = methodParameterTypes
                };

                CLIMethods.Add(cliCommandObject, instance);
            }
        }

        private Type[] BuildParameterTypeArray(ParameterInfo[] parameterInfo)
        {
            Type[] parameterTypes = new Type[parameterInfo.Length]; //creating an array to hold the parameter types.

            for (int i = 0; i < parameterInfo.Length; i++)
                parameterTypes[i] = parameterInfo[i].ParameterType;

            return parameterTypes;
        }

        public void CallMethod(string command)
        {
            //maybe this method should return a tuple or have "out" parameters.... tidy up later..
            var commandList = StringArrayFromCommand(command); //splitting up method name and arguments.
            var argumentList = commandList.Skip(1).ToList(); //argument array is the command array without first index

            var cliKVP = CLIMethods.Where(x => x.Key.MethodName == commandList[0]).FirstOrDefault(); //finding the appropriate CLI Method.

            var methodArguments = MethodParameterObjectArray(cliKVP.Key, argumentList);

            cliKVP.Value.GetType().GetMethod(cliKVP.Key.MethodNameDisplay).Invoke(cliKVP.Value, methodArguments);
        }

        private List<string> StringArrayFromCommand(string command) => command.Split("-", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

        private object[] MethodParameterObjectArray(CLICommandObject cliCommandObject, List<string> arguments)
        {
            object[] methodArguments = new object[arguments.Count];

            for(int i = 0; i < arguments.Count; i++)
            {
                var currentType = cliCommandObject.MethodParameterTypes[i];

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
                ConsoleEx.WriteLineDarkBlue($"Command: {item.Key.MethodName}, Some Description about what it does");
            }
        }
    }
}

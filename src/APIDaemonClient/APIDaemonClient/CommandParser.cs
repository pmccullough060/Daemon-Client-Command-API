﻿using Microsoft.Extensions.Logging;
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

            CLICommandConfigContainer();
        }

        private void CLICommandConfigContainer() //this is where you configure the all the interfaces and DI instances required.
        {
            ConfigureForCLI<IUpdateSetting>(_updateSetting);
            ConfigureForCLI<IDaemonHttpClient>(_daemon);
        }

        private void ConfigureForCLI<T>(dynamic instance)
        {
            var methodInfoList = typeof(T).GetMethods().Where(x => x.GetCustomAttributes(typeof(CLIMethodAttribute), true).Any()).ToList(); //getting the custom attributes

            foreach(var item in methodInfoList)
            {
                var attribute = (CLIMethodAttribute)item.GetCustomAttributes(typeof(CLIMethodAttribute), true).First(); //only consider first custom attribute

                var cliCommandObject = new CLICommandObject()
                {
                    MethodNameDisplay = item.Name,
                    MethodName = attribute.CommandName,
                    MethodParameters = attribute.CommandArguments?.Split(" ")
                };

                CLIMethods.Add(cliCommandObject, instance);
            }
        }

        public void CallMethod(string command)
        {
            //add string command parser//

            var cliKVP = CLIMethods.Where(x => x.Key.MethodName == command).FirstOrDefault();

            cliKVP.Value.GetType().GetMethod(cliKVP.Key.MethodNameDisplay).Invoke(cliKVP.Value, null);
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

        public void DisplayAllRegisteredCommands()
        {
            foreach(var item in CLIMethods)
            {
                ConsoleEx.WriteLineDarkBlue($"Command: {item.Key.MethodName}, Some Description about what it does");
            }
        }
    }
}

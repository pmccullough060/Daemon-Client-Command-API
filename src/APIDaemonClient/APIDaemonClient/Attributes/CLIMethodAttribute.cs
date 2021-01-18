using System;
using System.Collections.Generic;
using System.Text;

namespace APIDaemonClient.Attributes
{
    class CLIMethodAttribute : Attribute
    {
        public string CommandName { get; set; }
        public string CommandArguments { get; set; }

        public CLIMethodAttribute(string commandName)
        {
            CommandName = commandName;
        }

        public CLIMethodAttribute(string commandName, string commandArguments)
        {
            CommandName = commandName;
            CommandArguments = commandArguments;
        }
    }
}

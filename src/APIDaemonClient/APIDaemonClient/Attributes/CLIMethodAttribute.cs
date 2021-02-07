using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace APIDaemonClient.Attributes
{
    public class CLIMethodAttribute : Attribute
    {
        public string CommandName { get; }
        public string CommandArguments { get; }
        public string CommandDescription { get; }

        public CLIMethodAttribute( [NotNull] string commandName, [NotNull] string commandDescription)
        {
            CommandName = commandName;
            CommandDescription = commandDescription;
        }

        public CLIMethodAttribute([NotNull] string commandName, [NotNull] string commandDescription, [NotNull] string commandArguments)
        {
            CommandName = commandName;
            CommandDescription = commandDescription;
            CommandArguments = commandArguments;
        }
    }
}

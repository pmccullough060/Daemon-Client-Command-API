using System;
using System.Collections.Generic;
using System.Text;

namespace APIDaemonClient.Attributes
{
    class CLIMethodAttribute : Attribute
    {
        public string CLIArgument { get; set; }

        public CLIMethodAttribute(string cliArgument)
        {
            CLIArgument = cliArgument;
        }
    }
}

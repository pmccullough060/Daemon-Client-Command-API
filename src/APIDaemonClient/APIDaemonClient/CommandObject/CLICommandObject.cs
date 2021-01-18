using System;
using System.Collections.Generic;
using System.Text;

namespace APIDaemonClient.CommandObject
{
    public class CLICommandObject
    {
        public string MethodName { get; set; }
        public string MethodNameDisplay { get; set; }
        public string MethodDescription { get; set; }
        public object[] MethodParameters { get; set; }
    }
}

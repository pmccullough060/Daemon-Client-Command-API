using System;
using System.Collections.Generic;
using System.Text;
using APIDaemonClient.Attributes;

namespace APIDaemonClient_Tests.CommandParserTests
{
    public interface ITestInterface
    {
        [CLIMethod("TestMethod_1")]
        bool TestMethod_1(int number);



    }
}

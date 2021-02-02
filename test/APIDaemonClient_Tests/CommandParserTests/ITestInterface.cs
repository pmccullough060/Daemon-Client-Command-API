using System;
using System.Collections.Generic;
using System.Text;
using APIDaemonClient.Attributes;

namespace APIDaemonClient_Tests.CommandParserTests
{
    public interface ITestInterface
    {
        [CLIMethod("TestMethod_1")]
        bool TestMethod_1();

        [CLIMethod("TestMethod_2", "-InputParameterInt")]
        bool TestMethod_2(int intInput);

        [CLIMethod("TestMethod_3", "-InputParameterInt -InputParameterString")]
        bool TestMethod_2(int intInput, string stringInput);
    }
}

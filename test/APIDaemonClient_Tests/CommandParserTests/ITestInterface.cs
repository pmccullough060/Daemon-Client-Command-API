using System;
using System.Collections.Generic;
using System.Text;
using APIDaemonClient.Attributes;

namespace APIDaemonClient_Tests.CommandParserTests
{
    public interface ITestInterface
    {
        [CLIMethod("TestMethod_1", "A test method with no input parameters")]
        bool TestMethod_1();

        [CLIMethod("TestMethod_2","A test method with a single input parameter", "-InputParameterInt")]
        bool TestMethod_2(int intInput);

        [CLIMethod("TestMethod_3","A test method with two input parameters" ,"-InputParameterInt -InputParameterString")]
        bool TestMethod_2(int intInput, string stringInput);
    }
}

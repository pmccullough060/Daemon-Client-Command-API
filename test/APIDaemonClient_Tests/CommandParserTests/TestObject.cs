using System;
using System.Collections.Generic;
using System.Text;

namespace APIDaemonClient_Tests.CommandParserTests
{
    class TestObject : ITestInterface
    {
        public void TestMethod() => Console.WriteLine("Test");

        public void TestMethod(int intInput) => Console.WriteLine(intInput);

        public void TestMethod(string stringInput) => Console.WriteLine(stringInput);

        public void TestMethod(int firstInput, int secondInput) => Console.WriteLine(firstInput + secondInput);
    }
}

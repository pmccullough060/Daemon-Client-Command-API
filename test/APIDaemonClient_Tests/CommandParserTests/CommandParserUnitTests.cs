using APIDaemonClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace APIDaemonClient_Tests.CommandParserTests
{
    public class CommandParserUnitTests
    {
        public CommandParser commandParser { get; set; }

        public CommandParserUnitTests()
        {
            var testObject = new TestObject();

            commandParser = new CommandParser();

            commandParser.ConfigureForCLI<ITestInterface>(testObject);
        }

        [Fact]
        public void ParseMethod_MethodHasNoInputParameters_CorrectConsoleOutput()
        {
            //Arrange
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                //Act
                commandParser.CallMethod("TestMethod"); //simulating a typed user command

                string expected = string.Format("Test{0}", Environment.NewLine);

                //Assert
                Assert.Equal(expected, sw.ToString());
            }
        }

        [Fact]
        public void ParseMethod_MethodHasSingleIntInput_CorrectConsoleOutput()
        {
            //Arrange
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                //Act
                commandParser.CallMethod("TestMethod :-1"); //simulating a typed user command

                string expected = string.Format("-1{0}", Environment.NewLine);

                //Assert
                Assert.Equal(expected, sw.ToString());
            }
        }

        [Fact]
        public void ParseMethodStaticPolymorphism_MethodAddsTwoIntInputs_CorrectConsoleOutput()
        {
            //Arrange
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                //Act
                commandParser.CallMethod("TestMethod :-1 :20"); //simulating a typed user command

                string expected = string.Format("19{0}", Environment.NewLine);

                //Assert
                Assert.Equal(expected, sw.ToString());
            }
        }

    }
}

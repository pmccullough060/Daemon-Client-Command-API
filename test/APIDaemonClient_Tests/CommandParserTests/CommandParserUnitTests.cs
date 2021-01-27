using APIDaemonClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace APIDaemonClient_Tests.CommandParserTests
{
    public class CommandParserUnitTests
    {
        public static ILoggerFactory mockLoggerFactory { get; set; }
        public static ILogger<CommandParser> mockLogger { get; set; }

        public CommandParserUnitTests()
        {
            mockLoggerFactory = new NullLoggerFactory();
            mockLogger = mockLoggerFactory.CreateLogger<CommandParser>();
        }

        [Fact]
        public void SomeTest()
        {
            var commandParser = new CommandParser(mockLogger);

            var mockObject = new Mock<ITestInterface>();

            commandParser.ConfigureForCLI<ITestInterface>(mockObject.Object);
        }
    }
}

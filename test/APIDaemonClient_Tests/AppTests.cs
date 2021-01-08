using APIDaemonClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace APIDaemonClient_Tests
{
    /// <summary>
    /// The logic is simple so we're checking to make sure we can create a directory
    /// </summary>

    public class AppTests
    {
        public static ILoggerFactory mockLoggerFactory { get; set; }
        public static ILogger<App> mockLogger { get; set; }


        public AppTests()
        {
            mockLoggerFactory = new NullLoggerFactory();
            mockLogger = mockLoggerFactory.CreateLogger<App>();
        }

        [Fact]
        public async void IncorrectAuttDetails_UnableToGenerateAuthenticationResult_returnsFalse()
        {
            var mockConfig = new Mock<IConfiguration>();

            var mockClientAppBuilderWrapper = new Mock<IClientAppBuilderWrapper>();
            mockClientAppBuilderWrapper.Setup(x => x.GetAuthenticationResult()).Returns(Task.FromResult<AuthenticationResult>(null));

            var app = new App(mockConfig.Object, mockLogger, mockClientAppBuilderWrapper.Object);

            var result = await app.RunAsync();

            Assert.False(result);
        }

        [Fact]
        public async void CorrectAuttDetails_UnableToGenerateAuthenticationResult_returnsTrue()
        {
            var mockConfig = new Mock<IConfiguration>();

            var mockClientAppBuilderWrapper = new Mock<IClientAppBuilderWrapper>();
            mockClientAppBuilderWrapper.Setup(x => x.GetAuthenticationResult()).Returns(Task.FromResult<AuthenticationResult>(null));

            var app = new App(mockConfig.Object, mockLogger, mockClientAppBuilderWrapper.Object);

            var result = await app.RunAsync();

            Assert.False(result);
        }
    }
}

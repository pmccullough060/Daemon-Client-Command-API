using System;
using Xunit;

namespace APIDaemonClient_Tests
{
    /// <summary>
    /// The logic is simple so we're checking to make sure we can create a directory
    /// </summary>

    public class UserSecretsTest
    {

        public UserSecretsTest()
        {
            //Arrange - we need to create some file in a directory then delete it, if the file doesn't exist we need to create it in the APPDATA folder
        }

        [Fact]
        public void UserSecretsFileExists()
        {

        }
    }
}

using APIDaemonClient.Attributes;

namespace APIDaemonClient_Tests.CommandParserTests
{
    public interface ITestInterface
    {
        [CLIMethod("Test Method", "A test method with no input parameters")]
        void TestMethod();

        [CLIMethod("Test Method", "A test method with a single input parameter", ":InputParameterInt")]
        void TestMethod(int intInput);

        [CLIMethod("Test Method", "A test method with a single string parameter", ":InputParameterString")]
        void TestMethod(string intInput);

        [CLIMethod("Test Method", "A test method with two input parameters" ,":InputParameterInt :InputParameterInt")]
        void TestMethod(int firstInput, int secondInput);
    }
}

namespace APIDaemonClient
{
    public interface ICommandParser
    {
        void CallMethod(string command);
        void Parse();
        void DisplayAllRegisteredCommands();
    }
}
using System;

namespace APIDaemonClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var authConfig = new AuthConfig();

            Console.WriteLine($"Authority: {authConfig.Authority}");

            var returnbool = FileIO.CheckForExistingSettingsFile();

            Console.ReadLine();


        }
    }
}

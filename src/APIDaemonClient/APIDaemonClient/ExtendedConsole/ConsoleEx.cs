using System;
using System.Collections.Generic;
using System.Text;

namespace APIDaemonClient
{
    public static class ConsoleEx 
    {
        public static void WriteLineRed(string message) 
        {
            OutputToConsoleWithColour(message, ConsoleColor.Red);
        }

        public static void WriteLineGreen(string message)
        {
            OutputToConsoleWithColour(message, ConsoleColor.Green);
        }

        public static void WriteLineDarkGray(string message)
        {
            OutputToConsoleWithColour(message, ConsoleColor.DarkGray);
        }

        public static void WriteLineDarkBlue(string message)
        {
            OutputToConsoleWithColour(message, ConsoleColor.DarkBlue);
        }

        private static void OutputToConsoleWithColour(string message, ConsoleColor foregroundColour)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColour;
            Console.WriteLine(message);
            Console.ForegroundColor = oldColor;
        }
    }
}

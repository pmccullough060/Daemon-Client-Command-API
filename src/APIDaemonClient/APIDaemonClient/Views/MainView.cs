using APIDaemonClient.ExtendedConsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIDaemonClient.Views
{
    public class MainView : IMainView
    {
        private readonly ICommandParser _commandParser;

        public string[] asciiArt = new string[]
        {
         @"                                                                                                                                                          ",
         @"    $$$$$$\  $$$$$$$\ $$$$$$\       $$$$$$$\                                                               $$$$$$\  $$\ $$\                      $$\      ",
         @"   $$  __$$\ $$  __$$\\_$$  _|      $$  __$$\                                                             $$  __$$\ $$ |\__|                     $$ |     ",
         @"   $$ /  $$ |$$ |  $$ | $$ |        $$ |  $$ | $$$$$$\   $$$$$$\  $$$$$$\$$$$\   $$$$$$\  $$$$$$$\        $$ /  \__|$$ |$$\  $$$$$$\  $$$$$$$\ $$$$$$\    ",
         @"   $$$$$$$$ |$$$$$$$  | $$ |        $$ |  $$ | \____$$\ $$  __$$\ $$  _$$  _$$\ $$  __$$\ $$  __$$\       $$ |      $$ |$$ |$$  __$$\ $$  __$$\\_$$  _|   ",
         @"   $$  __$$ |$$  ____/  $$ |        $$ |  $$ | $$$$$$$ |$$$$$$$$ |$$ / $$ / $$ |$$ /  $$ |$$ |  $$ |      $$ |      $$ |$$ |$$$$$$$$ |$$ |  $$ | $$ |     ",
         @"   $$ |  $$ |$$ |       $$ |        $$ |  $$ |$$  __$$ |$$   ____|$$ | $$ | $$ |$$ |  $$ |$$ |  $$ |      $$ |  $$\ $$ |$$ |$$   ____|$$ |  $$ | $$ |$$\  ",
         @"   $$ |  $$ |$$ |     $$$$$$\       $$$$$$$  |\$$$$$$$ |\$$$$$$$\ $$ | $$ | $$ |\$$$$$$  |$$ |  $$ |      \$$$$$$  |$$ |$$ |\$$$$$$$\ $$ |  $$ | \$$$$  | ",
         @"   \__|  \__|\__|     \______|      \_______/  \_______| \_______|\__| \__| \__| \______/ \__|  \__|       \______/ \__|\__| \_______|\__|  \__|  \____/  "
        };

        public MainView(ICommandParser commandParser)
        {
            _commandParser = commandParser;

            ConfigureConsole();
        }

        private void ConfigureConsole() //configure console properties here such as size and background colour.
        {
            Console.WindowWidth = 155;
        }

        public void StartMenu()
        {
            foreach (var line in asciiArt)
                ConsoleEx.WriteLineDarkBlue(line);

            ConsoleEx.WriteLineDarkGray("\n\n A simple console application to interact with an API using Azure Active Directories AuthN and AuthZ");

            ConsoleEx.WriteLineDarkGray("\n\n Please Select from one of the following commands:");

            _commandParser.DisplayAllRegisteredCommands();
        }
        
        public void AllCommands()
        {

        }

    }
}

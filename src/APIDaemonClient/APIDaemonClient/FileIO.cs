using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace APIDaemonClient
{
    public static class FileIO
    {
        private const string folderName = "APIDaemonClient";

        private const string logName = "programLog.txt";
        public static string LogFilePath
        {
            get
            {
                return Path.Combine(AppLocalDirectory, folderName, logName); 
            }
        }

        private const string fileName = "Settings.json";
        public static string FileName
        {
            get
            {
                return fileName;
            }
        }

        public static string AppLocalDirectory
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            }
        }

        public static string FolderPath
        {
            get
            {
                return Path.Combine(AppLocalDirectory, folderName);
            }
        }

        public static string FilePath
        {
            get
            {
                return Path.Combine(AppLocalDirectory, folderName, FileName);
            }
        }

        public static void CheckForExistingSettingsFile ()
        {
            if (Directory.Exists(FolderPath) == false)
            {
                Directory.CreateDirectory(FolderPath);
            }

            if(File.Exists(FilePath) == false)
            {
                var settings = new Settings()
                {
                    LogOutputDirectory = LogFilePath,
                    SettingsDirectory = FilePath
                };

                string settingsString = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(FilePath, settingsString); //creating a new settings file in this directory if it doesn't exist.
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace APIDaemonClient
{
    public static class FileIO
    {
        private const string folderName = "APIDaemonClient";
        private const string fileName = "Settings.json";

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
                return Path.Combine(AppLocalDirectory, folderName, fileName);
            }
        }

        public static bool CheckForExistingSettingsFile ()
        {
            var fileAlreadyExists = true;

            if (Directory.Exists(FolderPath) == false)
            {
                Directory.CreateDirectory(FolderPath);
            }

            if(File.Exists(FilePath) == false)
            {
                File.Create(FilePath);
                fileAlreadyExists = false;
            }

            return fileAlreadyExists;
        }
    }
}

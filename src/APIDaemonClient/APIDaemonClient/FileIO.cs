using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace APIDaemonClient
{
    public static class FileIO
    {
        private const string folderName = "APIDaemonClient";
        private const string logName = "programLog.txt";

        public static string FileName { get; } = "Settings.json";

        public static string LogFilePath
        {
            get
            {
                return Path.Combine(AppLocalDirectory, folderName, logName); 
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

        public static void CheckForExistingSettingsFile() //not using the IFileProvider as the FileIO static class is used before the IOC container is configured.
        {
            if (Directory.Exists(FolderPath) == false)
            {
                Directory.CreateDirectory(FolderPath);
            }

            if(File.Exists(FilePath) == false)
            {
                File.WriteAllText(FilePath, CreateNewSettingsFile());
            }
        }

        public static string CreateNewSettingsFile() //creating a new JObject of settings parameters if the file doesn't already exist....
        {
            var instance = "https://login.microsoftonline.com/{0}";
            var tenantId = "f50df192-055e-42c9-81eb-37bf51d5bbb6";

            dynamic jsonObject = new JObject();

            jsonObject.Instance = instance;
            jsonObject.TenantId = tenantId;
            jsonObject.ClientId = "993ad1fd-2b76-47a7-a584-52bb6cb262d6";
            jsonObject.Authority = String.Format(CultureInfo.InvariantCulture, instance, tenantId);
            jsonObject.ClientSecret = "IFq.56Dz_bE~~VrqRhGgqm0b8g9Gd35A~e";
            jsonObject.BaseAddress = "https://localhost:5001/api/Commands/1";
            jsonObject.ResourceId = "api://c0a043f6-4915-4267-8cff-08265fef5a64/.default";
            jsonObject.LogOutputDirectory = LogFilePath;
            jsonObject.SettingsDirectory = FilePath;

            //jsonObject.list = JToken.FromObject(myList)

            return JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        }

    }
}

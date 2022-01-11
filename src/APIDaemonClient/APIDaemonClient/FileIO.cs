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
            var tenantId = "tenant_id_goes_here";

            dynamic jsonObject = new JObject();

            jsonObject.Instance = instance;
            jsonObject.TenantId = tenantId;
            jsonObject.ClientId = "client_id_goes_here";
            jsonObject.Authority = String.Format(CultureInfo.InvariantCulture, instance, tenantId);
            jsonObject.ClientSecret = "client_secret_goes_here";
            jsonObject.BaseAddress = "https://localhost:5001/api/Commands/1";
            jsonObject.ResourceId = "resource";
            jsonObject.LogOutputDirectory = LogFilePath;
            jsonObject.SettingsDirectory = FilePath;

            return JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
        }

    }
}

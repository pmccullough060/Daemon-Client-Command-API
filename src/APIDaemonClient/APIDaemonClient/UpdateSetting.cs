using APIDaemonClient.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace APIDaemonClient
{
    /// <summary>
    /// this class is responsible for updating the user settings.json file 
    /// </summary>
    public class UpdateSetting : IUpdateSetting
    {
        public JObject Settings { get; private set; }

        private IFileInfo fileInfo;

        private readonly ILogger<UpdateSetting> _logger;
        private readonly IFileProvider _file;

        public UpdateSetting(ILogger<UpdateSetting> logger, IFileProvider file)
        {
            _logger = logger;
            _file = file;
            fileInfo = _file.GetFileInfo(FileIO.FileName);

            GetSettingsAsJObject();
        }

        private void GetSettingsAsJObject() //getting an instance of the JObject in memory
        {
            var stream = fileInfo.CreateReadStream();
            stream.Position = 0;

            using (var streamReader = new StreamReader(stream))
            {
                Settings = JObject.Parse(streamReader.ReadToEnd());
            }
        }

        public void ChangeSettingValue(string settingName, string newValue) //only taking strings as arguments atm probably have an overload of this method to handle an array of strings.
        {
            try
            {
                var x = (JValue)Settings[settingName];
                x.Value = newValue;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Unable to change the parameter: {settingName}");
                _logger.LogInformation($"Unable to change the parameter: {settingName} \n\n\n + {e}");
                return;
            }

            UpdateJsonFile();

            _logger.LogInformation($"Parameter: {settingName} changed to: {newValue}");
        }

        public void ChangeSettingValue(string settingName, string[] newValue)
        {
            throw new NotImplementedException();
        }

        private void UpdateJsonFile()
        {
            using (var streamWriter = new StreamWriter(fileInfo.PhysicalPath, false))
            {
                streamWriter.Write(JsonConvert.SerializeObject(Settings, Formatting.Indented));
            }
        }

        public void Test() //a test method to make sure the attribute logic is working correctly.
        {
            Console.WriteLine("hey");
        }

    }
}

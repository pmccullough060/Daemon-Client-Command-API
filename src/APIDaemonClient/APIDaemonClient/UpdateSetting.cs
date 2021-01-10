using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
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
    public class UpdateSetting
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UpdateSetting> _logger;
        private readonly IFileProvider _file;

        public UpdateSetting(IConfiguration config, ILogger<UpdateSetting> logger, IFileProvider file)
        {
            _config = config;
            _logger = logger;
            _file = file;
        }

        public void GetSettingsAsJObject()
        {
            IFileInfo fileInfo = _file.GetFileInfo("Settings.json");

            var stream = fileInfo.CreateReadStream();
            var streamReader = new StreamReader(stream);
            string text = streamReader.ReadToEnd();
        }

    }
}

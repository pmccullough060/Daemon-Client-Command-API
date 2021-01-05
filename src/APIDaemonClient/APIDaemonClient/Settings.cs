using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace APIDaemonClient
{
    public class Settings
    {
        public string Instance { get; set; } = "https://login.microsoftonline.com/{0}";
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string Authority
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, Instance, TenantId);
            }
           
        }
        public string ClientSecret { get; set; }
        public string BaseAddress { get; set; }
        public string ResourceId { get; set; }
        public string LogOutputDirectory { get; set; }
        public string SettingsDirectory { get; set; }

        public Settings()
        {
            //parameterless constructor
        }
    }
}

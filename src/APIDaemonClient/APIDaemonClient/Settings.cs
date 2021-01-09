using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace APIDaemonClient
{
    public class Settings
    {
        public string Instance { get; set; } = "https://login.microsoftonline.com/{0}";
        public string TenantId { get; set; } = "f50df192-055e-42c9-81eb-37bf51d5bbb6";
        public string ClientId { get; set; } = "993ad1fd-2b76-47a7-a584-52bb6cb262d6";
        public string Authority
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, Instance, TenantId);
            }
           
        }
        public string ClientSecret { get; set; } = "IFq.56Dz_bE~~VrqRhGgqm0b8g9Gd35A~e";
        public string BaseAddress { get; set; } = "https://localhost:5001/api/Commands/1";
        public string ResourceId { get; set; } = "api://c0a043f6-4915-4267-8cff-08265fef5a64/.default";
        public string LogOutputDirectory { get; set; }
        public string SettingsDirectory { get; set; }

        public Settings()
        {
            //parameterless constructor
        }

        //the settings will be updated to contain arrays of the various end points then the daemon client can rattle through all of them..

        //also pick which client you want to post etc....
    }
}

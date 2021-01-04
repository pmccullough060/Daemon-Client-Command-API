using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace APIDaemonClient
{
    public class AuthConfig
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

        public AuthConfig()
        {
            //Retrieving the user secrets.
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            TenantId = config["TenantId"];
            Instance = config["Instance"];
            ClientSecret = config["ClientSecret"];
            ClientId = config["ClientId"];
            BaseAddress = config["BaseAddress"];
            ResourceId = config["ResourceId"];
        }
    }
}

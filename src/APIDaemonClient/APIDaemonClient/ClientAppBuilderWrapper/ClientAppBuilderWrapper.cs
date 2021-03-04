using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace APIDaemonClient
{
    /// <summary>
    /// This class is more or less a wrapper around the IConfidentialClientApplication interface, to make things easier to Test and keep the app.cs tidy
    /// </summary>

    public class ClientAppBuilderWrapper : IClientAppBuilderWrapper
    {
        public IConfidentialClientApplication app { get; set; }
        private string[] resourceIds;


        private readonly IConfiguration _config;
        private readonly ILogger<ClientAppBuilderWrapper> _logger;

        public ClientAppBuilderWrapper(IConfiguration config, ILogger<ClientAppBuilderWrapper> logger)
        {
            _config = config;
            _logger = logger;

            resourceIds = new string[]
            {
                _config["ResourceId"]
            };

            buildClient();
        }

        private void buildClient()
        {
            app = ConfidentialClientApplicationBuilder.Create(_config["ClientId"])
                .WithClientSecret(_config["ClientSecret"])
                .WithAuthority(new Uri(_config["Authority"]))
                .Build();
        }

        public async Task<AuthenticationResult> GetAuthenticationResult()
        {
            try
            {
                var result = await app.AcquireTokenForClient(resourceIds).ExecuteAsync();
                _logger.LogInformation("JWT Bearer Token Aquired from Azure Active Directory");
                return result;
            }
            catch (MsalClientException ex)
            {
                _logger.LogError("JWT Bearer Token *NOT* Aquired from Azure Active Directory" + ex);
                return null;
            }
            catch(Exception ex)
            {
                _logger.LogError("JWT Bearer Token *NOT* Aquired from Azure Active Directory" + ex);
                return null;
            }
        }
    }
}

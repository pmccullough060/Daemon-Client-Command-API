using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Headers;

namespace APIDaemonClient
{
    public class App
    {
        private readonly IConfiguration _config;
        private readonly ILogger<App> _logger;
        private readonly IClientAppBuilderWrapper _clientAppBuilderWrapper;
        private readonly IDaemonHttpClient _daemonHttpClient;
        private readonly IUpdateSettingDialogue _updateSettingDialogue;

        private string AccessToken;

        public App(IConfiguration config, ILogger<App> logger, IClientAppBuilderWrapper clientAppBuilderWrapper, IDaemonHttpClient daemonHttpClient, IUpdateSettingDialogue updateSettingDialogue )
        {
            _daemonHttpClient = daemonHttpClient;
            _config = config;
            _logger = logger;
            _clientAppBuilderWrapper = clientAppBuilderWrapper;
            _updateSettingDialogue = updateSettingDialogue;
        }

        public void Run()
        {
            _updateSettingDialogue.RunDialogue();

            _logger.LogInformation("Calling Azure AAD");

            Console.WriteLine("Making the call...");

            bool sucessfulAuth = GetAuthResult().GetAwaiter().GetResult(); //attempts to successfully authenticate using Azure AAD

            bool successfulHttp = MakeHttpRequests().GetAwaiter().GetResult();
        }

        public async Task<bool> GetAuthResult()
        {
            AuthenticationResult result = null;

            result = await _clientAppBuilderWrapper.GetAuthenticationResult();

            if(result == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred, check application log file for more info:" + FileIO.LogFilePath);
                Console.ResetColor();
                return false;
            }
            else
            {
                //storing the AccessToken....
                AccessToken = result.AccessToken;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Token acquired \n");
                Console.WriteLine(result.AccessToken);
                Console.ResetColor();
            }

            return true;
        }

        public async Task<bool> MakeHttpRequests()
        {
            _daemonHttpClient.ConfigureRequestHeaders(AccessToken);
            var returnbool = await _daemonHttpClient.HttpGetAsync(_config["BaseAddress"]);
            return true;
        }
    }
}

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

        public App(IConfiguration config, ILogger<App> logger, IClientAppBuilderWrapper clientAppBuilderWrapper)
        {
            _config = config;
            _logger = logger;
            _clientAppBuilderWrapper = clientAppBuilderWrapper;
        }

        public void Run()
        {
            _logger.LogInformation("Calling Azure AAD");
            Console.WriteLine("Making the call...");

            bool sucessfulCompletion = RunAsync().GetAwaiter().GetResult();
        }

        private async Task<bool> RunAsync()
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Token acquired \n");
                Console.WriteLine(result.AccessToken);
                Console.ResetColor();
                
            }

            if (!string.IsNullOrEmpty(result.AccessToken))
            {
                var httpClient = new HttpClient();
                var defaultRequestHeaders = httpClient.DefaultRequestHeaders;

                //setting the appropriate media type in the request headers

                if(defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(x => x.MediaType == "application/json"))
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }

                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);

                HttpResponseMessage response = await httpClient.GetAsync(_config["BaseAddress"]);

            }
            return true;
        }
    }
}

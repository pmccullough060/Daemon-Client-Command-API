using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Headers;

namespace APIDaemonClient
{
    public class App
    {
        private readonly IConfiguration _config;
        private readonly ILogger<App> _logger = null;

        public App(IConfiguration config, ILogger<App> logger)
        {
            _config = config;
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation("Calling Azure AAD");

            Console.WriteLine("Makign the call...");

            RunAsync().GetAwaiter().GetResult();


        }

        private async Task RunAsync()
        {
            IConfidentialClientApplication app;

            app = ConfidentialClientApplicationBuilder.Create(_config["ClientId"])
                .WithClientSecret(_config["ClientSecret"])
                .WithAuthority(new Uri(_config["Authority"]))
                .Build();

            string[] ResourceIds = new string[]
            {
                _config["ResourceId"]
            };

            AuthenticationResult result = null;

            try
            {
                result = await app.AcquireTokenForClient(ResourceIds).ExecuteAsync();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Token acquired \n");
                Console.WriteLine(result.AccessToken);
                Console.ResetColor();

                _logger.LogInformation("JWT Bearer Token Aquired from Azure Active Directory");
            }
            catch(MsalClientException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();

                _logger.LogError("JWT Bearer Token Aquired from Azure Active Directory" + ex);
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



        }
    }
}

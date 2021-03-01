using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using APIDaemonClient.Attributes;
using APIDaemonClient.ExtendedConsole;
using APIDaemonClient.Views;

namespace APIDaemonClient
{
    public class App
    {
        private readonly IConfiguration _config;
        private readonly ILogger<App> _logger;
        private readonly IClientAppBuilderWrapper _clientAppBuilderWrapper;
        private readonly IDaemonHttpClient _daemonHttpClient;
        private readonly ICommandParser _commandParser;
        private readonly IMainView _mainView;

        private string AccessToken;

        public App(ICommandParser commandParser, IMainView mainView, IConfiguration config, ILogger<App> logger, IClientAppBuilderWrapper clientAppBuilderWrapper, IDaemonHttpClient daemonHttpClient)
        {
            _daemonHttpClient = daemonHttpClient;
            _config = config;
            _logger = logger;
            _clientAppBuilderWrapper = clientAppBuilderWrapper;
            _commandParser = commandParser;
            _mainView = mainView;
        }

        public void Run()
        {
            _mainView.StartMenu();

            GetAuthResult().GetAwaiter().GetResult(); 

            //MakeHttpRequests().GetAwaiter().GetResult();

            while(true)  //loop has to be broken manually;
            {
                _commandParser.Parse();
            }
        }

        public async Task<bool> GetAuthResult()
        {
            AuthenticationResult result = null;

            result = await _clientAppBuilderWrapper.GetAuthenticationResult();

            if(result == null)
            {
                ConsoleEx.WriteLineRed("An error occurred, check application log file for more info:" + FileIO.LogFilePath);
                return false;
            }
            else
            {
                //storing the AccessToken....
                AccessToken = result.AccessToken;

                ConsoleEx.WriteLineDarkGray("Token acquired \n");
                ConsoleEx.WriteLineGreen(result.AccessToken);
            }

            return true;
        }

        public async Task<bool> MakeHttpRequests()
        {
            _daemonHttpClient.ConfigureRequestHeaders(AccessToken);
            var returnbool = await _daemonHttpClient.HttpGetAsync();
            return true;
        }
    }
}

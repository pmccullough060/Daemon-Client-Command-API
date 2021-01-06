using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

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
            _logger.LogInformation("hey");
        }
    }
}

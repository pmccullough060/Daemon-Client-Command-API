using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIDaemonClient
{
    public class App
    {
        private readonly IConfiguration _config;

        public App(IConfiguration config)
        {
            _config = config;
        }

        public void Run()
        {
            var settings = new Settings();
            var logDirectory = _config.GetValue<string>(nameof(settings.LogOutputDirectory));

            var log = new LoggerConfiguration()
                .WriteTo.File(logDirectory)
                .CreateLogger();

            log.Information("Application Started");

        }
    }
}

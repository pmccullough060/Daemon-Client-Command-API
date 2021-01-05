using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace APIDaemonClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            //calls the Run method in App, which replaces main...
            serviceProvider.GetService<App>().Run();

        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            var config = LoadConfiguration();

            var log = new LoggerConfiguration()
                .WriteTo.File("LogOutputDirectory")
                .CreateLogger();

            services.AddSingleton(config);

            //registering App.cs
            services.AddTransient<App>();

            return services;
        }

        public static IConfiguration LoadConfiguration()
        {
            FileIO.CheckForExistingSettingsFile(); //checks for settigns file, creates it if doesnt exist..

            var builder = new ConfigurationBuilder()
                .SetBasePath(FileIO.FolderPath)
                .AddJsonFile(FileIO.FileName)
                .Build();

            return builder;
        }
    }
}

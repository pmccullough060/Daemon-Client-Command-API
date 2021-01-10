using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace APIDaemonClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection servicesCollection = new ServiceCollection();
            var services = ConfigureServices(servicesCollection);

            var serviceProvider = services.BuildServiceProvider();

            //calls the Run method in App, which replaces main...
            serviceProvider.GetService<App>().Run();
        }

        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var serilogLogger = new LoggerConfiguration()
                .WriteTo.File(FileIO.LogFilePath)
                .CreateLogger();

            //loading configuration filek
            var config = LoadConfiguration();

            //adding serilog
            services.AddLogging(Builder =>
            {
                Builder.AddSerilog(logger: serilogLogger, dispose: true);
            });

            //isntantiating a physical file provider.. wraps the System.IO.File type and provides access to the physical file system.
            IFileProvider physicalProvider = new PhysicalFileProvider(FileIO.FolderPath);

            //singleton as: -> object is the same for everyobject and every request..
            services.AddSingleton(config);
            services.AddSingleton<IFileProvider>(physicalProvider);

            //registering App.cs
            services.AddTransient<App>();
            services.AddTransient<IClientAppBuilderWrapper,ClientAppBuilderWrapper>();
            services.AddTransient<IDaemonHttpClient, DaemonHttpClient>();

            return services;
        }

        public static IConfiguration LoadConfiguration()
        {
            FileIO.CheckForExistingSettingsFile(); //checks for settigns file, creates it if doesnt exist..

            var builder = new ConfigurationBuilder()
                .SetBasePath(FileIO.FolderPath)
                .AddJsonFile(FileIO.FileName, optional: false, reloadOnChange: true)
                .Build();

            return builder;
        }
    }
}

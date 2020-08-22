using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzFunctionsDomain
{
    public class HostBuilderFactory
    {
        public IHost BuildHost()
        {
            var hostBuilder = new HostBuilder();
            hostBuilder.ConfigureAppConfiguration(ConfigureApp);
            hostBuilder.ConfigureServices(ConfigureServices);

            var host = hostBuilder.Build();
            return host;
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
            services.AddTransient<IMyService, MyService>();
        }

        private void ConfigureApp(HostBuilderContext context, IConfigurationBuilder configuration)
        {

            var appSettings = GetFilesWithExtension("json")
                .FirstOrDefault(file => Path.GetFileName(file.ToLower()).Equals("appsettings.json"));

            if (!string.IsNullOrWhiteSpace(appSettings))
            {
                configuration.AddJsonFile(appSettings);
            }
        }

        private string[] GetFilesWithExtension(string extension)
        {
            var currentDirectoryPath = Environment.CurrentDirectory;
            var searchResults = Directory.EnumerateFiles(currentDirectoryPath)
                .Where(fileName => Path.GetExtension(fileName.ToLower()).EndsWith(extension))
                .ToArray();
            return searchResults;
        }
    }
}

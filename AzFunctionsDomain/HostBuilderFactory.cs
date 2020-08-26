using System;
using System.IO;
using System.Linq;
using AzCosmosDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AzFunctionsDomain
{
    public class HostBuilderFactory
    {
        private static Lazy<IHost> _host = new Lazy<IHost>(() =>
        {
            var host = new HostBuilderFactory().BuildHost();
            return host;
        });

        public static IHost Host
        {
            get
            {
                return _host.Value;
            }
        }

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
            
            services.Configure<AzCosmosOptions>(context.Configuration.GetSection("AzCosmosOptions"));
            services.AddTransient<IMyService, MyService>();
            services.AddSingleton<ICosmosClientFactory, CosmosClientFactory>();
            services.AddTransient<IUserDtoReader, UserDtoReader>();
        }

        private void ConfigureApp(HostBuilderContext context, IConfigurationBuilder configuration)
        {

            var appSettings = GetFilesWithExtension("json")
                .Where(file => Path.GetFileName(file.ToLower()).StartsWith("appsettings"));

            foreach (var appSetting in appSettings)
            {
                configuration.AddJsonFile(appSetting);
            }

            configuration.AddEnvironmentVariables();
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

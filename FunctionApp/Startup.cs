using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using Demo.Domain.Services;

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]

namespace FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.ConfigurationBuilder.Build();

        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.GetContext().Configuration;

            builder.Services.RegisterDomainServices(config);
        }
    }
}

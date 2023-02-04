using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog.Web;
using Microsoft.AspNetCore;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using WebApp.Infrastructure.Extensions;
using Demo.Domain.Infrastructure;
using WebApp.Infrastructure;
using Azure.Core;
using Azure.Identity;

namespace WebApp
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = typeof(Program).Namespace;

        public static int Main(string[] args)
        {
            IConfiguration configuration = GetConfiguration();

            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                logger.Info("Configuring web host ({ApplicationContext})...", AppName);

                IWebHost webhost = BuildWebHost(configuration, args);
                logger.Info("Applying migrations ({ApplicationContext})...", AppName);

                webhost.MigrateDbContext<BlogDbContext>((context, serviceProvider) =>
                {
                    var env = serviceProvider.GetService<IWebHostEnvironment>();
                    var settings = serviceProvider.GetService<IOptions<BlogingSettings>>();
                    var logger = serviceProvider.GetService<ILogger<BlogDbContextSeed>>();

                    new BlogDbContextSeed()
                        .SeedAsync(context, env, settings, logger)
                        .Wait();
                });

                logger.Info("Starting web host ({ApplicationContext})...", AppName);

                webhost.Run();
                return 0;
            }
            catch (Exception e)
            {
                //NLog: catch setup errors
                logger.Error(e, "Stopped program because of exception");
                return 1;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }

            //CreateHostBuilder(args).Build().Run();
        }


        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                              optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    // Requires `using Microsoft.Extensions.Logging;`
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseNLog()
                .Build();


        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            if (config.GetValue<bool>("UseVault", false))
            {
                builder.AddAzureKeyVault(
                    new Uri($"https://{config["Vault:Name"]}.vault.azure.net/"),
                    new ClientSecretCredential(config["Vault:TenantId"], config["Vault:ClientId"], config["Vault:ClientSecret"]));
            }

            return builder.Build();
        }
    }
}

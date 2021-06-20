using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Demo.Domain.Infrastructure;
using WebApi.Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using WebApi.Infrastructure;

namespace WebApi
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class WebApi : StatelessService
    {
        public WebApi(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        var webHost = BuildWebHost( serviceContext,  url,  listener);
                        

                    webHost.MigrateDbContext<BlogDbContext>((context, serviceProvider) =>
                    {
                        var env = serviceProvider.GetService<IWebHostEnvironment>();
                        var settings = serviceProvider.GetService<IOptions<BlogingSettings>>();
                        var logger = serviceProvider.GetService<ILogger<BlogDbContextSeed>>();

                        new BlogDbContextSeed()
                            .SeedAsync(context, env, settings, logger)
                            .Wait();
                    });


                        return webHost;

                        //ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        //return new WebHostBuilder() 
                        //            .UseKestrel(opt =>
                        //            {
                        //                int port = serviceContext.CodePackageActivationContext.GetEndpoint("ServiceEndpoint").Port;
                        //                opt.Listen(IPAddress.IPv6Any, port, listenOptions =>
                        //                {
                        //                    listenOptions.UseHttps(GetCertificateFromStore());
                        //                });
                        //            })
                        //            .ConfigureServices(
                        //                services => services
                        //                    .AddSingleton<StatelessServiceContext>(serviceContext))
                        //            .UseContentRoot(Directory.GetCurrentDirectory())
                        //            .ConfigureAppConfiguration((hostingContext, config) =>
                        //            {
                        //                var env = hostingContext.HostingEnvironment;
                        //                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        //                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                        //                          optional: true, reloadOnChange: true);
                        //                config.AddEnvironmentVariables();
                        //            })
                        //             .ConfigureLogging((loggingBuilder) =>
                        //            {
                        //                //loggingBuilder.AddApplicationInsights("");
                        //                loggingBuilder.AddConsole();
                        //                loggingBuilder.AddDebug();
                        //            })
                        //            .UseStartup<Startup>()
                        //            .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                        //            .UseUrls(url)
                        //            .UseSerilog()
                        //            .Build();
                    }))
            };
        }

        private IWebHost BuildWebHost(StatelessServiceContext serviceContext, string url, AspNetCoreCommunicationListener listener)
        {
            ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

            var webHost= new WebHostBuilder()
                                    .UseKestrel(opt =>
                                    {
                                        int port = serviceContext.CodePackageActivationContext.GetEndpoint("ServiceEndpoint").Port;
                                        opt.Listen(IPAddress.IPv6Any, port, listenOptions =>
                                        {
                                            listenOptions.UseHttps(GetCertificateFromStore());
                                        });
                                    })
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton<StatelessServiceContext>(serviceContext))
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .ConfigureAppConfiguration((hostingContext, config) =>
                                    {
                                        var env = hostingContext.HostingEnvironment;
                                        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                              .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                                                  optional: true, reloadOnChange: true);
                                        config.AddEnvironmentVariables();
                                    })
                                     .ConfigureLogging((hostingContext,loggingBuilder) =>
                                     {
                                         //loggingBuilder.AddApplicationInsights("");
                                         loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                                         loggingBuilder.AddConsole();
                                         loggingBuilder.AddDebug();
                                     })
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url)
                                    .UseSerilog()
                                    .Build();


            return webHost;
        }

        /// <summary>
        /// Finds the ASP .NET Core HTTPS development certificate in development environment. Update this method to use the appropriate certificate for production environment.
        /// </summary>
        /// <returns>Returns the ASP .NET Core HTTPS development certificate</returns>
        private static X509Certificate2 GetCertificateFromStore()
        {
            string aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.Equals(aspNetCoreEnvironment, "Development", StringComparison.OrdinalIgnoreCase))
            {
                const string aspNetHttpsOid = "1.3.6.1.4.1.311.84.1.1";
                const string CNName = "CN=localhost";
                using (X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
                {
                    store.Open(OpenFlags.ReadOnly);
                    var certCollection = store.Certificates;
                    var currentCerts = certCollection.Find(X509FindType.FindByExtension, aspNetHttpsOid, true);
                    currentCerts = currentCerts.Find(X509FindType.FindByIssuerDistinguishedName, CNName, true);
                    return currentCerts.Count == 0 ? null : currentCerts[0];
                }
            }
            else
            {
                throw new NotImplementedException("GetCertificateFromStore should be updated to retrieve the certificate for non Development environment");
            }
        }
    }
}

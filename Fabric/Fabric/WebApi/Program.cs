using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ServiceFabric.Services.Runtime;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Datadog.Logs;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.

                var AzureStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=serilogblobloggingname;AccountKey=blablablakey;EndpointSuffix=core.windows.net";

                //https://docs.datadoghq.com/logs/log_collection/csharp/?tab=serilog


                Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(new RenderedCompactJsonFormatter()).WriteTo.Debug(outputTemplate: DateTime.Now.ToString())

                //.WriteTo.AzureBlobStorage(AzureStorageConnectionString) //sink log to AzureBlobStorage
                //.WriteTo.DatadogLogs("<API_KEY>", configuration: new DatadogConfiguration { Url = "https://http-intake.logs.datadoghq.com" })//sink log to DatadogLogs

                //.WriteTo.ApplicationInsights(new TelemetryConfiguration { InstrumentationKey = "xxxxxxxxx" }, TelemetryConverter.Traces) //sink log to application insight

                .WriteTo.File("c:\\Logs\\log.txt", rollingInterval: RollingInterval.Day)//sink to file
                .CreateLogger();

                ServiceRuntime.RegisterServiceAsync("WebApiType",
                    context => new WebApi(context)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(WebApi).Name);

                // Prevents this host process from terminating so services keeps running. 
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}

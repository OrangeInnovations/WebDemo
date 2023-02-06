
namespace FunctionApp.Test.Helpers
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Threading.Tasks;
    using Corvus.Testing.AzureFunctions;
    using global::Xunit;
    using global::Xunit.Abstractions;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using ILogger = Microsoft.Extensions.Logging.ILogger;


    /// </summary>
    public class AzureFunctionFixture : IAsyncLifetime
    {
        private readonly FunctionsController function;
        public HttpClient HttpClient;

        public AzureFunctionFixture(IMessageSink output)
        {
            var path=Assembly.GetExecutingAssembly().Location;

            ILogger logger = new LoggerFactory()
                .AddSerilog(
                    new LoggerConfiguration()
                        .WriteTo.File(@$"{path}\{this.GetType().FullName}.log")
                        .WriteTo.TestOutput(output)
                        .MinimumLevel.Debug()
                        .CreateLogger())
                .CreateLogger("Xunit Demo Function App tests");

            this.function = new FunctionsController(logger);

            HttpClient = new HttpClient();

            Uri baseUri = new Uri(BasicUri);
            HttpClient.BaseAddress = baseUri;
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static int Port => 7076;
        public static string BasicUri => $"http://localhost:{Port}/api/";

        public async Task InitializeAsync()
        {
            await this.function.StartFunctionsInstance(
                "FunctionApp",
                Port,
                "net6.0");
        }

        public Task DisposeAsync()
        {
            this.function.TeardownFunctions();
            return Task.CompletedTask;
        }
    }
}

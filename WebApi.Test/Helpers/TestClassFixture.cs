using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;


namespace WebApi.Test.Helpers
{
    public class TestClassFixture : IDisposable
    {
        
        public TestServer Server { get; set; }
        public HttpClient Client { get; set; }
        public TestClassFixture()
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder();
            webHostBuilder.UseDefaultServiceProvider(options => options.ValidateScopes = false);
            webHostBuilder.UseEnvironment("Development");

            webHostBuilder.ConfigureAppConfiguration((builderContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                //config.AddJsonFile($"appsettings.Development.json", optional: false, reloadOnChange: true)
                //             .AddEnvironmentVariables();

                config.AddJsonFile("appsettings.json",
                    optional: true,
                    reloadOnChange: true);

            });

            Server = new TestServer(webHostBuilder.UseStartup<Startup>());
            Client = Server.CreateClient();
        }

        
        public void Dispose()
        {
            
            Client.Dispose();
            Server.Dispose();
        }
    }
}

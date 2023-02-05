using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.DependencyInjection.Logging;
using Xunit.DependencyInjection;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Demo.Domain.Services.Test
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder) => hostBuilder
        .ConfigureWebHost(webHostBuilder =>
        {
            webHostBuilder.UseTestServer().UseStartup<Startup>().Configure(applicationBuilder =>
            {
                
            });
        })
        .ConfigureAppConfiguration((context, builder) =>
        {
            builder.AddJsonFile("appsettings.json",
                    optional: true,
                    reloadOnChange: true);
        });

        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            var config = context.Configuration;
            services.RegisterDomainServices(config);

        }

        public void Configure(ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor)
        {
            loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor));
        }
    }
}

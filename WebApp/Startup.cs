using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using WebApp.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Demo.Domain.Services.AutofacModules;
using Demo.Domain.Services;

namespace WebApp
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("start ConfigureServices");

            OktaConfig oktaConfig = AddOktaConfig(services, Configuration);

            services
            .AddApplicationInsights(Configuration)
            .AddCustomMvc()
            .AddCustomDbContext(Configuration)
            .AddCustomSwagger(Configuration)
            .AddCustomIntegrations(Configuration)
            .AddCustomConfiguration(Configuration)
            .AddCustomOktaAuthentication(Configuration, oktaConfig)
            .AddCustomAuthorization(Configuration);

            
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddOtherServices(Configuration);


            //configure autofac
            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new ApplicationModule());

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            _logger.LogInformation("start Configure");

            //loggerFactory.AddAzureWebAppDiagnostics();
            //loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace);

            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                loggerFactory.CreateLogger<Startup>().LogDebug("Using PATH BASE '{pathBase}'", pathBase);
                app.UsePathBase(pathBase);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Blogging.API V1");
                   //c.OAuthClientId("bloggingswaggerui");
                   //c.OAuthAppName("Blogging Swagger UI");
               });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");
            ConfigureAuth(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        protected virtual void ConfigureAuth(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        private static OktaConfig AddOktaConfig(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<OktaConfig>(configuration.GetSection("Okta"));

            services.AddSingleton<IConfiguration>(configuration);


            IConfigurationSection section = configuration.GetSection("Okta");


            OktaConfig settings = new OktaConfig();

            section.Bind(settings);


            // then we register the instance into the services collection
            services.AddSingleton(settings);

            return settings;
        }
    }
}

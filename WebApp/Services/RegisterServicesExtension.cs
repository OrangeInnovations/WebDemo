using Demo.Domain.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using WebApp.Controllers;
using WebApp.Infrastructure.Filters;
using System.IdentityModel.Tokens.Jwt;
using Okta.AspNetCore;
using System.IO;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApp.Services
{
    public static class RegisterServicesExtension
    {
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration);
            //services.AddApplicationInsightsKubernetesEnricher();

            return services;
        }

        
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            // Add framework services.
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
            // Added for functional tests
            .AddApplicationPart(typeof(MyUserController).Assembly)
            .AddNewtonsoftJson()
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer()
                   .AddDbContext<BlogDbContext>(options =>
                       {
                           options.UseSqlServer(configuration["ConnectionString"],
                               sqlServerOptionsAction: sqlOptions =>
                               {
                                   sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                   sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                               });
                       },
                       ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                   );

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                //options.DescribeAllEnumsAsStrings();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Web API",
                    Description = "The Blogging Service HTTP API",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "David Chen",
                        Email = string.Empty,
                        
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under David",
                       // Url = new Uri("https://example.com/license"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
        }

        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<BlogingSettings>(configuration);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });

            return services;
        }

        public static IServiceCollection AddCustomOktaAuthentication(this IServiceCollection services, IConfiguration configuration, OktaConfig oktaConfig)
        {
            // prevent from mapping "sub" claim to nameidentifier.
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = oktaConfig.IssuerURI;// "https://dev-21067104.okta.com/oauth2/default";
                options.Audience = oktaConfig.Audience;// "api://default";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "groups"
                };
            });


            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;

            //}).AddJwtBearer(options =>
            //{
            //    options.Authority = oktaConfig.Issuer;//configuration.GetValue<string>("Okta:Issuer"); ;
            //    options.RequireHttpsMetadata = false;
            //    options.Audience = oktaConfig.ClientId;
            //});
            //.AddOpenIdConnect(options =>
            //{
            //    //options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.Authority = oktaConfig.Domain + "/oauth2/default";
            //    options.RequireHttpsMetadata = true;
            //    options.ClientId = oktaConfig.ClientId ;
            //    options.ClientSecret = oktaConfig.ClientSecret;
            //    options.ResponseType = OpenIdConnectResponseType.Code;
            //    options.GetClaimsFromUserInfoEndpoint = true;
            //    options.Scope.Add("openid");
            //    options.Scope.Add("profile");
            //    options.SaveTokens = true;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = "name",
            //        RoleClaimType = "groups",
            //        ValidateIssuer = true
            //    };
            //}).AddOktaWebApi(new OktaWebApiOptions()
            //{
            //    OktaDomain = oktaConfig.Domain,
            //    //AuthorizationServerId = "default",
            //    Audience = oktaConfig.ClientId,
            //    //ClientId=oktaConfig.ClientId,

            //});

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
            //    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
            //    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
            //})
            //.AddOktaWebApi(new OktaWebApiOptions()
            //{
            //    OktaDomain = configuration["Okta:Domain"],
            //});

            return services;
        }

        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization();


            return services;
        }
        public static IServiceCollection AddOtherServices(this IServiceCollection services, IConfiguration configuration)
        {

            //services.AddScoped<IDataServices, SampleDbDataServices>();

            

            return services;
        }
    }
}

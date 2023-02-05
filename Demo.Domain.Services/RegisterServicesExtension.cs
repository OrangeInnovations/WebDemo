using Demo.Domain.AggregatesModels.BlogAggregate;
using Demo.Domain.AggregatesModels.UserAggregate;
using Demo.Domain.Infrastructure;
using Demo.Domain.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Autofac;
using Demo.Domain.Services.Behaviors;

namespace Demo.Domain.Services
{
    public static class RegisterServicesExtension
    {
       
        public static IServiceCollection RegisterDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDbContext(configuration);

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddScoped<IMyUserRepository, MyUserRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();


            services.AddMediatR(Assembly.GetExecutingAssembly());

            //services.AddScoped<ServiceFactory>(context =>
            //{
            //    var componentContext = context.GetRequiredService<IComponentContext>();
            //    return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            //});

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

            return services;
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer()
                   .AddDbContext<BlogDbContext>(options =>
                   {
                       options.UseSqlServer(configuration["ConnectionString"],
                           sqlServerOptionsAction: sqlOptions =>
                           {
                               //sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                               sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                           });
                   },
                       ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                   );

            return services;
        }
    }
}

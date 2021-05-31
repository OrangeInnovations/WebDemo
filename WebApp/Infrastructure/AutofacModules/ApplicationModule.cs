using Autofac;
using Demo.Domain.AggregatesModels.BlogAggregate;
using Demo.Domain.AggregatesModels.UserAggregate;
using Demo.Domain.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MyUserRepository>()
                .As<IMyUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BlogRepository>()
                .As<IBlogRepository>()
                .InstancePerLifetimeScope();
        }
    }
}

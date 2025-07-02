using Autofac;
using Autofac.Integration.Mvc;
using gBanker.Data;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using gBanker.Service;
using gBanker.Web.Controllers;
using gBanker.Web.Mappings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Reflection;
using System.Web.Mvc;

namespace gBanker.Web.Tests.Dependencies
{
    public class DependencyInjector
    {
        public static IContainer SetAutofacContainer()
        {
            var builder = new ContainerBuilder();
            //var assemblies = Assembly.Load("gBanker.Web");
            builder.RegisterControllers(typeof(HomeController).Assembly);

            // builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<UnitOfWorkCodeFirst>().As<IUnitOfWorkCodeFirst>().InstancePerLifetimeScope();
            //builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterType<DatabaseFactoryCodeFirst>().As<IDatabaseFactoryCodeFirst>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(MemberCategoryRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(MemberCategoryService).Assembly)
           .Where(t => t.Name.EndsWith("Service"))
           .AsImplementedInterfaces().InstancePerLifetimeScope();

            //builder.RegisterAssemblyTypes(typeof(DefaultFormsAuthentication).Assembly)
            //.Where(t => t.Name.EndsWith("Authentication"))
            //.AsImplementedInterfaces().InstancePerHttpRequest();

            builder.Register(c => new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new UserManagementEntities())))
                .As<UserManager<ApplicationUser>>().InstancePerLifetimeScope();

            builder.RegisterFilterProvider();
          return  builder.Build();
           
        }
    }
}

using Autofac;
using Autofac.Integration.Mvc;
using gBanker.Data;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using gBanker.Service;
using gBanker.Web.Helpers;
using gBanker.Web.Mappings;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Reflection;
using System.Web.Mvc;
using Autofac.Integration.WebApi;
using gBanker.Web.ApiControllers;
using System.Web.Http;
using gBanker.Service.ReportServies;

namespace gBanker.Web
{
    public static class Bootstrapper
    {
        //This method will be called when the application runs for the first time....
        public static void Run()
        {
            SetAutofacContainer();
            //Configure AutoMapper
            AutoMapperConfiguration.Configure();
        }
        private static void SetAutofacContainer()
        {
            var config = GlobalConfiguration.Configuration;

            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<UnitOfWorkCodeFirst>().As<IUnitOfWorkCodeFirst>().InstancePerRequest();
            //builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerRequest();
            builder.RegisterType<DatabaseFactoryCodeFirst>().As<IDatabaseFactoryCodeFirst>().InstancePerRequest();
            
            builder.RegisterAssemblyTypes(typeof(MemberCategoryRepository).Assembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(MemberCategoryService).Assembly)
           .Where(t => t.Name.EndsWith("Service"))  
           .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(MemberCategoryService).Assembly)
            .Where(t => t.Name.EndsWith("OLRS"))
            .AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterType<Logger>().As<ILogger>().InstancePerRequest();
            //builder.RegisterAssemblyTypes(typeof(DefaultFormsAuthentication).Assembly)
            //.Where(t => t.Name.EndsWith("Authentication"))
            //.AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterType<SMSSendMessageService>().As<ISMSSendMessageService>().InstancePerRequest();
            builder.RegisterType<SMSSendMessageRepository>().As<ISMSSendMessageRepository>().InstancePerRequest();
            builder.RegisterType<SMSSenderService>().As<ISMSSenderService>().InstancePerRequest();

            builder.RegisterType<UltimateReportServiceMemberPortal>().As<IUltimateReportServiceMemberPortal>().InstancePerRequest();

            builder.Register(c => new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new UserManagementEntities())))
                .As<UserManager<ApplicationUser>>().InstancePerRequest();

            builder.RegisterFilterProvider();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //  DependencyResolver.SetResolver(new AutofacWebApiDependencyResolver(container));

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
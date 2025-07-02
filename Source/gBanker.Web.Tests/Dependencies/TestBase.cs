using Autofac;
using Autofac.Integration.Mvc;
using gBanker.Web.Mappings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using System.Web.Http.Dependencies;
using System.Web.Mvc;


namespace gBanker.Web.Tests.Dependencies
{

    public class TestBase
    {
        public IDependencyResolver _originalResolver = null;
        public ILifetimeScopeProvider _scopeProvider = null;
        public IContainer container = null;
        public TestBase()
        {
            container = DependencyInjector.SetAutofacContainer();
            InitializeDependencyResolver();
            //AutoMapperConfiguration.Configure();
            HttpContext.Current = HttpContextFake.FakeHttpContext();
            HttpContext.Current.User = new GenericPrincipal(
                    new GenericIdentity("0138"),
                    new string[0]
                    );
            PopulateSessionData();
            //
        }
        [ClassInitialize()]
        public void InitializeDependencyResolver()
        {
            this._scopeProvider = new AutofacTestLifetimeScopeProvider(container);
            var resolver = new AutofacDependencyResolver(container, _scopeProvider);
            this._originalResolver = DependencyResolver.Current;
            DependencyResolver.SetResolver(resolver);
        }
        [ClassCleanup()]
        public void DisposeDependencyResolver()
        {
            DependencyResolver.SetResolver(this._originalResolver);
        }
        /// <summary>
        /// Sets up session data to work correctly in the test cases.
        /// </summary>
       protected virtual void PopulateSessionData()
        {

        }
    }
}

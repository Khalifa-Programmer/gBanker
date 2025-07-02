using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Web.Tests.Dependencies
{

    public class AutofacTestLifetimeScopeProvider : ILifetimeScopeProvider
    {
        private readonly IContainer _container;
        private ILifetimeScope _scope;

        public AutofacTestLifetimeScopeProvider(IContainer container)
        {
            this._container = container;
        }

        public ILifetimeScope ApplicationContainer
        {
            get { return this._container; }
        }

        public void EndLifetimeScope()
        {
            if (this._scope != null)
            {
                this._scope.Dispose();
                this._scope = null;
            }
        }

        public ILifetimeScope GetLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            
            if (this._scope == null)
            {
                this._scope = (configurationAction == null)
                       ? this.ApplicationContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag)
                       : this.ApplicationContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag, configurationAction);
            }

            return this._scope;
        }
    }
}
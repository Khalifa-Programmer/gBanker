using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace gBanker.Web.Filters
{
    public class GlobalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private string resourceKey = string.Empty;
        public GlobalizedDisplayNameAttribute(string resourceKey)
        {
            this.resourceKey = resourceKey;
        }
        public override string DisplayName
        {
            get
            {
                string name = Labels.ResourceManager.GetString(resourceKey);
                return string.IsNullOrEmpty(name) ? string.Format("[{0}]", base.DisplayName) : name;
            }
        }
    }
}
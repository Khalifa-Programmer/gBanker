using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class AspNetSecurityModuleViewModel : BaseModel
    {
        public int AspNetSecurityModuleId { get; set; }
        public string SecurityModuleCode { get; set; }

        public string LinkText { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public int? ParentModuleId { get; set; }

        public bool? IsActive { get; set; }

        public bool IsSelectedForRole { get; set; }
        public int RoleModuleId { get; set; }
        /// <summary>
        public int SecurityLevelId { get; set; }

        public int RoleId { get; set; }
        public int MenuLevel { get; set; }
    }
}
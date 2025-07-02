using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{

    public partial class AspNetRoleModuleViewModel : BaseModel
    {
        public int AspNetRoledModuleViewModel { get; set; }

        [Required]
        [StringLength(128)]
        [Display( Name="Security Role Name")]
        public string RoleId { get; set; }

        public int ModuleId { get; set; }

        public int SecurityLevelId { get; set; }

    }
}
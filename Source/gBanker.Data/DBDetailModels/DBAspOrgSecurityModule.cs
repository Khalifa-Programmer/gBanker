using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace gBanker.Data.DBDetailModels
{
    public class DBAspOrgSecurityModule
    {

        public int AspNetSecurityModuleId { get; set; }

      
        public string SecurityModuleCode { get; set; }

      
        public string LinkText { get; set; }

     
        public string ControllerName { get; set; }

      
        public string ActionName { get; set; }

        public int? ParentModuleId { get; set; }

        public bool? IsActive { get; set; }


        [NotMapped]
        public bool IsSelectedForRole { get; set; }

        [NotMapped]
        public int RoleModuleId { get; set; }

        [NotMapped]
        public int SecurityLevelId { get; set; }
        [NotMapped]
        public int RoleId { get; set; }
        public int? OrgID { get; set; }
       //  public int AspNetOrgModuleID { get; set; }
        

    }
}

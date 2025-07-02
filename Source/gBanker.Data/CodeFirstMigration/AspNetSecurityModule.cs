namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AspNetSecurityModule")]
    public partial class AspNetSecurityModule
    {
        public AspNetSecurityModule()
        {
            AspNetRoleModules = new HashSet<AspNetRoleModule>();
        }

        public int AspNetSecurityModuleId { get; set; }

        [Required]
        [StringLength(10)]
        public string SecurityModuleCode { get; set; }

        [StringLength(100)]
        public string LinkText { get; set; }

        [StringLength(100)]
        public string ControllerName { get; set; }

        [StringLength(100)]
        public string ActionName { get; set; }

        public int? ParentModuleId { get; set; }

        public bool? IsActive { get; set; }


        public bool? IsMenuItem { get; set; }
        public int? DisplayOrder { get; set; }
        public int? MenuLevel { get; set; }
        /// <summary>
        /// This field is not mapped with db but used for UI.
        /// </summary>
        [NotMapped]
        public bool IsSelectedForRole { get; set; }
        /// <summary>
        /// This field is not mapped with db but used for UI.
        /// </summary>
        [NotMapped]
        public int RoleModuleId { get; set; }
        /// <summary>
        /// This field is not mapped with db but used for UI.
        /// </summary>
        [NotMapped]
        public int SecurityLevelId { get; set; }
        [NotMapped]
        public int RoleId { get; set; }
        [NotMapped]
        public int? OrgID { get; set; }
    
        public virtual ICollection<AspNetOrgModule> AspNetOrgModules { get; set; }
        public virtual ICollection<AspNetRoleModule> AspNetRoleModules { get; set; }

    }
}

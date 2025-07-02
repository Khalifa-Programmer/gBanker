namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AspNetRoleModule")]
    public partial class AspNetRoleModule
    {
        public int AspNetRoleModuleId { get; set; }

        [Required]
        [StringLength(128)]
        public string RoleId { get; set; }

        public int ModuleId { get; set; }

        public int SecurityLevelId { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [StringLength(256)]
        public string CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual AspNetRole AspNetRole { get; set; }

        public virtual AspNetSecurityLevel AspNetSecurityLevel { get; set; }

        public virtual AspNetSecurityModule AspNetSecurityModule { get; set; }

        /// <summary>
        /// This field is not mapped with db but used for UI.
        /// </summary>
        [NotMapped]
        public bool IsSelectedForRole { get; set; }
    }
}

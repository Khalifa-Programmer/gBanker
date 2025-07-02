namespace gBanker.Data.CodeFirstMigration
{
    using gBanker.Data.CodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AspNetOrgModule")]
    public partial class AspNetOrgModule
    {
        public AspNetOrgModule()
        {
            AspNetSecurityModules = new HashSet<AspNetSecurityModule>();
        }
        [Key]
        [Column(Order = 0)]
        public int AspNetOrgModuleID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AspNetSecurityModuleId { get; set; }

        public int? OrgID { get; set; }
        [NotMapped]
        public bool IsSelectedForRole { get; set; }
        public virtual ICollection<AspNetSecurityModule> AspNetSecurityModules { get; set; }
    }
}

namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AspNetSecurityLevel")]
    public partial class AspNetSecurityLevel
    {
        public AspNetSecurityLevel()
        {
            AspNetRoleModules = new HashSet<AspNetRoleModule>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AspNetSecurityLevelId { get; set; }

        [Required]
        [StringLength(10)]
        public string SecurityLevelCode { get; set; }

        [Required]
        [StringLength(50)]
        public string SecurityLevelName { get; set; }

        public virtual ICollection<AspNetRoleModule> AspNetRoleModules { get; set; }
    }
}

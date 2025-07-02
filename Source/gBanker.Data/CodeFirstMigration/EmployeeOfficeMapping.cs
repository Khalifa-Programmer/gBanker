namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeOfficeMapping")]
    public partial class EmployeeOfficeMapping
    {
        public int EmployeeOfficeMappingID { get; set; }

        public short EmployeeID { get; set; }

        public int OfficeID { get; set; }

        public bool? IsActive { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }
        public int OrgID { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Office Office { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }

    }
}

namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Holiday")]
    public partial class Holiday
    {
        public int HolidayID { get; set; }

        [Column(TypeName = "date")]
        public DateTime BusinessDate { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        [Required]
        [StringLength(120)]
        public string Description { get; set; }

        [StringLength(40)]
        public string HolidayType { get; set; }
        [Column(TypeName = "date")]
        public DateTime? HolidayEndDate { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public int OrgID { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Center Center { get; set; }

        public virtual Office Office { get; set; }
    }
}

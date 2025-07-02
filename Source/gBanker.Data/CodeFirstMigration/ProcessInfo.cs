namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProcessInfo")]
    public partial class ProcessInfo
    {
        public long ProcessInfoID { get; set; }

        public int OfficeID { get; set; }

        [Column(TypeName = "date")]
        public DateTime BusinessDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime InitialDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ClosingDate { get; set; }

        public byte InitialProcessCount { get; set; }

        public bool InitialStatus { get; set; }

        public bool ClosingStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MonthClosingDate { get; set; }

        public bool MonthClosingStatus { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }
        public int OrgID { get; set; }
        public virtual Organization Organization { get; set; }
    }
}

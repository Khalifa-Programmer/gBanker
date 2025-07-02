namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SmsLog")]
    public partial class SmsLog
    {
        public long SmsLogID { get; set; }

        public int OrgID { get; set; }

        public long MemberID { get; set; }

        [StringLength(5)]
        public string SmsType { get; set; }

        [StringLength(100)]
        public string SmsFrom { get; set; }

        [StringLength(100)]
        public string SmsTo { get; set; }

        [StringLength(500)]
        public string SmsBody { get; set; }

        public DateTime? DateSent { get; set; }

        [StringLength(10)]
        public string SmsStatus { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}

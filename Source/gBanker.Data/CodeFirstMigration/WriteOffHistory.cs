namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WriteOffHistory")]
    public partial class WriteOffHistory
    {
        public long WriteOffHistoryID { get; set; }

        [StringLength(20)]
        public string OldMemberCode { get; set; }

        [StringLength(20)]
        public string OldMemberCodeOld { get; set; }

        [StringLength(100)]
        public string OldMemberName { get; set; }

        public int? OfficeID { get; set; }

        public int? CenterID { get; set; }

        [StringLength(50)]
        public string FatherName { get; set; }

        [StringLength(50)]
        public string SpouseName { get; set; }

        [StringLength(50)]
        public string MotherName { get; set; }

        [StringLength(35)]
        public string PhoneNo { get; set; }

        [StringLength(20)]
        public string NationalID { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        public DateTime? DisburseDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DisburseAmount { get; set; }

        public DateTime? WriteOffDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WriteOffAmount { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WriteOffReceovery { get; set; }

        public long? MemberID { get; set; }

        public DateTime? OpeningDate { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(15)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}

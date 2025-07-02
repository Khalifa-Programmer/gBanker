namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberCategory")]
    public partial class MemberCategory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte MemberCategoryID { get; set; }

        [Required]
        [StringLength(5)]
        public string MemberCategoryCode { get; set; }

        [StringLength(100)]
        public string CategoryName { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryShortName { get; set; }

        [StringLength(100)]
        public string CategoryNameBng { get; set; }

        [StringLength(50)]
        public string CategoryShortNameBng { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public int OrgID { get; set; }
        [Column(TypeName = "numeric")]
        public decimal? AdmissionFee { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PassBookFee { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LoanFormFee { get; set; }
        public virtual Organization Organization { get; set; }
    }
}

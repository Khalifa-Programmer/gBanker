namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Branch")]
    public partial class Branch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BranchID { get; set; }

        [Required]
        [StringLength(10)]
        public string BranchCode { get; set; }

        [Required]
        [StringLength(35)]
        public string BranchName { get; set; }

        [StringLength(50)]
        public string BranchNameBng { get; set; }

        public int? AreaID { get; set; }

        [Column(TypeName = "date")]
        public DateTime OperationStartDate { get; set; }

        public int? GeoLocationID { get; set; }

        [StringLength(155)]
        public string BranchAddress { get; set; }

        [StringLength(15)]
        public string PostCode { get; set; }

        [StringLength(35)]
        public string Email { get; set; }

        [StringLength(35)]
        public string Phone { get; set; }

        public int? InvestorID { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual GeoLocation GeoLocation { get; set; }
    }
}

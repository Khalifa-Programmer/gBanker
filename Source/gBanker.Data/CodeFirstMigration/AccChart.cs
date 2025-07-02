namespace gBanker.Data.CodeFirstMigration
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccChart")]
    public partial class AccChart
    {
        public AccChart()
        {
            AccTrxDetails = new HashSet<AccTrxDetail>();
        }

        [Key]
        public int AccID { get; set; }

        [Required]
        [StringLength(50)]
        public string AccCode { get; set; }

        [StringLength(100)]
        public string AccName { get; set; }

        public int? AccLevel { get; set; }

        public string FirstLevel { get; set; }

        public string SecondLevel { get; set; }

        public string ThirdLevel { get; set; }

        public string FourthLevel { get; set; }

        public string FifthLevel { get; set; }

        public int? CategoryID { get; set; }

        public int? OfficeLevel { get; set; }

        public bool? IsTransaction { get; set; }
        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [StringLength(2)]
        public string Nature { get; set; }
        public int? ModuleID { get; set; }
        public int? NoteID { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public int OrgID { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual AccCategory AccCategory { get; set; }
        public virtual AccNote AccNote { get; set; }

        public virtual ICollection<AccTrxDetail> AccTrxDetails { get; set; }
    }
}

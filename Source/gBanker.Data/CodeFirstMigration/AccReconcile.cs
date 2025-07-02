namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccReconcile")]
    public partial class AccReconcile
    {
        [Key]
        public long AccReconcileID { get; set;}

        public long TrxMasterID { get; set; }

        [Column(TypeName = "date")]
        public DateTime TrxDate { get; set; }

        public int SenderOfficeId { get; set; }

        public int ReceiverOfficeId { get; set; }

        public string ReffNo { get; set; }

        [StringLength(100)]
        public string Purpose { get; set; }

        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public bool? IsReconcile { get; set; }               
        public int? OrgID { get; set; }
        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        // mahfuz new add 
        public long? Receiver_TrxMasterID { get; set; }
        public long? HO_TrxMasterID { get; set; }

    }
}

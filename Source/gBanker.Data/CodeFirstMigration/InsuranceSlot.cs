namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InsuranceSlot")]
    public partial class InsuranceSlot
    {
        public int InsuranceSlotID { get; set; }
        public int Duration { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }

        public decimal InsuranceRate { get; set; }

        public decimal AmountBy { get; set; }

        public DateTime InsuarnceDate { get; set; }

        public bool? IsRunning { get; set; }

        public int ProductID { get; set; }

        public string PaymentFrequency { get; set; }
        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }
        [Column(TypeName = "smalldatetime")]
        public string CreateDate { get; set; }
         
    }// End Class
}// End namespace

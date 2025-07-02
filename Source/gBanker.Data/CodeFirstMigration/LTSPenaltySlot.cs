namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LTSPenaltySlot")]
    public partial class LTSPenaltySlot
    {
        public int LTSPenaltySlotID { get; set; }
        public string PaymentFrequency { get; set; }
        public decimal? Duration { get; set; }
        [Display(Name = "Term Deposit")]
        public decimal? TermDeposit { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? Penalty { get; set; }
        

    }// End Class
}// End namespace



// class LTSPenaltySlot

namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InterestRate")]
    public partial class InterestRate
    {
        public int InterestRateID { get; set; }
        public decimal? Duration { get; set; }
        [Display(Name = "Insurance Rate")]
        public decimal? InterestRates { get; set; }
        
        public DateTime? EffectDate { get; set; }
        public decimal? EffextYear { get; set; }
        public int? EStatus { get; set; }
  
    }// End Class
}// End namespace

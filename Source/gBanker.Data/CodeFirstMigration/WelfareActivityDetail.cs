using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("WelfareActivityDetail")]
    public class WelfareActivityDetail
    {
        [Key]
        public int WelfareActivityId { get; set; }
        public int? OfficeId { get; set; }
        public DateTime? DateTo { get; set; }
        public int? ActivityId { get; set; }        
        public decimal? SurplusMicrofinance { get; set; }
        public decimal? SurplusOtherActivities { get; set; }
        public decimal? SurplusOwnFund { get; set; }
        public decimal? Donation { get; set; }
        public decimal? OtherSource { get; set; }
        public int? AreaCovered { get; set; }
        public int? NumberOfBeneficiaries { get; set; }
        public int? DurationOfActivity { get; set; }
        public decimal? AcitivityExpenditure { get; set; }
        public decimal? Surplus { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateUser { get; set; }
    }
}

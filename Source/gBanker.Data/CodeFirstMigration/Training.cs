using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("Training")]
    public class Training
    {
        [Key]
        public int TrainingID { get; set; }
        public int? OrgId { get; set; }
        public int? OfficeID { get; set; }
        public string TrainingType { get; set; }
        public DateTime? TrainingDate { get; set; }
        public int? NoOfParticipants { get; set; }
        public string CourseName { get; set; }
        public decimal? CostGeneralFund { get; set; }
        public decimal? CostMicroFinance { get; set; }
        public decimal? CostDonation { get; set; }
        public string OtherCostSource1 { get; set; }
        public decimal? CostAmount1 { get; set; }
        public string OtherCostSource2 { get; set; }
        public decimal? CostAmount2 { get; set; }
        public string OtherCostSource3 { get; set; }
        public decimal? CostAmount3 { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}

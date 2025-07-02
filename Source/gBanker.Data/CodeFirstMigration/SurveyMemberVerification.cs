using System.Web;

namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Data.Entity;

    [Table("SurveyMemberVerification")]
    public partial class SurveyMemberVerification
    {
        [Key]
        public long SMVerificationId { get; set; }
        public long SurveyId { get; set; }
        public string VarificationNo { get; set; }
        public int VarificationTypeId { get; set; }

        [Display(Name = "Varification Scan Copy")]
        public byte[] VarificationDocument { get; set; }
        public string Remarks { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}


namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SMS_SentLog")]
    public partial class SMS_SentLog
    {
        [Key]
        public Int64 SMS_SentLogID { get; set; }

        public Int64? SummaryId { get; set; }

        [StringLength(1000, ErrorMessage = "Maximum length is {1}")]
        public string GrettingsText { get; set; }

        public DateTime? DateSent { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string PhoneNo { get; set; }

        public int? SMSType1 { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string SMSType { get; set; }

        [StringLength(550, ErrorMessage = "Maximum length is {1}")]
        public string SMSDETAIL { get; set; }

        public decimal? SMSPrice { get; set; }

        public int? Length { get; set; }

        public int? OfficeId { get; set; }

        public int? SMSCount { get; set; }

        public Int64? recordId { get; set; }

        public int? OrgId { get; set; }

        public int? SentToAPI { get; set; }

        public int? SentOK { get; set; }

        public bool? isActive { get; set; }
    }
}

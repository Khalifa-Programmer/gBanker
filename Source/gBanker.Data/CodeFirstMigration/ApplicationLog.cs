using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("ApplicationLog")]
    public partial class ApplicationLog
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ApplicationLogId { get; set; }

        [StringLength(1000)]
        public string ActionURL { get; set; }

        public DateTime? LogDate { get; set; }

        [StringLength(50)]
        public string ClientIP { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string RequestUser { get; set; }

        public string RequestDetail { get; set; }

        public string ErrorDetail { get; set; }

        [StringLength(200)]
        public string UserAgent { get; set; }
         [StringLength(500)]
        public string QueryStringParams { get; set; }
         [StringLength(100)]
         public string ControllerName { get; set; }
         [StringLength(100)]
         public string ActionName { get; set; }
         [StringLength(10)]
         public string HttpMethod { get; set; }
        [StringLength(50)]
         public string SessionId { get; set; }
        //ControllerName
        public string OrganizationId { get; set; }
        [NotMapped]
        public string OfficeCode { get; set; }
        public int? OfficeID { get; set; }
    }
}

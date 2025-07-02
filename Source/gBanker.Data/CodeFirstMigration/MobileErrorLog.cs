using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class MobileErrorLog
    {
        public long ID { get; set; }

        public DateTime LogDate { get; set; }

        [Required]
        [StringLength(50)]
        public string UserID { get; set; }

        [StringLength(100)]
        public string ActionName { get; set; }

        public string InputParams { get; set; }

        public string ErrorDetail { get; set; }

        [StringLength(10)]
        public string ErrorType { get; set; }

        [StringLength(100)]
        public string PhoneModel { get; set; }

        [StringLength(50)]
        public string OSVersion { get; set; }

        [StringLength(50)]
        public string PhoneBuildNumber { get; set; }

        [StringLength(20)]
        public string GBAppVersion { get; set; }

        public DateTime CreateDate { get; set; }
    }
}

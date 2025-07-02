using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.AssetTransfer")]
    public class AssetTransfer
    {
        [Key]
        public long TransferID { get; set; }
        public int AssetGroupID { get; set; }
        public long AssetID { get; set; }
        public string AssetSerialOld { get; set; }
        public string AssetSerialNew { get; set; }
        public int OfficeFrom { get; set; }
        public int OfficeTo { get; set; }
        public long DailyTransactionIdOld { get; set; }
        public long DailyTransactionIdNew { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int OrgID { get; set; }
        public string Remarks { get; set; }
        public string AuthorisedBy { get; set; }
        public bool IsActive { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }           
    }
}

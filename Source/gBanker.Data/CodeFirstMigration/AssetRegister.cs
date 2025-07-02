using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.AssetRegister")]
    public class AssetRegister
    {
        [Key]
        public long AssetRegisterID { get; set; }        
        public long? AssetInfoID { get; set; }
        public string AssetSerial { get; set; }
        public string VoucherNo { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TransactionValue { get; set; }
        public decimal? Depriciation { get; set; }
        public string TranType { get; set; }
        public long? AssetClientId { get; set; }
        public string Remarks { get; set; }
        public int? OrgID { get; set; }
        public int? OfficeID { get; set; }        
        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? AssetUpdateID { get; set; }
    }
}

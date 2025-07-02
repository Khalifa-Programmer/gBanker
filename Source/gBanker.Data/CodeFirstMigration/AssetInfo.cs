using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.AssetInfo")]
    public class AssetInfo
    {
        [Key]
        public long AssetID { get; set; }
        public string AssetCode { get; set; }
        public int AssetGroupID { get; set; }
        public string AssetName { get; set; }
        public bool Depritiable { get; set; }
        public decimal Deprate { get; set; }
        public int DepriciationMethod { get; set; }
        public int OrgID { get; set; }
        //public int OfficeID { get; set; }
        public bool IsActive { get; set; }        
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

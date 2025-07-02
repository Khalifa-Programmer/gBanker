using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.LastAssetCodeInfo")]
    public class LastAssetCodeInfo
    {
        [Key]
        public int Id { get; set; }
        public string LastAssetCode { get; set; }
        public int? OfficeID { get; set; }
        public string AssetGroupCode { get; set; }
        public string AssetCode { get; set; }
        public bool? IsActive { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

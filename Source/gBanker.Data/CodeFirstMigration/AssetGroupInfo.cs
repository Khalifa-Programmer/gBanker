using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.AssetGroupInfo")]
    public class AssetGroupInfo
    {
        [Key]
        public int AssetGroupID { get; set; }
        public string AssetGroupCode { get; set; }
        public string AssetGroupName { get; set; }
        public string AssetInAccCode { get; set; }       
        public string AssetCurDepriDr { get; set; }
        public string AssetAccuDepriCr { get; set; }
        //public int OfficeID { get; set; }
        public int OrgID { get; set; }       
        public bool IsActive { get; set; }        
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}

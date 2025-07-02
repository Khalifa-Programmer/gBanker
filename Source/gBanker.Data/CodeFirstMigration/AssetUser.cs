using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.AssetUser")]
    public class AssetUser
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("inv.Inv_CategoryOrSubCategory")]
    public class Inv_CategoryOrSubCategory
    {
        [Key]
        public int CategorySubCategoryID { get; set; }
        public int? ParentCategoryID { get; set; }
        public string CateorSubCateCode { get; set; }
        public string CategorySubCategoryName { get; set; }
        public string NameInBangla { get; set; }
        public bool IsActive { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

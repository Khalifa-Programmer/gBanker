using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("inv.Inv_ItemPriceDetails")]
    public class Inv_ItemPriceDetails
    {
        [Key]
        public int ItemPriceSetID { get; set; }
        public int ItemID { get; set; }
        public int? OfficeID { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int? ChangeBy { get; set; }
        public DateTime? ChangeDate { get; set; }

    }
}

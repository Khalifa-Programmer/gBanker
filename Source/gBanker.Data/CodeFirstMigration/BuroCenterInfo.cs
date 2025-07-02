using gBankerCodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("BuroCenterInfo")]
    public class BuroCenterInfo
    {
        [Key]
        public int ID { get; set; }
        public int Sl { get; set; }
        public string BranchCode { get; set; }
        public string CenterType { get; set; }
        public string CenterID { get; set; }
        public string CenterName { get; set; }
        public DateTime? CenterOpeningDate { get; set; }
        public string CenterVillage { get; set; }
        public string CenterPO { get; set; }
        public string CenterUnion { get; set; }
        public string CenterThana { get; set; }
        public string CenterDistrict { get; set; }
        public int? CenterTime { get; set; }
        public string CenterDay { get; set; }
        public string PO_APO { get; set; }
        public string BA_ABA { get; set; }
        public string BM_ABM { get; set; }
        public string CenterChief { get; set; }
        public string AssCenterChief { get; set; }
        public string CenterLocation { get; set; }

    }
}

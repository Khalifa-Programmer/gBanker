namespace gBanker.Data.CodeFirstMigration
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("inv.Inv_ConsolidateDisposeRequest")]
    public partial class Inv_ConsolidateDisposeRequest
    {
        [Key]
        public int ConsolidateDisposeID { get; set; }
        public int ItemID { get; set; }
        public int Qty { get; set; }
        public DateTime ConsolidateDate { get; set; }
        public int ConsolidateOfficeID { get; set; }
        public long? ConsolidateBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? ApprovedOfficeID { get; set; }
        public long? ApprovedBy { get; set; }
        public bool IsActive { get; set; }
    }

}


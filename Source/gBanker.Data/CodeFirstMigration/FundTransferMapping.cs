namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FundTransferMapping")]
    public partial class FundTransferMapping
    {
        [Key]
        public Int64 FundTransferMappingId { get; set; }
        public int EmployeeId { get; set; }
        public int OfficeId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

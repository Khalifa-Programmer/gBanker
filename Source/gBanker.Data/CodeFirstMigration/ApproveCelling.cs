namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    [Table("ApproveCelling")]
    public partial class ApproveCelling
    {
        public long ApproveCellingID { get; set; }

        public int? RoleID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MinRange { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? MaxRange { get; set; }

        public int? ProdType { get; set; }

        [StringLength(150)]
        public string RoleName { get; set; }

        public int? ProductId { get; set; }
    }
}

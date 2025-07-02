namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberLastCode")]
    public partial class MemberLastCode
    {
        [Key]
        public int LastCodeID { get; set; }

        public int OfficeID { get; set; }

        public int GroupID { get; set; }

        [StringLength(30)]
        public string LastCode { get; set; }
        public int OrgID { get; set; }
        public virtual Organization Organization { get; set; }
    }
}

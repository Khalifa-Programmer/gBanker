namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Group")]
    public partial class Group
    {
        public Group()
        {
            Members = new HashSet<Member>();
        }

        public short GroupID { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupCode { get; set; }

        public int OfficeID { get; set; }

        [Column(TypeName = "date")]
        public DateTime FormationDate { get; set; }

        public byte GroupStatus { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public int OrgID { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Office Office { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }
}

namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GeoLocation")]
    public partial class GeoLocation
    {
        public GeoLocation()
        {
            Branches = new HashSet<Branch>();
            Centers = new HashSet<Center>();
            Offices = new HashSet<Office>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GeoLocationID { get; set; }

        [Required]
        [StringLength(65)]
        public string LocationName { get; set; }

        public byte LocationLevel { get; set; }

        [Required]
        [StringLength(10)]
        public string FirstLevel { get; set; }

        [StringLength(10)]
        public string SecondLevel { get; set; }

        [StringLength(10)]
        public string ThirdLevel { get; set; }

        [StringLength(10)]
        public string FourthLevel { get; set; }

        [StringLength(10)]
        public string FifthLevel { get; set; }

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
        public virtual ICollection<Branch> Branches { get; set; }

        public virtual ICollection<Center> Centers { get; set; }

        public virtual ICollection<Office> Offices { get; set; }
    }
}

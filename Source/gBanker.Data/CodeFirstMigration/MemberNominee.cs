namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberNominee")]
    public partial class MemberNominee
    {
        public long MemberNomineeId { get; set; }

        public long MemberId { get; set; }

        [StringLength(250)]
        public string NomineeName { get; set; }
        public string NomineeFather { get; set; }
        public string NomineeMother { get; set; }
        public string NomineeHusbandWife { get; set; }
        public DateTime? NomineeBirthdate { get; set; }
        public string NomineeMobileNo { get; set; }

        [StringLength(50)]
        public string NomineeRelation { get; set; }

        public int? IdType { get; set; }

        [StringLength(50)]
        public string NomineeNationalId { get; set; }


        
        public int? CountryID { get; set; }
        public string DivisionCode { get; set; }
        public string DistrictCode { get; set; }
        public string UpozillaCode { get; set; }
        public string UnionCode { get; set; }
        public string VillageCode { get; set; }
        public string ZipCode { get; set; }
        public string AddressLine1 { get; set; }
        public string NomineeImagePath { get; set; }

        public string NomineeSignaturePath { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(15)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        public string Gender { get; set; }
    }
}

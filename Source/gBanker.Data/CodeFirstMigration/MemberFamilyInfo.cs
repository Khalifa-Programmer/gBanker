namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberFamilyInfo")]
    public partial class MemberFamilyInfo
    {
        [Key]
        public long MemberFamilyID { get; set; }

        public int OrgID { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        public long MemberID { get; set; }

        [StringLength(30)]
        public string FamilyMemName { get; set; }

        public int? FamilyMemAge { get; set; }

        [StringLength(1)]
        public string FamilyMemGender { get; set; }

        [StringLength(10)]
        public string FamilyMemRelationship { get; set; }

        [StringLength(20)]
        public string FamilyMemOccupation { get; set; }

        public bool? LetterWritingAbility { get; set; }

        public bool? AddressWritingAbility { get; set; }

        public bool? FinishedClassFive { get; set; }

        public bool? DropBeforeClassFive { get; set; }

        public bool? Studying { get; set; }

        public bool? SignatureAbility { get; set; }
        public string NationalIdNo { get; set; }
        public string MobileNo { get; set; }
        public string MaritalStatus { get; set; }
        public string FamilyMemOccupation2 { get; set; }
        public bool? PhysicalDisability { get; set; }

        public string SocialSecurity { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}

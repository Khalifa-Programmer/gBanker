using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class MemberDetailViewModel : BaseModel
    {
        public long MemberFamilyID { get; set; }

        public int OrgID { get; set; }

        public int OfficeID { get; set; }

        [Display(Name = "Samity Name")]
        public int CenterID { get; set; }

        public long MemberID { get; set; }

        [Display(Name = "Family Member Name")]
        public string FamilyMemName { get; set; }

        [Display(Name = "Age")]
        public int? FamilyMemAge { get; set; }

        [Display(Name = "Gender")]
        public string FamilyMemGender { get; set; }

        [Display(Name = "Relationship")]
        public string FamilyMemRelationship { get; set; }

        [Display(Name = "Occupation")]
        public string FamilyMemOccupation { get; set; }

        public bool? LetterWritingAbility { get; set; }

        public bool? AddressWritingAbility { get; set; }

        public bool? FinishedClassFive { get; set; }

        public bool? DropBeforeClassFive { get; set; }

        public bool? Studying { get; set; }

        public bool? SignatureAbility { get; set; }
        [Display(Name = "NationalId No")]
        public string NationalIdNo { get; set; }
        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        [Display(Name = "Second Occupation")]
        public string FamilyMemOccupation2 { get; set; }
        [Display(Name = "Physical Disability")]
        public bool? PhysicalDisability { get; set; }
        [Display(Name = "Non-resident/Social Security")]
        public string SocialSecurity { get; set; }
        public IEnumerable<SelectListItem> CenterList { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public IEnumerable<SelectListItem> FamilyMemRelationshipList { get; set; }
        public IEnumerable<SelectListItem> MaritalStatusList { get; set; }



        public string FamilyMemNames { get; set; }
        public int? FamilyMemAges { get; set; }
        public string FamilyMemGenders { get; set; }
        public string FamilyMemRelationships { get; set; }
        public string FamilyMemOccupations { get; set; }
    }
}
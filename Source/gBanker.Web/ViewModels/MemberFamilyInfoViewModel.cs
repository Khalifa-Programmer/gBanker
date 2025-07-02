using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace gBanker.Web.ViewModels
{
    public class MemberFamilyInfoViewModel : BaseModel
    {
        public long MemberFamilyID { get; set; }

        public int OrgID { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        public long MemberID { get; set; }

        [Display(Name = "Member Name")]
        public string FamilyMemName { get; set; }

        public int? FamilyMemAge { get; set; }
        
        public string FamilyMemGender { get; set; }
       
        public string FamilyMemRelationship { get; set; }
        
        public string FamilyMemOccupation { get; set; }

        public bool? LetterWritingAbility { get; set; }

        public bool? AddressWritingAbility { get; set; }

        public bool? FinishedClassFive { get; set; }

        public bool? DropBeforeClassFive { get; set; }

        public bool? Studying { get; set; }

        public bool? SignatureAbility { get; set; }
        
    }
}
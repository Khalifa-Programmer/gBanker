using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class MemberPassBookRegisterViewModel : BaseModel
    {
        public long MemberPassBookRegisterID { get; set; }
         [Display(Name = "Member PassBook NO.")]

        public long MemberPassBookNO { get; set; }

        public int AccID { get; set; }
        public string AccName { get; set; }
        public string CashBook { get; set; }
        public string AccCode { get; set; }
        public int AccLevel { get; set; }


        public long MemberID { get; set; }

     
      
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OfficeID { get; set; }
         [Display(Name = "Office Code")]
        public string OfficeCode { get; set; }
         [Display(Name = "Office Name")]
        public string OfficeName { get; set; }
         [Display(Name = "Member Code")]
        public string MemberCode { get; set; }
         [Display(Name = "Member Name")]
        public string MemberName { get; set; }


        public int CenterID { get; set; }
         [Display(Name = "Center Code")]
        public string CenterCode { get; set; }
         [Display(Name = "Center Name")]
        public string CenterName { get; set; }
        public int OrgID { get; set; }

       [GlobalizedDisplayName("PassBookStartDate")]
        public DateTime? PassBookStartDate { get; set; }

       
        public DateTime? PassBookCloseDate { get; set; }
        public string PassBookStartDateMSG { get; set; }
        public string PassBookCloseDateMSG { get; set; }
        public int Status { get; set; }
         [Display(Name = "Lot No.")]
        public long? LotNo { get; set; }
        
  
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public IEnumerable<SelectListItem> LotListItems { get; set; }
        public IEnumerable<SelectListItem> StatusListItems { get; set; }
        public string memName { get; set; }
    }
}
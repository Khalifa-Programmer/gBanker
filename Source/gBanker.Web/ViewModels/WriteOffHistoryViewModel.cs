using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class WriteOffHistoryViewModel
    {
        public long WriteOffHistoryID { get; set; }
        public string OldMemberCode { get; set; }
        public string OldMemberName { get; set; }
        public string OldMemberCodeOld { get; set; }
        public int? OfficeID { get; set; }
        //public int? CenterID { get; set; }
        public string FatherName { get; set; }
        public string SpouseName { get; set; }
        public string MotherName { get; set; }
        public string PhoneNo { get; set; }
        public string NationalID { get; set; }
        public string Address { get; set; }
        public DateTime? DisburseDate { get; set; }
        public decimal? DisburseAmount { get; set; }
        public DateTime? WriteOffDate { get; set; }
        public decimal? WriteOffAmount { get; set; }
        public decimal? WriteOffReceovery { get; set; }
        public long? MemberID { get; set; }
        public DateTime? OpeningDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }


        public string WD { get; set; }
        public string DD { get; set; }
        public string OD { get; set; }


        // center dropdown
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public string centercodename { get; set; }
        public IEnumerable<SelectListItem> CenterList { get; set; }
        public string OldMemberCodeOldSerch { get; set; }

        public decimal WriteOffBalance { get; set; }
        

    }
}
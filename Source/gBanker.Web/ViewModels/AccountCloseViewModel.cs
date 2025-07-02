using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class AccountCloseViewModel
    {
        public long SavingSummaryID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public long MemberID { get; set; }


        public string MemberCode { get; set; }


        public string MemberName { get; set; }

        public short ProductID { get; set; }


        public string ProductCode { get; set; }


        public string ProductName { get; set; }

        public int CenterID { get; set; }
        public string CenterCode { get; set; }

        public int NoOfAccount { get; set; }


        public DateTime TransactionDate { get; set; }

        public decimal Balance { get; set; }


     
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> AccountListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        
        public IEnumerable<SelectListItem> memberListItems { get; set; }
       
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class CategoryTransferViewModel : BaseModel
    {
        public long TransferHistoryID { get; set; }
        public Nullable<int> OfficeID { get; set; }
        public string OfficeCode { get; set; }
         [Display(Name = "Samity ID")]
        public Nullable<int> CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
        public Nullable<long> MemberID { get; set; }
        public string MemberCode { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string ProductCode { get; set; }
        public Nullable<int> TrProductID { get; set; }
        public string TrProductCode { get; set; }
        public string Status { get; set; }
        [Display(Name = "Saving Balance")]
        public Nullable<decimal> Principal { get; set; }
        public Nullable<int> MemberCategoryId { get; set; }
        public Nullable<int> TrMemberCategoryID { get; set; }
        public string MemberCategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string trProductName { get; set; }
        public string ProductName { get; set; }
        public string TrMemberCategoryCode { get; set; }
        public string TrCategoryName { get; set; }
    
        public IEnumerable<SelectListItem> productListItems { get; set; }

        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> membercategoryListItems { get; set; }
        public IEnumerable<SelectListItem> productList { get; set; }
        public IEnumerable<SelectListItem> categoryList { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public string memName { get; set; }
        public string vMaxLoanTerm { get; set; }
        public IEnumerable<ProductViewModel> MemberProductItemsSelected { get; set; }
    }
}
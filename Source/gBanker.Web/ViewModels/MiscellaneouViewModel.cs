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
    public class MiscellaneouViewModel : BaseModel
    {
        public long MiscellaneousID { get; set; }
        public long MemberID { get; set; }
        [GlobalizedDisplayName("OfficeID")]
        [Required(ErrorMessage = "Office Code is required")]
        public int? OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string ProductCode { get; set; }
        public string CenterCode { get; set; }
        public string MemberName { get; set; }
        [GlobalizedDisplayName("CenterID")]
        [Required(ErrorMessage = "Samity is required")]
        public int? CenterID { get; set; }
        [GlobalizedDisplayName("ProductID")]
        [Required(ErrorMessage = "Product is required")]
        public short? ProductID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Amount { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public string Remarks { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TrxDate { get; set; }

        public long? rowSl { get; set; }
        public string ServiceMessage { get; set; }

        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        //public long MiscellaneousID { get; set; }
        //public long MemberID { get; set; }
        //[GlobalizedDisplayName("OfficeID")]
        //[Required(ErrorMessage = "Office Code is required")]
        //public int? OfficeID { get; set; }
        //public string OfficeCode { get; set; }
        //public string ProductCode { get; set; }
        //public string CenterCode { get; set; }
        //public string MemberName { get; set; }
        //[GlobalizedDisplayName("CenterID")]
        //[Required(ErrorMessage = "Samity is required")]
        //public int? CenterID { get; set; }
        //[GlobalizedDisplayName("ProductID")]
        //[Required(ErrorMessage = "Product is required")]
        //public short? ProductID { get; set; }

        //[Column(TypeName = "numeric")]
        //public decimal? Amount { get; set; }
        //[Required(ErrorMessage = "Date is required")]
        //public string Remarks { get; set; }
        //[Column(TypeName = "date")]
        //public DateTime? TrxDate { get; set; }

        //public long? rowSl { get; set; }
        //public int ServiceMessageID { get; set; }
        //public string ServiceMessage { get; set; }
        //public string FromDate { get; set; }
        //public string ToDate { get; set; }
        //public Boolean isActive { get; set; }
        //public Boolean? isOnline { get; set; }

        //public IEnumerable<SelectListItem> productListItems { get; set; }
        //public IEnumerable<SelectListItem> centerListItems { get; set; }
        //public IEnumerable<SelectListItem> officeListItems { get; set; }
    }
}
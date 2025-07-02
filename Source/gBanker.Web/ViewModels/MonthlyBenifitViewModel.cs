using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class MonthlyBenifitViewModel
    {
        [Display(Name = "Product")]
        public int ProductID{ get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<SelectListItem> IndividualProductList { get; set; }
    }
}
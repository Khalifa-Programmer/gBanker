using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class CenterCategoryViewModel
    { 
        [Display(Name = "Center Category Id")]
        public int CenterCategoryID { get; set; }

        [Display(Name = "Samity Code")]
        public string CenterCategoryName { get; set; }

    }
}
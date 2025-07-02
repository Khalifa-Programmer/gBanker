using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    //New
    public class ProductXEmploymentProductMappingViewModel : BaseModel
    {
        public int MappingId { get; set; }
        [Display(Name = "Main Product Code")]
        public string MainProductCode { get; set; }
        [Display(Name = "Main Product Name")]
        public string MainProductName { get; set; }
        [Display(Name = "Employment Product Name")]
        public string EmploymentProductName { get; set; }
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }
    
        
    }
}
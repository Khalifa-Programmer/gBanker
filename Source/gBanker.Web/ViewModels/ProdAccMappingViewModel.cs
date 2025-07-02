using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class ProdAccMappingViewModel:BaseModel
    {
        [Display(Name = "Select Office")]
        public int OfficeID { get; set; }
        public int SelectedOfficeID { get; set; }


        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string LoanComponent { get; set; }

        public IEnumerable<SelectListItem> OfficeList { get; set; }


        public IEnumerable<SelectListItem> ProductList { get; set; }

        public IEnumerable<SelectListItem> ProductListComponent { get; set; }

        public string ProductCodeComponent { get; set; }
        public string ProductNameComponent { get; set; }
        public long rowSl { get; set; }
        public int MappingId { get; set; }



    }// End Class
}// END Namespace
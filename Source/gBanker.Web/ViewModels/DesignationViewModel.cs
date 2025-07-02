using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class DesignationViewModel
    {

        public long rowSl { get; set; }
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }

        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public List<SelectListItem> Department { get; set; }
        public int DesignationTypeID { get; set; }
        public string DesignationTypeName { get; set; }
        public List<SelectListItem> DesignationType { get; set; }

    } // END CLASS
}// END Namespace
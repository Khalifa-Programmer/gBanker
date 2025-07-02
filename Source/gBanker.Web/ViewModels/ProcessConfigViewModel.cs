using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class ProcessConfigViewModel : BaseModel
    {
        public int Id { get; set; }
        public int startWorkProcessID { get; set; }
        public int completeWorkProcessID { get; set; }
        public int DateID { get; set; }

        [Display(Name = "Process Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ProcessDate { get; set; }


        [Display(Name = "Start Process On")]
        [DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]    //{0:t}
        public string startWorkProcess { get; set; }

        [Display(Name = "Complete Work Process On")]
        [DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]    //{0:t}
        public string EndWorkProcess { get; set; }

        public DateTime startWorkProcessDT { get; set; }
        public DateTime EndWorkProcessDT { get; set; }
        //public DateTime ProcessDate { get; set; }

        public bool KActive { get; set; }

        public int OfficeID { get; set; }
        public DateTime ClosingDate { get; set; }
        public int OrgID { get; set; }

    }//End of Class
}// End of Namespace
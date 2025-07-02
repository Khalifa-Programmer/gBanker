using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gBanker.Web.ViewModels
{
    public class GurantorViewModel
    {
        public Int64 MemberId { get; set; }
        public Int64 GuarantorId { get; set; }
        public string GuarantorName { get; set; }
        public long rowSl { get; set; }
        public string FatherName { get; set; }
        public string Relation { get; set; }
        public string DateOfBirth { get; set; }
        public string AgeDetails { get; set; }
        public string Address { get; set; }


    }//END Class
}// END Namespace
 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class DayEndViewModel :BaseModel
    {
        public int? OfficeId { get; set; }
        
        public DateTime? BusinessDate { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }

        public int LoginUserOfficeID { get; set; }
        public int LoginUserOrganizationID { get; set; }
        public string LoggedInOrganizationCode { get; set; }
        public string SendAutoSMS { get; set; }
        public string ApiBaseUrl { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class AreaViewModel:BaseModel
    {
        public int AreaID { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public IEnumerable<SelectListItem> ZoneList { get; set; }
        //public Nullable<bool> IsActive { get; set; }
        //public Nullable<System.DateTime> InActiveDate { get; set; }
        //public string CreateUser { get; set; }
        //public System.DateTime CreateDate { get; set; }
    }
}
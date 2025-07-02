using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class BaseModel
    {
        public string CreateUser
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                    return HttpContext.Current.User.Identity.Name;
                else
                    return "SYSTEM";
            }
        }
        public DateTime CreateDate { get { return DateTime.Now; } }


        public Nullable<bool> IsActive { get; set; }
        //public Nullable<System.DateTime> InActiveDate { get; set; }
        public Nullable<DateTime> InActiveDate { get; set; }

       // public Nullable<bool> IsActive { get { return IsActive.HasValue ? IsActive : true; } set; }
        //public Nullable<System.DateTime> InActiveDate { get; set { InActiveDate = IsActive.HasValue ? (IsActive.Value == false ? DateTime.Now : default(DateTime?)) : default(DateTime?); } }
    }
}
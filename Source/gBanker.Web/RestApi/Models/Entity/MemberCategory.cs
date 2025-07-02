using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class MemberCategory
    {
        public byte MemberCategoryID { get; set; }
        public string MemberCategoryCode { get; set; }
        public string CategoryName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class Group
    {
        public short GroupID { get; set; }
        public string GroupCode { get; set; }
        public int OfficeID { get; set; }
    }
}
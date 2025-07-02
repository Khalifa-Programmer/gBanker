using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class LastMemberCode
    {
        public int LastCodeID { get; set; }
        public int OfficeID { get; set; }
        public int GroupID { get; set; }
        public string LastCode { get; set; }
        public int OrgID { get; set; }
    }
}
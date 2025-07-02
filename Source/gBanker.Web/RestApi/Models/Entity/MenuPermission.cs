using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class MenuPermission
    {
        public int id { get; set; }
        public string RoleId { get; set; }
        public string alias { get; set; }
        public bool isactive { get; set; }
        public bool view { get; set; }
        public bool create { get; set; }
        public bool delete { get; set; }
        public bool approve { get; set; }
        public bool disburse { get; set; }

    }
}
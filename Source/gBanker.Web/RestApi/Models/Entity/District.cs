using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class District
    {
        public string DivisionCode { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
    }
}
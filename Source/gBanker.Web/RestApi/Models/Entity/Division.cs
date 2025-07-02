using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class Division
    {
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public int CountryID { get; set; }
    }
}
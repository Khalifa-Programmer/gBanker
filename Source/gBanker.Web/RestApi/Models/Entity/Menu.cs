using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class Menu
    {
        public int id;
        public string menu_name { get; set; }
        public string alias { get; set; }
        public string groups { get; set; }
        public int display_order { get; set; }
        public bool is_active { get; set; }
    }
}
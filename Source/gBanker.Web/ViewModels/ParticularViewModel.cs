using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class ParticularViewModel
    {
        internal List<SelectListItem> ParticularList;

        public int ParticularId { get; set; }
        public string ParticularName { get; set; }
    }
}
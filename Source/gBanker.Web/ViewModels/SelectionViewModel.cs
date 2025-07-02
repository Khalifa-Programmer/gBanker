using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class SelectionViewModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public bool IsSelected { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class PomisProcessResponseViewModel
    {
        public string Message { get; set; }
        public string IsRunning { get; set; }
        public string IsExist { get; set; }
    }
}
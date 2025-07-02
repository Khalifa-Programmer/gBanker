using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class ServerLoginCredentialsViewModel
    {
        public string ServerIP { get; set; }
        
        public string User { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }


    }// END Class
}// END Namespace
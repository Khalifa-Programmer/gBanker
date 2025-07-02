using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.SMSSenderServices.Models
{
    public class SMSSendRequest
    {
        public string RecipientMobile { get; set; }
        public string Message { get; set; }
        public string RequestType { get; set; }
        public string MessageType { get; set; }

        public string Organization { get; set; }
    }
}

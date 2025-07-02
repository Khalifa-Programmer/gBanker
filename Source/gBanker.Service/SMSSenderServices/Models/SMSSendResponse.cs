using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.SMSSenderServices.Models
{
    public class SMSSendResponse
    {
        public string request_type { get; set; }
        public string campaign_uid { get; set; }
        public string sms_uid { get; set; }
        public string[] invalid_numbers { get; set; }
        public string api_response_code { get; set; }
        public string api_response_message { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }

        //for GP
        public string statusCode { get; set; }
        public string message { get; set; }

    }

    public class SMSGPSendResponse
    {
        //for GP
        public string statusCode { get; set; }
        public string message { get; set; }

    }
}

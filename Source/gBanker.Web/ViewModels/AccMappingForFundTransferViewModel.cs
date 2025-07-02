using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class AccMappingForFundTransferViewModel
    {
        public int ID { get; set; }
        public string HOFundAccCode { get; set; }
        public string HOFundAccName { get; set; }
        public string BRFundAccCode { get; set; }
        public string BRFundAccName { get; set; }
    }
}
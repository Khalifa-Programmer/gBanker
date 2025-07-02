using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class BankInformationViewModel
    {
        public BankInformationViewModel() {
        
        }
        public long rowSl { get; set; }
        public long BankInfoID { get; set; }
        public int AccID { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string BankAddress { get; set; }
        public int officeId { get; set; }
        public string AccCode { get; set; }
        public string BankCode { get; set; }
        public string AccName { get; set; }


    }// END CLass
}// END Namespace
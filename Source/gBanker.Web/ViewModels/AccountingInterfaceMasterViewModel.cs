using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class AccountingInterfaceMasterViewModel: BaseModel
    {
        public int AccountingInterfaceID { get; set; }
        public long rowSl { get; set; }
        public string AccCode { get; set; }
 
        public string voucher_category { get; set; }
         
        public string voucher_type { get; set; }
         
        public string trx_type { get; set; }
         
        public string trx_ind { get; set; }

        public string office_type { get; set; }
        public string trx_ind_FullName { get; set; }
        public int OrgID { get; set; }

        public byte? ProductType { get; set; }



        public string ProductName { get; set; }
        //public string AccCode { get; set; }
        //public string voucher_category { get; set; }
        //public string voucher_type { get; set; }
    }
}
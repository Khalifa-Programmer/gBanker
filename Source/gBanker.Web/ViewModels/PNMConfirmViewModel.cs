using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class PNMConfirmViewModel:BaseModel
    {
        public int pnm_confirm_id { get; set; }
        
        public DateTime? entry_dt { get; set; }

        public decimal? due_to_site_amount { get; set; }

        public string due_to_site_currency { get; set; }

        public decimal? net_payment_amount { get; set; }

        public string net_payment_currency { get; set; }

        public string order_payee_identifier { get; set; }

        public decimal? payment_amount { get; set; }

        public string payment_currency { get; set; }

        public string payment_timestamp { get; set; }

        public DateTime? payment_timestamp_dt { get; set; }
        
        public string pnm_order_identifier { get; set; }

        public string pnm_payment_identifier { get; set; }

        public decimal? pnm_withheld_amount { get; set; }

        public string pnm_withheld_currency { get; set; }

        public string signature { get; set; }

        public string site_customer_identifier { get; set; }

        public string site_identifier { get; set; }

        public bool? standin { get; set; }

        public string status { get; set; }

        public bool? test { get; set; }

        public string timestamp { get; set; }

        public string version { get; set; }
    }
}
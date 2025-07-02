using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class PO_A_ACC_HEADModel
    {
        public string l1_code { get; set; }
        public string l2_code { get; set; }
        public string l3_code { get; set; }
        public string l4_code { get; set; }
        public string l5_code { get; set; }
        public string acchead { get; set; }
        public string acctype { get; set; }
        public string accgroup { get; set; }
        public string ins_user { get; set; }
        public DateTime? ins_date { get; set; }
        public string upd_user { get; set; }
        public DateTime? upd_date { get; set; }
        public string coa_id { get; set; }
        public string AccountType { get; set; }
        public string AccLayers { get; set; }
    }
}

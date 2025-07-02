using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class BasicDataModel
    {
        public string po_code { get; set; }
        public string mnyr { get; set; }
        public string ind_code { get; set; }
        public string m_f_flag { get; set; }
        public decimal? bd_pksf_fund { get; set; }
        public decimal? bd_non_pksf_fund { get; set; }
        public string created_by { get; set; }
    }
}

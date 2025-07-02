using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class SentLogSMSSummaryModel
    {
        public string DateSent { get; set; }
        public int SMSCount { get; set; }
        public int TotalCount { get; set; }
    }
}

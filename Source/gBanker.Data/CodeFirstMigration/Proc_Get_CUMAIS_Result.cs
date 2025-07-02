using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class Proc_Get_CUMAIS_Result
    {
        public int CumAisID { get; set; }
        public string OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string AccCode { get; set; }
        public string VoucherNo { get; set; }
        public System.DateTime AISDate { get; set; }
        public string Naration { get; set; }
        public string ReconPurposeCode { get; set; }
        public string Reference { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string VoucherType { get; set; }
    }
}

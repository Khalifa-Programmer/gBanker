using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DbCategoryTransfer
    {
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        public byte MemberCategoryID { get; set; }
        public string MemberCategoryCode { get; set; }

        public decimal Balance { get; set; }
        public System.DateTime ApproveDate { get; set; }
    }
}

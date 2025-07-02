using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class getTransferHistory_Result
    {
        public long TransferHistoryID { get; set; }
        public Nullable<int> OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public Nullable<int> CenterID { get; set; }
        public string CenterCode { get; set; }
        public Nullable<int> MemberID { get; set; }
        public string MemberCode { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string ProductCode { get; set; }
        public Nullable<int> TrProductID { get; set; }
        public string TrProductCode { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> Principal { get; set; }
        public Nullable<int> MemberCategoryId { get; set; }
        public Nullable<int> TrMemberCategoryID { get; set; }
        public string MemberCategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string trProductName { get; set; }
        public string ProductName { get; set; }
        public string TrMemberCategoryCode { get; set; }
        public string TrCategoryName { get; set; }
    }
}

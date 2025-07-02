using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DBRepaymentScheduleDetailModel
    {
        public long RepaymentScheduleID { get; set; }
        public long LoanSummaryID { get; set; }
        public int OfficeID { get; set; }
        public long MemberID { get; set; }
        public short ProductID { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public byte MemberCategoryID { get; set; }
        public int LoanTerm { get; set; }        
        public DateTime RepaymentDate { get; set; }
        public bool? IsActive { get; set; }                       
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime InstallmentDate { get; set; }
        public short InstallmentNo { get; set; }
        public decimal LoanInstallment { get; set; }
        public decimal IntInstallment { get; set; }
        public string CreateUser { get; set; }
        
    }
}

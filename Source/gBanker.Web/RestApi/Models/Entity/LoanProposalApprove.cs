using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class LoanProposalApprove
    {
        public int OfficeID { get; set; }
        public long MemberID { get; set; }
        public short ProductID { get; set; }
        public DateTime? InstallmentStartDate { get; set; }
        public Decimal IntCharge { get; set; }
        public Decimal LoanInstallment { get; set; }
        public Decimal IntInstallment { get; set; }
    }
}
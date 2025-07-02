using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class CsRefundGetResult
    {
        public long ImmatureLTSID { get; set; }
        public long SavingSummaryID { get; set; }
        public Decimal Calcnterest { get; set; }
        public Decimal Deposit { get; set; }
        public Decimal WithDrawal { get; set; }
        public Decimal Interest { get; set; }
        public bool Transfferred { get; set; }
        public DateTime TransDate { get; set; }
        public int ProductID { get; set; }
        public int OfficeID { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public Decimal CurrentInterest { get; set; }
        public Decimal InterestRate { get; set; }
        public Decimal WithdrawalRate { get; set; }
        public DateTime OpeningDate { get; set; }
        public Decimal SavingInstallment { get; set; }
        public int Duration { get; set; }
    }
}
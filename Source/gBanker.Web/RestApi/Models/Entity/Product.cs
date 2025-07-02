using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class Product
    {
        public Int16 ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        
        public Byte ProductType { get; set; }
        public Decimal InterestRate { get; set; }
        public Int16 Duration { get; set; }
        public string MainProductCode { get; set; }
        public Decimal LoanInstallment { get; set; }
        public Decimal InterestInstallment { get; set; }
        public Decimal SavingsInstallment { get; set; }
        public Decimal MinLimit { get; set; }
        public Decimal MaxLimit { get; set; }
        public string InterestCalculationMethod { get; set; }
        public string PaymentFrequency { get; set; }
        public string MainItemName { get; set; }
        public Int32 GracePeriod { get; set; } 
        public string SubMainCategory { get; set; }
        public Byte IsDisbursement { get; set; }
        public Boolean IsActive { get; set; }

    }
}
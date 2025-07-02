using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class ProductInstallment
    {
        public short ProductInstallmentMethodId { get; set; }
        public string ProductInstallmentMethodName { get; set; }
        public Decimal LoanInstallment { get; set; }
        public Decimal InterestInstallment { get; set; }
    }
}
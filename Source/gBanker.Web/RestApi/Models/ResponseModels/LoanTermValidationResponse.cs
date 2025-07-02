using gBanker.Web.RestApi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.ResponseModels
{
    public class LoanTermValidationResponse : ApiResponse
    {
        public LoanTermValidationResult result { get; set; }
        public List<ProductInstallment> productInstallment { get; set; }
    }
}
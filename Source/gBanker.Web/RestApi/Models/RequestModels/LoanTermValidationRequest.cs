using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class LoanTermValidationRequest
    {
        public int officeId { get; set; }
        public long memberId { get; set; }
        public string mainProductCode { get; set; }
        public int productId { get; set; }
        public string userId { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class CsRefundGetCalRequest
    {
        public int summaryId { get; set; }
        public string immatureDate { get; set; }
        public int productId { get; set; }
        public int operationStatus { get; set;}
        public int officeId { get; set; }
        public int createdUser { get; set; }

    }
}
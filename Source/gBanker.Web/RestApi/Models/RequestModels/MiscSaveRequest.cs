using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class MiscSaveRequest
    {
        public int officeId { get; set; }
        public int centerId { get; set; }
        public int memberId { get; set; }
        public int productId { get; set; }
        public double amount { get; set; }
        public String transDate { get; set; }
        public String remarks { get; set; }
        public String createUser { get; set; }
    }
}
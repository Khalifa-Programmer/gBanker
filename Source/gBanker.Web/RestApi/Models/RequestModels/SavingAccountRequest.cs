using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.RequestModels
{
    public class SavingAccountRequest
    {
        public SavingAccountModel[] accounts;
    }

    public class SavingAccountModel
    {
        public int officeId { get; set; }
        public int memberId { get; set; }
        public int productId { get; set; }
        public Decimal savingsInstallment { get; set; }
        public string createUser { get; set; }
        public DateTime createDate { get; set; }
    }
}
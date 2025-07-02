using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Web.RestApi.Models.RequestModels;
using gBanker.Web.RestApi.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace gBanker.Web.RestApi
{
    [System.Web.Mvc.RoutePrefix("/api/saving-accounts")]
    public class SavingsAccountController : ApiController
    {
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/approve")]
        public HttpResponseMessage ApproveAccounts(SavingAccountRequest requestModel)
        {
            MemberApproveResponse response = new MemberApproveResponse();
            int executed = 0;
            try {
                foreach (var m in requestModel.accounts)
                {
                    string sql = "Exec API_SavingsAccountOpening_Approved @OfficeID,@MemberID,@ProductID,@SavingsInstallment,@CreateUser,@CreateDate";
                    List<object> _params = new List<object>();
                    _params.Add(new SqlParameter("@OfficeID", m.officeId));
                    _params.Add(new SqlParameter("@MemberID", m.memberId));
                    _params.Add(new SqlParameter("@ProductID", m.productId));
                    _params.Add(new SqlParameter("@SavingsInstallment", m.savingsInstallment));
                    _params.Add(new SqlParameter("@CreateUser", m.createUser));
                    _params.Add(new SqlParameter("@CreateDate", m.createDate));
                    object[] allparams = _params.ToArray();

                    executed = new gBankerDbContext().Database.ExecuteSqlCommand(sql, allparams);
                }

                response.status = "true";
                response.message = "Savings Account approved successfully";
            }
            catch(Exception ex)
            {
                response.status = "false"; 
                response.message = "Savings Account approval falied "+ex.Message;
            }
        
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }
    }
}
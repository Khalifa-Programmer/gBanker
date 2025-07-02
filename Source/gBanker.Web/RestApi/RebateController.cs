using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Web.RestApi.Models.RequestModels;
using gBanker.Web.RestApi.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace gBanker.Web.RestApi
{
    [System.Web.Mvc.RoutePrefix("api/rebate")]
    public class RebateController : ApiController 
    {
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/calculate")]
        public HttpResponseMessage rebateCalculate(RebateCalculateRequest reqModel)
        {
            RebateCalcResponse response = new RebateCalcResponse();
            string sql = "Exec API_getRebateInfo " + reqModel.officeId+","+reqModel.loanSummaryId;
            try
            {
                using(var db = new gBankerDbContext())
                {
                    RebateCalcResult rcr = db.Database.SqlQuery<RebateCalcResult>(sql).FirstOrDefault();
                    response.status = "true";
                    response.rebate = rcr;
                }

            }catch(Exception ex)
            {
                response.status = "false";
                response.message = ex.Message;

            }
            return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
        }


    }
}
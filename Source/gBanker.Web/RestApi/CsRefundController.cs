using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Web.RestApi.Models.Entity;
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
    [System.Web.Mvc.RoutePrefix("api/cs-refund")]
    public class CsRefundController : ApiController
    {

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/calculate")]
        public HttpResponseMessage GetCalculatedResult(CsRefundGetCalRequest req)
        {
            CsRefundGetResultResponse crgrr = new CsRefundGetResultResponse();

            try { 
                 string sql = "EXEC API_getImmatureLTS " + req.summaryId + ",'" + req.immatureDate + "'," + req.productId + "," +
                req.operationStatus + "," + req.officeId + ",'" + req.createdUser + "'";

                using(var db = new gBankerDbContext())
                {
                    CsRefundGetResult  csGetResult =  db.Database.SqlQuery<CsRefundGetResult>(sql).FirstOrDefault();
                    crgrr.status = "true";
                    crgrr.csRefundGetResult = csGetResult;
                }

           }catch(Exception ex){
                crgrr.status = "false";
                crgrr.message =  ex.Message;
           } 

            return Request.CreateResponse(HttpStatusCode.OK, crgrr, Configuration.Formatters.JsonFormatter);
        }

        
    }
}
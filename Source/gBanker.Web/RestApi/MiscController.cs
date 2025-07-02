using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Web.RestApi.Models.Entity;
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
    [System.Web.Mvc.RoutePrefix("api/misc")]
    public class MiscController : ApiController
    {
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/save")]
        public HttpResponseMessage saveMisc(MiscSaveRequest reqModel)
        {
            MiscEntryResponse miscEntryResponse = new MiscEntryResponse();
            string sql = "Exec API_Insert_Miscellaneous " + reqModel.officeId + "," + reqModel.centerId + "," 
                + reqModel.productId + "," + reqModel.amount + ",'" + reqModel.transDate + "','" +
                reqModel.createUser + "'," + reqModel.memberId + ",'" + reqModel.remarks + "'";
            try
            {
                using(var db = new gBankerDbContext())
                {
                    MiscEntryResult mer = db.Database.SqlQuery<MiscEntryResult>(sql).FirstOrDefault();
                    miscEntryResponse.status = "true";
                    miscEntryResponse.mer = mer;
                }


            }catch(Exception ex)
            {
                miscEntryResponse.status = "false";
                miscEntryResponse.message = ex.Message;
            }

            return Request.CreateResponse(HttpStatusCode.OK,miscEntryResponse, Configuration.Formatters.JsonFormatter);
        }


        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("/delete")]
        public HttpResponseMessage deleteMisc(MiscDeleteRequest reqModel)
        {
            MiscDeleteResponse miscResponse = new MiscDeleteResponse();
            try
            {
                using(var db = new gBankerDbContext())
                {
                    string guarantorSql = "EXEC API_Miscellaneous_Del @OfficeID,@CenterID,@ProductID,"
                        + "@TrxDate,@CreateUser,@MemberID,@MiscellaneousId";

                    List<object> _params = new List<object>();
                    _params.Add(new SqlParameter("@OfficeID", reqModel.officeId));
                    _params.Add(new SqlParameter("@CenterID", reqModel.centerId));
                    _params.Add(new SqlParameter("@ProductID", reqModel.productId));
                    _params.Add(new SqlParameter("@TrxDate", reqModel.transDate));
                    _params.Add(new SqlParameter("@CreateUser", reqModel.createUser));
                    _params.Add(new SqlParameter("@MemberID", reqModel.memberId));
                    _params.Add(new SqlParameter("@MiscellaneousId", reqModel.miscId));
                    object[] allparams = _params.ToArray();

                    db.Database.ExecuteSqlCommand(guarantorSql, allparams);
                    miscResponse.status = "true";
                    miscResponse.message = "Successfully Deleted";
                }
            }catch(Exception ex)
            {
                miscResponse.status = "false";
                miscResponse.message = ex.Message;
            }
            return Request.CreateResponse(HttpStatusCode.OK, miscResponse, Configuration.Formatters.JsonFormatter);
        }

    }
}
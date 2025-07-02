using gBanker.Service;
using gBanker.Service.ReportServies;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Mvc;
namespace gBanker.Web.RestApi
{

    public class CollectionAPIModel
    {
        public string CollectionDate { get; set; }
        public string UserId { get; set; }
        public int APIVersion { get; set; }
        public List<LoanCollectionModel> Collections { get; set; }
    }
    public class LoanCollectionModel
    {
        public long MemberID { get; set; }
        public decimal Amount { get; set; }
        public int OfficeID { get; set; }
        public int CenterID { get; set; }
        public int ProductID { get; set; }
        //   public string loggedInUser { get; set; }
        public string Token { get; set; }
        public long CollectionID { get; set; }
        public long Sid { get; set; }
        public int TType { get; set; }
        public int PType { get; set; }
        public decimal IntCharge { get; set; }
        public decimal LoanInstallment { get; set; }
        public decimal IntInstallment { get; set; }
        public double fine { get; set; }
        // public int ApiVersion { get; set; }
    }
    public class UploadResponse
    {
        public HttpStatusCode status;
        public string message;


        public UploadResponse(HttpStatusCode s, string m)
        {
            this.status = s;
            this.message = m;
        }
    }

    public class UploadCollectionController : ApiController
    {
        // GET api/<controller>
        private readonly IUltimateReportService ultimateReportService;
        private readonly IOfficeService officeService;
        public UploadCollectionController(IOfficeService officeService, IUltimateReportService ultimateReportService)
        {
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;

        }


        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("api/post-mobile-collection")]
        public HttpResponseMessage PostMobileCollection([FromBody] CollectionAPIModel apiRequest) //(string apiRequest) //[FromBody] CollectionAPIModel apiRequest
        {
            UploadResponse output = null;
            var msg = "";
            if (apiRequest == null)
            {
                output = new UploadResponse(HttpStatusCode.OK, "Sorry Nothing To Upload");
                return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
            }
            var jsonStrings = new JavaScriptSerializer().Serialize(apiRequest);

            try
            {
                var param = new { JsonString = jsonStrings };
                var execute = ultimateReportService.GetDataWithParameter(param, "InsertIntoTabCollection");
                output = new UploadResponse(HttpStatusCode.OK, "Sucessfully Uploaded");
            }
            catch (Exception ex)
            {
                output = new UploadResponse(HttpStatusCode.NotAcceptable, "Sorry Try Again Later");
            }

            return Request.CreateResponse(HttpStatusCode.OK, output, Configuration.Formatters.JsonFormatter);
        }

    }
}

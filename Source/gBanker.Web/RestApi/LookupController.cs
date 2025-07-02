using gBanker.Data;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.RestApi.Models.ResponseModels;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace gBanker.Web.RestApi
{
    public class LookupController : ApiController
    {
        private readonly IUltimateReportService ultimateReportService;
        private readonly IOfficeService officeService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmployeeOfficeMappingService employeeOfficeService;
        private readonly ILogger loggger;
        private readonly IEmployeeService employeeService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IAspNetUserService aspNetUserService;
        public LookupController(IOfficeService officeService, IUltimateReportService ultimateReportService, UserManager<ApplicationUser> userManager, IEmployeeOfficeMappingService employeeOfficeService, ILogger loggger, IEmployeeService employeeService, IGroupwiseReportService groupwiseReportService, IAspNetUserService aspNetUserService)
        {
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.userManager = userManager;
            this.employeeOfficeService = employeeOfficeService;
            this.loggger = loggger;
            this.employeeService = employeeService;
            this.groupwiseReportService = groupwiseReportService;
            this.aspNetUserService = aspNetUserService;

        }



        
        [System.Web.Http.AcceptVerbs("GET")]
        [System.Web.Http.HttpGet]
        [System.Web.Mvc.Route("api/syncdata")]
        public HttpResponseMessage GetOfficeSyncData(int officeID, string loginID)
        {
            SyncDataModel syncDataModel =  new SyncDataModel();
            var aspnetUser = aspNetUserService.GetByUserId(loginID);
            //var empid = employeeService.GetByCode(loginID);
            if (aspnetUser == null)
                throw new Exception("Invalid Login ID");
            try
            {
                var param = new { OfficeId = officeID, EmployeeID = aspnetUser.EmployeeID };
                var alldata = new gBankerDbContext().Database.SqlQuery<Models.Entity.MemberProductDetail> ("Exec API_GetOfficeMemberDetailAllSamity " + officeID+","+ aspnetUser.EmployeeID).ToList();
                
                var versionNo = new gBankerDbContext().Database.SqlQuery<int>("Select VersionNo from APISettings").FirstOrDefault();

                syncDataModel.status = HttpStatusCode.OK;
                syncDataModel.data = alldata;
                syncDataModel.apiVersion = versionNo;
                return Request.CreateResponse(HttpStatusCode.OK, syncDataModel, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
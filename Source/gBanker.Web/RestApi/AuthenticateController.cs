using gBanker.Data;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.RestApi.Models.Entity;
using gBanker.Web.RestApi.Models.ResponseModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace gBanker.Web.RestApi
{
    public class CustomerModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    class EmployeeModel
    {
        public int OrgID { get; set; }
        public string EmployeeCode { get; set; }
    }
    public class AuthenticateController : ApiController
    {
        private readonly IUltimateReportService ultimateReportService;
        private readonly IOfficeService officeService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmployeeOfficeMappingService employeeOfficeService;
        private readonly ILogger loggger;
        public AuthenticateController(IOfficeService officeService, IUltimateReportService ultimateReportService, UserManager<ApplicationUser> userManager, IEmployeeOfficeMappingService employeeOfficeService, ILogger loggger)
        {
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.userManager = userManager;
            this.employeeOfficeService = employeeOfficeService;
            this.loggger = loggger;
        }

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("api/login")]
        public HttpResponseMessage login(CustomerModel model)
        {
            Login output;
            var user = userManager.Find(model.username, model.password);
            if (user != null)
            {
                LoginSucessData loginSucessData = new LoginSucessData();

                var centers = new gBankerDbContext().Database.SqlQuery<Models.Entity.Center>("Exec API_Centers '" + model.username+"'").ToList();
                var groups = new gBankerDbContext().Database.SqlQuery<Models.Entity.Group>("Exec API_Groups '" + model.username+"'").ToList();
                var memberCategorys = new gBankerDbContext().Database.SqlQuery<Models.Entity.MemberCategory>("Exec API_MemberCategories '" + model.username+"'").ToList();
                var countrys = new gBankerDbContext().Database.SqlQuery<Models.Entity.Country>("Exec API_Country '" + model.username+"'").ToList();
                var divisions = new gBankerDbContext().Database.SqlQuery<Models.Entity.Division>("Exec API_Divisions '" + model.username+"'").ToList();
                var districts = new gBankerDbContext().Database.SqlQuery<Models.Entity.District>("Exec API_Districts '" + model.username+"'").ToList();
                var subDistricts = new gBankerDbContext().Database.SqlQuery<Models.Entity.SubDistrict>("Exec API_Upozillas '" + model.username+"'").ToList();
                var unions = new gBankerDbContext().Database.SqlQuery<Models.Entity.Union>("Exec API_Unions '" + model.username+"'").ToList();
                var villages = new gBankerDbContext().Database.SqlQuery<Models.Entity.Village>("Exec API_Villages '" + model.username+"'").ToList();
                var placeOfBirths = new gBankerDbContext().Database.SqlQuery<Models.Entity.District>("Exec API_Districts '" + model.username+"'").ToList();
                var citizenships = new gBankerDbContext().Database.SqlQuery<Models.Entity.Citizenship>("Exec API_Citizenships '" + model.username+"'").ToList();
                var genders = new gBankerDbContext().Database.SqlQuery<Models.Entity.Gender>("Exec API_Genders '"+model.username+"'").ToList();
                var homeTypes = new gBankerDbContext().Database.SqlQuery<Models.Entity.HomeType>("Exec API_HomeTypes '" + model.username+"'").ToList();
                var groupTypes = new gBankerDbContext().Database.SqlQuery<Models.Entity.GroupType>("Exec API_GroupTypes '" + model.username+"'").ToList();
                var educations = new gBankerDbContext().Database.SqlQuery<Models.Entity.Education>("Exec API_Educations '" + model.username+"'").ToList();
                var economicActivities = new gBankerDbContext().Database.SqlQuery<Models.Entity.EconomicActivity>("Exec API_EconomicActivities '"+model.username+"'").ToList();
                var materialStatuses = new gBankerDbContext().Database.SqlQuery<Models.Entity.MaritalStatus>("Exec API_MaritalStatuses '"+model.username+"'").ToList();
                var products = new gBankerDbContext().Database.SqlQuery<Models.Entity.Product>("Exec API_Product "+model.username).ToList();
                var investors = new gBankerDbContext().Database.SqlQuery<Models.Entity.Investor>("Exec API_Invstors '" + model.username+"'").ToList();
                var purposes = new gBankerDbContext().Database.SqlQuery<Models.Entity.Purpose>("Exec API_Purpose '" + model.username+"'").ToList();
                var menus = new gBankerDbContext().Database.SqlQuery<Models.Entity.Menu>("Exec API_Menus").ToList();
                

                loginSucessData.centers = centers;
                loginSucessData.groups = groups;
                loginSucessData.memberCategorys = memberCategorys;
                loginSucessData.countrys = countrys;
                loginSucessData.divisions = divisions;
                loginSucessData.districts = districts;
                loginSucessData.subDistricts = subDistricts;
                loginSucessData.unions = unions;
                loginSucessData.villages = villages;
                loginSucessData.placeOfBirths = placeOfBirths;

                loginSucessData.citizenships = citizenships;

                loginSucessData.genders = genders;
                loginSucessData.groupTypes = groupTypes;
                loginSucessData.homeTypes = homeTypes;
                loginSucessData.groups = groups;
                loginSucessData.educations = educations;
                loginSucessData.economicActivities = economicActivities;
                loginSucessData.maritalStatuses = materialStatuses;
                loginSucessData.products = products;
                loginSucessData.investors = investors;
                loginSucessData.purposes = purposes;
                loginSucessData.menus = menus;

                var userEmpid = user.EmployeeID;
                var FirstName = user.FirstName;
                var apiSetting = new gBankerDbContext().Database.SqlQuery<ApiSetting>("SELECT * FROM APISettings").FirstOrDefault();
                var Employee = new gBankerDbContext().Database.SqlQuery<EmployeeModel>("SELECT OrgID,EmployeeCode FROM Employee WHERE EmployeeID=" + userEmpid).FirstOrDefault();
                var empcode = Employee.EmployeeCode;
                var office = employeeOfficeService.GetEmployeeOfficeMappings(empcode).Select((s) => new SelectListItem() { Value = s.OfficeID.ToString(), Text = string.Format("{0} - {1}", s.Office.OfficeCode, s.Office.OfficeName), Selected = true }).FirstOrDefault();
                loginSucessData.office = office;

                List<Bank> banks = new gBankerDbContext().Database.SqlQuery<Bank>("Exec API_BankList '" + model.username + "'").ToList();

                loginSucessData.permissions = new gBankerDbContext().Database.SqlQuery<MenuPermission>("Exec API_MenusPermission " + office.Value + ", '" + model.username+"'").ToList();
                loginSucessData.trxDate = new gBankerDbContext().Database.SqlQuery<string>("Exec API_InstallmentDate " + office.Value + "," + model.username).FirstOrDefault();

                loginSucessData.banks = banks;

                if (Employee.OrgID == 54)
                {
                    loginSucessData.MemberTypeListSpecfic();
                }
                else
                {
                    loginSucessData.MemberTypeList();
                }
                output = new Login(HttpStatusCode.OK, model.username,FirstName, "Login successful", user.Id, Employee.OrgID, loginSucessData);
                output.apiSetting = apiSetting;
            }
            else
            {
                output = new Login(HttpStatusCode.NotAcceptable, model.username,null, "Login failed, invalid username/password",null, 0, null);
            }

            return Request.CreateResponse(output.status, output, Configuration.Formatters.JsonFormatter);
        }



        

    }
}
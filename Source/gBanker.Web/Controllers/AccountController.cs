using System;
using System.Collections.Generic;
using System.Linq;
//using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using gBanker.Web.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using gBanker.Data;
using gBanker.Service;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using AutoMapper;
using gBanker.Web.Filters;
using System.Text;
using System.Data;
using gBanker.Service.ReportServies;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Web.Helpers;
using gBanker.Core.Utility;

namespace gBanker.Web.Controllers
{
    [Authorize]
    // [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> UserManager;
        private readonly IEmployeeService employeeService;
        private readonly IAspNetRoleService roleService;
        private readonly IDayInitialService dayInitialService;
        private readonly ISecurityService securityService;
        private readonly IAspNetUserService userService;
        private readonly ILogger loggger;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IOrganizationService organizationService;

        public AccountController(UserManager<ApplicationUser> userManager,
            IOrganizationService organizationService,

            IEmployeeService employeeService, IAspNetRoleService roleService, IDayInitialService dayInitialService, ISecurityService securityService, IAspNetUserService userService, ILogger loggger, IOfficeService officeService, IUltimateReportService ultimateReportService)
        {
            this.UserManager = userManager;
            this.employeeService = employeeService;
            this.roleService = roleService;
            this.dayInitialService = dayInitialService;
            this.securityService = securityService;
            this.userService = userService;
            this.loggger = loggger;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.organizationService = organizationService;
        }

        [AllowAnonymous]
        public ActionResult GetSecurityStamp()
        {
            var orgId = SessionHelper.LoginUserOrganizationID;
            SessionHelper.ServiceMessage = GetServiceMessageList();
            if (orgId == 3)
            {
                var currentUser = User.Identity.Name;
                var user = UserManager.FindByName(currentUser);

                var currentSecurityStamp = Session["SecurityStamp"] as string;
                if (currentSecurityStamp == null)
                {
                    currentSecurityStamp = user.SecurityStamp.ToString();
                    Session["SecurityStamp"] = currentSecurityStamp;
                }
                else if (currentSecurityStamp == user.SecurityStamp.ToString())
                {
                    Session["SecurityStamp"] = currentSecurityStamp;
                }

                if (currentSecurityStamp != user.SecurityStamp.ToString())
                {
                    return Json("logout", JsonRequestBehavior.AllowGet);
                }

                return Json(currentSecurityStamp, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("This is not for you", JsonRequestBehavior.AllowGet);
            }
        }

        //-------------------------------
        /// <summary>
        /// RAZIB
        //public ActionResult RegisterInfo()
        //{
        //    MapDropdownListValues();

        //    return View();
        //}

        /// </summary>
        /// <param name="logOff"></param>
        /// <returns></returns>
        public ActionResult LogOutSession(bool logOff)
        {
            var returnUrl = Url.Action("Login", "Account");
            LogRequest();
            AuthenticationManager.SignOut();
            Session.Clear();
            Session.Abandon();
            return Json(returnUrl, JsonRequestBehavior.AllowGet);
        }

        [SessionExpireFilter]
        [DisableCache]
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["UserList"] = items;
            LogRequest();
            return View();
        }
        //
        [SessionExpireFilter]
        [DisableCache]
        public ActionResult DeleteLogin(string Id)
        {
            try
            {
                IEnumerable<SelectListItem> items = new SelectList(" ");
                ViewData["UserList"] = items;
                userService.DeleteLogin(Id);
                return View("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [SessionExpireFilter]
        [DisableCache]
        public JsonResult EditUserRole(AspNetUser user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Id) || user.RoleId <= 0)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Form is not valid! " +
                          "Please correct it and try again."
                    });
                }
                var dbUser = userService.GetByUserId(user.Id);
                if (dbUser != null)
                {
                    dbUser.RoleId = user.RoleId;
                    userService.Update(dbUser);
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public string GetServiceMessageList()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                /*
                string WorkAreaIds = Convert.ToString(WorkAreaId);

                if (WorkAreaId != null) //"0"
                    sb.Append(" AND prwa.PRWorkAreaID =" + WorkAreaIds);
                  
                 */

                List<ServiceMessageViewModel> List_ViewModel = new List<ServiceMessageViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = ultimateReportService.GetDataWithParameter(param, "SP_ServiceMessage_List");
                string ServiceMessage = "";

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new ServiceMessageViewModel
                {
                    rowSl = row.Field<Int64>("rowSl"),
                    ServiceMessage = row.Field<string>("ServiceMessage")


                }).ToList();

                foreach (var message in List_ViewModel)
                {
                    ServiceMessage = ServiceMessage + " :: " + message.ServiceMessage;
                }



                return ServiceMessage;


            }
            catch (Exception ex)
            {
                return "";
            }

        }// End Function

        public ActionResult ManageServiceMessage()
        {
            return View();
        }

        #region ManageServiceMessage

        public JsonResult CreateUpdateServiceMessage(int ServiceMessageID = 0, string ServiceMessage = "", string FromDate = "", string ToDate = "")
        {
            string result = "OK";
            try
            {
                var param = new { ServiceMessageID = ServiceMessageID, Message = ServiceMessage, FromDate = FromDate, ToDate = ToDate };
                var val = ultimateReportService.GetDataWithParameter(param, "SP_PR_CreateUpdateServiceMessage");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // Delete Area
        public JsonResult DeleteServiceMessage(int ServiceMessageId)
        {
            string result = "OK";
            try
            {
                DateTime UpdateDate = DateTime.Now;

                var param2 = new { ServiceMessageId = ServiceMessageId };
                var val = ultimateReportService.GetDataWithParameter(param2, "SP_PR_DeleteServiceMessage");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TakeOnlineOffLine(int ServiceMessageId)
        {
            string result = "OK";
            try
            {
                DateTime UpdateDate = DateTime.Now;
                var param2 = new { ServiceMessageId = ServiceMessageId };
                var val = ultimateReportService.GetDataWithParameter(param2, "SP_PR_OnlineOfflineServiceMessage");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllServiceMessageList(string ServiceMessageId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string ServiceMessageIds = Convert.ToString(ServiceMessageId);

                if (ServiceMessageId != null) //"0"
                    sb.Append(" AND ServiceMessageId =" + ServiceMessageIds);

                List<ServiceMessageViewModel> List_ViewModel = new List<ServiceMessageViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = ultimateReportService.GetDataWithParameter(param, "SP_PR_Get_ServiceMessage_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new ServiceMessageViewModel
                {
                    rowSl = row.Field<Int64>("rowSl"),
                    ServiceMessageID = row.Field<int>("ServiceMessageId"),
                    ServiceMessage = row.Field<string>("Message"),
                    FromDate = row.Field<string>("FromDate"),
                    ToDate = row.Field<string>("ToDate"),
                    isActive = row.Field<bool>("isActive"),
                    isOnline = row.Field<bool?>("isOnline"),

                }).ToList();

                if (ServiceMessageId != null)
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function


        #endregion ManageServiceMessage



        [SessionExpireFilter]
        [DisableCache]
        public JsonResult RoleList(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var selectedRles = roleService.GetAll().Where(w => w.Id == id).Select(s => new { DisplayText = s.Name, Value = s.Id }).ToList();
                return new JsonResult() { Data = new { Result = "OK", Options = selectedRles }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var allRoles = roleService.GetAll().Select(s => new { DisplayText = s.Name, Value = s.Id }).ToList();
                return new JsonResult() { Data = new { Result = "OK", Options = allRoles }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult GetUserList()
        {
            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var empList = userService.GetAll();
            var viewEmp = empList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.UserName.ToString(),
                Text = x.UserName.ToString() + " " + x.FirstName.ToString()
            });
            var emp_items = new List<SelectListItem>();
            if (viewEmp.ToList().Count > 0)
            {
                emp_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            }
            emp_items.AddRange(viewEmp);
            return Json(emp_items, JsonRequestBehavior.AllowGet);
        }

        [SessionExpireFilter]
        [DisableCache]
        public ActionResult GetAllLogins(int jtStartIndex, int jtPageSize, string jtSorting, string EmpId)
        {
            try
            {
                var param = new { UserName = EmpId };
                var data = ultimateReportService.GetDataWithParameter(param, "GetAllLoginUser");
                var viewData = data.Tables[0].AsEnumerable().Select(p => new RegisterModel
                {
                    Id = p.Field<string>("Id"),
                    UserName = p.Field<string>("UserName"),
                    FirstName = p.Field<string>("FirstName"),
                    Email = p.Field<string>("Email"),
                    RoleId = p.Field<int>("RoleId"),
                    RoleName = p.Field<string>("RoleName")
                }).ToList();

                var totalCount = viewData.Count();
                var entities = viewData.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = entities, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            LogRequest();
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ServiceMessages = GetServiceMessageList();
            SessionHelper.ServiceMessage = GetServiceMessageList();
            return View();
        }

        //
        // POST: /Account/Login

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        //{
        //    LogRequest();
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindAsync(model.UserName, model.Password);
        //        if (user != null)
        //        {
        //            await SignInAsync(user, model.RememberMe);
        //            var employee = employeeService.GetByCode(model.UserName);
        //            if (employee != null)
        //            {
        //                var entity = Mapper.Map<Employee, EmployeeViewModel>(employee);
        //                var parentModules = securityService.GetAllPrentModule().ToList();
        //                var userModules = securityService.GeAllRoleModules(user.RoleId).ToList();
        //                Session[SessionKeys.LOGGED_IN_USER_ROLE] = user.RoleId;
        //                //KHALID
        //                var LoggedInEmpOfficeID = employee.OfficeID;
        //                var LoggedinOffice = officeService.GetById(LoggedInEmpOfficeID);
        //                // var OfficeLevel = LoggedinOffice.OfficeLevel;
        //                Session[SessionKeys.LOGGED_IN_Employee_Office_Level] = LoggedinOffice.OfficeLevel;
        //                // KHALID

        //                var allMoulules = securityService.GetAllModules().ToList();
        //                SessionHelper.LogSessionInfo(entity, parentModules, userModules, allMoulules);

        //                var orgInfo = organizationService.GetById(entity.OrgID);

        //                //track organization code into session
        //                SessionHelper.LoggedInOrganizationCode = orgInfo.OrganizationCode;
        //                SessionHelper.OrganizationName = orgInfo.OrganizationName;
        //                SessionHelper.OrganizationAddress = orgInfo.OrgAddress;
        //            }

        //            {
        //                ModelState.AddModelError("", "Employee information not found with for this user.");
        //            }
        //            try
        //            {
        //                DateTime? transactionDate;
        //                string OrginizationName;
        //                string Processtype;
        //                DateTime? LastDayEndDate;
        //                var dayInitialStatus = dayInitialService.ValidateDayInitial(employee.OfficeID, out transactionDate, out OrginizationName, out Processtype, out LastDayEndDate, Convert.ToInt16(employee.OrgID));
        //                SessionHelper.TransactionDay = dayInitialStatus;
        //                SessionHelper.TransactionDate = transactionDate.HasValue ? transactionDate.Value : default(DateTime);
        //                SessionHelper.OrganizationName = OrginizationName;
        //                SessionHelper.ProcessType = Processtype;
        //                SessionHelper.LastDayEndDate = LastDayEndDate;
        //                SessionHelper.IsDayInitiated = !string.IsNullOrEmpty(dayInitialStatus);
        //            }
        //            catch (Exception ex)
        //            {
        //                SessionHelper.IsDayInitiated = false;
        //            }
        //            return RedirectToLocal(returnUrl);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Invalid username or password.");
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            LogRequest();
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.UserName, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
                else
                {
                    AspNetUser user2 = userService.GetByUserId(user.Id);
                    var dayDifference = DateTime.Now.Date - user2.UpdateDate.Date;

                    if (dayDifference.Days <= 90)
                    {
                        if (dayDifference.TotalDays >= 83)
                        {
                            TempData["Expire_Msg"] = $"Your password will be expired within {90 - dayDifference.Days} days. Please Change your password before expired.";
                        }

                        await SignInAsync(user, model.RememberMe);
                        var employee = employeeService.GetByCode(model.UserName);
                        if (employee != null)
                        {
                            var entity = Mapper.Map<Employee, EmployeeViewModel>(employee);
                            var parentModules = securityService.GetAllPrentModule().ToList();
                            var userModules = securityService.GeAllRoleModules(user.RoleId).ToList();
                            Session[SessionKeys.LOGGED_IN_USER_ROLE] = user.RoleId;
                            //KHALID
                            var LoggedInEmpOfficeID = employee.OfficeID;
                            var LoggedinOffice = officeService.GetById(LoggedInEmpOfficeID);
                            // var OfficeLevel = LoggedinOffice.OfficeLevel;
                            Session[SessionKeys.LOGGED_IN_Employee_Office_Level] = LoggedinOffice.OfficeLevel;
                            // KHALID
                            var allMoulules = securityService.GetAllModules().ToList();
                            SessionHelper.LogSessionInfo(entity, parentModules, userModules, allMoulules);

                            var orgInfo = organizationService.GetById(entity.OrgID);

                            //track organization code into session
                            SessionHelper.LoggedInOrganizationCode = orgInfo.OrganizationCode;
                            SessionHelper.OrganizationName = orgInfo.OrganizationName;
                            SessionHelper.OrganizationAddress = orgInfo.OrgAddress;
                            SessionHelper.ServiceMessage = GetServiceMessageList();
                            //var image_retrieval_type = applicationSettingsService.GetByOrgID(orgInfo.OrgID);
                            //SessionHelper.ImageRetrievalType = image_retrieval_type.ImageRetrievalType;

                        }

                        {
                            ModelState.AddModelError("", "Employee information not found with for this user.");
                        }
                        try
                        {
                            DateTime? transactionDate;
                            string OrginizationName;
                            string Processtype;
                            DateTime? LastDayEndDate;
                            var dayInitialStatus = dayInitialService.ValidateDayInitial(employee.OfficeID, out transactionDate, out OrginizationName, out Processtype, out LastDayEndDate, Convert.ToInt16(employee.OrgID));
                            SessionHelper.TransactionDay = dayInitialStatus;
                            SessionHelper.TransactionDate = transactionDate.HasValue ? transactionDate.Value : default(DateTime);
                            SessionHelper.OrganizationName = OrginizationName;
                            SessionHelper.ProcessType = Processtype;
                            SessionHelper.LastDayEndDate = LastDayEndDate;
                            SessionHelper.IsDayInitiated = !string.IsNullOrEmpty(dayInitialStatus);
                        }
                        catch (Exception ex)
                        {
                            SessionHelper.IsDayInitiated = false;
                        }
                        return RedirectToLocal(returnUrl);
                    }
                    else
                        ModelState.AddModelError("", "Your password has been expired. Please change your password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [AllowAnonymous]
        public async Task<ActionResult> LoginExternal(string rd)
        {
            LogRequest();
            var requestDetail = LoginExternalHelper.ValidateAndGetSharedInfo(rd);
            if (requestDetail != null)
            {
                var url = GetUrl(requestDetail.UrlId);
                if (string.IsNullOrEmpty(url.Key))
                    ModelState.AddModelError("", "Invalid request.");
                else
                {
                    var user = await UserManager.FindAsync(requestDetail.LoginID, requestDetail.Password);
                    if (user != null)
                    {
                        await SignInAsync(user, false);
                        var employee = employeeService.GetByCode(requestDetail.LoginID);
                        if (employee != null)
                        {
                            var entity = Mapper.Map<Employee, EmployeeViewModel>(employee);
                            var parentModules = securityService.GetAllPrentModule().ToList();
                            var userModules = securityService.GeAllRoleModules(user.RoleId).ToList();
                            var allMoulules = securityService.GetAllModules().ToList();
                            SessionHelper.LogSessionInfo(entity, parentModules, userModules, allMoulules);



                        }
                        else
                        {
                            ModelState.AddModelError("", "Employee information not found with for this user.");
                        }
                        try
                        {
                            DateTime? transactionDate;
                            string OrginizationName;
                            string Processtype;
                            DateTime? LastDayEndDate;
                            var dayInitialStatus = dayInitialService.ValidateDayInitial(employee.OfficeID, out transactionDate, out OrginizationName, out Processtype, out LastDayEndDate, Convert.ToInt16(employee.OrgID));
                            SessionHelper.TransactionDay = dayInitialStatus;
                            SessionHelper.TransactionDate = transactionDate.HasValue ? transactionDate.Value : default(DateTime);
                            SessionHelper.OrganizationName = OrginizationName;
                            SessionHelper.ProcessType = Processtype;
                            SessionHelper.LastDayEndDate = LastDayEndDate;
                            SessionHelper.IsDayInitiated = !string.IsNullOrEmpty(dayInitialStatus);
                            LoadLoggedInUserOfficeDetail(requestDetail.OfficeId);

                        }
                        catch (Exception ex)
                        {
                            SessionHelper.IsDayInitiated = false;
                        }
                        return RedirectToAction(url.Key, url.Value);

                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
            }
            else
                ModelState.AddModelError("", "Unauthorized login attempt.");
            // If we got this far, something failed, redisplay form
            return View("Login");
        }
        [AllowAnonymous]
        public ActionResult ChangeCurrentPassword()
        {
            return View();
        }


        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> ChangeCurrentPassword(PasswordChangeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.CurrentPassword == model.NewPassword)
                    {
                        ModelState.AddModelError("", "Cannot use current password as new password.");
                    }
                    else
                    {
                        ApplicationUser user = await UserManager.FindAsync(model.UserName, model.CurrentPassword);

                        if (user != null)
                        {
                            user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.NewPassword);
                            var result = await UserManager.UpdateAsync(user);

                            AspNetUser aspNetUser = userService.GetByName(model.UserName);
                            aspNetUser.UpdateDate = DateTime.Now.Date;
                            userService.Update(aspNetUser);

                            if (result.Succeeded)
                            {
                                ModelState.Clear();
                                ViewBag.IsSuccess = true;
                            }
                            else
                            {
                                ModelState.AddModelError("", "Error Occured.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Please check username && password.");
                        }
                    }
                }
                return View(model);
            }
            catch (Exception error)
            {
                throw error;
            }
        }
        private void LoadLoggedInUserOfficeDetail(int officeId)
        {
            ViewBag.ShowPopup = false;

            if (officeId > 0)
            {
                SessionHelper.LoginUserOfficeID = officeId;
                var office = officeService.GetById(SessionHelper.LoginUserOfficeID.Value);
                var entity = Mapper.Map<Office, OfficeViewModel>(office);
                SessionHelper.LoggedInOfficeDetail = entity;
                try
                {
                    DateTime? transactionDate;
                    string OrginizationName;
                    string Processtype;
                    DateTime? LastDayEndDate;
                    DateTime? dDAte;
                    var dayInitialStatus = dayInitialService.ValidateDayInitial(SessionHelper.LoginUserOfficeID, out transactionDate, out OrginizationName, out Processtype, out LastDayEndDate, SessionHelper.LoginUserOrganizationID.Value);
                    SessionHelper.TransactionDay = dayInitialStatus;
                    SessionHelper.TransactionDate = transactionDate.HasValue ? transactionDate.Value : default(DateTime);
                    SessionHelper.OrganizationName = OrginizationName;
                    SessionHelper.ProcessType = Processtype;
                    SessionHelper.LastDayEndDate = LastDayEndDate;
                    SessionHelper.IsDayInitiated = !string.IsNullOrEmpty(dayInitialStatus);

                    //officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID))
                    //officeFullName = office.OfficeCode + ", " + office.OfficeName;
                    var officeFullName = office.OfficeCode + ", " + office.OfficeName + ", DashBoardDate:" + SessionHelper.LastDayEndDate.Value.ToString("dd-MMM-yyyy");
                    //OrgName = PreAnotation + " " + SessionHelper.OrganizationName + "-" + officeFullName;
                    var OrgName = SessionHelper.OrganizationName + "-" + officeFullName;
                    // OrgName = PreAnotation + " " + SessionHelper.OrganizationName ;
                }
                catch (Exception ex)
                {
                    SessionHelper.IsDayInitiated = false;
                }
            }
        }

        private IAuthenticationManager _authnManager;

        // Modified this from private to public and add the setter
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                if (_authnManager == null)
                    _authnManager = HttpContext.GetOwinContext().Authentication;
                return _authnManager;
            }
            set { _authnManager = value; }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        //public ActionResult Login(LoginModel model, string returnUrl)
        //{
        //    if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
        //    {
        //        return RedirectToLocal(returnUrl);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    ModelState.AddModelError("", "The user name or password provided is incorrect.");
        //    return View(model);
        //}

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            LogRequest();
            AuthenticationManager.SignOut();
            Session.Clear();
            Session.Abandon();
            //gBankerSessionFacade.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff(bool logOff)
        {
            LogRequest();
            AuthenticationManager.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        //
        // GET: /Account/Register

        //[AllowAnonymous]
        [SessionExpireFilter]
        [DisableCache]
        public ActionResult Register()
        {
            MapDropdownListValues();
            return View();
        }
        private void MapDropdownListValues()
        {
            var roleList = roleService.GetAll().ToList();
            roleList.Insert(0, new AspNetRole() { Id = "0", Name = "Select Role" });
            ViewBag.RoleList = roleList.Select(m => new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });
        }
        //
        // POST: /Account/Register

        [HttpPost]
        // [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        [DisableCache]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = employeeService.GetByCode(model.UserName);
                if (employee != null)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = model.UserName,
                        EmployeeID = employee.EmployeeID,
                        FirstName = employee.EmpName,
                        RoleId = model.RoleId,
                        Email = model.Email,
                        UpdateDate = DateTime.Now.Date
                    };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return Json(new { Result = "OK", Message = "Login Created successfully." }, JsonRequestBehavior.AllowGet);
                        //await SignInAsync(user, isPersistent: false);
                        //return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var msg = "";
                        foreach (var r in result.Errors)
                        {
                            msg = string.Format("{0}<br/>{1}", msg, r);
                        }
                        return Json(new { Result = "ERROR", Message = msg }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json(new { Result = "ERROR", Message = "Invalid Employee Code." }, JsonRequestBehavior.AllowGet);
                // ModelState.AddModelError("UserName", "Invalid Employee Code.");
            }
            return Json(new { Result = "ERROR", Message = "Please correct required fields." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditUser(string Id)
        {
            var user = userService.GetByUserId(Convert.ToString(Id));
            var entity = Mapper.Map<AspNetUser, RegisterModel>(user);
            MapDropdownListValues();
            return View(entity);
        }

        [HttpPost]
        [SessionExpireFilter]
        [DisableCache]
        public JsonResult EditUser(AspNetUser user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Id) || user.RoleId <= 0)
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "Form is not valid! " +
                          "Please correct it and try again."
                    });
                }
                var dbUser = userService.GetByUserId(user.Id);
                if (dbUser != null)
                {
                    dbUser.RoleId = user.RoleId;
                    dbUser.Email = user.Email;
                    userService.Update(dbUser);
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, new System.Transactions.TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }


        //
        // GET: /Account/Manage
        [SessionExpireFilter]
        [DisableCache]
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            // ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //[SessionExpireFilter]
        //[DisableCache]
        //public ActionResult RetypePassword(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
        //        : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
        //        : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //        : "";
        //    // ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //    ViewBag.ReturnUrl = Url.Action("RetypePassword");
        //    return View();
        //}
        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        [DisableCache]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[SessionExpireFilter]
        //[DisableCache]
        //public ActionResult RetypePassword(LocalPasswordModel model)
        //{
        //    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
        //    ViewBag.HasLocalPassword = hasLocalAccount;
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    if (hasLocalAccount)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // ChangePassword will throw an exception rather than return false in certain failure scenarios.
        //            bool changePasswordSucceeded;
        //            try
        //            {
        //                changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, "", model.NewPassword);
        //            }
        //            catch (Exception)
        //            {
        //                changePasswordSucceeded = false;
        //            }

        //            if (changePasswordSucceeded)
        //            {
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // User does not have a local password so remove any validation errors caused by a missing
        //        // OldPassword field
        //        ModelState state = ModelState["OldPassword"];
        //        if (state != null)
        //        {
        //            state.Errors.Clear();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
        //            }
        //            catch (Exception)
        //            {
        //                ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
        //            }
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilter]
        [DisableCache]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {

            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                // bool changePasswordSucceeded;
                try
                {

                    if (model.OldPassword == model.NewPassword)
                        return Json(new { Result = "ERROR", Message = "Old password and new password cannot be same" }, JsonRequestBehavior.AllowGet);


                    var userId = User.Identity.GetUserId();

                    var result = UserManager.ChangePassword(userId, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                        //changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                        //if (changePasswordSucceeded)
                        return Json(new { Result = "OK", Message = "Password changed successfully." }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { Result = "ERROR", Message = "Failed. " + string.Join(",", result.Errors) }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

            else
                return Json(new { Result = "ERROR", Message = "Please correct form date." }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[SessionExpireFilter]
        //[DisableCache]
        //public ActionResult ChangePassword(LocalPasswordModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        // ChangePassword will throw an exception rather than return false in certain failure scenarios.
        //       // bool changePasswordSucceeded;
        //        try
        //        {
        //            if(model.OldPassword==model.NewPassword)
        //                return Json(new { Result = "ERROR", Message = "Old password and new password cannot be same" }, JsonRequestBehavior.AllowGet);
        //            var userId = User.Identity.GetUserId();
        //            var result = UserManager.ChangePassword(userId, model.OldPassword, model.NewPassword);
        //            if (result.Succeeded)
        //            //changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
        //            //if (changePasswordSucceeded)
        //                return Json(new { Result = "OK", Message = "Password changed successfully." }, JsonRequestBehavior.AllowGet);
        //            else
        //                return Json(new { Result = "ERROR", Message = "Failed. "  + string.Join(",", result.Errors)}, JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //    else
        //        return Json(new { Result = "ERROR", Message = "Please correct form date." }, JsonRequestBehavior.AllowGet);
        //}

        //
        // POST: /Account/ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // User is new, ask for their desired membership name
                string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
                ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
                ViewBag.ReturnUrl = returnUrl;
                return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider = null;
            string providerUserId = null;

            if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                using (UsersContext db = new UsersContext())
                {
                    UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // Check if user already exists
                    if (user == null)
                    {
                        // Insert name into the profile table
                        db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
                        db.SaveChanges();

                        OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                        OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
                    }
                }
            }

            ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        private void LogRequest()
        {
            try
            {
                var logObject = Logger.GetLogObject();
                loggger.LogRequest(logObject);
            }
            catch (Exception ex)
            {

            }
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        private KeyValuePair<string, string> GetUrl(int id)
        {
            var urls = new Dictionary<int, KeyValuePair<string, string>>();
            urls.Add(1, new KeyValuePair<string, string>("Create", "Member"));
            urls.Add(2, new KeyValuePair<string, string>("Create", "LoanApproval"));
            urls.Add(3, new KeyValuePair<string, string>("Create", "Miscellaneou"));
            urls.Add(4, new KeyValuePair<string, string>("MemberBalanceInfoReport", "GroupwiseReport")); // Member balance.
            urls.Add(5, new KeyValuePair<string, string>("DailyRecoverableRecipt", "GroupwiseReport")); //top sheet
            urls.Add(6, new KeyValuePair<string, string>("LoanLedgerMemberwise", "GroupwiseReport")); //Loan Ledger
            urls.Add(7, new KeyValuePair<string, string>("SavingLedgerMemberwise", "GroupwiseReport")); //Savings Ledger
            urls.Add(8, new KeyValuePair<string, string>("Create", "SavingsAccountOpening")); //Savings account opening.
            urls.Add(999, new KeyValuePair<string, string>("Index", "Home")); //Savings account opening.
            if (urls.ContainsKey(id))
                return urls[id];
            else
                return new KeyValuePair<string, string>("", "");

        }

        /*
        #region ForgotPassword

        // [Route("ForgotPassword")]

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPasswordResetLink(string EmailID)
        {
            string message = "";
            var updateResetPasswordCode = userService.GetMany(p => p.Email == EmailID).FirstOrDefault();
            if (updateResetPasswordCode != null)
            {
                //using (gBankerDbContext dc = new gBankerDbContext())
                //{
                var account = userService.GetMany(a => a.Email == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //var resetCode = UserManager.GenerateUserToken("Reset_Password", account.Id);
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    //dc.Configuration.ValidateOnSaveEnabled = false;
                    if (account.ResetPasswordCode == null)
                        //dc.SaveChanges();
                        userService.Create(account);
                    else
                    {
                        updateResetPasswordCode.ResetPasswordCode = resetCode;
                        userService.Update(updateResetPasswordCode);
                    }
                    message = "Reset password link has been sent to your registered Email Account.";
                }
                else
                {
                    message = "Registered Email not found";
                }
                //}
            }
            else
            {
                message = "Registered Email not found";
            }
            ViewBag.Message = message;
            return Json(new { message = message }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Account/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var toAddress = emailID;

            string subject = "";
            string mailBody = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                mailBody = "<br/>" +
                    "<br/>We are excited to tell you that your Dotnet Awesome account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                mailBody = "Hi,<br/>We got request for reset your account password. Please click on the below link to reset your password." +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }
            SendMail(toAddress, mailBody, subject);
        }

        private void SendMail(string toAddress, string mailBody, string subject)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(toAddress);
            const string style = "<p style=" + "font-family:Cambria;font-size:11pt" + ">";//Calibri
            mailMessage.Body = style + mailBody + "</p>";
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            var fromMessage = ConfigurationManager.AppSettings["messageFrom"];
            mailMessage.From = new MailAddress(fromMessage);
            var client = new SmtpClient();
            var smtpHost = ConfigurationManager.AppSettings["smtpHost"];
            client.Host = smtpHost; //Set your smtp host address

            var portId = ConfigurationManager.AppSettings["portID"];

            client.Port = Convert.ToInt32(portId); // Set your smtp port address
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;  //new added
            var credentialId = ConfigurationManager.AppSettings["credentialID"];
            var credentialPassword = ConfigurationManager.AppSettings["credentialPassword"];
            client.Credentials = new NetworkCredential(credentialId, credentialPassword); //account name and password
            client.Send(mailMessage);
        }
        public ActionResult ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (gBankerDbContext dc = new gBankerDbContext())
            {
                var user = dc.AspNetUsers.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public JsonResult SetNewPassword(ResetPasswordModel model)
        {
            var message = "";
            int result = 0;

            var user = userService.GetByToken(model.ResetCode);
            if (user != null)
            {
                UserManager.RemovePassword(user.Id);
                UserManager.AddPassword(user.Id, model.ConfirmPassword);
                result = 1;
                message = "New password updated successfully";
            }
            ViewBag.Message = message;
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion ForgotPassword

        [HttpGet]
        public JsonResult GetForgotPaswordEncyption(string userCode)
        {
            var forgotPaswordEncyption = "";
            try
            {
                if (string.IsNullOrEmpty(userCode))
                    return Json(new { forgotPaswordEncyption }, JsonRequestBehavior.AllowGet);

                var secrectKey = ForgotPasswordEncDecKey.SecrectKey;
                forgotPaswordEncyption = DataEncrypterDecrypter.Encrypt(userCode, secrectKey);
                return Json(new { forgotPaswordEncyption }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { forgotPaswordEncyption }, JsonRequestBehavior.AllowGet);
            }
        }
        */
    }
}

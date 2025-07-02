using gBanker.Web.Helpers;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web.Security;
using gBanker.Data.CodeFirstMigration;
using System.Collections.Generic;
using System;
using System.Linq;
namespace gBanker.Web.Filters
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {

        private IAuthenticationManager _authnManager;
        private ILogger _logObject;
        // Modified this from private to public and add the setter
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                if (_authnManager == null)
                    _authnManager = HttpContext.Current.GetOwinContext().Authentication;
                return _authnManager;
            }
            set { _authnManager = value; }
        }
        public ILogger LogObject
        {
            get
            {
                if (_logObject == null)
                    _logObject = DependencyResolver.Current.GetService<ILogger>();
                return _logObject;
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var loggingObject = Logger.GetLogObject();
                LogObject.LogRequest(loggingObject);
            }
            catch (Exception ex)
            {
                //Send email that logger is not working....
            }
            HttpContext ctx = HttpContext.Current;
            // check if session is supported
            if (ctx.Session != null)
            {
                // check if a new session id was generated
                if (ctx.Session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must
                    // have timed out
                    //  string sessionCookie = ctx.Request.Headers["Cookie"];
                    string sessionCookie = ctx.Request.Headers["Cookie"];
                    if (null != sessionCookie)
                    {
                        FormsAuthentication.SignOut();
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                        ctx.Response.Redirect("~/Account/Login");
                    }
                }
                else if (!filterContext.HttpContext.Request.IsAjaxRequest())
                    EnsureRequestIsAuthorized();
            }
            base.OnActionExecuting(filterContext);
        }

        private void EnsureRequestIsAuthorized()
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                var userModules = SessionHelper.UserSecurityModules;
                if (userModules != null)
                {
                    var rd = HttpContext.Current.Request.RequestContext.RouteData;
                    string currentAction = rd.GetRequiredString("action");
                    string currentController = rd.GetRequiredString("controller");
                    var isAuthorized = userModules.Where(w => w.ControllerName == currentController).FirstOrDefault();

                    if (string.IsNullOrEmpty(currentAction))
                        currentAction = "Index";
                    var currentModule = userModules.Where(w => w.ControllerName.ToLower() == currentController.ToLower() && (w.ActionName.ToLower() == currentAction.ToLower() || w.ActionName.ToLower() == "index")).FirstOrDefault();

                    if (currentModule != null)
                    {
                        var id1 = currentModule.AspNetSecurityModuleId.ToString();
                        var id2 = "-1";
                        var id3 = "-1";
                        if (currentModule.ParentModuleId.HasValue)
                        {
                            var level2Parent = currentModule.ParentModuleId.Value;
                            id2 = level2Parent.ToString();
                            var level2Module = userModules.Where(u => u.AspNetSecurityModuleId == level2Parent).FirstOrDefault();
                            if (level2Module != null && level2Module.ParentModuleId.HasValue)
                                id3 = level2Module.ParentModuleId.Value.ToString();
                        }
                        var ids = string.Format("{0}_{1}_{2}", id3, id2, id1);
                        SessionHelper.CurrentModuleKeys = ids;
                    }
                    else
                    {
                        var AllModules = SessionHelper.AllModules;
                        var AllcurrentModule = AllModules.Where(w => w.ControllerName.ToLower() == currentController.ToLower() && (w.ActionName.ToLower() == currentAction.ToLower() || w.ActionName == "Index")).FirstOrDefault();
                        if (AllcurrentModule != null && !ExcludeScurity(currentController, currentAction))
                        {
                            HttpContext.Current.Session["UNAUTHORIZED_ACCES"] = "You are not authorized to access the specific action.";
                            throw new UnauthorizedAccessException("You are not authorized to access the specific action.");
                        }

                    }
                    //var isAuthorized = userModules.Where(w => w.ControllerName == currentController && (w.ActionName == currentAction || w.ActionName == "Index")).FirstOrDefault();
                    //if (isAuthorized == null && !(currentController == "Home" && currentAction == "Index"))
                    //{
                    //    HttpContext.Current.Session["UNAUTHORIZED_ACCES"] = "You are not authorized to access the specific action.";
                    //    throw new UnauthorizedAccessException("You are not authorized to access the specific action.");
                    //}


                    ////var isAuthorized = userModules.Where(w => w.ControllerName == currentController && (w.ActionName == currentAction || w.ActionName == "Index")).FirstOrDefault();
                    //isAuthorized = userModules.Where(w => w.ControllerName == currentController && (w.ActionName == currentAction)).FirstOrDefault();
                    //if (isAuthorized == null && !(currentController == "Home" && currentAction == "Index"))
                    //{
                    //    HttpContext.Current.Session["UNAUTHORIZED_ACCES"] = "You are not authorized to access the specific action.";
                    //    throw new UnauthorizedAccessException("You are not authorized to access the specific action.");
                    //}
                }
            }
        }
        private bool ExcludeScurity(string controller, string action)
        {
            Dictionary<string, List<string>> controlers = new Dictionary<string, List<string>>()

            {
                { "LoanSavingLedger", new List<string>() {"","","","" }},
                { "GroupwiseReport", new List<string>() { }},

                { "KhatwaryReport", new List<string>() { }},
                 { "LoanInstallmentCorrection", new List<string>() { }},
                  { "MonthlyCollectionSheet", new List<string>() { }},
                   { "CreditScore", new List<string>() { }},
                   { "WeeklyCollectionSheet", new List<string>() { }},
                   { "FullyRepaid", new List<string>() { }},
                    { "WeeklySamityWiseReport", new List<string>() { }},
                      { "WeeklyMonitoringReport", new List<string>() { }},
                      { "MonthlyStatisticalReport", new List<string>() { }},
                       { "MonthlyProjectStatementReport", new List<string>() { }},
                        { "OverdueMemberlist", new List<string>() { }},
                        { "POMIS5A", new List<string>() { }},
                          { "POMIS", new List<string>() { }},
                           { "MRA", new List<string>() { }},
                            { "MRAMIS", new List<string>() { }},
                             { "MraCdb", new List<string>() { }},
                              { "AccTrialBalance", new List<string>() { }},
                             { "AccRcvPayReport", new List<string>() { }},
                              { "AccBalanceSheet", new List<string>() { }},
                            { "AccIncExpReport", new List<string>() { }},
                            { "WriteOffList", new List<string>() { }},
                            { "SSRSReport", new List<string>() { }},
                            { "SavingInterestUpdate", new List<string>() { }},
                             { "MonthClosing", new List<string>() {"","","" }},
                             { "ExcelMigration", new List<string>() {"","","" }},
            };

            var exist = controlers.Count(w => w.Key.ToLower() == controller.ToLower()) > 0;
            return exist;
        }

    }
}
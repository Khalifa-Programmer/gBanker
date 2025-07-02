using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using gBanker.Web.Filters;
using gBanker.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using gBanker.Data;
using gBanker.Service;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using AutoMapper;
using System.Text;

namespace gBanker.Web.Helpers
{
    public class SessionHelper
    {
        public static bool IsAuthenticated
        {
            get { return HttpContext.Current.Request.IsAuthenticated; }
        }
        public static void LogSessionInfo(EmployeeViewModel employee, List<AspNetSecurityModule> parentModules, List<AspNetSecurityModule> userSecurityModules, List<AspNetSecurityModule> AllModules)
        {
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[SessionKeys.LOGGEDIN_EMPLOYEE] = employee;
                HttpContext.Current.Session[SessionKeys.PARENT_MODULES] = parentModules;
                HttpContext.Current.Session[SessionKeys.USER_SECURITY_MODULES] = userSecurityModules;
                HttpContext.Current.Session[SessionKeys.USER_SECURITY_AllMODULES] = AllModules;               
            }
        }
        public static EmployeeViewModel LoggedInEmployee
        {
            get
            {
                if (IsAuthenticated && HttpContext.Current.Session[SessionKeys.LOGGEDIN_EMPLOYEE] != null)
                    return HttpContext.Current.Session[SessionKeys.LOGGEDIN_EMPLOYEE] as EmployeeViewModel;
                else
                    return null;
            }
        }
        public static string ServiceMessage
        {
            get { return HttpContext.Current.Session[SessionKeys.SERVICE_MESSAGE_KEYS] as string; }
            set { HttpContext.Current.Session[SessionKeys.SERVICE_MESSAGE_KEYS] = value; }
        }

        public static OfficeViewModel LoggedInOfficeDetail
        {
            get
            {
                return HttpContext.Current.Session[SessionKeys.LOGGED_IN_OFFICE_DETAIL] as OfficeViewModel;

            }

            set { HttpContext.Current.Session[SessionKeys.LOGGED_IN_OFFICE_DETAIL] = value; }
        }
        public static List<AspNetSecurityModule> AllPrentModules
        {
            get
            {
                if (IsAuthenticated && HttpContext.Current.Session[SessionKeys.PARENT_MODULES] != null)
                    return HttpContext.Current.Session[SessionKeys.PARENT_MODULES] as List<AspNetSecurityModule>;
                else
                    return null;
            }
        }
        public static List<AspNetSecurityModule> AllModules
        {
            get
            {
                if (IsAuthenticated && HttpContext.Current.Session[SessionKeys.USER_SECURITY_AllMODULES] != null)
                    return HttpContext.Current.Session[SessionKeys.USER_SECURITY_AllMODULES] as List<AspNetSecurityModule>;
                else
                    return null;
            }
        }
        public static List<AspNetSecurityModule> UserSecurityModules
        {
            get
            {
                if (IsAuthenticated && HttpContext.Current.Session[SessionKeys.USER_SECURITY_MODULES] != null)
                    return HttpContext.Current.Session[SessionKeys.USER_SECURITY_MODULES] as List<AspNetSecurityModule>;
                else
                    return null;
            }
        }
        public static int? LoginUserOfficeID
        {
            get { return HttpContext.Current.Session[SessionKeys.LOGGED_IN_OFFICE_ID] as int?; }
            set { HttpContext.Current.Session[SessionKeys.LOGGED_IN_OFFICE_ID] = value; }
        }
        public static int? LoginUserEmployeeID { get { return LoggedInEmployee == null ? default(System.Nullable<int>) : LoggedInEmployee.EmployeeID; } }
        public static string UserFullName
        {
            get { return LoggedInEmployee == null ? "" : LoggedInEmployee.EmpName; }
        }
        public static int? LoggedInEmployeeID { get { return LoggedInEmployee == null ? default(System.Nullable<int>) : LoggedInEmployee.EmployeeID; } }
        public static DateTime TransactionDate
        {
            get { return DateTime.Parse(HttpContext.Current.Session[SessionKeys.TRANSACTION_DATE].ToString()); }
            set { HttpContext.Current.Session[SessionKeys.TRANSACTION_DATE] = value; }
        }
        public static DateTime? LastDayEndDate
        {

            get
            {
                if (HttpContext.Current.Session[SessionKeys.LASTDAYEND_DATE] != null)
                    return DateTime.Parse(HttpContext.Current.Session[SessionKeys.LASTDAYEND_DATE].ToString());
                else
                {
                    return default(DateTime?);
                }
            }
            set { HttpContext.Current.Session[SessionKeys.LASTDAYEND_DATE] = value; }
        }
        public static string OrganizationName
        {
            get { return HttpContext.Current.Session[SessionKeys.ORGANIZATION_NAME] == null ? "" : HttpContext.Current.Session[SessionKeys.ORGANIZATION_NAME].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORGANIZATION_NAME] = value; }
        }
        public static string OrganizationAddress
        {
            //get { return HttpContext.Current.Session[SessionKeys.ORGANIZATION_Address].ToString(); }
            //set { HttpContext.Current.Session[SessionKeys.ORGANIZATION_Address] = value; }
            get { return HttpContext.Current.Session[SessionKeys.ORGANIZATION_Address] == null ? "" : HttpContext.Current.Session[SessionKeys.ORGANIZATION_Address].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORGANIZATION_Address] = value; }
        }
        public static string ProcessType
        {
            get { return HttpContext.Current.Session[SessionKeys.PROCESS_TYPE].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.PROCESS_TYPE] = value; }
        }
        public static string TransactionDay
        {
            get { return HttpContext.Current.Session[SessionKeys.TRANSACTION_DAY].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.TRANSACTION_DAY] = value; }
        }
        public static bool IsDayInitiated
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.IS_DAY_INITIATED] != null)
                    return (bool)HttpContext.Current.Session[SessionKeys.IS_DAY_INITIATED];
                return false;

            }
            set { HttpContext.Current.Session[SessionKeys.IS_DAY_INITIATED] = value; }
        }
        public static string TransactionDashBoardString
        {
            get
            {
                var detail = new StringBuilder();
                if (IsDayInitiated && HttpContext.Current.Session[SessionKeys.TRANSACTION_DATE] != null && TransactionDate != default(DateTime))
                    detail.Append(string.Format("Transaction Date:{0} | Day:{1} | ", TransactionDate.ToString("dd MMM, yyyy"), TransactionDay));
                else
                    detail.Append(" No Transaction Day Initiated | ");
                if (HttpContext.Current.Session[SessionKeys.LASTDAYEND_DATE] != null && LastDayEndDate != default(DateTime?))
                    detail.Append(string.Format(" Last Day End:{0} | ", LastDayEndDate.Value.ToString("dd MMM, yyyy")));
                if (LoggedInOfficeDetail != null)
                    detail.Append(string.Format(" Office: {0} - {1} ", LoggedInOfficeDetail.OfficeCode, LoggedInOfficeDetail.OfficeName));

                // Working Office:<span id="officeName"> @string.Format("{0} - {1}", gBanker.Web.Helpers.SessionHelper.LoggedInOfficeDetail.OfficeCode, gBanker.Web.Helpers.SessionHelper.LoggedInOfficeDetail.OfficeName)</span>
                return detail.ToString();
            }
        }
        public static string getOfficeName
        {
            get
            {
                var detail = new StringBuilder();

                if (LoggedInOfficeDetail != null)
                    detail.Append(string.Format(" Office: {0} - {1} ", LoggedInOfficeDetail.OfficeCode, LoggedInOfficeDetail.OfficeName));
                    return detail.ToString();
            }
        }
        public static int? LoginUserOrganizationID
        {
            get { return LoggedInEmployee == null ? default(System.Nullable<int>) : LoggedInEmployee.OrgID; }
        }

        public static string LoggedInOrganizationCode
        {
            get { return HttpContext.Current.Session[SessionKeys.ORGANIZATION_CODE] == null ? "" : HttpContext.Current.Session[SessionKeys.ORGANIZATION_CODE].ToString(); }
            set { HttpContext.Current.Session[SessionKeys.ORGANIZATION_CODE] = value; }
        }

        public static string CurrentModuleKeys
        {
            get { return HttpContext.Current.Session[SessionKeys.CURRENT_MODULE_KEYS] as string; }
            set { HttpContext.Current.Session[SessionKeys.CURRENT_MODULE_KEYS] = value; }
        }
        public static int LoginUserRoleId
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.Session[SessionKeys.LOGGED_IN_USER_ROLE] != null)
                    return (int)HttpContext.Current.Session[SessionKeys.LOGGED_IN_USER_ROLE];
                else
                    return -1;
            }
        }

    }
    public class SessionKeys
    {
        public const string LOGGEDIN_EMPLOYEE = "LOGGEDIN_EMPLOYEE";
        public const string USER_CENTER_ID = "USER_CENTER_ID";
        public const string TRANSACTION_DATE = "TRANSACTION_DATE";
        public const string TRANSACTION_DAY = "TRANSACTION_DAY";
        public const string IS_DAY_INITIATED = "IS_DAY_INITIATED";
        public const string ORGANIZATION_CODE = "ORGANIZATION_CODE";
        public const string ORGANIZATION_NAME = "ORGANIZATION_NAME";
        public const string ORGANIZATION_Address = "ORGANIZATION_Address";
        public const string PROCESS_TYPE = "PROCESS_TYPE";
        public const string PARENT_MODULES = "PARENT_MODULES";
        public const string USER_SECURITY_MODULES = "ROLE_MODULES";
        public const string LOGGED_IN_OFFICE_ID = "LOGGED_IN_OFFICE_ID";
        public const string LOGGED_IN_OFFICE_DETAIL = "LOGGED_IN_OFFICE_DETAIL";
        public const string LASTDAYEND_DATE = "LASTDAYEND_DATE";
        public const string ORGANIZATION_ID = "ORGANIZATION_ID";
        public const string CURRENT_MODULE_KEYS= "CURRENT_MODULE_KEYS";
        public const string USER_SECURITY_AllMODULES = "USER_SECURITY_AllMODULES";
        public const string LOGGED_IN_USER_ROLE = "LOGGED_IN_USER_ROLE";
        public const string LOGGED_IN_Employee_Office_Level = "LOGGED_IN_Employee_Office_Level"; //khalid

        public const string SERVICE_MESSAGE_KEYS = "SERVICE_MESSAGE_KEYS";
    }

}


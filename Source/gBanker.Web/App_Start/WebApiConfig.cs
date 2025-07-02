using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

//namespace gBanker.Web
//{
//    public static class WebApiConfig
//    {
//        public static void Register(HttpConfiguration config)
//        {
//            config.Routes.MapHttpRoute(
//                name: "DefaultApi",
//                routeTemplate: "api/{controller}/{action}",
//                defaults: new { action = RouteParameter.Optional }
//            );

//            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
//            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
//            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
//            //config.EnableQuerySupport();
//        }
//    }
//}



namespace gBanker.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "LoginApi",
                routeTemplate: "api/login",
                defaults: new { controller = "Authenticate", action = "login" }
            );

            // Member
            config.Routes.MapHttpRoute(
                name: "MemberApproveAPI",
                routeTemplate: "api/member/approve",
                defaults: new { controller = "Member", action = "ApproveMember" }
            );

            config.Routes.MapHttpRoute(
                name: "MemberListAPI",
                routeTemplate: "api/member/list",
                defaults: new { controller = "Member", action = "GetMembers" }
            );

            config.Routes.MapHttpRoute(
                name: "MemberImgSyncAPI",
                routeTemplate: "api/member/sync-img",
                defaults: new { controller = "Member", action = "SyncMemberImg" }
            );

            config.Routes.MapHttpRoute(
                name: "MemberAddAPI",
                routeTemplate: "api/member/new",
                defaults: new { controller = "Member", action = "AddMember" }
            );

            config.Routes.MapHttpRoute(
                name: "MemberIdentityCheckAPI",
                routeTemplate: "api/member/check-identity",
                defaults: new { Controller = "Member", action = "CheckMemberIdentity" }
            );

            config.Routes.MapHttpRoute(
                name: "GetMemberProductsForApproval",
                routeTemplate: "api/member/get-approval-products",
                defaults: new { Controller = "Member", action = "GetMemberProductsForApproval" }
            );

            // END Member

            config.Routes.MapHttpRoute(
                name: "SavingAccountsPostToLedger",
                routeTemplate: "api/saving-accounts/approve",
                defaults: new { Controller = "SavingsAccount", action = "ApproveAccounts" }
            );

            // Loan proposal 
            config.Routes.MapHttpRoute(
                name: "LoanProposalValidateTerm",
                routeTemplate: "api/loan-proposal/validate-term",
                defaults: new { Controller = "LoanProposal", action = "validateTerm" }
            );

            config.Routes.MapHttpRoute(
                name: "LoanProposalList",
                routeTemplate: "api/loan-proposal/list",
                defaults: new { Controller = "LoanProposal", action = "getLoanProposals" }
            );

            config.Routes.MapHttpRoute(
                name: "LoanProposalSave",
                routeTemplate: "api/loan-proposal/save",
                defaults: new { Controller = "LoanProposal", action = "saveLoanProposal" }
            );

            config.Routes.MapHttpRoute(
                name: "LoanProposalEligible",
                routeTemplate: "api/loan-proposal/approve",
                defaults: new { Controller = "LoanProposal", action = "approveLoanProposal" }
            );
            config.Routes.MapHttpRoute(
                name: "LoanProposalDisburse",
                routeTemplate: "api/loan-proposal/disburse",
                defaults: new { Controller = "LoanProposal", action = "disburseLoanProposal" }
            );
            config.Routes.MapHttpRoute(
                name: "LoanProposalDelete",
                routeTemplate: "api/loan-proposal/delete",
                defaults: new { Controller = "LoanProposal", action = "deleteLoanProposal" }
            );

            // End Loan Proposal


            // Cs Refund

            config.Routes.MapHttpRoute(
                name: "CsRefundCalculateAndSave",
                routeTemplate: "api/cs-refund/calculate",
                defaults: new { Controller = "CsRefund", action = "GetCalculatedResult" }
            );

            // End Cs Refund

            // Rebate

            config.Routes.MapHttpRoute(
                name: "RebateCalc",
                routeTemplate: "api/rebate/calculate",
                defaults: new { Controller = "Rebate", action = "rebateCalculate" }
            );

            // End Rebate

            // Misc

            config.Routes.MapHttpRoute(
                name: "MiscSave",
                routeTemplate: "api/misc/save",
                defaults: new { Controller = "Misc", action = "saveMisc" }
            );
            config.Routes.MapHttpRoute(
                name: "MiscDelete",
                routeTemplate: "api/misc/delete",
                defaults: new { Controller = "Misc", action = "deleteMisc" }
            );

            // End Misc

            config.Routes.MapHttpRoute(
                name: "SyncdataApi",
                routeTemplate: "api/syncdata",
                defaults: new { controller = "Lookup", action = "GetOfficeSyncData" }
            );

            config.Routes.MapHttpRoute(
                name: "PostMobileDataAPI",
                routeTemplate: "api/post-mobile-collection",
                defaults: new { controller = "UploadCollection", action = "PostMobileCollection" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { action = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();
        }
    }
}
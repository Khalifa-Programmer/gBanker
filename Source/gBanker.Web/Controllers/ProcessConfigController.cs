using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gBanker.Web.Helpers;
using System.Threading.Tasks;

namespace gBanker.Web.Controllers
{
    public class ProcessConfigController : BaseController
    {

        #region variables
        private readonly IGroupwiseReportService groupwiseReportService;

        public ProcessConfigController(IGroupwiseReportService groupwiseReportService)
        {
            this.groupwiseReportService = groupwiseReportService;
             
        }

        #endregion


        //
        // GET: /ProcessConfig/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChkAutoVoucherProcess()
        {
            return View();
        }

        #region Methods
        public JsonResult GetCurrentSetting(int id)
        {
            List<ProcessConfigViewModel> List_ViewModel = new List<ProcessConfigViewModel>();
            
             var param = new { @Processid = id};
            var empList = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetCurrentSetting");

            //var param = new { OfficeId = LoginUserOfficeID, MemberId = Member_id };
            //var div_items = ultimateReportService.GetMemberPasBookList(param);

              //var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
              //  var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberList_Dropdown_LoanSummary");

             List_ViewModel = empList.Tables[0].AsEnumerable()
               .Select(row => new ProcessConfigViewModel
               {
                   Id = row.Field<int>("id"),
                   startWorkProcess = row.Field<string>("Time"),
                   KActive = row.Field<Boolean>("IsActive")

               }).ToList();
            return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
        }//End Function

        public JsonResult UpdateData(
             

            //////////////////////////Bappa Test
            string startWorkTime,
            string IsStartWorkAuto,
             
            string completeWorkTime,
            string IsCompleteWorkAuto,

            string MonthClosingDate,
            string IsMonthClosing
            
            )
        {
            string result = "OK";
            var param = new { 
                @startWorkTime	    = 	startWorkTime       ,
                @IsStartWorkAuto	=   IsStartWorkAuto     ,
                
                @completeWorkTime	=   completeWorkTime    ,
                @IsCompleteWorkAuto =   IsCompleteWorkAuto  ,
                
                @MonthClosingDate	=   MonthClosingDate    ,
                @IsMonthClosing	    =   IsMonthClosing
            
             
            };
            var empList = groupwiseReportService.GetDataUltimateReleaseReport(param, "UpdateProcessConfig");

            
            return Json(result, JsonRequestBehavior.AllowGet);
        }//End Function





        public JsonResult GetMessage(int OfficeID)
        {
            string result = string.Empty;
            try
            {
                var filter = new { OfficeId = SessionHelper.LoginUserOfficeID };
                var filteredList = groupwiseReportService.GetDataUltimateReleaseReport(filter, "getAutoVoucherProcessCheck");

                var message = filteredList.Tables[0].Rows[0]["status"].ToString();

                result = message;
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }// End of Message


        #endregion



    }// End of class
}// End of Namespace
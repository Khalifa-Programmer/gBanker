using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class POMISController : BaseController
    {
        #region Variables
        private readonly IPOMISReportService pomisReportService;
        private readonly IMISConsolidationProcessService iMISConsolidationProcessService;

        public POMISController(IPOMISReportService pomisReportService, IMISConsolidationProcessService iMISConsolidationProcessService)
        {
            this.pomisReportService = pomisReportService;
            this.iMISConsolidationProcessService = iMISConsolidationProcessService;
        }
        #endregion

        #region Methods
        public JsonResult SavingProcess(string ProcessDate)
        {
            var result = 0;

            var param = new { OrgID = Convert.ToInt32(SessionHelper.LoginUserOrganizationID), BranchID = Convert.ToInt32(SessionHelper.LoginUserOfficeID), ProcessDate = Convert.ToDateTime(ProcessDate) };
            var POMIS = pomisReportService.POMIS2SavingStatement(param);
            result = 1;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoanstatementProcess(string ProcessDate)
        {
            var result = 0;
            var param = new { Org = Convert.ToInt32(SessionHelper.LoginUserOrganizationID), BranchID = Convert.ToInt32(SessionHelper.LoginUserOfficeID), ProcessDate = Convert.ToDateTime(ProcessDate) };
            var POMIS = pomisReportService.POMISLoanStatementHO(param);


            var param2 = new { Org = Convert.ToInt32(SessionHelper.LoginUserOrganizationID), BranchID = Convert.ToInt32(SessionHelper.LoginUserOfficeID), ProcessDate = Convert.ToDateTime(ProcessDate) };
            var POMIS2 = pomisReportService.POMISLoanStatement(param2);
            result = 2;
            result = 2;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult overdueProcess(string ProcessDate)
        {
            var result = 0;

            // var param = new { LastDate=Convert.ToDateTime(ProcessDate), OrgID = SessionHelper.LoginUserOrganizationID, OfficeIDSelect = 00 , DateFrom = Convert.ToDateTime(ProcessDate) , DateTo = Convert.ToDateTime(ProcessDate) , OfficeID =00};
            //var param = new {OrgID = SessionHelper.LoginUserOrganizationID, OfficeID = 00, OfficeIDTO = 99999, CenterID=00, CenterIDTo=99999,
            //    StaffID=0000,StaffIDTo=99999,productID=00000,ProductIDTo=99999,DateFrom = Convert.ToDateTime(ProcessDate), DateTo = Convert.ToDateTime(ProcessDate),
            //    Qtype=1
            //};
            var param = new
            {

                OfficeID = 00,
                OfficeIDTO = 99999,
                CenterID = 00,
                CenterIDTo = 99999,
                StaffID = 0000,
                StaffIDTo = 99999,
                productID = 00000,
                ProductIDTo = 99999,
                DateFrom = Convert.ToDateTime(ProcessDate),
                DateTo = Convert.ToDateTime(ProcessDate),
                OrgID_PO = SessionHelper.LoginUserOrganizationID
            };
            var POMIS = pomisReportService.OverDueAgeingConsolidation(param);

            result = 3;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult overdueProcessDueType(string ProcessDate)
        {
            var result = 0;

            var param = new { OrgID = SessionHelper.LoginUserOrganizationID, DateFrom = Convert.ToDateTime(ProcessDate), DateTo = Convert.ToDateTime(ProcessDate) };
            result = 4;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult StaffProfile(string ProcessDate)
        {
            var result = 0;
            var param = new { Org = Convert.ToInt32(SessionHelper.LoginUserOrganizationID), Office = Convert.ToInt32(SessionHelper.LoginUserOfficeID), DateTo = Convert.ToDateTime(ProcessDate) };
            var POMIS = pomisReportService.POMISStaffProfile(param);

            result = 5;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RunPMOISProcess(string checkInfo, string ProcessDate)
        {
            var result = 0;
            if (checkInfo == "1")
            {
                var param = new { OrgID = Convert.ToInt32(SessionHelper.LoginUserOrganizationID), BranchID = Convert.ToInt32(SessionHelper.LoginUserOfficeID), ProcessDate = Convert.ToDateTime(ProcessDate) };
                var POMIS = pomisReportService.POMIS2SavingStatement(param);
                result = 1;
            }
            else if (checkInfo == "2")
            {
                var param2 = new { Org = Convert.ToInt32(SessionHelper.LoginUserOrganizationID), BranchID = Convert.ToInt32(SessionHelper.LoginUserOfficeID), ProcessDate = Convert.ToDateTime(ProcessDate) };
                var POMIS2 = pomisReportService.POMISLoanStatementHO(param2);


                var param = new { Org = Convert.ToInt32(SessionHelper.LoginUserOrganizationID), BranchID = Convert.ToInt32(SessionHelper.LoginUserOfficeID), ProcessDate = Convert.ToDateTime(ProcessDate) };
                var POMIS = pomisReportService.POMISLoanStatement(param);
                result = 2;
            }
            else if (checkInfo == "3")
            {
                // var param = new { LastDate = Convert.ToDateTime(ProcessDate), OrgID_PO = SessionHelper.LoginUserOrganizationID, OfficeIDSelect = 00, DateFrom = Convert.ToDateTime(ProcessDate), DateTo = Convert.ToDateTime(ProcessDate), OfficeID = 99999 };
                var param = new
                {

                    OfficeID = 00,
                    OfficeIDTO = 99999,
                    CenterID = 00,
                    CenterIDTo = 99999,
                    StaffID = 0000,
                    StaffIDTo = 99999,
                    productID = 00000,
                    ProductIDTo = 99999,
                    DateFrom = Convert.ToDateTime(ProcessDate),
                    DateTo = Convert.ToDateTime(ProcessDate),
                    OrgID_PO = SessionHelper.LoginUserOrganizationID
                };
                var POMIS = pomisReportService.OverdueClassificationConsolidation(param);
                result = 3;
            }
            else if (checkInfo == "4")
            {
                var param = new { OrgID = SessionHelper.LoginUserOrganizationID, DateFrom = Convert.ToDateTime(ProcessDate), DateTo = Convert.ToDateTime(ProcessDate) };
                result = 4;
            }
            else if (checkInfo == "5")
            {
                var param = new { Org = Convert.ToInt32(SessionHelper.LoginUserOrganizationID), Office = Convert.ToInt32(SessionHelper.LoginUserOfficeID), DateTo = Convert.ToDateTime(ProcessDate) };
                var POMIS = pomisReportService.POMISStaffProfile(param);
                result = 5;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveMISConsolidationProcess(short queueNo, string ProcessDate)
        {
            var message = "";
            short? consoQueueNo;
            var officeId = LoginUserOfficeID;

            var checkIsRunning = iMISConsolidationProcessService.GetMany(p => p.IsRunning == true);
            if (checkIsRunning.Any())
            {
                message = "At a time one process is runnning, decline another process. Please try again later.";
                return Json(new { message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var entity = new MISConsolidationProcess();
                entity.ConsoQueue = queueNo;
                entity.OfficeID = officeId;
                entity.ProcessDate = Convert.ToDateTime(ProcessDate);
                entity.CreateUser = LoggedInEmployeeID.ToString();
                entity.CreateDate = DateTime.Now;
                entity.IsRunning = true;
                consoQueueNo = iMISConsolidationProcessService.Create(entity).ConsoQueue;
            }
            return Json(consoQueueNo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateQueueStatus(short QueueNo)
        {
            var getData = iMISConsolidationProcessService.GetMany(p => p.ConsoQueue == QueueNo).FirstOrDefault();
            if (getData != null)
                getData.IsRunning = false;
            iMISConsolidationProcessService.Update(getData);
            var message = "Process done successfully";
            return Json(new { message = message }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GeneratePOMIS1Report(string Date)
        {
            try
            {

                var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Org = SessionHelper.LoginUserOrganizationID };

                var POMIS1As = pomisReportService.GetDataPOMIS1AReport(param);
                var POMIS1Bs = pomisReportService.GetDataPOMIS1BReport(param);
                var POMIS1Cs = pomisReportService.GetDataPOMIS1CReport(param);
                var POMIS1Ds = pomisReportService.GetDataPOMIS1DReport(param);

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("SubRptSavingsStatement", POMIS1Bs.Tables[0]);
                subReportDB.Add("ItemWiseTotal", POMIS1Cs.Tables[0]);
                subReportDB.Add("SubRptMemberAdmission", POMIS1Ds.Tables[0]);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintWithSubReport("rpt_POMIS1_SavingsStatement.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_POMIS1_SavingsStatement());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //public ActionResult GeneratePOMIS1Report(string Date)
        //{
        //    try
        //    {

        //        var param = new { OfficeId = SessionHelper.LoginUserOfficeID, MemberID = 1, ProductId = 1, LoanTerm = 1 };

        //        var POMIS1As = pomisReportService.RepaymentScheduleRegister_1(param);
        //        var POMIS1Bs = pomisReportService.RepaymentScheduleRegister_2(param);

        //        var POMIS1Cs = pomisReportService.RepaymentScheduleRegister_3(param);
        //        //var POMIS1Ds = pomisReportService.GetDataPOMIS1DReport(param);

        //        var subReportDB = new Dictionary<string, DataTable>();
        //        subReportDB.Add("getRepaymentScheduleRegister_1", POMIS1Bs.Tables[0]);
        //        subReportDB.Add("getRepaymentScheduleRegister_2", POMIS1Cs.Tables[0]);
        //        //subReportDB.Add("SubRptMemberAdmission", POMIS1Ds.Tables[0]);

        //        var reportParam = new Dictionary<string, object>();
        //        //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        ReportHelper.PrintWithSubReport("RepaymentScheduleRegister_Sub.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new RepaymentScheduleRegister_Sub());
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GeneratePOMIS_5_AReport(string Date, string Qtype)
        {
            try
            {

                var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID };
                var POMIS5As = pomisReportService.GetDataPOMIS_5_AReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", Date);


                if (Qtype == "2")
                {
                    ReportHelper.PrintReport("POMIS_5_A_3_Final.rpt", POMIS5As.Tables[0], reportParam);
                }
                else if (Qtype == "3")
                {
                    ReportHelper.PrintReport("POMIS_5_A_4_Final.rpt", POMIS5As.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GeneratePOMIS_2Report(string ProcessDate)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, ProcessDate = ProcessDate, Org = SessionHelper.LoginUserOrganizationID };
                var POMIS2s = pomisReportService.GetDataLoanStatementReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", ProcessDate);
                ReportHelper.PrintReport("rpt_POMIS2_LoanStatement.rpt", POMIS2s.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GeneratePOMIS_3_AReport(string ProcessDate)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, ProcessDate = ProcessDate, Org = SessionHelper.LoginUserOrganizationID };
                var POMIS3As = pomisReportService.GetDataOverdueClassificationReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", ProcessDate);
                ReportHelper.PrintReport("rpt_POMIS3A_OverdueClassification.rpt", POMIS3As.Tables[0], reportParam);
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion

        public JsonResult ConsolidationProcess_Note4(short queueNo, string ProcessDate) //Khalid
        {
            string result = string.Empty;

            try
            {
                var param = new { Org = Convert.ToInt32(SessionHelper.LoginUserOrganizationID), OfficeFrom = "00000", OfficeTo = "99999", DateTo = Convert.ToDateTime(ProcessDate), qType = queueNo };
                var POMIS = pomisReportService.ConsolidationProcess_Note4(param);
                result = "Process Completed Successfully";

            }
            catch (Exception ex)
            {
                result = ex.Message;

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }//


        #region Events
        //
        // GET: /POMIS/
        public ActionResult POMISProcess()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult POMIS1()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult POMIS5A()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult POMIS2()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult POMIS3A()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        //
        // GET: /POMIS/Create
        public ActionResult Create()
        {
            return View();
        }
        //
        // POST: /POMIS/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        //
        // GET: /POMIS/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        //
        // POST: /POMIS/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        //
        // GET: /POMIS/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        //
        // POST: /POMIS/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
#endregion


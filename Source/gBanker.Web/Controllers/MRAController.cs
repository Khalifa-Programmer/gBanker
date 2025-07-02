using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gBanker.Web.ViewModels;

namespace gBanker.Web.Controllers
{
    public class MRAController : BaseController
    {
        #region Variables
        private readonly IMRAReportService mraReportService;
        private readonly IUltimateReportService ultimateReportService;
        public MRAController(IMRAReportService mraReportService, IUltimateReportService ultimateReportService)
        {
            this.mraReportService = mraReportService;
            this.ultimateReportService = ultimateReportService;
        }
        #endregion
        public ActionResult GenerateMraReport(string Date, string Qtype)
        {
            try
            {

                var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID };
                var Mras = mraReportService.GetDataMraReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", Date);
                // ReportHelper.PrintReport("rptTodaysSummary.rpt", TodaysSummarys.Tables[0], reportParam);

                if (Qtype == "1")
                {
                    //ReportHelper.PrintReport("MRA_01.rpt", Mras.Tables[0], new Dictionary<string, object>());                    
                    ReportHelper.PrintReport("MRA_01.rpt", Mras.Tables[0], reportParam);
                }
                else if (Qtype == "2")
                {

                    //ReportHelper.PrintReport("MRA_02.rpt", Mras.Tables[0], new Dictionary<string, object>());                    
                    ReportHelper.PrintReport("MRA_02.rpt", Mras.Tables[0], reportParam);
                }
                else if (Qtype == "3")
                {

                    //ReportHelper.PrintReport("MRA_03.rpt", Mras.Tables[0], new Dictionary<string, object>());                    
                    ReportHelper.PrintReport("MRA_03.rpt", Mras.Tables[0], reportParam);
                }
                else if (Qtype == "4")
                {
                    //ReportHelper.PrintReport("MRA_04.rpt", Mras.Tables[0], new Dictionary<string, object>());
                    ReportHelper.PrintReport("MRA_04.rpt", Mras.Tables[0], reportParam);
                }
                else if (Qtype == "5")
                {

                    //ReportHelper.PrintReport("MRA_05.rpt", Mras.Tables[0], new Dictionary<string, object>());
                    ReportHelper.PrintReport("MRA_05.rpt", Mras.Tables[0], reportParam);
                }
                else if (Qtype == "6")
                {

                    //ReportHelper.PrintReport("rpt_MRA_ProvisionCalculation.rpt", Mras.Tables[0], new Dictionary<string, object>());
                    ReportHelper.PrintReport("rpt_MRA_ProvisionCalculation.rpt", Mras.Tables[0], reportParam);
                }

                return Content(string.Empty);


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        //
        // GET: /MRA/
        public ActionResult MRA()
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
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }

        //
        // GET: /MRA/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /MRA/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MRA/Create
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
        // GET: /MRA/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MRA/Edit/5
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
        // GET: /MRA/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MRA/Delete/5
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


        public ActionResult MRAErrorCheckView()
        {

            return View();

        }


        public JsonResult MRAErrorCheckListInfo(string filterColumn, string filterValue, int jtStartIndex = 0, int jtPageSize = 20, string jtSorting = null)
        {
            try
            {

                var param = new { @Officeid = SessionHelper.LoginUserOfficeID, @filterColumn = filterColumn, @filterValue = filterValue };
                //var result_set = ultimateReportService.GetDataWithParameter(param, "Proc_GetMamlaInfo");

                var result_set = ultimateReportService.GetDataWithParameter(param, "MRA_CIB_ErrorCheckList");

                var result_List = result_set.Tables[0].AsEnumerable()
                    .Select(x => new MamlaListViewModel()
                    {
                        RowSl = x.Field<int>("RowSl"),
                        ZoneCode = x.Field<string>("ZoneCode"),
                        OfficeCode = x.Field<string>("OfficeCode"),
                        OfficeName = x.Field<string>("OfficeName"),
                        CenterCode = x.Field<string>("CenterCode"),
                        CenterName = x.Field<string>("CenterName"),
                        Branchwise_sl = x.Field<Int64>("Branchwise_sl"),
                        BranchCode = x.Field<string>("BranchCode"),
                        MemberCode = x.Field<string>("MemberCode"),
                        Name = x.Field<string>("Name"),
                        ErrorColumnName = x.Field<string>("ErrorColumnName"),
                        ErrorCode = x.Field<string>("ErrorCode"),
                        ErrorDescription = x.Field<string>("ErrorDescription")

                    }).ToList();
                var totalCount = result_List.Count();
                var currentPageRecords = result_List.Skip(jtStartIndex).Take(jtPageSize).ToList();

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
    }
}

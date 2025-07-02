using gBanker.Service;
using gBanker.Service.Helpers;
using gBanker.Service.ReportExecutionService;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class AccCashBookController : BaseController
    {
        #region Variables
        private readonly IAccTrxMasterService accTrxMasterService;
        private readonly IAccTrxDetailService accTrxDetailService;
        private readonly IAccChartService accChartService;
        private readonly IAccLastVoucherService accLastVoucherService;
        private readonly IProcessInfoService processInfoService;
        private readonly IAccReportService accReportService;
        private readonly IOfficeService officeService;
        private readonly IWeeklyReportService weeklyReportService;
        public AccCashBookController(IAccTrxMasterService accTrxMasterService, IAccTrxDetailService accTrxDetailService, IAccChartService accChartService, IAccLastVoucherService accLastVoucherService, IProcessInfoService processInfoService, IAccReportService accReportService, IOfficeService officeService,IWeeklyReportService weeklyReportService)
        {
            this.accTrxMasterService = accTrxMasterService;
            this.accTrxDetailService = accTrxDetailService;
            this.accChartService = accChartService;
            this.accLastVoucherService = accLastVoucherService;
            this.processInfoService = processInfoService;
            this.accReportService = accReportService;
            this.officeService = officeService;
            this.weeklyReportService = weeklyReportService;
        }
        #endregion

        #region Methods
        public JsonResult GetHOList()
        {
            var First_Level = officeService.GetByOfficeOrgID(Convert.ToInt32(SessionHelper.LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 1 && c.FirstLevel == First_Level.FirstLevel);
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetZoneList(string HO_val)
        {
            var ofcId = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 2 && c.FirstLevel == ofcId && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAreaList(string HO_val,string zone_val)
        {
            var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 3 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOfficeList(string HO_val, string zone_val, string area_val)
        {
            var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var area_code = officeService.GetById(Convert.ToInt32(area_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 4 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.ThirdLevel == area_code && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        protected void PrintSSRSReport(string reportName, ParameterValue[] parameters, string connectionStringName = "gBankerReport")
        {
            var result = "";
            var binaryObj = SSRSReportProcessHelper.RenderReport(reportName, parameters, ref result, connectionStringName);
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(binaryObj);
            Response.End();
        }


        public ActionResult GenerateCashBookReportDateWise(string office_id, string from_date, string to_date, string acc_level)
        {
            try
            {

                DataSet allVouchers;
                if (SessionHelper.LoginUserOrganizationID == 54)
                {
                    var paramValues = new List<ParameterValue>();
                    paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id.ToString() });
                    paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date.ToString() });
                    paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date.ToString() });
                    paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = acc_level.ToString() });
                    PrintSSRSReport("/gBanker_Reports/acc_cashbook", paramValues.ToArray(), "gBankerReport");
                }
                else
                {
                    var param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level };
                    if (LoggedInOrganizationID == 54)
                    {
                        allVouchers = accReportService.GetDataCashBookReport(param);
                        var reportParam = new Dictionary<string, object>();
                        reportParam.Add("Param_OrgName", ApplicationSettings.OrganiztionName);
                        reportParam.Add("FromDate", from_date);
                        reportParam.Add("ToDate", to_date);


                        ReportHelper.PrintReport("rpt_acc_cashbook_DateWise.rpt", allVouchers.Tables[0], reportParam);
                    }

                    else if (LoggedInOrganizationID == 94)
                    {
                        allVouchers = accReportService.GetDataCashBookReportDateWise(param);
                        var reportParam = new Dictionary<string, object>();
                        reportParam.Add("Param_OrgName", ApplicationSettings.OrganiztionName);
                        reportParam.Add("FromDate", from_date);
                        reportParam.Add("ToDate", to_date);


                        ReportHelper.PrintReport("rpt_acc_cashbook_DateWise.rpt", allVouchers.Tables[0], reportParam);
                    }
                  


                   


                }
                // var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date };
                //   ReportHelper.PrintReport("rpt_MRA_ProvisionCalculation.rpt", Mras.Tables[0], new Dictionary<string, object>());



                //    public ActionResult GenerateMraMIS4BReport(string Date)
                //{
                //    try
                //    {
                //        var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date };

                //        var MraMIS4B1s = mraReportService.GetDataMraMISReport4B1(param);
                //        var MraMIS4B2s = mraReportService.GetDataMraMISReport4B2(param);

                //        var subReportDB = new Dictionary<string, DataTable>();
                //        subReportDB.Add("MRA-MIS-04B_Part02.rpt", MraMIS4B2s.Tables[0]);

                //        ReportHelper.PrintWithSubReport("MRA-MIS-04B.rpt", MraMIS4B1s.Tables[0], new Dictionary<string, object>(), subReportDB, new MRA_MIS_04B());


                //        return Content(string.Empty);
                //    }
                //    catch (Exception ex)
                //    {
                //        return Json(new { Result = "ERROR", Message = ex.Message });
                //    }
                //}
                //ReportHelper.MyPrintReport("rpt_acc_cashbook.rpt", allVouchers.Tables[0], allbanks.Tables[0], reportParam);
                //ReportHelper.PrintWithSubReport("rpt_acc_cashbook.rpt", allVouchers.Tables[0], reportParam, subReportDB);

                //ReportDocument crDocument = new ReportDocument();

                //ExportOptions crExportOptions = new ExportOptions();
                //DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                //string strFName;
                //string strReportPathAbsolute = System.Web.HttpContext.Current.Server.MapPath("~/Reports/" + "rpt_acc_cashbook.rpt");
                //crDocument.Load(strReportPathAbsolute);
                ////crDocument.OpenSubreport("rpt_acc_cashbook_bank.rpt").SetDataSource(allbanks);
                //crDocument.SetDataSource(allVouchers);
                //crDocument.SetParameterValue("Param_OrgName", ApplicationSettings.OrganiztionName);
                //crDocument.SetParameterValue("FromDate", from_date);
                //crDocument.SetParameterValue("ToDate", to_date);


                //strFName = System.Web.HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                //crDiskFileDestination.DiskFileName = strFName;
                //crExportOptions = crDocument.ExportOptions;
                //crExportOptions.DestinationOptions = crDiskFileDestination;
                //crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //crDocument.Export();
                //System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                //System.Web.HttpContext.Current.Response.WriteFile(strFName);
                //System.Web.HttpContext.Current.Response.End();
                //// Response.Close();
                //System.IO.File.Delete(strFName);
                //*********************************************************







                return Content(string.Empty);

                //Incase of subreport.
                //var subReportDataSources = new Dictionary<string, DataTable>();
                //subReportDataSources.Add("Subreport1", allproducts.Tables[0]);
                //subReportDataSources.Add("Subreport2", allproducts.Tables[0]);
                //ReportHelper.PrintWithSubReport("", allproducts.Tables[0], new Dictionary<string, object>(), subReportDataSources);
                // return Content(string.Empty); 
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GenerateCashBookReport(string office_id, string from_date, string to_date, string acc_level)
        {
            try
            {

                DataSet allVouchers;
                if (SessionHelper.LoginUserOrganizationID == 54)
                {
                    var paramValues = new List<ParameterValue>();
                    paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id.ToString() });
                    paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date.ToString() });
                    paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date.ToString() });
                    paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = acc_level.ToString() });
                    PrintSSRSReport("/gBanker_Reports/acc_cashbook", paramValues.ToArray(), "gBankerReport");
                }
                else
                {
                    var param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level };
                    if (LoggedInOrganizationID == 54)
                    {
                        allVouchers = accReportService.GetDataCashBookReport(param);
                    }
                    else if (LoggedInOrganizationID==190)
                    {

                        allVouchers = accReportService.GetDataCashBookReportRDRS(param);
                    }
                    else
                    {
                        allVouchers = accReportService.GetDataCashBookReport(param);
                    }



                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("Param_OrgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("FromDate", from_date);
                    reportParam.Add("ToDate", to_date);


                    ReportHelper.PrintReport("rpt_acc_cashbook.rpt", allVouchers.Tables[0], reportParam);


                }
                // var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date };
                //   ReportHelper.PrintReport("rpt_MRA_ProvisionCalculation.rpt", Mras.Tables[0], new Dictionary<string, object>());



                //    public ActionResult GenerateMraMIS4BReport(string Date)
                //{
                //    try
                //    {
                //        var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date };

                //        var MraMIS4B1s = mraReportService.GetDataMraMISReport4B1(param);
                //        var MraMIS4B2s = mraReportService.GetDataMraMISReport4B2(param);

                //        var subReportDB = new Dictionary<string, DataTable>();
                //        subReportDB.Add("MRA-MIS-04B_Part02.rpt", MraMIS4B2s.Tables[0]);

                //        ReportHelper.PrintWithSubReport("MRA-MIS-04B.rpt", MraMIS4B1s.Tables[0], new Dictionary<string, object>(), subReportDB, new MRA_MIS_04B());


                //        return Content(string.Empty);
                //    }
                //    catch (Exception ex)
                //    {
                //        return Json(new { Result = "ERROR", Message = ex.Message });
                //    }
                //}
                //ReportHelper.MyPrintReport("rpt_acc_cashbook.rpt", allVouchers.Tables[0], allbanks.Tables[0], reportParam);
                //ReportHelper.PrintWithSubReport("rpt_acc_cashbook.rpt", allVouchers.Tables[0], reportParam, subReportDB);

                //ReportDocument crDocument = new ReportDocument();

                //ExportOptions crExportOptions = new ExportOptions();
                //DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                //string strFName;
                //string strReportPathAbsolute = System.Web.HttpContext.Current.Server.MapPath("~/Reports/" + "rpt_acc_cashbook.rpt");
                //crDocument.Load(strReportPathAbsolute);
                ////crDocument.OpenSubreport("rpt_acc_cashbook_bank.rpt").SetDataSource(allbanks);
                //crDocument.SetDataSource(allVouchers);
                //crDocument.SetParameterValue("Param_OrgName", ApplicationSettings.OrganiztionName);
                //crDocument.SetParameterValue("FromDate", from_date);
                //crDocument.SetParameterValue("ToDate", to_date);


                //strFName = System.Web.HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                //crDiskFileDestination.DiskFileName = strFName;
                //crExportOptions = crDocument.ExportOptions;
                //crExportOptions.DestinationOptions = crDiskFileDestination;
                //crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //crDocument.Export();
                //System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                //System.Web.HttpContext.Current.Response.WriteFile(strFName);
                //System.Web.HttpContext.Current.Response.End();
                //// Response.Close();
                //System.IO.File.Delete(strFName);
                //*********************************************************







                return Content(string.Empty);

                //Incase of subreport.
                //var subReportDataSources = new Dictionary<string, DataTable>();
                //subReportDataSources.Add("Subreport1", allproducts.Tables[0]);
                //subReportDataSources.Add("Subreport2", allproducts.Tables[0]);
                //ReportHelper.PrintWithSubReport("", allproducts.Tables[0], new Dictionary<string, object>(), subReportDataSources);
                // return Content(string.Empty); 
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public ActionResult GenerateCashFlowReport(string Date)
        {
            try
            {
                var paramValues = new List<ParameterValue>();          
                paramValues.Add(new ParameterValue() { Name = "Param_OrgName", Value = ApplicationSettings.OrganiztionName });

                paramValues.Add(new ParameterValue() { Name = "office_id", Value = "0" });
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = Date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = Date });

                var year = Convert.ToDateTime(Date).Year;

                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = year.ToString() });

                paramValues.Add(new ParameterValue() { Name = "Date", Value = Date.ToString() });


                PrintSSRSReport("/gBanker_Reports/CashFlow", paramValues.ToArray(), "gBankerReport");


                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        #endregion

        #region Events
        // GET: AccCashBook
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            if (LoggedInOrganizationID != 54)
            {
                if (IsDayInitiated)
                {


                    var param1 = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                    var LoanInstallMent = weeklyReportService.CheckAutoVoucher(param1);

                    if (LoanInstallMent.Tables[0].Rows.Count == 0)
                    {
                        var param = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                        weeklyReportService.AutoVoucherCollectionProcess(param);
                    }

                }
            }

            return View();
        }

        // GET: AccCashBook/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccCashBook/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccCashBook/Create
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

        // GET: AccCashBook/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccCashBook/Edit/5
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

        // GET: AccCashBook/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccCashBook/Delete/5
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

        public ActionResult CashFlowReport()
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


        #endregion
    }
}

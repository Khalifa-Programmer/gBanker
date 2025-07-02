using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
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
using Utility.Extensions;

namespace gBanker.Web.Controllers
{
    public class MRAReportController : BaseController
    {
        #region Variables
        private readonly IMRAActivityListService mRAActivityListService;
        private readonly IWelfareActivityDetailService welfareActivityDetailService;
        private readonly IRemittanceActivityService remittanceActivityService;
        private readonly ITrainingService trainingService;
        private readonly IMFIInfoService mFIInfoService;
        private readonly IPrimaryRegistrationService primaryRegistrationService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IOfficeService officeService;
        public MRAReportController(IMRAActivityListService mRAActivityListService, IWelfareActivityDetailService welfareActivityDetailService,
            IRemittanceActivityService remittanceActivityService, ITrainingService trainingService, IMFIInfoService mFIInfoService, IPrimaryRegistrationService primaryRegistrationService, 
            IUltimateReportService ultimateReportService, IOfficeService officeService)
        {
            this.mRAActivityListService = mRAActivityListService;
            this.welfareActivityDetailService = welfareActivityDetailService;
            this.remittanceActivityService = remittanceActivityService;
            this.trainingService = trainingService;
            this.mFIInfoService = mFIInfoService;
            this.primaryRegistrationService = primaryRegistrationService;
            this.ultimateReportService = ultimateReportService;
            this.officeService = officeService;
        }
        #endregion

        #region Events
        public ActionResult CreateWelfareActivities()
        {
            var model = new WelfareActivityViewModel();
            MapDropdownForActivityList(model);
            return View(model);
        }
        public ActionResult IndexWelfareActivities()
        {
            return View();
        }
        public ActionResult EditWelfareActivities(int WelfareActivityId)
        {
            var welfareActivityInfo = welfareActivityDetailService.GetById(WelfareActivityId);
            var viewWelfareActivityInfo = AutoMapper.Mapper.Map<WelfareActivityDetail, WelfareActivityViewModel>(welfareActivityInfo);
            viewWelfareActivityInfo.DateTo = Convert.ToDateTime(welfareActivityInfo.DateTo).ToString("dd-MMM-yyyy");
            MapDropdownForActivityList(viewWelfareActivityInfo);
            return View(viewWelfareActivityInfo);
        }
        public ActionResult CreateRemittanceActivity()
        {
            return View();
        }
        public ActionResult IndexRemittanceActivity()
        {
            return View();
        }
        public ActionResult EditRemittanceActivity(int RemittanceActivityId)
        {
            var remittanceInfo = remittanceActivityService.GetById(RemittanceActivityId);
            var viewRemittanceInfo = AutoMapper.Mapper.Map<RemittanceActivity, RemittanceActivityViewModel>(remittanceInfo);
            viewRemittanceInfo.TransactionDate = Convert.ToDateTime(viewRemittanceInfo.TransactionDate).ToString("dd-MMM-yyyy");
            return View(viewRemittanceInfo);
        }
        public ActionResult CreateTrainingSummary()
        {
            var model = new TrainingViewModel();
            MapDropdownTrainingType(model);
            return View(model);
        }
        public ActionResult IndexTrainingSummary()
        {
            return View();
        }
        public ActionResult EditTrainingSummery(int TrainingID)
        {
            var trainingInfo = trainingService.GetById(TrainingID);
            var viewTrainingInfo = AutoMapper.Mapper.Map<Training, TrainingViewModel>(trainingInfo);
            MapDropdownTrainingType(viewTrainingInfo);
            viewTrainingInfo.TrainingDate = Convert.ToDateTime(viewTrainingInfo.TrainingDate).ToString("dd-MMM-yyyy");
            return View(viewTrainingInfo);
        }
        public ActionResult CreateMFIInfo()
        {
            var model = new MFIInfoViewModel();
            MapDropdownForMFI(model);
            return View(model);
        }
        public ActionResult IndexMFIInfo()
        {
            return View();
        }
        public ActionResult EditMFIInfo(int MFIId)
        {
            var mfiInfo = mFIInfoService.GetById(MFIId);
            var view_mfiInfo = AutoMapper.Mapper.Map<MFIInformation, MFIInfoViewModel>(mfiInfo);
            view_mfiInfo.DateTo = Convert.ToDateTime(view_mfiInfo.DateTo).ToString("dd-MMM-yyyy");
            view_mfiInfo.GBDateOfLastMeeting = Convert.ToDateTime(view_mfiInfo.GBDateOfLastMeeting).ToString("dd-MMM-yyyy");
            view_mfiInfo.EBExpiryDate = Convert.ToDateTime(view_mfiInfo.EBExpiryDate).ToString("dd-MMM-yyyy");
            view_mfiInfo.EBDateOfLastMeeting = Convert.ToDateTime(view_mfiInfo.EBDateOfLastMeeting).ToString("dd-MMM-yyyy");
            view_mfiInfo.ServiceRules = view_mfiInfo.ServiceRules == "1" ? "1": "0";
            view_mfiInfo.FinancialPolicy = view_mfiInfo.FinancialPolicy == "1" ? "1" : "0";
            view_mfiInfo.SavingsAndCreditPolicy = view_mfiInfo.SavingsAndCreditPolicy == "1" ? "1" : "0";
            view_mfiInfo.NISAndAntiMoneyLaunderingGuideline = view_mfiInfo.NISAndAntiMoneyLaunderingGuideline == "1" ? "1" : "0";
            view_mfiInfo.CitizenCharter = view_mfiInfo.CitizenCharter == "1" ? "1" : "0";
            MapDropdownForMFI(view_mfiInfo);
            return View(view_mfiInfo);
        }

        #endregion

        #region Methods

        public ActionResult Print_MRA_DBMS_3_Report_ForExcel(string dateTo, string whichReport)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var office = SessionHelper.LoginUserOfficeID;
                var vOffice = officeService.GetById((int)office);
                var reportTitle = $"{vOffice.OfficeCode}_{vOffice.OfficeName}_MRA_DBMS_3_({whichReport})_{DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz")}";

                var reportParam = new Dictionary<string, object>();

                if (whichReport == "A")
                {
                    var paramA = new { Org = orgId, Office = office, Date = dateTo };
                    var Report_A = ultimateReportService.GetDataWithParameterMRA(paramA, "Proc_Set_MRA_MIS_03A_Part01__New");
                    ReportHelper.ExportExcelReportFormated("MRA_DBMS_3_Excel_A.rpt", Report_A.Tables[0], reportParam, reportTitle);
                }
                else
                {
                    var qType = 1;
                    var paramB = new { OrgId = orgId, OfficeId = office, DateTo = dateTo, QType = qType };
                    var Report_B = ultimateReportService.GetDataWithParameterMRA(paramB, "RPT_SavingsSize_Buro__New");
                    ReportHelper.ExportExcelReportFormated("MRA_DBMS_3_Excel_B.rpt", Report_B.Tables[0], reportParam, reportTitle);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Print_MRA_DBMS_4E_Report_ForExcel(string dateFrom, string dateTo, string whichReport)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var office = SessionHelper.LoginUserOfficeID;
                var vOffice = officeService.GetById((int)office);
                var reportTitle = $"{vOffice.OfficeCode}_{vOffice.OfficeName}_MRA_DBMS_4E_({whichReport})_{DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz")}";

                var reportParam = new Dictionary<string, object>();

                if (whichReport == "A")
                {
                    var qType = 1;
                    var paramA = new { OrgId = orgId, OfficeId = office, DateTo = dateTo, QType = qType };
                    var Report_A = ultimateReportService.GetDataWithParameterMRA(paramA, "RPT_LoanSize_MRA__New");
                    ReportHelper.ExportExcelReportFormated("MRA_DBMS_4E_Excel_A.rpt", Report_A.Tables[0], reportParam, reportTitle);
                }
                else
                {
                    var qType = 2;
                    var paramB = new { OfficeID = office, OfficeIDTO = office, CenterID = 00, CenterIDTo = 99999999, StaffID = 00, StaffIDTo = 99999999, productID = 00, ProductIDTo = 99999999, DateFrom = dateFrom, DateTo = dateTo, Qtype = qType };
                    var Report_B = ultimateReportService.GetDataWithParameterMRA(paramB, "DBMS4EReport__New");
                    ReportHelper.ExportExcelReportFormated("MRA_DBMS_4E_Excel_B.rpt", Report_B.Tables[0], reportParam, reportTitle);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void MRAReportList(MRAReportViewModel model)
        {
            var reportList = new List<SelectListItem>();
            reportList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            reportList.Add(new SelectListItem() { Text = "Operational & Management Related Statement of MFIs (DBMS-1)", Value = 1.ToString() });
            reportList.Add(new SelectListItem() { Text = "Branch Wise Memebers Savings Report (DBMS-3)", Value = "3" });
            reportList.Add(new SelectListItem() { Text = "Branch Wise Classification of Loan and LLP Statement (DBMS-4(E))", Value = "4(E)" });
            reportList.Add(new SelectListItem() { Text = "Yearly Statement on Consolidated Statement of Financial Position (DBMS-5)", Value = "5" });
            reportList.Add(new SelectListItem() { Text = "Yearly Statement on Comprehensive Income Expenditure (DBMS-6)", Value = "6" });
            reportList.Add(new SelectListItem() { Text = "Yearly Statement on Other Activities (Risk Fund & Welfare) (DBMS-7)", Value = "7" });
            reportList.Add(new SelectListItem() { Text = "Yearly Statement on Other Activities (Remittance & Training) (DBMS-8)", Value = "8" });
            model.MRAReportList = reportList;
        }
        private void MapDropdownForActivityList(WelfareActivityViewModel model)
        {
            var activity = mRAActivityListService.GetAll().Where(p => p.IsActive == true).ToList();
            var viewActivity = activity.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.ActivityName,
                Value = p.ActivityId.ToString()
            }).ToList();
            var acitivityList = new List<SelectListItem>();
            acitivityList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            acitivityList.AddRange(viewActivity);
            model.ActivityList = acitivityList;
        }
        [HttpPost]
        public JsonResult SaveWelfareActivity(WelfareActivityViewModel welfareActivity)
        {
            var result = 0;
            var message = "";

            try
            {
                var officeId = SessionHelper.LoginUserOfficeID;
                var entity = new WelfareActivityDetail();
                entity.OfficeId = officeId;
                entity.DateTo = welfareActivity.DateTo.ToDateTime();
                entity.ActivityId = welfareActivity.ActivityId;
                entity.SurplusMicrofinance = welfareActivity.SurplusMicrofinance;
                entity.SurplusOtherActivities = welfareActivity.SurplusOtherActivities;
                entity.SurplusOwnFund = welfareActivity.SurplusOwnFund;
                entity.Donation = welfareActivity.Donation;
                entity.OtherSource = welfareActivity.OtherSource;
                entity.AreaCovered = welfareActivity.AreaCovered;
                entity.NumberOfBeneficiaries = welfareActivity.NumberOfBeneficiaries;
                entity.DurationOfActivity = welfareActivity.DurationOfActivity;
                entity.AcitivityExpenditure = welfareActivity.AcitivityExpenditure;
                entity.Surplus = welfareActivity.Surplus;
                entity.IsActive = true;
                entity.CreateDate = DateTime.UtcNow;
                entity.CreateUser = SessionHelper.LoginUserEmployeeID;
                welfareActivityDetailService.Create(entity);
                result = 1;
                message = "Saved successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult GetWelfareAcitivityList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var welfareInfo = welfareActivityDetailService.GetAll().Where(p => p.IsActive == true).ToList();
            var activityList = mRAActivityListService.GetAll().Where(p => p.IsActive == true).ToList();
            var viewWelfareInfo = from wfi in welfareInfo
                                  join actList in activityList on wfi.ActivityId equals actList.ActivityId
                                  select new WelfareActivityViewModel()
                                  {
                                      WelfareActivityId = wfi.WelfareActivityId,
                                      DateTo = Convert.ToDateTime(wfi.DateTo).ToString("dd-MMM-yyyy"),
                                      SurplusMicrofinance = wfi.SurplusMicrofinance,
                                      SurplusOtherActivities = wfi.SurplusOtherActivities,
                                      SurplusOwnFund = wfi.SurplusOwnFund,
                                      Donation = wfi.Donation,
                                      OtherSource = wfi.OtherSource,
                                      AreaCovered = wfi.AreaCovered,
                                      NumberOfBeneficiaries = wfi.NumberOfBeneficiaries,
                                      DurationOfActivity = wfi.DurationOfActivity,
                                      AcitivityExpenditure = wfi.AcitivityExpenditure,
                                      Surplus = wfi.Surplus
                                  };
            var currentPageRecords = viewWelfareInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewWelfareInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }
        public JsonResult UpdateWelfareAcitivity(WelfareActivityViewModel welfareActivity)
        {
            var result = 0;
            var message = "";

            try
            {
                var officeId = SessionHelper.LoginUserOfficeID;
                var entity = welfareActivityDetailService.GetById(welfareActivity.WelfareActivityId);
                entity.OfficeId = officeId;
                entity.DateTo = welfareActivity.DateTo.ToDateTime();
                entity.ActivityId = welfareActivity.ActivityId;
                entity.SurplusMicrofinance = welfareActivity.SurplusMicrofinance;
                entity.SurplusOtherActivities = welfareActivity.SurplusOtherActivities;
                entity.SurplusOwnFund = welfareActivity.SurplusOwnFund;
                entity.Donation = welfareActivity.Donation;
                entity.OtherSource = welfareActivity.OtherSource;
                entity.AreaCovered = welfareActivity.AreaCovered;
                entity.NumberOfBeneficiaries = welfareActivity.NumberOfBeneficiaries;
                entity.DurationOfActivity = welfareActivity.DurationOfActivity;
                entity.AcitivityExpenditure = welfareActivity.AcitivityExpenditure;
                entity.Surplus = welfareActivity.Surplus;
                entity.IsActive = true;
                entity.CreateDate = DateTime.UtcNow;
                entity.CreateUser = SessionHelper.LoginUserEmployeeID;
                welfareActivityDetailService.Update(entity);
                result = 1;
                message = "Updated successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult DeleteWelfareActivities(int WelfareActivityId)
        {
            var result = 0;
            var message = "";

            var entity = welfareActivityDetailService.GetById(WelfareActivityId);
            entity.IsActive = false;
            entity.CreateDate = DateTime.UtcNow;
            entity.CreateUser = SessionHelper.LoginUserEmployeeID;
            welfareActivityDetailService.Update(entity);
            result = 1;
            message = "Deleted successfully";
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveRemittanceAcitivity(RemittanceActivityViewModel remittanceActivity)
        {
            var result = 0;
            var message = "";
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var officeId = SessionHelper.LoginUserOfficeID;
                var entity = new RemittanceActivity();
                entity.OrgId = orgId;
                entity.OfficeID = officeId;
                entity.NoOfClient = remittanceActivity.NoOfClient;
                entity.RemittedAmount = remittanceActivity.RemittedAmount;
                entity.Commission = remittanceActivity.Commission;
                entity.LinkedBank = remittanceActivity.LinkedBank;
                entity.Remark = remittanceActivity.Remark;
                entity.TransactionDate = Convert.ToDateTime(remittanceActivity.TransactionDate).Date;
                entity.IsActive = true;
                entity.CreateDate = DateTime.UtcNow;
                entity.CreateUser = SessionHelper.LoginUserEmployeeID;
                remittanceActivityService.Create(entity);
                result = 1;
                message = "Saved successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        public JsonResult GetRemittanceActivityList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var remittanceInfo = remittanceActivityService.GetAll().Where(p => p.IsActive == true).ToList();
            var viewRemittanceInfo = remittanceInfo.AsEnumerable().Select(p => new RemittanceActivityViewModel
            {
                RemittanceActivityId = p.RemittanceActivityId,
                NoOfClient = p.NoOfClient,
                TransactionDate = Convert.ToDateTime(p.TransactionDate).ToString("dd-MMM-yyyy"),
                RemittedAmount = p.RemittedAmount,
                Commission = p.Commission,
                LinkedBank = p.LinkedBank,
                Remark = p.Remark
            }).ToList();
            var currentPageRecords = viewRemittanceInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewRemittanceInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateRemittanceAcitivity(RemittanceActivityViewModel remittanceActivity)
        {
            var result = 0;
            var message = "";
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var officeId = SessionHelper.LoginUserOfficeID;
                var entity = remittanceActivityService.GetById(remittanceActivity.RemittanceActivityId);
                entity.OrgId = orgId;
                entity.OfficeID = officeId;
                entity.NoOfClient = remittanceActivity.NoOfClient;
                entity.RemittedAmount = remittanceActivity.RemittedAmount;
                entity.Commission = remittanceActivity.Commission;
                entity.LinkedBank = remittanceActivity.LinkedBank;
                entity.Remark = remittanceActivity.Remark;
                entity.TransactionDate = Convert.ToDateTime(remittanceActivity.TransactionDate).Date;
                entity.IsActive = true;
                entity.CreateDate = DateTime.UtcNow;
                entity.CreateUser = SessionHelper.LoginUserEmployeeID;
                remittanceActivityService.Update(entity);
                result = 1;
                message = "Updated successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public JsonResult DeleteRemittanceActivity(int RemittanceActivityId)
        {
            var result = 0;
            var message = "";

            var entity = remittanceActivityService.GetById(RemittanceActivityId);
            entity.IsActive = false;
            entity.CreateDate = DateTime.UtcNow;
            entity.CreateUser = SessionHelper.LoginUserEmployeeID;
            remittanceActivityService.Update(entity);
            result = 1;
            message = "Deleted successfully";
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        private void MapDropdownTrainingType(TrainingViewModel model)
        {
            var trainingTypeList = new List<SelectListItem>();
            trainingTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            trainingTypeList.Add(new SelectListItem { Text = "Domestic", Value = "D" });
            trainingTypeList.Add(new SelectListItem { Text = "Foreign", Value = "F" });
            model.TrainingTypeList = trainingTypeList;
        }
        [HttpPost]
        public JsonResult SaveTrainingSummary(TrainingViewModel trainingSummary)
        {
            var result = 0;
            var message = "";
            var orgId = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            var entity = new Training();
            entity.OrgId = orgId;
            entity.OfficeID = officeId;
            entity.TrainingType = trainingSummary.TrainingType;
            entity.TrainingDate = Convert.ToDateTime(trainingSummary.TrainingDate).Date;
            entity.NoOfParticipants = trainingSummary.NoOfParticipants;
            entity.CourseName = trainingSummary.CourseName;
            entity.CostGeneralFund = trainingSummary.CostGeneralFund;
            entity.CostMicroFinance = trainingSummary.CostMicroFinance;
            entity.CostDonation = trainingSummary.CostDonation;
            entity.OtherCostSource1 = trainingSummary.OtherCostSource1;
            entity.CostAmount1 = trainingSummary.CostAmount1;
            entity.OtherCostSource2 = trainingSummary.OtherCostSource2;
            entity.CostAmount2 = trainingSummary.CostAmount2;
            entity.OtherCostSource3 = trainingSummary.OtherCostSource3;
            entity.CostAmount3 = trainingSummary.CostAmount3;
            entity.IsActive = true;
            entity.CreateDate = DateTime.UtcNow;
            entity.CreateUser = SessionHelper.LoginUserEmployeeID;
            trainingService.Create(entity);
            result = 1;
            message = "Saved successfully";
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTrainingSummery(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var trainingInfo = ultimateReportService.GetDataWithoutParameter("SP_TrainingSummary");
            var viewTrainingInfo = trainingInfo.Tables[0].AsEnumerable().Select(p => new TrainingViewModel
            {
                TrainingID = p.Field<int>("TrainingID"),
                TrainingType = p.Field<string>("TrainingType"),
                TrainingDate = p.Field<string>("TrainingDate"),
                NoOfParticipants = p.Field<int>("NoOfParticipants"),
                CourseName =p.Field<string>("CourseName"),
                CostGeneralFund=p.Field<decimal?>("CostGeneralFund"),
                CostMicroFinance = p.Field<decimal?>("CostMicroFinance"),
                CostDonation=p.Field<decimal?>("CostDonation"),
                OtherCostSource1=p.Field<string>("OtherCostSource1"),
                CostAmount1=p.Field<decimal?>("CostAmount1"),
                OtherCostSource2 = p.Field<string>("OtherCostSource2"),
                CostAmount2 = p.Field<decimal?>("CostAmount2"),
                OtherCostSource3 = p.Field<string>("OtherCostSource3"),
                CostAmount3 = p.Field<decimal?>("CostAmount3"),
            }).ToList();
            var currentPageRecords = viewTrainingInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewTrainingInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateTrainingSummary(TrainingViewModel trainingSummary)
        {
            var result = 0;
            var message = "";
            var orgId = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            var entity = trainingService.GetById(trainingSummary.TrainingID);
            entity.OrgId = orgId;
            entity.OfficeID = officeId;
            entity.TrainingType = trainingSummary.TrainingType;
            entity.TrainingDate = Convert.ToDateTime(trainingSummary.TrainingDate).Date;
            entity.NoOfParticipants = trainingSummary.NoOfParticipants;
            entity.CourseName = trainingSummary.CourseName;
            entity.CostGeneralFund = trainingSummary.CostGeneralFund;
            entity.CostMicroFinance = trainingSummary.CostMicroFinance;
            entity.CostDonation = trainingSummary.CostDonation;
            entity.OtherCostSource1 = trainingSummary.OtherCostSource1;
            entity.CostAmount1 = trainingSummary.CostAmount1;
            entity.OtherCostSource2 = trainingSummary.OtherCostSource2;
            entity.CostAmount2 = trainingSummary.CostAmount2;
            entity.OtherCostSource3 = trainingSummary.OtherCostSource3;
            entity.CostAmount3 = trainingSummary.CostAmount3;
            entity.IsActive = true;
            entity.CreateDate = DateTime.UtcNow;
            entity.CreateUser = SessionHelper.LoginUserEmployeeID;
            trainingService.Update(entity);
            result = 1;
            message = "Updated successfully";
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteTrainingSummery(int TrainingID)
        {
            var result = 0;
            var message = "";

            var entity = trainingService.GetById(TrainingID);
            entity.IsActive = false;
            entity.CreateDate = DateTime.UtcNow;
            entity.CreateUser = SessionHelper.LoginUserEmployeeID;
            trainingService.Update(entity);
            result = 1;
            message = "Deleted successfully";
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        private void MapDropdownForMFI(MFIInfoViewModel model)
        {
            //var theSocietiesRegistrationAct = new List<SelectListItem>();
            //theSocietiesRegistrationAct.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            //theSocietiesRegistrationAct.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            //theSocietiesRegistrationAct.Add(new SelectListItem() { Text = "No", Value = "0" });
            //model.TheSocietiesRegistrationActList = theSocietiesRegistrationAct;

            //var trustAct = new List<SelectListItem>();
            //trustAct.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            //trustAct.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            //trustAct.Add(new SelectListItem() { Text = "No", Value = "0" });
            //model.TrustActList = trustAct;

            //var companyAct = new List<SelectListItem>();
            //companyAct.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            //companyAct.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            //companyAct.Add(new SelectListItem() { Text = "No", Value = "0" });
            //model.CompanyActList = companyAct;

            //var theVoluntarySocialWelfareAgencies = new List<SelectListItem>();
            //theVoluntarySocialWelfareAgencies.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            //theVoluntarySocialWelfareAgencies.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            //theVoluntarySocialWelfareAgencies.Add(new SelectListItem() { Text = "No", Value = "0" });
            //model.TheVoluntarySocialWelfareAgenciesList = theVoluntarySocialWelfareAgencies;

            var serviceRules = new List<SelectListItem>();
            serviceRules.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            serviceRules.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            serviceRules.Add(new SelectListItem() { Text = "No", Value = "0" });
            model.ServiceRulesList = serviceRules;

            var financialPolicy = new List<SelectListItem>();
            financialPolicy.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            financialPolicy.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            financialPolicy.Add(new SelectListItem() { Text = "No", Value = "0" });
            model.FinancialPolicyList = financialPolicy;

            var savingsAndCreditPolicy = new List<SelectListItem>();
            savingsAndCreditPolicy.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            savingsAndCreditPolicy.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            savingsAndCreditPolicy.Add(new SelectListItem() { Text = "No", Value = "0" });
            model.SavingsAndCreditPolicyList = savingsAndCreditPolicy;

            var nISAndAntiMoneyLaunderingGuideline = new List<SelectListItem>();
            nISAndAntiMoneyLaunderingGuideline.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            nISAndAntiMoneyLaunderingGuideline.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            nISAndAntiMoneyLaunderingGuideline.Add(new SelectListItem() { Text = "No", Value = "0" });
            model.NISAndAntiMoneyLaunderingGuidelineList = nISAndAntiMoneyLaunderingGuideline;

            var citizenCharter = new List<SelectListItem>();
            citizenCharter.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            citizenCharter.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            citizenCharter.Add(new SelectListItem() { Text = "No", Value = "0" });
            model.CitizenCharterList = citizenCharter;

            var primaryRegistration = new List<SelectListItem>();
            var pr = primaryRegistrationService.GetAll().Where(p => p.IsActive == true).ToList();
            var prList = pr.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.RegistrationName,
                Value = p.PrimaryRegistrationID.ToString()
            });
            primaryRegistration.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            primaryRegistration.AddRange(prList);
            model.PrimaryRegistrationList = primaryRegistration;
        }
        [HttpPost]
        public JsonResult SaveMFIInfo(MFIInfoViewModel MFIInfoObject)
        {
            var result = 0;
            var message = "";
            try
            {
                var entity = new MFIInformation();
                entity.MFIName = MFIInfoObject.MFIName;
                entity.LicenseNo = MFIInfoObject.LicenseNo;
                entity.DateTo = Convert.ToDateTime(MFIInfoObject.DateTo).Date;
                entity.HOMailingAddress = MFIInfoObject.HOMailingAddress;
                entity.HODistrict = MFIInfoObject.HODistrict;
                entity.HOThana = MFIInfoObject.HOThana;
                entity.HOUnion = MFIInfoObject.HOUnion;
                entity.HOEmail = MFIInfoObject.HOEmail;
                entity.HOWebAddress = MFIInfoObject.HOWebAddress;
                entity.HOPhone = MFIInfoObject.HOPhone;
                entity.HOMobile = MFIInfoObject.HOMobile;
                entity.HOFax = MFIInfoObject.HOFax;
                entity.LOAddress = MFIInfoObject.LOAddress;
                entity.LOTelephone = MFIInfoObject.LOTelephone;
                entity.LOMobile = MFIInfoObject.LOMobile;
                entity.LOFax = MFIInfoObject.LOFax;
                entity.LOEmail = MFIInfoObject.LOEmail;
                entity.PrimaryRegistrationID = MFIInfoObject.PrimaryRegistrationID;
                entity.GBNoOfMaleMember = MFIInfoObject.GBNoOfMaleMember;
                entity.GBNoOfFemaleMember = MFIInfoObject.GBNoOfFemaleMember;
                entity.GBNoOfYearlyMeetingsHeld = MFIInfoObject.GBNoOfYearlyMeetingsHeld;
                entity.GBDateOfLastMeeting = Convert.ToDateTime(MFIInfoObject.GBDateOfLastMeeting).Date;
                entity.GBNoOfParticipantsInTheLastMeeting = MFIInfoObject.GBNoOfParticipantsInTheLastMeeting;
                entity.EBExpiryDate = Convert.ToDateTime(MFIInfoObject.EBExpiryDate).Date;
                entity.EBNoOfMaleMember = MFIInfoObject.EBNoOfMaleMember;
                entity.EBNoOfFemaleMember = MFIInfoObject.EBNoOfFemaleMember;
                entity.EBNoOfYearlyMeetingsHeld = MFIInfoObject.EBNoOfYearlyMeetingsHeld;
                entity.EBDateOfLastMeeting = Convert.ToDateTime(MFIInfoObject.EBDateOfLastMeeting).Date;
                entity.EBNoOfParticipantsInTheLastMeeting = MFIInfoObject.EBNoOfParticipantsInTheLastMeeting;
                entity.ServiceRules = Convert.ToInt32(MFIInfoObject.ServiceRules);
                entity.FinancialPolicy = Convert.ToInt32(MFIInfoObject.FinancialPolicy);
                entity.SavingsAndCreditPolicy = Convert.ToInt32(MFIInfoObject.SavingsAndCreditPolicy);
                entity.NISAndAntiMoneyLaunderingGuideline = Convert.ToInt32(MFIInfoObject.NISAndAntiMoneyLaunderingGuideline);
                entity.CitizenCharter = Convert.ToInt32(MFIInfoObject.CitizenCharter);
                entity.MaleMicroCreditProgramBranch = MFIInfoObject.MaleMicroCreditProgramBranch;
                entity.FemaleMicroCreditProgramBranch = MFIInfoObject.FemaleMicroCreditProgramBranch;
                entity.MaleMicroCreditProgramArea = MFIInfoObject.MaleMicroCreditProgramArea;
                entity.FemaleMicroCreditProgramArea = MFIInfoObject.FemaleMicroCreditProgramArea;
                entity.MaleMicroCreditProgramHO = MFIInfoObject.MaleMicroCreditProgramHO;
                entity.FemaleMicroCreditProgramHO = MFIInfoObject.FemaleMicroCreditProgramHO;
                entity.MaleOrganizationBranch = MFIInfoObject.MaleOrganizationBranch;
                entity.FemaleOrganizationBranch = MFIInfoObject.FemaleOrganizationBranch;
                entity.MaleOrganizationArea = MFIInfoObject.MaleOrganizationArea;
                entity.FemaleOrganizationArea = MFIInfoObject.FemaleOrganizationArea;
                entity.MaleOrganizationHO = MFIInfoObject.MaleOrganizationHO;
                entity.FemaleOrganizationHO = MFIInfoObject.FemaleOrganizationHO;
                entity.HighestMonthlySalary = MFIInfoObject.HighestMonthlySalary;
                entity.HighestMonthlySalaryDesignation = MFIInfoObject.HighestMonthlySalaryDesignation;
                entity.LowestMonthlySalary = MFIInfoObject.LowestMonthlySalary;
                entity.LowestMonthlySalaryDesignation = MFIInfoObject.LowestMonthlySalaryDesignation;
                entity.OtherInformation = MFIInfoObject.OtherInformation;
                entity.IsActive = true;
                entity.CreateDate = DateTime.UtcNow;
                entity.CreateUser = SessionHelper.LoginUserEmployeeID;
                mFIInfoService.Create(entity);
                result = 1;
                message = "Saved successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetMFIInfo(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var mfiInfo = mFIInfoService.GetAll().Where(p => p.IsActive == true).ToList();           
            var currentPageRecords = mfiInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = mfiInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateMFIInfo(MFIInfoViewModel MFIInfoObject)
        {
            var result = 0;
            var message = "";
            try
            {
                var entity = mFIInfoService.GetById(MFIInfoObject.MFIId);
                entity.MFIName = MFIInfoObject.MFIName;
                entity.LicenseNo = MFIInfoObject.LicenseNo;
                entity.DateTo = Convert.ToDateTime(MFIInfoObject.DateTo).Date;
                entity.HOMailingAddress = MFIInfoObject.HOMailingAddress;
                entity.HODistrict = MFIInfoObject.HODistrict;
                entity.HOThana = MFIInfoObject.HOThana;
                entity.HOUnion = MFIInfoObject.HOUnion;
                entity.HOEmail = MFIInfoObject.HOEmail;
                entity.HOWebAddress = MFIInfoObject.HOWebAddress;
                entity.HOPhone = MFIInfoObject.HOPhone;
                entity.HOMobile = MFIInfoObject.HOMobile;
                entity.HOFax = MFIInfoObject.HOFax;
                entity.LOAddress = MFIInfoObject.LOAddress;
                entity.LOTelephone = MFIInfoObject.LOTelephone;
                entity.LOMobile = MFIInfoObject.LOMobile;
                entity.LOFax = MFIInfoObject.LOFax;
                entity.LOEmail = MFIInfoObject.LOEmail;
                entity.PrimaryRegistrationID = MFIInfoObject.PrimaryRegistrationID;
                entity.GBNoOfMaleMember = MFIInfoObject.GBNoOfMaleMember;
                entity.GBNoOfFemaleMember = MFIInfoObject.GBNoOfFemaleMember;
                entity.GBNoOfYearlyMeetingsHeld = MFIInfoObject.GBNoOfYearlyMeetingsHeld;
                entity.GBDateOfLastMeeting = Convert.ToDateTime(MFIInfoObject.GBDateOfLastMeeting).Date;
                entity.GBNoOfParticipantsInTheLastMeeting = MFIInfoObject.GBNoOfParticipantsInTheLastMeeting;
                entity.EBExpiryDate = Convert.ToDateTime(MFIInfoObject.EBExpiryDate).Date;
                entity.EBNoOfMaleMember = MFIInfoObject.EBNoOfMaleMember;
                entity.EBNoOfFemaleMember = MFIInfoObject.EBNoOfFemaleMember;
                entity.EBNoOfYearlyMeetingsHeld = MFIInfoObject.EBNoOfYearlyMeetingsHeld;
                entity.EBDateOfLastMeeting = Convert.ToDateTime(MFIInfoObject.EBDateOfLastMeeting).Date;
                entity.EBNoOfParticipantsInTheLastMeeting = MFIInfoObject.EBNoOfParticipantsInTheLastMeeting;
                entity.ServiceRules = Convert.ToInt32(MFIInfoObject.ServiceRules);
                entity.FinancialPolicy = Convert.ToInt32(MFIInfoObject.FinancialPolicy);
                entity.SavingsAndCreditPolicy = Convert.ToInt32(MFIInfoObject.SavingsAndCreditPolicy);
                entity.NISAndAntiMoneyLaunderingGuideline = Convert.ToInt32(MFIInfoObject.NISAndAntiMoneyLaunderingGuideline);
                entity.CitizenCharter = Convert.ToInt32(MFIInfoObject.CitizenCharter);
                entity.MaleMicroCreditProgramBranch = MFIInfoObject.MaleMicroCreditProgramBranch;
                entity.FemaleMicroCreditProgramBranch = MFIInfoObject.FemaleMicroCreditProgramBranch;
                entity.MaleMicroCreditProgramArea = MFIInfoObject.MaleMicroCreditProgramArea;
                entity.FemaleMicroCreditProgramArea = MFIInfoObject.FemaleMicroCreditProgramArea;
                entity.MaleMicroCreditProgramHO = MFIInfoObject.MaleMicroCreditProgramHO;
                entity.FemaleMicroCreditProgramHO = MFIInfoObject.FemaleMicroCreditProgramHO;
                entity.MaleOrganizationBranch = MFIInfoObject.MaleOrganizationBranch;
                entity.FemaleOrganizationBranch = MFIInfoObject.FemaleOrganizationBranch;
                entity.MaleOrganizationArea = MFIInfoObject.MaleOrganizationArea;
                entity.FemaleOrganizationArea = MFIInfoObject.FemaleOrganizationArea;
                entity.MaleOrganizationHO = MFIInfoObject.MaleOrganizationHO;
                entity.FemaleOrganizationHO = MFIInfoObject.FemaleOrganizationHO;
                entity.HighestMonthlySalary = MFIInfoObject.HighestMonthlySalary;
                entity.HighestMonthlySalaryDesignation = MFIInfoObject.HighestMonthlySalaryDesignation;
                entity.LowestMonthlySalary = MFIInfoObject.LowestMonthlySalary;
                entity.LowestMonthlySalaryDesignation = MFIInfoObject.LowestMonthlySalaryDesignation;
                entity.OtherInformation = MFIInfoObject.OtherInformation;
                entity.IsActive = true;
                entity.CreateDate = DateTime.UtcNow;
                entity.CreateUser = SessionHelper.LoginUserEmployeeID;
                mFIInfoService.Update(entity);
                result = 1;
                message = "Updated successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteMFIInfo(int MFIId)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = mFIInfoService.GetById(MFIId);
                entity.IsActive = false;
                entity.CreateDate = DateTime.UtcNow;
                entity.CreateUser = SessionHelper.LoginUserEmployeeID;
                mFIInfoService.Update(entity);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Reports
        public ActionResult MRAReport()
        {
            var model = new MRAReportViewModel();
            MRAReportList(model);
            return View(model);
        }       
        public ActionResult Print_MRA_DBMS_1_Report(string dateTo)
        {
            try
            {
                var param = new { DateTo = dateTo };
                var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "SP_MFIInformation");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateTo", dateTo);
                ReportHelper.PrintReport("MRA_DBMS_1_New.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public ActionResult Print_MRA_DBMS_3_Report(string dateTo)
        //{
        //    try
        //    {
        //        var orgId = SessionHelper.LoginUserOrganizationID;
        //        var office = SessionHelper.LoginUserOfficeID;
        //        var param = new { Org = orgId, Office = office, Date = dateTo };
        //        var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "Proc_Set_MRA_MIS_03A_Part01");
        //        var reportParam = new Dictionary<string, object>();
        //        var subReportDB = new Dictionary<string, DataTable>();
        //        var qType = 1;
        //        var paramSubReport = new { OrgId = orgId, OfficeId = office, DateTo = dateTo, QType = qType };
        //        var subReport = ultimateReportService.GetDataWithParameterMRA(paramSubReport, "RPT_SavingsSize_Buro");
        //        subReportDB.Add("DistributionOfSavingBySize", subReport.Tables[0]);
        //        ReportHelper.PrintWithSubReport("MRA_DBMS_3.rpt", mainReport.Tables[0], reportParam, subReportDB, new MRA_DBMS_3());
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult Print_MRA_DBMS_4E_Report(string dateFrom, string dateTo)
        //{
        //    try
        //    {
        //        var orgId = SessionHelper.LoginUserOrganizationID;
        //        var office = SessionHelper.LoginUserOfficeID;
        //        var qType = 1;
        //        var param = new { OrgId = orgId, OfficeId = office, DateTo = dateTo, QType = qType };
        //        var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "RPT_LoanSize_MRA");
        //        var reportParam = new Dictionary<string, object>();

        //        var subReportDB = new Dictionary<string, DataTable>();
        //        var qType1 = 1;
        //        var paramSubReportForQType1 = new { OfficeID = office, OfficeIDTO = office, CenterID = 00, CenterIDTo = 99999999, StaffID = 00, StaffIDTo = 99999999, productID = 00, ProductIDTo = 99999999, DateFrom = dateFrom, DateTo = dateTo, Qtype = qType1 };
        //        var subReportData1 = ultimateReportService.GetDataWithParameterMRA(paramSubReportForQType1, "DBMS4E");

        //        var qType2 = 2;
        //        var paramSubReportForQType2 = new { OfficeID = office, OfficeIDTO = office, CenterID = 00, CenterIDTo = 99999999, StaffID = 00, StaffIDTo = 99999999, productID = 00, ProductIDTo = 99999999, DateFrom = dateFrom, DateTo = dateTo, Qtype = qType2 };
        //        var subReportData2 = ultimateReportService.GetDataWithParameterMRA(paramSubReportForQType2, "DBMS4EReport");
        //        subReportDB.Add("LLP_WrittenOff", subReportData2.Tables[0]);

        //        ReportHelper.PrintWithSubReport("MRA_DBMS_4E.rpt", mainReport.Tables[0], reportParam, subReportDB, new MRA_DBMS_4E());
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public ActionResult Print_MRA_DBMS_3_Report(string dateTo)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var office = SessionHelper.LoginUserOfficeID;
                var param = new { Org = orgId, Office = office, Date = dateTo };
                var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "Proc_Set_MRA_MIS_03A_Part01__New");
                var reportParam = new Dictionary<string, object>();
                var subReportDB = new Dictionary<string, DataTable>();
                var qType = 1;
                var paramSubReport = new { OrgId = orgId, OfficeId = office, DateTo = dateTo, QType = qType };
                var subReport = ultimateReportService.GetDataWithParameterMRA(paramSubReport, "RPT_SavingsSize_Buro__New");
                subReportDB.Add("DistributionOfSavingBySize", subReport.Tables[0]);
                ReportHelper.PrintWithSubReport("MRA_DBMS_3_New.rpt", mainReport.Tables[0], reportParam, subReportDB, new MRA_DBMS_3_New());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Print_MRA_DBMS_4E_Report(string dateFrom, string dateTo)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var office = SessionHelper.LoginUserOfficeID;
                var qType = 1;
                var param = new { OrgId = orgId, OfficeId = office, DateTo = dateTo, QType = qType };
                var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "RPT_LoanSize_MRA__New");
                var reportParam = new Dictionary<string, object>();

                var subReportDB = new Dictionary<string, DataTable>();
                var qType1 = 1;
                var paramSubReportForQType1 = new { OfficeID = office, OfficeIDTO = office, CenterID = 00, CenterIDTo = 99999999, StaffID = 00, StaffIDTo = 99999999, productID = 00, ProductIDTo = 99999999, DateFrom = dateFrom, DateTo = dateTo, Qtype = qType1 };
                var subReportData1 = ultimateReportService.GetDataWithParameterMRA(paramSubReportForQType1, "DBMS4E");

                var qType2 = 2;
                var paramSubReportForQType2 = new { OfficeID = office, OfficeIDTO = office, CenterID = 00, CenterIDTo = 99999999, StaffID = 00, StaffIDTo = 99999999, productID = 00, ProductIDTo = 99999999, DateFrom = dateFrom, DateTo = dateTo, Qtype = qType2 };
                var subReportData2 = ultimateReportService.GetDataWithParameterMRA(paramSubReportForQType2, "DBMS4EReport__New");
                subReportDB.Add("LLP_WrittenOff", subReportData2.Tables[0]);

                ReportHelper.PrintWithSubReport("MRA_DBMS_4E_New.rpt", mainReport.Tables[0], reportParam, subReportDB, new MRA_DBMS_4E_New());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Print_MRA_DBMS_5_Report(string dateTo)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var office = SessionHelper.LoginUserOfficeID;
                var param = new { OrgId = orgId, OfficeId = office, Date = dateTo };
                var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "Consolidated_Financial_Position");
                var reportParam = new Dictionary<string, object>();
                var orgName = (ApplicationSettings.OrganiztionName);
                reportParam.Add("CompanyName", orgName);
                reportParam.Add("Date", dateTo);
                ReportHelper.PrintReport("MRA_DBMS_5.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Print_MRA_DBMS_6_Report(string dateTo)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var office = SessionHelper.LoginUserOfficeID;
                var param = new { OrgId = orgId, OfficeId = office, Date = dateTo };
                var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "Income_Expenditure_Statement");
                var reportParam = new Dictionary<string, object>();
                var orgName = (ApplicationSettings.OrganiztionName);
                reportParam.Add("CompanyName", orgName);
                reportParam.Add("Date", dateTo);
                ReportHelper.PrintReport("MRA_DBMS_6.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Print_MRA_DBMS_7_Report(string dateTo)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var office = SessionHelper.LoginUserOfficeID;
                var param = new { OrgId = orgId, OfficeId = office, DateTo = dateTo };
                var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "SP_Risk_Fund_Reporting");
                var reportParam = new Dictionary<string, object>();
                var subReportDB = new Dictionary<string, DataTable>();
                var paramSubReport = new { OfficeID = office, Date = dateTo };
                var subReport = ultimateReportService.GetDataWithParameterMRA(paramSubReport, "SP_Welfare_Activity_Detail");
                subReportDB.Add("WelfareActivitiesReporting", subReport.Tables[0]);
                ReportHelper.PrintWithSubReport("MRA_DBMS_7.rpt", mainReport.Tables[0], reportParam, subReportDB, new MRA_DBMS_7());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Print_MRA_DBMS_8_Report(string dateTo)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var office = SessionHelper.LoginUserOfficeID;
                var param = new { OrgId = orgId, OfficeID = office, DateTo = dateTo };
                var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "SP_GetRemittanceActivity");
                var reportParam = new Dictionary<string, object>();
                var subReportDB = new Dictionary<string, DataTable>();
                var paramSubReport = new { OrgId = orgId, OfficeID = office, Date = dateTo };
                var subReport = ultimateReportService.GetDataWithParameterMRA(paramSubReport, "SP_GetTrainingSummary");
                subReportDB.Add("TrainingSummaryReporting", subReport.Tables[0]);
                ReportHelper.PrintWithSubReport("MRA_DBMS_8.rpt", mainReport.Tables[0], reportParam, subReportDB, new MRA_DBMS_8());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void MapDropdownOffice(MRAReportViewModel model)
        {
            var office = officeService.GetAll().Where(p => p.IsActive == true && p.OfficeLevel == 4 && p.OfficeID==LoginUserOfficeID).ToList();
            var viewOfficeList = office.AsEnumerable().Select(p => new SelectListItem()
            {
                Text=p.OfficeName,
                Value=p.OfficeID.ToString()
            }).ToList();
            var officeList = new List<SelectListItem>();
            officeList.Add(new SelectListItem { Text = "Please Select", Value = "", Selected=true });
            //officeList.Add(new SelectListItem { Text = "All Office", Value = "0"});
            officeList.AddRange(viewOfficeList);
            model.OfficeList = officeList;
        }
        //public ActionResult Print_MRA_DBMS_4_Report()
        //{
        //    var model = new MRAReportViewModel();
        //    MapDropdownOffice(model);
        //    return View(model);
        //}

        public ActionResult Print_MRA_DBMS_4_Report()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["MainProductList"] = items;

            var model = new MRAReportViewModel();
            MapDropdownOffice(model);
            return View(model);
        }

        public ActionResult DBMS_2()
        {
            var model = new MRAReportViewModel();
            MapDropdownOffice(model);
            return View(model);
        }
        //public ActionResult MRA_DBMS_4_Report_Print(string date, int office)
        //{
        //    try
        //    {
        //        var param = new { date = date, OfficeID = office, OfficeIDTo = office };
        //        if (office == 0)
        //        {
        //           param = new { date = date, OfficeID = 00000, OfficeIDTo = 99999 };
        //        }                
        //        var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "Proc_get_MRA4");
        //        var reportParam = new Dictionary<string, object>();
        //        var orgName = (ApplicationSettings.OrganiztionName);
        //        reportParam.Add("CompanyName", orgName);
        //        reportParam.Add("DateTo", date);
        //        ReportHelper.PrintReport("rptMRADBMS4.rpt", mainReport.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public ActionResult MRA_DBMS_4_Report_Print(string date, int office, string mainProdCode)
        {
            try
            {
                var param = new { date = date, OfficeID = office, OfficeIDTo = office, @MainProductCode = mainProdCode };
                if (office == 0)
                {
                    param = new { date = date, OfficeID = 00000, OfficeIDTo = 99999, @MainProductCode = mainProdCode };
                }
                var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "Proc_get_MRA4_New");
                var reportParam = new Dictionary<string, object>();
                var orgName = (ApplicationSettings.OrganiztionName);
                reportParam.Add("CompanyName", orgName);
                reportParam.Add("DateTo", date);
                ReportHelper.PrintReport("rptMRADBMS4_New.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult MRA_DBMS_4_Report_Print_Excel(string date, int office, string mainProdCode)
        {
            try
            {
                var vOffice = officeService.GetById(office);

                if (mainProdCode == "0")
                    mainProdCode = " ";

                var reportTitle = $"{vOffice.OfficeCode}_{vOffice.OfficeName}_{mainProdCode}_MRA_DBMS_4_{DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz")}";

                var param = new { date = date, OfficeID = office, OfficeIDTo = office, @MainProductCode = mainProdCode };
                if (office == 0)
                {
                    param = new { date = date, OfficeID = 00000, OfficeIDTo = 99999, @MainProductCode = mainProdCode };
                }
                var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "Proc_get_MRA4_New");
                var reportParam = new Dictionary<string, object>();
                var orgName = (ApplicationSettings.OrganiztionName);
                reportParam.Add("CompanyName", orgName);
                reportParam.Add("DateTo", date);
                ReportHelper.ExportExcelReportFormated("rptMRADBMS4_New_Excel.rpt", mainReport.Tables[0], reportParam, reportTitle);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetMainProductCode()
        {
            try
            {

                List<ProductViewModel> productList = new List<ProductViewModel>();
                var empList = ultimateReportService.GetDataWithoutParameter("Get_Main_ProductCode");

                productList = empList.Tables[0].AsEnumerable()
                .Select(row => new ProductViewModel
                {
                    MainProductCode = row.Field<string>("ProductCode"),
                    MainItemName = row.Field<string>("ProductName"),
                }).ToList();



                var viewProduct = productList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.MainProductCode.ToString(),
                    Text = x.MainProductCode.ToString() + ", " + x.MainItemName.ToString()
                });

                var ddlProduct = new List<SelectListItem>();
                ddlProduct.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
                ddlProduct.AddRange(viewProduct);
                return Json(ddlProduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MRA_DBMS_2_Report_Print(string date, int office)
        {
            try
            {
                var param = new { Org = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, Date = date };
                var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "Rpt_WorkingArea_DBMS_2");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateTo", date);
                var subReportDB = new Dictionary<string, DataTable>();
                var paramSubReport = new { Org = LoggedInOrganizationID, OfficeID = office, Date = date };
                var subReport = ultimateReportService.GetDataWithParameterMRA(paramSubReport, "Rpt_WorkingArea_DBMS_2");
                subReportDB.Add("DistributionofSavingBySize ", subReport.Tables[0]);
                ReportHelper.PrintWithSubReport("rptMRADBMS2.rpt", mainReport.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMRADBMS2());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MFIMonthlyInformation()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            }           
            if (!IsDayInitiated)
            {                
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }            
            return View();
        }
        public ActionResult GenerateMFIMonthlyInformationReport(string office_id, string from_date, string to_date)
        {
            try
            {
                var param = new { FirstDate = from_date, Date= to_date, OfficeID = office_id, };
                var mainReport = ultimateReportService.GetDataWithParameterMRA(param, "RPT_MRA_MFI_MonthlyInformation_HO");
                var reportParam = new Dictionary<string, object>();
                var orgName = (ApplicationSettings.OrganiztionName);
                reportParam.Add("CompanyName", orgName);
                reportParam.Add("DateTo", to_date);
                //ReportHelper.PrintReport("RPT_MFI_MonthlyInformation.rpt", mainReport.Tables[0], reportParam);
                ReportHelper.PrintReport("rpt_mfiInfo.rpt", mainReport.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
using gBanker.Data;
using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Service.StoredProcedure;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using gBanker.Data.CodeFirstMigration.Db;

namespace gBanker.Web.Controllers
{

    //15 April 2019
    public class FixedAssetController : BaseController
    {

        #region Variables
        private readonly IWeeklyReportService weeklyReportService;
        private readonly IDailyReportService dailyReportService;
        private readonly IAssetGroupInfoService assetGroupInfoService;
        private readonly IAccChartService accChartService;
        private readonly ILastAssetCodeInfoService lastAssetCodeInfoService;
        private readonly IAssetInfoService assetInfoService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IAssetClientInfoService assetClientInfoService;
        private readonly ITransactionTypeService transactionTypeService;
        private readonly IFixAssetUpdatesService fixAssetUpdatesService;
        private readonly IDailyTransactionService dailyTransactionService;
        private readonly IAssetRegisterService assetRegisterService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IOfficeService officeService;
        private readonly IDepreciationMethodService depreciationMethodService;
        private readonly IClientTypeService clientTypeService;
        private readonly IProjectInfoService projectInfoService;
        private readonly IAssetValuerService assetValuerService;
        private readonly IAssetTransferService assetTransferService;
        private readonly IAssetRevaluationService assetRevaluationService;
        private readonly IAssetOutService assetOutService;
        private readonly IAssetPartialOutService assetPartialOutService;
        private readonly IAssetOverhaulingService assetOverhaulingService;
        private readonly IDepriciationRateChangeService depriciationRateChangeService;
        private readonly IAssetProcessInfoService assetProcessInfoService;
        private readonly IAssetUserDepartmentService assetUserDepartmentService;
        private readonly IAssetUserDesignationService assetUserDesignationService;
        private readonly IAssetUserService assetUserService;
        private readonly IEmployeeService employeeService;
        private readonly IDayInitialService dayInitialService;
        //private readonly IAssetDepreciationInfoService assetDepreciationInfoService;
        public FixedAssetController(IWeeklyReportService weeklyReportService, IDailyReportService dailyReportService, IAssetGroupInfoService assetGroupInfoService,
            IAccChartService accChartService, ILastAssetCodeInfoService lastAssetCodeInfoService, IAssetInfoService assetInfoService, IEmployeeSPService employeeSPService,
            IAssetClientInfoService assetClientInfoService, ITransactionTypeService transactionTypeService, IFixAssetUpdatesService fixAssetUpdatesService,
            IDailyTransactionService dailyTransactionService, IAssetRegisterService assetRegisterService, IUltimateReportService ultimateReportService, IOfficeService officeService,
            IDepreciationMethodService depreciationMethodService, IClientTypeService clientTypeService, IProjectInfoService projectInfoService, IAssetValuerService assetValuerService,
            IAssetTransferService assetTransferService, IAssetRevaluationService assetRevaluationService, IAssetOutService assetOutService, IAssetPartialOutService assetPartialOutService,
            IAssetOverhaulingService assetOverhaulingService, IDepriciationRateChangeService depriciationRateChangeService, IAssetProcessInfoService assetProcessInfoService,
            IAssetUserDepartmentService assetUserDepartmentService, IAssetUserDesignationService assetUserDesignationService, IAssetUserService assetUserService, IEmployeeService employeeService, IDayInitialService dayInitialService
            //, IAssetDepreciationInfoService assetDepreciationInfoService
            )
        {
            this.weeklyReportService = weeklyReportService;
            this.dailyReportService = dailyReportService;
            this.assetGroupInfoService = assetGroupInfoService;
            this.accChartService = accChartService;
            this.lastAssetCodeInfoService = lastAssetCodeInfoService;
            this.assetInfoService = assetInfoService;
            this.employeeSPService = employeeSPService;
            this.assetClientInfoService = assetClientInfoService;
            this.transactionTypeService = transactionTypeService;
            this.fixAssetUpdatesService = fixAssetUpdatesService;
            this.dailyTransactionService = dailyTransactionService;
            this.assetRegisterService = assetRegisterService;
            this.ultimateReportService = ultimateReportService;
            this.officeService = officeService;
            this.depreciationMethodService = depreciationMethodService;
            this.clientTypeService = clientTypeService;
            this.projectInfoService = projectInfoService;
            this.assetValuerService = assetValuerService;
            this.assetTransferService = assetTransferService;
            this.assetRevaluationService = assetRevaluationService;
            this.assetOutService = assetOutService;
            this.assetPartialOutService = assetPartialOutService;
            this.assetOverhaulingService = assetOverhaulingService;
            this.depriciationRateChangeService = depriciationRateChangeService;
            this.assetProcessInfoService = assetProcessInfoService;
            this.assetUserDepartmentService = assetUserDepartmentService;
            this.assetUserDesignationService = assetUserDesignationService;
            this.assetUserService = assetUserService;
            this.employeeService = employeeService;
            this.dayInitialService = dayInitialService;
            //this.assetDepreciationInfoService = assetDepreciationInfoService;
        }

        #endregion

        #region Events
        private void OfficeWiseDropDownList()
        {
            IEnumerable<SelectListItem> items = new SelectList("");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["OrganizerList"] = items;
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
        }
        private void ToOfficeWiseDropDownList()
        {
            IEnumerable<SelectListItem> items2 = new SelectList("");
            ViewData["HOListTo"] = items2;
            ViewData["ZoneListTo"] = items2;
            ViewData["AreaListTo"] = items2;
            ViewData["OfficeListTo"] = items2;
            ViewData["OrganizerListTo"] = items2;
            var offcdetailTo = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevelTo"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetailTo.OfficeLevel == 1)
            {
                ViewData["FirstLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.FirstLevel).OfficeID;
                ViewData["SecondLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.FirstLevel).OfficeID;
                ViewData["ThirdLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.FirstLevel).OfficeID;
                ViewData["FourthLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.FirstLevel).OfficeID;
            }
            else if (offcdetailTo.OfficeLevel == 2)
            {
                ViewData["FirstLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.FirstLevel).OfficeID;
                ViewData["SecondLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.SecondLevel).OfficeID;
                ViewData["ThirdLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.SecondLevel).OfficeID;
                ViewData["FourthLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.SecondLevel).OfficeID;
            }
            else if (offcdetailTo.OfficeLevel == 3)
            {
                ViewData["FirstLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.FirstLevel).OfficeID;
                ViewData["SecondLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.SecondLevel).OfficeID;
                ViewData["ThirdLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.ThirdLevel).OfficeID;
                ViewData["FourthLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.FirstLevel).OfficeID;
                ViewData["SecondLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.SecondLevel).OfficeID;
                ViewData["ThirdLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.ThirdLevel).OfficeID;
                ViewData["FourthLevelTo"] = officeService.GetByOfficeCode(offcdetailTo.FourthLevel).OfficeID;
            }
        }
        public ActionResult ItemCodeEntry(FixedAssetViewModel model)
        {
            var accList = new List<SelectListItem>();
            var accCode = accChartService.GetMany(p => p.IsActive == true && p.CategoryID == 1).ToList();
            var accCodeList = accCode.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = string.Format("{0}-{1}", p.AccCode, p.AccName),
                Value = p.AccCode
            }).ToList();
            accList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            accList.AddRange(accCodeList);
            model.AccountCodeList = accList;
            return View(model);
        }
        public ActionResult ClientInfoEntry()
        {
            var model = new FixedAssetViewModel();
            MapDropdownForClientType(model);
            return View(model);
        }
        public ActionResult FixedAssetIn()
        {
            OfficeWiseDropDownList();
            var model = new FixedAssetViewModel();
            GetDropDownForOpeningBalance(model);
            return View(model);
        }
        public ActionResult FixedAssetOut()
        {
            OfficeWiseDropDownList();
            return View(this.GetDropdownDataAssetOut());
        }
        public ActionResult FixedAssetReports()
        {
            return View(this.DropdownForFixedAssetDepriciationRegisterReport());
        }
        public ActionResult AssetInformationEntry()
        {
            var model = new FixedAssetViewModel();
            MapDropdownForAssetInfo(model);
            return View(model);
        }
        public ActionResult TransactionType()
        {
            var model = new FixedAssetViewModel();
            MapDropdownForTransactionType(model);
            return View(model);
        }
        public ActionResult FixAssetUpdates()
        {
            var model = new FixedAssetViewModel();
            GetDropDownForOpeningBalance(model);
            return View(model);
        }
        public ActionResult ProjectInfo()
        {
            return View();
        }
        public ActionResult AssetValuer()
        {
            return View();
        }
        public ActionResult AssetTransfer()
        {
            OfficeWiseDropDownList();
            ToOfficeWiseDropDownList();
            var entity = new FixedAssetViewModel();
            MapDropdownForAssetTransfer(entity);
            return View(entity);
        }
        public ActionResult AssetRevaluation()
        {
            var entity = new FixedAssetViewModel();
            MapDropdownForAssetRevaluation(entity);
            return View(entity);
        }
        public ActionResult AssetPartialOut()
        {
            OfficeWiseDropDownList();
            return View(this.GetDropdownDataAssetOut());
        }
        public ActionResult AssetOverhauling()
        {
            OfficeWiseDropDownList();
            var model = new FixedAssetViewModel();
            MapDropdownForAssetOverhauling(model);
            return View(model);
        }
        public ActionResult DepriciationRateChange()
        {
            var model = new FixedAssetViewModel();
            MapDropdownForDepriciationRateChange(model);
            return View(model);
        }
        public ActionResult ClientType()
        {
            return View();
        }
        public ActionResult ProcessFile()
        {
            return View();
        }
        public ActionResult YearEndProcess()
        {
            OfficeWiseDropDownList();
            return View();
        }
        public JsonResult RunYearEndProcess(int office_id)
        {
            var message = "";
            try
            {
                var createUser = SessionHelper.LoggedInEmployeeID;
                var orgID = SessionHelper.LoginUserOrganizationID;
                var param = new { OfficeId = office_id, LastMonthEndDate=TransactionDate, OrgID = orgID, CreateUser = createUser };
                ultimateReportService.GetDataWithParameter(param, "fix.CalculateDepriciation");
                message = "Process have Done Successfully";                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteYearEndProcess(int office_id)
        {
            var message = "";
            try
            {
                var createUser = SessionHelper.LoggedInEmployeeID;
                var orgID = SessionHelper.LoginUserOrganizationID;
                var param = new { OfficeId = office_id, LastMonthEndDate = TransactionDate, OrgID = orgID, CreateUser = createUser };
                ultimateReportService.GetDataWithParameter(param, "fix.DeleteCalculateDepriciation");
                message = "Process Deleted Successfully";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssetUser()
        {
            var model = new FixedAssetViewModel();
            MapDropDownForAssetUser(model);
            return View(model);
        }

        public ActionResult AssetInfoCorrection()
        {
            OfficeWiseDropDownList();
            var model = new FixedAssetViewModel();
            GetDropDownForOpeningBalance(model);
            return View(model);
        }
        #endregion

        #region Methods

        #region CommonMethods
        private FixedAssetViewModel GetDropdownDataAssetIn()
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var fixedAssetViewModel = new FixedAssetViewModel();
            var transactionTypeList = transactionTypeService.GetMany(p => p.IsActive == true && p.TransactionTypeInOut == "FI").ToList();
            var view_transactionTypeList = transactionTypeList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.TransactionName,
                Value = p.TransactionCode
            }).ToList();

            var TransactionTypeList = new List<SelectListItem>();
            TransactionTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            TransactionTypeList.AddRange(view_transactionTypeList);
            fixedAssetViewModel.TransactionTypeList = TransactionTypeList;

            var serialList = new List<SelectListItem>();
            serialList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            fixedAssetViewModel.AssetSerialList = serialList;

            var assetCodeList = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var view_assetCodeList = assetCodeList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.AssetCode, p.AssetName),
                Value = p.AssetID.ToString()
            }).ToList();
            var AssetCodeList = new List<SelectListItem>();
            AssetCodeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            AssetCodeList.AddRange(view_assetCodeList);
            fixedAssetViewModel.AssetCodeList = AssetCodeList;

            var UseableList = new List<SelectListItem>();
            UseableList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            UseableList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            UseableList.Add(new SelectListItem { Text = "No", Value = "0" });
            fixedAssetViewModel.UsableList = UseableList;

            var suppList = assetClientInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var view_suppList = suppList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.AssetClientCode, p.AssetClientName),
                Value = p.AssetClientCode
            }).ToList();
            var clientList = new List<SelectListItem>();
            clientList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            clientList.AddRange(view_suppList);
            fixedAssetViewModel.ClientList = clientList;

            var transactionDate = SessionHelper.TransactionDate;
            ViewData["TrxDate"] = transactionDate.ToString("dd-MMM-yyyy");

            return fixedAssetViewModel;

        }
        private void MapDropdownForAssetInfo(FixedAssetViewModel model)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var assetGroupInfo = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).OrderBy(p => p.AssetGroupCode).ToList();
            var View_assetGroupInfo = assetGroupInfo.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.AssetGroupCode,
                Value = p.AssetGroupID.ToString()
            }).ToList();
            var assetGroupList = new List<SelectListItem>();
            assetGroupList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            assetGroupList.AddRange(View_assetGroupInfo);
            model.AssetCodeList = assetGroupList;

            var assetGroupName = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var View_assetGroupName = assetGroupInfo.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.AssetGroupName,
                Value = p.AssetGroupName
            }).ToList();
            var assetGroupNameList = new List<SelectListItem>();
            assetGroupNameList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            assetGroupNameList.AddRange(View_assetGroupName);
            model.GroupNameList = assetGroupNameList;

            var depList = new List<SelectListItem>();
            depList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            depList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            depList.Add(new SelectListItem { Text = "No", Value = "0" });
            model.DepriciableList = depList;

            var depMethod = depreciationMethodService.GetMany(p => p.IsActive == true).ToList();
            var View_depMethod = depMethod.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.DepriciationName,
                Value = p.Id.ToString()
            }).ToList();
            var depMethodList = new List<SelectListItem>();
            depMethodList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            depMethodList.AddRange(View_depMethod);
            model.DepriciationMethodList = depMethodList;
        }
        private void MapDropdownForClientType(FixedAssetViewModel model)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            var clientList = clientTypeService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var View_ClientList = clientList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.ClientCategory,
                Value = p.ClientTypeID.ToString()
            }).ToList();
            var clientTypeList = new List<SelectListItem>();
            clientTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            clientTypeList.AddRange(View_ClientList);
            model.ClientList = clientTypeList;
        }
        private void MapDropdownForTransactionType(FixedAssetViewModel model)
        {
            var trTypeList = new List<SelectListItem>();
            trTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            trTypeList.Add(new SelectListItem { Text = "Fixed Asset In", Value = "FI" });
            trTypeList.Add(new SelectListItem { Text = "Fixed Asset Out", Value = "FO" });
            model.TransactionTypeList = trTypeList;
        }
        private FixedAssetViewModel GetDropDownFixAssetUpdates()
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var fixedAssetViewModel = new FixedAssetViewModel();
            var assetInfo = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var view_AssetInfo = assetInfo.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = string.Format("{0}-{1}", p.AssetCode, p.AssetName),
                Value = p.AssetID.ToString()
            }).ToList();
            var assetCodeList = new List<SelectListItem>();
            assetCodeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            assetCodeList.AddRange(view_AssetInfo);
            fixedAssetViewModel.AssetCodeList = assetCodeList;

            var clientInfo = assetClientInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var view_ClientInfo = clientInfo.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = string.Format("{0}-{1}", p.AssetClientCode, p.AssetClientName),
                Value = p.AssetClientCode
            }).ToList();
            var clientCodelIst = new List<SelectListItem>();
            clientCodelIst.Add(new SelectListItem { Text = "Please Select", Value = "" });
            clientCodelIst.AddRange(view_ClientInfo);
            fixedAssetViewModel.ClientList = clientCodelIst;

            var tranType = transactionTypeService.GetMany(p => p.IsActive == true).ToList();
            var view_TranType = tranType.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = string.Format("{0}-{1}", p.TransactionCode, p.TransactionName),
                Value = p.TransactionCode
            }).ToList();
            var tranTypeList = new List<SelectListItem>();
            tranTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            tranTypeList.AddRange(view_TranType);
            fixedAssetViewModel.TransactionTypeList = tranTypeList;

            var usable = new List<SelectListItem>();
            usable.Add(new SelectListItem { Text = "Please Select", Value = "" });
            usable.Add(new SelectListItem { Text = "Yes", Value = "1" });
            usable.Add(new SelectListItem { Text = "No", Value = "0" });
            fixedAssetViewModel.UsableList = usable;

            var transactionDate = SessionHelper.TransactionDate;
            ViewData["TrxDate"] = transactionDate.ToString("dd-MMM-yyyy");

            return fixedAssetViewModel;
        }
        [HttpPost]
        public JsonResult SaveToRegister(FixedAssetViewModel FixedAssetIn)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var checkDuplicate = assetRegisterService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId && p.AssetInfoID == Convert.ToInt64(FixedAssetIn.AssetCode) && p.AssetSerial == FixedAssetIn.AssetSerial && p.TranType == FixedAssetIn.TransactionType && p.OfficeID == officeId).ToList();
            if (checkDuplicate.Any())
            {
                result = 0;
                message = "Duplicate Asset Info found, save denied";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var model = new AssetRegister();
                model.AssetInfoID = Convert.ToInt64(FixedAssetIn.AssetCode);
                model.AssetSerial = FixedAssetIn.AssetSerial;
                model.VoucherNo = FixedAssetIn.VoucherNo;
                model.UnitPrice = FixedAssetIn.UnitPrice;
                model.TransactionValue = FixedAssetIn.TransactionValue;
                model.Depriciation = FixedAssetIn.CurrentDepri;
                //model.TranType = FixedAssetIn.TransactionType;
                model.AssetClientId = Convert.ToInt64(FixedAssetIn.AssetClientCode);
                model.Remarks = FixedAssetIn.Remarks;
                model.OrgID = orgID;
                model.OfficeID = officeId;
                model.IsActive = true;
                model.InActiveDate = DateTime.Now;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.Now;
                assetRegisterService.Create(model);
                result = 1;
                message = "Saved successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LoadAssetSerialWiseAssetInfo(string assetSerial)
        {
            var serialWiseAssetInfo = fixAssetUpdatesService.GetMany(p => p.IsActive == true && p.AssetSerial == assetSerial).ToList();
            return Json(serialWiseAssetInfo, JsonRequestBehavior.AllowGet);
        }
        private void MapDropDownForAssetUser(FixedAssetViewModel model)
        {
            var deptList = assetUserDepartmentService.GetMany(p => p.IsActive == true).ToList();
            var viewDeptList = deptList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.DepartmentName,
                Value = p.DepartmentID.ToString()
            });
            var departmentList = new List<SelectListItem>();
            departmentList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            departmentList.AddRange(viewDeptList);
            model.DepartmentList = departmentList;

            var desigList = employeeService.GetMany(p => p.IsActive == true).ToList();
            var viewDesigList = desigList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.Designation,
                Value = p.Designation
            });
            var designationList = new List<SelectListItem>();
            designationList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            departmentList.AddRange(viewDesigList);
            model.DesignationList = designationList;

            var empList = employeeService.GetMany(p => p.IsActive == true).ToList();
            var viewEmpList = empList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.EmployeeCode, p.EmpName),
                Value = p.EmployeeID.ToString()
            });
            var employeeList = new List<SelectListItem>();
            employeeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            employeeList.AddRange(viewEmpList);
            model.EmployeeList = employeeList;




            //var desigList = assetUserDesignationService.GetMany(p => p.IsActive == true).ToList();
            //var ViewdesigList = desigList.AsEnumerable().Select(p => new SelectListItem
            //{
            //    Text = p.DesignationName,
            //    Value = p.DesignationID.ToString()
            //});
            //var designationList = new List<SelectListItem>();
            //designationList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            //designationList.AddRange(ViewdesigList);
            //model.DesignationList = designationList;

            //var employeeList = new List<SelectListItem>();
            //employeeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            //employeeList.Add(new SelectListItem { Text = "Rafee", Value = "1" });
            //employeeList.Add(new SelectListItem { Text = "Sabet", Value = "2" });            
            //model.EmployeeList = employeeList;
        }

        #endregion

        #region AssetGroupInfoEntry

        [HttpPost]
        public JsonResult SaveAssetGroupInformation(FixedAssetViewModel obj)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            //var officeId = SessionHelper.LoginUserOfficeID;

            try
            {
                var checkDuplicateGroupCode = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && (p.AssetGroupCode.ToUpper().Trim() == obj.GroupCode.ToUpper().Trim() || p.AssetGroupName.ToUpper().Trim() == obj.GroupName.ToUpper().Trim())).ToList();
                if (checkDuplicateGroupCode.Any())
                {
                    result = 0;
                    message = "Duplicate Group Info found, save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var model = new AssetGroupInfo();
                    model.AssetGroupCode = obj.GroupCode;
                    model.AssetGroupName = obj.GroupName;
                    model.AssetInAccCode = obj.AccountCode;
                    model.AssetCurDepriDr = obj.DepriciationDebit;
                    model.AssetAccuDepriCr = obj.DepriciationCredit;
                    //model.OfficeID = Convert.ToInt32(officeId);
                    model.OrgID = Convert.ToInt32(orgID);
                    model.IsActive = true;
                    model.CreateDate = DateTime.Now;
                    model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    model.UpdateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    model.UpdateDate = DateTime.Now;
                    assetGroupInfoService.Create(model);
                    result = 1;
                    message = "Saved successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult GetAssetGroupInfo(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            //var officeId = SessionHelper.LoginUserOfficeID;
            var assetGroupInfo = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var currentPageRecords = assetGroupInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = assetGroupInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateAssetGroupInformation(FixedAssetViewModel obj)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            //var officeId = SessionHelper.LoginUserOfficeID;
            try
            {
                var checkDuplicateGroupCode = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.AssetGroupID != obj.AssetGroupInfoID && (p.AssetGroupCode.ToUpper().Trim() == obj.GroupCode.ToUpper().Trim() || p.AssetGroupName.ToUpper().Trim() == obj.GroupName.ToUpper().Trim())).ToList();
                if (checkDuplicateGroupCode.Any())
                {
                    result = 0;
                    message = "Duplicate Group Info found, update denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var model = assetGroupInfoService.GetById(obj.AssetGroupInfoID);
                    model.AssetGroupCode = obj.GroupCode;
                    model.AssetGroupName = obj.GroupName;
                    model.AssetInAccCode = obj.AccountCode;
                    model.AssetCurDepriDr = obj.DepriciationDebit;
                    model.AssetAccuDepriCr = obj.DepriciationCredit;
                    //model.OfficeID = Convert.ToInt32(officeId);
                    model.OrgID = Convert.ToInt32(orgID);
                    model.IsActive = true;
                    model.UpdateDate = DateTime.Now;
                    model.UpdateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    assetGroupInfoService.Update(model);
                    result = 1;
                    message = "Updated successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteGroupInfo(int AssetGroupInfoID)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = assetGroupInfoService.GetById(AssetGroupInfoID);
                model.IsActive = false;
                model.UpdateDate = DateTime.Now;
                model.UpdateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                assetGroupInfoService.Update(model);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AssetInfoEntry
        public JsonResult LoadGroupCodeWiseGroupName(int GroupId)
        {
            var message = "";
            var groupName = string.Empty;
            var groupInfo = assetGroupInfoService.GetMany(p => p.IsActive == true && p.AssetGroupID == GroupId).FirstOrDefault();
            if (groupInfo != null)
            {
                groupName = groupInfo.AssetGroupName;
            }
            var param = new { GroupId = GroupId };
            var assetCode = ultimateReportService.GetDataWithParameter(param, "fix.SP_GetNextAssetCode");
            var nextAssetCode = assetCode.Tables[0].AsEnumerable().Select(p => p.Field<string>("NextAssetCode"));
            return Json(new { message = message, groupName = groupName, nextAssetCode = nextAssetCode }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveAssetInformation(FixedAssetViewModel obj)
        {
            var message = "";
            var result = 0;
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var checkDuplicate = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.AssetGroupID == obj.GroupId && (p.AssetCode == obj.AssetCode || p.AssetName.ToUpper().Trim() == obj.AssetName.ToUpper().Trim())).ToList();
            if (checkDuplicate.Any())
            {
                result = 0;
                message = "Duplicate Asset Info found, save denied";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var model = new AssetInfo();
                model.AssetGroupID = obj.GroupId;
                model.AssetCode = obj.AssetCode;
                model.AssetName = obj.AssetName;
                model.Depritiable = obj.Depriciable;
                model.Deprate = obj.DepriciationRate;
                model.DepriciationMethod = Convert.ToInt32(obj.DepriciationMethod);
                model.IsActive = true;
                model.OrgID = Convert.ToInt32(orgID);
                //model.OfficeID = Convert.ToInt32(officeId);
                model.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                model.CreateDate = DateTime.Now;
                model.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                model.UpdateDate = DateTime.Now;
                assetInfoService.Create(model);
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
        public JsonResult GetGroupWiseAssetInfo(int jtStartIndex, int jtPageSize, string jtSorting, int GroupId)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var assetInfo = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.AssetGroupID == GroupId).ToList();
            var assetGroupInfo = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.AssetGroupID == GroupId).ToList();
            var viewAssetInfo = (from ai in assetInfo
                                 join gi in assetGroupInfo on ai.AssetGroupID equals gi.AssetGroupID
                                 select new FixedAssetViewModel
                                 {
                                     GroupId = gi.AssetGroupID,
                                     GroupName = gi.AssetGroupName,
                                     AssetInfoID = ai.AssetID,
                                     AssetCode = ai.AssetCode,
                                     AssetName = ai.AssetName,
                                     Depritiable = ai.Depritiable,
                                     Deprate = ai.Deprate,
                                     DepriciationMethod = (ai.DepriciationMethod).ToString(),
                                     AssetGroupInfoCode = gi.AssetGroupCode
                                 }).ToList();
            var currentPageRecords = viewAssetInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewAssetInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateAssetInformation(FixedAssetViewModel obj)
        {
            var message = "";
            var result = 0;
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var checkDuplicate = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.AssetID != obj.AssetInfoID && p.AssetGroupID == obj.GroupId && (p.AssetCode == obj.AssetCode || p.AssetName.ToUpper().Trim() == obj.AssetName.ToUpper().Trim())).ToList();
            if (checkDuplicate.Any())
            {
                result = 0;
                message = "Duplicate Entry found, update denied";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var model = assetInfoService.GetById(Convert.ToInt32(obj.AssetInfoID));
                model.AssetGroupID = obj.GroupId;
                model.AssetCode = obj.AssetCode;
                model.AssetName = obj.AssetName;
                model.Depritiable = obj.Depriciable;
                model.Deprate = obj.DepriciationRate;
                model.DepriciationMethod = Convert.ToInt32(obj.DepriciationMethod);
                model.IsActive = true;
                model.OrgID = Convert.ToInt32(orgID);
                //model.OfficeID = Convert.ToInt32(officeId);
                model.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                model.UpdateDate = DateTime.Now;
                assetInfoService.Update(model);
                result = 1;
                message = "Updated successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteAssetInfo(int AssetInfoID)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = assetInfoService.GetById(AssetInfoID);
                model.IsActive = false;
                model.CreateDate = DateTime.Now;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                assetInfoService.Update(model);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ClientCategoryEntry

        public JsonResult SaveClientInformation(string clientCategory)
        {
            var message = "";
            var result = 0;
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            try
            {
                var checkDuplicate = clientTypeService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.ClientCategory.ToUpper().Trim() == clientCategory);
                if (checkDuplicate.Any())
                {
                    result = 1;
                    message = "Duplicate Client Category found, Save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var entity = new ClientType();
                    entity.ClientCategory = clientCategory;
                    entity.IsActive = true;
                    entity.OrgID = Convert.ToInt32(orgID);
                    //entity.OfficeID = Convert.ToInt32(officeId);
                    entity.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                    entity.CreateDate = DateTime.Now;
                    entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                    entity.UpdateDate = DateTime.Now;
                    clientTypeService.Create(entity);
                    result = 1;
                    message = "Saved successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult GetClientCategory(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            var clientCategory = clientTypeService.GetMany(p => p.IsActive == true && p.OrgID == orgID);
            var currentPageRecords = clientCategory.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = clientCategory.LongCount(), JsonRequestBehavior.AllowGet });
        }
        public JsonResult UpdateClientCategory(int ClientTypeID, string clientCategory)
        {
            var message = "";
            var result = 0;
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            try
            {
                var checkDuplicate = clientTypeService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.ClientTypeID != ClientTypeID && p.ClientCategory.ToUpper().Trim() == clientCategory);
                if (checkDuplicate.Any())
                {
                    result = 1;
                    message = "Duplicate Client Category found, Save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var entity = clientTypeService.GetById(ClientTypeID);
                    entity.ClientCategory = clientCategory;
                    entity.IsActive = true;
                    entity.OrgID = Convert.ToInt32(orgID);
                    //entity.OfficeID = Convert.ToInt32(officeId);
                    entity.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                    entity.CreateDate = DateTime.Now;
                    entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                    entity.UpdateDate = DateTime.Now;
                    clientTypeService.Update(entity);
                    result = 1;
                    message = "Updated successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteClientCategoryInfo(int ClientTypeID)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = clientTypeService.GetById(ClientTypeID);
                model.IsActive = false;
                model.CreateDate = DateTime.Now;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                clientTypeService.Update(model);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region ClientInfoEntry

        [HttpPost]
        public JsonResult SaveClientInformation(FixedAssetViewModel AssetClientInfo)
        {
            var result = 0;
            var message = "";

            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;
                var officeID = SessionHelper.LoginUserOfficeID;
                //var checkDuplicate = assetClientInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && (p.AssetClientCode == AssetClientInfo.AssetClientCode || p.AssetClientName == AssetClientInfo.AssetClientName) || p.BusLicNo == AssetClientInfo.BusLicNo || p.VATRegistrationNo == AssetClientInfo.VATRegistrationNo || p.TIN == AssetClientInfo.TIN || p.Phone == AssetClientInfo.Phone || p.Email.ToUpper().Trim() == AssetClientInfo.Email.ToUpper().Trim()).ToList();
                var checkDuplicate = assetClientInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.BusLicNo == AssetClientInfo.BusLicNo && (p.AssetClientCode == AssetClientInfo.AssetClientCode || p.AssetClientName == AssetClientInfo.AssetClientName) || p.BusLicNo == AssetClientInfo.BusLicNo || p.VATRegistrationNo == AssetClientInfo.VATRegistrationNo || p.TIN == AssetClientInfo.TIN || p.Phone == AssetClientInfo.Phone || p.Email.ToUpper().Trim() == AssetClientInfo.Email.ToUpper().Trim()).ToList();
                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Duplicate Client Info found, save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var entity = new AssetClientInfo();
                    entity.AssetClientCode = AssetClientInfo.AssetClientCode;
                    entity.AssetClientName = AssetClientInfo.AssetClientName;
                    entity.ClientType = AssetClientInfo.ClientType;
                    entity.AssetClientAddress = AssetClientInfo.AssetClientAddress;
                    entity.BusLicNo = AssetClientInfo.BusLicNo;
                    entity.VATRegistrationNo = AssetClientInfo.VATRegistrationNo;
                    entity.CorporateStatus = AssetClientInfo.CorporateStatus;
                    entity.BusinessExperience = AssetClientInfo.BusinessExperience;
                    entity.TIN = AssetClientInfo.TIN;
                    entity.Phone = AssetClientInfo.Phone;
                    entity.Email = AssetClientInfo.Email;
                    entity.Remarks = AssetClientInfo.Remarks;
                    entity.OrgID = orgID;
                    //entity.OfficeID = officeID;
                    entity.IsActive = true;
                    entity.InActiveDate = DateTime.Now;
                    entity.CreateDate = DateTime.Now;
                    entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    assetClientInfoService.Create(entity);
                    result = 1;
                    message = "Saved successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult GetClientInfo(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeID = SessionHelper.LoginUserOfficeID;
            var clientInfo = assetClientInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var currentPageRecords = clientInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = clientInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult UpdateClientInformation(FixedAssetViewModel AssetClientInfo)
        {
            var result = 0;
            var message = "";

            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;
                var officeID = SessionHelper.LoginUserOfficeID;

                //var checkDuplicate = assetClientInfoService.GetMany(p => p.IsActive == true && (p.OrgID == orgID && p.AssetClientInfoID != AssetClientInfo.AssetClientInfoID && (p.AssetClientCode == AssetClientInfo.AssetClientCode || p.AssetClientName == AssetClientInfo.AssetClientName)) || p.BusLicNo == AssetClientInfo.BusLicNo || p.TIN == AssetClientInfo.TIN || p.Phone == AssetClientInfo.Phone || p.Email.ToUpper().Trim() == AssetClientInfo.Email.ToUpper().Trim()).ToList();
                //if (checkDuplicate.Any())
                //{
                //    result = 0;
                //    message = "Duplicate Client Info found, update denied";
                //    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                var entity = assetClientInfoService.GetById(Convert.ToInt32(AssetClientInfo.AssetClientInfoID));
                entity.AssetClientCode = AssetClientInfo.AssetClientCode;
                entity.AssetClientName = AssetClientInfo.AssetClientName;
                entity.ClientType = AssetClientInfo.ClientType;
                entity.AssetClientAddress = AssetClientInfo.AssetClientAddress;
                entity.BusLicNo = AssetClientInfo.BusLicNo;
                entity.VATRegistrationNo = AssetClientInfo.VATRegistrationNo;
                entity.CorporateStatus = AssetClientInfo.CorporateStatus;
                entity.BusinessExperience = AssetClientInfo.BusinessExperience;
                entity.TIN = AssetClientInfo.TIN;
                entity.Phone = AssetClientInfo.Phone;
                entity.Email = AssetClientInfo.Email;
                entity.Remarks = AssetClientInfo.Remarks;
                entity.OrgID = orgID;
                //entity.OfficeID = officeID;
                entity.IsActive = true;
                entity.InActiveDate = DateTime.Now;
                entity.CreateDate = DateTime.Now;
                entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                assetClientInfoService.Update(entity);
                result = 1;
                message = "Updated successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteClientInfo(int AssetClientInfoID)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = assetClientInfoService.GetById(AssetClientInfoID);
                model.IsActive = false;
                model.CreateDate = DateTime.Now;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                assetClientInfoService.Update(model);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region TransactionTypeEntry

        [HttpPost]
        public JsonResult SaveTransactionTypeInformation(FixedAssetViewModel TransactionTypeInfo)
        {
            var result = 0;
            var message = "";
            try
            {
                var checkDuplicate = transactionTypeService.GetMany(p => p.IsActive == true && (p.TransactionCode == TransactionTypeInfo.TransactionCode || p.TransactionName == TransactionTypeInfo.TransactionName)).ToList();
                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Duplicate Transaction Type found, save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var model = new TransactionType();
                    model.TransactionCode = TransactionTypeInfo.TransactionCode;
                    model.TransactionName = TransactionTypeInfo.TransactionName;
                    model.TransactionTypeInOut = TransactionTypeInfo.TransactionType;
                    model.IsActive = true;
                    model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.Now;
                    transactionTypeService.Create(model);
                    result = 1;
                    message = "Saved successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetTransactionTypeInfo(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var tranTypeList = transactionTypeService.GetMany(p => p.IsActive == true).ToList();
            var currentPageRecords = tranTypeList.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = tranTypeList.LongCount(), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult UpdateTransactionTypeInformation(FixedAssetViewModel TransactionTypeInfo)
        {
            var result = 0;
            var message = "";

            try
            {
                var checkDuplicate = transactionTypeService.GetMany(p => p.IsActive == true && p.TransactionId != TransactionTypeInfo.TransactionId && (p.TransactionCode == TransactionTypeInfo.TransactionCode || p.TransactionName == TransactionTypeInfo.TransactionName)).ToList();
                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Duplicate Transaction Type found, update denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var model = transactionTypeService.GetById(TransactionTypeInfo.TransactionId);
                    model.TransactionCode = TransactionTypeInfo.TransactionCode;
                    model.TransactionName = TransactionTypeInfo.TransactionName;
                    model.TransactionTypeInOut = TransactionTypeInfo.TransactionType;
                    model.IsActive = true;
                    model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    model.CreateDate = DateTime.Now;
                    transactionTypeService.Update(model);
                    result = 1;
                    message = "Updated successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteTransactionTypeInfo(int TransactionId)
        {
            var result = 0;
            var message = "";

            var model = transactionTypeService.GetById(TransactionId);
            model.IsActive = false;
            model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
            model.CreateDate = DateTime.Now;
            transactionTypeService.Update(model);
            result = 1;
            message = "Deleted successfully";
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region FixedAssetOpeningEntry

        private void GetDropDownForOpeningBalance(FixedAssetViewModel model)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var assetGroup = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).OrderBy(p => p.AssetGroupCode);
            var viewAssetGroup = assetGroup.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.AssetGroupCode, p.AssetGroupName),
                Value = p.AssetGroupID.ToString()
            });
            var groupList = new List<SelectListItem>();
            groupList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            groupList.AddRange(viewAssetGroup);
            model.AssetGroupIdList = groupList;

            var depMethod = depreciationMethodService.GetMany(p => p.IsActive == true);
            var viewDepMethod = depMethod.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.DepriciationName,
                Value = p.Id.ToString()
            });
            var deprList = new List<SelectListItem>();
            deprList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            deprList.AddRange(viewDepMethod);
            model.DepriciationMethodList = deprList;

            var AssetCodeList = new List<SelectListItem>();
            AssetCodeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            model.AssetCodeList = AssetCodeList;

            var serialList = new List<SelectListItem>();
            serialList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            model.AssetSerialList = serialList;

            var UseableList = new List<SelectListItem>();
            UseableList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            UseableList.Add(new SelectListItem { Text = "Yes", Value = "1", Selected = true });
            UseableList.Add(new SelectListItem { Text = "No", Value = "0" });
            model.UsableList = UseableList;

            var suppList = assetClientInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var view_suppList = suppList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.AssetClientCode, p.AssetClientName),
                Value = p.AssetClientInfoID.ToString(),
            }).ToList();
            var clientList = new List<SelectListItem>();
            //clientList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            clientList.AddRange(view_suppList);
            model.ClientList = clientList;

            var tranType = transactionTypeService.GetMany(p => p.TransactionTypeInOut == "FI").ToList();
            var view_TranType = tranType.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = string.Format("{0}-{1}", p.TransactionCode, p.TransactionName),
                Value = p.TransactionId.ToString()
            }).ToList();
            var tranTypeList = new List<SelectListItem>();
            //tranTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            tranTypeList.AddRange(view_TranType);
            model.TransactionTypeList = tranTypeList;

            var assetUsers = employeeService.GetAll().Where(x => x.OfficeID == SessionHelper.LoginUserOfficeID);
            var userList = assetUsers.Select(x => x).ToList().Select(x => new SelectListItem()
            {
                Text = String.Format("{0} - {1}", x.EmployeeCode, x.EmpName),
                Value = x.EmployeeID.ToString()
            });

            var employeeList = new List<SelectListItem>();
            employeeList.AddRange(userList);           
            model.EmployeeList = employeeList;

            var proj = projectInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var viewProj = proj.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.ProjectName,
                Value = p.ProjectID.ToString()
            });
            var projectList = new List<SelectListItem>();
            projectList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            projectList.AddRange(viewProj);
            model.ProjectList = projectList;

            //var transactionDate = SessionHelper.TransactionDate;
            //ViewData["TrxDate"] = transactionDate.ToString("dd-MMM-yyyy");


        }
        public JsonResult GetOfficeWiseAssetUserList(int officeID)
        {
            var empList = employeeService.GetMany(p => p.IsActive == true && p.OfficeID == officeID).ToList();
            var viewEmpList = empList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.EmployeeCode, p.EmpName),
                Value = p.EmployeeID.ToString()
            });
            var employeeList = new List<SelectListItem>();
            employeeList.Add(new SelectListItem { Text = "This Office", Value = "0", Selected = true });
            employeeList.AddRange(viewEmpList);
            return Json(employeeList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveFixedAssetUpdates(FixedAssetViewModel fixedAssetUpdates)
        {
            var result = 0;
            var message = "";
            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;
                var officeId = SessionHelper.LoginUserOfficeID;
                var assetID = Convert.ToInt64(fixedAssetUpdates.AssetCode);
                var checkDuplicate = fixAssetUpdatesService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId && p.AssetSerial == fixedAssetUpdates.AssetSerial && p.AssetID == assetID).ToList();
                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Duplicate Asset entry found, save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var entity = new FixAssetUpdates();
                    entity.TransactionDate = Convert.ToDateTime(fixedAssetUpdates.TransactionDate);
                    entity.AssetGroupID = fixedAssetUpdates.AssetGroupID;
                    entity.AssetID = Convert.ToInt64(fixedAssetUpdates.AssetCode);
                    entity.AssetSerial = fixedAssetUpdates.AssetSerial;
                    entity.VoucherNo = fixedAssetUpdates.VoucherNo;
                    entity.UnitPrice = fixedAssetUpdates.UnitPrice;
                    entity.AccumulatedDepri = fixedAssetUpdates.AccumulatedDepri;
                    entity.CurrentDepri = fixedAssetUpdates.CurrentDepri;
                    entity.BookValue = fixedAssetUpdates.BookValue;
                    entity.AssetClientId = Convert.ToInt64(fixedAssetUpdates.AssetClientCode);
                    entity.TransactionType = fixedAssetUpdates.TransactionType;
                    entity.AssetUser = fixedAssetUpdates.AssetUser;
                    entity.Usable = Convert.ToBoolean(fixedAssetUpdates.Usable);
                    //entity.DepCalcDate = Convert.ToDateTime(fixedAssetUpdates.DepCalcDate);
                    entity.OrgID = orgID;
                    entity.OfficeID = Convert.ToInt32(officeId);
                    entity.PurchaseDate = Convert.ToDateTime(fixedAssetUpdates.PurchaseDate).Date;
                    entity.OperationDate = Convert.ToDateTime(fixedAssetUpdates.OperationDate).Date;
                    entity.InstallationCost = fixedAssetUpdates.InstallationCost;
                    entity.CarringCost = fixedAssetUpdates.CarringCost;
                    entity.OtherCost = fixedAssetUpdates.OtherCost;
                    entity.TotalCost = fixedAssetUpdates.TotalCost;
                    entity.ProjectID = fixedAssetUpdates.ProjectID;
                    entity.TotalOpeningBalanceCost = fixedAssetUpdates.TotalOpeningBalanceCost;
                    entity.DepriciationRate = fixedAssetUpdates.Deprate;
                    entity.DepriciationMethod = Convert.ToInt32(fixedAssetUpdates.DepriciationMethod);
                    entity.IsCapitalizedAsset = fixedAssetUpdates.IsCapitalizedAsset;
                    entity.OpeningDepriciationBalance = fixedAssetUpdates.OpeningDepriciationBalance;
                    entity.OpeningBookValue = fixedAssetUpdates.OpeningBookValue;
                    entity.InsuranceValue = fixedAssetUpdates.InsuranceValue;
                    entity.InsuranceExpDate = Convert.ToDateTime(fixedAssetUpdates.InsuranceExpDate);
                    entity.WarrantyGurantee = fixedAssetUpdates.WarrantyGurantee;
                    entity.UsefulLifeYear = fixedAssetUpdates.UsefulLifeYear;
                    entity.PurchaseOrderNo = fixedAssetUpdates.PurchaseOrderNo;
                    entity.PurchaseOrderDate = Convert.ToDateTime(fixedAssetUpdates.PurchaseOrderDate);
                    entity.DepriciationClosingDate = Convert.ToDateTime(fixedAssetUpdates.DepriciationClosingDate);
                    entity.IsActive = true;
                    entity.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                    entity.CreateDate = DateTime.Now;
                    entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                    entity.UpdateDate = DateTime.Now;
                    fixAssetUpdatesService.Create(entity);
                    result = 1;
                    message = "Saved successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetFixedAssetUpdates(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;


            var fixAsstUpdates = fixAssetUpdatesService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId);
            var assetGroupInfo = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID);
            var assetInfo = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID);

            var viewAssetOpeningInfo = (from p in fixAsstUpdates
                                        join ag in assetGroupInfo on p.AssetGroupID equals ag.AssetGroupID
                                        join ai in assetInfo on p.AssetID equals ai.AssetID
                                        select new FixedAssetViewModel
                                        {
                                            FixAssetUpdateID = p.FixAssetUpdateID,
                                            TransactionDate = p.TransactionDate.ToString("dd-MMM-yyyy"),
                                            AssetGroupID = p.AssetGroupID,
                                            GroupName = ag.AssetGroupName,
                                            AssetID = Convert.ToInt32(p.AssetID),
                                            AssetName = ai.AssetName,
                                            AssetSerial = p.AssetSerial,
                                            VoucherNo = p.VoucherNo,
                                            UnitPrice = p.UnitPrice,
                                            AccumulatedDepri = p.AccumulatedDepri,
                                            CurrentDepri = p.CurrentDepri,
                                            BookValue = p.BookValue,
                                            ClientCode = p.AssetClientId.ToString(),
                                            TransactionType = p.TransactionType,
                                            AssetUser = p.AssetUser,
                                            Usable = p.Usable,
                                            //DepCalcDate = p.DepCalcDate.ToString("dd-MMM-yyyy"),
                                            PurchaseDate = Convert.ToDateTime(p.PurchaseDate).ToString("dd-MMM-yyyy"),
                                            OperationDate = Convert.ToDateTime(p.OperationDate).ToString("dd-MMM-yyyy"),
                                            InstallationCost = p.InstallationCost,
                                            CarringCost = p.CarringCost,
                                            OtherCost = p.OtherCost,
                                            TotalOpeningBalanceCost = p.TotalOpeningBalanceCost,
                                            Deprate = p.DepriciationRate,
                                            OpeningDepriciationBalance = p.OpeningDepriciationBalance,
                                            OpeningBookValue = p.OpeningBookValue,
                                            InsuranceValue = p.InsuranceValue,
                                            //InsuranceExpDate = Convert.ToDateTime(p.InsuranceExpDate).ToString("dd-MMM-yyyy"),
                                            WarrantyGurantee = p.WarrantyGurantee,
                                            UsefulLifeYear = p.UsefulLifeYear,
                                            PurchaseOrderNo = p.PurchaseOrderNo,
                                            DepriciationClosingDate = Convert.ToDateTime(p.DepriciationClosingDate).ToString("dd-MMM-yyyy"),
                                            //PurchaseOrderDate = (p.PurchaseOrderDate).ToString("dd-MMM-yyyy"),
                                        });
            var currentPageRecords = viewAssetOpeningInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewAssetOpeningInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult UpdateFixedAssetUpdatedInfo(FixedAssetViewModel fixedAssetUpdates)
        {
            var result = 0;
            var message = "";
            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;
                var officeId = SessionHelper.LoginUserOfficeID;
                var assetID = Convert.ToInt64(fixedAssetUpdates.AssetCode);
                var checkDuplicate = fixAssetUpdatesService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId && p.FixAssetUpdateID != fixedAssetUpdates.FixAssetUpdateID && p.AssetSerial == fixedAssetUpdates.AssetSerial && p.AssetID == assetID && p.OfficeID == officeId).ToList();
                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Duplicate Asset entry found, update denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var entity = fixAssetUpdatesService.GetById(fixedAssetUpdates.FixAssetUpdateID);
                    entity.TransactionDate = Convert.ToDateTime(fixedAssetUpdates.TransactionDate);
                    entity.AssetGroupID = fixedAssetUpdates.AssetGroupID;
                    entity.AssetID = Convert.ToInt64(fixedAssetUpdates.AssetCode);
                    entity.AssetSerial = fixedAssetUpdates.AssetSerial;
                    entity.VoucherNo = fixedAssetUpdates.VoucherNo;
                    entity.UnitPrice = fixedAssetUpdates.UnitPrice;
                    entity.AccumulatedDepri = fixedAssetUpdates.AccumulatedDepri;
                    entity.CurrentDepri = fixedAssetUpdates.CurrentDepri;
                    entity.BookValue = fixedAssetUpdates.BookValue;
                    entity.AssetClientId = Convert.ToInt64(fixedAssetUpdates.AssetClientCode);
                    entity.TransactionType = fixedAssetUpdates.TransactionType;
                    entity.AssetUser = fixedAssetUpdates.AssetUser;
                    entity.Usable = Convert.ToBoolean(fixedAssetUpdates.Usable);
                    //entity.DepCalcDate = Convert.ToDateTime(fixedAssetUpdates.DepCalcDate);
                    entity.OrgID = orgID;
                    entity.OfficeID = Convert.ToInt32(officeId);
                    entity.PurchaseDate = Convert.ToDateTime(fixedAssetUpdates.PurchaseDate).Date;
                    entity.OperationDate = Convert.ToDateTime(fixedAssetUpdates.OperationDate);
                    entity.InstallationCost = fixedAssetUpdates.InstallationCost;
                    entity.CarringCost = fixedAssetUpdates.CarringCost;
                    entity.OtherCost = fixedAssetUpdates.OtherCost;
                    entity.TotalOpeningBalanceCost = fixedAssetUpdates.TotalOpeningBalanceCost;
                    entity.DepriciationRate = fixedAssetUpdates.Deprate;
                    entity.OpeningDepriciationBalance = fixedAssetUpdates.OpeningDepriciationBalance;
                    entity.OpeningBookValue = fixedAssetUpdates.OpeningBookValue;
                    entity.InsuranceValue = fixedAssetUpdates.InsuranceValue;
                    entity.InsuranceExpDate = Convert.ToDateTime(fixedAssetUpdates.InsuranceExpDate);
                    entity.WarrantyGurantee = fixedAssetUpdates.WarrantyGurantee;
                    entity.UsefulLifeYear = fixedAssetUpdates.UsefulLifeYear;
                    entity.PurchaseOrderNo = fixedAssetUpdates.PurchaseOrderNo;
                    entity.PurchaseOrderDate = Convert.ToDateTime(fixedAssetUpdates.PurchaseOrderDate);
                    entity.DepriciationClosingDate = Convert.ToDateTime(fixedAssetUpdates.DepriciationClosingDate);
                    entity.IsActive = true;
                    entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                    entity.UpdateDate = DateTime.Now;
                    fixAssetUpdatesService.Update(entity);
                    result = 1;
                    message = "Updated successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteFixedAssetUpdates(int FixAssetUpdateID)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = fixAssetUpdatesService.GetById(FixAssetUpdateID);
                entity.IsActive = false;
                entity.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.CreateDate = DateTime.Now;
                fixAssetUpdatesService.Update(entity);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AssetDailyTransactionEntry        

        /*
        private DailyTransaction SaveSerialMultiple(string[] serials, FixedAssetViewModel fixedAssetUpdates, int orgID)
        {
            var firstSerial = serials[0].Split('.');
            var lastSerial = serials[1].Split('.');

            var calculateFirst = Convert.ToDecimal(firstSerial[4].ToString());
            var calculateLast = Convert.ToDecimal(lastSerial[4].ToString());

            var gapBetween = calculateLast - calculateFirst;

            var getData = new DailyTransaction();

            for (int i = 0; i <= gapBetween; i++)
            {
                var newString = string.Empty;
                if (i == 0)
                {
                    newString = firstSerial[0] + "." + firstSerial[1] + "." + firstSerial[2] + "." + firstSerial[3] + "." + (calculateFirst).ToString().PadLeft(5, '0');
                }
                else
                {
                    newString = firstSerial[0] + "." + firstSerial[1] + "." + firstSerial[2] + "." + firstSerial[3] + "." + (calculateFirst + i).ToString().PadLeft(5, '0');
                }

                var entity = new DailyTransaction();
                entity.TransactionDate = Convert.ToDateTime(fixedAssetUpdates.TransactionDate);
                entity.AssetGroupID = fixedAssetUpdates.AssetGroupID;
                entity.AssetID = Convert.ToInt64(fixedAssetUpdates.AssetCode);
                entity.AssetSerial = newString.Trim();
                entity.AssetDescription = fixedAssetUpdates.AssetDescription;
                entity.PurchasePrice = fixedAssetUpdates.UnitPrice;
                entity.AssetClientId = Convert.ToInt64(fixedAssetUpdates.AssetClientCode);
                entity.TransactionType = Convert.ToInt32(fixedAssetUpdates.TransactionType);
                entity.AssetUser = fixedAssetUpdates.AssetUser;
                entity.Usable = Convert.ToBoolean(fixedAssetUpdates.Usable);
                entity.DepCalcDate = Convert.ToDateTime(fixedAssetUpdates.DepCalcDate);
                entity.OrgID = orgID;
                entity.OfficeID = fixedAssetUpdates.OfficeID;
                entity.PurchaseDate = Convert.ToDateTime(fixedAssetUpdates.PurchaseDate);
                entity.OperationDate = Convert.ToDateTime(fixedAssetUpdates.OperationDate);
                entity.InstallationCost = fixedAssetUpdates.InstallationCost;
                entity.CarringCost = fixedAssetUpdates.CarringCost;
                entity.OtherCost = fixedAssetUpdates.OtherCost;
                entity.TotalCost = fixedAssetUpdates.TotalCost;
                entity.ProjectID = fixedAssetUpdates.ProjectID;
                entity.DepriciationMethod = Convert.ToInt32(fixedAssetUpdates.DepriciationMethod);
                entity.IsCapitalizedAsset = fixedAssetUpdates.IsCapitalizedAsset;
                entity.IsInstallmentAsset = fixedAssetUpdates.IsInstallmentAsset;
                entity.DepriciationRate = fixedAssetUpdates.Deprate;
                entity.InsuranceValue = fixedAssetUpdates.InsuranceValue;
                entity.InsuranceExpDate = fixedAssetUpdates.InsuranceExpDate;
                entity.WarrantyGurantee = fixedAssetUpdates.WarrantyGurantee;
                entity.UsefulLifeYear = fixedAssetUpdates.UsefulLifeYear;
                entity.PurchaseOrderNo = fixedAssetUpdates.PurchaseOrderNo;
                entity.PurchaseOrderDate = fixedAssetUpdates.PurchaseOrderDate;
                entity.Remarks = fixedAssetUpdates.Remarks;
                entity.AssetStatus = Convert.ToInt32(fixedAssetUpdates.TransactionType);
                entity.IsActive = true;
                entity.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.CreateDate = DateTime.Now;
                entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.UpdateDate = DateTime.Now;

                entity.DownPayment = fixedAssetUpdates.DownPayment;
                entity.InstallmentNumber = fixedAssetUpdates.InstallmentNo;
                entity.InstallmentAmount = fixedAssetUpdates.InstallmentAmount;
                entity.BankName = fixedAssetUpdates.BankName;
                entity.AccCardNo = fixedAssetUpdates.AccountNo;

                getData = dailyTransactionService.AddNewDailyTransaction(entity);


            }
            return getData;
        }
        */

        private DailyTransaction SaveSerialMultiple(string[] serials, FixedAssetViewModel fixedAssetUpdates, int orgID)
        {
            var firstSerial = serials[0].Split('.');
            var lastSerial = serials[1].Split('.');

            var calculateFirst = Convert.ToDecimal(firstSerial[3].ToString());
            var calculateLast = Convert.ToDecimal(lastSerial[3].ToString());

            var gapBetween = calculateLast - calculateFirst;

            var getData = new DailyTransaction();

            for (int i = 0; i <= gapBetween; i++)
            {
                var newString = string.Empty;
                if (i == 0)
                {
                    newString = firstSerial[0] + "." + firstSerial[1] + "." + firstSerial[2] + "." + "." + (calculateFirst).ToString().PadLeft(5, '0');
                }
                else
                {
                    newString = firstSerial[0] + "." + firstSerial[1] + "." + firstSerial[2] + "." + "." + (calculateFirst + i).ToString().PadLeft(5, '0');
                }

                var entity = new DailyTransaction();
                entity.TransactionDate = Convert.ToDateTime(fixedAssetUpdates.TransactionDate);
                entity.AssetGroupID = fixedAssetUpdates.AssetGroupID;
                entity.AssetID = Convert.ToInt64(fixedAssetUpdates.AssetCode);
                entity.AssetSerial = newString.Trim();
                entity.AssetDescription = fixedAssetUpdates.AssetDescription;
                entity.PurchasePrice = fixedAssetUpdates.UnitPrice;
                entity.AssetClientId = Convert.ToInt64(fixedAssetUpdates.AssetClientCode);
                entity.TransactionType = Convert.ToInt32(fixedAssetUpdates.TransactionType);
                entity.AssetUser = fixedAssetUpdates.AssetUser;
                entity.Usable = Convert.ToBoolean(fixedAssetUpdates.Usable);
                entity.DepCalcDate = Convert.ToDateTime(fixedAssetUpdates.DepCalcDate);
                entity.OrgID = orgID;
                entity.OfficeID = fixedAssetUpdates.OfficeID;
                entity.PurchaseDate = Convert.ToDateTime(fixedAssetUpdates.PurchaseDate);
                entity.OperationDate = Convert.ToDateTime(fixedAssetUpdates.OperationDate);
                entity.InstallationCost = fixedAssetUpdates.InstallationCost;
                entity.CarringCost = fixedAssetUpdates.CarringCost;
                entity.OtherCost = fixedAssetUpdates.OtherCost;
                entity.TotalCost = fixedAssetUpdates.TotalCost;
                entity.ProjectID = fixedAssetUpdates.ProjectID;
                entity.DepriciationMethod = Convert.ToInt32(fixedAssetUpdates.DepriciationMethod);
                entity.IsCapitalizedAsset = fixedAssetUpdates.IsCapitalizedAsset;
                entity.IsInstallmentAsset = fixedAssetUpdates.IsInstallmentAsset;
                entity.DepriciationRate = fixedAssetUpdates.Deprate;
                entity.InsuranceValue = fixedAssetUpdates.InsuranceValue;
                entity.InsuranceExpDate = fixedAssetUpdates.InsuranceExpDate;
                entity.WarrantyGurantee = fixedAssetUpdates.WarrantyGurantee;
                entity.UsefulLifeYear = fixedAssetUpdates.UsefulLifeYear;
                entity.PurchaseOrderNo = fixedAssetUpdates.PurchaseOrderNo;
                entity.PurchaseOrderDate = fixedAssetUpdates.PurchaseOrderDate;
                entity.Remarks = fixedAssetUpdates.Remarks;
                entity.AssetStatus = Convert.ToInt32(fixedAssetUpdates.TransactionType);
                entity.IsActive = true;
                entity.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.CreateDate = DateTime.Now;
                entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.UpdateDate = DateTime.Now;

                entity.DownPayment = fixedAssetUpdates.DownPayment;
                entity.InstallmentNumber = fixedAssetUpdates.InstallmentNo;
                entity.InstallmentAmount = fixedAssetUpdates.InstallmentAmount;
                entity.BankName = fixedAssetUpdates.BankName;
                entity.AccCardNo = fixedAssetUpdates.AccountNo;

                getData = dailyTransactionService.AddNewDailyTransaction(entity);


            }
            return getData;
        }

        private DailyTransaction SaveSerial(FixedAssetViewModel fixedAssetUpdates, int orgID)
        {
            var entity = new DailyTransaction();
            entity.TransactionDate = Convert.ToDateTime(fixedAssetUpdates.TransactionDate);
            entity.AssetGroupID = fixedAssetUpdates.AssetGroupID;
            entity.AssetID = Convert.ToInt64(fixedAssetUpdates.AssetCode);
            entity.AssetSerial = fixedAssetUpdates.AssetSerial;
            entity.AssetDescription = fixedAssetUpdates.AssetDescription;
            entity.PurchasePrice = fixedAssetUpdates.UnitPrice;
            entity.AssetClientId = Convert.ToInt64(fixedAssetUpdates.AssetClientCode);
            entity.TransactionType = Convert.ToInt32(fixedAssetUpdates.TransactionType);
            entity.AssetUser = fixedAssetUpdates.AssetUser;
            entity.Usable = Convert.ToBoolean(fixedAssetUpdates.Usable);
            entity.DepCalcDate = Convert.ToDateTime(fixedAssetUpdates.DepCalcDate);
            entity.OrgID = orgID;
            entity.OfficeID = fixedAssetUpdates.OfficeID;
            entity.PurchaseDate = Convert.ToDateTime(fixedAssetUpdates.PurchaseDate);
            entity.OperationDate = Convert.ToDateTime(fixedAssetUpdates.OperationDate);
            entity.InstallationCost = fixedAssetUpdates.InstallationCost;
            entity.CarringCost = fixedAssetUpdates.CarringCost;
            entity.OtherCost = fixedAssetUpdates.OtherCost;
            entity.TotalCost = fixedAssetUpdates.TotalCost;
            entity.ProjectID = fixedAssetUpdates.ProjectID;
            entity.DepriciationMethod = Convert.ToInt32(fixedAssetUpdates.DepriciationMethod);
            entity.IsCapitalizedAsset = fixedAssetUpdates.IsCapitalizedAsset;
            entity.IsInstallmentAsset = fixedAssetUpdates.IsInstallmentAsset;
            entity.DepriciationRate = fixedAssetUpdates.Deprate;
            entity.InsuranceValue = fixedAssetUpdates.InsuranceValue;
            entity.InsuranceExpDate = fixedAssetUpdates.InsuranceExpDate;
            entity.WarrantyGurantee = fixedAssetUpdates.WarrantyGurantee;
            entity.UsefulLifeYear = fixedAssetUpdates.UsefulLifeYear;
            entity.PurchaseOrderNo = fixedAssetUpdates.PurchaseOrderNo;
            entity.PurchaseOrderDate = fixedAssetUpdates.PurchaseOrderDate;
            entity.Remarks = fixedAssetUpdates.Remarks;
            entity.AssetStatus = Convert.ToInt32(fixedAssetUpdates.TransactionType);
            entity.IsActive = true;
            entity.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
            entity.CreateDate = DateTime.Now;
            entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
            entity.UpdateDate = DateTime.Now;

            entity.DownPayment = fixedAssetUpdates.DownPayment;
            entity.InstallmentNumber = fixedAssetUpdates.InstallmentNo;
            entity.InstallmentAmount = fixedAssetUpdates.InstallmentAmount;
            entity.BankName = fixedAssetUpdates.BankName;
            entity.AccCardNo = fixedAssetUpdates.AccountNo;            
            entity.selectedOfficeId = fixedAssetUpdates.selectedOfficeId;
            return dailyTransactionService.AddNewDailyTransaction(entity);

        }

        [HttpPost]
        public JsonResult SaveDailyTransactionInfo(FixedAssetViewModel fixedAssetUpdates)
        {
            var result = 0;
            var message = "";
            bool isOperationSuccess = true;
            var getOfficeID = 0; long getAssetID = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var orgID = SessionHelper.LoginUserOrganizationID;
                    var assetID = Convert.ToInt64(fixedAssetUpdates.AssetCode);   

                    if (fixedAssetUpdates.AssetSerial != null && fixedAssetUpdates.AssetSerial != string.Empty)
                    {
                        if (fixedAssetUpdates.AssetSerial.Contains('-'))
                        {
                            var serials = fixedAssetUpdates.AssetSerial.Split('-');
                            if (serials.Any())
                            {
                                var getData = SaveSerialMultiple(serials, fixedAssetUpdates, Convert.ToInt32(orgID));
                                getOfficeID = getData.OfficeID;
                                getAssetID = getData.AssetID;
                            }
                        }
                        else
                        {
                            var getData = SaveSerial(fixedAssetUpdates, Convert.ToInt32(orgID));
                            getOfficeID = getData.OfficeID;
                            getAssetID = getData.AssetID;
                        }
                    }

                    result = 1;
                    message = "Saved successfully";                    
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;                  
                    throw ex;
                }

                if (isOperationSuccess)                
                    scope.Complete();                

                scope.Dispose();
            }

            return Json(new { result = result, message = message, getOfficeID = getOfficeID, getAssetID = getAssetID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDailyTransactionInfo(int jtStartIndex, int jtPageSize, string jtSorting, int officeID, long assetID)
        {
            try
            {
                var param = new { OfficeID = officeID, AssetID = assetID };
                var dailyInfo = ultimateReportService.GetDataWithParameter(param, "fix.GetDailyTransactionInfo");
                var viewDailyInfo = dailyInfo.Tables[0].AsEnumerable().Select(p => new FixedAssetViewModel
                {
                    DailyTransactionId = p.Field<long>("DailyTransactionId"),
                    TransactionDate = p.Field<DateTime>("TransactionDate").ToString("dd-MMM-yyyy"),
                    AssetGroupID = p.Field<int>("AssetGroupID"),
                    GroupName = p.Field<string>("AssetGroupName"),
                    AssetID = Convert.ToInt32(p.Field<long>("AssetID")),
                    AssetName = p.Field<string>("AssetName"),
                    AssetSerial = p.Field<string>("AssetSerial"),
                    AssetDescription = p.Field<string>("AssetDescription"),
                    UnitPrice = p.Field<decimal>("PurchasePrice"),
                    TotalCost = p.Field<decimal?>("TotalCost"),
                    ClientCode = p.Field<long>("AssetClientId").ToString(),
                    TranType = p.Field<int>("TransactionType"),
                    AssetUser = p.Field<string>("AssetUser").ToString(),
                    Usable = p.Field<bool>("Usable"),

                    DepCalcDate = p.Field<DateTime>("DepCalcDate").ToString("dd-MMM-yyyy"),
                    PurchaseDate = Convert.ToDateTime(p.Field<DateTime?>("PurchaseDate")).ToString("dd-MMM-yyyy"),
                    OperationDate = Convert.ToDateTime(p.Field<DateTime?>("OperationDate")).ToString("dd-MMM-yyyy"),
                    InstallationCost = p.Field<decimal?>("InstallationCost"),
                    CarringCost = p.Field<decimal?>("CarringCost"),
                    OtherCost = p.Field<decimal?>("OtherCost"),
                    ProjectID = p.Field<int?>("ProjectID"),
                    Deprate = p.Field<decimal?>("DepriciationRate"),
                    DepriciationMethod = p.Field<int?>("DepriciationMethod").ToString(),
                    InsuranceValue = p.Field<decimal?>("InsuranceValue"),
                    InsExpDate = Convert.ToDateTime(p.Field<DateTime?>("InsuranceExpDate")).ToString("dd-MMM-yyyy"),
                    WarrantyGurantee = p.Field<string>("WarrantyGurantee"),
                    UsefulLifeYear = p.Field<int?>("UsefulLifeYear"),
                    PurchaseOrderNo = p.Field<string>("PurchaseOrderNo"),
                    PurOrderDate = Convert.ToDateTime(p.Field<DateTime?>("PurchaseOrderDate")).ToString("dd-MMM-yyyy"),
                    Remarks = p.Field<string>("Remarks"),

                    DownPayment = p.Field<decimal?>("DownPayment"),
                    InstallmentNo = p.Field<int?>("InstallmentNumber"),
                    InstallmentAmount = p.Field<decimal?>("InstallmentAmount"),
                    BankName = p.Field<string>("BankName"),
                    AccountNo = p.Field<string>("AccCardNo"),
                    OfficeName = p.Field<string>("OfficeName"),
                    selectedOfficeId = p.Field<int>("selectedOfficeId")

                }).ToList();
                var currentPageRecords = viewDailyInfo.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewDailyInfo.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public JsonResult UpdateDailyTransactionInfo(FixedAssetViewModel fixedAssetUpdates)
        {
            var result = 0;
            var message = "";
            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;
                long assetID = Convert.ToInt64(fixedAssetUpdates.AssetCode);
                var checkDuplicate = dailyTransactionService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == fixedAssetUpdates.OfficeID && p.DailyTransactionId != fixedAssetUpdates.DailyTransactionId && p.AssetSerial == fixedAssetUpdates.AssetSerial && p.AssetID == assetID).ToList();
                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Duplicate Asset entry found, update denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var entity = dailyTransactionService.GetById(Convert.ToInt32(fixedAssetUpdates.DailyTransactionId));
                    entity.TransactionDate = Convert.ToDateTime(fixedAssetUpdates.TransactionDate);
                    entity.AssetGroupID = fixedAssetUpdates.AssetGroupID;
                    entity.AssetID = Convert.ToInt64(fixedAssetUpdates.AssetCode);
                    entity.AssetSerial = fixedAssetUpdates.AssetSerial;
                    entity.AssetDescription = fixedAssetUpdates.AssetDescription;
                    entity.PurchasePrice = fixedAssetUpdates.UnitPrice;
                    entity.AssetClientId = Convert.ToInt64(fixedAssetUpdates.AssetClientCode);
                    entity.TransactionType = Convert.ToInt32(fixedAssetUpdates.TransactionType);
                    entity.AssetUser = fixedAssetUpdates.AssetUser;
                    entity.Usable = Convert.ToBoolean(fixedAssetUpdates.Usable);
                    entity.DepCalcDate = Convert.ToDateTime(fixedAssetUpdates.DepCalcDate);
                    entity.PurchaseDate = Convert.ToDateTime(fixedAssetUpdates.PurchaseDate).Date;
                    entity.OperationDate = Convert.ToDateTime(fixedAssetUpdates.OperationDate);
                    entity.InstallationCost = fixedAssetUpdates.InstallationCost;
                    entity.CarringCost = fixedAssetUpdates.CarringCost;
                    entity.OtherCost = fixedAssetUpdates.OtherCost;
                    entity.DepriciationRate = fixedAssetUpdates.Deprate;
                    entity.DepriciationMethod = Convert.ToInt32(fixedAssetUpdates.DepriciationMethod);
                    entity.TotalCost = fixedAssetUpdates.TotalCost;
                    entity.ProjectID = fixedAssetUpdates.ProjectID;
                    entity.OfficeID = fixedAssetUpdates.OfficeID;
                    entity.IsCapitalizedAsset = fixedAssetUpdates.IsCapitalizedAsset;
                    entity.IsInstallmentAsset = fixedAssetUpdates.IsInstallmentAsset;
                    entity.InsuranceValue = fixedAssetUpdates.InsuranceValue;
                    entity.InsuranceExpDate = Convert.ToDateTime(fixedAssetUpdates.InsuranceExpDate);
                    entity.WarrantyGurantee = fixedAssetUpdates.WarrantyGurantee;
                    entity.UsefulLifeYear = fixedAssetUpdates.UsefulLifeYear;
                    entity.PurchaseOrderNo = fixedAssetUpdates.PurchaseOrderNo;
                    entity.PurchaseOrderDate = Convert.ToDateTime(fixedAssetUpdates.PurchaseOrderDate);
                    entity.Remarks = fixedAssetUpdates.Remarks;
                    //entity.IsActive = true;
                    entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                    entity.UpdateDate = DateTime.Now;
                    //entity.OrgID = orgID;
                    entity.DownPayment = fixedAssetUpdates.DownPayment;
                    entity.InstallmentNumber = fixedAssetUpdates.InstallmentNo;
                    entity.InstallmentAmount = fixedAssetUpdates.InstallmentAmount;
                    entity.BankName = fixedAssetUpdates.BankName;
                    entity.AccCardNo = fixedAssetUpdates.AccountNo;
                    entity.selectedOfficeId = fixedAssetUpdates.selectedOfficeId;

                    dailyTransactionService.UpdateDailyTransaction(entity);
                    //dailyTransactionService.Update(entity);

                    //var model = assetDepreciationInfoService.GetById(Convert.ToInt32(fixedAssetUpdates.DailyTransactionId));                  
                    //model.AssetID = Convert.ToInt64(fixedAssetUpdates.AssetCode);
                    //model.AssetSerial = fixedAssetUpdates.AssetSerial;       
                    //assetDepreciationInfoService.UpdateAssetDepreciationInfo(model);

                    var paramForUpdateDepriciationInfo = new { DailyTransactionId = fixedAssetUpdates.DailyTransactionId, AssetID = Convert.ToInt64(fixedAssetUpdates.AssetCode), AssetSerial = fixedAssetUpdates.AssetSerial };
                    ultimateReportService.GetDataWithParameter(paramForUpdateDepriciationInfo, "UpdateAssetDepreciationInfo");

                    result = 1;
                    message = "Updated successfully";
                    return Json(new { result = result, message = message, entity = entity }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteDailyTransactionInfo(int DailyTransactionId)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = dailyTransactionService.GetById(DailyTransactionId);
                entity.IsActive = false;
                entity.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.CreateDate = DateTime.Now;
                dailyTransactionService.Update(entity);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion        

        #region FixedAssetOut

        public JsonResult GetOfficeWiseAssetGroup(int officeID, string assetOutDate)
        {
            var param = new { OfficeID = officeID, OutDate = assetOutDate };
            var officeWiseAssetGroup = ultimateReportService.GetDataWithParameter(param, "fix.SP_OfficeWiseAssetGroup");
            var View_officeWiseAssetGroup = officeWiseAssetGroup.Tables[0].AsEnumerable().Select(p => new FixedAssetViewModel
            {
                AssetGroupID = p.Field<int>("AssetGroupID"),
                GroupCode = p.Field<string>("AssetGroupCode"),
                GroupName = p.Field<string>("AssetGroupName"),
            }).ToList();
            var ddofficeWiseAssetGroup = View_officeWiseAssetGroup.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.GroupCode, p.GroupName),
                Value = p.AssetGroupID.ToString()
            });
            var assetGroupList = new List<SelectListItem>();
            assetGroupList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            assetGroupList.AddRange(ddofficeWiseAssetGroup);
            return Json(assetGroupList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAssetGroupWiseAssetName(int office_id, int groupId, string assetOutDate)
        {
            var param = new { OfficeID = office_id, OutDate = assetOutDate, AssetGroupID = groupId };
            var assetGroupWiseAssetName = ultimateReportService.GetDataWithParameter(param, "fix.SP_AssetGroupWiseAssetName");
            var View_assetGroupWiseAssetName = assetGroupWiseAssetName.Tables[0].AsEnumerable().Select(p => new FixedAssetViewModel
            {
                AssetInfoID = p.Field<long>("AssetID"),
                AssetCode = p.Field<string>("AssetCode"),
                AssetName = p.Field<string>("AssetName"),
            }).ToList();
            var ddassetGroupWiseAssetName = View_assetGroupWiseAssetName.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.AssetCode, p.AssetName),
                Value = p.AssetInfoID.ToString()
            });
            var assetNameList = new List<SelectListItem>();
            assetNameList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            assetNameList.AddRange(ddassetGroupWiseAssetName);
            return Json(assetNameList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAssetWiseAssetSerial(int office_id, string assetOutDate, string assetCode)
        {
            var param = new { OfficeID = office_id, OutDate = assetOutDate, AssetID = assetCode };
            var assetWiseAssetSerial = ultimateReportService.GetDataWithParameter(param, "fix.SP_AssetWiseAssetSerial");
            var View_assetWiseAssetSerial = assetWiseAssetSerial.Tables[0].AsEnumerable().Select(p => new FixedAssetViewModel
            {
                AssetSerial = p.Field<string>("AssetSerial")
            }).ToList();
            var ddassetWiseAssetSerial = View_assetWiseAssetSerial.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.AssetSerial,
                Value = p.AssetSerial
            });
            var assetSerialList = new List<SelectListItem>();
            assetSerialList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            assetSerialList.AddRange(ddassetWiseAssetSerial);
            return Json(assetSerialList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAssetWiseAssetSerialForTransfer(int office_id, string assetOutDate, string assetCode)
        {
            {
                var param = new { OfficeID = office_id, OutDate = assetOutDate, AssetID = assetCode };
                var assetWiseAssetSerial = ultimateReportService.GetDataWithParameter(param, "fix.SP_AssetWiseAssetSerial");
                var View_assetWiseAssetSerial = assetWiseAssetSerial.Tables[0].AsEnumerable().Select(p => new FixedAssetViewModel
                {
                    AssetSerial = p.Field<string>("AssetSerial"),
                    DailyTransactionId = p.Field<long>("DailyTransactionId")
                }).ToList();
                //var ddassetWiseAssetSerial = View_assetWiseAssetSerial.AsEnumerable().Select(p => new SelectListItem
                //{
                //    Text = p.AssetSerial,
                //    Value = p.AssetSerial
                //});
                //var assetSerialList = new List<SelectListItem>();                
                //assetSerialList.AddRange(ddassetWiseAssetSerial);
                return Json(View_assetWiseAssetSerial, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAssetSerialWiseAssetInfo(int office_id, string assetOutDate, string assetSerial)
        {
            var param = new { OfficeID = office_id, OutDate = assetOutDate, AssetSerial = assetSerial };
            var assetSerialWiseAssetInfo = ultimateReportService.GetDataWithParameter(param, "fix.SP_AssetSerialWiseAssetInfo");
            var View_assetSerialWiseAssetInfo = assetSerialWiseAssetInfo.Tables[0].AsEnumerable().Select(p => new FixedAssetViewModel
            {
                TotalCost = p.Field<decimal?>("TotalCost"),
                AccumulatedDepriciation = p.Field<decimal?>("AccumulatedDepriciation"),
                CurrentDepriciation = p.Field<decimal?>("CurrentDep"),
                BookValue = p.Field<decimal?>("BookValue"),
                DailyTransactionId = p.Field<long>("DailyTransactionId"),
                Deprate = p.Field<decimal?>("DepriciationRate")
            }).ToList();

            return Json(View_assetSerialWiseAssetInfo.ToList(), JsonRequestBehavior.AllowGet);
        }
        private FixedAssetViewModel GetDropdownDataAssetOut()
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var fixedAssetViewModel = new FixedAssetViewModel();
            var transactionTypeList = transactionTypeService.GetMany(p => p.IsActive == true && p.TransactionTypeInOut == "FO").ToList();
            var view_transactionTypeList = transactionTypeList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.TransactionName,
                Value = p.TransactionId.ToString()
            }).ToList();

            var TransactionTypeList = new List<SelectListItem>();
            TransactionTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            TransactionTypeList.AddRange(view_transactionTypeList);
            fixedAssetViewModel.TransactionTypeList = TransactionTypeList;

            //var assetGroup = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).OrderBy(p=>p.AssetGroupCode);
            //var viewAssetGroup = assetGroup.AsEnumerable().Select(p => new SelectListItem
            //{
            //    Text = string.Format("{0}-{1}", p.AssetGroupCode, p.AssetGroupName),
            //    Value = p.AssetGroupID.ToString()
            //});
            var groupList = new List<SelectListItem>();
            groupList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            //groupList.AddRange(viewAssetGroup);
            fixedAssetViewModel.AssetGroupIdList = groupList;

            var AssetCodeList = new List<SelectListItem>();
            AssetCodeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            fixedAssetViewModel.AssetCodeList = AssetCodeList;

            var serialList = new List<SelectListItem>();
            serialList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            fixedAssetViewModel.AssetSerialList = serialList;

            var UseableList = new List<SelectListItem>();
            UseableList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            UseableList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            UseableList.Add(new SelectListItem { Text = "No", Value = "0" });
            fixedAssetViewModel.UsableList = UseableList;

            var suppList = assetClientInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var view_suppList = suppList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.AssetClientCode, p.AssetClientName),
                Value = p.AssetClientInfoID.ToString()
            }).ToList();
            var clientList = new List<SelectListItem>();
            clientList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            clientList.AddRange(view_suppList);
            fixedAssetViewModel.ClientList = clientList;

            //var transactionDate = SessionHelper.TransactionDate;
            //ViewData["TrxDate"] = transactionDate.ToString("dd-MMM-yyyy");

            return fixedAssetViewModel;

        }
        [HttpPost]
        public JsonResult SaveAssetOutInfo(FixedAssetViewModel AssetOutInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;

            //TransactionScope scope = new TransactionScope();
            //{
            try
            {
                var entity = new AssetOut();
                entity.DailyTransactionId = AssetOutInfo.DailyTransactionId;
                entity.OutDate = Convert.ToDateTime(AssetOutInfo.FixedAssetOutDate).Date;
                entity.TranType = Convert.ToInt32(AssetOutInfo.TransactionType);
                entity.AssetGroupID = AssetOutInfo.AssetGroupID;
                entity.AssetID = AssetOutInfo.AssetID;
                entity.AssetSerial = AssetOutInfo.AssetSerial;
                entity.SellingPrice = Convert.ToDecimal(AssetOutInfo.SellingPrice);
                entity.AssetCost = AssetOutInfo.AssetCost;
                entity.AccumulatedDepriciation = AssetOutInfo.AccumulatedDepriciation;
                entity.CurrentDepriciation = AssetOutInfo.CurrentDepri;
                entity.BookValue = AssetOutInfo.BookValue;
                entity.TotalProfitLoss = AssetOutInfo.TotalProfitLoss;
                entity.CapitalGain = AssetOutInfo.CapitalGain;
                entity.BusinessGain = AssetOutInfo.BusinessGain;
                entity.Remarks = AssetOutInfo.Remarks;
                entity.OrgID = Convert.ToInt32(orgID);
                entity.OfficeID = AssetOutInfo.OfficeID;
                entity.IsActive = true;
                entity.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.CreateDate = DateTime.Now;
                entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.UpdateDate = DateTime.Now;
                assetOutService.Create(entity);

                result = 1;
                message = "Asset Sale/Disposed successfully";

                // update dailytransaction table

                var model = dailyTransactionService.GetMany(p => p.DailyTransactionId == AssetOutInfo.DailyTransactionId).First();
                model.AssetStatus = Convert.ToInt32(AssetOutInfo.TransactionType);
                model.StatusDate = Convert.ToDateTime(AssetOutInfo.FixedAssetOutDate);
                dailyTransactionService.Update(model);
                //scope.Complete();                    
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //scope.Dispose();
                throw ex;
            }
            //}           
        }

        [HttpPost]
        public JsonResult GetFixedAssetOutList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var assetOutInfo = assetOutService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId);
            var assetInfo = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID);
            var viewAssetOutInfo = (from ao in assetOutInfo
                                    join ai in assetInfo on ao.AssetID equals ai.AssetID
                                    select new FixedAssetViewModel
                                    {
                                        AssetOutID = ao.AssetOutID,
                                        FixedAssetOutDate = Convert.ToDateTime(ao.OutDate).ToString("dd-MMM-yyyy"),
                                        TransactionType = ao.TranType.ToString(),
                                        AssetGroupID = Convert.ToInt32(ao.AssetGroupID),
                                        AssetID = Convert.ToInt32(ao.AssetID),
                                        AssetName = ai.AssetName,
                                        AssetSerial = ao.AssetSerial,
                                        SellingPrice = ao.SellingPrice,
                                        AssetCost = ao.AssetCost,
                                        AccumulatedDepriciation = ao.AccumulatedDepriciation,
                                        CurrentDepri = Convert.ToDecimal(ao.CurrentDepriciation),
                                        BookValue = ao.BookValue,
                                        TotalProfitLoss = ao.TotalProfitLoss,
                                        CapitalGain = ao.CapitalGain,
                                        BusinessGain = ao.BusinessGain,
                                        Remarks = ao.Remarks,
                                    });
            var currentPageRecords = viewAssetOutInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewAssetOutInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateAssetOutInfo(FixedAssetViewModel AssetOutInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var entity = assetOutService.GetById(Convert.ToInt32(AssetOutInfo.AssetOutID));
            entity.OutDate = Convert.ToDateTime(AssetOutInfo.FixedAssetOutDate).Date;
            entity.TranType = Convert.ToInt32(AssetOutInfo.TransactionType);
            entity.AssetGroupID = AssetOutInfo.AssetGroupID;
            entity.AssetID = AssetOutInfo.AssetID;
            entity.AssetSerial = AssetOutInfo.AssetSerial;
            entity.SellingPrice = Convert.ToDecimal(AssetOutInfo.SellingPrice);
            entity.AssetCost = AssetOutInfo.AssetCost;
            entity.AccumulatedDepriciation = AssetOutInfo.AccumulatedDepriciation;
            entity.CurrentDepriciation = AssetOutInfo.CurrentDepri;
            entity.BookValue = AssetOutInfo.BookValue;
            entity.TotalProfitLoss = AssetOutInfo.TotalProfitLoss;
            entity.CapitalGain = AssetOutInfo.CapitalGain;
            entity.Remarks = AssetOutInfo.Remarks;
            entity.OrgID = Convert.ToInt32(orgID);
            entity.OfficeID = Convert.ToInt32(officeId);
            entity.IsActive = true;
            entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
            entity.UpdateDate = DateTime.Now;
            assetOutService.Update(entity);
            result = 1;
            message = "Updated successfully";
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteFixedAssetOut(long AssetOutID)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = assetOutService.GetByIdLong(AssetOutID);
                entity.IsActive = false;
                entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.UpdateDate = DateTime.Now;
                assetOutService.Update(entity);
                result = 1;
                message = "Deleted successfully";

                var param = new { AssetOutID = AssetOutID };
                var getAssetOutInfo = ultimateReportService.GetDataWithParameter(param, "fix.GetAssetOutIdWiseDailyTranInfo");
                var getDailyTranID = getAssetOutInfo.Tables[0].AsEnumerable().Select(p => new FixedAssetViewModel
                {
                    DailyTransactionId = p.Field<long>("DailyTransactionId"),
                });
                var dailyTransactionID = getDailyTranID.First().DailyTransactionId;
                var dailyTranInfo = dailyTransactionService.GetMany(p => p.DailyTransactionId == dailyTransactionID).First();

                var getTranType = dailyTranInfo.TransactionType;

                dailyTranInfo.AssetStatus = getTranType;
                dailyTranInfo.StatusDate = null;
                dailyTransactionService.Update(dailyTranInfo);

                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AssetPartialOut

        public JsonResult GetGroupWiseAssetInfo(int GroupId)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeID = SessionHelper.LoginUserOfficeID;
            var assetNameList = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.AssetGroupID == GroupId);
            var viewAssetNameList = assetNameList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-({1})", p.AssetName, p.AssetCode),
                Value = p.AssetID.ToString()
            });


            decimal? depRate;
            int depMethod;
            var list = assetNameList.FirstOrDefault();
            if (list != null)
            {
                depRate = list.Deprate;
                depMethod = list.DepriciationMethod;
            }
            else
            {
                depRate = 0;
                depMethod = 0;
            }
            return Json(new { viewAssetNameList, depRate, depMethod }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQuantityWiseAssetSerialForDailyEntry(int officeID, int assetID, int assetQuantity)
        {
            List<FixedAssetViewModel> objFixedAssetViewModel = new List<FixedAssetViewModel>();

            var param = new { OfficeID = officeID, AssetID = assetID, Quantity = assetQuantity };
            var assetSerialRange = ultimateReportService.GetDataWithParameter(param, "fix.GenerateAssetSerial");
            objFixedAssetViewModel = assetSerialRange.Tables[0].AsEnumerable().Select(row => new FixedAssetViewModel()
            {
                AssetSerial = row.Field<string>("AssetSerial"),
                AssetSerialMin = row.Field<string>("AssetSerialMin")
            }).ToList();

            return Json(objFixedAssetViewModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCodeWiseAssetSerialForOpeningEntry(int assetID)
        {
            int nextAssetSerial = 0;
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var param = new { OrgId= orgID, OfficeID = officeId, AssetID = assetID };
            var assetSerialRange = ultimateReportService.GetDataWithParameter(param, "fix.getMaxAssetSerial");
            nextAssetSerial = Convert.ToInt16(assetSerialRange.Tables[0].Rows[0]["assetsserial"].ToString());
            //var assetInfo = fixAssetUpdatesService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId && p.AssetID == assetID).FirstOrDefault();
            //if (assetInfo != null)
            //{
            //    if (assetInfo.AssetSerial != null)
            //    {
            //        nextAssetSerial = Convert.ToInt32(assetInfo.AssetSerial) + 1;
            //    }
            //    else
            //    {
            //        nextAssetSerial = 1;
            //    }
            //}
            //else
            //{
            //    nextAssetSerial = 1;
            //}
            return Json(nextAssetSerial, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQuantityWiseAssetSerialForOpeningEntry(int assetID, int assetQuantity)
        {
            int nextAssetSerial = 0;
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            var assetInfo = fixAssetUpdatesService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId && p.AssetID == assetID).FirstOrDefault();
            if (assetInfo != null)
            {
                if (assetInfo.AssetSerial != null)
                {
                    nextAssetSerial = Convert.ToInt32(assetInfo.AssetSerial) + 1;
                }
                else
                {
                    nextAssetSerial = 1;
                }
            }
            else
            {
                nextAssetSerial = 1;
            }
            return Json(nextAssetSerial, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult SaveAssetPartialOutInfo(FixedAssetViewModel assetPartialOutInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            try
            {
                var entity = new AssetPartialOut();
                entity.AssetGroupID = assetPartialOutInfo.AssetGroupID;
                entity.AssetID = assetPartialOutInfo.AssetID;
                entity.AssetSerial = assetPartialOutInfo.AssetSerial;
                entity.DepriciationRate = assetPartialOutInfo.DepriciationRate;
                entity.CurrAssetCost = assetPartialOutInfo.CurrAssetCost;
                entity.DisposalAmount = assetPartialOutInfo.DisposalAmount;
                entity.CurrCostAfterDisposal = assetPartialOutInfo.CurrCostAfterDisposal;
                entity.EffectiveDate = Convert.ToDateTime(assetPartialOutInfo.EffectiveDate).Date;
                entity.PreviousBookValue = assetPartialOutInfo.PreviousBookValue;
                entity.AccuDeprWholeAsset = assetPartialOutInfo.AccuDeprWholeAsset;
                entity.AccuDeprDisposedAsset = assetPartialOutInfo.AccuDeprDisposedAsset;
                entity.NewBookValue = assetPartialOutInfo.NewBookValue;
                entity.OrgID = Convert.ToInt32(orgID);
                entity.OfficeID = Convert.ToInt32(officeId);
                entity.IsActive = true;
                entity.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.CreateDate = DateTime.Now;
                entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.UpdateDate = DateTime.Now;
                assetPartialOutService.Create(entity);
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
        public JsonResult GetPartialAssetOutList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var assetPartialOutList = assetPartialOutService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == p.OfficeID);
            var assetGroupInfo = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID);
            var assetInfo = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID);
            var viewAssetPartialOutInfo = (from apo in assetPartialOutList
                                           join ag in assetGroupInfo on apo.AssetGroupID equals ag.AssetGroupID
                                           join ai in assetInfo on apo.AssetID equals ai.AssetID
                                           select new FixedAssetViewModel
                                           {
                                               AssetPartialOutID = apo.AssetPartialOutID,
                                               AssetGroupID = apo.AssetGroupID,
                                               GroupName = ag.AssetGroupName,
                                               AssetID = Convert.ToInt32(apo.AssetID),
                                               AssetName = ai.AssetName,
                                               AssetSerial = apo.AssetSerial,
                                               Deprate = apo.DepriciationRate,
                                               CurrAssetCost = Convert.ToDecimal(apo.CurrAssetCost),
                                               DisposalAmount = apo.DisposalAmount,
                                               CurrCostAfterDisposal = apo.CurrCostAfterDisposal,
                                               EffectiveDate = Convert.ToDateTime(apo.EffectiveDate).ToString("dd-MMM-yyyy"),
                                               PreviousBookValue = apo.PreviousBookValue,
                                               AccuDeprWholeAsset = apo.AccuDeprWholeAsset,
                                               AccuDeprDisposedAsset = apo.AccuDeprDisposedAsset,
                                               NewBookValue = apo.NewBookValue
                                           });
            var currentPageRecords = viewAssetPartialOutInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewAssetPartialOutInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult UpdateAssetPartialOutInfo(FixedAssetViewModel assetPartialOutInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            try
            {
                var entity = assetPartialOutService.GetById(assetPartialOutInfo.AssetPartialOutID);
                entity.AssetGroupID = assetPartialOutInfo.AssetGroupID;
                entity.AssetID = assetPartialOutInfo.AssetID;
                entity.AssetSerial = assetPartialOutInfo.AssetSerial;
                entity.DepriciationRate = assetPartialOutInfo.DepriciationRate;
                entity.CurrAssetCost = assetPartialOutInfo.CurrAssetCost;
                entity.DisposalAmount = assetPartialOutInfo.DisposalAmount;
                entity.CurrCostAfterDisposal = assetPartialOutInfo.CurrCostAfterDisposal;
                entity.EffectiveDate = Convert.ToDateTime(assetPartialOutInfo.EffectiveDate).Date;
                entity.PreviousBookValue = assetPartialOutInfo.PreviousBookValue;
                entity.AccuDeprWholeAsset = assetPartialOutInfo.AccuDeprWholeAsset;
                entity.AccuDeprDisposedAsset = assetPartialOutInfo.AccuDeprDisposedAsset;
                entity.NewBookValue = assetPartialOutInfo.NewBookValue;
                entity.OrgID = Convert.ToInt32(orgID);
                entity.OfficeID = Convert.ToInt32(officeId);
                entity.IsActive = true;
                entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.UpdateDate = DateTime.Now;
                assetPartialOutService.Update(entity);
                result = 1;
                message = "Updated successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteAssetPartialOut(int AssetPartialOutID)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var entity = assetPartialOutService.GetById(AssetPartialOutID);
            entity.IsActive = false;
            entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
            entity.UpdateDate = DateTime.Now;
            assetPartialOutService.Update(entity);
            result = 1;
            message = "Deleted successfully";
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ProjectInfo

        [HttpPost]
        public JsonResult SaveProjectInformation(FixedAssetViewModel projectInfo)
        {
            var result = 0;
            var message = "";

            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;
                var officeId = SessionHelper.LoginUserOfficeID;

                var checkDuplicate = projectInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId && p.ProjectName == projectInfo.ProjectName).ToList();
                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Duplicate Project Info found, save denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var entity = new ProjectInfo();
                    entity.ProjectName = projectInfo.ProjectName;
                    entity.FundingAgency = projectInfo.FundingAgency;
                    entity.Description = projectInfo.Description;
                    entity.OfficeID = Convert.ToInt32(officeId);
                    entity.OrgID = Convert.ToInt32(orgID);
                    entity.IsActive = true;
                    entity.CreateDate = DateTime.Now;
                    entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    projectInfoService.Create(entity);
                    result = 1;
                    message = "Saved successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetProjectInfo(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            var projectInfo = projectInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId).ToList();
            var currentPageRecords = projectInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = projectInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult UpdateProjectInformation(FixedAssetViewModel projectInfo)
        {
            var result = 0;
            var message = "";

            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;
                var officeId = SessionHelper.LoginUserOfficeID;

                var checkDuplicate = projectInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId && p.ProjectID != projectInfo.ProjectID && p.ProjectName == projectInfo.ProjectName).ToList();
                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Duplicate Project Info found, update denied";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var entity = projectInfoService.GetById(Convert.ToInt32(projectInfo.ProjectID));
                    entity.ProjectName = projectInfo.ProjectName;
                    entity.FundingAgency = projectInfo.FundingAgency;
                    entity.Description = projectInfo.Description;
                    entity.OfficeID = Convert.ToInt32(officeId);
                    entity.OrgID = Convert.ToInt32(orgID);
                    entity.IsActive = true;
                    entity.UpdateDate = DateTime.Now;
                    entity.UpdateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    projectInfoService.Update(entity);
                    result = 1;
                    message = "Updated successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteProjectInfo(int ProjectID)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = projectInfoService.GetById(ProjectID);
                entity.IsActive = false;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                projectInfoService.Update(entity);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AssetValuer

        [HttpPost]
        public JsonResult SaveAssetValuerInformation(FixedAssetViewModel valuerInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            try
            {
                var entity = new AssetValuer();
                entity.ValuerName = valuerInfo.ValuerName;
                entity.Address = valuerInfo.Address;
                entity.Phone = valuerInfo.Phone;
                entity.Email = valuerInfo.Email;
                entity.ContactPerson = valuerInfo.ContactPerson;
                entity.OrgID = Convert.ToInt32(orgID);
                entity.OfficeID = Convert.ToInt32(officeId);
                entity.IsActive = true;
                entity.CreateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.CreateDate = DateTime.Now;
                entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.UpdateDate = DateTime.Now;
                assetValuerService.Create(entity);
                result = 1;
                message = "Saved Successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetAssetValuerInfo(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            var assetValuerInfo = assetValuerService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId).ToList();
            var currentPageRecords = assetValuerInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = assetValuerInfo.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateAssetValuerInformation(FixedAssetViewModel valuerInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            try
            {
                var entity = assetValuerService.GetById(valuerInfo.ValuerID);
                entity.ValuerName = valuerInfo.ValuerName;
                entity.Address = valuerInfo.Address;
                entity.Phone = valuerInfo.Phone;
                entity.Email = valuerInfo.Email;
                entity.ContactPerson = valuerInfo.ContactPerson;
                entity.OrgID = Convert.ToInt32(orgID);
                entity.OfficeID = Convert.ToInt32(officeId);
                entity.IsActive = true;
                entity.UpdateUser = Convert.ToString(SessionHelper.LoginUserEmployeeID);
                entity.UpdateDate = DateTime.Now;
                assetValuerService.Update(entity);
                result = 1;
                message = "Updated Successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteAssetValuerInfo(int ValuerID)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = assetValuerService.GetById(ValuerID);
                entity.IsActive = false;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                assetValuerService.Update(entity);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region AssetTransfer

        private void MapDropdownForAssetTransfer(FixedAssetViewModel model)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var assetGroup = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).OrderBy(p => p.AssetGroupCode);
            var groupList = new List<SelectListItem>();
            groupList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            model.AssetGroupIdList = groupList;

            var listOfAsset = new List<SelectListItem>();
            listOfAsset.Add(new SelectListItem { Text = "Please Select", Value = "" });
            model.AssetList = listOfAsset;

            var serialList = new List<SelectListItem>();
            serialList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            model.AssetSerialList = serialList;

            var userList = new List<SelectListItem>();
            userList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            model.EmployeeList = userList;
        }
        public JsonResult GetGroupWiseAssetName(int GroupId)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeID = SessionHelper.LoginUserOfficeID;
            var assetNameList = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.AssetGroupID == GroupId);
            var viewAssetNameList = assetNameList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.AssetCode, p.AssetName),
                Value = p.AssetID.ToString()
            });
            return Json(viewAssetNameList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveAssetTransferInformation(FixedAssetViewModel transferInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            int createUser = Convert.ToInt32(LoggedInEmployeeID);
            int updateUser = Convert.ToInt32(LoggedInEmployeeID);
            DateTime createDate = DateTime.Now;
            DateTime updateDate = DateTime.Now;

            try
            {                
                if (transferInfo.allTransactionId.Length > 0)
                {
                    foreach (var item in transferInfo.allTransactionId)
                    {
                        var DailyTransactionId = item;
                        var transferOfficeTo = transferInfo.TransferOfficeID;
                        var param = new { DailyTransactionId = DailyTransactionId, TransferDate = transferInfo.EffectiveDate, TransferOfficeId = transferInfo.TransferOfficeID, AssetUser = transferInfo.AssetUser, Remarks = transferInfo.Remarks, AuthorisedBy = transferInfo.AuthorisedBy, CreateUser = createUser, CreateDate = createDate, UpdateUser = updateUser, UpdateDate = updateDate };
                        var insertData = ultimateReportService.GetDataWithParameter(param, "fix.SaveAssetTransferInformation");
                    }                    
                };
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
        public JsonResult GetAssetTransferInfo(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            var assetTransferInfo = assetTransferService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var assetGroup = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var assetName = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID);
            var officeList = officeService.GetMany(p => p.IsActive == true && p.OrgID == orgID);

            var viewTransferList = (from at in assetTransferInfo
                                    join ag in assetGroup on at.AssetGroupID equals ag.AssetGroupID
                                    join an in assetName on at.AssetID equals an.AssetID
                                    //join o in officeList on at.OfficeFrom equals o.OfficeID
                                    select new FixedAssetViewModel
                                    {
                                        TransferID = Convert.ToInt32(at.TransferID),
                                        GroupId = at.AssetGroupID,
                                        GroupName = ag.AssetGroupName,
                                        AssetID = Convert.ToInt32(at.AssetID),
                                        AssetName = an.AssetName,
                                        OfficeFrom = at.OfficeFrom,
                                        TransferOfficeID = at.OfficeTo,
                                        EffectiveDate = Convert.ToString(at.EffectiveDate),
                                        Remarks = at.Remarks,
                                        AuthorisedBy = at.AuthorisedBy
                                    }).ToList();
            var currentPageRecords = viewTransferList.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewTransferList.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateAssetTransferInformation(FixedAssetViewModel transferInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;

            try
            {
                var entity = assetTransferService.GetById(transferInfo.TransferID);
                entity.AssetGroupID = transferInfo.GroupId;
                entity.AssetID = transferInfo.AssetID;
                entity.OfficeFrom = transferInfo.OfficeFrom;
                entity.OfficeTo = transferInfo.TransferOfficeID;
                entity.EffectiveDate = Convert.ToDateTime(transferInfo.EffectiveDate).Date;
                entity.OrgID = Convert.ToInt32(orgID);
                entity.Remarks = transferInfo.Remarks;
                entity.AuthorisedBy = transferInfo.AuthorisedBy;
                entity.IsActive = true;
                entity.UpdateUser = SessionHelper.LoginUserEmployeeID.ToString();
                entity.UpdateDate = DateTime.Now;
                assetTransferService.Update(entity);
                result = 1;
                message = "Updated successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteAssetTransferInfo(int TransferID)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = assetTransferService.GetById(TransferID);
                entity.IsActive = false;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                assetTransferService.Update(entity);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AssetRevaluation
        private void MapDropdownForAssetRevaluation(FixedAssetViewModel model)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeID = SessionHelper.LoginUserOfficeID;
            var assetGroup = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID);
            var viewAssetGroup = assetGroup.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.AssetGroupCode, p.AssetGroupName),
                Value = p.AssetGroupID.ToString()
            });
            var groupList = new List<SelectListItem>();
            groupList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            groupList.AddRange(viewAssetGroup);
            model.AssetGroupIdList = groupList;

            var listOfAsset = new List<SelectListItem>();
            listOfAsset.Add(new SelectListItem { Text = "Please Select", Value = "" });
            model.AssetList = listOfAsset;

            var valuerList = assetValuerService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeID);
            var viewValuerList = valuerList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.ValuerName,
                Value = p.ValuerID.ToString()
            });
            var vList = new List<SelectListItem>();
            vList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            vList.AddRange(viewValuerList);
            model.ValuerList = vList;
        }

        [HttpPost]
        public JsonResult SaveAssetRevaluationInformation(FixedAssetViewModel revaluationInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeID = SessionHelper.LoginUserOfficeID;
            try
            {
                var entity = new AssetRevaluation();
                entity.AssetGroupID = revaluationInfo.GroupId;
                entity.AssetID = revaluationInfo.AssetID;
                entity.EffectiveDate = Convert.ToDateTime(revaluationInfo.EffectiveDate).Date;
                entity.CurrAssetCost = revaluationInfo.CurrAssetCost;
                entity.Valuer = revaluationInfo.ValuerID;
                entity.RevaluatedValue = revaluationInfo.RevaluatedValue;
                entity.DeprRate = revaluationInfo.DeprRate;
                entity.Remarks = revaluationInfo.Remarks;
                entity.OrgID = Convert.ToInt32(orgID);
                entity.OfficeID = Convert.ToInt32(officeID);
                entity.IsActive = true;
                entity.CreateUser = SessionHelper.LoginUserEmployeeID.ToString();
                entity.CreateDate = DateTime.Now;
                entity.UpdateUser = SessionHelper.LoginUserEmployeeID.ToString();
                entity.UpdateDate = DateTime.Now;
                assetRevaluationService.Create(entity);
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
        public JsonResult GetAssetRevaluationInfo(int jtStartIndex, int jtPageSize, string jtSorting, int groupId)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            var assetRevaluationInfo = assetRevaluationService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId && p.AssetGroupID == groupId);
            var assetGroup = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var assetName = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID);
            var assetValuer = assetValuerService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId);
            var dataList = (from ar in assetRevaluationInfo
                            join ag in assetGroup on ar.AssetGroupID equals ag.AssetGroupID
                            join an in assetName on ar.AssetID equals an.AssetID
                            join av in assetValuer on ar.Valuer equals av.ValuerID
                            select new FixedAssetViewModel
                            {
                                AssetRevaluationID = ar.AssetRevaluationID,
                                AssetGroupID = ar.AssetGroupID,
                                GroupName = ag.AssetGroupName,
                                AssetID = Convert.ToInt32(ar.AssetID),
                                AssetName = an.AssetName,
                                EffectiveDate = ar.EffectiveDate.ToString("dd-MMM-yyyy"),
                                CurrAssetCost = ar.CurrAssetCost,
                                ValuerID = ar.Valuer,
                                ValuerName = av.ValuerName,
                                RevaluatedValue = ar.RevaluatedValue,
                                DeprRate = ar.DeprRate,
                                Remarks = ar.Remarks
                            }).ToList();
            var currentPageRecords = dataList.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = dataList.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateAssetRevaluationInformation(FixedAssetViewModel revaluationInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeID = SessionHelper.LoginUserOfficeID;
            try
            {
                var entity = assetRevaluationService.GetById(revaluationInfo.AssetRevaluationID);
                entity.AssetGroupID = revaluationInfo.GroupId;
                entity.AssetID = revaluationInfo.AssetID;
                entity.EffectiveDate = Convert.ToDateTime(revaluationInfo.EffectiveDate).Date;
                entity.CurrAssetCost = revaluationInfo.CurrAssetCost;
                entity.Valuer = revaluationInfo.ValuerID;
                entity.RevaluatedValue = revaluationInfo.RevaluatedValue;
                entity.DeprRate = revaluationInfo.DeprRate;
                entity.Remarks = revaluationInfo.Remarks;
                entity.OrgID = Convert.ToInt32(orgID);
                entity.OfficeID = Convert.ToInt32(officeID);
                entity.IsActive = true;
                entity.UpdateUser = SessionHelper.LoginUserEmployeeID.ToString();
                entity.UpdateDate = DateTime.Now;
                assetRevaluationService.Update(entity);
                result = 1;
                message = "Updated successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteAssetRevaluationInfo(int AssetRevaluationID)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = assetRevaluationService.GetById(AssetRevaluationID);
                entity.IsActive = false;
                entity.UpdateUser = SessionHelper.LoginUserEmployeeID.ToString();
                entity.UpdateDate = DateTime.Now;
                assetRevaluationService.Update(entity);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AssetOverhauling

        private void MapDropdownForAssetOverhauling(FixedAssetViewModel model)
        {
            var groupList = new List<SelectListItem>();
            groupList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            model.AssetGroupIdList = groupList;

            var assetList = new List<SelectListItem>();
            assetList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            model.AssetCodeList = assetList;

            var assetSerialList = new List<SelectListItem>();
            assetSerialList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            model.AssetSerialList = assetSerialList;
        }

        [HttpPost]
        public JsonResult SaveAssetOverhauling(FixedAssetViewModel AssetOverhaulingInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;

            try
            {
                var entity = new AssetOverhauling();
                entity.AssetGroupID = AssetOverhaulingInfo.AssetGroupID;
                entity.AssetID = AssetOverhaulingInfo.AssetID;
                entity.AssetSerial = AssetOverhaulingInfo.AssetSerial;
                entity.CurrTotalCost = AssetOverhaulingInfo.CurrTotalCost;
                entity.OverhaulingCost = AssetOverhaulingInfo.OverhaulingCost;
                entity.DepriciationRate = AssetOverhaulingInfo.DepriciationRate;
                entity.EffectiveDate = Convert.ToDateTime(AssetOverhaulingInfo.EffectiveDate);
                entity.Remarks = AssetOverhaulingInfo.Remarks;
                entity.OrgID = Convert.ToInt32(orgID);
                entity.OfficeID = AssetOverhaulingInfo.OfficeID;
                entity.DailyTransactionId = AssetOverhaulingInfo.DailyTransactionId;
                entity.IsActive = true;
                entity.CreateUser = SessionHelper.LoginUserEmployeeID.ToString();
                entity.CreateDate = DateTime.Now;
                entity.UpdateUser = SessionHelper.LoginUserEmployeeID.ToString();
                entity.UpdateDate = DateTime.Now;
                assetOverhaulingService.Create(entity);
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
        public JsonResult GetAssetOverhaulingInfo(int jtStartIndex, int jtPageSize, string jtSorting, int OfficeID, int assetGroupID, long assetID)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var assetOverhaulingInfo = assetOverhaulingService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == OfficeID && p.AssetGroupID == assetGroupID && p.AssetID == assetID);
            var assetGroup = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var assetName = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID);

            var viewOverhauling = (from ov in assetOverhaulingInfo
                                   join ag in assetGroup on ov.AssetGroupID equals ag.AssetGroupID
                                   join an in assetName on ov.AssetID equals an.AssetID
                                   select new FixedAssetViewModel
                                   {
                                       AssetOverhaulingID = ov.AssetOverhaulingID,
                                       AssetGroupID = ov.AssetGroupID,
                                       GroupName = ag.AssetGroupName,
                                       AssetID = Convert.ToInt32(ov.AssetID),
                                       AssetName = an.AssetName,
                                       AssetSerial = ov.AssetSerial,
                                       CurrTotalCost = ov.CurrTotalCost,
                                       OverhaulingCost = ov.OverhaulingCost,
                                       DepriciationRate = ov.DepriciationRate,
                                       EffectiveDate = ov.EffectiveDate.ToString("dd-MMM-yyyy"),
                                       Remarks = ov.Remarks
                                   });
            var currentPageRecords = viewOverhauling.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewOverhauling.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateAssetOverhauling(FixedAssetViewModel AssetOverhaulingInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;

            try
            {
                var entity = assetOverhaulingService.GetById(AssetOverhaulingInfo.AssetOverhaulingID);
                entity.AssetGroupID = AssetOverhaulingInfo.AssetGroupID;
                entity.AssetID = AssetOverhaulingInfo.AssetID;
                entity.AssetSerial = AssetOverhaulingInfo.AssetSerial;
                entity.CurrTotalCost = AssetOverhaulingInfo.CurrTotalCost;
                entity.OverhaulingCost = AssetOverhaulingInfo.OverhaulingCost;
                entity.DepriciationRate = AssetOverhaulingInfo.DepriciationRate;
                entity.EffectiveDate = Convert.ToDateTime(AssetOverhaulingInfo.EffectiveDate);
                entity.Remarks = AssetOverhaulingInfo.Remarks;
                entity.OrgID = Convert.ToInt32(orgID);
                entity.OfficeID = AssetOverhaulingInfo.OfficeID;
                entity.IsActive = true;
                entity.UpdateUser = SessionHelper.LoginUserEmployeeID.ToString();
                entity.UpdateDate = DateTime.Now;
                assetOverhaulingService.Update(entity);
                result = 1;
                message = "Updated successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteAssetOverhauling(int AssetOverhaulingID)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = assetOverhaulingService.GetById(AssetOverhaulingID);
                entity.IsActive = false;
                entity.UpdateUser = SessionHelper.LoginUserEmployeeID.ToString();
                entity.UpdateDate = DateTime.Now;
                assetOverhaulingService.Update(entity);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DepriciationRateChange

        private void MapDropdownForDepriciationRateChange(FixedAssetViewModel model)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeID = SessionHelper.LoginUserOfficeID;
            var assetGroup = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID);
            var viewAssetGroup = assetGroup.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.AssetGroupCode, p.AssetGroupName),
                Value = p.AssetGroupID.ToString()
            });
            var groupList = new List<SelectListItem>();
            groupList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            groupList.AddRange(viewAssetGroup);
            model.AssetGroupIdList = groupList;

            var listOfAsset = new List<SelectListItem>();
            listOfAsset.Add(new SelectListItem { Text = "Please Select", Value = "" });
            model.AssetList = listOfAsset;
        }
        public JsonResult LoadAssetIdWiseAssetInfo(long assetID)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            var assetInfo = fixAssetUpdatesService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId && p.AssetID == assetID);
            return Json(assetInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveDepriciationRateChange(FixedAssetViewModel DepRateChangeInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var entity = new DepriciationRateChange();
            entity.AssetGroupID = DepRateChangeInfo.AssetGroupID;
            entity.AssetID = DepRateChangeInfo.AssetID;
            entity.CurrDepRate = DepRateChangeInfo.CurrDepRate;
            entity.NewDepRate = DepRateChangeInfo.NewDepRate;
            entity.EffectiveDate = Convert.ToDateTime(DepRateChangeInfo.EffectiveDate).Date;
            entity.Remarks = DepRateChangeInfo.Remarks;
            entity.OrgID = Convert.ToInt32(orgID);
            entity.OfficeID = Convert.ToInt32(officeId);
            entity.IsActive = true;
            entity.CreateUser = SessionHelper.LoginUserEmployeeID.ToString();
            entity.CreateDate = DateTime.Now;
            entity.UpdateUser = SessionHelper.LoginUserEmployeeID.ToString();
            entity.UpdateDate = DateTime.Now;
            depriciationRateChangeService.Create(entity);
            result = 1;
            message = "Saved successfully";
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAssetDepRateChangeInfo(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;
            var depInfo = depriciationRateChangeService.GetMany(p => p.IsActive == true && p.OrgID == orgID && p.OfficeID == officeId);
            var assetGroup = assetGroupInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID).ToList();
            var assetName = assetInfoService.GetMany(p => p.IsActive == true && p.OrgID == orgID);
            var viewDepInfo = (from dr in depInfo
                               join ag in assetGroup on dr.AssetGroupID equals ag.AssetGroupID
                               join an in assetName on dr.AssetID equals an.AssetID
                               select new FixedAssetViewModel
                               {
                                   DepRateChangeID = dr.DepRateChangeID,
                                   AssetGroupID = dr.AssetGroupID,
                                   GroupName = ag.AssetGroupName,
                                   AssetID = Convert.ToInt32(dr.AssetID),
                                   AssetName = an.AssetName,
                                   CurrDepRate = dr.CurrDepRate,
                                   NewDepRate = dr.NewDepRate,
                                   EffectiveDate = dr.EffectiveDate.ToString("dd-MMM-yyyy"),
                                   Remarks = dr.Remarks
                               });
            var currentPageRecords = viewDepInfo.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewDepInfo.LongCount(), JsonRequestBehavior.AllowGet });

        }
        [HttpPost]
        public JsonResult UpdateDepriciationRateChange(FixedAssetViewModel DepRateChangeInfo)
        {
            var result = 0;
            var message = "";
            var orgID = SessionHelper.LoginUserOrganizationID;
            var officeId = SessionHelper.LoginUserOfficeID;

            var entity = depriciationRateChangeService.GetById(DepRateChangeInfo.DepRateChangeID);
            entity.AssetGroupID = DepRateChangeInfo.AssetGroupID;
            entity.AssetID = DepRateChangeInfo.AssetID;
            entity.CurrDepRate = DepRateChangeInfo.CurrDepRate;
            entity.NewDepRate = DepRateChangeInfo.NewDepRate;
            entity.EffectiveDate = Convert.ToDateTime(DepRateChangeInfo.EffectiveDate).Date;
            entity.Remarks = DepRateChangeInfo.Remarks;
            entity.OrgID = Convert.ToInt32(orgID);
            entity.OfficeID = Convert.ToInt32(officeId);
            entity.IsActive = true;
            entity.UpdateUser = SessionHelper.LoginUserEmployeeID.ToString();
            entity.UpdateDate = DateTime.Now;
            depriciationRateChangeService.Update(entity);
            result = 1;
            message = "updated successfully";
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteDepRateInfo(int DepRateChangeID)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = depriciationRateChangeService.GetById(DepRateChangeID);
                entity.IsActive = false;
                entity.UpdateUser = SessionHelper.LoginUserEmployeeID.ToString();
                entity.UpdateDate = DateTime.Now;
                depriciationRateChangeService.Update(entity);
                result = 1;
                message = "Deleted successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Process
        public ActionResult FixedAssetDayInitial()
        {
            DateTime vdate = TransactionDate;
            var model = new DayInitialViewModel();
            if (IsDayInitiated)

                model.BusinessDate = vdate;
            MapDropDownOfficeList(model);
            return View(model);
        }
        private void MapDropDownOfficeList(DayInitialViewModel model)
        {

            var alloffice = officeService.GetAll().Where(d => d.OfficeID == LoginUserOfficeID && d.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

        }
        public JsonResult DayInitialProcess(int officeId, DateTime businessDate)
        {
            var result = 0;
            var message = "";
            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;
                var checkDuplicate = assetProcessInfoService.GetMany(p => p.OrgID == orgID && p.OfficeID == officeId && p.DeprDate == businessDate);
                if (checkDuplicate.Any())
                {
                    result = 0;
                    message = "Already Day Initial have done";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var entity = new AssetProcessInfo();
                    entity.DeprDate = businessDate;
                    entity.ProcessYN = false;
                    entity.OrgID = Convert.ToInt32(orgID);
                    entity.OfficeID = officeId;
                    entity.CreateUser = Convert.ToInt32(SessionHelper.LoginUserEmployeeID);
                    entity.CreateDate = DateTime.Now;
                    assetProcessInfoService.Create(entity);
                    result = 1;
                    message = "Day Initialized Successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult RunProcessFile()
        {
            var result = 0;
            var message = "";
            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;
                var officeId = SessionHelper.LoginUserOfficeID;
                var monthEndDate = SessionHelper.TransactionDate;
                var createUser = SessionHelper.LoginUserEmployeeID;
                var checkDuplicateProcess = assetProcessInfoService.GetAll().Where(p => p.OrgID == orgID && p.OfficeID == officeId && p.DeprDate == monthEndDate);
                if (checkDuplicateProcess.Any())
                {
                    result = 0;
                    message = "Already Process have done";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var param = new { OrgID = orgID, OfficeID = officeId, MonthEndDate = monthEndDate, CreateUser = createUser };
                    var data = ultimateReportService.GetDataWithParameter(param, "fix.FixMonthClosingNew");
                    result = 1;
                    message = "Data Processed Successfully";
                    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #endregion
        public JsonResult SelectOffice(int officeId)
        {
            var officeFullName = "";
            var OrgName = "";
            if (officeId > 0)
            {
                SessionHelper.LoginUserOfficeID = officeId;
                var office = officeService.GetById(SessionHelper.LoginUserOfficeID.Value);
                var entity = AutoMapper.Mapper.Map<Office, OfficeViewModel>(office);
                SessionHelper.LoggedInOfficeDetail = entity;
                try
                {
                    officeFullName = office.OfficeCode + ", " + office.OfficeName;
                    OrgName = SessionHelper.OrganizationName + "-" + officeFullName;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            var resultObj = new { OfficeName = officeFullName };
            return Json(resultObj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckEntryDateValidation(int office_id)
        {
            var param = new { OfficeID = office_id };
            var data = ultimateReportService.GetDataWithParameter(param, "fix.Get_FixedAsset_YearClosingDate");
            var getYearClosingDate = data.Tables[0].AsEnumerable().Select(p => p.Field<DateTime>("YearClosingDate")).First();
            
            var YearClosingDate = getYearClosingDate.Date.ToString("dd-MMM-yyyy",CultureInfo.InvariantCulture);
            return Json(YearClosingDate, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region GC Reports

        public ActionResult FixedAssetLedgerReport()
        {
            return View();
        }
        public ActionResult FixedAssetScheduleReport()
        {
            return View(this.DropdownForFixedAssetScheduleReport());
        }
        public ActionResult FixAssetDepriciationRegisterReport()
        {
            return View(this.DropdownForFixedAssetDepriciationRegisterReport());
        }
        public ActionResult FixedAssetLedgerReportPrint()
        {
            try
            {
                var officeId = SessionHelper.LoginUserOfficeID;
                var param = new { OfficeID = officeId };
                var data = ultimateReportService.GetDataWithParameter(param, "fix.SP_RPT_FixedAssetLedger");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("FixedAssetLedger.rpt", data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult AssetCodeWiseFixedAssetScheduleReportPrint(int Year)
        {
            try
            {
                var officeId = SessionHelper.LoginUserOfficeID;
                var param = new { Year = Year, OfficeId = officeId };
                var data = ultimateReportService.GetDataWithParameter(param, "fix.SP_RPT_FixedAssetScheduleSummeryYearWise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("Year", Year);
                ReportHelper.PrintReport("Rpt_FixedAssetScheduleSummaryYearWise.rpt", data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult FixAssetDepriciationRegisterReportPrint(int Year, int Month)
        {
            try
            {
                var firstDateOfMonth = new DateTime(Year, Month, 1);
                DateTime firstDateOfNextMonth = new DateTime(Year, Month, 1).AddMonths(1);
                var lastDateOfMonth = firstDateOfNextMonth.AddDays(-1);
                var param = new { firstDateOfMonth = firstDateOfMonth, lastDateOfMonth = lastDateOfMonth };
                var data = ultimateReportService.GetDataWithParameter(param, "fix.SP_RPT_FixAssetDepriciationRegister");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("rpt_FixAssetDepriciationRegister.rpt", data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult PrintDailyTransactionReport()
        {
            try
            {
                var data = ultimateReportService.GetDataWithoutParameter("SP_RPT_GetDailyTransaction");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("FixedAssetDailyTransaction.rpt", data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult TopSheetFixedAssetScheduleReportPrint(int IsForAllBranch, DateTime Date)
        {
            try
            {
                if (IsForAllBranch == 1)
                {
                    var officeIdStartFrom = 00000;
                    var OfficeIdTo = 99999;
                    var param = new { Date = Date, OfficeIdFrom = officeIdStartFrom, OfficeIdTo = OfficeIdTo };
                    var data = ultimateReportService.GetDataWithParameter(param, "fix.SP_RPT_GetAssetTopSheet");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("Date", Date);
                    ReportHelper.PrintReport("Rpt_FixedAssetScheduleSummaryTopSheet.rpt", data.Tables[0], reportParam);
                    return Content(string.Empty);
                }
                else
                {
                    var officeIdFrom = SessionHelper.LoginUserOfficeID;
                    var officeIdTo = SessionHelper.LoginUserOfficeID;
                    var param = new { Date = Date, OfficeIdFrom = officeIdFrom, OfficeIdTo = officeIdTo };
                    var data = ultimateReportService.GetDataWithParameter(param, "fix.SP_RPT_GetAssetTopSheet");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("Date", Date);
                    ReportHelper.PrintReport("Rpt_FixedAssetScheduleSummaryTopSheet.rpt", data.Tables[0], reportParam);
                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FixedAssetViewModel DropdownForFixedAssetScheduleReport()
        {
            var fixedAssetViewModel = new FixedAssetViewModel();
            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            for (int year = (DateTime.Today.Year - 1); year <= DateTime.Today.Year; year++)
            {
                yearList.Add(new SelectListItem { Text = year.ToString(), Value = year.ToString() });
            }
            fixedAssetViewModel.YearList = yearList;
            return fixedAssetViewModel;
        }
        public FixedAssetViewModel DropdownForFixedAssetDepriciationRegisterReport()
        {
            var fixedAssetViewModel = new FixedAssetViewModel();
            var yearList = new List<SelectListItem>();
            yearList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            for (int year = 1976; year < 2099; year++)
            {
                yearList.Add(new SelectListItem { Text = year.ToString(), Value = year.ToString() });
            }
            fixedAssetViewModel.YearList = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            for (int month = 1; month <= 12; month++)
            {
                monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(month), Value = month.ToString() });
            }
            fixedAssetViewModel.MonthList = monthList;
            return fixedAssetViewModel;
        }



        #endregion

        #region BURO Reports

        private void MapDropdownForGroupWiseAssetList()
        {
            var loginUserRoleId =  SessionHelper.LoginUserRoleId;           
            gBankerDbContext context = new gBankerDbContext();

            var reportType = context.ReportType.Where(p => p.IsActive == true).ToList();
            var reportTypeMapping = context.ReportTypeMapping.Where(p => p.IsActive == true && p.RoleId == loginUserRoleId).ToList();

            var selectedReportType = from rt in reportType
                                     join rtm in reportTypeMapping on rt.ReportTypeId equals rtm.ReportTypeId
                                     select new SelectListItem { Text = rt.ReportTypeName, Value = rt.ReportTypeId.ToString() };
            //var reportType = new List<SelectListItem>();
            //reportType.Add(new SelectListItem { Text = "All", Value = "1", Selected = true });
            //reportType.Add(new SelectListItem { Text = "Details (Asset)", Value = "2" });
            //reportType.Add(new SelectListItem { Text = "Details (Office)", Value = "3" });
            //reportType.Add(new SelectListItem { Text = "Summery (Zone)", Value = "4" });            
            //reportType.Add(new SelectListItem { Text = "Summery (Division)", Value = "6" });
            //reportType.Add(new SelectListItem { Text = "Summery (CHRD)", Value = "5" });
            //reportType.Add(new SelectListItem { Text = "Summery (Rest House)", Value = "14" });
            //reportType.Add(new SelectListItem { Text = "Summery (Health Care)", Value = "15" });
            //reportType.Add(new SelectListItem { Text = "Summery (All)", Value = "7" });
            //reportType.Add(new SelectListItem { Text = "Summery (Grand Total)", Value = "8" });
            //reportType.Add(new SelectListItem { Text = "Details (Zone)", Value = "9" });
            //reportType.Add(new SelectListItem { Text = "Details (Division)", Value = "10" });
            //reportType.Add(new SelectListItem { Text = "Details (CHRD)", Value = "11" });
            //reportType.Add(new SelectListItem { Text = "Details (Rest House)", Value = "12" });
            //reportType.Add(new SelectListItem { Text = "Details (Health Care)", Value = "13" });            
            ViewData["ReportType"] = selectedReportType;

            var assetGroup = assetGroupInfoService.GetAll().ToList();
            var ViewAssetGroup = assetGroup.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.AssetGroupCode, p.AssetGroupName),
                Value = p.AssetGroupID.ToString()
            }).ToList();
            var groupList = new List<SelectListItem>();
            groupList.Add(new SelectListItem { Text = "All", Value = "0", Selected = true });
            groupList.AddRange(ViewAssetGroup);
            ViewData["AssetGroupFrom"] = groupList;

            //var groupToList = new List<SelectListItem>();
            //groupToList.Add(new SelectListItem { Text = "All", Value = "99999", Selected = true });
            //groupToList.AddRange(ViewAssetGroup);
            //ViewData["AssetGroupTo"] = groupToList;
        }
        public ActionResult GroupWiseAssetList()
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
            ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            MapDropdownForGroupWiseAssetList();
            return View();
        }
        public ActionResult GenerateGroupWiseAssetListReport(int office_id, string to_date, int qType, int AllOffice, int groupFrom)
        {
            int assetGroupIdFrom;
            int assetGroupIdTo;
            if (groupFrom == 0)
            {
                assetGroupIdFrom = 0;
                assetGroupIdTo = 99999;
            }
            else
            {
                assetGroupIdFrom = groupFrom;
                assetGroupIdTo = groupFrom;
            }
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = office_id.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Date", Value = to_date });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AssetGroupIdFrom", Value = assetGroupIdFrom.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AssetGroupIdTo", Value = assetGroupIdTo.ToString() });

                PrintSSRSReport("/gBanker_Reports/FIX_Group_Wise_Asset_List", paramValues.ToArray(), "gBankerDbContext");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult CompanyWiseAssetStatement()
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
            ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            MapDropdownForGroupWiseAssetList();
            return View();
        }
        public ActionResult GenerateCompanyWiseAssetStatementReport(int office_id, string to_date, int qType, int AllOffice)
        {
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = office_id.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Date", Value = to_date });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });

                PrintSSRSReport("/gBanker_Reports/FIX_Compnay_Wise_Asset_Statement", paramValues.ToArray(), "gBankerDbContext");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult UserWiseAssetList()
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
            ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            MapDropdownForGroupWiseAssetList();
            return View();
        }
        public ActionResult GenerateUserWiseAssetListReport(int office_id, string to_date, int qType, int AllOffice, int groupFrom)
        {
            int assetGroupIdFrom;
            int assetGroupIdTo;
            if (groupFrom == 0)
            {
                assetGroupIdFrom = 0;
                assetGroupIdTo = 99999;
            }
            else
            {
                assetGroupIdFrom = groupFrom;
                assetGroupIdTo = groupFrom;
            }
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = office_id.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Date", Value = to_date });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AssetGroupIdFrom", Value = assetGroupIdFrom.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AssetGroupIdTo", Value = assetGroupIdTo.ToString() });

                PrintSSRSReport("/gBanker_Reports/FIX_User_Wise_Asset_list", paramValues.ToArray(), "gBankerDbContext");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LocationWiseAssetList()
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
            ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            MapDropdownForGroupWiseAssetList();
            return View();
        }
        public ActionResult GenerateLocationWiseAssetListReport(int office_id, string to_date, int qType, int AllOffice, int groupFrom)
        {
            int assetGroupIdFrom;
            int assetGroupIdTo;
            if (groupFrom == 0)
            {
                assetGroupIdFrom = 0;
                assetGroupIdTo = 99999;
            }
            else
            {
                assetGroupIdFrom = groupFrom;
                assetGroupIdTo = groupFrom;
            }
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = office_id.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Date", Value = to_date });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AssetGroupIdFrom", Value = assetGroupIdFrom.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AssetGroupIdTo", Value = assetGroupIdTo.ToString() });

                PrintSSRSReport("/gBanker_Reports/FIX_Location_Wise_Asset_list", paramValues.ToArray(), "gBankerDbContext");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ListOfAssetAdditions()
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
            ViewData["TrxDateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["TrxDateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            MapDropdownForGroupWiseAssetList();
            return View();
        }
        public ActionResult GenerateListOfAssetAdditionstReport(int office_id, string from_date, string to_date, int qType, int AllOffice, int groupFrom)
        {
            int assetGroupIdFrom;
            int assetGroupIdTo;
            if (groupFrom == 0)
            {
                assetGroupIdFrom = 0;
                assetGroupIdTo = 99999;
            }
            else
            {
                assetGroupIdFrom = groupFrom;
                assetGroupIdTo = groupFrom;
            }
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = office_id.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Date", Value = to_date });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AssetGroupIdFrom", Value = assetGroupIdFrom.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AssetGroupIdTo", Value = assetGroupIdTo.ToString() });

                PrintSSRSReport("/gBanker_Reports/FIX_List_Of_Asset_Additions", paramValues.ToArray(), "gBankerDbContext");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ListOfAssetSales()
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
            ViewData["TrxDateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["TrxDateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            MapDropdownForGroupWiseAssetList();
            return View();
        }
        public ActionResult GenerateListOfAssetSalesReport(int office_id, string from_date, string to_date, int qType, int AllOffice, int groupFrom)
        {
            int assetGroupIdFrom;
            int assetGroupIdTo;
            if (groupFrom == 0)
            {
                assetGroupIdFrom = 0;
                assetGroupIdTo = 99999;
            }
            else
            {
                assetGroupIdFrom = groupFrom;
                assetGroupIdTo = groupFrom;
            }
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = office_id.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Date", Value = to_date });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AssetGroupIdFrom", Value = assetGroupIdFrom.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AssetGroupIdTo", Value = assetGroupIdTo.ToString() });

                PrintSSRSReport("/gBanker_Reports/FIX_List_Of_Asset_Sales", paramValues.ToArray(), "gBankerDbContext");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult FixedAssetsDepreciationSchedule()
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
            ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            MapDropdownForGroupWiseAssetList();
            return View();
        }
        public ActionResult GenerateAssetDepriciationScheduletReport(int office_id, string to_date, int qType, int AllOffice)
        {
            try
            {
                var paramValues = new List<Service.ReportExecutionService.ParameterValue>();
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "OfficeId", Value = office_id.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Date", Value = to_date });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new Service.ReportExecutionService.ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });

                PrintSSRSReport("/gBanker_Reports/FIX_Fixed_Assets_And_Depreciation_Schedule", paramValues.ToArray(), "gBankerDbContext");
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
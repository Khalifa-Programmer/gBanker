using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class BudgetController : BaseController
    {
        #region Variables
        private readonly IAccChartService accChartService;
        private readonly IBudgetService budgetService;
        private readonly IOfficeService officeService;
        private readonly IAccReportService accReportService;
        private readonly IParticularService particularService;
        private readonly ITargetAchievementService targetAchievementService;
        private readonly ITargetAchievementBuroService targetAchievementburoService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IApplicationSettingsService applicationSettingsService;
        private readonly IProductService productService;
        private readonly IEmployeeService employeeService;
        public BudgetController(IAccChartService accChartService, IBudgetService budgetService, IOfficeService officeService, IAccReportService accReportService, IParticularService particularService, ITargetAchievementService targetAchievementService, 
            IUltimateReportService ultimateReportService, IApplicationSettingsService applicationSettingsService, IProductService productService, IEmployeeService employeeService, ITargetAchievementBuroService targetAchievementburoService)
        {
            this.accChartService = accChartService;
            this.budgetService = budgetService;
            this.officeService = officeService;
            this.accReportService = accReportService;
            this.particularService = particularService;
            this.targetAchievementService = targetAchievementService;
            this.ultimateReportService = ultimateReportService;
            this.applicationSettingsService = applicationSettingsService;
            this.productService = productService;
            this.employeeService = employeeService;
            this.targetAchievementburoService = targetAchievementburoService;
        }
        #endregion

        #region Methods
        public JsonResult GetBudgetList_VERC(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long TotCount;

                if (filterColumn == null)
                    filterColumn = "";

                if (filterValue == null)
                    filterValue = "";

                var param1 = new { @OfficeID = LoginUserOfficeID, @OrgID = LoggedInOrganizationID, @filterColumn = filterColumn, filterValue = filterValue };
                var AccountCodeListbudgetAndParticularwise = ultimateReportService.GetAccountCodeListbudgetAndParticularwise(param1);

                List<BudgetViewModel> List_ViewModel = new List<BudgetViewModel>();
                List_ViewModel = AccountCodeListbudgetAndParticularwise.Tables[0].AsEnumerable()
                .Select(row => new BudgetViewModel
                {
                    SlNo = row.Field<int>("SlNo"),
                    BudgetID = row.Field<int>("BudgetID"),
                    OrgID = row.Field<int>("OrgID"),
                    OfficeID = row.Field<int>("OfficeID"),
                    TrxDate = row.Field<DateTime?>("TrxDate"),
                    BudgetYear = row.Field<int?>("BudgetYear"),
                    AccID = row.Field<int>("AccID"),
                    BudgetAmount = row.Field<decimal>("BudgetAmount"),
                    BudgetType = row.Field<int?>("BudgetType"),
                    IsActive = row.Field<bool>("IsActive"),
                    AccName = row.Field<string>("AccName"),
                    IsFinancial = row.Field<bool>("IsFinancial")
                }).ToList();
                var detail = List_ViewModel.ToList();
                TotCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult BudgetWithParticularReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(LoginUserOfficeID));
            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }




        public ActionResult GenerateBudgetWithParticularReport(string office_id, string from_date, string to_date)
        {
            try
            {

                var param = new { from_date = from_date, to_date = to_date, office_id = office_id };
                var allVouchers = accReportService.GetBudgetWithParticularReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", to_date);
                ReportHelper.PrintReport("rpt_acc_budgetWithParticular.rpt", allVouchers.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }




        public JsonResult GetAccCodeForProgram(string acc_code, int OfficeLevel, string TransactionType, string IsReconcile)
        {
            IEnumerable<AccChart> chart;

            if (IsReconcile == null)
            {
                IsReconcile = "false";
            }
            if (TransactionType == null)
            {
                TransactionType = "Jr";
            }
            List<AccChartViewModel> List_ProductViewModel = new List<AccChartViewModel>();
            var param = new { OfficeID = LoginUserOfficeID, OfficeLevel = OfficeLevel, TransactionType = TransactionType, IsReconcile = IsReconcile, OrgID = LoggedInOrganizationID };
            var div_items = ultimateReportService.GetAccCodeListAccordingToOfficeBudgetForProgram(param);
            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new AccChartViewModel
            {
                AccID = row.Field<int>("AccID"),
                AccCode = row.Field<string>("AccCode"),
                AccName = row.Field<string>("AccName")
            }).ToList();
            var acc = List_ProductViewModel.Where(m => string.Format("{0} - {1}", m.AccCode, m.AccName).ToLower().Contains(acc_code.ToLower())).Select(m1 => new { m1.AccID, AccFullName = string.Format("{0} - {1}", m1.AccCode, m1.AccName) }).ToList();
            return Json(acc, JsonRequestBehavior.AllowGet);
        }






        public JsonResult GetBudgetList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                long TotCount;

                var param1 = new { @OfficeID = LoginUserOfficeID, @OrgID = LoggedInOrganizationID };
                var AccountCodeListbudgetAndParticularwise = ultimateReportService.GetAccountCodeListbudgetAndParticularwise(param1);

                List<BudgetViewModel> List_ViewModel = new List<BudgetViewModel>();
                List_ViewModel = AccountCodeListbudgetAndParticularwise.Tables[0].AsEnumerable()
                .Select(row => new BudgetViewModel
                {
                    SlNo = row.Field<int>("SlNo"),
                    BudgetID = row.Field<int>("BudgetID"),
                    OrgID = row.Field<int>("OrgID"),
                    OfficeID = row.Field<int>("OfficeID"),
                    TrxDate = row.Field<DateTime?>("TrxDate"),
                    BudgetYear = row.Field<int?>("BudgetYear"),
                    AccID = row.Field<int>("AccID"),
                    BudgetAmount = row.Field<decimal>("BudgetAmount"),
                    BudgetType = row.Field<int?>("BudgetType"),
                    IsActive = row.Field<bool>("IsActive"),
                    AccName = row.Field<string>("AccName"),
                    IsFinancial = row.Field<bool>("IsFinancial")
                }).ToList();
                var detail = List_ViewModel.ToList();
                TotCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public JsonResult GetBudgetList(int jtStartIndex, int jtPageSize, string jtSorting)
        //{
        //    try
        //    {
        //        var budgetDetail = budgetService.GetAll().Where(m => m.IsActive == true && m.OrgID == LoggedInOrganizationID && m.OfficeID == LoginUserOfficeID);
        //        var viewDetail = Mapper.Map<IEnumerable<Budget>, IEnumerable<BudgetViewModel>>(budgetDetail);
        //        List<BudgetViewModel> detail = new List<BudgetViewModel>();
        //        int row_indx = 1;
        //        foreach (var vd in viewDetail)
        //        {
        //            var accName = accChartService.GetById(vd.AccID);
        //            var details = new BudgetViewModel() { SlNo = row_indx, BudgetID = vd.BudgetID, TrxDate = vd.TrxDate, BudgetYear = vd.BudgetYear, AccName = accName.AccCode + ", " + accName.AccName, BudgetAmount = vd.BudgetAmount };
        //            detail.Add(details);
        //            row_indx++;
        //        }
        //        var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);

        //        return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = row_indx - 1 });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult DeleteBudget(string BudgetId)
        {
            var bud = budgetService.GetById(Convert.ToInt32(BudgetId));
            bud.IsActive = false;
            bud.InActiveDate = DateTime.Now;
            budgetService.Update(bud);
            return RedirectToAction("Index");
        }
        public ActionResult GenerateBudgetReport(string office_id, string from_date, string to_date)
        {
            try
            {

                var param = new { from_date = from_date, to_date = to_date, office_id = office_id };
                var allVouchers = accReportService.GetBudgetReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", to_date);
                ReportHelper.PrintReport("rpt_acc_budget.rpt", allVouchers.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult SaveParticular(string particular)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = new Particular();
                entity.ParticularName = particular;
                entity.IsActive = true;
                entity.CreateUser = LoggedInEmployeeID;
                entity.CreateDate = DateTime.UtcNow;
                particularService.Create(entity);
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
        public JsonResult GetParticularList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var particularList = particularService.GetAll().Where(p => p.IsActive == true).ToList();
            var currentPageRecords = particularList.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = particularList.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public JsonResult UpdateParticular(ParticularViewModel objParticular)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = particularService.GetById(objParticular.ParticularId);
                entity.ParticularName = objParticular.ParticularName;
                entity.IsActive = true;
                entity.CreateUser = LoggedInEmployeeID;
                entity.CreateDate = DateTime.UtcNow;
                particularService.Update(entity);
                result = 1;
                message = "Updated Successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteParticular(int ParticularId)
        {
            var result = 0;
            var message = "";

            var entity = particularService.GetById(ParticularId);
            entity.IsActive = false;
            entity.CreateUser = LoggedInEmployeeID;
            entity.CreateDate = DateTime.UtcNow;
            particularService.Update(entity);
            result = 1;
            message = "Deleted Successfully";
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        private void MapDropDownForParticular(TargetAchievementViewModel model)
        {
            var particularList = particularService.GetAll().Where(p => p.IsActive == true).ToList();
            var viewParticularList = particularList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.ParticularName,
                Value = p.ParticularId.ToString()
            }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Please Select", Value = "" });
            list.AddRange(viewParticularList);
            model.ParticularList = list;
            
        }
        private void MapDropDownBuroForParticular(TargetAchievementBuroViewModel model)
        {
            var particularList = particularService.GetAll().Where(p => p.IsActive == true).ToList();
            var viewParticularList = particularList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.ParticularName,
                Value = p.ParticularId.ToString()
            }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Please Select", Value = "" });
            list.AddRange(viewParticularList);
            model.ParticularList = list;

            var ViewemployeeList = employeeService.GetAll().Where(p => p.IsActive == true && p.OfficeID==LoginUserOfficeID && p.EmployeeStatus==1).ToList();
            var employeeList = ViewemployeeList.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = string.Format("{0} - {1}", p.EmployeeCode.ToString(), p.EmpName.ToString()),
                Value = p.EmployeeID.ToString()
            }).ToList();
            var Elist = new List<SelectListItem>();
            Elist.Add(new SelectListItem { Text = "Please Select", Value = "" });
            Elist.AddRange(employeeList);
            model.EmployeeList = Elist;

        }
        public JsonResult GetProductList(string prodCode)
        {
            var productType = productService.GetMany(p => p.IsActive == true && (p.ProductType == 1 || p.ProductType == 0) && p.ProductCode.StartsWith(prodCode)).ToList();
            var viewProductType = productType.AsEnumerable().Select(p => new SelectListItem()
            {
                Text = string.Format("{0}-{1}", p.ProductCode, p.ProductName),
                Value = p.ProductID.ToString()
            }).ToList();
            var productTypeList = new List<SelectListItem>();
            //productTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            productTypeList.AddRange(viewProductType);
            return Json(productTypeList, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Events
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult Create()
        {
            var model = new BudgetViewModel();
            model.TrxDate = TransactionDate;
            model.OfficeID = Convert.ToInt32(LoginUserOfficeID);
            model.OfficeLevel = officeService.GetById(model.OfficeID).OfficeLevel;
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(BudgetViewModel model)
        {
            try
            {


                var entity = Mapper.Map<BudgetViewModel, Budget>(model);

                if (entity.AccID == 0)
                {

                    return GetErrorMessageResult("Please put the account Code");
                }
                entity.IsActive = true;
                entity.OrgID = Convert.ToInt32(LoggedInOrganizationID);
                entity.OfficeID = Convert.ToInt32(LoginUserOfficeID);
                budgetService.Create(entity);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        public JsonResult GetAccCode(string acc_code, int OfficeLevel, string TransactionType, string IsReconcile)
        {
            IEnumerable<AccChart> chart;

            if (IsReconcile == null)
            {
                IsReconcile = "false";
            }
            if (TransactionType == null)
            {
                TransactionType = "Jr";
            }
            List<AccChartViewModel> List_ProductViewModel = new List<AccChartViewModel>();
            var param = new { OfficeID = LoginUserOfficeID, OfficeLevel = OfficeLevel, TransactionType = TransactionType, IsReconcile = IsReconcile, OrgID = LoggedInOrganizationID };
            var div_items = ultimateReportService.GetAccCodeListAccordingToOfficeBudget(param);
            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new AccChartViewModel
            {
                AccID = row.Field<int>("AccID"),
                AccCode = row.Field<string>("AccCode"),
                AccName = row.Field<string>("AccName")
            }).ToList();
            var acc = List_ProductViewModel.Where(m => string.Format("{0} - {1}", m.AccCode, m.AccName).ToLower().Contains(acc_code.ToLower())).Select(m1 => new { m1.AccID, AccFullName = string.Format("{0} - {1}", m1.AccCode, m1.AccName) }).ToList();
            return Json(acc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {
            var budget = budgetService.GetById(id);
            var budgetModel = Mapper.Map<Budget, BudgetViewModel>(budget);
            var acc = accChartService.GetById(budget.AccID);
            budgetModel.AccName = string.Format("{0} - {1}", acc.AccCode, acc.AccName);
            budgetModel.OfficeLevel = officeService.GetById(budget.OfficeID).OfficeLevel;
            return View(budgetModel);
        }
        [HttpPost]
        public ActionResult Edit(BudgetViewModel model)
        {
            try
            {
                var entity = budgetService.GetById(Convert.ToInt32(model.BudgetID));
                if (ModelState.IsValid)
                {
                    entity.TrxDate = model.TrxDate;
                    entity.BudgetYear = model.BudgetYear;
                    entity.AccID = model.AccID;
                    entity.BudgetAmount = model.BudgetAmount;
                    budgetService.Update(entity);
                    return GetSuccessMessageResult();
                }
                else
                    return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        public ActionResult BudgetReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult Delete(int id)
        {
            return View();
        }
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
        public ActionResult CreateParticular()
        {
            return View();
        }
        public ActionResult TargetAchievementBuro()
        {
            var model = new TargetAchievementBuroViewModel();
            MapDropDownBuroForParticular(model);
            model.Date = DateTime.Parse("01 Jul 2019").Date.ToString("dd-MMM-yyyy");
            return View(model);
        }
        public ActionResult TargetAchievement()
        {
            var model = new TargetAchievementViewModel();
            MapDropDownForParticular(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult SaveTargetAchievement(TargetAchievementViewModel objTarget)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = new TargetAchievement();
                entity.ParticularId = objTarget.ParticularId;
                entity.Balance = 0;
                entity.TargetCurrentYear = objTarget.TargetCurrentYear;
                entity.Target = objTarget.Target;
                entity.Achievement = 0;
                entity.Date = Convert.ToDateTime(objTarget.Date).Date;
                entity.ProductID = objTarget.ProductID;
                entity.IsActive = true;
                entity.CreateUser = LoggedInEmployeeID;
                entity.CreateDate = DateTime.UtcNow;
                entity.OfficeID = Convert.ToInt16(LoginUserOfficeID);
                targetAchievementService.Create(entity);
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
        public ActionResult SaveTargetAchievementBuro(TargetAchievementBuroViewModel objTarget)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = new TargetAchievementBuro();
                entity.Date = Convert.ToDateTime("01 Jul 2019").Date;
                if (objTarget.ParticularId == null || objTarget.EmployeeID == null)
                {
                    result = 0;
                    message = "Fail to save";
                }
                else
                {


                    entity.ParticularId = objTarget.ParticularId;
                    entity.Balance = 0;
                    if (objTarget.TargetCurrentYear == null)
                    {
                        entity.TargetCurrentYear = DateTime.Parse(objTarget.Date).Year;
                    }
                    else
                        entity.TargetCurrentYear = objTarget.TargetCurrentYear;
                    if (objTarget.Target == null)
                    {
                        entity.Target = 0;
                    }
                    else
                        entity.Target = objTarget.Target;
                    entity.Achievement = 0;
                   
                    entity.ProductID = objTarget.ProductID;
                    entity.IsActive = true;
                    entity.CreateUser = LoggedInEmployeeID;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.OfficeID = Convert.ToInt16(LoginUserOfficeID);
                    entity.EmployeeID = objTarget.EmployeeID;
                    targetAchievementburoService.Create(entity);
                    result = 1;
                    message = "Saved Successfully";
                }
              
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult GetTargetListBuro(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            List<TargetAchievementBuroViewModel> List_AccTrxMasterViewModel = new List<TargetAchievementBuroViewModel>();
            var param = new { OfficeID = LoginUserOfficeID };
            var empList = accReportService.GetAccDataForReport(param, "getTargetBuroDetails");

            List_AccTrxMasterViewModel = empList.Tables[0].AsEnumerable()
            .Select(row => new TargetAchievementBuroViewModel
            {
                TargetId = row.Field<int>("TargetId"),
                ParticularId = row.Field<int>("ParticularId"),
                ParticularName = row.Field<string>("ParticularName"),
                TargetCurrentYear = row.Field<decimal>("TargetCurrentYear"),
                Target = row.Field<decimal>("Target"),
                ProductNameWithCode = row.Field<string>("ProductNameWithCode"),
                DateMSG = row.Field<string>("DateMSG"),
                EmployeeID = row.Field<int>("EmployeeID"),
                ProductID = row.Field<int>("ProductID"),
                Balance = row.Field<decimal>("Balance"),
                Achievement = row.Field<decimal>("Achievement"),
                EmployeeName = row.Field<string>("EmpName"),
                EmployeeNameBn = row.Field<string>("EmpNameBen")
            }).ToList();
            var currentPageRecords = List_AccTrxMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_AccTrxMasterViewModel.LongCount(), JsonRequestBehavior.AllowGet });

       }
        [HttpPost]
        public JsonResult GetTargetList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            var particular = particularService.GetAll().Where(p => p.IsActive == true).ToList();
            var target = targetAchievementService.GetAll().Where(p => p.IsActive == true && p.OfficeID == LoginUserOfficeID).ToList();
            var product = productService.GetMany(p => p.IsActive == true).ToList();
            var viewTarget = from p in particular
                             join t in target on p.ParticularId equals t.ParticularId
                             join pr in product on t.ProductID equals pr.ProductID
                             select new TargetAchievementViewModel()
                             {
                                 TargetId = t.TargetId,
                                 ParticularId = t.ParticularId,
                                 ParticularName = p.ParticularName,
                                 Balance = t.Balance,
                                 TargetCurrentYear = t.TargetCurrentYear,
                                 Target = t.Target,
                                 Achievement = t.Achievement,
                                 Date = Convert.ToDateTime(t.Date).ToString("dd-MMM-yyyy"),
                                 ProductID = Convert.ToInt32(t.ProductID),
                                 ProductNameWithCode=pr.ProductCode+"-"+pr.ProductName                                 
                             };
            var currentPageRecords = viewTarget.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewTarget.LongCount(), JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public ActionResult UpdateTargetAchievement(TargetAchievementViewModel objTarget)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = targetAchievementService.GetById(objTarget.TargetId);
                entity.ParticularId = objTarget.ParticularId;
                entity.Balance = 0;
                entity.TargetCurrentYear = objTarget.TargetCurrentYear;
                entity.Target = objTarget.Target;
                entity.Achievement = 0;
                entity.Date = Convert.ToDateTime(objTarget.Date).Date;
                entity.ProductID = objTarget.ProductID;
                entity.IsActive = true;
                entity.CreateUser = LoggedInEmployeeID;
                entity.CreateDate = DateTime.UtcNow;
                entity.OfficeID = Convert.ToInt16(LoginUserOfficeID);
                targetAchievementService.Update(entity);
                result = 1;
                message = "Updated Successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult UpdateTargetAchievementBuro(TargetAchievementBuroViewModel objTarget)
        {
            var result = 0;
            var message = "";

            try
            {
                var entity = targetAchievementburoService.GetById(objTarget.TargetId);
                entity.ParticularId = objTarget.ParticularId;
                entity.Balance = 0;
               // entity.TargetCurrentYear = objTarget.TargetCurrentYear;
                entity.Target = objTarget.Target;
                entity.Achievement = 0;
                entity.Date = Convert.ToDateTime(objTarget.Date).Date;
                entity.ProductID = objTarget.ProductID;
                entity.IsActive = true;
                entity.CreateUser = LoggedInEmployeeID;
                entity.CreateDate = DateTime.UtcNow;
                //entity.OfficeID = Convert.ToInt16(objTarget.OfficeID);
                entity.EmployeeID = Convert.ToInt16(objTarget.EmployeeID);
                targetAchievementburoService.Update(entity);
                result = 1;
                message = "Updated Successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteTargetAchievement(int TargetId)
        {
            var result = 0;
            var message = "";
            try
            {
                var entity = targetAchievementService.GetById(TargetId);
                entity.IsActive = false;
                entity.CreateUser = LoggedInEmployeeID;
                entity.CreateDate = DateTime.UtcNow;
                targetAchievementService.Update(entity);
                result = 1;
                message = "Deleted Successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteTargetAchievementBuro(int TargetId)
        {
            var result = 0;
            var message = "";
            try
            {
                var param = new { TargetID = TargetId };
                var empList = accReportService.DelTargetBuro(param, "DelTargetBuro");

                //var entity = targetAchievementService.GetById(TargetId);
                //entity.IsActive = false;
                //entity.CreateUser = LoggedInEmployeeID;
                //entity.CreateDate = DateTime.UtcNow;
                //targetAchievementService.Update(entity);
                result = 1;
                message = "Deleted Successfully";
                return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void MapDropdownForYearMonth()
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var yearList = new List<SelectListItem>();
            yearList.Add(PleaseSelect);
            for (int i = DateTime.Now.Year + 1; i >= (DateTime.Now.Year) - 1; i--)
            {
                yearList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            ViewData["YearList"] = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
            }
            ViewData["MonthList"] = monthList;
        }
        public ActionResult TargetAndAchievementReport()
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
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForYearMonth();
            return View();
        }
        public ActionResult GenerateTargetAndAchievementReport(string office_id, string year, string month)
        {
            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;
                var officeFrom = office_id;
                var officeTo = office_id;

                var yearOpeningDate = applicationSettingsService.GetAll().Where(p => p.IsActive == true && p.OrganizationID == orgID && p.YearClosingDate.Value.Year == Convert.ToInt32(year)).FirstOrDefault();
                var firstDateOfYear = new DateTime(Convert.ToInt32(yearOpeningDate.YearClosingDate.Value.Year), Convert.ToInt32(yearOpeningDate.YearClosingDate.Value.Month), 1).AddMonths(1).ToString();
                var lastDateOfYear = new DateTime(Convert.ToInt32(yearOpeningDate.YearClosingDate.Value.Year + 1), Convert.ToInt32(yearOpeningDate.YearClosingDate.Value.Month), 1).AddMonths(1).AddDays(-1).ToString();

                var firstDayOfMonth = "";
                var lastDayOfMonth = "";
                if (!string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(year))
                {
                    firstDayOfMonth = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1).ToString();
                    lastDayOfMonth = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1).AddMonths(1).AddDays(-1).ToString();
                }
                else if (!string.IsNullOrEmpty(year))
                {
                    string currentMonth = DateTime.Now.ToString("MM");
                    firstDayOfMonth = new DateTime(Convert.ToInt32(year), Convert.ToInt32(currentMonth), 1).ToString();
                    lastDayOfMonth = new DateTime(Convert.ToInt32(year), Convert.ToInt32(currentMonth), 1).AddMonths(1).AddDays(-1).ToString();
                }
                var param = new { org = orgID, OfficeFrom = officeFrom, OfficeTo = officeTo, StartDate = firstDateOfYear, EndDate = lastDateOfYear, StartDateOfMonth = firstDayOfMonth, EndDateOfMonth = lastDayOfMonth };
                var data = ultimateReportService.GetDataWithParameter(param, "Rpt_POMISTargetAndAchievementWithParticular");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptPOMISTargetAndAchievementNew.rpt", data.Tables[0], reportParam);
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

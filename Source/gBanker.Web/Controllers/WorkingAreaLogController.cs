using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gBanker.Data.CodeFirstMigration;
using AutoMapper;
namespace gBanker.Web.Controllers
{
    public class WorkingAreaLogController : BaseController
    {
        private readonly IOfficeService officeService;
        private readonly IWorkingAreaLogService WorkingAreaLogService;
        private readonly IUltimateReportService ultimateReportService;
        public WorkingAreaLogController(IOfficeService officeService, IWorkingAreaLogService WorkingAreaLogService, IUltimateReportService ultimateReportService)
        {
            this.WorkingAreaLogService = WorkingAreaLogService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            
        }
        //
        // GET: /WorkingAreaLog/
        public ActionResult Index()
        {
            return View();
        }
        private void MapDropDownList(WorkingAreaLogViewModel model)
        {
            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OrgID == LoggedInOrganizationID && m.OfficeID == offc_id).ToList();
            var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.OfficeList = ofc_items;

            List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
            var param = new { @OrgID = SessionHelper.LoginUserOfficeID };
            var vmodel = ultimateReportService.GetMainProductCode(param);

            List_ProductViewModel = vmodel.Tables[0].AsEnumerable()
            .Select(row => new ProductViewModel
            {

                ProductCode = row.Field<string>("MainProductCode"),
                ProductName = row.Field<string>("MainItemName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductCode.ToString(),
                Text = x.ProductCode.ToString() + " " + x.ProductName.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);

            model.productListItems = d_items;
        }
        public JsonResult GetProductList()
        {
            List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
            var param = new { @OrgID = SessionHelper.LoginUserOfficeID };
            var vmodel = ultimateReportService.GetMainProductCode(param);

            List_ProductViewModel = vmodel.Tables[0].AsEnumerable()
            .Select(row => new ProductViewModel
            {

                ProductCode = row.Field<string>("MainProductCode"),
                ProductName = row.Field<string>("MainItemName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductCode.ToString(),
                Text = x.ProductCode.ToString() + " " + x.ProductName.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(new { Result = "OK", Options = viewProduct });
           // model.productListItems = d_items;
           // return Json(d_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWorkingLogInfo(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                List<WorkingAreaLogViewModel> List_ProductViewModel = new List<WorkingAreaLogViewModel>();
                var param = new {  @OfficeID = SessionHelper.LoginUserOfficeID};
                var model = ultimateReportService.GetWorkingLogInfo(param);

                List_ProductViewModel = model.Tables[0].AsEnumerable()
                .Select(row => new WorkingAreaLogViewModel
                {
                    WorkingAreaLogID = row.Field<long>("WorkingAreaLogID"),
                    Program = row.Field<string>("MainItemName"),
                    WorkingArea = row.Field<string>("WorkingArea"),
                    Upzilla= row.Field<decimal>("Upzilla"),
                    Municipality = row.Field<decimal>("Municipality"),
                    SelfEnterprenuerMale = row.Field<decimal>("SelfEnterprenuerMale"),
                    SelfEnterprenuerFeMale = row.Field<decimal>("SelfEnterprenuerFeMale"),
                    PaidEnterPrenuerOwnFamilyMale = row.Field<decimal>("PaidEnterPrenuerOwnFamilyMale"),
                    PaidEnterPrenuerOwnFamilyFeMale = row.Field<decimal>("PaidEnterPrenuerOwnFamilyFeMale"),
                    PaidEnterPrenuerOutSideMale = row.Field<decimal>("PaidEnterPrenuerOutSideMale"),
                    PaidEnterPrenuerOutSideFeMale = row.Field<decimal>("PaidEnterPrenuerOutSideFeMale")

                }).ToList();

                var detail = List_ProductViewModel.ToList();
                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        //
        // GET: /WorkingAreaLog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /WorkingAreaLog/Create
        public ActionResult Create()
        {
            var model = new WorkingAreaLogViewModel();
            MapDropDownList(model);
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            return View(model);
        }

        //
        // POST: /WorkingAreaLog/Create
        [HttpPost]
        public ActionResult Create(WorkingAreaLogViewModel model)
        {
            try
            {
                var entity = Mapper.Map<WorkingAreaLogViewModel, WorkingAreaLog>(model);

                // TODO: Add insert logic here
               
                //Add Validlation Logic.

                if (ModelState.IsValid)
                {


                    var errors = WorkingAreaLogService.IsValidSaving(entity);
                    if (errors.ToList().Count == 0)
                    {
                        WorkingAreaLogService.Create(entity);
                        return GetSuccessMessageResult();

                    }

                    else
                        return GetErrorMessageResult(errors);
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
             }
        }

        //
        // GET: /WorkingAreaLog/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /WorkingAreaLog/Edit/5
        [HttpPost]
        public ActionResult Edit(WorkingAreaLogViewModel model)
        {
            try
            {
                var entity = WorkingAreaLogService.GetByIdLong(model.WorkingAreaLogID);
                if (ModelState.IsValid)
                {
                    entity.Municipality = model.Municipality;
                    entity.PaidEnterPrenuerOutSideFeMale = model.PaidEnterPrenuerOutSideFeMale;
                    entity.PaidEnterPrenuerOutSideMale = model.PaidEnterPrenuerOutSideMale;
                    entity.PaidEnterPrenuerOwnFamilyFeMale = model.PaidEnterPrenuerOwnFamilyFeMale;
                    entity.PaidEnterPrenuerOwnFamilyMale = model.PaidEnterPrenuerOwnFamilyMale;
                    entity.SelfEnterprenuerFeMale = model.SelfEnterprenuerFeMale;
                    entity.SelfEnterprenuerMale = model.SelfEnterprenuerMale;
                   
                    WorkingAreaLogService.Update(entity);
                    return GetSuccessMessageResult();
                }
                // return Json(new { Result = "OK" });
                //return RedirectToAction("Index");
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //
        // GET: /WorkingAreaLog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /WorkingAreaLog/Delete/5
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

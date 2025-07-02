using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class ApproveCellingController : BaseController
    {
        private readonly IOfficeService officeService;
        private readonly IApproveCellingService approveCellingService;
        private readonly IAspNetRoleService aspnetroleService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IProductService productService;

        public ApproveCellingController(IApproveCellingService approveCellingService, IAspNetRoleService aspnetroleService, IOfficeService officeService, IUltimateReportService ultimateReportService, IProductService productService)
        {
            this.approveCellingService = approveCellingService;
            this.aspnetroleService = aspnetroleService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.productService = productService;

        }

        private void MapDropDownList(ApproveCellingViewModel model)
        {

            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }




            var allrole = aspnetroleService.GetAll().ToList();
            var viewOffice = allrole.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name.ToString()
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.roleList = ofc_items;

            var processtype = new List<SelectListItem>();
            processtype.Add(new SelectListItem() { Text = "Loan", Value = "1", Selected = true });
            processtype.Add(new SelectListItem() { Text = "Savings", Value = "0" });
            model.producttype = processtype.AsEnumerable();


            var productList = productService.GetMany(w => w.IsActive == true).ToList();

            var viewproductList = productList.OrderBy(x => x.ProductID).Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductName.ToString()
            });

            var productList_items = new List<SelectListItem>();
            productList_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            productList_items.AddRange(viewproductList);
            model.productlist = productList_items;

            //var allroleName = aspnetroleService.GetAll().ToList();
            //var viewRoleName = allrole.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.Name.ToString(),
            //    Text = x.Name.ToString()
            //});
            //var allroleNameItem = new List<SelectListItem>();
            //allroleNameItem.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //allroleNameItem.AddRange(viewRoleName);
            //model.roleName = allroleNameItem;
        }
        public JsonResult GetApproveCelling(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                List<ApproveCellingViewModel> List_LoanApprovalViewModel = new List<ApproveCellingViewModel>();
                var param = new { OrgID = SessionHelper.LoginUserOrganizationID };
                var allSavingsummary = ultimateReportService.getArroveCellingList(param);

                List_LoanApprovalViewModel = allSavingsummary.Tables[0].AsEnumerable()
                  .Select(row => new ApproveCellingViewModel
                  {
                      ApproveCellingID = row.Field<long>("ApproveCellingID"),
                      RoleID = row.Field<int>("RoleID"),
                      ProductId = row.Field<int?>("ProductId"),
                      MinRange = row.Field<decimal>("MinRange"),

                      MaxRange = row.Field<decimal>("MaxRange"),

                      ProdType = row.Field<int>("ProdType"),
                      RoleName = row.Field<string>("RoleName"),
                      ProductName = row.Field<string>("ProductName")

                  }).ToList();

                var currentPageRecords = List_LoanApprovalViewModel;
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = allSavingsummary.Tables[0].Rows.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        // GET: ApproveCelling
        public ActionResult Index()
        {
            return View();
        }

        // GET: ApproveCelling/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ApproveCelling/Create
        public ActionResult Create()
        {
            var model = new ApproveCellingViewModel();
            MapDropDownList(model);
            return View(model);
        }

        // POST: ApproveCelling/Create
        [HttpPost]
        public ActionResult Create(ApproveCellingViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");

            try
            {
                var entity = Mapper.Map<ApproveCellingViewModel, ApproveCelling>(model);
                foreach (var roleID in model.RoleIDs)
                {
                    //if exist then update approve celling configuration
                    var updateApproveCelling = approveCellingService.GetApproveCellingbyroleAndproductId(roleID, model.ProductId);
                    if (updateApproveCelling != null && updateApproveCelling.ApproveCellingID > 0)
                    {
                        int id = (int)Convert.ToInt64(updateApproveCelling.ApproveCellingID);
                        var updateModel = approveCellingService.GetById(id);
                        updateModel.RoleID = model.RoleID;
                        updateModel.RoleName = model.RoleName;
                        updateModel.MinRange = model.MinRange;
                        updateModel.MaxRange = model.MaxRange;
                        updateModel.ProdType = model.ProdType;
                        updateModel.ProductId = model.ProductId;

                        approveCellingService.Update(updateModel);

                        continue;
                    }

                    //if not exist then add new celling
                    entity.RoleID = roleID;
                    approveCellingService.Create(entity);
                }

                return GetSuccessMessageResult("Data Saved Successfully");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: ApproveCelling/Edit/5
        public ActionResult Edit(int id)
        {
            var loanapproval = approveCellingService.GetById(id);
            var entity = Mapper.Map<ApproveCelling, ApproveCellingViewModel>(loanapproval);

            ViewBag.purposeList = string.Format("{0} - {1}", loanapproval.RoleID, loanapproval.RoleName);



            MapDropDownList(entity);

            return View(entity);
        }

        // POST: ApproveCelling/Edit/5
        [HttpPost]
        public ActionResult Edit(ApproveCellingViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult();

            try
            {
                var entity = Mapper.Map<ApproveCellingViewModel, ApproveCelling>(model);
                var updateApproveCelling = approveCellingService.GetApproveCellingbyroleAndproductId(model.RoleID, model.ProductId);
                var approvCeilingId = entity.ApproveCellingID;

                //if exist then update approve celling configuration
                if (updateApproveCelling != null && updateApproveCelling.ApproveCellingID > 0)
                {
                    updateApproveCelling.RoleID = entity.RoleID;
                    updateApproveCelling.ProductId = entity.ProductId;

                    updateApproveCelling.RoleName = entity.RoleName;
                    updateApproveCelling.MinRange = entity.MinRange;
                    updateApproveCelling.MaxRange = entity.MaxRange;
                    updateApproveCelling.ProdType = entity.ProdType;

                    approveCellingService.Update(updateApproveCelling);
                    return GetSuccessMessageResult();
                }

                //if not exist then add new celling
                entity.ApproveCellingID = 0;
                approveCellingService.Create(entity);

                //let's delete unwanted configuration
                approveCellingService.Delete((int)approvCeilingId);

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: ApproveCelling/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ApproveCelling/Delete/5
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

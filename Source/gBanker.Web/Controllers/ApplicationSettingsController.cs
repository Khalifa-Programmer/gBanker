using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class ApplicationSettingsController : BaseController
    {
        private readonly IOfficeService officeService;
        private readonly IApplicationSettingsService applicationSettingsService;
        private readonly IProductService productService;
        public ApplicationSettingsController(IApplicationSettingsService applicationSettingsService, IProductService productService, IOfficeService officeService)
          {
              this.applicationSettingsService = applicationSettingsService;
              this.productService = productService;
              this.officeService = officeService;
             
          }

        // GET: ApplicationSettings
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetApplicationSetting(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var allApplication = applicationSettingsService.GetApplicationDetailDetail( LoggedInOrganizationID,LoginUserOfficeID);

                var totalCount = allApplication.Count();
                var entities = allApplication.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<DBApplicationSettingsDetail>, IEnumerable<ApplicationSettingViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        private void MapDropDownList(ApplicationSettingViewModel model)
        {

            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }

            var allbranch = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.OrgID==LoggedInOrganizationID);

            var viewbranch = allbranch.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.branchListItems = viewbranch;

            var processtype = new List<SelectListItem>();
            processtype.Add(new SelectListItem() { Text = "Average", Value = "Average", Selected = true });
            processtype.Add(new SelectListItem() { Text = "Lowest", Value = "Lowest" });
            model.processtypeItems = processtype.AsEnumerable();
            // paymentMode.Add(new SelectListItem() { Text = "Bank", Value = "98" });

        }
        // GET: ApplicationSettings/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ApplicationSettings/Create
        public ActionResult Create()
        {

            var model = new ApplicationSettingViewModel() { TransactionDate=System.DateTime.Now,YearClosingDate=System.DateTime.Now,LicenseStartDate=System.DateTime.Now,LicenseEndDate=System.DateTime.Now,MonthClosingDate=System.DateTime.Now,OperationStartDate=System.DateTime.Now};
           
            MapDropDownList(model);

            return View(model);
        }

        // POST: ApplicationSettings/Create
        [HttpPost]
        public ActionResult Create(ApplicationSettingViewModel model, FormCollection form)
        {
            try
            {

                var entity = Mapper.Map<ApplicationSettingViewModel, ApplicationSetting>(model);

                //Add Validlation Logic.

                if (ModelState.IsValid)
                {
                    string msg = "";
                    entity.IsActive = true;
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                    var errors = applicationSettingsService.IsValidSettings(entity);
                    if (errors.ToList().Count == 0)
                    {
                        entity.OrganizationID = Convert.ToInt16(entity.OfficeID);
                        applicationSettingsService.Create(entity);
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

        // GET: ApplicationSettings/Edit/5
        public ActionResult Edit(int id)
        {
            var applicationSetting = applicationSettingsService.GetById(id);
            var entity = Mapper.Map<ApplicationSetting, ApplicationSettingViewModel>(applicationSetting);
            MapDropDownList(entity);
            return View(entity);
        }

        // POST: ApplicationSettings/Edit/5
        [HttpPost]
        public ActionResult Edit(ApplicationSettingViewModel model)
        {
            try
            {

                var entity = Mapper.Map<ApplicationSettingViewModel, ApplicationSetting>(model);
                var getApplicationSetting = applicationSettingsService.GetById(Convert.ToInt16(entity.ApplicationSettingsID));
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {

                    var errors = applicationSettingsService.IsValidEdit(entity);
                    if (errors.ToList().Count == 0)
                    {
                        entity.IsActive = true;
                        getApplicationSetting.BankAccount = entity.BankAccount;
                        getApplicationSetting.CashBook = entity.CashBook;
                        getApplicationSetting.CellNo = entity.CellNo;
                        getApplicationSetting.Email = entity.Email;
                        getApplicationSetting.LicenseEndDate = entity.LicenseEndDate;
                        getApplicationSetting.LicenseNo = entity.LicenseNo;
                        getApplicationSetting.LicenseStartDate = entity.LicenseStartDate;
                        getApplicationSetting.MonthClosingDate = entity.MonthClosingDate;
                        getApplicationSetting.OperationStartDate = entity.OperationStartDate;
                        getApplicationSetting.OrganizationAddress = entity.OrganizationAddress;
                        getApplicationSetting.OrganizationName = entity.OrganizationName;
                        getApplicationSetting.PhoneNo = entity.PhoneNo;
                        getApplicationSetting.PLAccount = entity.PLAccount;
                        getApplicationSetting.ProcessType = entity.ProcessType;
                        getApplicationSetting.TransactionDate = entity.TransactionDate;
                        getApplicationSetting.YearClosingDate = entity.YearClosingDate;
                        applicationSettingsService.Update(getApplicationSetting);
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

        // GET: ApplicationSettings/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ApplicationSettings/Delete/5
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

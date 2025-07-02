using AutoMapper;
using gBanker.Data.CodeFirstMigration;
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
    public class MemberPassBookStockController : BaseController
    {
        #region Variables
        private readonly IMemberPassBookRegisterService MemberPassBookRegisterService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly ICenterService centerService;
        private readonly IMemberPassBookStockService MemberPassBookStockService;
        public MemberPassBookStockController(IOfficeService officeService, IMemberPassBookRegisterService MemberPassBookRegisterService, IMemberService memberService, ICenterService centerService,IMemberPassBookStockService MemberPassBookStockService)
        {
            this.memberService = memberService;
            this.officeService = officeService;
            this.MemberPassBookRegisterService = MemberPassBookRegisterService;
            this.centerService = centerService;
            this.MemberPassBookStockService = MemberPassBookStockService;
        }
        #endregion

        public JsonResult GetMemberPassBookStock(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                long totalCount;
                var allSavingsummary = MemberPassBookStockService.getPassBookStock(SessionHelper.LoginUserOfficeID.Value);

                var currentPageRecords = Mapper.Map<IEnumerable<getMemberPassBookStock_Result>, IEnumerable<MemberPassBookStockViewModel>>(allSavingsummary);
                return Json(new { Result = "OK", Records = currentPageRecords });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        private void MapDropDownList(MemberPassBookStockViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
         



            var alloffice = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value && l.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;




        




        }
        // GET: MemberPassBookStock
        public ActionResult Index()
        {
            return View();
        }

        // GET: MemberPassBookStock/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MemberPassBookStock/Create
        public ActionResult Create()
        {
            var model = new MemberPassBookStockViewModel();
          

            MapDropDownList(model);

            return View(model);
        }

        // POST: MemberPassBookStock/Create
        [HttpPost]
        public ActionResult Create(MemberPassBookStockViewModel model, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<MemberPassBookStockViewModel, MemberPassBookStock>(model);
                    entity.OrgID =Convert.ToInt16(LoggedInOrganizationID);
                    var errors = MemberPassBookStockService.IsValidPassBookStock(entity);
                    if (errors.ToList().Count == 0)
                    {
                        entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                        //Add Validlation Logic.
                        entity.IsActive = true;
                        entity.LotNo = model.LotNo;
                        entity.Qty = model.Qty;
                        entity.StartingNo = model.StartingNo;
                        entity.LastIssue = model.LastIssue;
                        MemberPassBookStockService.Create(entity);
                        return GetSuccessMessageResult();
                    }
                    else
                    {

                        return GetErrorMessageResult(errors);
                    }
                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: MemberPassBookStock/Edit/5
        public ActionResult Edit(long id)
        {
            var loanapproval = MemberPassBookStockService.GetByIdLong(id);

        
            var entity = Mapper.Map<MemberPassBookStock, MemberPassBookStockViewModel>(loanapproval);
           
            MapDropDownList(entity);

            return View(entity);
        }

        // POST: MemberPassBookStock/Edit/5
        [HttpPost]
        public ActionResult Edit(MemberPassBookStockViewModel model)
        {
            try
            {
                var entity = Mapper.Map<MemberPassBookStockViewModel, MemberPassBookStock>(model);
                var getPurpose = MemberPassBookStockService.GetByIdLong(entity.MemberPassBookStockID);
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    var errors = MemberPassBookStockService.IsValidPassBookStock(entity);
                    if (errors.ToList().Count == 0)
                    {
                    getPurpose.LotNo = entity.LotNo;
                    getPurpose.Qty = entity.Qty;
                    getPurpose.StartingNo = entity.StartingNo;
                    getPurpose.LastIssue = entity.LastIssue; ;
                    MemberPassBookStockService.Update(getPurpose);
                    return GetSuccessMessageResult();
                    }
                    else
                    {

                        return GetErrorMessageResult(errors);
                    }
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: MemberPassBookStock/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MemberPassBookStock/Delete/5
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

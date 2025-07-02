using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
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
    public class MemberWisePassbookBalanceEntryController : BaseController
    {
        // GET: ProductXEmploymentProductMapping
        private readonly IProductService productService;
        private readonly ICenterService centerService;
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly IMemberWisePassbookBalanceEntryService memberWisePassbookBalanceEntryService;

        public MemberWisePassbookBalanceEntryController(IProductService productService, IMemberWisePassbookBalanceEntryService memberWisePassbookBalanceEntryService,
            ICenterService centerService,
            IOfficeService officeService, IMemberService memberService
            )
        {
            this.productService = productService;
            this.memberWisePassbookBalanceEntryService = memberWisePassbookBalanceEntryService;
            this.centerService = centerService;
            this.productService = productService;
            this.officeService = officeService;
            this.memberService = memberService;

        }
        public ActionResult Create()
        {
            var model = new MemberWisePassbookBalanceEntryViewModel();

            model.OfficeId = (int)SessionHelper.LoginUserOfficeID;
            MapDropDownList(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateEntry(MemberWisePassbookBalanceEntryViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");

            try
            {

                if (model.MemberWisePassbookBalanceEntryId > 0)
                {
                    var entity = Mapper.Map<MemberWisePassbookBalanceEntryViewModel, MemberWisePassbookBalanceEntry>(model);

                    entity.OfficeId = model.OfficeId;
                    entity.CenterId = model.CenterId;
                    entity.ProductId = model.ProductId;
                    entity.Amount = model.Amount;
                    entity.IsActive = true;
                    memberWisePassbookBalanceEntryService.Update(entity);
                }

                else
                {
                    var entity = Mapper.Map<MemberWisePassbookBalanceEntryViewModel, MemberWisePassbookBalanceEntry>(model);
                    entity.IsActive = true;
                    memberWisePassbookBalanceEntryService.Create(entity);
                }
                
                //return GetSuccessMessageResult("Data Saved Successfully");
                return Json ("Data Saved Successfully");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        public ActionResult Edit(int id)
        {
            var employMappingRes = memberWisePassbookBalanceEntryService.GetById(id);
          
            var entity = Mapper.Map<MemberWisePassbookBalanceEntry ,MemberWisePassbookBalanceEntryViewModel>(employMappingRes);
            var memberInfo = memberService.GetById(entity.MemberId);

            MapDropDownList(entity);
            entity.MemberName = memberInfo.MemberCode+"- "+memberInfo.FirstName + " " + memberInfo.LastName;

            return View(entity);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                memberWisePassbookBalanceEntryService.DeleteById(id);
                return GetSuccessMessageResult("Data Deleted Successfully");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        private void MapDropDownList(MemberWisePassbookBalanceEntryViewModel model)
        {

            var productList = productService.GetProductCodeList();

            var viewproductList = productList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductCode.ToString(),
                Text = x.ProductName.ToString()
            });

            var productList_items = new List<SelectListItem>();
            productList_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            productList_items.AddRange(viewproductList);
            model.ProductList = productList_items;

            //var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));
            var allcenter = centerService.GetAll();
            //var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt16(LoggedInEmployeeID));
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll();

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;
        }

        public ActionResult GetMemberList(string memberid, string centerId, string officeId)
        {

            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            //if (Session[MemberByCenterSessionKey] != null)
            //    memberList = Session[MemberByCenterSessionKey] as List<Member>;
            //else
            //{
                var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(officeId), Convert.ToInt16(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            //}
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName) + ' ' + (string.IsNullOrEmpty(m.RefereeName) ? "" : m.RefereeName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName) + ' ' + (string.IsNullOrEmpty(m1.RefereeName) ? "" : m1.RefereeName)) }).ToList();
            return Json(members, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                List<MemberWisePassbookBalanceEntryViewModel> List_MemberWisePassbookBalanceEntryViewModel = new List<MemberWisePassbookBalanceEntryViewModel>();
                var param = new { };
                var memberWisePassbookBalanceEntryList = memberWisePassbookBalanceEntryService.GetMemberWisePassbookBalanceEntryList().ToList();

                var currentPageRecords = memberWisePassbookBalanceEntryList;
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = memberWisePassbookBalanceEntryList.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
    
}
}
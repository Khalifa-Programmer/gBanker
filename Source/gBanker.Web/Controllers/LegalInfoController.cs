using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class LegalInfoController : BaseController
    {
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly ICenterService centerService;
        private readonly IProductService productService;
        private readonly ILegalInfoService legalInfoService;
        private readonly IUltimateReportService ultimateReportService;

        public LegalInfoController(IOfficeService officeService, IMemberService memberService, ICenterService centerService, IProductService productService, ILegalInfoService legalInfoService, IUltimateReportService ultimateReportService)
        {
            this.officeService = officeService;
            this.memberService = memberService;
            this.centerService = centerService;
            this.productService = productService;
            this.legalInfoService = legalInfoService;
            this.ultimateReportService = ultimateReportService;
        }

        #region Events
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            var model = new LegalInfoViewModel();
            MapDropDownList(model);
            return View(model);
        }
        public ActionResult Edit(long Id)
        {
            var entity = legalInfoService.GetById((int)Id);
            var mappedData = Mapper.Map<LegalInfo, LegalInfoViewModel>(entity);
            MapDropDownListEdit(mappedData);
            return View(mappedData);
        }       

        #endregion Events

        #region Methods


        [HttpPost]
        public ActionResult Create(LegalInfoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<LegalInfoViewModel, LegalInfo>(model);
                    entity.IsActive = true;
                    entity.CreateUser = LoggedInEmployeeID.ToString();
                    entity.CreateDate = DateTime.Now;
                    legalInfoService.Create(entity);
                    return GetSuccessMessageResult();
                }
                else
                {
                    return GetErrorMessageResult();
                }
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        [HttpPost]
        public ActionResult Edit(LegalInfoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<LegalInfoViewModel, LegalInfo>(model);
                    entity.IsActive = true;
                    entity.UpdateUser = LoggedInEmployeeID.ToString();
                    entity.UpdateDate = DateTime.Now;
                    legalInfoService.Update(entity);
                    return GetSuccessMessageResult();
                }
                else
                {
                    return GetErrorMessageResult();
                }
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        private void MapDropDownList(LegalInfoViewModel model)
         {

            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allcenter = centerService.GetByOfficeId(LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });
            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll().Where(a => a.OfficeID == LoginUserOfficeID.Value && a.OrgID == LoggedInOrganizationID.Value);
            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });
            model.officeListItems = viewOffice;

            var allSearchProd = productService.SearchProduct(0, Convert.ToInt32(LoggedInOrganizationID), "S");
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.productListItems = proditems;

        }

        private void MapDropDownListEdit(LegalInfoViewModel model)
        {

            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allcenter = centerService.GetByOfficeId(LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });
            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll().Where(a => a.OfficeID == LoginUserOfficeID.Value && a.OrgID == LoggedInOrganizationID.Value);
            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });
            model.officeListItems = viewOffice;

            var allSearchProd = productService.SearchProduct(0, Convert.ToInt32(LoggedInOrganizationID), "S");
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            proditems.AddRange(viewProdList);
            model.productListItems = proditems;
        }

        public JsonResult GetLegalInfoList(int jtStartIndex, int jtPageSize)
        {
            try
            {
                var getData = ultimateReportService.GetDataWithoutParameter("GetLegalInfoList");
                var viewData = getData.Tables[0].AsEnumerable().Select(p => new LegalInfoViewModel
                {
                    Id = p.Field<long>("Id"),
                    OfficeName = p.Field<string>("OfficeName"),
                    CenterName = p.Field<string>("CenterName"),
                    MemberName = p.Field<string>("MemberName"),
                    ProductName = p.Field<string>("ProductName"),
                    CaseNo = p.Field<string>("CaseNo"),
                    CaseDateS = p.Field<string>("CaseDate"),
                    Remarks = p.Field<string>("Remarks"),
                }).ToList();

                var totalCount = viewData.Count();
                var currentPageRecords = viewData.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        #endregion Methods
    }
}
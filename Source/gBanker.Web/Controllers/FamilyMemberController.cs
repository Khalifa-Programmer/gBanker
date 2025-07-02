using gBanker.Data;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class FamilyMemberController : BaseController
    {
        #region Variables


        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IGeoLocationService geoLocationService;
        private readonly ICenterService centerService;
        private readonly IGroupService groupService;
        private readonly IMemberCategoryService memberCategoryService;
        private readonly IProductService productService;
        private readonly IMemberLastCodeService memberLastCodeService;
        private readonly ICountryService countryService;
        private readonly ILgVillageService lgVillageService;
        private UserManager<ApplicationUser> userManager;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IAccReportService accReportService;
        private readonly ISavingSummaryService savingSummaryService;
        private readonly ISavingTrxService savingTrxService;
        private readonly IOrganizationService organizationService;


        private object receiver;

        public FamilyMemberController(IOfficeService officeService, IGeoLocationService geoLocationService, IMemberService memberService, ICenterService centerService, IGroupService groupService, IMemberCategoryService memberCategoryService, IProductService productService, IMemberLastCodeService memberLastCodeService, ICountryService countryService, ILgVillageService lgVillageService, UserManager<ApplicationUser> userManager, IUltimateReportService ultimateReportService, IAccReportService accReportService, ISavingSummaryService savingSummaryService, ISavingTrxService savingTrxService, IOrganizationService organizationService)
        {
            this.memberService = memberService;
            this.officeService = officeService;
            this.geoLocationService = geoLocationService;
            this.centerService = centerService;
            this.groupService = groupService;
            this.memberCategoryService = memberCategoryService;
            this.productService = productService;
            this.memberLastCodeService = memberLastCodeService;
            this.countryService = countryService;
            this.lgVillageService = lgVillageService;
            this.userManager = userManager;
            this.ultimateReportService = ultimateReportService;
            this.accReportService = accReportService;
            this.savingSummaryService = savingSummaryService;
            this.savingTrxService = savingTrxService;
            this.organizationService = organizationService;


        }
        #endregion

        // GET: FamilyMemberSameHousehold
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            var model = new MemberDetailViewModel();
            MapDropDownList(model);
            return View(model);
        }
        private void MapDropDownList(MemberDetailViewModel model)
        {
            var offc_id = Convert.ToInt32(LoginUserOfficeID);
            var allCenter = centerService.GetByOfficeId(offc_id, Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + ", " + x.CenterName.ToString()
            });
            var center_items = new List<SelectListItem>();
            center_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            center_items.AddRange(viewOffice);
            model.CenterList = center_items;

            var gender_item = new List<SelectListItem>();
            gender_item.Add(new SelectListItem() { Text = "Male", Value = "M" });
            gender_item.Add(new SelectListItem() { Text = "Female", Value = "F", Selected = true });
            model.GenderList = gender_item;

            var relationship_item = new List<SelectListItem>();
            relationship_item.Add(new SelectListItem() { Text = "Son", Value = "1", Selected = true });
            relationship_item.Add(new SelectListItem() { Text = "Husband", Value = "2" });
            relationship_item.Add(new SelectListItem() { Text = "Father", Value = "3" });
            relationship_item.Add(new SelectListItem() { Text = "Mother", Value = "4" });
            relationship_item.Add(new SelectListItem() { Text = "Brother", Value = "5" });
            relationship_item.Add(new SelectListItem() { Text = "Sister", Value = "6" });
            relationship_item.Add(new SelectListItem() { Text = "Uncle", Value = "7" });
            // relationship_item.Add(new SelectListItem() { Text = "Son", Value = "8" });
            relationship_item.Add(new SelectListItem() { Text = "Daughter", Value = "8" });
            model.FamilyMemRelationshipList = relationship_item;


            var MaritalStatus = new List<SelectListItem>();
            MaritalStatus.Add(new SelectListItem() { Text = "Married", Value = "M" });
            MaritalStatus.Add(new SelectListItem() { Text = "UnMarried", Value = "U", Selected = true });
            model.MaritalStatusList = MaritalStatus;
        }
    }
}
using AutoMapper;
using gBanker.Data;
//using gBanker.Data.Db;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using gBanker.Service.ReportServies;
using BasicDataAccess;
using System.Configuration;
using System.Data.Common;
using BasicDataAccess.Data;
using gBanker.Data.DBDetailModels;
using System.Drawing.Imaging; //Added for Image
using System.Net.Http;
using System.Net.Http.Headers;
using gBanker.Core.Common;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Text;

namespace gBanker.Web.Controllers
{
    public class MemberController : BaseController
    {
        // GET: Member
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
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IEmployeeService employeeService;


        private object receiver;
        private static DataTable dtINIDList;
        public MemberController(IOfficeService officeService, IGeoLocationService geoLocationService, IMemberService memberService, ICenterService centerService, IGroupService groupService, IMemberCategoryService memberCategoryService, IProductService productService, IMemberLastCodeService memberLastCodeService, ICountryService countryService, ILgVillageService lgVillageService, UserManager<ApplicationUser> userManager, IUltimateReportService ultimateReportService, IAccReportService accReportService, ISavingSummaryService savingSummaryService, ISavingTrxService savingTrxService, IOrganizationService organizationService, IGroupwiseReportService groupwiseReportService, IEmployeeService employeeService)
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
            this.groupwiseReportService = groupwiseReportService;
            this.employeeService = employeeService;
        }
        #endregion

        #region Methods
        private void MapDropDownList(MemberViewModel model)
        {
            List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
            var param = new { SearchByCode = "", SearchBy = "", SearchType = "dis" };
            var disList = ultimateReportService.GetLocationList(param);

            List_MemberViewModel = disList.Tables[0].AsEnumerable()
            .Select(row => new MemberViewModel
            {
                DistrictCode = row.Field<string>("DistrictCode"),
                DistrictName = row.Field<string>("DistrictName")
            }).ToList();
            var viewDist = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.DistrictCode.ToString(),
                Text = x.DistrictName.ToString()
            });
            var pob_items = new List<SelectListItem>();
            pob_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            pob_items.AddRange(viewDist);
            model.PlaceOfBirthList = pob_items;




            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OfficeID == offc_id && m.OrgID == LoggedInOrganizationID).ToList();
            // var allOffice = officeService.GetById(offc_id);
            var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            //var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.OfficeList = ofc_items;

            var allMemcatService = memberCategoryService.GetAll().Where(w => w.IsActive == true && w.OrgID == LoggedInOrganizationID).OrderBy(o => o.MemberCategoryCode).ToList();
            var viewMemCatService = allMemcatService.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MemberCategoryID.ToString(),
                Text = string.Format("{0}, {1}", x.MemberCategoryCode.ToString(), x.CategoryName.ToString())
            });
            var cat_items = new List<SelectListItem>();
            cat_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            cat_items.AddRange(viewMemCatService);
            model.MemberCategoryList = cat_items;


            var allGeoLocation = geoLocationService.GetAll();
            var viewGeoLocation = allGeoLocation.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GeoLocationID.ToString(),
                Text = x.LocationName.ToString()
            });
            var geo_items = new List<SelectListItem>();
            geo_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            geo_items.AddRange(viewGeoLocation);
            model.GeoLocationList = geo_items;

            var allCountry = countryService.GetAll();
            var viewCountry = allCountry.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString(),
                Selected = x.CountryId.ToString() == "18" ? true : false
            });
            var country_items = new List<SelectListItem>();
            country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            country_items.AddRange(viewCountry);
            model.CountryList = country_items;

            //blank division
            var divisionList = new List<SelectListItem>();
            divisionList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.DivisionList = divisionList;

            //blank district
            var districtList = new List<SelectListItem>();
            districtList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.DistrictList = districtList;

            //blank upozilla
            var upozillaList = new List<SelectListItem>();
            upozillaList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.UpozillaList = upozillaList;

            //blank Union
            var unionList = new List<SelectListItem>();
            unionList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.UnionList = unionList;

            //blank Village
            var villageList = new List<SelectListItem>();
            villageList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.VillageList = villageList;

            var gender_item = new List<SelectListItem>();
            gender_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            gender_item.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            gender_item.Add(new SelectListItem() { Text = "Female", Value = "Female" });
            gender_item.Add(new SelectListItem() { Text = "Transgender", Value = "T" });
            model.GenderList = gender_item;

            var status_item = new List<SelectListItem>();
            status_item.Add(new SelectListItem() { Text = "Active", Value = "1", Selected = true });
            status_item.Add(new SelectListItem() { Text = "In Active", Value = "0" });
            status_item.Add(new SelectListItem() { Text = "Drop", Value = "2" });
            status_item.Add(new SelectListItem() { Text = "Dead", Value = "3" });
            status_item.Add(new SelectListItem() { Text = "Blacklist", Value = "4" });
            status_item.Add(new SelectListItem() { Text = "Rejected", Value = "5" });
            model.MemberStatusList = status_item;

            var cityzen_item = new List<SelectListItem>();
            cityzen_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            cityzen_item.Add(new SelectListItem() { Text = "By Birth", Value = "BB" });
            cityzen_item.Add(new SelectListItem() { Text = "Migrated", Value = "MI" });
            cityzen_item.Add(new SelectListItem() { Text = "Marital", Value = "MA" });
            cityzen_item.Add(new SelectListItem() { Text = "Nutralization", Value = "NU" });
            model.CityzenshipList = cityzen_item;

            var homeType_item = new List<SelectListItem>();
            homeType_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            homeType_item.Add(new SelectListItem() { Text = "Building", Value = "BU" });
            homeType_item.Add(new SelectListItem() { Text = "Muddy", Value = "MU" });
            homeType_item.Add(new SelectListItem() { Text = "Rented", Value = "RE" });
            homeType_item.Add(new SelectListItem() { Text = "Semi Building", Value = "SB" });
            homeType_item.Add(new SelectListItem() { Text = "Tin Shade", Value = "TN" });
            model.HomeTypeList = homeType_item;

            var groupType_item = new List<SelectListItem>();
            groupType_item.Add(new SelectListItem() { Text = "Solidarity", Value = "SO", Selected = true });
            groupType_item.Add(new SelectListItem() { Text = "Non Solidarity", Value = "NS" });
            groupType_item.Add(new SelectListItem() { Text = "Individual", Value = "IN" });
            groupType_item.Add(new SelectListItem() { Text = "Corporate", Value = "CO" });
            model.GroupTypeList = groupType_item;

            var education_item = new List<SelectListItem>();
            //education_item.Add(new SelectListItem() { Text = "Under Matric", Value = "UMA", Selected = true });
            education_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            education_item.Add(new SelectListItem() { Text = "Pre-Primary", Value = "1" });
            education_item.Add(new SelectListItem() { Text = "Primary", Value = "2" });
            //education_item.Add(new SelectListItem() { Text = "JSC", Value = "JSC" });
            education_item.Add(new SelectListItem() { Text = "Secondary", Value = "3" });
            education_item.Add(new SelectListItem() { Text = "Higher Secondary", Value = "4" });
            //education_item.Add(new SelectListItem() { Text = "Diploma", Value = "DIP" });
            education_item.Add(new SelectListItem() { Text = "Graduate", Value = "5" });
            education_item.Add(new SelectListItem() { Text = "PostGraduate", Value = "6" });
            //education_item.Add(new SelectListItem() { Text = "Illiterate", Value = "ILL" });
            education_item.Add(new SelectListItem() { Text = "Other", Value = "7" });
            model.EducationList = education_item;

            var economic_item = new List<SelectListItem>();
            economic_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            economic_item.Add(new SelectListItem() { Text = "Self Employed", Value = "SM" });
            //economic_item.Add(new SelectListItem() { Text = "House Hold", Value = "HH" });
            economic_item.Add(new SelectListItem() { Text = "Service", Value = "SE" });
            economic_item.Add(new SelectListItem() { Text = "Business", Value = "BU" });
            economic_item.Add(new SelectListItem() { Text = "House Wife", Value = "HW" });
            economic_item.Add(new SelectListItem() { Text = "Agriculture", Value = "AG" });
            economic_item.Add(new SelectListItem() { Text = "Others", Value = "OH" });
            model.EconomicActivityList = economic_item;

            var marital_item = new List<SelectListItem>();
            marital_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            marital_item.Add(new SelectListItem() { Text = "Married", Value = "Married" });
            marital_item.Add(new SelectListItem() { Text = "Unmarried", Value = "Unmarried" });
            marital_item.Add(new SelectListItem() { Text = "Single", Value = "Single" });
            //marital_item.Add(new SelectListItem() { Text = "Divorce", Value = "Divorce" });
            //marital_item.Add(new SelectListItem() { Text = "Widow", Value = "Widow" });
            model.MaritalStatusList = marital_item;
            var memCat_item = new List<SelectListItem>();

            if (LoggedInOrganizationID == 54 || LoggedInOrganizationID == 4)
            {
                memCat_item.Add(new SelectListItem() { Text = "Member", Value = "1", Selected = true });
            }
            else
            {
                memCat_item.Add(new SelectListItem() { Text = "Member", Value = "1", Selected = true });
                memCat_item.Add(new SelectListItem() { Text = "Member", Value = "1", Selected = true });
                memCat_item.Add(new SelectListItem() { Text = "Depositor", Value = "2" });
                memCat_item.Add(new SelectListItem() { Text = "Family", Value = "3" });
                memCat_item.Add(new SelectListItem() { Text = "Others", Value = "4" });
                memCat_item.Add(new SelectListItem() { Text = "Dormant", Value = "5" });

            }

            model.MemCategoryList = memCat_item;

            var employee_item = new List<SelectListItem>();
            employee_item.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.EmployeeList = employee_item;

        }
        public JsonResult GetDivisionList(string country_id)
        {
            List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
            var param = new { SearchByCode = country_id, SearchBy = "con", SearchType = "div" };
            var divList = ultimateReportService.GetLocationList(param);

            List_MemberViewModel = divList.Tables[0].AsEnumerable()
            .Select(row => new MemberViewModel
            {
                DivisionCode = row.Field<string>("DivisionCode"),
                DivisionName = row.Field<string>("DivisionName")
            }).ToList();

            var viewDivision = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.DivisionCode.ToString(),
                Text = x.DivisionCode.ToString() + " " + x.DivisionName.ToString()
            });
            var div_items = new List<SelectListItem>();
            div_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            div_items.AddRange(viewDivision);
            return Json(div_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDistrictList(string div_id)
        {
            List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
            var param = new { SearchByCode = div_id, SearchBy = "div", SearchType = "dis" };
            var disList = ultimateReportService.GetLocationList(param);

            List_MemberViewModel = disList.Tables[0].AsEnumerable()
            .Select(row => new MemberViewModel
            {
                DistrictCode = row.Field<string>("DistrictCode"),
                DistrictName = row.Field<string>("DistrictName")
            }).ToList();

            var viewDist = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.DistrictCode.ToString(),
                Text = x.DistrictCode.ToString() + " " + x.DistrictName.ToString()
            });
            var dis_items = new List<SelectListItem>();
            dis_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            dis_items.AddRange(viewDist);
            return Json(dis_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUpozillaList(string dis_id)
        {
            List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
            var param = new { SearchByCode = dis_id, SearchBy = "dis", SearchType = "upo" };
            var upoList = ultimateReportService.GetLocationList(param);

            List_MemberViewModel = upoList.Tables[0].AsEnumerable()
            .Select(row => new MemberViewModel
            {
                UpozillaCode = row.Field<string>("UpozillaCode"),
                UpozillaName = row.Field<string>("UpozillaName")
            }).ToList();

            var viewUpo = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.UpozillaCode.ToString(),
                Text = x.UpozillaCode.ToString() + " " + x.UpozillaName.ToString()
            });
            var upo_items = new List<SelectListItem>();
            upo_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            upo_items.AddRange(viewUpo);
            return Json(upo_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUnionList(string upo_id)
        {
            List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
            var param = new { SearchByCode = upo_id, SearchBy = "upo", SearchType = "uni" };
            var uniList = ultimateReportService.GetLocationList(param);

            List_MemberViewModel = uniList.Tables[0].AsEnumerable()
            .Select(row => new MemberViewModel
            {
                UnionCode = row.Field<string>("UnionCode"),
                UnionName = row.Field<string>("UnionName")
            }).ToList();

            var viewUni = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.UnionCode.ToString(),
                Text = x.UnionCode.ToString() + " " + x.UnionName.ToString()
            });
            var uni_items = new List<SelectListItem>();
            uni_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            uni_items.AddRange(viewUni);
            return Json(uni_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVillageList(string uni_id)
        {
            List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
            var param = new { SearchByCode = uni_id, SearchBy = "uni", SearchType = "vil" };
            var vilList = ultimateReportService.GetLocationList(param);

            List_MemberViewModel = vilList.Tables[0].AsEnumerable()
            .Select(row => new MemberViewModel
            {
                VillageCode = row.Field<string>("VillageCode"),
                VillageName = row.Field<string>("VillageName")
            }).ToList();

            var viewVil = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.VillageCode.ToString(),
                Text = x.VillageCode.ToString() + " " + x.VillageName.ToString()
            });
            var vil_items = new List<SelectListItem>();
            vil_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            vil_items.AddRange(viewVil);
            return Json(vil_items, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetDivisionList(string country_id)
        //{
        //    List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
        //    var param = new { SearchByCode = country_id, SearchBy = "con", SearchType = "div" };
        //    var divList = ultimateReportService.GetLocationList(param);

        //    List_MemberViewModel = divList.Tables[0].AsEnumerable()
        //    .Select(row => new MemberViewModel
        //    {
        //        DivisionCode = row.Field<string>("DivisionCode"),
        //        DivisionName = row.Field<string>("DivisionName")
        //    }).ToList();

        //    var viewDivision = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.DivisionCode.ToString(),
        //        Text = x.DivisionCode.ToString() + " " + x.DivisionName.ToString()
        //    });
        //    var div_items = new List<SelectListItem>();
        //    div_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
        //    div_items.AddRange(viewDivision);
        //    return Json(div_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetDistrictList(string div_id)
        //{
        //    List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
        //    var param = new { SearchByCode = div_id, SearchBy = "div", SearchType = "dis" };
        //    var disList = ultimateReportService.GetLocationList(param);

        //    List_MemberViewModel = disList.Tables[0].AsEnumerable()
        //    .Select(row => new MemberViewModel
        //    {
        //        DistrictCode = row.Field<string>("DistrictCode"),
        //        DistrictName = row.Field<string>("DistrictName")
        //    }).ToList();

        //    var viewDist = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.DistrictCode.ToString(),
        //        Text = x.DistrictCode.ToString() + " " + x.DistrictName.ToString()
        //    });
        //    var dis_items = new List<SelectListItem>();
        //    dis_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
        //    dis_items.AddRange(viewDist);
        //    return Json(dis_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetUpozillaList(string dis_id)
        //{
        //    List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
        //    var param = new { SearchByCode = dis_id, SearchBy = "dis", SearchType = "upo" };
        //    var upoList = ultimateReportService.GetLocationList(param);

        //    List_MemberViewModel = upoList.Tables[0].AsEnumerable()
        //    .Select(row => new MemberViewModel
        //    {
        //        UpozillaCode = row.Field<string>("UpozillaCode"),
        //        UpozillaName = row.Field<string>("UpozillaName")
        //    }).ToList();

        //    var viewUpo = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.UpozillaCode.ToString(),
        //        Text = x.UpozillaCode.ToString() + " " + x.UpozillaName.ToString()
        //    });
        //    var upo_items = new List<SelectListItem>();
        //    upo_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
        //    upo_items.AddRange(viewUpo);
        //    return Json(upo_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetUnionList(string upo_id)
        //{
        //    List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
        //    var param = new { SearchByCode = upo_id, SearchBy = "upo", SearchType = "uni" };
        //    var uniList = ultimateReportService.GetLocationList(param);

        //    List_MemberViewModel = uniList.Tables[0].AsEnumerable()
        //    .Select(row => new MemberViewModel
        //    {
        //        UnionCode = row.Field<string>("UnionCode"),
        //        UnionName = row.Field<string>("UnionName")
        //    }).ToList();

        //    var viewUni = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.UnionCode.ToString(),
        //        Text = x.UnionCode.ToString() + " " + x.UnionName.ToString()
        //    });
        //    var uni_items = new List<SelectListItem>();
        //    uni_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
        //    uni_items.AddRange(viewUni);
        //    return Json(uni_items, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetVillageList(string uni_id)
        //{
        //    List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
        //    var param = new { SearchByCode = uni_id, SearchBy = "uni", SearchType = "vil" };
        //    var vilList = ultimateReportService.GetLocationList(param);

        //    List_MemberViewModel = vilList.Tables[0].AsEnumerable()
        //    .Select(row => new MemberViewModel
        //    {
        //        VillageCode = row.Field<string>("VillageCode"),
        //        VillageName = row.Field<string>("VillageName")
        //    }).ToList();

        //    var viewVil = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.VillageCode.ToString(),
        //        Text = x.VillageCode.ToString() + " " + x.VillageName.ToString()
        //    });
        //    var vil_items = new List<SelectListItem>();
        //    vil_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
        //    vil_items.AddRange(viewVil);
        //    return Json(vil_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetDivisionList(string country_id)
        //{
        //    List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
        //    var param = new { SearchByCode = country_id, SearchBy = "con", SearchType = "div" };
        //    var divList = ultimateReportService.GetLocationList(param);

        //    List_MemberViewModel = divList.Tables[0].AsEnumerable()
        //    .Select(row => new MemberViewModel
        //    {
        //        DivisionCode = row.Field<string>("DivisionCode"),
        //        DivisionName = row.Field<string>("DivisionName")
        //    }).ToList();

        //    var viewDivision = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.DivisionCode.ToString(),
        //        Text = x.DivisionName.ToString()
        //    });
        //    var div_items = new List<SelectListItem>();
        //    div_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
        //    div_items.AddRange(viewDivision);
        //    return Json(div_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetDistrictList(string div_id)
        //{
        //    List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
        //    var param = new { SearchByCode = div_id, SearchBy = "div", SearchType = "dis" };
        //    var disList = ultimateReportService.GetLocationList(param);

        //    List_MemberViewModel = disList.Tables[0].AsEnumerable()
        //    .Select(row => new MemberViewModel
        //    {
        //        DistrictCode = row.Field<string>("DistrictCode"),
        //        DistrictName = row.Field<string>("DistrictName")
        //    }).ToList();

        //    var viewDist = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.DistrictCode.ToString(),
        //        Text = x.DistrictName.ToString()
        //    });
        //    var dis_items = new List<SelectListItem>();
        //    dis_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
        //    dis_items.AddRange(viewDist);
        //    return Json(dis_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetUpozillaList(string dis_id)
        //{
        //    List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
        //    var param = new { SearchByCode = dis_id, SearchBy = "dis", SearchType = "upo" };
        //    var upoList = ultimateReportService.GetLocationList(param);

        //    List_MemberViewModel = upoList.Tables[0].AsEnumerable()
        //    .Select(row => new MemberViewModel
        //    {
        //        UpozillaCode = row.Field<string>("UpozillaCode"),
        //        UpozillaName = row.Field<string>("UpozillaName")
        //    }).ToList();

        //    var viewUpo = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.UpozillaCode.ToString(),
        //        Text = x.UpozillaName.ToString()
        //    });
        //    var upo_items = new List<SelectListItem>();
        //    upo_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
        //    upo_items.AddRange(viewUpo);
        //    return Json(upo_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetUnionList(string upo_id)
        //{
        //    List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
        //    var param = new { SearchByCode = upo_id, SearchBy = "upo", SearchType = "uni" };
        //    var uniList = ultimateReportService.GetLocationList(param);

        //    List_MemberViewModel = uniList.Tables[0].AsEnumerable()
        //    .Select(row => new MemberViewModel
        //    {
        //        UnionCode = row.Field<string>("UnionCode"),
        //        UnionName = row.Field<string>("UnionName")
        //    }).ToList();

        //    var viewUni = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.UnionCode.ToString(),
        //        Text = x.UnionName.ToString()
        //    });
        //    var uni_items = new List<SelectListItem>();
        //    uni_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
        //    uni_items.AddRange(viewUni);
        //    return Json(uni_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetVillageList(string uni_id)
        //{
        //    List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
        //    var param = new { SearchByCode = uni_id, SearchBy = "uni", SearchType = "vil" };
        //    var vilList = ultimateReportService.GetLocationList(param);

        //    List_MemberViewModel = vilList.Tables[0].AsEnumerable()
        //    .Select(row => new MemberViewModel
        //    {
        //        VillageCode = row.Field<string>("VillageCode"),
        //        VillageName = row.Field<string>("VillageName")
        //    }).ToList();

        //    var viewVil = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.VillageCode.ToString(),
        //        Text = x.VillageName.ToString()
        //    });
        //    var vil_items = new List<SelectListItem>();
        //    vil_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
        //    vil_items.AddRange(viewVil);
        //    return Json(vil_items, JsonRequestBehavior.AllowGet);
        //}
        private string GetNewMemberCode(int offc_id, int group_id)
        {
            string last_code = "";
            var v = memberLastCodeService.GetByOffcGroupId(Convert.ToInt16(LoggedInOrganizationID), offc_id, group_id);
            var groupCode = groupService.GetById(group_id).GroupCode;
            var offcCode = officeService.GetById(offc_id).OfficeCode;
            if (v == null || v.LastCode == "") // if there is no voucher for this office
            {
                last_code = offcCode + groupCode + "0001";
                string new_code = offcCode + groupCode + "0002";
                var crt = new MemberLastCode();
                crt.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                crt.OfficeID = offc_id;
                crt.GroupID = group_id;
                crt.LastCode = new_code;
                memberLastCodeService.Create(crt);
            }
            else // collect last voucher no
            {
                last_code = v.LastCode;
                //int strt_val = last_code.Length - 3;
                //int cl_val = last_code.Length-1;
                //string new_code = last_code.Substring(strt_val, 3);

                string new_code = IncMemberCode(last_code.Substring(last_code.Length - 4, 4), "Add");
                var updt = new MemberLastCode();
                updt = memberLastCodeService.GetByLastCodeId(Convert.ToInt32(v.LastCodeID));
                updt.LastCode = offcCode + groupCode + new_code;
                memberLastCodeService.Update(updt);
            }

            return last_code;
        }
        private string IncMemberCode(string lastCode, string purpose)
        {
            string MemCode = "";
            if (purpose == "Add")
                lastCode = (Convert.ToInt32(lastCode) + 1).ToString();
            else
                lastCode = (Convert.ToInt32(lastCode) - 1).ToString();
            //MemCode = lastCode.ToString().PadLeft(7, '0');  
            if (lastCode.Length == 1)
            {
                MemCode = "0000" + lastCode;
            }
            else if (lastCode.Length == 2)
            {
                MemCode = "000" + lastCode;
            }
            else if (lastCode.Length == 3)
            {
                MemCode = "00" + lastCode;
            }
            else if (lastCode.Length == 4)
            {
                MemCode = "0" + lastCode;
                // MemCode = lastCode;
            }
            else if (lastCode.Length == 5)
            {
                MemCode = lastCode;
                // MemCode = lastCode;
            }
            return MemCode;
        }
        private Boolean IsSelected(Product product)
        {
            if (product.ProductID == 1 || product.ProductID == 2)
                return true;
            else
                return false;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCenterList(string office_id)
        {
            if (String.IsNullOrEmpty(office_id))
            {
                throw new ArgumentNullException("office_id");
            }
            var param1 = new { @EmpID = LoggedInEmployeeID };
            var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);
            IEnumerable<DBCenterDetailModel> getCenterByOfficeId;
            if (LoanInstallMent != null)
            {
                var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
                if (empType == "FO")
                {
                    var param3 = new { @OrgID = LoggedInOrganizationID, @OfficeID = LoginUserOfficeID, @EmployeeID = Convert.ToInt16(LoggedInEmployeeID), @Qtype = 1 };
                    var getMemberTolrecordEmp = ultimateReportService.GetCenterListEmployeeWise(param3);
                    getCenterByOfficeId = getMemberTolrecordEmp;
                }
                else
                {
                    var param3 = new { @OrgID = LoggedInOrganizationID, @OfficeID = LoginUserOfficeID, @EmployeeID = Convert.ToInt16(LoggedInEmployeeID), @Qtype = 2 };
                    var getMemberTolrecordEmp = ultimateReportService.GetCenterListEmployeeWise(param3);
                    getCenterByOfficeId = getMemberTolrecordEmp;
                }

            }

            else
            {
                var param3 = new { @OrgID = LoggedInOrganizationID, @OfficeID = LoginUserOfficeID, @EmployeeID = Convert.ToInt16(LoggedInEmployeeID), @Qtype = 2 };
                var getMemberTolrecordEmp = ultimateReportService.GetCenterListEmployeeWise(param3);
                getCenterByOfficeId = getMemberTolrecordEmp;
            }
            var viewCenter = getCenterByOfficeId.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = string.Format("{0}, {1}", x.CenterCode, x.CenterName)
            });
            var center_items = new List<SelectListItem>();
            if (viewCenter.ToList().Count > 0)
            {
                center_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            center_items.AddRange(viewCenter);
            return Json(center_items, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetGroupList(string office_id)
        {
            if (String.IsNullOrEmpty(office_id))
            {
                throw new ArgumentNullException("office_id");
            }
            var getGroupByOfficeId = groupService.GetAll().Where(m => m.OfficeID == Convert.ToInt32(office_id) && m.OrgID == Convert.ToInt16(LoggedInOrganizationID)).OrderBy(m => m.GroupCode);
            var viewGroup = getGroupByOfficeId.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GroupID.ToString(),
                Text = string.Format("{0}", x.GroupCode)
            });
            var group_items = new List<SelectListItem>();
            //if (viewGroup.ToList().Count > 0)
            //{
            //   // group_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //    group_items.Add(new SelectListItem());
            //}

            group_items.AddRange(viewGroup);
            return Json(group_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemberInfo(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, string TypeFilterColumn, string DateFromValue, string DateToValue)
        {
            try
            {
                long TotCount;

                var param1 = new { @EmpID = LoggedInEmployeeID };
                var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);
                IEnumerable<DBMemberDetailModel> memberDetail;
                var param2 = new { @orgID = LoggedInOrganizationID, @OfficeID = LoginUserOfficeID, @filterColumnName = filterColumn, @filterValue = filterValue, @TypeFilterColumn = TypeFilterColumn, @DateFrom = DateFromValue, @DateTo = DateToValue };
                var getMemberTolrecord = ultimateReportService.GetMemberDetailsasList(param2);
                var getMember = getMemberTolrecord.Skip(jtStartIndex).Take(jtPageSize);
                TotCount = getMemberTolrecord.Count;
                if (LoanInstallMent != null)
                {
                    var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
                    if (empType == "FO")
                    {
                        var param3 = new { @orgID = LoggedInOrganizationID, @OfficeID = LoginUserOfficeID, @filterColumnName = filterColumn, @filterValue = filterValue, @TypeFilterColumn = TypeFilterColumn, @EmployeeID = Convert.ToInt16(LoggedInEmployeeID), @DateFrom = DateToValue, @DateTo = DateToValue };
                        var getMemberTolrecordEmp = ultimateReportService.GetMemberDetailsasListEmployeeDateWise(param3);
                        var getMemberEmp = getMemberTolrecord.Skip(jtStartIndex).Take(jtPageSize);
                        TotCount = getMemberTolrecordEmp.Count;
                        memberDetail = getMemberEmp;
                    }
                    else
                        memberDetail = getMember;
                }

                else
                    memberDetail = getMember;
                var detail = memberDetail.ToList();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetEligibleMemberInfo(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long TotCount;

                var param1 = new { @EmpID = LoggedInEmployeeID };
                var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);
                var param2 = new { @orgID = LoggedInOrganizationID, @OfficeID = LoginUserOfficeID, @filterColumnName = filterColumn, @filterValue = filterValue, @TypeFilterColumn = "", @EmployeeID = Convert.ToInt16(LoggedInEmployeeID) };
                var getMemberTolrecord = ultimateReportService.GetElegibleMemberDetailsasList(param2);
                var getMember = getMemberTolrecord.Skip(jtStartIndex).Take(jtPageSize);
                TotCount = getMemberTolrecord.Count;
                IEnumerable<DBMemberDetailModel> memberDetail;
                if (LoanInstallMent != null)
                {
                    var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
                    if (empType == "FO")
                    {
                        var param3 = new { @orgID = LoggedInOrganizationID, @OfficeID = LoginUserOfficeID, @filterColumnName = filterColumn, @filterValue = filterValue, @TypeFilterColumn = "", @EmployeeID = Convert.ToInt16(LoggedInEmployeeID) };
                        var getMemberTolrecordEmp = ultimateReportService.GetElegibleMemberDetailsasListEmployeeWise(param2);
                        var getMemberEmp = getMemberTolrecord.Skip(jtStartIndex).Take(jtPageSize);
                        TotCount = getMemberTolrecordEmp.Count;
                        memberDetail = getMemberEmp;
                    }
                    else
                        memberDetail = getMember;
                }
                else
                    memberDetail = getMember;
                var detail = memberDetail.ToList();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        // Age Wise Member List
        public ActionResult MemberListAgeWise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }

        //GetMemberListAgeWise
        public JsonResult GetMemberListAgeWise(int jtStartIndex, int jtPageSize, string jtSorting, string centerID, int ageFrom, int ageTo)
        {
            try
            {
                long TotCount;

                IEnumerable<DBMemberDetailModel> model;
                var param2 = new { @OfficeID = LoginUserOfficeID, @CenterID = centerID, @AgeFrom = ageFrom, @AgeTo = ageTo };
                var memberList = ultimateReportService.GetMemberAsListAgeWise(param2);
                var getMember = memberList.Skip(jtStartIndex).Take(jtPageSize);
                TotCount = memberList.Count;

                model = getMember;
                var currentPageRecords = model.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //MemberListAgeWiseReport
        public ActionResult GenerateMemberListAgeWiseReport(string center, int ageFrom, int ageTo)
        {
            try
            {
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = center, @AgeFrom = ageFrom, @AgeTo = ageTo };
                var alldata = groupwiseReportService.GenerateMemberListReportAgeWise(param, "Proc_GetMemberListAgeWise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("@OfficeID", SessionHelper.LoginUserOfficeID);
                //reportParam.Add("@CenterID", center);
                reportParam.Add("AgeFrom", ageFrom);
                reportParam.Add("AgeTo", ageTo);
                ReportHelper.PrintReport("RptMemberListAgeWise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EligibleMember(string MemId)
        {
            var member = memberService.GetByIdLong(Convert.ToInt64(MemId));
            member.MemberStatus = "1";
            memberService.Update(member);
            return RedirectToAction("Eligible");
        }
        public byte[] GetImageFromDataBase(Int64 Id)
        {
            var memberDetail = memberService.GetByMemberId(Id);
            var img = memberDetail.MemberImg;
            //var q = from temp in  where temp.ID == Id select temp.Image;
            byte[] cover = img;
            return cover;



            ////Bitmap img;
            //var memberDetail = memberService.GetByMemberId(Id);
            //var img =  memberDetail.MemberImg;
            //DataSet vMemberImage;
            //byte[] nn;
            //if (memberDetail.MemberImg == null)
            //{
            //    var param2 = new { @MemberID = Id };
            //    vMemberImage = ultimateReportService.GetMemberImage(param2);
            //    if (vMemberImage.Tables[0].Rows.Count > 0)
            //    {
            //        nn = Convert.ToByte(vMemberImage.Tables[0].Rows[0]["MemberImg"].ToString());

            //    }

            //}



            ////var q = from temp in  where temp.ID == Id select temp.Image;
            //// byte[] cover = img;
            //return img;
        }
        //public ActionResult RetrieveImage(Int64 id)
        //{
        //    if (id == null)
        //    {
        //        string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
        //        Image img = Image.FromFile(strImgPathAbsolute);
        //        byte[] blnk;
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //            blnk = ms.ToArray();
        //        }

        //        return File(blnk, "image/*");
        //    }
        //    else
        //    {
        //        byte[] cover = GetImageFromDataBase(id);
        //        if (cover != null)
        //        {
        //            return File(cover, "image/*");
        //        }
        //        else
        //        {
        //            string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
        //            Image img = Image.FromFile(strImgPathAbsolute);
        //            byte[] blnk;
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //                blnk = ms.ToArray();
        //            }

        //            return File(blnk, "image/*");
        //        }
        //    }
        //}


        public ActionResult RetrieveImage(Int64 id)
        {
            if (id == null)
            {
                string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                Image img = Image.FromFile(strImgPathAbsolute);
                byte[] blnk;
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    blnk = ms.ToArray();
                }

                return File(blnk, "image/*");
            }
            else
            {
                DataSet db_resultset;
                var mem_img_path = string.Empty;
                var mem_sign_path = string.Empty;
                var paramStringPath = new { @ImageType = 2, @MemberID = id };  // ImageType 2 = Image string path from database
                db_resultset = ultimateReportService.GetDataWithParameter(paramStringPath, "Proc_GetMemberImage_AND_Signature");

                if (db_resultset.Tables.Count > 0 && db_resultset.Tables[0].Rows.Count > 0)
                {
                    mem_img_path = db_resultset.Tables[0].Rows[0]["MemberImagePath"].ToString();
                    mem_sign_path = db_resultset.Tables[0].Rows[0]["MemberSignPath"].ToString();
                }

                if (string.IsNullOrEmpty(mem_img_path) && string.IsNullOrEmpty(mem_sign_path))
                {
                    var paramBase64 = new { @ImageType = 1, @MemberID = id };  // ImageType 1 = BASE64 image from database
                    db_resultset = ultimateReportService.GetDataWithParameter(paramBase64, "Proc_GetMemberImage_AND_Signature");
                }

                var resultList = db_resultset.Tables[0].AsEnumerable()
                    .Select(x => new MemberViewModel
                    {
                        ImageType = x.Field<int>("ImageType"),
                        MemberImg = x.Field<byte[]>("MemberImg"),
                        ThumbImg = x.Field<byte[]>("ThumbImg"),
                        MemberImagePath = x.Field<string>("MemberImagePath"),
                        MemberSignPath = x.Field<string>("MemberSignPath"),
                    }).ToList();

                MemberViewModel model = resultList.FirstOrDefault();

                if (model != null)
                {
                    if (model.ImageType == 1)
                    {
                        byte[] imageBytes;
                        if (model.MemberImg == null || model.MemberImg.Length < 2)
                        {
                            string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                            Image img = Image.FromFile(strImgPathAbsolute);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                imageBytes = ms.ToArray();
                            }
                        }
                        else
                        {
                            imageBytes = model.MemberImg;
                        }
                        return File(imageBytes, "image/*");
                    }
                    else
                    {
                        byte[] imagePath;
                        if (string.IsNullOrEmpty(model.MemberImagePath))
                        {
                            string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                            Image img = Image.FromFile(strImgPathAbsolute);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                imagePath = ms.ToArray();
                            }
                        }
                        else
                        {
                            string path = model.MemberImagePath;
                            string dir = Server.MapPath(path);

                            try
                            {
                                imagePath = System.IO.File.ReadAllBytes(dir);
                            }
                            catch (Exception ex)
                            {
                                string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                                Image img = Image.FromFile(strImgPathAbsolute);

                                using (MemoryStream ms = new MemoryStream())
                                {
                                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    imagePath = ms.ToArray();
                                }
                            }
                        }

                        return File(imagePath, "image/*");
                    }
                }
                else
                {
                    string imagePath = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                    return File(imagePath, "image/*");
                }
            }
        }
        public byte[] GetFingerImageFromDataBase(Int64 Id)
        {
            var memberDetail = memberService.GetByMemberId(Id);
            var img = memberDetail.ThumbImg;
            //var q = from temp in  where temp.ID == Id select temp.Image;
            byte[] cover = img;
            return cover;
        }
        //public ActionResult RetrieveFingerImage(Int64 id)
        //{
        //    byte[] cover = GetFingerImageFromDataBase(id);
        //    if (cover != null)
        //    {
        //        return File(cover, "image/*");
        //    }
        //    else
        //    {
        //        string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
        //        Image img = Image.FromFile(strImgPathAbsolute);
        //        byte[] blnk;
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //            blnk = ms.ToArray();
        //        }

        //        return File(blnk, "image/*"); ;
        //    }
        //}

        public ActionResult RetrieveFingerImage(Int64 id)
        {
            if (id == null)
            {
                string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                Image img = Image.FromFile(strImgPathAbsolute);
                byte[] blnk;
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    blnk = ms.ToArray();
                }

                return File(blnk, "image/*");
            }
            else
            {
                DataSet db_resultset;
                var mem_img_path = string.Empty;
                var mem_sign_path = string.Empty;
                var paramStringPath = new { @ImageType = 2, @MemberID = id };  // ImageType 2 = Image string path from database
                db_resultset = ultimateReportService.GetDataWithParameter(paramStringPath, "Proc_GetMemberImage_AND_Signature");

                if (db_resultset.Tables.Count > 0 && db_resultset.Tables[0].Rows.Count > 0)
                {
                    mem_img_path = db_resultset.Tables[0].Rows[0]["MemberImagePath"].ToString();
                    mem_sign_path = db_resultset.Tables[0].Rows[0]["MemberSignPath"].ToString();
                }

                if (string.IsNullOrEmpty(mem_img_path) && string.IsNullOrEmpty(mem_sign_path))
                {
                    var paramBase64 = new { @ImageType = 1, @MemberID = id };  // ImageType 1 = BASE64 image from database
                    db_resultset = ultimateReportService.GetDataWithParameter(paramBase64, "Proc_GetMemberImage_AND_Signature");
                }

                var resultList = db_resultset.Tables[0].AsEnumerable()
                    .Select(x => new MemberViewModel
                    {
                        ImageType = x.Field<int>("ImageType"),
                        MemberImg = x.Field<byte[]>("MemberImg"),
                        ThumbImg = x.Field<byte[]>("ThumbImg"),
                        MemberImagePath = x.Field<string>("MemberImagePath"),
                        MemberSignPath = x.Field<string>("MemberSignPath"),
                    }).ToList();

                MemberViewModel model = resultList.FirstOrDefault();
                if (model != null)
                {
                    if (model.ImageType == 1)
                    {
                        byte[] signBytes;
                        if (model.ThumbImg == null || model.ThumbImg.Length < 2)
                        {
                            string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                            Image img = Image.FromFile(strImgPathAbsolute);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                signBytes = ms.ToArray();
                            }
                        }
                        else
                        {
                            signBytes = model.ThumbImg;
                        }
                        return File(signBytes, "image/*");
                    }
                    else
                    {
                        byte[] signPath;
                        if (string.IsNullOrEmpty(model.MemberSignPath))
                        {
                            string strImgPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                            Image img = Image.FromFile(strImgPathAbsolute);

                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                signPath = ms.ToArray();
                            }
                        }
                        else
                        {
                            string path = model.MemberSignPath;
                            string dir = Server.MapPath(path);

                            try
                            {
                                signPath = System.IO.File.ReadAllBytes(dir);
                            }
                            catch (Exception ex)
                            {
                                string strSignPathAbsolute = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                                Image img = Image.FromFile(strSignPathAbsolute);

                                using (MemoryStream ms = new MemoryStream())
                                {
                                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    signPath = ms.ToArray();
                                }
                            }

                        }

                        return File(signPath, "image/*");
                    }
                }
                else
                {
                    string imagePath = HttpContext.Server.MapPath("~/images/blank-headshot.jpg");
                    return File(imagePath, "image/*");
                }
            }
        }

        public ActionResult DeleteMember(string MemId)
        {
            var member = memberService.GetByIdLong(Convert.ToInt64(MemId));
            member.IsActive = false;
            member.InActiveDate = DateTime.Now;
            memberService.Update(member);
            return RedirectToAction("Index");
        }
        public JsonResult GetLatestMemCode(string office_id, string group_id)
        {
            string last_code = "";
            var v = memberLastCodeService.GetByOffcGroupId(Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt32(office_id), Convert.ToInt32(group_id));
            var groupCode = groupService.GetById(Convert.ToInt32(group_id)).GroupCode;
            var offcCode = officeService.GetById(Convert.ToInt32(office_id)).OfficeCode;
            if (v == null || v.LastCode == "") // if there is no voucher for this office
            {
                last_code = offcCode + groupCode + "00001";
                string new_code = offcCode + groupCode + "00002";
                var crt = new MemberLastCode();
                crt.OfficeID = Convert.ToInt32(office_id);
                crt.GroupID = Convert.ToInt32(group_id);
                crt.LastCode = new_code;
                crt.OrgID = Convert.ToInt32(LoggedInOrganizationID);
            }
            else // collect last voucher no
            {
                last_code = v.LastCode;
                string new_code = IncMemberCode(last_code.Substring(last_code.Length - 5, 5), "Add");
                var updt = new MemberLastCode();
                updt = memberLastCodeService.GetByLastCodeId(Convert.ToInt32(v.LastCodeID));
                updt.LastCode = offcCode + groupCode + new_code;
            }

            return Json(last_code, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReverseMemCode(string office_id, string group_id)
        {
            string last_code = "";
            var v = memberLastCodeService.GetByOffcGroupId(Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt32(office_id), Convert.ToInt32(group_id));
            var groupCode = groupService.GetById(Convert.ToInt32(group_id)).GroupCode;
            var offcCode = officeService.GetById(Convert.ToInt32(office_id)).OfficeCode;

            last_code = v.LastCode;
            string reverse_code = IncMemberCode(last_code.Substring(last_code.Length - 5, 5), "Sub");
            var updt = new MemberLastCode();
            updt = memberLastCodeService.GetByLastCodeId(Convert.ToInt32(v.LastCodeID));
            updt.LastCode = offcCode + groupCode + reverse_code;
            last_code = offcCode + groupCode + reverse_code;
            return Json(last_code, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ApproveAllMember()
        {
            long TotCount;
            var memberDetail = memberService.GetApprovalMember(Convert.ToInt16(LoggedInOrganizationID), SessionHelper.LoginUserOfficeID, out TotCount);
            var detail = memberDetail.ToList();
            int cnt = 0;
            foreach (var d in detail)
            {
                Member updt = new Member();
                updt = memberService.GetByIdLong(d.MemberID);
                updt.MemberStatus = "1";
                memberService.Update(updt);
                cnt++;
            }
            string result;
            if (TotCount == cnt)
                result = "Ok";
            else
                result = "Cancel";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemberProdList(string id)
        {
            try
            {
                List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
                var param = new { @OrgId = SessionHelper.LoginUserOrganizationID, @MemberID = id };
                var allProducts = accReportService.GetMemberProductListCategoryWise(param);
                List_MemberViewModel = allProducts.Tables[0].AsEnumerable()
                .Select(row => new MemberViewModel
                {
                    ProductID = row.Field<Int16>("ProductID"),
                    ProductName = row.Field<string>("ProductName")

                }).ToList();

                return Json(List_MemberViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        public JsonResult CheckNationalId(string nationalId)
        {
            string cnt_status = "No";
            return Json(cnt_status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckNationalIdEditMode(string nationalId, string mid)
        {
            string cnt_status = "No";
            //var mem = memberService.CheckMemberNationalIdEdit(nationalId,Convert.ToInt64(mid));
            //if (mem.ToList().Count > 0)
            //{
            //    cnt_status = "Yes";
            //}
            return Json(cnt_status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckPhoneNoEditMode(string PhoneNo, string mid)
        {
            string cnt_status = "No";
            //var mem = memberService.CheckMemberPhoneNoEdit(PhoneNo, Convert.ToInt64(mid));
            //if (mem.ToList().Count > 0)
            //{
            //    cnt_status = "Yes";
            //}
            return Json(cnt_status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckPhoneNo(string PhoneNo)
        {
            string cnt_status = "No";
            //var mem = memberService.CheckMemberPhoneNo(PhoneNo);
            //if (mem.ToList().Count > 0)
            //{
            //    cnt_status = "Yes";
            //}
            return Json(cnt_status, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Events
        public ActionResult Index()
        {
            //var model = new MemberViewModel();
            //var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            //var allProducts = accReportService.GetLastInitialDate(param);
            //var detail = allProducts.ToString();
            //if (!IsDayInitiated)
            //{
            //    if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
            //    {
            //        model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
            //    }
            //}
            //else
            //{
            //    model.JoinDate = TransactionDate;
            //}

            //ViewData["LoggedInOrg"] = SessionHelper.LoginUserOrganizationID;

            //return View();

            var model = new MemberViewModel();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;
            var businessDate = SessionHelper.TransactionDate.Year;

            // 01 - Jan - 0001

            ViewData["IsDayOpen"] = businessDate == 1 ? false : true;


            ViewData["LoggedInOrg"] = SessionHelper.LoginUserOrganizationID;
            ViewData["RoleId"] = SessionHelper.LoginUserRoleId;
            return View();
        }
        public JsonResult GetNIDList()
        {
            if (dtINIDList == null)
            {
                StringBuilder sb = new StringBuilder();
                var param = new { AndCondition = sb.ToString() };
                var MessageList = ultimateReportService.GetDataWithParameter(param, "GET_NIDType");
                dtINIDList = new DataTable();
                dtINIDList = MessageList.Tables[0];
            }

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = dtINIDList.AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                surveyIdentityTypeId = row.Field<int>("NIDStatusID"),
                IdentityName = row.Field<string>("NIDText")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.surveyIdentityTypeId.ToString(),
                Text = x.IdentityName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();
            List_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOnlyMemberInfo()
        {
            return View();
        }
        // GET: Member/Eligible/5
        public ActionResult Eligible()
        {
            //GetSuccessMessageResult();
            return View();
        }
        //========================= Admin Member Edit : Mostak =================================//

        public ActionResult AdminMemberEdit()  //Index Page for Admin
        {
            var model = new MemberViewModel();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;

            ViewData["LoggedInOrg"] = SessionHelper.LoginUserOrganizationID;
            ViewData["RoleId"] = SessionHelper.LoginUserRoleId;
            return View();

        }


        public ActionResult AdminMemberEditSumbit(int id)  //Edit viewPage for Admin
        {
            var member = memberService.GetById(id);
            var memberModel = Mapper.Map<Member, MemberViewModel>(member);
            //            memberModel.OfficeID = 2;
            MapDropDownList(memberModel);
            var allCenter = centerService.GetByOfficeId(memberModel.OfficeID, Convert.ToInt16(LoggedInOrganizationID));
            var viewCenter = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + "-" + x.CenterName.ToString()
            });
            memberModel.CenterList = viewCenter;
            var allGroup = groupService.GetByOfficeId(memberModel.OfficeID);
            var viewGroup = allGroup.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GroupID.ToString(),
                Text = x.GroupCode.ToString()
            });
            memberModel.GroupList = viewGroup;
            memberModel.GroupID = member.GroupID;
            memberModel.Gender = member.Gender;
            memberModel.Location = member.Location;
            memberModel.PlaceOfBirth = member.PlaceOfBirth;
            memberModel.ServerCurrentDate = DateTime.Now;
            memberModel.ExpireDate = member.ExpireDate;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;


            var OrgInfo = organizationService.GetById((int)SessionHelper.LoginUserOrganizationID);
            //var v = OrgInfo.MemberAge;
            var MemberAge = 60;

            if (OrgInfo.MemberAge == null)
            {

            }
            else
            {
                MemberAge = (int)OrgInfo.MemberAge;
            }
            ViewData["MemberAge"] = MemberAge;


            var imgentityparam = new { @MemberID = memberModel.MemberID, @OfficeId = memberModel.OfficeID };
            var Entity = ultimateReportService.GetDataWithParameter(imgentityparam, "Proc_GetVerificationStatus");

            var NIDStatusID = 0;
            var NIDStatusIDst = "";
            var NIDAddress = "";
            var NIDVerificationId = "";
            if (Entity.Tables[0].Rows.Count > 0)
            {
                NIDStatusIDst = Entity.Tables[0].Rows[0]["NIDStatusID"].ToString();
                NIDAddress = Entity.Tables[0].Rows[0]["NIDAddress"].ToString();
                NIDVerificationId = Entity.Tables[0].Rows[0]["NIDVerificationId"].ToString();
            }

            if (NIDStatusIDst != null)
            {
                if (NIDStatusIDst != "")
                {

                    NIDStatusID = Convert.ToInt32(NIDStatusIDst);
                }
                else
                {
                    NIDStatusID = Convert.ToInt32("1");
                }
            }

            if (NIDVerificationId != null)
            {
                if (NIDVerificationId != "")
                {
                    memberModel.NIDVerificationId = null;
                }
            }

            memberModel.NIDCheckStatusId = NIDStatusID;
            memberModel.txtAddress = NIDAddress;


            ViewData["LoggedInOrgID"] = LoggedInOrganizationID;

            return View(memberModel);
        }


        [HttpPost]
        public ActionResult AdminMemberEditSumbit(MemberEditViewModel model, string base64image, string base64imageFingerThumb)
        {
            try
            {
                var entity = memberService.GetByIdLong(model.MemberID);

                if (!ModelState.IsValid)
                    return GetErrorMessageResult();

                if (entity.OfficeID != LoginUserOfficeID)
                {
                    return GetErrorMessageResult("Invalid Office...........");
                }
                entity.CenterID = model.CenterID;
                entity.GroupID = model.GroupID;
                entity.FirstName = model.FirstName;
                entity.MiddleName = model.MiddleName;
                entity.LastName = model.LastName;
                entity.AddressLine1 = model.AddressLine1;
                entity.AddressLine2 = model.AddressLine2;
                entity.RefereeName = model.RefereeName;
                entity.BirthDate = model.BirthDate;
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                var allProducts = accReportService.GetLastInitialDate(param);

                entity.NationalID = model.NationalID;
                entity.HomeType = model.HomeType;
                entity.FamilyContactNo = model.FamilyContactNo;

                entity.Email = model.Email;
                entity.PhoneNo = model.PhoneNo;
                entity.MaritalStatus = model.MaritalStatus;
                entity.MemCategory = model.MemCategory;
                entity.EconomicActivity = model.EconomicActivity;

                // NEW ADD KHALID
                entity.AddressLine1 = model.AddressLine1;
                entity.AddressLine2 = model.AddressLine2;
                entity.CountryID = model.CountryID;
                entity.DivisionCode = model.DivisionCode;
                entity.DistrictCode = model.DistrictCode;
                entity.UpozillaCode = model.UpozillaCode;
                entity.UnionCode = model.UnionCode;
                entity.VillageCode = model.VillageCode;
                entity.ZipCode = model.ZipCode;
                entity.Education = model.Education;


                entity.IsAnyFS = model.IsAnyFS;
                entity.FServiceName = model.FServiceName;
                entity.FinServiceChoiceId = model.FinServiceChoiceId;
                entity.TransactionChoiceId = model.TransactionChoiceId;

                entity.BanglaName = model.BanglaName;
                entity.PerAddressLine1 = model.PerAddressLine1;
                entity.PerAddressLine2 = model.PerAddressLine2;
                entity.PerCountryID = model.PerCountryID;
                entity.PerDivisionCode = model.PerDivisionCode;
                entity.PerDistrictCode = model.PerDistrictCode;
                entity.PerUpozillaCode = model.PerUpozillaCode;
                entity.PerUnionCode = model.PerUnionCode;
                entity.PerVillageCode = model.PerVillageCode;
                entity.PerZipCode = model.PerZipCode;
                entity.FatherNameBN = model.FatherNameBN;
                entity.MotherNameBN = model.MotherNameBN;
                entity.IdentTypeID = model.IdentTypeID;
                entity.ExpireDate = model.ExpireDate;
                entity.ProvidedByCountryID = model.ProvidedByCountryID;
                entity.SpouseName = model.SpouseName;
                entity.SpouseNameBN = model.SpouseNameBN;
                entity.TIN = model.TIN;
                entity.TaxAmount = model.TaxAmount;
                entity.OtherIdNo = model.OtherIdNo;
                entity.CoApplicantName = model.CoApplicantName;
                entity.MemberStatus = model.MemberStatus;
                entity.JoinDate = model.JoinDate;
                entity.Gender = model.Gender;
                entity.FamilyMember = model.FamilyMember;
                entity.Cityzenship = model.Cityzenship;
                entity.ExpireDate = model.ExpireDate;
                // END

                if (model.DivisionCode != null)
                {
                    if (model.DivisionCode != "0")
                    {
                        entity.DivisionCode = model.DivisionCode;
                    }
                    else
                    {
                        entity.DivisionCode = "";
                    }
                }
                if (model.DistrictCode != null)
                {
                    if (model.DistrictCode != "0")
                    {
                        entity.DistrictCode = model.DistrictCode;
                    }
                    else
                    {
                        entity.DistrictCode = "";
                    }
                }
                if (model.UpozillaCode != null)
                {
                    if (model.UpozillaCode != "0")
                    {
                        entity.UpozillaCode = model.UpozillaCode;
                    }
                    else
                    {
                        entity.UpozillaCode = "";
                    }
                }
                if (model.UnionCode != null)
                {
                    if (model.UnionCode != "0")
                    {
                        entity.UnionCode = model.UnionCode;
                    }
                    else
                    {
                        entity.UnionCode = "";
                    }
                }
                if (model.VillageCode != null)
                {
                    if (model.VillageCode != "0")
                    {
                        entity.VillageCode = model.VillageCode;
                    }
                    else
                    {
                        entity.VillageCode = "";
                    }
                }


                if (model.PerDivisionCode != null)
                {
                    if (model.PerDivisionCode != "0")
                    {
                        entity.PerDivisionCode = model.PerDivisionCode;
                    }
                    else
                    {
                        entity.PerDivisionCode = "";
                    }
                }
                if (model.PerDistrictCode != null)
                {
                    if (model.PerDistrictCode != "0")
                    {
                        entity.PerDistrictCode = model.PerDistrictCode;
                    }
                    else
                    {
                        entity.PerDistrictCode = "";
                    }
                }
                if (model.PerUpozillaCode != null)
                {
                    if (model.PerUpozillaCode != "0")
                    {
                        entity.PerUpozillaCode = model.PerUpozillaCode;
                    }
                    else
                    {
                        entity.PerUpozillaCode = "";
                    }
                }
                if (model.PerUnionCode != null)
                {
                    if (model.PerUnionCode != "0")
                    {
                        entity.PerUnionCode = model.PerUnionCode;
                    }
                    else
                    {
                        entity.PerUnionCode = "";
                    }
                }
                if (model.PerVillageCode != null)
                {
                    if (model.PerVillageCode != "0")
                    {
                        entity.PerVillageCode = model.PerVillageCode;
                    }
                    else
                    {
                        entity.PerVillageCode = "";
                    }
                }

                if (model.PlaceOfBirth != null)
                {
                    entity.PlaceOfBirth = model.PlaceOfBirth;

                }
                if (model.TotalWealth != null)
                {
                    entity.TotalWealth = model.TotalWealth;

                }
                if (model.MotherName != null)
                {
                    entity.MotherName = model.MotherName;

                }
                if (model.FatherName != null)
                {
                    entity.FatherName = model.FatherName;

                }
                if (model.NationalID != null)
                {
                    entity.NationalID = model.NationalID;
                }
                if (model.NationalID == null)
                {
                    entity.NationalID = "0";
                }

                if (model.SmartCard != null)
                {
                    entity.SmartCard = model.SmartCard;
                }
                if (model.SmartCard == null)
                {
                    entity.SmartCard = "0";
                }
                if (model.PhoneNo != null)
                {
                    entity.PhoneNo = model.PhoneNo;

                }
                //if (entity.PhoneNo == null)
                //{
                //    return GetErrorMessageResult("PhoneNo cann't be null");
                //}


                var vNatio = "";
                var vSmat = "";
                var vphone = "";
                var OtherID = "";
                var vMemberStatus = "";
                int? vNIDCheckStatusId = 0;
                if (entity.NationalID == null)
                {
                    vNatio = "0";
                }
                else
                {
                    vNatio = entity.NationalID;
                }
                if (entity.SmartCard == null)
                {
                    vSmat = "0";
                }
                else
                {
                    vSmat = entity.SmartCard;

                }
                if (entity.PhoneNo == null)
                {
                    vphone = "0";
                }
                else
                {
                    vphone = entity.PhoneNo;
                }

                if (model.NIDCheckStatusId == null)
                {
                    vNIDCheckStatusId = 1;
                }
                else
                {
                    vNIDCheckStatusId = model.NIDCheckStatusId;
                }
                var CheckDupli = new { @NationalID = vNatio, @SmartCard = vSmat, @PhoneNo = vphone, @Qtype = 2, @MemberCode = entity.MemberCode };
                var CheckMemberDupli = ultimateReportService.CheckDuplicateMember(CheckDupli);

                if (CheckMemberDupli.Tables.Count > 0)
                {
                    if (CheckMemberDupli.Tables[0].Rows.Count > 0)
                    {
                        return GetErrorMessageResult(CheckMemberDupli.Tables[0].Rows[0]["ErrorName"].ToString());
                    }
                }

                if (Session["CapturedImage"] == null && (model.ImgFile != null && model.ImgFile.ContentLength > 0))
                {
                    entity.MemberImg = null;
                }
                else if (model.ImgFile == null && (base64image != null && base64image != ""))
                {
                    entity.MemberImg = null;
                }

                if (model.ThumbImgFile != null && model.ThumbImgFile.ContentLength > 0)
                {
                    entity.ThumbImg = null;
                }
                else if (model.ThumbImgFile == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                {
                    entity.ThumbImg = null;
                }


                var paramCenterUpdate = new { OfficeID = LoginUserOfficeID, CenterID = entity.CenterID, MemberID = entity.MemberID, CreateUser = LoggedInEmployeeID };
                ultimateReportService.UpdateCenterIDInAllRelatedTable(paramCenterUpdate);


                memberService.Update(entity);


                //==========================================================================================//
                // Edit     : Mostak Ahammed                                                                //
                // Date     : 21 Aug 2023                                                                   //
                // Desc     : Image will save in project directory and path will save in the database       //
                //==========================================================================================//

                var imgentityparam = new
                {
                    @OfficeID = SessionHelper.LoginUserOfficeID,
                    @CenterID = model.CenterID,
                    @MemberID = model.MemberID,
                    @MemberImagePath = "",
                    @MemberSignPath = "",
                    @QType = 3 // Return Image data for delete existing image to update new image
                };
                var imgEntity = ultimateReportService.GetDataWithParameter(imgentityparam, "Proc_SaveEdit_MemberImage");

                var existing_img = "";
                var existing_sign = "";

                if (imgEntity.Tables[0].Rows.Count > 0)
                {
                    existing_img = imgEntity.Tables[0].Rows[0]["MemberImagePath"].ToString();
                    existing_sign = imgEntity.Tables[0].Rows[0]["MemberSignPath"].ToString();
                }

                var vMemberImg = "";
                var vMemberSign = "";
                //Path Directory in project folder
                string memberImageDirectory = Server.MapPath("~/Uploads/Member/Image/");
                string memberSignDirectory = Server.MapPath("~/Uploads/Member/Signature/");
                string memberImagePath = "~/Uploads/Member/Image/";
                string memberSignPath = "~/Uploads/Member/Signature/";

                //If directory not exists then create new directory
                if (!Directory.Exists(memberImageDirectory))
                {
                    Directory.CreateDirectory(memberImageDirectory);
                }
                if (!Directory.Exists(memberSignDirectory))
                {
                    Directory.CreateDirectory(memberSignDirectory);
                }

                //If user select the image
                if (Session["CapturedImage"] == null && (model.ImgFile != null && model.ImgFile.ContentLength > 0))
                {
                    //Delete old image if exists
                    string oldImagePath = Server.MapPath(existing_img);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                    if (size > 100)
                    {
                        memberService.CheckImageSize();
                        Response.StatusCode = 400;
                        throw new System.InvalidOperationException("Image File is Bigger than 100 KB.");
                    }

                    //var shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 10);
                    string fileName = $"img_{model.MemberID}" + Path.GetExtension(model.ImgFile.FileName);
                    string imagePath = Path.Combine(memberImageDirectory, fileName);
                    model.ImgFile.SaveAs(imagePath);

                    vMemberImg = memberImagePath + fileName;
                    Session["CapturedImage"] = null;
                }
                // MemberImage upload as Captured
                else if (model.ImgFile == null && (base64image != null && base64image != ""))
                {
                    //Delete old image if exists
                    string oldImagePath = Server.MapPath(existing_img);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    var main_baseString = base64image.Substring(22);
                    byte[] imageByte = Convert.FromBase64String(main_baseString);

                    var path = Server.MapPath("~/Uploads/Member/Image/");
                    var capturedPath = "~/Uploads/Member/Image/";

                    //var shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 10);
                    var capturedFileName = $"img_{model.MemberID}" + ".jpg";

                    var fileLocation = Path.Combine(path, capturedFileName);

                    System.IO.File.WriteAllBytes(fileLocation, imageByte);
                    vMemberImg = capturedPath + capturedFileName;
                }

                if (model.ThumbImgFile != null && model.ThumbImgFile.ContentLength > 0)
                {
                    string oldSignPath = Server.MapPath(existing_sign);
                    if (System.IO.File.Exists(oldSignPath))
                    {
                        System.IO.File.Delete(oldSignPath);
                    }


                    decimal size = Math.Round(((decimal)model.ThumbImgFile.ContentLength / (decimal)1024), 2);
                    if (size > 100)
                    {
                        memberService.CheckImageSize();
                        Response.StatusCode = 400;
                        throw new System.InvalidOperationException("Image File is Bigger than 100 KB.");
                    }
                    //var shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 10);
                    string fileName = $"sign_{model.MemberID}" + Path.GetExtension(model.ThumbImgFile.FileName);
                    string thumbImagePath = Path.Combine(memberSignDirectory, fileName);
                    model.ThumbImgFile.SaveAs(thumbImagePath);

                    vMemberSign = memberSignPath + fileName;
                    Session["CapturedImage"] = null;
                }
                // Member Signature upload as Capture
                else if (model.ThumbImgFile == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                {
                    string oldSignPath = Server.MapPath(existing_sign);
                    if (System.IO.File.Exists(oldSignPath))
                    {
                        System.IO.File.Delete(oldSignPath);
                    }

                    var main_baseString = base64imageFingerThumb.Substring(22);
                    byte[] imageByte = Convert.FromBase64String(main_baseString);

                    var path = Server.MapPath("~/Uploads/Member/Signature/");
                    var capturedPath = "~/Uploads/Member/Signature/";

                    //var shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 10);
                    var capturedFileName = $"sign_{model.MemberID}" + ".jpg";
                    var fileLocation = Path.Combine(path, capturedFileName);

                    System.IO.File.WriteAllBytes(fileLocation, imageByte);
                    vMemberSign = capturedPath + capturedFileName;
                }


                //MemberImage Edit
                var imgSaveParam = new
                {
                    @OfficeID = SessionHelper.LoginUserOfficeID,
                    @CenterID = model.CenterID,
                    @MemberID = model.MemberID,
                    @MemberImagePath = vMemberImg,
                    @MemberSignPath = vMemberSign,
                    @QType = 2 // Edit Image QType
                };

                ultimateReportService.SaveEditMemberImage(imgSaveParam);

                //======================================= Mostak End ========================================//


                // ----------------------------------------------------
                var param3 = new
                {
                    @NIDVerificationId = model.NIDVerificationId,
                    @OfficeID = LoginUserOfficeID,
                    @MemberID = model.MemberID,
                    @NationalID = vNatio,
                    @SmartCard = vSmat,
                    @OtherIdNo = OtherID,
                    @NIDAddress = model.txtAddress == null ? "" : model.txtAddress,
                    @NIDStatusID = vNIDCheckStatusId,
                    @CreateUser = LoggedInEmployeeID,
                    @UpdateUser = LoggedInEmployeeID,
                    @IsAdmin = true,
                };
                var MemberIDValidate3 = ultimateReportService.GenerateNIDStatusForAdminOnly(param3);
                return GetSuccessMessageResult();


            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        //========================= Admin Member Edit End =================================//
        public ActionResult DormantMember()
        {
            return View();
        }


        // DormantMemberViewModel

        public JsonResult GetDormantMemberList(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, string typeFilterColumn)
        {
            try
            {
                List<DormantMemberViewModel> List_EmployeeViewModel = new List<DormantMemberViewModel>();
                var param = new
                {
                    OfficeID = SessionHelper.LoginUserOfficeID
                };
                var DataList = new DataSet();
                DataList = ultimateReportService.GetDataWithParameter(param, "sp_Get_DormantList");

                List_EmployeeViewModel = DataList.Tables[0].AsEnumerable()
                .Select(row => new DormantMemberViewModel
                {

                    MemberCode = row.Field<string>("MemberCode"),
                    MemberName = row.Field<string>("MemberName"),
                    FatherName = row.Field<string>("FatherName"),
                    SamityCode = row.Field<string>("SamityCode"),
                    SamityName = row.Field<string>("SamityName"),
                    SavingsBalance = row.Field<decimal>("SavingsBalance"),
                    LastTransactionDate = row.Field<string>("LastTransactionDate"),
                    DormantDate = row.Field<string>("DormantDate")


                }).ToList();

                var currentPageRecords = List_EmployeeViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords.OrderBy(x => x.MemberCode).ToList(), TotalRecordCount = List_EmployeeViewModel.LongCount(), JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// list 
        public JsonResult GetMembersCount(int Center_Id)
        {

            try
            {
                // var currentMembers = memberService.GetMany(x => x.IsActive == true && x.CenterID == Center_Id).Count();

                var param = new
                {
                    OfficeID = SessionHelper.LoginUserOfficeID,
                    Center_Id = Center_Id
                };

                var DataList = new DataSet();
                DataList = ultimateReportService.GetDataWithParameter(param, "GetMembersCount");

                var count = DataList.Tables[0].Rows[0]["MembersCount"].ToString();


                return Json(new { Count = count }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }

        }


        public JsonResult GetMembersByNationalID(string NationlId)
        {

            try
            {

                var param = new
                {
                    OfficeID = SessionHelper.LoginUserOfficeID,
                    NationlId = NationlId
                };

                var DataList = new DataSet();
                DataList = ultimateReportService.GetDataWithParameter(param, "GetMembersByNationalID");

                var count = DataList.Tables[0].Rows[0]["MembersCount"].ToString();
                var OfficeCode = DataList.Tables[0].Rows[0]["OfficeCode"].ToString();
                var MemberStatus = DataList.Tables[0].Rows[0]["MemberStatus"].ToString();

                return Json(new { Count = count, OfficeCode = OfficeCode, MemberStatus = MemberStatus }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }

        }


        public JsonResult GetMembersBySmartCard(string SmartCardId)
        {
            try
            {
                var param = new
                {
                    OfficeID = SessionHelper.LoginUserOfficeID,
                    SmartCardId = SmartCardId
                };

                var DataList = new DataSet();
                DataList = ultimateReportService.GetDataWithParameter(param, "GetMembersBySmartCardId");

                var count = DataList.Tables[0].Rows[0]["MembersCount"].ToString();
                var OfficeCode = DataList.Tables[0].Rows[0]["OfficeCode"].ToString();
                var MemberStatus = DataList.Tables[0].Rows[0]["MemberStatus"].ToString();

                return Json(new { Count = count, OfficeCode = OfficeCode, MemberStatus = MemberStatus }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }

        }

        // GET: Member/Create
        public ActionResult Create()
        {
            var model = new MemberViewModel();
            MapDropDownList(model);
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }
            model.ServerCurrentDate = DateTime.Now;
            var blnk_items = new List<SelectListItem>();
            model.CenterList = blnk_items;
            model.GroupList = blnk_items;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;

            var OrgInfo = organizationService.GetById((int)SessionHelper.LoginUserOrganizationID);
            //var v = OrgInfo.MemberAge;
            var MemberAge = 60;

            if (OrgInfo.MemberAge == null)
            {

            }
            else
            {
                MemberAge = (int)OrgInfo.MemberAge;
            }
            ViewData["MemberAge"] = MemberAge;

            ViewData["LoggedInOrg"] = SessionHelper.LoginUserOrganizationID;
            ViewBag.OrgId = LoggedInOrganizationID;
            return View(model);
        }
        public JsonResult CheckSmartCardId(string SmartCard)
        {
            string cnt_status = "No";
            //var mem = memberService.CheckMemberNationalId(SmartCard);
            //if (mem.ToList().Count > 0)
            //{
            //    cnt_status = "Yes";
            //}
            return Json(cnt_status, JsonRequestBehavior.AllowGet);
        }
        // POST: Member/Create
        [HttpPost]
        public JsonResult Create(MemberViewModel model, string MemCode, string base64image, string base64imageFingerThumb)
        {
            try
            {
                var entity = Mapper.Map<MemberViewModel, Member>(model);
                if (ModelState.IsValid)
                {
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                    var errors = memberService.IsValidMember(entity);

                    //if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != "") && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                    //{
                    //    var t = base64image.Substring(22);  // remove data:image/png;base64,
                    //    byte[] bytes = Convert.FromBase64String(t);
                    //    Image image;
                    //    using (MemoryStream ms = new MemoryStream(bytes))
                    //    {
                    //        image = Image.FromStream(ms);
                    //    }
                    //    var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                    //    entity.MemberImg = bytes;

                    //    var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                    //    byte[] bytesthumb = Convert.FromBase64String(tthumb);
                    //    Image imagethumb;
                    //    using (MemoryStream msa = new MemoryStream(bytesthumb))
                    //    {
                    //        imagethumb = Image.FromStream(msa);
                    //    }
                    //    var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                    //    entity.ThumbImg = bytesthumb;
                    //}
                    //else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != ""))
                    //{
                    //    var t = base64image.Substring(22);  // remove data:image/png;base64,
                    //    byte[] bytes = Convert.FromBase64String(t);
                    //    Image image;
                    //    using (MemoryStream ms = new MemoryStream(bytes))
                    //    {
                    //        image = Image.FromStream(ms);
                    //    }
                    //    var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                    //    entity.MemberImg = bytes;
                    //}
                    //else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                    //{
                    //    var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                    //    byte[] bytesthumb = Convert.FromBase64String(tthumb);
                    //    Image imagethumb;
                    //    using (MemoryStream msa = new MemoryStream(bytesthumb))
                    //    {
                    //        imagethumb = Image.FromStream(msa);
                    //    }
                    //    var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                    //    entity.ThumbImg = bytesthumb;
                    //}
                    //else if (model.ImgFile == null && Session["CapturedImage"] != null)
                    //{
                    //    var path = Server.MapPath("~/CapturedImages/");

                    //    var FileLocation = path + Session["CapturedImage"].ToString();
                    //    System.IO.File.Exists(FileLocation);
                    //    System.IO.FileInfo file = new System.IO.FileInfo(FileLocation);

                    //    using (System.Drawing.Image image = System.Drawing.Image.FromFile(file.FullName))
                    //    {
                    //        ImageConverter converter = new ImageConverter();
                    //        byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                    //        entity.MemberImg = data;


                    //        decimal size = Math.Round(((decimal)entity.MemberImg.Length / (decimal)1024), 2);
                    //        if (size > 100)
                    //        {
                    //            errors = memberService.CheckImageSize();
                    //            Response.StatusCode = 400;
                    //        }
                    //    }
                    //    Array.ForEach(Directory.GetFiles(path), System.IO.File.Delete);

                    //}
                    //else if (model.ImgFile != null)
                    //{

                    //    decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                    //    if (size > 100)
                    //    {
                    //        errors = memberService.CheckImageSize();
                    //        Response.StatusCode = 400;
                    //        throw new System.InvalidOperationException("Image File is Big than 100 KB.");
                    //    }

                    //    byte[] data = new byte[model.ImgFile.ContentLength];
                    //    if (data != null)
                    //    {
                    //        model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                    //        entity.MemberImg = data;
                    //    }
                    //}

                    //if (model.ThumbImgFile != null)
                    //{
                    //    decimal size = Math.Round(((decimal)model.ThumbImgFile.ContentLength / (decimal)1024), 2);
                    //    if (size > 100)
                    //    {
                    //        errors = memberService.CheckImageSize();
                    //        Response.StatusCode = 400;
                    //        throw new System.InvalidOperationException("Image File is Big than 100 KB.");
                    //    }

                    //    byte[] data = new byte[model.ThumbImgFile.ContentLength];
                    //    if (data != null)
                    //    {
                    //        model.ThumbImgFile.InputStream.Read(data, 0, model.ThumbImgFile.ContentLength);
                    //        entity.ThumbImg = data;
                    //    }
                    //}


                    if (model.DivisionCode != null)
                    {
                        if (model.DivisionCode != "0")
                        {
                            entity.DivisionCode = model.DivisionCode;
                        }
                        else
                        {
                            entity.DivisionCode = "";
                        }
                    }
                    if (model.DistrictCode != null)
                    {
                        if (model.DistrictCode != "0")
                        {
                            entity.DistrictCode = model.DistrictCode;
                        }
                        else
                        {
                            entity.DistrictCode = "";
                        }
                    }
                    if (model.UpozillaCode != null)
                    {
                        if (model.UpozillaCode != "0")
                        {
                            entity.UpozillaCode = model.UpozillaCode;
                        }
                        else
                        {
                            entity.UpozillaCode = "";
                        }
                    }
                    if (model.UnionCode != null)
                    {
                        if (model.UnionCode != "0")
                        {
                            entity.UnionCode = model.UnionCode;
                        }
                        else
                        {
                            entity.UnionCode = "";
                        }
                    }
                    if (model.VillageCode != null)
                    {
                        if (model.VillageCode != "0")
                        {
                            entity.VillageCode = model.VillageCode;
                        }
                        else
                        {
                            entity.VillageCode = "";
                        }
                    }
                    if (model.PlaceOfBirth != null)
                    {
                        entity.PlaceOfBirth = model.PlaceOfBirth;

                    }
                    if (model.TotalWealth != null)
                    {
                        entity.TotalWealth = model.TotalWealth;

                    }
                    if (model.MotherName != null)
                    {
                        entity.MotherName = model.MotherName;

                    }
                    if (model.ExpireDate != null)
                    {
                        entity.ExpireDate = model.ExpireDate;

                    }
                    if (model.FatherName != null)
                    {
                        entity.FatherName = model.FatherName;

                    }
                    if (model.NationalID != null)
                    {
                        entity.NationalID = model.NationalID;

                        if (LoggedInOrganizationID == 218)
                        {
                            if (entity.NationalID.Length != 13)
                            {
                                return GetErrorMessageResult("NationalID  No cann't be less than 13 digits");
                            }
                        }
                        else if (LoggedInOrganizationID != 150)
                        {
                            if (entity.NationalID.Length != 17)
                            {
                                return GetErrorMessageResult("NationalID  No cann't be less than 17 digits");
                            }
                        }
                    }

                    //if (model.NationalID != null)
                    //{
                    //    entity.NationalID = model.NationalID;
                    //    if (LoggedInOrganizationID != 150)
                    //    {
                    //        if (entity.NationalID.Length != 17)
                    //        {
                    //            return GetErrorMessageResult("NationalID  No cann't be less than 17 digits");
                    //        }
                    //    }
                    //}
                    if (model.NationalID == null)
                    {
                        entity.NationalID = "0";
                    }

                    if (model.SmartCard != null)
                    {
                        entity.SmartCard = model.SmartCard;
                        if (LoggedInOrganizationID != 150)
                        {
                            if (entity.SmartCard.Length != 10)
                            {
                                return GetErrorMessageResult("SmartCard No cann't be less than 10 digits");
                            }
                        }

                    }

                    if (model.SmartCard == null)
                    {
                        entity.SmartCard = "0";
                    }

                    if (model.PhoneNo != null)
                    {
                        entity.PhoneNo = model.PhoneNo;

                    }
                    //if (entity.PhoneNo == null)
                    //{
                    //    return GetErrorMessageResult("PhoneNo cann't be null");
                    //}

                    if (LoggedInOrganizationID == 150)
                        entity.GroupID = 1;

                    entity.IsActive = true;
                    entity.MemberStatus = "0";
                    entity.PwdStatus = "D";
                    entity.MemberType = 1;

                    var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                    var allProducts = accReportService.GetLastInitialDate(param);

                    var detail = allProducts.ToString();

                    if (!IsDayInitiated)
                    {
                        if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                        {
                            entity.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                        }
                    }
                    else
                    {
                        entity.JoinDate = TransactionDate;
                    }
                    //var vNatio = "";
                    //var vSmat = "";
                    //var vphone = "";
                    var vNatio = "";
                    var vSmat = "";
                    var vphone = "";
                    var OtherID = "";
                    var vMemberStatus = "";
                    int? vNIDCheckStatusId = 0;
                    if (entity.NationalID == null)
                    {
                        vNatio = "0";
                    }
                    else
                    {
                        vNatio = entity.NationalID;
                    }
                    if (entity.SmartCard == null)
                    {
                        vSmat = "0";
                    }
                    else
                    {
                        vSmat = entity.SmartCard;

                    }
                    if (entity.PhoneNo == null)
                    {
                        vphone = "0";
                    }
                    else
                    {
                        vphone = entity.PhoneNo;
                    }
                    if (model.NIDCheckStatusId == null)
                    {
                        vNIDCheckStatusId = 1;
                    }
                    else
                    {
                        vNIDCheckStatusId = model.NIDCheckStatusId;
                    }
                    var CheckDupli = new { @NationalID = vNatio, @SmartCard = vSmat, @PhoneNo = vphone, @Qtype = 1, @MemberCode = "0" };
                    var CheckMemberDupli = ultimateReportService.CheckDuplicateMember(CheckDupli);

                    if (CheckMemberDupli.Tables.Count > 0)
                    {
                        if (CheckMemberDupli.Tables[0].Rows.Count > 0)
                        {
                            return GetErrorMessageResult(CheckMemberDupli.Tables[0].Rows[0]["ErrorName"].ToString());
                        }
                    }
                    //if (CheckMemberDupli.Tables[0].Rows.Count > 0)
                    //{
                    //    var message = CheckMemberDupli.Tables[0].Rows[0]["ErrorName"].ToString();
                    //    return GetDuplicateErrorMessageResult(message);
                    //}

                    DataSet LoanInstallMent;
                    var param1 = new { @OfficeID = LoginUserOfficeID };
                    var param2 = new { @OfficeID = LoginUserOfficeID, @CenterID = entity.CenterID };
                    if (LoggedInOrganizationID == 126)
                    {
                        LoanInstallMent = ultimateReportService.GenerateMemberLastCodeMemberSSS(param2);
                        entity.MemberCode = LoanInstallMent.Tables[0].Rows[0]["LastCode"].ToString();
                    }
                    else
                    {
                        LoanInstallMent = ultimateReportService.GenerateMemberLastCodeMember(param1);
                        entity.MemberCode = LoanInstallMent.Tables[0].Rows[0]["LastCode"].ToString();
                    }
                    entity.MemberImg = null;
                    entity.ThumbImg = null;
                    var vMemID = memberService.Create(entity);
                    var ent = new { MemberID = entity.MemberID, MemberCode = entity.MemberCode };

                    //==========================================================================================//
                    // Edit     : Mostak Ahammed                                                                //
                    // Date     : 25 Apr 2024                                                                   //
                    // Desc     : Image will save in project directory and path will save in the database       //
                    //==========================================================================================//

                    var vMemberImg = "";
                    var vMemberSign = "";
                    //Path Directory in project folder
                    string memberImageDirectory = Server.MapPath("~/Uploads/Member/Image/");
                    string memberSignDirectory = Server.MapPath("~/Uploads/Member/Signature/");
                    string memberImagePath = "~/Uploads/Member/Image/";
                    string memberSignPath = "~/Uploads/Member/Signature/";

                    //If directory not exists then create new directory
                    if (!Directory.Exists(memberImageDirectory))
                    {
                        Directory.CreateDirectory(memberImageDirectory);
                    }
                    if (!Directory.Exists(memberSignDirectory))
                    {
                        Directory.CreateDirectory(memberSignDirectory);
                    }

                    // MemberImage upload as file
                    if (Session["CapturedImage"] == null && (model.ImgFile != null && model.ImgFile.ContentLength > 0))
                    {
                        decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                        if (size > 100)
                        {
                            errors = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                            throw new System.InvalidOperationException("Image File is Bigger than 100 KB.");
                        }

                        //var shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 10);
                        string fileName = $"img_{ent.MemberID}" + Path.GetExtension(model.ImgFile.FileName);
                        string imagePath = Path.Combine(memberImageDirectory, fileName);
                        model.ImgFile.SaveAs(imagePath);

                        vMemberImg = memberImagePath + fileName;

                    }
                    // MemberImage upload as Captured
                    else if (model.ImgFile == null && (base64image != null && base64image != ""))
                    {
                        var main_baseString = base64image.Substring(22);
                        byte[] imageByte = Convert.FromBase64String(main_baseString);

                        var path = Server.MapPath("~/Uploads/Member/Image/");
                        var capturedPath = "~/Uploads/Member/Image/";

                        var shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 10);
                        var capturedFileName = $"img_{ent.MemberID}" + ".jpg";

                        var fileLocation = Path.Combine(path, capturedFileName);

                        System.IO.File.WriteAllBytes(fileLocation, imageByte);
                        vMemberImg = capturedPath + capturedFileName;
                    }
                    // Member Signature upload as file
                    if (model.ThumbImgFile != null && model.ThumbImgFile.ContentLength > 0)
                    {
                        decimal size = Math.Round(((decimal)model.ThumbImgFile.ContentLength / (decimal)1024), 2);
                        if (size > 100)
                        {
                            errors = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                            throw new System.InvalidOperationException("Image File is Bigger than 100 KB.");
                        }
                        //var shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 10);
                        string fileName = $"sign_{ent.MemberID}" + Path.GetExtension(model.ThumbImgFile.FileName);
                        string thumbImagePath = Path.Combine(memberSignDirectory, fileName);
                        model.ThumbImgFile.SaveAs(thumbImagePath);

                        vMemberSign = memberSignPath + fileName;
                    }
                    // Member Signature upload as Capture
                    else if (model.ThumbImgFile == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                    {
                        var main_baseString = base64imageFingerThumb.Substring(22);
                        byte[] imageByte = Convert.FromBase64String(main_baseString);

                        var path = Server.MapPath("~/Uploads/Member/Signature/");
                        var capturedPath = "~/Uploads/Member/Signature/";

                        //var shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 10);
                        var capturedFileName = $"sign_{ent.MemberID}" + ".jpg";
                        var fileLocation = Path.Combine(path, capturedFileName);

                        System.IO.File.WriteAllBytes(fileLocation, imageByte);
                        vMemberSign = capturedPath + capturedFileName;
                    }

                    //MemberImage Save
                    var imgSaveParam = new
                    {
                        @OfficeID = SessionHelper.LoginUserOfficeID,
                        @CenterID = model.CenterID,
                        @MemberID = vMemID.MemberID,
                        @MemberImagePath = vMemberImg,
                        @MemberSignPath = vMemberSign,
                        @QType = 1 // QType 1 is for Insert New Member Image
                    };

                    ultimateReportService.SaveEditMemberImage(imgSaveParam);

                    //======================================= Mostak End ========================================//

                    var param3 = new
                    {
                        @NIDVerificationId = 0,
                        @OfficeID = LoginUserOfficeID,
                        @MemberID = ent.MemberID,
                        @NationalID = vNatio,
                        @SmartCard = vSmat,
                        @OtherIdNo = OtherID,
                        @NIDAddress = model.txtAddress == null ? "" : model.txtAddress,
                        @NIDStatusID = vNIDCheckStatusId,
                        @CreateUser = LoggedInEmployeeID,
                        @UpdateUser = LoggedInEmployeeID
                    };
                    var MemberIDValidate3 = ultimateReportService.GenerateNIDStatus(param3);
                    return Json(new { data = ent }, JsonRequestBehavior.AllowGet);

                }
                else
                    return GetErrorMessageResult();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
                //return GetErrorMessageResult(e);
            }
            //catch (Exception ex)
            //{
            //    return GetErrorMessageResult(ex);
            //}
        }

        public void Capture()
        {
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
                dump = reader.ReadToEnd();
            var dt = DateTime.Now;
            var ImageName = dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + ".jpg";

            var path = Server.MapPath("~/CapturedImages/");

            path = System.IO.Path.Combine(path, ImageName);

            System.Web.HttpContext.Current.Session["CapturedImage"] = ImageName;

            System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));
        }
        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;
            byte[] bytes = new byte[numBytes];

            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }

            return bytes;
        }
        public JsonResult GetCapturedImage()
        {

            string data = Session["CapturedImage"].ToString();  //"CapturedImage/";

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //End
        //end CaptureImage

        public ActionResult AdminIndex()
        {
            var model = new MemberViewModel();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;

            ViewData["LoggedInOrg"] = SessionHelper.LoginUserOrganizationID;
            ViewData["RoleId"] = SessionHelper.LoginUserRoleId;
            return View();
        }

        public ActionResult AdminEdit(int id)
        {
            var member = memberService.GetById(id);
            var memberModel = Mapper.Map<Member, MemberViewModel>(member);
            //            memberModel.OfficeID = 2;
            MapDropDownList(memberModel);
            var allCenter = centerService.GetByOfficeId(memberModel.OfficeID, Convert.ToInt16(LoggedInOrganizationID));
            var viewCenter = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + "-" + x.CenterName.ToString()
            });
            memberModel.CenterList = viewCenter;
            var allGroup = groupService.GetByOfficeId(memberModel.OfficeID);
            var viewGroup = allGroup.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GroupID.ToString(),
                Text = x.GroupCode.ToString()
            });
            memberModel.GroupList = viewGroup;
            memberModel.GroupID = member.GroupID;
            memberModel.Gender = member.Gender;
            memberModel.Location = member.Location;
            memberModel.PlaceOfBirth = member.PlaceOfBirth;
            memberModel.ServerCurrentDate = DateTime.Now;
            memberModel.ExpireDate = member.ExpireDate;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;


            var OrgInfo = organizationService.GetById((int)SessionHelper.LoginUserOrganizationID);
            //var v = OrgInfo.MemberAge;
            var MemberAge = 60;

            if (OrgInfo.MemberAge == null)
            {

            }
            else
            {
                MemberAge = (int)OrgInfo.MemberAge;
            }
            ViewData["MemberAge"] = MemberAge;


            var imgentityparam = new { @MemberID = memberModel.MemberID, @OfficeId = memberModel.OfficeID };
            var Entity = ultimateReportService.GetDataWithParameter(imgentityparam, "Proc_GetVerificationStatus");

            var NIDStatusID = 0;
            var NIDStatusIDst = "";
            var NIDAddress = "";
            var NIDVerificationId = "";
            if (Entity.Tables[0].Rows.Count > 0)
            {
                NIDStatusIDst = Entity.Tables[0].Rows[0]["NIDStatusID"].ToString();
                NIDAddress = Entity.Tables[0].Rows[0]["NIDAddress"].ToString();
                NIDVerificationId = Entity.Tables[0].Rows[0]["NIDVerificationId"].ToString();
            }

            if (NIDStatusIDst != null)
            {
                if (NIDStatusIDst != "")
                {

                    NIDStatusID = Convert.ToInt32(NIDStatusIDst);
                }
                else
                {
                    NIDStatusID = Convert.ToInt32("1");
                }
            }

            if (NIDVerificationId != null)
            {
                if (NIDVerificationId != "")
                {
                    memberModel.NIDVerificationId = null;
                }
            }

            memberModel.NIDCheckStatusId = NIDStatusID;
            memberModel.txtAddress = NIDAddress;




            return View(memberModel);
        }
        // GET: Member/Edit/5
        public ActionResult Edit(int id)
        {
            var member = memberService.GetById(id);
            var memberModel = Mapper.Map<Member, MemberViewModel>(member);
            //            memberModel.OfficeID = 2;
            MapDropDownList(memberModel);
            var allCenter = centerService.GetByOfficeId(memberModel.OfficeID, Convert.ToInt16(LoggedInOrganizationID));
            var viewCenter = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + "-" + x.CenterName.ToString()
            });
            memberModel.CenterList = viewCenter;
            var allGroup = groupService.GetByOfficeId(memberModel.OfficeID);
            var viewGroup = allGroup.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GroupID.ToString(),
                Text = x.GroupCode.ToString()
            });
            memberModel.GroupList = viewGroup;
            memberModel.GroupID = member.GroupID;
            memberModel.Gender = member.Gender;
            memberModel.Location = member.Location;
            memberModel.PlaceOfBirth = member.PlaceOfBirth;
            memberModel.ServerCurrentDate = DateTime.Now;
            memberModel.ExpireDate = member.ExpireDate;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;


            var OrgInfo = organizationService.GetById((int)SessionHelper.LoginUserOrganizationID);
            //var v = OrgInfo.MemberAge;
            var MemberAge = 60;

            if (OrgInfo.MemberAge == null)
            {

            }
            else
            {
                MemberAge = (int)OrgInfo.MemberAge;
            }
            ViewData["MemberAge"] = MemberAge;


            var imgentityparam = new { @MemberID = memberModel.MemberID, @OfficeId = memberModel.OfficeID };
            var Entity = ultimateReportService.GetDataWithParameter(imgentityparam, "Proc_GetVerificationStatus");

            var NIDStatusID = 0;
            var NIDStatusIDst = "";
            var NIDAddress = "";
            var NIDVerificationId = "";
            if (Entity.Tables[0].Rows.Count > 0)
            {
                NIDStatusIDst = Entity.Tables[0].Rows[0]["NIDStatusID"].ToString();
                NIDAddress = Entity.Tables[0].Rows[0]["NIDAddress"].ToString();
                NIDVerificationId = Entity.Tables[0].Rows[0]["NIDVerificationId"].ToString();
            }

            if (NIDStatusIDst != null)
            {
                if (NIDStatusIDst != "")
                {

                    NIDStatusID = Convert.ToInt32(NIDStatusIDst);
                }
                else
                {
                    NIDStatusID = Convert.ToInt32("1");
                }
            }

            if (NIDVerificationId != null)
            {
                if (NIDVerificationId != "")
                {
                    memberModel.NIDVerificationId = null;
                }
            }

            memberModel.NIDCheckStatusId = NIDStatusID;
            memberModel.txtAddress = NIDAddress;




            return View(memberModel);
        }
        // POST: Member/Edit/5
        [HttpPost]
        public ActionResult Edit(MemberEditViewModel model, string base64image, string base64imageFingerThumb)
        {
            try
            {
                var entity = memberService.GetByIdLong(model.MemberID);

                //foreach (ModelState modelState in ModelState.Values)
                //{
                //    foreach (ModelError error in modelState.Errors)
                //    {
                //        var myerr = error.ErrorMessage;
                //    }
                //}
                //if (entity.MemberImg == null)
                //{
                //    DataSet vMemberImage;

                //    if (entity.MemberImg == null)
                //    {
                //        var param2 = new { @MemberID = model.MemberID };

                //        vMemberImage = ultimateReportService.GetMemberImage(param2);
                //        var img = entity.MemberImg;
                //        if (vMemberImage.Tables[0].Rows.Count > 0)
                //        {
                //            img = entity.MemberImg;
                //        }
                //        img = entity.MemberImg;
                //    }
                //}


                if (!ModelState.IsValid)
                    return GetErrorMessageResult();

                //if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != "") && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                //{
                //    var t = base64image.Substring(22);  // remove data:image/png;base64,
                //    byte[] bytes = Convert.FromBase64String(t);
                //    Image image;
                //    using (MemoryStream ms = new MemoryStream(bytes))
                //    {
                //        image = Image.FromStream(ms);
                //    }
                //    var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                //    entity.MemberImg = bytes;

                //    ///////////////////////////

                //    var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                //    byte[] bytesthumb = Convert.FromBase64String(tthumb);
                //    Image imagethumb;
                //    using (MemoryStream msa = new MemoryStream(bytesthumb))
                //    {
                //        imagethumb = Image.FromStream(msa);
                //    }
                //    var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                //    entity.ThumbImg = bytesthumb;
                //}
                //else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != ""))
                //{
                //    var t = base64image.Substring(22);  // remove data:image/png;base64,
                //    byte[] bytes = Convert.FromBase64String(t);
                //    Image image;
                //    using (MemoryStream ms = new MemoryStream(bytes))
                //    {
                //        image = Image.FromStream(ms);
                //    }
                //    var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                //    //byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                //    entity.MemberImg = bytes;
                //}
                //else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                //{
                //    var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                //    byte[] bytesthumb = Convert.FromBase64String(tthumb);
                //    Image imagethumb;
                //    using (MemoryStream msa = new MemoryStream(bytesthumb))
                //    {
                //        imagethumb = Image.FromStream(msa);
                //    }
                //    var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                //    entity.ThumbImg = bytesthumb;
                //}
                //else if (model.ImgFile == null && Session["CapturedImage"] != null)
                //{
                //    var path = Server.MapPath("~/CapturedImages/");

                //    var FileLocation = path + Session["CapturedImage"].ToString();
                //    System.IO.File.Exists(FileLocation);
                //    System.IO.FileInfo file = new System.IO.FileInfo(FileLocation);

                //    using (System.Drawing.Image image = System.Drawing.Image.FromFile(file.FullName))
                //    {
                //        //Image image = System.Drawing.Image.FromFile(file.FullName);
                //        ImageConverter converter = new ImageConverter();
                //        byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                //        entity.MemberImg = data;

                //        decimal size = Math.Round(((decimal)entity.MemberImg.Length / (decimal)1024), 2);
                //        if (size > 100)
                //        {
                //            var errors = memberService.CheckImageSize();
                //            Response.StatusCode = 400;
                //            //("File size must not exceed 100 KB.");
                //            //CustomValidator1.ErrorMessage = "File size must not exceed 100 KB.";
                //            //errors("File size must not exceed 100 KB.");
                //            // e.IsValid = false;
                //        }

                //    }

                //    Array.ForEach(Directory.GetFiles(path), System.IO.File.Delete);

                //}
                //else if (model.ImgFile != null)
                //{
                //    decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                //    if (size > 100)
                //    {
                //        var error = memberService.CheckImageSize();
                //        Response.StatusCode = 400;
                //        throw new System.InvalidOperationException("Image File is Big than 100 KB.");
                //    }

                //    byte[] data = new byte[model.ImgFile.ContentLength];
                //    if (data != null)
                //    {
                //        decimal size2 = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                //        if (size2 > 100)
                //        {
                //            var error = memberService.CheckImageSize();
                //            Response.StatusCode = 400;
                //            throw new System.InvalidOperationException("Image File is Big than 100 KB.");

                //        }


                //        model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                //        entity.MemberImg = data;
                //    }
                //}

                //if (model.ThumbImgFile != null)
                //{
                //    decimal size2 = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                //    if (size2 > 100)
                //    {
                //        var error = memberService.CheckImageSize();
                //        Response.StatusCode = 400;
                //    }

                //    byte[] data = new byte[model.ThumbImgFile.ContentLength];
                //    if (data != null)
                //    {
                //        model.ThumbImgFile.InputStream.Read(data, 0, model.ThumbImgFile.ContentLength);
                //        entity.ThumbImg = data;
                //    }
                //}

                if (entity.OfficeID != LoginUserOfficeID)
                {
                    return GetErrorMessageResult("Invalid Office...........");
                }

                entity.GroupID = model.GroupID;
                entity.FirstName = model.FirstName;
                entity.MiddleName = model.MiddleName;
                entity.LastName = model.LastName;
                entity.AddressLine1 = model.AddressLine1;
                entity.AddressLine2 = model.AddressLine2;
                entity.RefereeName = model.RefereeName;
                entity.BirthDate = model.BirthDate;
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                var allProducts = accReportService.GetLastInitialDate(param);

                entity.NationalID = model.NationalID;
                entity.HomeType = model.HomeType;
                entity.FamilyContactNo = model.FamilyContactNo;

                entity.Email = model.Email;
                entity.PhoneNo = model.PhoneNo;
                entity.MaritalStatus = model.MaritalStatus;
                entity.MemCategory = model.MemCategory;
                entity.EconomicActivity = model.EconomicActivity;

                // NEW ADD KHALID
                entity.AddressLine1 = model.AddressLine1;
                entity.AddressLine2 = model.AddressLine2;
                entity.CountryID = model.CountryID;
                entity.DivisionCode = model.DivisionCode;
                entity.DistrictCode = model.DistrictCode;
                entity.UpozillaCode = model.UpozillaCode;
                entity.UnionCode = model.UnionCode;
                entity.VillageCode = model.VillageCode;
                entity.ZipCode = model.ZipCode;
                entity.Education = model.Education;


                entity.IsAnyFS = model.IsAnyFS;
                entity.FServiceName = model.FServiceName;
                entity.FinServiceChoiceId = model.FinServiceChoiceId;
                entity.TransactionChoiceId = model.TransactionChoiceId;

                entity.BanglaName = model.BanglaName;
                entity.PerAddressLine1 = model.PerAddressLine1;
                entity.PerAddressLine2 = model.PerAddressLine2;
                entity.PerCountryID = model.PerCountryID;
                entity.PerDivisionCode = model.PerDivisionCode;
                entity.PerDistrictCode = model.PerDistrictCode;
                entity.PerUpozillaCode = model.PerUpozillaCode;
                entity.PerUnionCode = model.PerUnionCode;
                entity.PerVillageCode = model.PerVillageCode;
                entity.PerZipCode = model.PerZipCode;
                entity.FatherNameBN = model.FatherNameBN;
                entity.MotherNameBN = model.MotherNameBN;
                entity.IdentTypeID = model.IdentTypeID;
                entity.ExpireDate = model.ExpireDate;
                entity.ProvidedByCountryID = model.ProvidedByCountryID;
                entity.SpouseName = model.SpouseName;
                entity.SpouseNameBN = model.SpouseNameBN;
                entity.TIN = model.TIN;
                entity.TaxAmount = model.TaxAmount;
                entity.OtherIdNo = model.OtherIdNo;
                entity.CoApplicantName = model.CoApplicantName;
                entity.Gender = model.Gender;
                entity.FamilyMember = model.FamilyMember;
                entity.Cityzenship = model.Cityzenship;
                entity.ExpireDate = model.ExpireDate;
                // END

                if (model.DivisionCode != null)
                {
                    if (model.DivisionCode != "0")
                    {
                        entity.DivisionCode = model.DivisionCode;
                    }
                    else
                    {
                        entity.DivisionCode = "";
                    }
                }
                if (model.DistrictCode != null)
                {
                    if (model.DistrictCode != "0")
                    {
                        entity.DistrictCode = model.DistrictCode;
                    }
                    else
                    {
                        entity.DistrictCode = "";
                    }
                }
                if (model.UpozillaCode != null)
                {
                    if (model.UpozillaCode != "0")
                    {
                        entity.UpozillaCode = model.UpozillaCode;
                    }
                    else
                    {
                        entity.UpozillaCode = "";
                    }
                }
                if (model.UnionCode != null)
                {
                    if (model.UnionCode != "0")
                    {
                        entity.UnionCode = model.UnionCode;
                    }
                    else
                    {
                        entity.UnionCode = "";
                    }
                }
                if (model.VillageCode != null)
                {
                    if (model.VillageCode != "0")
                    {
                        entity.VillageCode = model.VillageCode;
                    }
                    else
                    {
                        entity.VillageCode = "";
                    }
                }


                if (model.PerDivisionCode != null)
                {
                    if (model.PerDivisionCode != "0")
                    {
                        entity.PerDivisionCode = model.PerDivisionCode;
                    }
                    else
                    {
                        entity.PerDivisionCode = "";
                    }
                }
                if (model.PerDistrictCode != null)
                {
                    if (model.PerDistrictCode != "0")
                    {
                        entity.PerDistrictCode = model.PerDistrictCode;
                    }
                    else
                    {
                        entity.PerDistrictCode = "";
                    }
                }
                if (model.PerUpozillaCode != null)
                {
                    if (model.PerUpozillaCode != "0")
                    {
                        entity.PerUpozillaCode = model.PerUpozillaCode;
                    }
                    else
                    {
                        entity.PerUpozillaCode = "";
                    }
                }
                if (model.PerUnionCode != null)
                {
                    if (model.PerUnionCode != "0")
                    {
                        entity.PerUnionCode = model.PerUnionCode;
                    }
                    else
                    {
                        entity.PerUnionCode = "";
                    }
                }
                if (model.PerVillageCode != null)
                {
                    if (model.PerVillageCode != "0")
                    {
                        entity.PerVillageCode = model.PerVillageCode;
                    }
                    else
                    {
                        entity.PerVillageCode = "";
                    }
                }

                if (model.PlaceOfBirth != null)
                {
                    entity.PlaceOfBirth = model.PlaceOfBirth;

                }
                if (model.TotalWealth != null)
                {
                    entity.TotalWealth = model.TotalWealth;

                }
                if (model.MotherName != null)
                {
                    entity.MotherName = model.MotherName;

                }
                if (model.FatherName != null)
                {
                    entity.FatherName = model.FatherName;

                }
                if (model.NationalID != null)
                {
                    if (model.NationalID.Length < 10) // if NationalId and Smart Card both not given
                    {
                        return GetErrorMessageResult("Warning! NationalId Or Smart or OtherNo Card Must be Given...");
                    }
                    else
                        entity.NationalID = model.NationalID;
                }
                if (model.NationalID == null)
                {
                    entity.NationalID = "0";
                }

                if (model.SmartCard != null)
                {
                    if (model.SmartCard.Length < 10) // if NationalId and Smart Card both not given
                    {
                        return GetErrorMessageResult("Warning! NationalId Or Smart or OtherNo Card Must be Given...");
                    }
                    else
                        entity.SmartCard = model.SmartCard;
                }
                if (model.SmartCard == null)
                {
                    entity.SmartCard = "0";
                }
                if (model.PhoneNo != null)
                {
                    entity.PhoneNo = model.PhoneNo;

                }
                //if (entity.PhoneNo == null)
                //{
                //    return GetErrorMessageResult("PhoneNo cann't be null");
                //}


                var vNatio = "";
                var vSmat = "";
                var vphone = "";
                var OtherID = "";
                var vMemberStatus = "";
                int? vNIDCheckStatusId = 0;


                if (entity.NationalID == null)
                {
                    vNatio = "0";
                }
                else
                {
                   
                    vNatio = entity.NationalID;
                }
                if (entity.SmartCard == null)
                {
                    vSmat = "0";
                }
                else
                {
                   
                    vSmat = entity.SmartCard;

                }
                if (entity.PhoneNo == null)
                {
                    vphone = "0";
                }
                else
                {
                    vphone = entity.PhoneNo;
                }

                if (model.NIDCheckStatusId == null)
                {
                    vNIDCheckStatusId = 1;
                }
                else
                {
                    vNIDCheckStatusId = model.NIDCheckStatusId;
                }

               


                var CheckDupli = new { @NationalID = vNatio, @SmartCard = vSmat, @PhoneNo = vphone, @Qtype = 2, @MemberCode = entity.MemberCode };
                var CheckMemberDupli = ultimateReportService.CheckDuplicateMember(CheckDupli);
                if (CheckMemberDupli.Tables.Count > 0)
                {
                    if (CheckMemberDupli.Tables[0].Rows.Count > 0)
                    {
                        return GetErrorMessageResult(CheckMemberDupli.Tables[0].Rows[0]["ErrorName"].ToString());
                    }
                }
                //if (CheckMemberDupli.Tables[0].Rows.Count > 0)
                //{
                //    return GetErrorMessageResult(CheckMemberDupli.Tables[0].Rows[0]["ErrorName"].ToString());
                //}

                if (Session["CapturedImage"] == null && (model.ImgFile != null && model.ImgFile.ContentLength > 0))
                {
                    entity.MemberImg = null;
                }
                else if (model.ImgFile == null && (base64image != null && base64image != ""))
                {
                    entity.MemberImg = null;
                }

                if (model.ThumbImgFile != null && model.ThumbImgFile.ContentLength > 0)
                {
                    entity.ThumbImg = null;
                }
                else if (model.ThumbImgFile == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                {
                    entity.ThumbImg = null;
                }
                memberService.Update(entity);

                // ----------------------------------------------------

                //==========================================================================================//
                // Edit     : Mostak Ahammed                                                                //
                // Date     : 21 Aug 2023                                                                   //
                // Desc     : Image will save in project directory and path will save in the database       //
                //==========================================================================================//

                var imgentityparam = new
                {
                    @OfficeID = SessionHelper.LoginUserOfficeID,
                    @CenterID = model.CenterID,
                    @MemberID = model.MemberID,
                    @MemberImagePath = "",
                    @MemberSignPath = "",
                    @QType = 3 // Return Image data for delete existing image to update new image
                };
                var imgEntity = ultimateReportService.GetDataWithParameter(imgentityparam, "Proc_SaveEdit_MemberImage");

                var existing_img = "";
                var existing_sign = "";

                if (imgEntity.Tables[0].Rows.Count > 0)
                {
                    existing_img = imgEntity.Tables[0].Rows[0]["MemberImagePath"].ToString();
                    existing_sign = imgEntity.Tables[0].Rows[0]["MemberSignPath"].ToString();
                }

                var vMemberImg = "";
                var vMemberSign = "";
                //Path Directory in project folder
                string memberImageDirectory = Server.MapPath("~/Uploads/Member/Image/");
                string memberSignDirectory = Server.MapPath("~/Uploads/Member/Signature/");
                string memberImagePath = "~/Uploads/Member/Image/";
                string memberSignPath = "~/Uploads/Member/Signature/";

                //If directory not exists then create new directory
                if (!Directory.Exists(memberImageDirectory))
                {
                    Directory.CreateDirectory(memberImageDirectory);
                }
                if (!Directory.Exists(memberSignDirectory))
                {
                    Directory.CreateDirectory(memberSignDirectory);
                }

                //If user select the image
                if (Session["CapturedImage"] == null && (model.ImgFile != null && model.ImgFile.ContentLength > 0))
                {
                    //Delete old image if exists
                    string oldImagePath = Server.MapPath(existing_img);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                    if (size > 100)
                    {
                        memberService.CheckImageSize();
                        Response.StatusCode = 400;
                        throw new System.InvalidOperationException("Image File is Bigger than 100 KB.");
                    }

                    //var shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 10);
                    string fileName = $"img_{model.MemberID}" + Path.GetExtension(model.ImgFile.FileName);
                    string imagePath = Path.Combine(memberImageDirectory, fileName);
                    model.ImgFile.SaveAs(imagePath);

                    vMemberImg = memberImagePath + fileName;
                    Session["CapturedImage"] = null;
                }
                // MemberImage upload as Captured
                else if (model.ImgFile == null && (base64image != null && base64image != ""))
                {
                    //Delete old image if exists
                    string oldImagePath = Server.MapPath(existing_img);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    var main_baseString = base64image.Substring(22);
                    byte[] imageByte = Convert.FromBase64String(main_baseString);

                    var path = Server.MapPath("~/Uploads/Member/Image/");
                    var capturedPath = "~/Uploads/Member/Image/";

                    //var shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 10);
                    var capturedFileName = $"img_{model.MemberID}" + ".jpg";

                    var fileLocation = Path.Combine(path, capturedFileName);

                    System.IO.File.WriteAllBytes(fileLocation, imageByte);
                    vMemberImg = capturedPath + capturedFileName;
                }

                if (model.ThumbImgFile != null && model.ThumbImgFile.ContentLength > 0)
                {
                    string oldSignPath = Server.MapPath(existing_sign);
                    if (System.IO.File.Exists(oldSignPath))
                    {
                        System.IO.File.Delete(oldSignPath);
                    }


                    decimal size = Math.Round(((decimal)model.ThumbImgFile.ContentLength / (decimal)1024), 2);
                    if (size > 100)
                    {
                        memberService.CheckImageSize();
                        Response.StatusCode = 400;
                        throw new System.InvalidOperationException("Image File is Bigger than 100 KB.");
                    }
                    //var shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 10);
                    string fileName = $"sign_{model.MemberID}" + Path.GetExtension(model.ThumbImgFile.FileName);
                    string thumbImagePath = Path.Combine(memberSignDirectory, fileName);
                    model.ThumbImgFile.SaveAs(thumbImagePath);

                    vMemberSign = memberSignPath + fileName;
                    Session["CapturedImage"] = null;
                }
                // Member Signature upload as Capture
                else if (model.ThumbImgFile == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                {
                    string oldSignPath = Server.MapPath(existing_sign);
                    if (System.IO.File.Exists(oldSignPath))
                    {
                        System.IO.File.Delete(oldSignPath);
                    }

                    var main_baseString = base64imageFingerThumb.Substring(22);
                    byte[] imageByte = Convert.FromBase64String(main_baseString);

                    var path = Server.MapPath("~/Uploads/Member/Signature/");
                    var capturedPath = "~/Uploads/Member/Signature/";

                    //var shortGuid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 10);
                    var capturedFileName = $"sign_{model.MemberID}" + ".jpg";
                    var fileLocation = Path.Combine(path, capturedFileName);

                    System.IO.File.WriteAllBytes(fileLocation, imageByte);
                    vMemberSign = capturedPath + capturedFileName;
                }


                //MemberImage Edit
                var imgSaveParam = new
                {
                    @OfficeID = SessionHelper.LoginUserOfficeID,
                    @CenterID = model.CenterID,
                    @MemberID = model.MemberID,
                    @MemberImagePath = vMemberImg,
                    @MemberSignPath = vMemberSign,
                    @QType = 2 // Edit Image QType
                };

                ultimateReportService.SaveEditMemberImage(imgSaveParam);

                //======================================= Mostak End ========================================//

                var param3 = new
                {
                    @NIDVerificationId = model.NIDVerificationId,
                    @OfficeID = LoginUserOfficeID,
                    @MemberID = model.MemberID,
                    @NationalID = vNatio,
                    @SmartCard = vSmat,
                    @OtherIdNo = OtherID,
                    @NIDAddress = model.txtAddress == null ? "" : model.txtAddress,
                    @NIDStatusID = vNIDCheckStatusId,
                    @CreateUser = LoggedInEmployeeID,
                    @UpdateUser = LoggedInEmployeeID
                };
                var MemberIDValidate3 = ultimateReportService.GenerateNIDStatus(param3);
                return GetSuccessMessageResult();


            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        public ActionResult IndexCIB()
        {
            var model = new CIBViewModel();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }
            return View();
        }

        public ActionResult CreateCIB()
        {
            var model = new CIBViewModel();
            CIBMapDropDownList(model);
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }
            model.ServerCurrentDate = DateTime.Now;
            var blnk_items = new List<SelectListItem>();
            model.CenterList = blnk_items;
            model.GroupList = blnk_items;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;

            return View(model);
        }

        // POST: Member/CreateCIB
        [HttpPost]
        public JsonResult CreateCIB(CIBViewModel model, string MemCode, string base64image, string base64imageFingerThumb)
        {
            try
            {

                var entity = Mapper.Map<CIBViewModel, Member>(model);
                if (ModelState.IsValid)
                {
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                    var errors = memberService.IsValidMember(entity);
                    //{
                    if (errors.ToList().Count == 0)
                    {
                        if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != "") && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                        {
                            var t = base64image.Substring(22);  // remove data:image/png;base64,
                            byte[] bytes = Convert.FromBase64String(t);
                            Image image;
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                image = Image.FromStream(ms);
                            }
                            var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                            //var fullPath = Path.Combine(Server.MapPath("~/CapturedImages/"), randomFileName);
                            //image.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
                            entity.MemberImg = bytes;

                            ///////////////////////////

                            var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                            byte[] bytesthumb = Convert.FromBase64String(tthumb);
                            Image imagethumb;
                            using (MemoryStream msa = new MemoryStream(bytesthumb))
                            {
                                imagethumb = Image.FromStream(msa);
                            }
                            var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                            //var fullPaththumb = Path.Combine(Server.MapPath("~/CapturedImages/"), randomFileNamethumb);
                            //imagethumb.Save(fullPaththumb, System.Drawing.Imaging.ImageFormat.Png);
                            entity.ThumbImg = bytesthumb;
                        }
                        else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != ""))
                        {
                            var t = base64image.Substring(22);  // remove data:image/png;base64,
                            byte[] bytes = Convert.FromBase64String(t);
                            Image image;
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                image = Image.FromStream(ms);
                            }
                            var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                            entity.MemberImg = bytes;
                        }
                        else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                        {
                            var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                            byte[] bytesthumb = Convert.FromBase64String(tthumb);
                            Image imagethumb;
                            using (MemoryStream msa = new MemoryStream(bytesthumb))
                            {
                                imagethumb = Image.FromStream(msa);
                            }
                            var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                            entity.ThumbImg = bytesthumb;
                        }
                        else if (model.ImgFile == null && Session["CapturedImage"] != null)
                        {
                            var path = Server.MapPath("~/CapturedImages/");

                            var FileLocation = path + Session["CapturedImage"].ToString();
                            System.IO.File.Exists(FileLocation);
                            System.IO.FileInfo file = new System.IO.FileInfo(FileLocation);

                            using (System.Drawing.Image image = System.Drawing.Image.FromFile(file.FullName))
                            {
                                //Image image = System.Drawing.Image.FromFile(file.FullName);
                                ImageConverter converter = new ImageConverter();
                                byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                                entity.MemberImg = data;


                                decimal size = Math.Round(((decimal)entity.MemberImg.Length / (decimal)1024), 2);
                                if (size > 100)
                                {
                                    errors = memberService.CheckImageSize();
                                    Response.StatusCode = 400;
                                    //("File size must not exceed 100 KB.");
                                    //CustomValidator1.ErrorMessage = "File size must not exceed 100 KB.";
                                    //errors("File size must not exceed 100 KB.");
                                    // e.IsValid = false;
                                }
                            }


                            Array.ForEach(Directory.GetFiles(path), System.IO.File.Delete);

                        }
                        else if (model.ImgFile != null)
                        {

                            decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                            if (size > 100)
                            {
                                //NEW ADD NEW ADD KHALID 19 NOV,2019  Validate Image 100 KB
                                errors = memberService.CheckImageSize();
                                Response.StatusCode = 400;
                                //("File size must not exceed 100 KB.");
                                //CustomValidator1.ErrorMessage = "File size must not exceed 100 KB.";
                                //errors("File size must not exceed 100 KB.");
                                // e.IsValid = false;
                            }


                            byte[] data = new byte[model.ImgFile.ContentLength];
                            if (data != null)
                            {
                                model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                                entity.MemberImg = data;
                            }
                        }

                        if (model.ThumbImgFile != null)
                        {
                            decimal size = Math.Round(((decimal)model.ThumbImgFile.ContentLength / (decimal)1024), 2);
                            if (size > 100)
                            {
                                errors = memberService.CheckImageSize();
                                Response.StatusCode = 400;
                            }

                            byte[] data = new byte[model.ThumbImgFile.ContentLength];
                            if (data != null)
                            {
                                model.ThumbImgFile.InputStream.Read(data, 0, model.ThumbImgFile.ContentLength);
                                entity.ThumbImg = data;
                            }
                        }



                        if (model.DivisionCode != null)
                        {
                            if (model.DivisionCode != "0")
                            {
                                entity.DivisionCode = model.DivisionCode;
                            }
                            else
                            {
                                entity.DivisionCode = "";
                            }
                        }
                        if (model.DistrictCode != null)
                        {
                            if (model.DistrictCode != "0")
                            {
                                entity.DistrictCode = model.DistrictCode;
                            }
                            else
                            {
                                entity.DistrictCode = "";
                            }
                        }
                        if (model.UpozillaCode != null)
                        {
                            if (model.UpozillaCode != "0")
                            {
                                entity.UpozillaCode = model.UpozillaCode;
                            }
                            else
                            {
                                entity.UpozillaCode = "";
                            }
                        }
                        if (model.UnionCode != null)
                        {
                            if (model.UnionCode != "0")
                            {
                                entity.UnionCode = model.UnionCode;
                            }
                            else
                            {
                                entity.UnionCode = "";
                            }
                        }
                        if (model.VillageCode != null)
                        {
                            if (model.VillageCode != "0")
                            {
                                entity.VillageCode = model.VillageCode;
                            }
                            else
                            {
                                entity.VillageCode = "";
                            }
                        }
                        if (model.PlaceOfBirth != null)
                        {
                            entity.PlaceOfBirth = model.PlaceOfBirth;

                        }
                        if (model.TotalWealth != null)
                        {
                            entity.TotalWealth = model.TotalWealth;

                        }
                        if (model.MotherName != null)
                        {
                            entity.MotherName = model.MotherName;

                        }
                        if (model.FatherName != null)
                        {
                            entity.FatherName = model.FatherName;

                        }
                        if (model.NationalID != null)
                        {
                            entity.NationalID = model.NationalID;

                        }
                        if (model.PhoneNo != null)
                        {
                            entity.PhoneNo = model.PhoneNo;

                        }
                        //var mem = memberService.CheckMemberNationalId(entity.NationalID);
                        //if (mem.ToList().Count > 0)
                        //{
                        //    return GetErrorMessageResult("NationalID Already Exists");
                        //}
                        //HttpClient Client = new HttpClient();
                        //Client.BaseAddress = new Uri("http://192.192.192.233:9099/"); //api/nonmaskingsms/easysend?sender=01713140127&message=hello%20bangladesh)
                        //Client.DefaultRequestHeaders.Clear();
                        //Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        ////Client.DefaultRequestHeaders.Add("Accept", "application/json");
                        ////Client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                        //HttpResponseMessage message = Client.GetAsync($"buro/api/NationalID/{model.NationalID}").Result;
                        //if (message.IsSuccessStatusCode)
                        //{
                        //    var jdata = message.Content.ReadAsAsync<MemberNationalIDViewModel>();
                        //    if (jdata.Result!=null)
                        //    {
                        //        return GetErrorMessageResult("NationalID Already Exists(IMFASDATA)");
                        //    }
                        //}

                        //var memPhone = memberService.CheckMemberPhoneNo(entity.PhoneNo);
                        //if (memPhone.ToList().Count > 0)
                        //{
                        //    return GetErrorMessageResult("PhoneNo Already Exists");
                        //}
                        entity.IsActive = true;
                        entity.MemberStatus = "0";
                        entity.PwdStatus = "D";
                        entity.MemberType = 1;

                        var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                        var allProducts = accReportService.GetLastInitialDate(param);

                        var detail = allProducts.ToString();

                        if (!IsDayInitiated)
                        {
                            if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                            {
                                entity.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                            }
                        }
                        else
                        {
                            entity.JoinDate = TransactionDate;
                        }

                        var param1 = new { @OfficeID = LoginUserOfficeID };
                        var LoanInstallMent = ultimateReportService.GenerateMemberLastCode(param1);
                        entity.MemberCode = LoanInstallMent.Tables[0].Rows[0]["LastCode"].ToString();


                        memberService.Create(entity);
                        var ent = new { MemberID = entity.MemberID, MemberCode = entity.MemberCode };
                        return Json(new { data = ent }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return GetErrorMessageResult(errors);
                }
                else
                    return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: Member/Edit/5
        public ActionResult EditCIB(int id)
        {
            var member = memberService.GetById(id);
            var memberModel = Mapper.Map<Member, CIBViewModel>(member);
            //            memberModel.OfficeID = 2;
            CIBMapDropDownList(memberModel);
            var allCenter = centerService.GetByOfficeId(memberModel.OfficeID, Convert.ToInt16(LoggedInOrganizationID));
            var viewCenter = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + "-" + x.CenterName.ToString()
            });
            memberModel.CenterList = viewCenter;
            var allGroup = groupService.GetByOfficeId(memberModel.OfficeID);
            var viewGroup = allGroup.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GroupID.ToString(),
                Text = x.GroupCode.ToString()
            });
            memberModel.GroupList = viewGroup;
            memberModel.GroupID = member.GroupID;
            memberModel.Gender = member.Gender;
            memberModel.Location = member.Location;
            memberModel.PlaceOfBirth = member.PlaceOfBirth;
            memberModel.ServerCurrentDate = DateTime.Now;



            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;

            return View(memberModel);
        }
        // POST: Member/Edit/5
        [HttpPost]
        public ActionResult EditCIB(CIBViewModel model, string base64image, string base64imageFingerThumb)
        {
            try
            {
                var entity = memberService.GetByIdLong(model.MemberID);
                if (ModelState.IsValid)
                {
                    if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != "") && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                    {
                        var t = base64image.Substring(22);  // remove data:image/png;base64,
                        byte[] bytes = Convert.FromBase64String(t);
                        Image image;
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            image = Image.FromStream(ms);
                        }
                        var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                        entity.MemberImg = bytes;

                        ///////////////////////////

                        var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                        byte[] bytesthumb = Convert.FromBase64String(tthumb);
                        Image imagethumb;
                        using (MemoryStream msa = new MemoryStream(bytesthumb))
                        {
                            imagethumb = Image.FromStream(msa);
                        }
                        var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                        entity.ThumbImg = bytesthumb;
                    }
                    else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != ""))
                    {
                        var t = base64image.Substring(22);  // remove data:image/png;base64,
                        byte[] bytes = Convert.FromBase64String(t);
                        Image image;
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            image = Image.FromStream(ms);
                        }
                        var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                        //byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                        entity.MemberImg = bytes;
                    }
                    else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                    {
                        var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                        byte[] bytesthumb = Convert.FromBase64String(tthumb);
                        Image imagethumb;
                        using (MemoryStream msa = new MemoryStream(bytesthumb))
                        {
                            imagethumb = Image.FromStream(msa);
                        }
                        var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                        entity.ThumbImg = bytesthumb;
                    }
                    else if (model.ImgFile == null && Session["CapturedImage"] != null)
                    {
                        var path = Server.MapPath("~/CapturedImages/");

                        var FileLocation = path + Session["CapturedImage"].ToString();
                        System.IO.File.Exists(FileLocation);
                        System.IO.FileInfo file = new System.IO.FileInfo(FileLocation);

                        using (System.Drawing.Image image = System.Drawing.Image.FromFile(file.FullName))
                        {
                            //Image image = System.Drawing.Image.FromFile(file.FullName);
                            ImageConverter converter = new ImageConverter();
                            byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                            entity.MemberImg = data;

                            decimal size = Math.Round(((decimal)entity.MemberImg.Length / (decimal)1024), 2);
                            if (size > 100)
                            {
                                var errors = memberService.CheckImageSize();
                                Response.StatusCode = 400;
                                //("File size must not exceed 100 KB.");
                                //CustomValidator1.ErrorMessage = "File size must not exceed 100 KB.";
                                //errors("File size must not exceed 100 KB.");
                                // e.IsValid = false;
                            }

                        }

                        Array.ForEach(Directory.GetFiles(path), System.IO.File.Delete);

                    }
                    else if (model.ImgFile != null)
                    {
                        decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                        if (size > 100)
                        {
                            var error = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                        }

                        byte[] data = new byte[model.ImgFile.ContentLength];
                        if (data != null)
                        {
                            decimal size2 = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                            if (size2 > 100)
                            {
                                var error = memberService.CheckImageSize();
                                Response.StatusCode = 400;
                            }


                            model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                            entity.MemberImg = data;
                        }
                    }

                    if (model.ThumbImgFile != null)
                    {
                        decimal size2 = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                        if (size2 > 100)
                        {
                            var error = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                        }

                        byte[] data = new byte[model.ThumbImgFile.ContentLength];
                        if (data != null)
                        {
                            model.ThumbImgFile.InputStream.Read(data, 0, model.ThumbImgFile.ContentLength);
                            entity.ThumbImg = data;
                        }
                    }

                    if (entity.OfficeID != LoginUserOfficeID)
                    {
                        return GetErrorMessageResult("Invalid Office...........");
                    }
                    entity.GroupID = model.GroupID;
                    entity.FirstName = model.FirstName;
                    entity.MiddleName = model.MiddleName;
                    entity.LastName = model.LastName;
                    entity.AddressLine1 = model.AddressLine1;
                    entity.AddressLine2 = model.AddressLine2;
                    entity.RefereeName = model.RefereeName;
                    entity.BirthDate = model.BirthDate;
                    var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                    var allProducts = accReportService.GetLastInitialDate(param);

                    entity.NationalID = model.NationalID;

                    entity.Email = model.Email;
                    entity.PhoneNo = model.PhoneNo;
                    entity.MaritalStatus = model.MaritalStatus;
                    entity.MemCategory = model.MemCategory;

                    // NEW ADD KHALID

                    entity.AddressLine1 = model.AddressLine1;
                    entity.AddressLine2 = model.AddressLine2;
                    entity.CountryID = model.CountryID;
                    entity.DivisionCode = model.DivisionCode;
                    entity.DistrictCode = model.DistrictCode;
                    entity.UpozillaCode = model.UpozillaCode;
                    entity.UnionCode = model.UnionCode;
                    entity.VillageCode = model.VillageCode;
                    entity.ZipCode = model.ZipCode;



                    entity.IsAnyFS = model.IsAnyFS;
                    entity.FServiceName = model.FServiceName;
                    entity.FinServiceChoiceId = model.FinServiceChoiceId;
                    entity.TransactionChoiceId = model.TransactionChoiceId;

                    entity.BanglaName = model.BanglaName;
                    entity.PerAddressLine1 = model.PerAddressLine1;
                    entity.PerAddressLine2 = model.PerAddressLine2;
                    entity.PerCountryID = model.PerCountryID;
                    entity.PerDivisionCode = model.PerDivisionCode;
                    entity.PerDistrictCode = model.PerDistrictCode;
                    entity.PerUpozillaCode = model.PerUpozillaCode;
                    entity.PerUnionCode = model.PerUnionCode;
                    entity.PerVillageCode = model.PerVillageCode;
                    entity.PerZipCode = model.PerZipCode;
                    entity.FatherNameBN = model.FatherNameBN;
                    entity.MotherNameBN = model.MotherNameBN;
                    entity.IdentTypeID = model.IdentTypeID;
                    entity.ExpireDate = model.ExpireDate;
                    entity.ProvidedByCountryID = model.ProvidedByCountryID;
                    entity.SpouseName = model.SpouseName;
                    entity.SpouseNameBN = model.SpouseNameBN;
                    entity.TIN = model.TIN;
                    entity.TaxAmount = model.TaxAmount;
                    entity.AsOnDateAge = model.AsOnDateAge;
                    entity.FamilyContactNo = model.FamilyContactNo;
                    entity.CardIssueDate = model.CardIssueDate;


                    // END

                    if (model.DivisionCode != null)
                    {
                        if (model.DivisionCode != "0")
                        {
                            entity.DivisionCode = model.DivisionCode;
                        }
                        else
                        {
                            entity.DivisionCode = "";
                        }
                    }
                    if (model.DistrictCode != null)
                    {
                        if (model.DistrictCode != "0")
                        {
                            entity.DistrictCode = model.DistrictCode;
                        }
                        else
                        {
                            entity.DistrictCode = "";
                        }
                    }
                    if (model.UpozillaCode != null)
                    {
                        if (model.UpozillaCode != "0")
                        {
                            entity.UpozillaCode = model.UpozillaCode;
                        }
                        else
                        {
                            entity.UpozillaCode = "";
                        }
                    }
                    if (model.UnionCode != null)
                    {
                        if (model.UnionCode != "0")
                        {
                            entity.UnionCode = model.UnionCode;
                        }
                        else
                        {
                            entity.UnionCode = "";
                        }
                    }
                    if (model.VillageCode != null)
                    {
                        if (model.VillageCode != "0")
                        {
                            entity.VillageCode = model.VillageCode;
                        }
                        else
                        {
                            entity.VillageCode = "";
                        }
                    }


                    if (model.PerDivisionCode != null)
                    {
                        if (model.PerDivisionCode != "0")
                        {
                            entity.PerDivisionCode = model.PerDivisionCode;
                        }
                        else
                        {
                            entity.PerDivisionCode = "";
                        }
                    }
                    if (model.PerDistrictCode != null)
                    {
                        if (model.PerDistrictCode != "0")
                        {
                            entity.PerDistrictCode = model.PerDistrictCode;
                        }
                        else
                        {
                            entity.PerDistrictCode = "";
                        }
                    }
                    if (model.PerUpozillaCode != null)
                    {
                        if (model.PerUpozillaCode != "0")
                        {
                            entity.PerUpozillaCode = model.PerUpozillaCode;
                        }
                        else
                        {
                            entity.PerUpozillaCode = "";
                        }
                    }
                    if (model.PerUnionCode != null)
                    {
                        if (model.PerUnionCode != "0")
                        {
                            entity.PerUnionCode = model.PerUnionCode;
                        }
                        else
                        {
                            entity.PerUnionCode = "";
                        }
                    }
                    if (model.PerVillageCode != null)
                    {
                        if (model.PerVillageCode != "0")
                        {
                            entity.PerVillageCode = model.PerVillageCode;
                        }
                        else
                        {
                            entity.PerVillageCode = "";
                        }
                    }





                    if (model.PlaceOfBirth != null)
                    {
                        entity.PlaceOfBirth = model.PlaceOfBirth;

                    }
                    if (model.TotalWealth != null)
                    {
                        entity.TotalWealth = model.TotalWealth;

                    }
                    if (model.MotherName != null)
                    {
                        entity.MotherName = model.MotherName;

                    }
                    if (model.FatherName != null)
                    {
                        entity.FatherName = model.FatherName;

                    }
                    if (model.NationalID != null)
                    {
                        entity.NationalID = model.NationalID;

                    }
                    //var mem = memberService.CheckMemberNationalIdEdit(entity.NationalID, entity.MemberID);
                    //if (mem.ToList().Count > 0)
                    //{
                    //    return GetErrorMessageResult("NationalID Already Exists");
                    //}
                    if (model.PhoneNo != null)
                    {
                        entity.PhoneNo = model.PhoneNo;

                    }
                    //var memPhone = memberService.CheckMemberPhoneNoEdit(entity.PhoneNo, entity.MemberID);
                    //if (memPhone.ToList().Count > 0)
                    //{
                    //    return GetErrorMessageResult("PhoneNo Already Exists");
                    //}

                    memberService.Update(entity);
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

        private void CIBMapDropDownList(CIBViewModel model)
        {
            List<CIBViewModel> List_MemberViewModel = new List<CIBViewModel>();
            var param = new { SearchByCode = "", SearchBy = "", SearchType = "dis" };
            var disList = ultimateReportService.GetLocationList(param);

            List_MemberViewModel = disList.Tables[0].AsEnumerable()
            .Select(row => new CIBViewModel
            {
                DistrictCode = row.Field<string>("DistrictCode"),
                DistrictName = row.Field<string>("DistrictName")
            }).ToList();
            var viewDist = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.DistrictCode.ToString(),
                Text = x.DistrictName.ToString()
            });
            var pob_items = new List<SelectListItem>();
            pob_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            pob_items.AddRange(viewDist);
            model.PlaceOfBirthList = pob_items;




            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OfficeID == offc_id && m.OrgID == LoggedInOrganizationID).ToList();
            // var allOffice = officeService.GetById(offc_id);
            var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            //var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.OfficeList = ofc_items;

            var allMemcatService = memberCategoryService.GetAll().Where(w => w.IsActive == true && w.OrgID == LoggedInOrganizationID).OrderBy(o => o.MemberCategoryCode).ToList();
            var viewMemCatService = allMemcatService.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MemberCategoryID.ToString(),
                Text = string.Format("{0}, {1}", x.MemberCategoryCode.ToString(), x.CategoryName.ToString())
            });
            var cat_items = new List<SelectListItem>();
            cat_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            cat_items.AddRange(viewMemCatService);
            model.MemberCategoryList = cat_items;


            var allGeoLocation = geoLocationService.GetAll();
            var viewGeoLocation = allGeoLocation.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GeoLocationID.ToString(),
                Text = x.LocationName.ToString()
            });
            var geo_items = new List<SelectListItem>();
            geo_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            geo_items.AddRange(viewGeoLocation);
            model.GeoLocationList = geo_items;

            var allCountry = countryService.GetAll();
            var viewCountry = allCountry.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString(),
                Selected = x.CountryId.ToString() == "18" ? true : false
            });
            var country_items = new List<SelectListItem>();
            country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            country_items.AddRange(viewCountry);
            model.CountryList = country_items;

            //blank division
            var divisionList = new List<SelectListItem>();
            divisionList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.DivisionList = divisionList;

            //blank district
            var districtList = new List<SelectListItem>();
            districtList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.DistrictList = districtList;

            //blank upozilla
            var upozillaList = new List<SelectListItem>();
            upozillaList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.UpozillaList = upozillaList;

            //blank Union
            var unionList = new List<SelectListItem>();
            unionList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.UnionList = unionList;

            //blank Village
            var villageList = new List<SelectListItem>();
            villageList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.VillageList = villageList;

            var gender_item = new List<SelectListItem>();
            gender_item.Add(new SelectListItem() { Text = "", Value = "Please Select", Selected = true });
            gender_item.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            gender_item.Add(new SelectListItem() { Text = "Female", Value = "Female" });
            gender_item.Add(new SelectListItem() { Text = "Transgender", Value = "T" });
            model.GenderList = gender_item;

            var status_item = new List<SelectListItem>();
            status_item.Add(new SelectListItem() { Text = "Active", Value = "1", Selected = true });
            status_item.Add(new SelectListItem() { Text = "In Active", Value = "0" });
            status_item.Add(new SelectListItem() { Text = "Drop", Value = "2" });
            status_item.Add(new SelectListItem() { Text = "Dead", Value = "3" });
            status_item.Add(new SelectListItem() { Text = "Blacklist", Value = "4" });
            status_item.Add(new SelectListItem() { Text = "Rejected", Value = "5" });
            model.MemberStatusList = status_item;

            var cityzen_item = new List<SelectListItem>();
            cityzen_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            cityzen_item.Add(new SelectListItem() { Text = "By Birth", Value = "BB" });
            cityzen_item.Add(new SelectListItem() { Text = "Migrated", Value = "MI" });
            cityzen_item.Add(new SelectListItem() { Text = "Marital", Value = "MA" });
            cityzen_item.Add(new SelectListItem() { Text = "Nutralization", Value = "NU" });
            model.CityzenshipList = cityzen_item;

            var homeType_item = new List<SelectListItem>();
            homeType_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            homeType_item.Add(new SelectListItem() { Text = "Building", Value = "BU" });
            homeType_item.Add(new SelectListItem() { Text = "Muddy", Value = "MU" });
            homeType_item.Add(new SelectListItem() { Text = "Rented", Value = "RE" });
            homeType_item.Add(new SelectListItem() { Text = "Semi Building", Value = "SB" });
            homeType_item.Add(new SelectListItem() { Text = "Tin Shade", Value = "TN" });
            model.HomeTypeList = homeType_item;

            var groupType_item = new List<SelectListItem>();
            groupType_item.Add(new SelectListItem() { Text = "Solidarity", Value = "SO", Selected = true });
            groupType_item.Add(new SelectListItem() { Text = "Non Solidarity", Value = "NS" });
            groupType_item.Add(new SelectListItem() { Text = "Individual", Value = "IN" });
            groupType_item.Add(new SelectListItem() { Text = "Corporate", Value = "CO" });
            model.GroupTypeList = groupType_item;

            var education_item = new List<SelectListItem>();
            education_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            //education_item.Add(new SelectListItem() { Text = "Under Matric", Value = "UMA", Selected = true });
            education_item.Add(new SelectListItem() { Text = "Pre-Primary", Value = "1" });
            education_item.Add(new SelectListItem() { Text = "Primary", Value = "2" });
            // education_item.Add(new SelectListItem() { Text = "JSC", Value = "JSC" });
            education_item.Add(new SelectListItem() { Text = "Secondary", Value = "3" });
            education_item.Add(new SelectListItem() { Text = "Higher Secondary", Value = "4" });
            // education_item.Add(new SelectListItem() { Text = "Diploma", Value = "DIP" });
            education_item.Add(new SelectListItem() { Text = "Graduate", Value = "5" });
            education_item.Add(new SelectListItem() { Text = "PostGraduate", Value = "6" });
            // education_item.Add(new SelectListItem() { Text = "Illiterate", Value = "ILL" });
            education_item.Add(new SelectListItem() { Text = "Other", Value = "7" });
            model.EducationList = education_item;

            var economic_item = new List<SelectListItem>();
            economic_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            economic_item.Add(new SelectListItem() { Text = "SelfEmployed", Value = "SF" });
            economic_item.Add(new SelectListItem() { Text = "Service", Value = "SE" });
            economic_item.Add(new SelectListItem() { Text = "Business", Value = "BU" });

            economic_item.Add(new SelectListItem() { Text = "House Hold", Value = "HH" });
            economic_item.Add(new SelectListItem() { Text = "Farmer", Value = "FR" });
            economic_item.Add(new SelectListItem() { Text = "Agriculture", Value = "AG" });
            economic_item.Add(new SelectListItem() { Text = "Others", Value = "OT" });


            model.EconomicActivityList = economic_item;

            var marital_item = new List<SelectListItem>();
            marital_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            marital_item.Add(new SelectListItem() { Text = "Married", Value = "Married" });
            marital_item.Add(new SelectListItem() { Text = "Unmarried", Value = "Unmarried" });
            //marital_item.Add(new SelectListItem() { Text = "Divorce", Value = "Divorce" });
            //marital_item.Add(new SelectListItem() { Text = "Widow", Value = "Widow" });
            marital_item.Add(new SelectListItem() { Text = "Single", Value = "Single" });
            model.MaritalStatusList = marital_item;
            var memCat_item = new List<SelectListItem>();
            if (LoggedInOrganizationID == 54)
            {
                memCat_item.Add(new SelectListItem() { Text = "Member", Value = "1", Selected = true });
            }
            else
            {
                memCat_item.Add(new SelectListItem() { Text = "Member", Value = "1", Selected = true });
                memCat_item.Add(new SelectListItem() { Text = "Member", Value = "1", Selected = true });
                memCat_item.Add(new SelectListItem() { Text = "Depositor", Value = "2" });
                memCat_item.Add(new SelectListItem() { Text = "Family", Value = "3" });
                memCat_item.Add(new SelectListItem() { Text = "Others", Value = "4" });
                memCat_item.Add(new SelectListItem() { Text = "Dormant", Value = "5" });

            }

            model.MemCategoryList = memCat_item;

        }




        // FOR BURO 

        public ActionResult IndexBuro()
        {
            var model = new MemberViewModel();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }
            return View();
        }

        public ActionResult CreateBuro()
        {
            //var model = new BuroMemberViewModel();
            var model = new BuroMemberViewModel();

            BuroMapDropDownList(model);
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }
            model.ServerCurrentDate = DateTime.Now;
            var blnk_items = new List<SelectListItem>();
            model.CenterList = blnk_items;
            model.GroupList = blnk_items;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;

            return View(model);
        }

        // POST: Member/CreateCIB
        [HttpPost]
        public JsonResult CreateBuro(BuroMemberViewModel model, string MemCode, string base64image, string base64imageFingerThumb)
        {
            try
            {

                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        var myerr = error.ErrorMessage;
                    }
                }

                var entity = Mapper.Map<BuroMemberViewModel, Member>(model);
                if (ModelState.IsValid)
                {
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                    var errors = memberService.IsValidMember(entity);
                    //{
                    if (errors.ToList().Count == 0)
                    {
                        if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != "") && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                        {
                            var t = base64image.Substring(22);  // remove data:image/png;base64,
                            byte[] bytes = Convert.FromBase64String(t);
                            Image image;
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                image = Image.FromStream(ms);
                            }
                            var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                            //var fullPath = Path.Combine(Server.MapPath("~/CapturedImages/"), randomFileName);
                            //image.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
                            entity.MemberImg = bytes;

                            ///////////////////////////

                            var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                            byte[] bytesthumb = Convert.FromBase64String(tthumb);
                            Image imagethumb;
                            using (MemoryStream msa = new MemoryStream(bytesthumb))
                            {
                                imagethumb = Image.FromStream(msa);
                            }
                            var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                            //var fullPaththumb = Path.Combine(Server.MapPath("~/CapturedImages/"), randomFileNamethumb);
                            //imagethumb.Save(fullPaththumb, System.Drawing.Imaging.ImageFormat.Png);
                            entity.ThumbImg = bytesthumb;
                        }
                        else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != ""))
                        {
                            var t = base64image.Substring(22);  // remove data:image/png;base64,
                            byte[] bytes = Convert.FromBase64String(t);
                            Image image;
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                image = Image.FromStream(ms);
                            }
                            var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                            entity.MemberImg = bytes;
                        }
                        else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                        {
                            var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                            byte[] bytesthumb = Convert.FromBase64String(tthumb);
                            Image imagethumb;
                            using (MemoryStream msa = new MemoryStream(bytesthumb))
                            {
                                imagethumb = Image.FromStream(msa);
                            }
                            var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                            entity.ThumbImg = bytesthumb;
                        }
                        else if (model.ImgFile == null && Session["CapturedImage"] != null)
                        {
                            var path = Server.MapPath("~/CapturedImages/");

                            var FileLocation = path + Session["CapturedImage"].ToString();
                            System.IO.File.Exists(FileLocation);
                            System.IO.FileInfo file = new System.IO.FileInfo(FileLocation);

                            using (System.Drawing.Image image = System.Drawing.Image.FromFile(file.FullName))
                            {
                                //Image image = System.Drawing.Image.FromFile(file.FullName);
                                ImageConverter converter = new ImageConverter();
                                byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                                entity.MemberImg = data;


                                decimal size = Math.Round(((decimal)entity.MemberImg.Length / (decimal)1024), 2);
                                if (size > 100)
                                {
                                    errors = memberService.CheckImageSize();
                                    Response.StatusCode = 400;
                                    //("File size must not exceed 100 KB.");
                                    //CustomValidator1.ErrorMessage = "File size must not exceed 100 KB.";
                                    //errors("File size must not exceed 100 KB.");
                                    // e.IsValid = false;
                                }
                            }


                            Array.ForEach(Directory.GetFiles(path), System.IO.File.Delete);

                        }
                        else if (model.ImgFile != null)
                        {

                            decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                            if (size > 100)
                            {
                                //NEW ADD NEW ADD KHALID 19 NOV,2019  Validate Image 100 KB
                                errors = memberService.CheckImageSize();
                                Response.StatusCode = 400;
                                //("File size must not exceed 100 KB.");
                                //CustomValidator1.ErrorMessage = "File size must not exceed 100 KB.";
                                //errors("File size must not exceed 100 KB.");
                                // e.IsValid = false;
                            }


                            byte[] data = new byte[model.ImgFile.ContentLength];
                            if (data != null)
                            {
                                model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                                entity.MemberImg = data;
                            }
                        }

                        if (model.ThumbImgFile != null)
                        {
                            decimal size = Math.Round(((decimal)model.ThumbImgFile.ContentLength / (decimal)1024), 2);
                            if (size > 100)
                            {
                                errors = memberService.CheckImageSize();
                                Response.StatusCode = 400;
                            }

                            byte[] data = new byte[model.ThumbImgFile.ContentLength];
                            if (data != null)
                            {
                                model.ThumbImgFile.InputStream.Read(data, 0, model.ThumbImgFile.ContentLength);
                                entity.ThumbImg = data;
                            }
                        }



                        if (model.DivisionCode != null)
                        {
                            if (model.DivisionCode != "0")
                            {
                                entity.DivisionCode = model.DivisionCode;
                            }
                            else
                            {
                                entity.DivisionCode = "";
                            }
                        }
                        if (model.DistrictCode != null)
                        {
                            if (model.DistrictCode != "0")
                            {
                                entity.DistrictCode = model.DistrictCode;
                            }
                            else
                            {
                                entity.DistrictCode = "";
                            }
                        }
                        if (model.UpozillaCode != null)
                        {
                            if (model.UpozillaCode != "0")
                            {
                                entity.UpozillaCode = model.UpozillaCode;
                            }
                            else
                            {
                                entity.UpozillaCode = "";
                            }
                        }
                        if (model.UnionCode != null)
                        {
                            if (model.UnionCode != "0")
                            {
                                entity.UnionCode = model.UnionCode;
                            }
                            else
                            {
                                entity.UnionCode = "";
                            }
                        }
                        if (model.VillageCode != null)
                        {
                            if (model.VillageCode != "0")
                            {
                                entity.VillageCode = model.VillageCode;
                            }
                            else
                            {
                                entity.VillageCode = "";
                            }
                        }
                        if (model.PlaceOfBirth != null)
                        {
                            entity.PlaceOfBirth = model.PlaceOfBirth;

                        }
                        if (model.TotalWealth != null)
                        {
                            entity.TotalWealth = model.TotalWealth;

                        }
                        if (model.MotherName != null)
                        {
                            entity.MotherName = model.MotherName;

                        }
                        if (model.FatherName != null)
                        {
                            entity.FatherName = model.FatherName;

                        }
                        if (model.NationalID != null)
                        {
                            entity.NationalID = model.NationalID;

                        }
                        if (model.PhoneNo != null)
                        {
                            entity.PhoneNo = model.PhoneNo;

                        }
                        //var mem = memberService.CheckMemberNationalId(entity.NationalID);
                        //if (mem.ToList().Count > 0)
                        //{
                        //    return GetErrorMessageResult("NationalID Already Exists");
                        //}
                        //HttpClient Client = new HttpClient();
                        //Client.BaseAddress = new Uri("http://192.192.192.233:9099/"); //api/nonmaskingsms/easysend?sender=01713140127&message=hello%20bangladesh)
                        //Client.DefaultRequestHeaders.Clear();
                        //Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        ////Client.DefaultRequestHeaders.Add("Accept", "application/json");
                        ////Client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                        //HttpResponseMessage message = Client.GetAsync($"buro/api/NationalID/{model.NationalID}").Result;
                        //if (message.IsSuccessStatusCode)
                        //{
                        //    var jdata = message.Content.ReadAsAsync<MemberNationalIDViewModel>();
                        //    if (jdata.Result!=null)
                        //    {
                        //        return GetErrorMessageResult("NationalID Already Exists(IMFASDATA)");
                        //    }
                        //}

                        //var memPhone = memberService.CheckMemberPhoneNo(entity.PhoneNo);
                        //if (memPhone.ToList().Count > 0)
                        //{
                        //    return GetErrorMessageResult("PhoneNo Already Exists");
                        //}
                        entity.IsActive = true;
                        entity.MemberStatus = "0";
                        entity.PwdStatus = "D";
                        entity.MemberType = 1;

                        var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                        var allProducts = accReportService.GetLastInitialDate(param);

                        var detail = allProducts.ToString();

                        if (!IsDayInitiated)
                        {
                            if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                            {
                                entity.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                            }
                        }
                        else
                        {
                            entity.JoinDate = TransactionDate;
                        }

                        var param1 = new { @OfficeID = LoginUserOfficeID };
                        var LoanInstallMent = ultimateReportService.GenerateMemberLastCode(param1);
                        entity.MemberCode = LoanInstallMent.Tables[0].Rows[0]["LastCode"].ToString();


                        memberService.Create(entity);
                        var ent = new { MemberID = entity.MemberID, MemberCode = entity.MemberCode };
                        return Json(new { data = ent }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return GetErrorMessageResult(errors);
                }
                else
                    return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: Member/Edit/5
        public ActionResult EditBuro(int id)
        {
            var member = memberService.GetById(id);
            //var memberModel = Mapper.Map<Member, BuroMemberViewModel>(member);
            var memberModel = Mapper.Map<Member, BuroMemberViewModel>(member);

            //            memberModel.OfficeID = 2;
            BuroMapDropDownList(memberModel);
            var allCenter = centerService.GetByOfficeId(memberModel.OfficeID, Convert.ToInt16(LoggedInOrganizationID));
            var viewCenter = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + "-" + x.CenterName.ToString()
            });
            memberModel.CenterList = viewCenter;
            var allGroup = groupService.GetByOfficeId(memberModel.OfficeID);
            var viewGroup = allGroup.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GroupID.ToString(),
                Text = x.GroupCode.ToString()
            });
            memberModel.GroupList = viewGroup;
            memberModel.GroupID = member.GroupID;
            memberModel.Gender = member.Gender;
            memberModel.Location = member.Location;
            memberModel.PlaceOfBirth = member.PlaceOfBirth;
            memberModel.ServerCurrentDate = DateTime.Now;



            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;

            return View(memberModel);
        }
        // POST: Member/Edit/5
        [HttpPost]
        public ActionResult EditBuro(BuroMemberViewModel model, string base64image, string base64imageFingerThumb)
        {
            try
            {
                var entity = memberService.GetByIdLong(model.MemberID);

                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        var myerr = error.ErrorMessage;
                    }
                }

                if (ModelState.IsValid)
                {
                    if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != "") && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                    {
                        var t = base64image.Substring(22);  // remove data:image/png;base64,
                        byte[] bytes = Convert.FromBase64String(t);
                        Image image;
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            image = Image.FromStream(ms);
                        }
                        var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                        entity.MemberImg = bytes;

                        ///////////////////////////

                        var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                        byte[] bytesthumb = Convert.FromBase64String(tthumb);
                        Image imagethumb;
                        using (MemoryStream msa = new MemoryStream(bytesthumb))
                        {
                            imagethumb = Image.FromStream(msa);
                        }
                        var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                        entity.ThumbImg = bytesthumb;
                    }
                    else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != ""))
                    {
                        var t = base64image.Substring(22);  // remove data:image/png;base64,
                        byte[] bytes = Convert.FromBase64String(t);
                        Image image;
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            image = Image.FromStream(ms);
                        }
                        var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                        //byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                        entity.MemberImg = bytes;
                    }
                    else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                    {
                        var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                        byte[] bytesthumb = Convert.FromBase64String(tthumb);
                        Image imagethumb;
                        using (MemoryStream msa = new MemoryStream(bytesthumb))
                        {
                            imagethumb = Image.FromStream(msa);
                        }
                        var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                        entity.ThumbImg = bytesthumb;
                    }
                    else if (model.ImgFile == null && Session["CapturedImage"] != null)
                    {
                        var path = Server.MapPath("~/CapturedImages/");

                        var FileLocation = path + Session["CapturedImage"].ToString();
                        System.IO.File.Exists(FileLocation);
                        System.IO.FileInfo file = new System.IO.FileInfo(FileLocation);

                        using (System.Drawing.Image image = System.Drawing.Image.FromFile(file.FullName))
                        {
                            //Image image = System.Drawing.Image.FromFile(file.FullName);
                            ImageConverter converter = new ImageConverter();
                            byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                            entity.MemberImg = data;

                            decimal size = Math.Round(((decimal)entity.MemberImg.Length / (decimal)1024), 2);
                            if (size > 100)
                            {
                                var errors = memberService.CheckImageSize();
                                Response.StatusCode = 400;
                                //("File size must not exceed 100 KB.");
                                //CustomValidator1.ErrorMessage = "File size must not exceed 100 KB.";
                                //errors("File size must not exceed 100 KB.");
                                // e.IsValid = false;
                            }

                        }

                        Array.ForEach(Directory.GetFiles(path), System.IO.File.Delete);

                    }
                    else if (model.ImgFile != null)
                    {
                        decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                        if (size > 100)
                        {
                            var error = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                        }

                        byte[] data = new byte[model.ImgFile.ContentLength];
                        if (data != null)
                        {
                            decimal size2 = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                            if (size2 > 100)
                            {
                                var error = memberService.CheckImageSize();
                                Response.StatusCode = 400;
                            }


                            model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                            entity.MemberImg = data;
                        }
                    }

                    if (model.ThumbImgFile != null)
                    {
                        decimal size2 = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                        if (size2 > 100)
                        {
                            var error = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                        }

                        byte[] data = new byte[model.ThumbImgFile.ContentLength];
                        if (data != null)
                        {
                            model.ThumbImgFile.InputStream.Read(data, 0, model.ThumbImgFile.ContentLength);
                            entity.ThumbImg = data;
                        }
                    }

                    if (entity.OfficeID != LoginUserOfficeID)
                    {
                        return GetErrorMessageResult("Invalid Office...........");
                    }
                    entity.GroupID = model.GroupID;
                    entity.FirstName = model.FirstName;
                    entity.MiddleName = model.MiddleName;
                    entity.LastName = model.LastName;
                    entity.AddressLine1 = model.AddressLine1;
                    entity.AddressLine2 = model.AddressLine2;
                    entity.RefereeName = model.RefereeName;
                    entity.BirthDate = model.BirthDate;
                    var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                    var allProducts = accReportService.GetLastInitialDate(param);

                    entity.NationalID = model.NationalID;

                    entity.Email = model.Email;
                    entity.PhoneNo = model.PhoneNo;
                    entity.MaritalStatus = model.MaritalStatus;
                    entity.MemCategory = model.MemCategory;

                    // NEW ADD KHALID

                    entity.AddressLine1 = model.AddressLine1;
                    entity.AddressLine2 = model.AddressLine2;
                    entity.CountryID = model.CountryID;
                    entity.DivisionCode = model.DivisionCode;
                    entity.DistrictCode = model.DistrictCode;
                    entity.UpozillaCode = model.UpozillaCode;
                    entity.UnionCode = model.UnionCode;
                    entity.VillageCode = model.VillageCode;
                    entity.ZipCode = model.ZipCode;



                    entity.IsAnyFS = model.IsAnyFS;
                    entity.FServiceName = model.FServiceName;
                    entity.FinServiceChoiceId = model.FinServiceChoiceId;
                    entity.TransactionChoiceId = model.TransactionChoiceId;

                    entity.BanglaName = model.BanglaName;
                    entity.PerAddressLine1 = model.PerAddressLine1;
                    entity.PerAddressLine2 = model.PerAddressLine2;
                    entity.PerCountryID = model.PerCountryID;
                    entity.PerDivisionCode = model.PerDivisionCode;
                    entity.PerDistrictCode = model.PerDistrictCode;
                    entity.PerUpozillaCode = model.PerUpozillaCode;
                    entity.PerUnionCode = model.PerUnionCode;
                    entity.PerVillageCode = model.PerVillageCode;
                    entity.PerZipCode = model.PerZipCode;
                    entity.FatherNameBN = model.FatherNameBN;
                    entity.MotherNameBN = model.MotherNameBN;
                    entity.IdentTypeID = model.IdentTypeID;
                    entity.ExpireDate = model.ExpireDate;
                    entity.ProvidedByCountryID = model.ProvidedByCountryID;
                    entity.SpouseName = model.SpouseName;
                    entity.SpouseNameBN = model.SpouseNameBN;
                    entity.TIN = model.TIN;
                    entity.TaxAmount = model.TaxAmount;
                    entity.AsOnDateAge = model.AsOnDateAge;
                    entity.FamilyContactNo = model.FamilyContactNo;
                    entity.CardIssueDate = model.CardIssueDate;


                    // END

                    if (model.DivisionCode != null)
                    {
                        if (model.DivisionCode != "0")
                        {
                            entity.DivisionCode = model.DivisionCode;
                        }
                        else
                        {
                            entity.DivisionCode = "";
                        }
                    }
                    if (model.DistrictCode != null)
                    {
                        if (model.DistrictCode != "0")
                        {
                            entity.DistrictCode = model.DistrictCode;
                        }
                        else
                        {
                            entity.DistrictCode = "";
                        }
                    }
                    if (model.UpozillaCode != null)
                    {
                        if (model.UpozillaCode != "0")
                        {
                            entity.UpozillaCode = model.UpozillaCode;
                        }
                        else
                        {
                            entity.UpozillaCode = "";
                        }
                    }
                    if (model.UnionCode != null)
                    {
                        if (model.UnionCode != "0")
                        {
                            entity.UnionCode = model.UnionCode;
                        }
                        else
                        {
                            entity.UnionCode = "";
                        }
                    }
                    if (model.VillageCode != null)
                    {
                        if (model.VillageCode != "0")
                        {
                            entity.VillageCode = model.VillageCode;
                        }
                        else
                        {
                            entity.VillageCode = "";
                        }
                    }


                    if (model.PerDivisionCode != null)
                    {
                        if (model.PerDivisionCode != "0")
                        {
                            entity.PerDivisionCode = model.PerDivisionCode;
                        }
                        else
                        {
                            entity.PerDivisionCode = "";
                        }
                    }
                    if (model.PerDistrictCode != null)
                    {
                        if (model.PerDistrictCode != "0")
                        {
                            entity.PerDistrictCode = model.PerDistrictCode;
                        }
                        else
                        {
                            entity.PerDistrictCode = "";
                        }
                    }
                    if (model.PerUpozillaCode != null)
                    {
                        if (model.PerUpozillaCode != "0")
                        {
                            entity.PerUpozillaCode = model.PerUpozillaCode;
                        }
                        else
                        {
                            entity.PerUpozillaCode = "";
                        }
                    }
                    if (model.PerUnionCode != null)
                    {
                        if (model.PerUnionCode != "0")
                        {
                            entity.PerUnionCode = model.PerUnionCode;
                        }
                        else
                        {
                            entity.PerUnionCode = "";
                        }
                    }
                    if (model.PerVillageCode != null)
                    {
                        if (model.PerVillageCode != "0")
                        {
                            entity.PerVillageCode = model.PerVillageCode;
                        }
                        else
                        {
                            entity.PerVillageCode = "";
                        }
                    }

                    if (model.PlaceOfBirth != null)
                    {
                        entity.PlaceOfBirth = model.PlaceOfBirth;

                    }
                    if (model.TotalWealth != null)
                    {
                        entity.TotalWealth = model.TotalWealth;

                    }
                    if (model.MotherName != null)
                    {
                        entity.MotherName = model.MotherName;

                    }
                    if (model.FatherName != null)
                    {
                        entity.FatherName = model.FatherName;

                    }
                    if (model.NationalID != null)
                    {
                        entity.NationalID = model.NationalID;

                    }
                    //var mem = memberService.CheckMemberNationalIdEdit(entity.NationalID, entity.MemberID);
                    //if (mem.ToList().Count > 0)
                    //{
                    //    return GetErrorMessageResult("NationalID Already Exists");
                    //}
                    if (model.PhoneNo != null)
                    {
                        entity.PhoneNo = model.PhoneNo;

                    }
                    //var memPhone = memberService.CheckMemberPhoneNoEdit(entity.PhoneNo, entity.MemberID);
                    //if (memPhone.ToList().Count > 0)
                    //{
                    //    return GetErrorMessageResult("PhoneNo Already Exists");
                    //}

                    memberService.Update(entity);
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

        private void BuroMapDropDownList(BuroMemberViewModel model) //BuroMemberViewModel model
        {
            //List<BuroMemberViewModel> List_MemberViewModel = new List<BuroMemberViewModel>();
            List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();

            var param = new { SearchByCode = "", SearchBy = "", SearchType = "dis" };
            var disList = ultimateReportService.GetLocationList(param);

            List_MemberViewModel = disList.Tables[0].AsEnumerable()
            .Select(row => new MemberViewModel
            {
                DistrictCode = row.Field<string>("DistrictCode"),
                DistrictName = row.Field<string>("DistrictName")
            }).ToList();
            var viewDist = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.DistrictCode.ToString(),
                Text = x.DistrictName.ToString()
            });
            var pob_items = new List<SelectListItem>();
            pob_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            pob_items.AddRange(viewDist);
            model.PlaceOfBirthList = pob_items;

            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OfficeID == offc_id && m.OrgID == LoggedInOrganizationID).ToList();
            // var allOffice = officeService.GetById(offc_id);
            var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            //var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.OfficeList = ofc_items;

            var allMemcatService = memberCategoryService.GetAll().Where(w => w.IsActive == true && w.OrgID == LoggedInOrganizationID).OrderBy(o => o.MemberCategoryCode).ToList();
            var viewMemCatService = allMemcatService.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MemberCategoryID.ToString(),
                Text = string.Format("{0}, {1}", x.MemberCategoryCode.ToString(), x.CategoryName.ToString())
            });
            var cat_items = new List<SelectListItem>();
            cat_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            cat_items.AddRange(viewMemCatService);
            model.MemberCategoryList = cat_items;


            var allGeoLocation = geoLocationService.GetAll();
            var viewGeoLocation = allGeoLocation.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GeoLocationID.ToString(),
                Text = x.LocationName.ToString()
            });
            var geo_items = new List<SelectListItem>();
            geo_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            geo_items.AddRange(viewGeoLocation);
            model.GeoLocationList = geo_items;

            var allCountry = countryService.GetAll();
            var viewCountry = allCountry.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString(),
                Selected = x.CountryId.ToString() == "18" ? true : false
            });
            var country_items = new List<SelectListItem>();
            country_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            country_items.AddRange(viewCountry);
            model.CountryList = country_items;

            //blank division
            var divisionList = new List<SelectListItem>();
            divisionList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.DivisionList = divisionList;

            //blank district
            var districtList = new List<SelectListItem>();
            districtList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.DistrictList = districtList;

            //blank upozilla
            var upozillaList = new List<SelectListItem>();
            upozillaList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.UpozillaList = upozillaList;

            //blank Union
            var unionList = new List<SelectListItem>();
            unionList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.UnionList = unionList;

            //blank Village
            var villageList = new List<SelectListItem>();
            villageList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.VillageList = villageList;

            var gender_item = new List<SelectListItem>();
            gender_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            gender_item.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            gender_item.Add(new SelectListItem() { Text = "Female", Value = "Female" });
            gender_item.Add(new SelectListItem() { Text = "Transgender", Value = "T" });
            model.GenderList = gender_item;

            var status_item = new List<SelectListItem>();
            status_item.Add(new SelectListItem() { Text = "Active", Value = "1", Selected = true });
            status_item.Add(new SelectListItem() { Text = "In Active", Value = "0" });
            status_item.Add(new SelectListItem() { Text = "Drop", Value = "2" });
            status_item.Add(new SelectListItem() { Text = "Dead", Value = "3" });
            status_item.Add(new SelectListItem() { Text = "Blacklist", Value = "4" });
            status_item.Add(new SelectListItem() { Text = "Rejected", Value = "5" });
            model.MemberStatusList = status_item;

            var cityzen_item = new List<SelectListItem>();
            cityzen_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            cityzen_item.Add(new SelectListItem() { Text = "By Birth", Value = "BB" });
            cityzen_item.Add(new SelectListItem() { Text = "Migrated", Value = "MI" });
            cityzen_item.Add(new SelectListItem() { Text = "Marital", Value = "MA" });
            cityzen_item.Add(new SelectListItem() { Text = "Nutralization", Value = "NU" });
            model.CityzenshipList = cityzen_item;

            var homeType_item = new List<SelectListItem>();
            homeType_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            homeType_item.Add(new SelectListItem() { Text = "Building", Value = "BU" });
            homeType_item.Add(new SelectListItem() { Text = "Muddy", Value = "MU" });
            homeType_item.Add(new SelectListItem() { Text = "Rented", Value = "RE" });
            homeType_item.Add(new SelectListItem() { Text = "Semi Building", Value = "SB" });
            homeType_item.Add(new SelectListItem() { Text = "Tin Shade", Value = "TN" });
            model.HomeTypeList = homeType_item;

            var groupType_item = new List<SelectListItem>();
            groupType_item.Add(new SelectListItem() { Text = "Solidarity", Value = "SO", Selected = true });
            groupType_item.Add(new SelectListItem() { Text = "Non Solidarity", Value = "NS" });
            groupType_item.Add(new SelectListItem() { Text = "Individual", Value = "IN" });
            groupType_item.Add(new SelectListItem() { Text = "Corporate", Value = "CO" });
            model.GroupTypeList = groupType_item;

            var education_item = new List<SelectListItem>();
            education_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            education_item.Add(new SelectListItem() { Text = "Under Matric", Value = "UMA" });
            education_item.Add(new SelectListItem() { Text = "Pre-Primary", Value = "1" });
            education_item.Add(new SelectListItem() { Text = "Primary", Value = "2" });
            education_item.Add(new SelectListItem() { Text = "JSC", Value = "JSC" });
            education_item.Add(new SelectListItem() { Text = "Secondary", Value = "3" });
            education_item.Add(new SelectListItem() { Text = "Higher Secondary", Value = "4" });
            education_item.Add(new SelectListItem() { Text = "Diploma", Value = "DIP" });
            education_item.Add(new SelectListItem() { Text = "Graduate", Value = "5" });
            education_item.Add(new SelectListItem() { Text = "PostGraduate", Value = "6" });
            education_item.Add(new SelectListItem() { Text = "Illiterate", Value = "ILL" });
            education_item.Add(new SelectListItem() { Text = "Other", Value = "7" });
            model.EducationList = education_item;

            var economic_item = new List<SelectListItem>();
            economic_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            economic_item.Add(new SelectListItem() { Text = "Business", Value = "BU" });
            economic_item.Add(new SelectListItem() { Text = "House Hold", Value = "HH" });
            economic_item.Add(new SelectListItem() { Text = "Service", Value = "SE" });
            economic_item.Add(new SelectListItem() { Text = "Farmer", Value = "FR" });
            model.EconomicActivityList = economic_item;

            var marital_item = new List<SelectListItem>();
            marital_item.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });

            marital_item.Add(new SelectListItem() { Text = "Married", Value = "Married" });
            marital_item.Add(new SelectListItem() { Text = "Unmarried", Value = "Unmarried" });
            //marital_item.Add(new SelectListItem() { Text = "Divorce", Value = "Divorce" });
            //marital_item.Add(new SelectListItem() { Text = "Widow", Value = "Widow" });
            marital_item.Add(new SelectListItem() { Text = "Single", Value = "Single" });
            model.MaritalStatusList = marital_item;
            var memCat_item = new List<SelectListItem>();
            if (LoggedInOrganizationID == 54)
            {
                memCat_item.Add(new SelectListItem() { Text = "Member", Value = "1", Selected = true });
            }
            else
            {
                memCat_item.Add(new SelectListItem() { Text = "Member", Value = "1", Selected = true });
                memCat_item.Add(new SelectListItem() { Text = "Member", Value = "1", Selected = true });
                memCat_item.Add(new SelectListItem() { Text = "Depositor", Value = "2" });
                memCat_item.Add(new SelectListItem() { Text = "Family", Value = "3" });
                memCat_item.Add(new SelectListItem() { Text = "Others", Value = "4" });
                memCat_item.Add(new SelectListItem() { Text = "Dormant", Value = "5" });

            }

            model.MemCategoryList = memCat_item;

        }

        //END BUO


        public ActionResult TestIframe()
        {
            var model = new MemberViewModel();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }
            return View();
        }


        //public ActionResult Edit(MemberEditViewModel model)
        //{
        //    try
        //    {
        //        var entity = memberService.GetByIdLong(model.MemberID);
        //        if (ModelState.IsValid)
        //        {

        //            if (model.ImgFile == null && Session["CapturedImage"] != null)
        //            {
        //                var path = Server.MapPath("~/CapturedImages/");

        //                var FileLocation = path + Session["CapturedImage"].ToString();
        //                System.IO.File.Exists(FileLocation);
        //                System.IO.FileInfo file = new System.IO.FileInfo(FileLocation);

        //                using (System.Drawing.Image image = System.Drawing.Image.FromFile(file.FullName))
        //                {
        //                    //Image image = System.Drawing.Image.FromFile(file.FullName);
        //                    ImageConverter converter = new ImageConverter();
        //                    byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
        //                    entity.MemberImg = data;

        //                    decimal size = Math.Round(((decimal)entity.MemberImg.Length / (decimal)1024), 2);
        //                    if (size > 100)
        //                    {
        //                        var errors = memberService.CheckImageSize();
        //                        Response.StatusCode = 400;
        //                        //("File size must not exceed 100 KB.");
        //                        //CustomValidator1.ErrorMessage = "File size must not exceed 100 KB.";
        //                        //errors("File size must not exceed 100 KB.");
        //                        // e.IsValid = false;
        //                    }

        //                }

        //                Array.ForEach(Directory.GetFiles(path), System.IO.File.Delete);

        //            }
        //            else if (model.ImgFile != null)
        //            {
        //                decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
        //                if (size > 100)
        //                {
        //                    var error = memberService.CheckImageSize();
        //                    Response.StatusCode = 400;
        //                }

        //                byte[] data = new byte[model.ImgFile.ContentLength];
        //                if (data != null)
        //                {
        //                    decimal size2 = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
        //                    if (size2 > 100)
        //                    {
        //                        var error = memberService.CheckImageSize();
        //                        Response.StatusCode = 400;
        //                    }


        //                    model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
        //                    entity.MemberImg = data;
        //                }
        //            }

        //            if (model.ThumbImgFile != null)
        //            {
        //                decimal size2 = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
        //                if (size2 > 100)
        //                {
        //                    var error = memberService.CheckImageSize();
        //                    Response.StatusCode = 400;
        //                }

        //                byte[] data = new byte[model.ThumbImgFile.ContentLength];
        //                if (data != null)
        //                {
        //                    model.ThumbImgFile.InputStream.Read(data, 0, model.ThumbImgFile.ContentLength);
        //                    entity.ThumbImg = data;
        //                }
        //            }

        //            if (entity.OfficeID != LoginUserOfficeID)
        //            {
        //                return GetErrorMessageResult("Invalid Office...........");
        //            }
        //            entity.GroupID = model.GroupID;
        //            entity.FirstName = model.FirstName;
        //            entity.MiddleName = model.MiddleName;
        //            entity.LastName = model.LastName;
        //            entity.AddressLine1 = model.AddressLine1;
        //            entity.AddressLine2 = model.AddressLine2;
        //            entity.RefereeName = model.RefereeName;
        //            entity.BirthDate = model.BirthDate;
        //            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
        //            var allProducts = accReportService.GetLastInitialDate(param);

        //            entity.NationalID = model.NationalID;

        //            entity.Email = model.Email;
        //            entity.PhoneNo = model.PhoneNo;
        //            entity.MaritalStatus = model.MaritalStatus;
        //            entity.MemCategory = model.MemCategory;
        //            if (model.DivisionCode != null)
        //            {
        //                if (model.DivisionCode != "0")
        //                {
        //                    entity.DivisionCode = model.DivisionCode;
        //                }
        //                else
        //                {
        //                    entity.DivisionCode = "";
        //                }
        //            }
        //            if (model.DistrictCode != null)
        //            {
        //                if (model.DistrictCode != "0")
        //                {
        //                    entity.DistrictCode = model.DistrictCode;
        //                }
        //                else
        //                {
        //                    entity.DistrictCode = "";
        //                }
        //            }
        //            if (model.UpozillaCode != null)
        //            {
        //                if (model.UpozillaCode != "0")
        //                {
        //                    entity.UpozillaCode = model.UpozillaCode;
        //                }
        //                else
        //                {
        //                    entity.UpozillaCode = "";
        //                }
        //            }
        //            if (model.UnionCode != null)
        //            {
        //                if (model.UnionCode != "0")
        //                {
        //                    entity.UnionCode = model.UnionCode;
        //                }
        //                else
        //                {
        //                    entity.UnionCode = "";
        //                }
        //            }
        //            if (model.VillageCode != null)
        //            {
        //                if (model.VillageCode != "0")
        //                {
        //                    entity.VillageCode = model.VillageCode;
        //                }
        //                else
        //                {
        //                    entity.VillageCode = "";
        //                }
        //            }
        //            if (model.PlaceOfBirth != null)
        //            {
        //                entity.PlaceOfBirth = model.PlaceOfBirth;

        //            }
        //            if (model.TotalWealth != null)
        //            {
        //                entity.TotalWealth = model.TotalWealth;

        //            }
        //            if (model.MotherName != null)
        //            {
        //                entity.MotherName = model.MotherName;

        //            }
        //            if (model.FatherName != null)
        //            {
        //                entity.FatherName = model.FatherName;

        //            }
        //            if (model.NationalID != null)
        //            {
        //                entity.NationalID = model.NationalID;

        //            }
        //            var mem = memberService.CheckMemberNationalIdEdit(entity.NationalID,entity.MemberID);
        //            if (mem.ToList().Count > 0)
        //            {
        //                return GetErrorMessageResult("NationalID Already Exists");
        //            }
        //            if (model.PhoneNo != null)
        //            {
        //                entity.PhoneNo = model.PhoneNo;

        //            }
        //            var memPhone = memberService.CheckMemberPhoneNoEdit(entity.PhoneNo,entity.MemberID);
        //            if (memPhone.ToList().Count > 0)
        //            {
        //                return GetErrorMessageResult("PhoneNo Already Exists");
        //            }

        //            memberService.Update(entity);
        //            return GetSuccessMessageResult();
        //        }
        //        else
        //            return GetErrorMessageResult();
        //    }
        //    catch (Exception ex)
        //    {
        //        return GetErrorMessageResult(ex);
        //    }
        //}

        // GET: Member/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: Member/Delete/5
        [HttpPost]
        public ActionResult Delete(MemberViewModel model)
        {
            try
            {
                var entity = memberService.GetByIdLong(model.MemberID);
                if (ModelState.IsValid)
                {
                    entity.IsActive = false;
                    entity.InActiveDate = DateTime.Now;
                    memberService.Update(entity);
                }
                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        //===========================================================================================//
        //============================= Mostak Ahammed (24 Aug 2023) ================================//
        //===========================================================================================//

        //---------------------- Index ----------------------------//

        public ActionResult IndexRDRS()
        {
            var model = new MemberViewModel();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }

            ViewData["LoggedInOrg"] = SessionHelper.LoginUserOrganizationID;

            return View();
        }

        //------------------- Create ------------------------------//
        public ActionResult CreateRDRS()
        {
            var model = new MemberViewModel();
            MapDropDownList(model);
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }
            model.ServerCurrentDate = DateTime.Now;
            var blnk_items = new List<SelectListItem>();
            model.CenterList = blnk_items;
            model.GroupList = blnk_items;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;

            var OrgInfo = organizationService.GetById((int)SessionHelper.LoginUserOrganizationID);
            //var v = OrgInfo.MemberAge;
            var MemberAge = 60;

            if (OrgInfo.MemberAge == null)
            {

            }
            else
            {
                MemberAge = (int)OrgInfo.MemberAge;
            }
            ViewData["MemberAge"] = MemberAge;

            ViewData["LoggedInOrg"] = SessionHelper.LoginUserOrganizationID;
            ViewBag.OrgId = LoggedInOrganizationID;
            return View(model);
        }

        [HttpPost]
        public JsonResult CreateRDRS(MemberViewModel model, string MemCode, string base64image, string base64imageFingerThumb)
        {
            try
            {
                var entity = Mapper.Map<MemberViewModel, Member>(model);
                if (ModelState.IsValid)
                {
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                    var errors = memberService.IsValidMember(entity);

                    if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != "") && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                    {
                        var t = base64image.Substring(22);  // remove data:image/png;base64,
                        byte[] bytes = Convert.FromBase64String(t);
                        Image image;
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            image = Image.FromStream(ms);
                        }
                        var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                        entity.MemberImg = bytes;

                        var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                        byte[] bytesthumb = Convert.FromBase64String(tthumb);
                        Image imagethumb;
                        using (MemoryStream msa = new MemoryStream(bytesthumb))
                        {
                            imagethumb = Image.FromStream(msa);
                        }
                        var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                        entity.ThumbImg = bytesthumb;
                    }
                    else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != ""))
                    {
                        var t = base64image.Substring(22);  // remove data:image/png;base64,
                        byte[] bytes = Convert.FromBase64String(t);
                        Image image;
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            image = Image.FromStream(ms);
                        }
                        var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                        entity.MemberImg = bytes;
                    }
                    else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                    {
                        var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                        byte[] bytesthumb = Convert.FromBase64String(tthumb);
                        Image imagethumb;
                        using (MemoryStream msa = new MemoryStream(bytesthumb))
                        {
                            imagethumb = Image.FromStream(msa);
                        }
                        var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                        entity.ThumbImg = bytesthumb;
                    }
                    else if (model.ImgFile == null && Session["CapturedImage"] != null)
                    {
                        var path = Server.MapPath("~/CapturedImages/");

                        var FileLocation = path + Session["CapturedImage"].ToString();
                        System.IO.File.Exists(FileLocation);
                        System.IO.FileInfo file = new System.IO.FileInfo(FileLocation);

                        using (System.Drawing.Image image = System.Drawing.Image.FromFile(file.FullName))
                        {
                            ImageConverter converter = new ImageConverter();
                            byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                            entity.MemberImg = data;


                            decimal size = Math.Round(((decimal)entity.MemberImg.Length / (decimal)1024), 2);
                            if (size > 100)
                            {
                                errors = memberService.CheckImageSize();
                                Response.StatusCode = 400;
                            }
                        }
                        Array.ForEach(Directory.GetFiles(path), System.IO.File.Delete);

                    }
                    else if (model.ImgFile != null)
                    {

                        decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                        if (size > 100)
                        {
                            errors = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                            throw new System.InvalidOperationException("Image File is Big than 100 KB.");
                        }

                        byte[] data = new byte[model.ImgFile.ContentLength];
                        if (data != null)
                        {
                            model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                            entity.MemberImg = data;
                        }
                    }

                    if (model.ThumbImgFile != null)
                    {
                        decimal size = Math.Round(((decimal)model.ThumbImgFile.ContentLength / (decimal)1024), 2);
                        if (size > 100)
                        {
                            errors = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                            throw new System.InvalidOperationException("Image File is Big than 100 KB.");
                        }

                        byte[] data = new byte[model.ThumbImgFile.ContentLength];
                        if (data != null)
                        {
                            model.ThumbImgFile.InputStream.Read(data, 0, model.ThumbImgFile.ContentLength);
                            entity.ThumbImg = data;
                        }
                    }


                    if (model.EmployeeID != null)
                    {
                        entity.EmployeeID = model.EmployeeID;
                    }

                    if (model.DivisionCode != null)
                    {
                        if (model.DivisionCode != "0")
                        {
                            entity.DivisionCode = model.DivisionCode;
                        }
                        else
                        {
                            entity.DivisionCode = "";
                        }
                    }
                    if (model.DistrictCode != null)
                    {
                        if (model.DistrictCode != "0")
                        {
                            entity.DistrictCode = model.DistrictCode;
                        }
                        else
                        {
                            entity.DistrictCode = "";
                        }
                    }
                    if (model.UpozillaCode != null)
                    {
                        if (model.UpozillaCode != "0")
                        {
                            entity.UpozillaCode = model.UpozillaCode;
                        }
                        else
                        {
                            entity.UpozillaCode = "";
                        }
                    }
                    if (model.UnionCode != null)
                    {
                        if (model.UnionCode != "0")
                        {
                            entity.UnionCode = model.UnionCode;
                        }
                        else
                        {
                            entity.UnionCode = "";
                        }
                    }
                    if (model.VillageCode != null)
                    {
                        if (model.VillageCode != "0")
                        {
                            entity.VillageCode = model.VillageCode;
                        }
                        else
                        {
                            entity.VillageCode = "";
                        }
                    }
                    if (model.PlaceOfBirth != null)
                    {
                        entity.PlaceOfBirth = model.PlaceOfBirth;

                    }
                    if (model.TotalWealth != null)
                    {
                        entity.TotalWealth = model.TotalWealth;

                    }
                    if (model.MotherName != null)
                    {
                        entity.MotherName = model.MotherName;

                    }
                    if (model.ExpireDate != null)
                    {
                        entity.ExpireDate = model.ExpireDate;

                    }
                    if (model.FatherName != null)
                    {
                        entity.FatherName = model.FatherName;

                    }

                    if (model.NationalID != null)
                    {
                        entity.NationalID = model.NationalID;
                        if (LoggedInOrganizationID != 150)
                        {
                            if (entity.NationalID.Length != 17)
                            {
                                return GetErrorMessageResult("NationalID  No cann't be less than 17 digits");
                            }
                        }
                    }
                    if (model.NationalID == null)
                    {
                        entity.NationalID = "0";
                    }
                    if (model.SmartCard != null)
                    {
                        entity.SmartCard = model.SmartCard;
                        if (LoggedInOrganizationID != 150)
                        {
                            if (entity.SmartCard.Length != 10)
                            {
                                return GetErrorMessageResult("SmartCard No cann't be less than 10 digits");
                            }
                        }

                    }
                    if (model.PhoneNo != null)
                    {
                        entity.PhoneNo = model.PhoneNo;

                    }

                    if (LoggedInOrganizationID == 150)
                        entity.GroupID = 1;

                    entity.IsActive = true;
                    entity.MemberStatus = "0";
                    entity.PwdStatus = "D";
                    entity.MemberType = 1;

                    var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                    var allProducts = accReportService.GetLastInitialDate(param);

                    var detail = allProducts.ToString();

                    if (!IsDayInitiated)
                    {
                        if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                        {
                            entity.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                        }
                    }
                    else
                    {
                        entity.JoinDate = TransactionDate;
                    }

                    var vNatio = "";
                    var vSmat = "";
                    var vphone = "";
                    var OtherID = "";
                    var vMemberStatus = "";
                    int? vNIDCheckStatusId = 0;

                    if (entity.NationalID == null)
                    {
                        vNatio = "0";
                    }
                    else
                    {
                        vNatio = entity.NationalID;
                    }
                    if (entity.SmartCard == null)
                    {
                        vSmat = "0";
                    }
                    else
                    {
                        vSmat = entity.SmartCard;

                    }
                    if (entity.PhoneNo == null)
                    {
                        vphone = "0";
                    }
                    else
                    {
                        vphone = entity.PhoneNo;
                    }
                    if (model.NIDCheckStatusId == null)
                    {
                        vNIDCheckStatusId = 1;
                    }
                    else
                    {
                        vNIDCheckStatusId = model.NIDCheckStatusId;
                    }
                    var CheckDupli = new { @NationalID = vNatio, @SmartCard = vSmat, @PhoneNo = vphone, @Qtype = 1, @MemberCode = "0" };
                    var CheckMemberDupli = ultimateReportService.CheckDuplicateMember(CheckDupli);
                    if (CheckMemberDupli.Tables[0].Rows.Count > 0)
                    {
                        var message = CheckMemberDupli.Tables[0].Rows[0]["ErrorName"].ToString();
                        return GetDuplicateErrorMessageResult(message);
                    }

                    DataSet LoanInstallMent;
                    var param1 = new { @OfficeID = LoginUserOfficeID };
                    var param2 = new { @OfficeID = LoginUserOfficeID, @CenterID = entity.CenterID };
                    if (LoggedInOrganizationID == 126)
                    {
                        LoanInstallMent = ultimateReportService.GenerateMemberLastCodeMemberSSS(param2);
                        entity.MemberCode = LoanInstallMent.Tables[0].Rows[0]["LastCode"].ToString();
                    }
                    else
                    {
                        LoanInstallMent = ultimateReportService.GenerateMemberLastCodeMember(param1);
                        entity.MemberCode = LoanInstallMent.Tables[0].Rows[0]["LastCode"].ToString();
                    }

                    memberService.Create(entity);
                    var ent = new { MemberID = entity.MemberID, MemberCode = entity.MemberCode };

                    var param3 = new
                    {
                        @NIDVerificationId = 0,
                        @OfficeID = LoginUserOfficeID,
                        @MemberID = ent.MemberID,
                        @NationalID = vNatio,
                        @SmartCard = vSmat,
                        @OtherIdNo = OtherID,
                        @NIDAddress = model.txtAddress == null ? "" : model.txtAddress,
                        @NIDStatusID = vNIDCheckStatusId,
                        @CreateUser = LoggedInEmployeeID,
                        @UpdateUser = LoggedInEmployeeID
                    };
                    var MemberIDValidate3 = ultimateReportService.GenerateNIDStatus(param3);
                    return Json(new { data = ent }, JsonRequestBehavior.AllowGet);

                }
                else
                    return GetErrorMessageResult();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }


        public JsonResult GetEmployeeForCenter(int centerId)
        {
            try
            {
                MemberViewModel model = new MemberViewModel();

                var param = new { CenterID = centerId, OfficeID = LoginUserOfficeID };
                var alldata = ultimateReportService.GetDataWithParameter(param, "Proc_GetEmployeeByCenterOfficeWise");
                var emp_list = alldata.Tables[0].AsEnumerable()
                    .Select(x => new MemberViewModel
                    {
                        EmployeeID = x.Field<short>("EmployeeID"),
                        EmployeeCode = x.Field<string>("EmployeeCode"),
                        EmployeeName = x.Field<string>("EmployeeName"),
                    }).ToList();
                var dropDown_lists = emp_list.AsEnumerable().Select(x => x).ToList().Select(x => new SelectListItem()
                {
                    Text = x.EmployeeName,
                    Value = x.EmployeeID.ToString()
                }).ToList();

                var dropDown_items = new List<SelectListItem>();
                dropDown_items.Add(new SelectListItem() { Text = "Select One", Value = "0", Selected = true });
                dropDown_items.AddRange(dropDown_lists);
                model.EmployeeList = dropDown_items;
                return Json(dropDown_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }


        //------------------- Edit -------------------------------//
        //------------------- Edit -------------------------------//
        public ActionResult EditRDRS(int id)
        {
            var member = memberService.GetById(id);
            var memberModel = Mapper.Map<Member, MemberViewModel>(member);
            //            memberModel.OfficeID = 2;
            MapDropDownList(memberModel);
            var allCenter = centerService.GetByOfficeId(memberModel.OfficeID, Convert.ToInt16(LoggedInOrganizationID));
            var viewCenter = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + "-" + x.CenterName.ToString()
            });
            memberModel.CenterList = viewCenter;
            var allGroup = groupService.GetByOfficeId(memberModel.OfficeID);
            var viewGroup = allGroup.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GroupID.ToString(),
                Text = x.GroupCode.ToString()
            });

            var param = new { CenterID = memberModel.CenterID };
            var alldata = ultimateReportService.GetDataWithParameter(param, "Proc_GetEmployeeByCenter");
            var emp_list = alldata.Tables[0].AsEnumerable()
                .Select(x => new MemberViewModel
                {
                    EmployeeID = x.Field<short>("EmployeeID"),
                    EmployeeCode = x.Field<string>("EmployeeCode"),
                    EmployeeName = x.Field<string>("EmployeeName"),
                }).ToList();
            var dropDown_lists = emp_list.AsEnumerable().Select(x => x).ToList().Select(x => new SelectListItem()
            {
                Text = x.EmployeeName,
                Value = x.EmployeeID.ToString()
            }).ToList();
            var lists = new List<SelectListItem>();
            lists.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            lists.AddRange(dropDown_lists);

            memberModel.GroupList = viewGroup;
            memberModel.GroupID = member.GroupID;
            memberModel.Gender = member.Gender;
            memberModel.Location = member.Location;
            memberModel.PlaceOfBirth = member.PlaceOfBirth;
            memberModel.ServerCurrentDate = DateTime.Now;
            memberModel.ExpireDate = member.ExpireDate;
            memberModel.EmployeeID = (short?)member.EmployeeID;
            memberModel.EmployeeList = lists;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;


            var OrgInfo = organizationService.GetById((int)SessionHelper.LoginUserOrganizationID);
            //var v = OrgInfo.MemberAge;
            var MemberAge = 60;

            if (OrgInfo.MemberAge == null)
            {

            }
            else
            {
                MemberAge = (int)OrgInfo.MemberAge;
            }
            ViewData["MemberAge"] = MemberAge;




            var imgentityparam = new { @MemberID = memberModel.MemberID, @OfficeId = memberModel.OfficeID };
            var Entity = ultimateReportService.GetDataWithParameter(imgentityparam, "Proc_GetVerificationStatus");

            var NIDStatusID = 0;
            var NIDStatusIDst = "";
            var NIDAddress = "";
            var NIDVerificationId = "";
            if (Entity.Tables[0].Rows.Count > 0)
            {
                NIDStatusIDst = Entity.Tables[0].Rows[0]["NIDStatusID"].ToString();
                NIDAddress = Entity.Tables[0].Rows[0]["NIDAddress"].ToString();
                NIDVerificationId = Entity.Tables[0].Rows[0]["NIDVerificationId"].ToString();
            }

            if (NIDStatusIDst != null)
            {
                if (NIDStatusIDst != "")
                {

                    NIDStatusID = Convert.ToInt32(NIDStatusIDst);
                }
                else
                {
                    NIDStatusID = Convert.ToInt32("1");
                }
            }

            if (NIDVerificationId != null)
            {
                if (NIDVerificationId != "")
                {
                    memberModel.NIDVerificationId = null;
                }
            }

            memberModel.NIDCheckStatusId = NIDStatusID;
            memberModel.txtAddress = NIDAddress;



            return View(memberModel);
        }
        // POST: Member/EditRDRS/5
        [HttpPost]
        public ActionResult EditRDRS(MemberEditViewModel model, string base64image, string base64imageFingerThumb)
        {
            try
            {
                var entity = memberService.GetByIdLong(model.MemberID);

                if (!ModelState.IsValid)
                    return GetErrorMessageResult();

                if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != "") && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                {
                    var t = base64image.Substring(22);  // remove data:image/png;base64,
                    byte[] bytes = Convert.FromBase64String(t);
                    Image image;
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = Image.FromStream(ms);
                    }
                    var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                    entity.MemberImg = bytes;

                    ///////////////////////////

                    var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                    byte[] bytesthumb = Convert.FromBase64String(tthumb);
                    Image imagethumb;
                    using (MemoryStream msa = new MemoryStream(bytesthumb))
                    {
                        imagethumb = Image.FromStream(msa);
                    }
                    var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                    entity.ThumbImg = bytesthumb;
                }
                else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != ""))
                {
                    var t = base64image.Substring(22);  // remove data:image/png;base64,
                    byte[] bytes = Convert.FromBase64String(t);
                    Image image;
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = Image.FromStream(ms);
                    }
                    var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                    //byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                    entity.MemberImg = bytes;
                }
                else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                {
                    var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                    byte[] bytesthumb = Convert.FromBase64String(tthumb);
                    Image imagethumb;
                    using (MemoryStream msa = new MemoryStream(bytesthumb))
                    {
                        imagethumb = Image.FromStream(msa);
                    }
                    var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                    entity.ThumbImg = bytesthumb;
                }
                else if (model.ImgFile == null && Session["CapturedImage"] != null)
                {
                    var path = Server.MapPath("~/CapturedImages/");

                    var FileLocation = path + Session["CapturedImage"].ToString();
                    System.IO.File.Exists(FileLocation);
                    System.IO.FileInfo file = new System.IO.FileInfo(FileLocation);

                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(file.FullName))
                    {
                        //Image image = System.Drawing.Image.FromFile(file.FullName);
                        ImageConverter converter = new ImageConverter();
                        byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                        entity.MemberImg = data;

                        decimal size = Math.Round(((decimal)entity.MemberImg.Length / (decimal)1024), 2);
                        if (size > 100)
                        {
                            var errors = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                            //("File size must not exceed 100 KB.");
                            //CustomValidator1.ErrorMessage = "File size must not exceed 100 KB.";
                            //errors("File size must not exceed 100 KB.");
                            // e.IsValid = false;
                        }

                    }

                    Array.ForEach(Directory.GetFiles(path), System.IO.File.Delete);

                }
                else if (model.ImgFile != null)
                {
                    decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                    if (size > 100)
                    {
                        var error = memberService.CheckImageSize();
                        Response.StatusCode = 400;
                        throw new System.InvalidOperationException("Image File is Big than 100 KB.");
                    }

                    byte[] data = new byte[model.ImgFile.ContentLength];
                    if (data != null)
                    {
                        decimal size2 = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                        if (size2 > 100)
                        {
                            var error = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                            throw new System.InvalidOperationException("Image File is Big than 100 KB.");

                        }


                        model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                        entity.MemberImg = data;
                    }
                }

                if (model.ThumbImgFile != null)
                {
                    decimal size2 = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                    if (size2 > 100)
                    {
                        var error = memberService.CheckImageSize();
                        Response.StatusCode = 400;
                    }

                    byte[] data = new byte[model.ThumbImgFile.ContentLength];
                    if (data != null)
                    {
                        model.ThumbImgFile.InputStream.Read(data, 0, model.ThumbImgFile.ContentLength);
                        entity.ThumbImg = data;
                    }
                }

                if (entity.OfficeID != LoginUserOfficeID)
                {
                    return GetErrorMessageResult("Invalid Office...........");
                }
                entity.EmployeeID = model.EmployeeID;
                entity.GroupID = model.GroupID;
                entity.FirstName = model.FirstName;
                entity.MiddleName = model.MiddleName;
                entity.LastName = model.LastName;
                entity.AddressLine1 = model.AddressLine1;
                entity.AddressLine2 = model.AddressLine2;
                entity.RefereeName = model.RefereeName;
                entity.BirthDate = model.BirthDate;
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                var allProducts = accReportService.GetLastInitialDate(param);

                entity.NationalID = model.NationalID;
                entity.HomeType = model.HomeType;
                entity.FamilyContactNo = model.FamilyContactNo;

                entity.Email = model.Email;
                entity.PhoneNo = model.PhoneNo;
                entity.MaritalStatus = model.MaritalStatus;
                entity.MemCategory = model.MemCategory;
                entity.EconomicActivity = model.EconomicActivity;

                // NEW ADD KHALID
                entity.AddressLine1 = model.AddressLine1;
                entity.AddressLine2 = model.AddressLine2;
                entity.CountryID = model.CountryID;
                entity.DivisionCode = model.DivisionCode;
                entity.DistrictCode = model.DistrictCode;
                entity.UpozillaCode = model.UpozillaCode;
                entity.UnionCode = model.UnionCode;
                entity.VillageCode = model.VillageCode;
                entity.ZipCode = model.ZipCode;
                entity.Education = model.Education;


                entity.IsAnyFS = model.IsAnyFS;
                entity.FServiceName = model.FServiceName;
                entity.FinServiceChoiceId = model.FinServiceChoiceId;
                entity.TransactionChoiceId = model.TransactionChoiceId;

                entity.BanglaName = model.BanglaName;
                entity.PerAddressLine1 = model.PerAddressLine1;
                entity.PerAddressLine2 = model.PerAddressLine2;
                entity.PerCountryID = model.PerCountryID;
                entity.PerDivisionCode = model.PerDivisionCode;
                entity.PerDistrictCode = model.PerDistrictCode;
                entity.PerUpozillaCode = model.PerUpozillaCode;
                entity.PerUnionCode = model.PerUnionCode;
                entity.PerVillageCode = model.PerVillageCode;
                entity.PerZipCode = model.PerZipCode;
                entity.FatherNameBN = model.FatherNameBN;
                entity.MotherNameBN = model.MotherNameBN;
                entity.IdentTypeID = model.IdentTypeID;
                entity.ExpireDate = model.ExpireDate;
                entity.ProvidedByCountryID = model.ProvidedByCountryID;
                entity.SpouseName = model.SpouseName;
                entity.SpouseNameBN = model.SpouseNameBN;
                entity.TIN = model.TIN;
                entity.TaxAmount = model.TaxAmount;
                entity.OtherIdNo = model.OtherIdNo;
                entity.CoApplicantName = model.CoApplicantName;
                entity.Gender = model.Gender;
                entity.FamilyMember = model.FamilyMember;
                entity.Cityzenship = model.Cityzenship;
                entity.ExpireDate = model.ExpireDate;
                // END

                if (model.DivisionCode != null)
                {
                    if (model.DivisionCode != "0")
                    {
                        entity.DivisionCode = model.DivisionCode;
                    }
                    else
                    {
                        entity.DivisionCode = "";
                    }
                }
                if (model.DistrictCode != null)
                {
                    if (model.DistrictCode != "0")
                    {
                        entity.DistrictCode = model.DistrictCode;
                    }
                    else
                    {
                        entity.DistrictCode = "";
                    }
                }
                if (model.UpozillaCode != null)
                {
                    if (model.UpozillaCode != "0")
                    {
                        entity.UpozillaCode = model.UpozillaCode;
                    }
                    else
                    {
                        entity.UpozillaCode = "";
                    }
                }
                if (model.UnionCode != null)
                {
                    if (model.UnionCode != "0")
                    {
                        entity.UnionCode = model.UnionCode;
                    }
                    else
                    {
                        entity.UnionCode = "";
                    }
                }
                if (model.VillageCode != null)
                {
                    if (model.VillageCode != "0")
                    {
                        entity.VillageCode = model.VillageCode;
                    }
                    else
                    {
                        entity.VillageCode = "";
                    }
                }


                if (model.PerDivisionCode != null)
                {
                    if (model.PerDivisionCode != "0")
                    {
                        entity.PerDivisionCode = model.PerDivisionCode;
                    }
                    else
                    {
                        entity.PerDivisionCode = "";
                    }
                }
                if (model.PerDistrictCode != null)
                {
                    if (model.PerDistrictCode != "0")
                    {
                        entity.PerDistrictCode = model.PerDistrictCode;
                    }
                    else
                    {
                        entity.PerDistrictCode = "";
                    }
                }
                if (model.PerUpozillaCode != null)
                {
                    if (model.PerUpozillaCode != "0")
                    {
                        entity.PerUpozillaCode = model.PerUpozillaCode;
                    }
                    else
                    {
                        entity.PerUpozillaCode = "";
                    }
                }
                if (model.PerUnionCode != null)
                {
                    if (model.PerUnionCode != "0")
                    {
                        entity.PerUnionCode = model.PerUnionCode;
                    }
                    else
                    {
                        entity.PerUnionCode = "";
                    }
                }
                if (model.PerVillageCode != null)
                {
                    if (model.PerVillageCode != "0")
                    {
                        entity.PerVillageCode = model.PerVillageCode;
                    }
                    else
                    {
                        entity.PerVillageCode = "";
                    }
                }

                if (model.PlaceOfBirth != null)
                {
                    entity.PlaceOfBirth = model.PlaceOfBirth;

                }
                if (model.TotalWealth != null)
                {
                    entity.TotalWealth = model.TotalWealth;

                }
                if (model.MotherName != null)
                {
                    entity.MotherName = model.MotherName;

                }
                if (model.FatherName != null)
                {
                    entity.FatherName = model.FatherName;

                }
                if (model.NationalID != null)
                {
                    entity.NationalID = model.NationalID;
                }
                if (model.NationalID == null)
                {
                    entity.NationalID = "0";


                }

                if (model.SmartCard != null)
                {
                    entity.SmartCard = model.SmartCard;
                }

                if (model.PhoneNo != null)
                {
                    entity.PhoneNo = model.PhoneNo;

                }
                //if (entity.PhoneNo == null)
                //{
                //    return GetErrorMessageResult("PhoneNo cann't be null");
                //}

                var vNatio = "";
                var vSmat = "";
                var vphone = "";
                var OtherID = "";
                var vMemberStatus = "";
                int? vNIDCheckStatusId = 0;

                if (entity.NationalID == null)
                {
                    vNatio = "0";
                }
                else
                {
                    vNatio = entity.NationalID;
                }
                if (entity.SmartCard == null)
                {
                    vSmat = "0";
                }
                else
                {
                    vSmat = entity.SmartCard;
                }
                if (entity.PhoneNo == null)
                {
                    vphone = "0";
                }
                else
                {
                    vphone = entity.PhoneNo;
                }

                if (model.NIDCheckStatusId == null)
                {
                    vNIDCheckStatusId = 1;
                }
                else
                {
                    vNIDCheckStatusId = model.NIDCheckStatusId;
                }

                var CheckDupli = new { @NationalID = vNatio, @SmartCard = vSmat, @PhoneNo = vphone, @Qtype = 2, @MemberCode = entity.MemberCode };
                var CheckMemberDupli = ultimateReportService.CheckDuplicateMember(CheckDupli);
                if (CheckMemberDupli.Tables[0].Rows.Count > 0)
                {
                    return GetErrorMessageResult(CheckMemberDupli.Tables[0].Rows[0]["ErrorName"].ToString());
                }

                memberService.Update(entity);

                // ----------------------------------------------------
                var param3 = new
                {
                    @NIDVerificationId = model.NIDVerificationId,
                    @OfficeID = LoginUserOfficeID,
                    @MemberID = model.MemberID,
                    @NationalID = vNatio,
                    @SmartCard = vSmat,
                    @OtherIdNo = OtherID,
                    @NIDAddress = model.txtAddress == null ? "" : model.txtAddress,
                    @NIDStatusID = vNIDCheckStatusId,
                    @CreateUser = LoggedInEmployeeID,
                    @UpdateUser = LoggedInEmployeeID
                };
                var MemberIDValidate3 = ultimateReportService.GenerateNIDStatus(param3);

                return GetSuccessMessageResult();


            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        //===========================================================================================//
        public ActionResult EligibleCreate(long id)
        {
            var member = memberService.GetByIdLong(id);
            var memberModel = Mapper.Map<Member, MemberViewModel>(member);
            //            memberModel.OfficeID = 2;
            MapDropDownList(memberModel);
            var allCenter = centerService.GetByOfficeId(memberModel.OfficeID, Convert.ToInt16(LoggedInOrganizationID));
            var viewCenter = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + "-" + x.CenterName.ToString()
            });
            memberModel.CenterList = viewCenter;

            var allGroup = groupService.GetByOfficeId(memberModel.OfficeID);
            var viewGroup = allGroup.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GroupID.ToString(),
                Text = x.GroupCode.ToString()
            });
            memberModel.GroupList = viewGroup;
            memberModel.GroupID = member.GroupID;
            memberModel.MemberCategoryID = member.MemberCategoryID;
            memberModel.Gender = member.Gender;
            memberModel.Location = member.Location;
            memberModel.PlaceOfBirth = member.PlaceOfBirth;
            return View(memberModel);
        }
        [HttpPost]
        public ActionResult EligibleCreate(MemberViewModelApproval model, List<string> ProdList)
        {
            try
            {
                // Retrieve the member entity
                var entity = memberService.GetByIdLong(Convert.ToInt32(model.MemberID));

                // Log model state errors if any
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        // You might want to log the error message or handle it as needed
                    }
                }

                // Proceed if the model state is valid
                if (ModelState.IsValid)
                {
                    // Check if the organization ID is not South Africa (218)
                    if (LoggedInOrganizationID != 218)
                    {
                        // Process the product list
                        foreach (var productId in ProdList)
                        {
                            var savingSummary = new SavingSummary
                            {
                                ProductID = Convert.ToInt16(productId)
                            };

                            savingSummaryService.Proc_Set_SavingOpeingWhenMemberEligible(
                                Convert.ToInt32(SessionHelper.LoginUserOfficeID),
                                Convert.ToInt32(entity.CenterID),
                                entity.MemberID,
                                Convert.ToInt16(productId),
                                1,
                                entity.JoinDate,
                                1,
                                entity.JoinDate,
                                Convert.ToInt32(entity.Center.EmployeeId),
                                Convert.ToInt32(entity.MemberCategoryID),
                                Convert.ToInt16(LoggedInOrganizationID),
                                LoggedInEmployeeID.ToString(),
                                DateTime.Now
                            );
                        }
                    }
                    else
                    {
                        // Update the member if the organization ID is South Africa (218)
                        entity.MemberStatus = "1";
                        memberService.Update(entity);
                       
                    }

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
        //[HttpPost]
        //public ActionResult EligibleCreate(MemberViewModelApproval model, List<string> ProdList)
        //{
        //    try
        //    {
        //        var entity = memberService.GetByIdLong(Convert.ToInt32(model.MemberID));

        //        foreach (ModelState modelState in ModelState.Values)
        //        {
        //            foreach (ModelError error in modelState.Errors)
        //            {
        //                var myerr = error.ErrorMessage;
        //            }
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            foreach (var p in ProdList)
        //            {
        //                SavingSummary SS = new SavingSummary();
        //                SS.ProductID = Convert.ToInt16(p);
        //                savingSummaryService.Proc_Set_SavingOpeingWhenMemberEligible(Convert.ToInt32(SessionHelper.LoginUserOfficeID), Convert.ToInt32(entity.CenterID), entity.MemberID, Convert.ToInt16(p), 1, entity.JoinDate, 1, entity.JoinDate, Convert.ToInt32(entity.Center.EmployeeId), Convert.ToInt32(entity.MemberCategoryID), Convert.ToInt16(LoggedInOrganizationID), LoggedInEmployeeID.ToString(), System.DateTime.Now);
        //            }

        //            return GetSuccessMessageResult();
        //        }
        //        else
        //            return GetErrorMessageResult();
        //    }
        //    catch (Exception ex)
        //    {
        //        return GetErrorMessageResult(ex);
        //    }
        //}
        public ActionResult EligibleEdit(int id)
        {
            var member = memberService.GetByIdLong(id);
            if (ModelState.IsValid)
            {
                member.MemberStatus = "5";
                memberService.Update(member);

                return RedirectToAction("Eligible", "Member");
            }
            else
                return GetErrorMessageResult();
        }
        #endregion


        public async Task<JsonResult> LoadAge(string FromDate, string ToDate)
        {
            List<MemberViewModel> List_ViewModel = new List<MemberViewModel>();

            var param = new
            {
                FromDate = FromDate,
                ToDate = ToDate

            };

            var empList = ultimateReportService.GetDataWithParameter(param, "SP_Get_Age");

            List_ViewModel = empList.Tables[0].AsEnumerable()
            .Select(row => new MemberViewModel
            {
                AsOnDateAge = row.Field<string>("AsOnDateAge")

            }).ToList();

            return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
        }


        #region JCF Member

        public ActionResult IndexJCF()
        {
            var model = new MemberViewModel();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }

            ViewData["LoggedInOrg"] = SessionHelper.LoginUserOrganizationID;

            return View();
        }

        public ActionResult CreateJCF()
        {
            var model = new MemberViewModel();
            MapDropDownList(model);
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
            if (!IsDayInitiated)
            {
                if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                {
                    model.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                }
            }
            else
            {
                model.JoinDate = TransactionDate;
            }
            model.ServerCurrentDate = DateTime.Now;
            var blnk_items = new List<SelectListItem>();
            model.CenterList = blnk_items;
            model.GroupList = blnk_items;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;

            var OrgInfo = organizationService.GetById((int)SessionHelper.LoginUserOrganizationID);
            //var v = OrgInfo.MemberAge;
            var MemberAge = 60;

            if (OrgInfo.MemberAge == null)
            {

            }
            else
            {
                MemberAge = (int)OrgInfo.MemberAge;
            }
            ViewData["MemberAge"] = MemberAge;

            ViewData["LoggedInOrg"] = SessionHelper.LoginUserOrganizationID;
            ViewBag.OrgId = LoggedInOrganizationID;
            return View(model);
        }

        [HttpPost]
        public JsonResult CreateJCF(MemberViewModel model, string MemCode, string base64image, string base64imageFingerThumb)
        {
            try
            {
                var entity = Mapper.Map<MemberViewModel, Member>(model);
                if (ModelState.IsValid)
                {
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                    var errors = memberService.IsValidMember(entity);

                    if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != "") && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                    {
                        var t = base64image.Substring(22);  // remove data:image/png;base64,
                        byte[] bytes = Convert.FromBase64String(t);
                        Image image;
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            image = Image.FromStream(ms);
                        }
                        var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                        entity.MemberImg = bytes;

                        var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                        byte[] bytesthumb = Convert.FromBase64String(tthumb);
                        Image imagethumb;
                        using (MemoryStream msa = new MemoryStream(bytesthumb))
                        {
                            imagethumb = Image.FromStream(msa);
                        }
                        var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                        entity.ThumbImg = bytesthumb;
                    }
                    else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != ""))
                    {
                        var t = base64image.Substring(22);  // remove data:image/png;base64,
                        byte[] bytes = Convert.FromBase64String(t);
                        Image image;
                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            image = Image.FromStream(ms);
                        }
                        var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                        entity.MemberImg = bytes;
                    }
                    else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                    {
                        var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                        byte[] bytesthumb = Convert.FromBase64String(tthumb);
                        Image imagethumb;
                        using (MemoryStream msa = new MemoryStream(bytesthumb))
                        {
                            imagethumb = Image.FromStream(msa);
                        }
                        var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                        entity.ThumbImg = bytesthumb;
                    }
                    else if (model.ImgFile == null && Session["CapturedImage"] != null)
                    {
                        var path = Server.MapPath("~/CapturedImages/");

                        var FileLocation = path + Session["CapturedImage"].ToString();
                        System.IO.File.Exists(FileLocation);
                        System.IO.FileInfo file = new System.IO.FileInfo(FileLocation);

                        using (System.Drawing.Image image = System.Drawing.Image.FromFile(file.FullName))
                        {
                            ImageConverter converter = new ImageConverter();
                            byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                            entity.MemberImg = data;


                            decimal size = Math.Round(((decimal)entity.MemberImg.Length / (decimal)1024), 2);
                            if (size > 100)
                            {
                                errors = memberService.CheckImageSize();
                                Response.StatusCode = 400;
                            }
                        }
                        Array.ForEach(Directory.GetFiles(path), System.IO.File.Delete);

                    }
                    else if (model.ImgFile != null)
                    {

                        decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                        if (size > 100)
                        {
                            errors = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                            throw new System.InvalidOperationException("Image File is Big than 100 KB.");
                        }

                        byte[] data = new byte[model.ImgFile.ContentLength];
                        if (data != null)
                        {
                            model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                            entity.MemberImg = data;
                        }
                    }

                    if (model.ThumbImgFile != null)
                    {
                        decimal size = Math.Round(((decimal)model.ThumbImgFile.ContentLength / (decimal)1024), 2);
                        if (size > 100)
                        {
                            errors = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                            throw new System.InvalidOperationException("Image File is Big than 100 KB.");
                        }

                        byte[] data = new byte[model.ThumbImgFile.ContentLength];
                        if (data != null)
                        {
                            model.ThumbImgFile.InputStream.Read(data, 0, model.ThumbImgFile.ContentLength);
                            entity.ThumbImg = data;
                        }
                    }


                    if (model.DivisionCode != null)
                    {
                        if (model.DivisionCode != "0")
                        {
                            entity.DivisionCode = model.DivisionCode;
                        }
                        else
                        {
                            entity.DivisionCode = "";
                        }
                    }
                    if (model.DistrictCode != null)
                    {
                        if (model.DistrictCode != "0")
                        {
                            entity.DistrictCode = model.DistrictCode;
                        }
                        else
                        {
                            entity.DistrictCode = "";
                        }
                    }
                    if (model.UpozillaCode != null)
                    {
                        if (model.UpozillaCode != "0")
                        {
                            entity.UpozillaCode = model.UpozillaCode;
                        }
                        else
                        {
                            entity.UpozillaCode = "";
                        }
                    }
                    if (model.UnionCode != null)
                    {
                        if (model.UnionCode != "0")
                        {
                            entity.UnionCode = model.UnionCode;
                        }
                        else
                        {
                            entity.UnionCode = "";
                        }
                    }
                    if (model.VillageCode != null)
                    {
                        if (model.VillageCode != "0")
                        {
                            entity.VillageCode = model.VillageCode;
                        }
                        else
                        {
                            entity.VillageCode = "";
                        }
                    }
                    if (model.PlaceOfBirth != null)
                    {
                        entity.PlaceOfBirth = model.PlaceOfBirth;

                    }
                    if (model.TotalWealth != null)
                    {
                        entity.TotalWealth = model.TotalWealth;

                    }
                    if (model.MotherName != null)
                    {
                        entity.MotherName = model.MotherName;

                    }
                    if (model.ExpireDate != null)
                    {
                        entity.ExpireDate = model.ExpireDate;

                    }
                    if (model.FatherName != null)
                    {
                        entity.FatherName = model.FatherName;

                    }

                    if (model.NationalID != null)
                    {
                        entity.NationalID = model.NationalID;
                        if (LoggedInOrganizationID != 150)
                        {
                            if (entity.NationalID.Length != 17)
                            {
                                return GetErrorMessageResult("NationalID  No cann't be less than 17 digits");
                            }
                        }
                    }
                    if (model.NationalID == null)
                    {
                        entity.NationalID = "0";
                    }
                    if (model.SmartCard != null)
                    {
                        entity.SmartCard = model.SmartCard;
                        if (LoggedInOrganizationID != 150)
                        {
                            if (entity.SmartCard.Length != 10)
                            {
                                return GetErrorMessageResult("SmartCard No cann't be less than 10 digits");
                            }
                        }

                    }
                    if (model.PhoneNo != null)
                    {
                        entity.PhoneNo = model.PhoneNo;

                    }
                    //if (entity.PhoneNo == null)
                    //{
                    //    return GetErrorMessageResult("PhoneNo cann't be null");
                    //}

                    if (LoggedInOrganizationID == 150)
                        entity.GroupID = 1;

                    entity.IsActive = true;
                    entity.MemberStatus = "0";
                    entity.PwdStatus = "D";
                    entity.MemberType = 1;

                    var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                    var allProducts = accReportService.GetLastInitialDate(param);

                    var detail = allProducts.ToString();

                    if (!IsDayInitiated)
                    {
                        if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                        {
                            entity.JoinDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                        }
                    }
                    else
                    {
                        entity.JoinDate = TransactionDate;
                    }
                    var vNatio = "";
                    var vSmat = "";
                    var vphone = "";
                    if (entity.NationalID == null)
                    {
                        vNatio = "0";
                    }
                    else
                    {
                        vNatio = entity.NationalID;
                    }
                    if (entity.SmartCard == null)
                    {
                        vSmat = "0";
                    }
                    else
                    {
                        vSmat = entity.SmartCard;

                    }
                    if (entity.PhoneNo == null)
                    {
                        vphone = "0";
                    }
                    else
                    {
                        vphone = entity.PhoneNo;
                    }
                    var CheckDupli = new { @NationalID = vNatio, @SmartCard = vSmat, @PhoneNo = vphone, @Qtype = 1, @MemberCode = "0" };
                    var CheckMemberDupli = ultimateReportService.CheckDuplicateMember(CheckDupli);
                    if (CheckMemberDupli.Tables[0].Rows.Count > 0)
                    {
                        var message = CheckMemberDupli.Tables[0].Rows[0]["ErrorName"].ToString();
                        return GetDuplicateErrorMessageResult(message);
                    }

                    DataSet LoanInstallMent;
                    var param1 = new { @OfficeID = LoginUserOfficeID };
                    var param2 = new { @OfficeID = LoginUserOfficeID, @CenterID = entity.CenterID };
                    if (LoggedInOrganizationID == 126)
                    {
                        LoanInstallMent = ultimateReportService.GenerateMemberLastCodeMemberSSS(param2);
                        entity.MemberCode = LoanInstallMent.Tables[0].Rows[0]["LastCode"].ToString();
                    }
                    else
                    {
                        LoanInstallMent = ultimateReportService.GenerateMemberLastCodeMember(param1);
                        entity.MemberCode = LoanInstallMent.Tables[0].Rows[0]["LastCode"].ToString();
                    }

                    memberService.Create(entity);
                    var ent = new { MemberID = entity.MemberID, MemberCode = entity.MemberCode };
                    return Json(new { data = ent }, JsonRequestBehavior.AllowGet);

                }
                else
                    return GetErrorMessageResult();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
                //return GetErrorMessageResult(e);
            }
            //catch (Exception ex)
            //{
            //    return GetErrorMessageResult(ex);
            //}
        }

        public ActionResult EditJCF(int id)
        {
            var member = memberService.GetById(id);
            var memberModel = Mapper.Map<Member, MemberViewModel>(member);
            //            memberModel.OfficeID = 2;
            MapDropDownList(memberModel);
            var allCenter = centerService.GetByOfficeId(memberModel.OfficeID, Convert.ToInt16(LoggedInOrganizationID));
            var viewCenter = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + "-" + x.CenterName.ToString()
            });
            memberModel.CenterList = viewCenter;
            var allGroup = groupService.GetByOfficeId(memberModel.OfficeID);
            var viewGroup = allGroup.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GroupID.ToString(),
                Text = x.GroupCode.ToString()
            });
            memberModel.GroupList = viewGroup;
            memberModel.GroupID = member.GroupID;
            memberModel.Gender = member.Gender;
            memberModel.Location = member.Location;
            memberModel.PlaceOfBirth = member.PlaceOfBirth;
            memberModel.ServerCurrentDate = DateTime.Now;
            memberModel.ExpireDate = member.ExpireDate;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;


            var OrgInfo = organizationService.GetById((int)SessionHelper.LoginUserOrganizationID);
            //var v = OrgInfo.MemberAge;
            var MemberAge = 60;

            if (OrgInfo.MemberAge == null)
            {

            }
            else
            {
                MemberAge = (int)OrgInfo.MemberAge;
            }
            ViewData["MemberAge"] = MemberAge;





            return View(memberModel);
        }

        [HttpPost]
        public ActionResult EditJCF(MemberEditViewModel model, string base64image, string base64imageFingerThumb)
        {
            try
            {
                var entity = memberService.GetByIdLong(model.MemberID);

                //foreach (ModelState modelState in ModelState.Values)
                //{
                //    foreach (ModelError error in modelState.Errors)
                //    {
                //        var myerr = error.ErrorMessage;
                //    }
                //}
                //if (entity.MemberImg == null)
                //{
                //    DataSet vMemberImage;

                //    if (entity.MemberImg == null)
                //    {
                //        var param2 = new { @MemberID = model.MemberID };

                //        vMemberImage = ultimateReportService.GetMemberImage(param2);
                //        var img = entity.MemberImg;
                //        if (vMemberImage.Tables[0].Rows.Count > 0)
                //        {
                //            img = entity.MemberImg;
                //        }
                //        img = entity.MemberImg;
                //    }
                //}


                if (!ModelState.IsValid)
                    return GetErrorMessageResult();

                if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != "") && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                {
                    var t = base64image.Substring(22);  // remove data:image/png;base64,
                    byte[] bytes = Convert.FromBase64String(t);
                    Image image;
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = Image.FromStream(ms);
                    }
                    var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                    entity.MemberImg = bytes;

                    ///////////////////////////

                    var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                    byte[] bytesthumb = Convert.FromBase64String(tthumb);
                    Image imagethumb;
                    using (MemoryStream msa = new MemoryStream(bytesthumb))
                    {
                        imagethumb = Image.FromStream(msa);
                    }
                    var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                    entity.ThumbImg = bytesthumb;
                }
                else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64image != null && base64image != ""))
                {
                    var t = base64image.Substring(22);  // remove data:image/png;base64,
                    byte[] bytes = Convert.FromBase64String(t);
                    Image image;
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = Image.FromStream(ms);
                    }
                    var randomFileName = Guid.NewGuid().ToString().Substring(0, 4) + ".png";
                    //byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                    entity.MemberImg = bytes;
                }
                else if (model.ImgFile == null && Session["CapturedImage"] == null && (base64imageFingerThumb != null && base64imageFingerThumb != ""))
                {
                    var tthumb = base64imageFingerThumb.Substring(22);  // remove data:image/png;base64,
                    byte[] bytesthumb = Convert.FromBase64String(tthumb);
                    Image imagethumb;
                    using (MemoryStream msa = new MemoryStream(bytesthumb))
                    {
                        imagethumb = Image.FromStream(msa);
                    }
                    var randomFileNamethumb = Guid.NewGuid().ToString().Substring(0, 6) + ".png";
                    entity.ThumbImg = bytesthumb;
                }
                else if (model.ImgFile == null && Session["CapturedImage"] != null)
                {
                    var path = Server.MapPath("~/CapturedImages/");

                    var FileLocation = path + Session["CapturedImage"].ToString();
                    System.IO.File.Exists(FileLocation);
                    System.IO.FileInfo file = new System.IO.FileInfo(FileLocation);

                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(file.FullName))
                    {
                        //Image image = System.Drawing.Image.FromFile(file.FullName);
                        ImageConverter converter = new ImageConverter();
                        byte[] data = (byte[])converter.ConvertTo(image, typeof(byte[]));
                        entity.MemberImg = data;

                        decimal size = Math.Round(((decimal)entity.MemberImg.Length / (decimal)1024), 2);
                        if (size > 100)
                        {
                            var errors = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                            //("File size must not exceed 100 KB.");
                            //CustomValidator1.ErrorMessage = "File size must not exceed 100 KB.";
                            //errors("File size must not exceed 100 KB.");
                            // e.IsValid = false;
                        }

                    }

                    Array.ForEach(Directory.GetFiles(path), System.IO.File.Delete);

                }
                else if (model.ImgFile != null)
                {
                    decimal size = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                    if (size > 100)
                    {
                        var error = memberService.CheckImageSize();
                        Response.StatusCode = 400;
                        throw new System.InvalidOperationException("Image File is Big than 100 KB.");
                    }

                    byte[] data = new byte[model.ImgFile.ContentLength];
                    if (data != null)
                    {
                        decimal size2 = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                        if (size2 > 100)
                        {
                            var error = memberService.CheckImageSize();
                            Response.StatusCode = 400;
                            throw new System.InvalidOperationException("Image File is Big than 100 KB.");

                        }


                        model.ImgFile.InputStream.Read(data, 0, model.ImgFile.ContentLength);
                        entity.MemberImg = data;
                    }
                }

                if (model.ThumbImgFile != null)
                {
                    decimal size2 = Math.Round(((decimal)model.ImgFile.ContentLength / (decimal)1024), 2);
                    if (size2 > 100)
                    {
                        var error = memberService.CheckImageSize();
                        Response.StatusCode = 400;
                    }

                    byte[] data = new byte[model.ThumbImgFile.ContentLength];
                    if (data != null)
                    {
                        model.ThumbImgFile.InputStream.Read(data, 0, model.ThumbImgFile.ContentLength);
                        entity.ThumbImg = data;
                    }
                }

                if (entity.OfficeID != LoginUserOfficeID)
                {
                    return GetErrorMessageResult("Invalid Office...........");
                }

                entity.GroupID = model.GroupID;
                entity.FirstName = model.FirstName;
                entity.MiddleName = model.MiddleName;
                entity.LastName = model.LastName;
                entity.AddressLine1 = model.AddressLine1;
                entity.AddressLine2 = model.AddressLine2;
                entity.RefereeName = model.RefereeName;
                entity.BirthDate = model.BirthDate;
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                var allProducts = accReportService.GetLastInitialDate(param);

                entity.NationalID = model.NationalID;
                entity.HomeType = model.HomeType;
                entity.FamilyContactNo = model.FamilyContactNo;

                entity.Email = model.Email;
                entity.PhoneNo = model.PhoneNo;
                entity.MaritalStatus = model.MaritalStatus;
                entity.MemCategory = model.MemCategory;
                entity.EconomicActivity = model.EconomicActivity;

                // NEW ADD KHALID
                entity.AddressLine1 = model.AddressLine1;
                entity.AddressLine2 = model.AddressLine2;
                entity.CountryID = model.CountryID;
                entity.DivisionCode = model.DivisionCode;
                entity.DistrictCode = model.DistrictCode;
                entity.UpozillaCode = model.UpozillaCode;
                entity.UnionCode = model.UnionCode;
                entity.VillageCode = model.VillageCode;
                entity.ZipCode = model.ZipCode;
                entity.Education = model.Education;


                entity.IsAnyFS = model.IsAnyFS;
                entity.FServiceName = model.FServiceName;
                entity.FinServiceChoiceId = model.FinServiceChoiceId;
                entity.TransactionChoiceId = model.TransactionChoiceId;

                entity.BanglaName = model.BanglaName;
                entity.PerAddressLine1 = model.PerAddressLine1;
                entity.PerAddressLine2 = model.PerAddressLine2;
                entity.PerCountryID = model.PerCountryID;
                entity.PerDivisionCode = model.PerDivisionCode;
                entity.PerDistrictCode = model.PerDistrictCode;
                entity.PerUpozillaCode = model.PerUpozillaCode;
                entity.PerUnionCode = model.PerUnionCode;
                entity.PerVillageCode = model.PerVillageCode;
                entity.PerZipCode = model.PerZipCode;
                entity.FatherNameBN = model.FatherNameBN;
                entity.MotherNameBN = model.MotherNameBN;
                entity.IdentTypeID = model.IdentTypeID;
                entity.ExpireDate = model.ExpireDate;
                entity.ProvidedByCountryID = model.ProvidedByCountryID;
                entity.SpouseName = model.SpouseName;
                entity.SpouseNameBN = model.SpouseNameBN;
                entity.TIN = model.TIN;
                entity.TaxAmount = model.TaxAmount;
                entity.OtherIdNo = model.OtherIdNo;
                entity.CoApplicantName = model.CoApplicantName;
                entity.Gender = model.Gender;
                entity.FamilyMember = model.FamilyMember;
                entity.Cityzenship = model.Cityzenship;
                entity.ExpireDate = model.ExpireDate;
                // END

                if (model.DivisionCode != null)
                {
                    if (model.DivisionCode != "0")
                    {
                        entity.DivisionCode = model.DivisionCode;
                    }
                    else
                    {
                        entity.DivisionCode = "";
                    }
                }
                if (model.DistrictCode != null)
                {
                    if (model.DistrictCode != "0")
                    {
                        entity.DistrictCode = model.DistrictCode;
                    }
                    else
                    {
                        entity.DistrictCode = "";
                    }
                }
                if (model.UpozillaCode != null)
                {
                    if (model.UpozillaCode != "0")
                    {
                        entity.UpozillaCode = model.UpozillaCode;
                    }
                    else
                    {
                        entity.UpozillaCode = "";
                    }
                }
                if (model.UnionCode != null)
                {
                    if (model.UnionCode != "0")
                    {
                        entity.UnionCode = model.UnionCode;
                    }
                    else
                    {
                        entity.UnionCode = "";
                    }
                }
                if (model.VillageCode != null)
                {
                    if (model.VillageCode != "0")
                    {
                        entity.VillageCode = model.VillageCode;
                    }
                    else
                    {
                        entity.VillageCode = "";
                    }
                }


                if (model.PerDivisionCode != null)
                {
                    if (model.PerDivisionCode != "0")
                    {
                        entity.PerDivisionCode = model.PerDivisionCode;
                    }
                    else
                    {
                        entity.PerDivisionCode = "";
                    }
                }
                if (model.PerDistrictCode != null)
                {
                    if (model.PerDistrictCode != "0")
                    {
                        entity.PerDistrictCode = model.PerDistrictCode;
                    }
                    else
                    {
                        entity.PerDistrictCode = "";
                    }
                }
                if (model.PerUpozillaCode != null)
                {
                    if (model.PerUpozillaCode != "0")
                    {
                        entity.PerUpozillaCode = model.PerUpozillaCode;
                    }
                    else
                    {
                        entity.PerUpozillaCode = "";
                    }
                }
                if (model.PerUnionCode != null)
                {
                    if (model.PerUnionCode != "0")
                    {
                        entity.PerUnionCode = model.PerUnionCode;
                    }
                    else
                    {
                        entity.PerUnionCode = "";
                    }
                }
                if (model.PerVillageCode != null)
                {
                    if (model.PerVillageCode != "0")
                    {
                        entity.PerVillageCode = model.PerVillageCode;
                    }
                    else
                    {
                        entity.PerVillageCode = "";
                    }
                }

                if (model.PlaceOfBirth != null)
                {
                    entity.PlaceOfBirth = model.PlaceOfBirth;

                }
                if (model.TotalWealth != null)
                {
                    entity.TotalWealth = model.TotalWealth;

                }
                if (model.MotherName != null)
                {
                    entity.MotherName = model.MotherName;

                }
                if (model.FatherName != null)
                {
                    entity.FatherName = model.FatherName;

                }
                if (model.NationalID != null)
                {
                    entity.NationalID = model.NationalID;
                }
                if (model.NationalID == null)
                {
                    entity.NationalID = "0";


                }

                if (model.SmartCard != null)
                {
                    entity.SmartCard = model.SmartCard;
                }

                if (model.PhoneNo != null)
                {
                    entity.PhoneNo = model.PhoneNo;

                }
                //if (entity.PhoneNo == null)
                //{
                //    return GetErrorMessageResult("PhoneNo cann't be null");
                //}

                var vNatio = "";
                var vSmat = "";
                var vphone = "";
                if (entity.NationalID == null)
                {
                    vNatio = "0";
                }
                else
                {
                    vNatio = entity.NationalID;
                }
                if (entity.SmartCard == null)
                {
                    vSmat = "0";
                }
                else
                {
                    vSmat = entity.SmartCard;

                }
                if (entity.PhoneNo == null)
                {
                    vphone = "0";
                }
                else
                {
                    vphone = entity.PhoneNo;
                }
                var CheckDupli = new { @NationalID = vNatio, @SmartCard = vSmat, @PhoneNo = vphone, @Qtype = 2, @MemberCode = entity.MemberCode };
                var CheckMemberDupli = ultimateReportService.CheckDuplicateMember(CheckDupli);
                if (CheckMemberDupli.Tables[0].Rows.Count > 0)
                {
                    return GetErrorMessageResult(CheckMemberDupli.Tables[0].Rows[0]["ErrorName"].ToString());
                }

                memberService.Update(entity);
                return GetSuccessMessageResult();


            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        public JsonResult GetMemberInfoJCF(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, string TypeFilterColumn, string DateFromValue, string DateToValue)
        {
            try
            {
                long TotCount;

                var param1 = new { @EmpID = LoggedInEmployeeID };
                var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);
                IEnumerable<DBMemberDetailModel> memberDetail;
                var param2 = new { @orgID = LoggedInOrganizationID, @OfficeID = LoginUserOfficeID, @filterColumnName = filterColumn, @filterValue = filterValue, @TypeFilterColumn = TypeFilterColumn, @DateFrom = DateFromValue, @DateTo = DateToValue };
                var getMemberTolrecord = ultimateReportService.GetMemberDetailsasList(param2);
                var getMember = getMemberTolrecord.Skip(jtStartIndex).Take(jtPageSize);
                TotCount = getMemberTolrecord.Count;
                if (LoanInstallMent != null)
                {
                    var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
                    if (empType == "FO")
                    {
                        var param3 = new { @orgID = LoggedInOrganizationID, @OfficeID = LoginUserOfficeID, @filterColumnName = filterColumn, @filterValue = filterValue, @TypeFilterColumn = TypeFilterColumn, @EmployeeID = Convert.ToInt16(LoggedInEmployeeID), @DateFrom = DateToValue, @DateTo = DateToValue };
                        var getMemberTolrecordEmp = ultimateReportService.GetMemberDetailsasListEmployeeDateWise(param3);
                        var getMemberEmp = getMemberTolrecord.Skip(jtStartIndex).Take(jtPageSize);
                        TotCount = getMemberTolrecordEmp.Count;
                        memberDetail = getMemberEmp;
                    }
                    else
                        memberDetail = getMember;
                }

                else
                    memberDetail = getMember;
                var detail = memberDetail.ToList();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetCenterListJCF(string office_id, string MemberCategoryID)
        {
            StringBuilder sb = new StringBuilder();
            if (MemberCategoryID == "3")
                sb.Append(" AND lower(LEFT(Organizer, 1)) = 'F' ");
            sb.Append(" AND OfficeID = " + office_id);
            // sb.Append(" AND asett.OfficeID = " + SessionHelper.LoginUserOfficeID.Value.ToString());

            // SessionHelper.LoginUserOfficeID.Value
            var officeId = SessionHelper.LoginUserOfficeID;

            List<CenterViewModel> List_ViewModel = new List<CenterViewModel>();
            var param = new { AndCondition = sb.ToString() };
            var List = ultimateReportService.GetDataWithParameter(param, "SP_Get_CenterList");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new CenterViewModel
            {
                CenterID = row.Field<int>("CenterId"),
                CenterCode = row.Field<string>("CenterCode"),
                CenterName = row.Field<string>("CenterName")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = string.Format("{0}, {1}", x.CenterCode, x.CenterName)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);

        }

        #endregion JCF Member

        //============================= For RDRS: 2024-May-02 ==================

        public ActionResult IndividualMemberEmployeeChange()
        {
            var model = new MemberViewModel();
            MapDropDownListForIndividualMemberEmployeeChange(model);
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["MemberList"] = items;
            return View(model);
        }

        private void MapDropDownListForIndividualMemberEmployeeChange(MemberViewModel model)
        {
            var allCenter = centerService.GetAll()
                .Where(w => w.IsActive == true && w.OfficeID == Convert.ToInt32(SessionHelper.LoginUserOfficeID) && w.OrgID == Convert.ToInt32(LoggedInOrganizationID) && w.CenterCode == "0000")
                .OrderBy(o => o.CenterCode);
            var viewCenter = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + ", " + x.CenterName.ToString()
            });
            var center_items = new List<SelectListItem>();
            center_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            center_items.AddRange(viewCenter);
            model.CenterList = center_items;
        }

        public JsonResult GetMemberInfoForEmployeeChange(int jtStartIndex, int jtPageSize, string jtSorting, string CenterId, string MemberId)
        {
            try
            {
                int Center_Id = Convert.ToInt32(CenterId);
                int Member_Id = Convert.ToInt32(string.IsNullOrEmpty(MemberId) ? "0" : MemberId);

                long TotCount;

                List<DBMemberDetailModel> loaneeDetail = new List<DBMemberDetailModel>();
                var param = new { @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Center_Id, @MemberID = Member_Id };
                var ArrearItem = ultimateReportService.GetDataWithParameter(param, "Proc_GetMemberListForEmployeeChange");

                loaneeDetail = ArrearItem.Tables[0].AsEnumerable()
                .Select(row => new DBMemberDetailModel
                {
                    MemberID = row.Field<long>("MemberID"),
                    MemberCode = row.Field<string>("MemberCode"),
                    OfficeID = row.Field<int>("OfficeID"),
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName"),
                    CenterID = row.Field<int>("CenterID"),
                    CenterCode = row.Field<string>("CenterCode"),
                    CenterName = row.Field<string>("CenterName"),
                    FullName = row.Field<string>("FullName"),
                    EmployeeID = row.Field<int>("EmployeeID")
                }).ToList();

                var detail = loaneeDetail.ToList();
                var totCountq = detail.Count();
                var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = totCountq });

            }
            catch (Exception)
            {
                //return Json(new { Result = "ERROR", Message = ex.Message });
                throw;
            }

        }


        public JsonResult GetMemberListDDL(string centerId)
        {
            int offcId = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            try
            {
                List<DBMemberDetailModel> List_ProductViewModel = new List<DBMemberDetailModel>();

                var param = new { @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = centerId, @MemberID = 0 };
                var ArrearItem = ultimateReportService.GetDataWithParameter(param, "Proc_GetMemberListForEmployeeChange");

                List_ProductViewModel = ArrearItem.Tables[0].AsEnumerable()
                .Select(row => new DBMemberDetailModel
                {
                    MemberID = row.Field<long>("MemberID"),
                    MemberCode = row.Field<string>("MemberCode"),
                    FullName = row.Field<string>("FullName")
                }).ToList();

                var members = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.MemberID.ToString(),
                    Text = x.FullName.ToString()
                });
                //var members = memberService.GetAll().Where(w => w.IsActive == true && w.MemberStatus == "1" && w.OrgID == LoggedInOrganizationID && w.OfficeID == offcId && w.CenterID == Convert.ToInt32(centerId) && w.GroupID == Convert.ToInt32(groupId)).Select(c => new { DisplayText = c.MemberCode + ", " + c.FirstName + " " + c.MiddleName + " " + c.LastName, Value = c.MemberID });
                return Json(new { Result = "OK", Options = members });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult GetEmployeeListDDL()
        {
            try
            {
                // Construct a session key based on the organization ID and office ID
                var sessionKey = "EmployeeList_" + LoggedInOrganizationID + "_" + LoginUserOfficeID;

                // Check if the employee list is available in session
                var employees = Session[sessionKey] as IEnumerable<object>;

                if (employees == null)
                {
                    // If not in session, fetch from the database
                    employees = employeeService.GetAll()
                        .Where(w => w.IsActive == true && w.OrgID == LoggedInOrganizationID && w.OfficeID == LoginUserOfficeID)
                        .Select(c => new { DisplayText = c.EmployeeCode + ", " + c.EmpName, Value = c.EmployeeID })
                        .ToList();

                    // Cache the employee list in session
                    Session[sessionKey] = employees;
                }

                return Json(new { Result = "OK", Options = employees });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public ActionResult SaveMemberEmployeeSelected(Dictionary<string, string> allTrx, List<string> allLoanTrxId)
        {
            try
            {

                foreach (var emp in allTrx)
                {
                    string key = emp.Key;
                    long MemberID = Convert.ToInt64(new string(key.Where(char.IsDigit).ToArray()));
                    int EmployeeID = Convert.ToInt32(emp.Value);
                    var param = new { @MemberID = MemberID, @NewEmployeeID = EmployeeID };
                    ultimateReportService.GetDataWithParameter(param, "Proc_IndividualMemberEmployeeChange");
                }

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

    }// END Class
}// ENd Controller


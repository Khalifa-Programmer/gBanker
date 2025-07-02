using AutoMapper;
//using gBanker.Data.Db;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Web.Core.Extensions;
using gBanker.Web.Helpers;
using gBanker.Service.ReportServies;
using System.Data;
using gBanker.Data.DBDetailModels;

namespace gBanker.Web.Controllers
{
    public class CenterController : BaseController
    {
        #region Variables
        private readonly ICenterService centerService;
        private readonly IOfficeService officeService;
        private readonly IGeoLocationService geoLocationService;
        private readonly IEmployeeService employeeService;
        private readonly IGroupService groupService;
        private readonly ILgVillageService lgVillageService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ICountryService countryService;




        private static DataTable dtDivisionList;
        private static DataTable dtDistrictList;
        private static DataTable dtUpozillaList;
        private static DataTable dtUnionList;
        private static DataTable dtVillageList;




        public CenterController(IOfficeService officeService, IGeoLocationService geoLocationService, ICenterService centerService, IEmployeeService employeeService, IGroupService groupService, ILgVillageService lgVillageService, IUltimateReportService ultimateReportService, ICountryService countryService)
        {
            this.centerService = centerService;
            this.officeService = officeService;
            this.geoLocationService = geoLocationService;
            this.centerService = centerService;
            this.employeeService = employeeService;
            this.groupService = groupService;
            this.lgVillageService = lgVillageService;
            this.ultimateReportService = ultimateReportService;
            this.countryService = countryService;
        }
        #endregion

        #region Methods
        private void MapDropDownList(CenterViewModel model)
        {
            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OrgID == LoggedInOrganizationID).ToList();
            // var allOffice = officeService.GetById(offc_id);
            var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.OfficeList = ofc_items;

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

            var allEmp = employeeService.GetAll().Where(e => e.OfficeID == offc_id && e.OrgID == LoggedInOrganizationID && e.EmployeeStatus == 1);
            var viewEmployee = allEmp.ToList().Select(x => new SelectListItem
            {
                Value = x.EmployeeID.ToString(),
                Text = x.EmployeeCode.ToString() + " " + x.EmpName.ToString()
            });
            var emp_items = new List<SelectListItem>();
            emp_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            emp_items.AddRange(viewEmployee);
            model.EmployeeList = emp_items;

            var collDay_item = new List<SelectListItem>();
            collDay_item.Add(new SelectListItem() { Text = "Saturday", Value = "Saturday" });
            collDay_item.Add(new SelectListItem() { Text = "Sunday", Value = "Sunday" });
            collDay_item.Add(new SelectListItem() { Text = "Monday", Value = "Monday" });
            collDay_item.Add(new SelectListItem() { Text = "Tuesday", Value = "Tuesday" });
            collDay_item.Add(new SelectListItem() { Text = "Wednesday", Value = "Wednesday" });
            collDay_item.Add(new SelectListItem() { Text = "Thursday", Value = "Thursday" });
            //collDay_item.Add(new SelectListItem() { Text = "Friday", Value = "Friday" });
            model.CollectionDayList = collDay_item;

            var collType_item = new List<SelectListItem>();
            collType_item.Add(new SelectListItem() { Text = "Regular", Value = "R" });
            collType_item.Add(new SelectListItem() { Text = "Fortnightly", Value = "F" });

            model.CenterCollectionType = collType_item;


            List<CenterViewModel> List_ProductViewModel = new List<CenterViewModel>();
            var param = new { Qtype = 1 };
            var div_items = ultimateReportService.GetCenterTypeList(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new CenterViewModel
            {
                CenterTypeID = row.Field<byte>("CenterTypeID"),
                CenterType = row.Field<string>("CenterType")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterTypeID.ToString(),
                Text = x.CenterType.ToString()
            });
            var d_items = new List<SelectListItem>();
            // d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            model.CenterTypeList = d_items;

            var status_item = new List<SelectListItem>();
            status_item.Add(new SelectListItem() { Text = "Active", Value = "1", Selected = true });
            //status_item.Add(new SelectListItem() { Text = "Silver", Value = "2", Selected = false });
            //status_item.Add(new SelectListItem() { Text = "Copper", Value = "3", Selected = false });
            status_item.Add(new SelectListItem() { Text = "Drop", Value = "0", Selected = false });
            model.CenterStatusList = status_item;

            var org_item = new List<SelectListItem>();
            org_item.Add(new SelectListItem() { Text = "Female", Value = "Female", Selected = true });
            org_item.Add(new SelectListItem() { Text = "Male", Value = "Male", Selected = false });
            org_item.Add(new SelectListItem() { Text = "Both", Value = "Both", Selected = false });
            model.OrganizerList = org_item;


            ////ADDRESS
            ///

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



            // Center Category -- _25

            List<CenterCategoryViewModel> List_CenterCategoryViewModel = new List<CenterCategoryViewModel>();
            var div_items_ = ultimateReportService.GetDataWithoutParameter("sp_GetCenterCategory");

            List_CenterCategoryViewModel = div_items_.Tables[0].AsEnumerable()
            .Select(row => new CenterCategoryViewModel
            {
                CenterCategoryID = row.Field<int>("CenterCategoryID"),
                CenterCategoryName = row.Field<string>("CenterCategoryName")
            }).ToList();

            var List_CenterCategoryDll = List_CenterCategoryViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterCategoryID.ToString(),
                Text = x.CenterCategoryName.ToString()
            });

            var CenterCategoryList = new List<SelectListItem>();
            CenterCategoryList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            CenterCategoryList.AddRange(List_CenterCategoryDll);
            model.CenterCategoryList = CenterCategoryList;

            // END Address



            // END Address








        }
        public JsonResult GetCenterInfo(int jtStartIndex, int jtPageSize, string jtSorting, string EmpId, string CollDay)
        {
            try
            {
                var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
                if (CollDay == "0")
                {
                    CollDay = "";
                }
                if (EmpId == "0")
                {
                    EmpId = "";
                }

                var Center_List = string.Format("CenterList_{0}", (int)SessionHelper.LoginUserOfficeID);
                var centerList = new List<DBCenterDetailModel>();
                if (Session[Center_List] != null)
                    centerList = Session[Center_List] as List<DBCenterDetailModel>;
                else
                {
                    var param = new { OrgId = (int)LoggedInOrganizationID, OfficeId = SessionHelper.LoginUserOfficeID };
                    var alldata = ultimateReportService.GetDataWithParameter(param, "GetCenterData");


                    centerList = alldata.Tables[0].AsEnumerable()
                    .Select(row => new DBCenterDetailModel
                    {
                        CenterID = row.Field<int>("CenterID"),
                        CenterCode = row.Field<string>("CenterCode"),
                        OfficeID = row.Field<int>("OfficeID"),
                        OfficeCode = row.Field<string>("OfficeCode"),
                        OfficeName = row.Field<string>("OfficeName"),
                        OfficeFullName = row.Field<string>("OfficeFullName"),
                        CenterName = row.Field<string>("CenterName"),
                        CenterFullName = row.Field<string>("CenterFullName"),
                        CenterAddress = row.Field<string>("CenterAddress"),
                        CenterNameBng = row.Field<string>("CenterNameBng"),
                        Organizer = row.Field<string>("Organizer"),
                        EmployeeId = row.Field<Int16>("EmployeeId"),
                        EmployeeFullName = row.Field<string>("EmployeeFullName"),
                        CollectionDay = row.Field<string>("CollectionDay"),
                        CollectionDate = row.Field<DateTime>("CollectionDate"),
                        GeoLocationID = row.Field<int?>("GeoLocationID"),
                        LocationName = row.Field<string>("LocationName"),
                        OperationStartDate = row.Field<DateTime>("OperationStartDate"),
                        CenterStatus = row.Field<byte>("CenterStatus"),
                        IsActive = row.Field<bool>("IsActive"),
                        CenterTime = row.Field<DateTime?>("CenterTime"),

                        CenterChief = row.Field<long?>("CenterChief"),
                        AssoCenterChief = row.Field<long?>("AssoCenterChief"),
                        PanelMember = row.Field<long?>("PanelMember"),

                        CenterChiefName = row.Field<string>("CenterChiefName"),
                        AssoCenterChiefName = row.Field<string>("AssoCenterChiefName"),
                        PanelMemberName = row.Field<string>("PanelMemberName"),
                        CenterCategoryName = row.Field<string>("CenterCategoryName"),

                    }).ToList();

                    //centerList = centerService.GetCenterDetail(LoggedInOrganizationID).ToList();
                    Session[Center_List] = centerList;
                }



                var centerDetail = centerList.Where(c => c.IsActive == true && c.OfficeID == offc_id);
                if (EmpId != "" && CollDay != "")
                    centerDetail = centerDetail.Where(w => w.EmployeeId == Convert.ToInt16(EmpId) && w.CollectionDay == CollDay);
                else if (EmpId != "")
                    centerDetail = centerDetail.Where(w => w.EmployeeId == Convert.ToInt16(EmpId));
                else if (CollDay == "0")
                    centerDetail = centerDetail.Where(c => c.IsActive == true && c.OfficeID == offc_id);
                else if (CollDay != "")
                    centerDetail = centerDetail.Where(w => w.CollectionDay == CollDay);
                else centerDetail = centerDetail.Where(c => c.IsActive == true && c.OfficeID == offc_id);
                var detail = centerDetail.ToList();

                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        //public JsonResult GetEmployee()
        //{
        //    try
        //    {
        //        var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);

        //        var empList = employeeService.GetAll().Where(m => m.OfficeID == offc_id && m.OrgID==LoggedInOrganizationID && m.IsActive==true && m.EmployeeStatus==1).Select(c => new { DisplayText = string.Format("{0} {1}", c.EmployeeCode, c.EmpName), Value = c.EmployeeID }).OrderBy(s => s.DisplayText);
        //        return Json(new { Result = "OK", Options = empList });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}


        public JsonResult GetEmployee()
        {
            try
            {
                var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);

                var EmpList = string.Format("EmployeeList_{0}", offc_id);
                var EmployeeList = new List<Employee>();
                if (Session[EmpList] != null)
                    EmployeeList = Session[EmpList] as List<Employee>;
                else
                {
                    var empList = employeeService.GetAll();
                    EmployeeList = empList.ToList();
                    Session[EmpList] = EmployeeList;

                }
                var x = EmployeeList.Where(m => m.OfficeID == offc_id && m.OrgID == LoggedInOrganizationID && m.IsActive == true && m.EmployeeStatus == 1).Select(c => new { DisplayText = string.Format("{0} {1}", c.EmployeeCode, c.EmpName), Value = c.EmployeeID }).OrderBy(s => s.DisplayText);
                return Json(new { Result = "OK", Options = x });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetCollectionDay()
        {

            try
            {
                var Center_List = string.Format("CenterLists_{0}", (int)LoggedInOrganizationID);
                var centerList = new List<Center>();
                if (Session[Center_List] != null)
                    centerList = Session[Center_List] as List<Center>;
                else
                {
                    centerList = centerService.GetAll().ToList();// centerService.GetCenterDetail(LoggedInOrganizationID).ToList();
                    Session[Center_List] = centerList;
                }

                var geoList = centerList.Select(c => new { DisplayText = c.CollectionDay, Value = c.CollectionDay }).Distinct().OrderBy(s => s.DisplayText);

                return Json(new { Result = "OK", Options = geoList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetCollDay()
        {
            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var CenList = centerService.GetAll().Where(c => c.OfficeID == offc_id && c.OrgID == LoggedInOrganizationID).Distinct();
            var viewEmp = CenList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CollectionDay.ToString(),
                Text = x.CollectionDay.ToString()
            }).Distinct();
            var cen_items = new List<SelectListItem>();
            if (viewEmp.ToList().Count > 0)
            {
                cen_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            }
            cen_items.AddRange(viewEmp);
            return Json(cen_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmpList()
        {
            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);

            var EmployeeList = string.Format("Employee_{0}", offc_id);
            var employeeList = new List<Employee>();
            if (Session[EmployeeList] != null)
                employeeList = Session[EmployeeList] as List<Employee>;
            else
            {
                employeeList = employeeService.GetAll().Where(c => c.OfficeID == offc_id && c.OrgID == LoggedInOrganizationID).ToList();
                Session[EmployeeList] = employeeList;

            }


            var empList = employeeList;
            var viewEmp = empList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.EmployeeID.ToString(),
                Text = x.EmployeeCode.ToString() + " " + x.EmpName.ToString()
            });
            var emp_items = new List<SelectListItem>();
            if (viewEmp.ToList().Count > 0)
            {
                emp_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            }
            emp_items.AddRange(viewEmp);
            return Json(emp_items, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Events
        // GET: Center
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmpList"] = items;

            IEnumerable<SelectListItem> CenList = new SelectList(" ");
            ViewData["CenList"] = CenList;
            return View();
        }
        public ActionResult AdminIndex()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmpList"] = items;

            IEnumerable<SelectListItem> CenList = new SelectList(" ");
            ViewData["CenList"] = CenList;
            return View();
        }
        // GET: Center/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Center/Create
        public ActionResult Create()
        {
            var model = new CenterViewModel();
            var centerType = 0;
            MapDropDownList(model);
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            if (model.CenterTypeID == 0)
            {
                centerType = 1;
                model.CenterTypeID = 1;
            }

            else
                centerType = model.CenterTypeID;
            var param1 = new { @OfficeID = model.OfficeID, @CenterTypeID = centerType };
            var LoanInstallMent = ultimateReportService.GetLastCenterCode(param1);
            model.CenterCode = LoanInstallMent.Tables[0].Rows[0]["CenterCode"].ToString();

            return View(model);
        }
        public ActionResult AdminCreate()
        {
            var model = new CenterViewModel();
            var centerType = 0;
            MapDropDownList(model);
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            if (model.CenterTypeID == 0)
            {
                centerType = 1;
                model.CenterTypeID = 1;
            }

            else
                centerType = model.CenterTypeID;
            var param1 = new { @OfficeID = model.OfficeID, @CenterTypeID = centerType };
            var LoanInstallMent = ultimateReportService.GetLastCenterCode(param1);
            model.CenterCode = LoanInstallMent.Tables[0].Rows[0]["CenterCode"].ToString();
            return View(model);
        }
        // POST: Center/Create
        [HttpPost]
        public ActionResult Create(CenterViewModel model)
        {
            try
            {

                var entity = Mapper.Map<CenterViewModel, Center>(model);
                if (ModelState.IsValid)
                {

                    entity.IsActive = true;
                    var errors = centerService.IsValidCenter(entity);
                    if (errors.ToList().Count == 0)
                    {
                        var currentDate = DateTime.Now;

                        var fullDate = Convert.ToDateTime(currentDate.ToString("yyyy-MM-dd")).Add(TimeSpan.Parse(model.CenterTime.Replace(" ", "")));

                        if (entity.EmployeeId == 0)
                        {
                            return GetErrorMessageResult("Pls. Check Employee");
                        }
                        if (entity.GeoLocationID == 0)
                        {
                            return GetErrorMessageResult("Pls. Check GeoLocation");
                        }
                        entity.CenterStatus = 1;
                        entity.CenterTime = fullDate;
                        entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                        centerService.Create(entity);


                        var param1 = new { @OfficeID = model.OfficeID, @CenterTypeID = entity.CenterTypeID };
                        var LoanInstallMent = ultimateReportService.GetLastCenterCode(param1);
                        var centerCode = LoanInstallMent.Tables[0].Rows[0]["CenterCode"].ToString();

                        var Center_List = string.Format("CenterList_{0}", (int)SessionHelper.LoginUserOfficeID);
                        var centerList = new List<DBCenterDetailModel>();
                        Session[Center_List] = null;

                        return GetSuccessMessageResult(centerCode, "Data saved successfully.");
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
        [HttpPost]
        public ActionResult AdminCreate(CenterViewModel model)
        {
            try
            {
                var entity = Mapper.Map<CenterViewModel, Center>(model);
                if (ModelState.IsValid)
                {
                    entity.IsActive = true;
                    var errors = centerService.IsValidCenter(entity);
                    if (errors.ToList().Count == 0)
                    {
                        var currentDate = DateTime.Now;

                        var fullDate = Convert.ToDateTime(currentDate.ToString("yyyy-MM-dd")).Add(TimeSpan.Parse(model.CenterTime.Replace(" ", "")));

                        if (entity.EmployeeId == 0)
                        {
                            return GetErrorMessageResult("Pls. Check Employee");
                        }
                        if (entity.GeoLocationID == 0)
                        {
                            return GetErrorMessageResult("Pls. Check GeoLocation");
                        }
                        entity.CenterStatus = 1;
                        entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                        entity.CenterTime = fullDate;
                        centerService.Create(entity);


                        var param1 = new { @OfficeID = model.OfficeID, @CenterTypeID = entity.CenterTypeID };
                        var LoanInstallMent = ultimateReportService.GetLastCenterCode(param1);
                        var centerCode = LoanInstallMent.Tables[0].Rows[0]["CenterCode"].ToString();

                        var Center_List = string.Format("CenterList_{0}", (int)SessionHelper.LoginUserOfficeID);
                        var centerList = new List<DBCenterDetailModel>();
                        Session[Center_List] = null;


                        return GetSuccessMessageResult(centerCode, "Data saved successfully.");
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
        // GET: Center/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        //public ActionResult AdminEdit(int id)
        //{
        //    return View();
        //}

        public ActionResult AdminEdit(int CenterID) // KHALID Modified
        {
            var entity = centerService.GetById(CenterID);

            var centerType = 0;

            if (entity.CenterTypeID == 0)
            {
                centerType = 1;
                entity.CenterTypeID = 1;
            }

            else
                centerType = entity.CenterTypeID;

            var viewModel = Mapper.Map<Center, CenterViewModel>(entity);

            //model.CountryID         = entity.CountryID;
            //model.DivisionCode      = entity.DivisionCode;
            //model.DistrictCode      = entity.DistrictCode;
            //model.UpozillaCode      = entity.UpozillaCode;
            //model.UnionCode         = entity.UnionCode;
            //model.VillageCode       = entity.VillageCode;
            //model.ZipCode           = entity.ZipCode;
            //model.AddressLine1      = entity.AddressLine1;

            DateTime dt = (DateTime)entity.CenterTime;
            viewModel.CenterTime = dt.ToString("HH:mm");

            MapDropDownList(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AdminEdit(CenterViewModel model)
        {
            try
            {
                var entity = centerService.GetById(model.CenterID);
                if (ModelState.IsValid)
                {
                    var currentDate = DateTime.Now;

                    var fullDate = Convert.ToDateTime(currentDate.ToString("yyyy-MM-dd")).Add(TimeSpan.Parse(model.CenterTime.Replace(" ", "")));

                    entity.CenterName = model.CenterName;
                    entity.CenterNameBng = model.CenterNameBng;
                    entity.EmployeeId = model.EmployeeId;
                    entity.GeoLocationID = model.GeoLocationID;
                    entity.CenterAddress = model.CenterAddress;
                    //entity.CollectionDay = model.CollectionDay;
                    entity.CollectionDate = model.CollectionDate;
                    entity.OperationStartDate = model.OperationStartDate;
                    entity.CenterStatus = model.CenterStatus;
                    entity.Organizer = model.Organizer;
                    entity.CenterTime = fullDate;

                    entity.CountryID = model.CountryID;
                    entity.DivisionCode = model.DivisionCode;
                    entity.DistrictCode = model.DistrictCode;
                    entity.UpozillaCode = model.UpozillaCode;
                    entity.UnionCode = model.UnionCode;
                    entity.VillageCode = model.VillageCode;
                    entity.ZipCode = model.ZipCode;
                    entity.AddressLine1 = model.AddressLine1;

                    entity.CenterChief = model.CenterChief;
                    entity.AssoCenterChief = model.AssoCenterChief;
                    entity.PanelMember = model.PanelMember;
                    entity.CenterCategoryID = model.CenterCategoryID;


                    if (entity.DivisionCode == null || entity.DivisionCode == null || entity.DistrictCode == null ||
                        entity.UpozillaCode == null || entity.UnionCode == null || entity.VillageCode == null
                           || entity.ZipCode == null
                       )
                    {
                        return Json(new { Result = "ERROR", Message = "Please complete address..." });

                    }



                    // Check if Address is Not Complete
                    if (entity.DivisionCode.Length < 2 || entity.DivisionCode.Length < 2 || entity.DistrictCode.Length < 2 ||
                         entity.UpozillaCode.Length < 2 || entity.UnionCode.Length < 2 || entity.VillageCode.Length < 2
                            || entity.ZipCode.Length < 2
                        )
                    {

                        return Json(new { Result = "ERROR", Message = "Please complete address..." });

                    }





                    //InsertCenterLog

                    if (model.CenterChief != null && model.AssoCenterChief != null && model.PanelMember != null)
                    {
                        var param1 = new
                        {
                            @CenterChief = model.CenterChief,
                            @AssoCenterChief = model.AssoCenterChief,
                            @PanelMember = model.PanelMember,
                            @CollectionDay = model.CollectionDay,
                            @EmployeeId = model.EmployeeId,
                            @CenterStatus = model.CenterStatus,
                            @CenterID = model.CenterID,
                            @OfficeID = model.OfficeID,
                            @CreateUser = SessionHelper.LoggedInEmployee.EmployeeCode

                        };
                        var SavedCenterLog = ultimateReportService.SaveCenterLog(param1);
                    }

                    centerService.Update(entity);// Finally Update in Center Table

                    var Center_List = string.Format("CenterList_{0}", (int)SessionHelper.LoginUserOfficeID);
                    var centerList = new List<DBCenterDetailModel>();
                    Session[Center_List] = null;


                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EditCenter(int CenterID) //KHALID New Add
        {
            var entity = centerService.GetById(CenterID);

            var centerType = 0;

            if (entity.CenterTypeID == 0)
            {
                centerType = 1;
                entity.CenterTypeID = 1;
            }

            else
                centerType = entity.CenterTypeID;

            var viewModel = Mapper.Map<Center, CenterViewModel>(entity);

            //model.CountryID         = entity.CountryID;
            //model.DivisionCode      = entity.DivisionCode;
            //model.DistrictCode      = entity.DistrictCode;
            //model.UpozillaCode      = entity.UpozillaCode;
            //model.UnionCode         = entity.UnionCode;
            //model.VillageCode       = entity.VillageCode;
            //model.ZipCode           = entity.ZipCode;
            //model.AddressLine1      = entity.AddressLine1;

            DateTime dt = (DateTime)entity.CenterTime;
            viewModel.CenterTime = dt.ToString("HH:mm");

            MapDropDownList(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditCenter(CenterViewModel model)
        {
            try
            {
                var entity = centerService.GetById(model.CenterID);
                if (ModelState.IsValid)
                {
                    var currentDate = DateTime.Now;
                    var fullDate = Convert.ToDateTime(currentDate.ToString("yyyy-MM-dd")).Add(TimeSpan.Parse(model.CenterTime.Replace(" ", "")));
                    entity.CenterName = model.CenterName;
                    entity.CenterNameBng = model.CenterNameBng;
                    entity.EmployeeId = model.EmployeeId;
                    entity.GeoLocationID = model.GeoLocationID;
                    entity.CenterAddress = model.CenterAddress;
                    entity.CollectionDay = model.CollectionDay;
                    entity.CollectionDate = model.CollectionDate;
                    entity.OperationStartDate = model.OperationStartDate;
                    entity.CenterStatus = model.CenterStatus;
                    entity.Organizer = model.Organizer;
                    entity.CenterTime = fullDate;
                    entity.CountryID = model.CountryID;
                    entity.DivisionCode = model.DivisionCode;
                    entity.DistrictCode = model.DistrictCode;
                    entity.UpozillaCode = model.UpozillaCode;
                    entity.UnionCode = model.UnionCode;
                    entity.VillageCode = model.VillageCode;
                    entity.ZipCode = model.ZipCode;
                    entity.AddressLine1 = model.AddressLine1;
                    entity.CenterChief = model.CenterChief;
                    entity.AssoCenterChief = model.AssoCenterChief;
                    entity.PanelMember = model.PanelMember;
                    entity.CenterCategoryID = model.CenterCategoryID;
                    //InsertCenterLog


                    if (model.CenterChief != null && model.AssoCenterChief != null && model.PanelMember != null)
                    {
                        var param1 = new
                        {
                            @CenterChief = model.CenterChief,
                            @AssoCenterChief = model.AssoCenterChief,
                            @PanelMember = model.PanelMember,
                            @CollectionDay = model.CollectionDay,
                            @EmployeeId = model.EmployeeId,
                            @CenterStatus = model.CenterStatus,
                            @CenterID = model.CenterID,
                            @OfficeID = model.OfficeID,
                            @CreateUser = SessionHelper.LoggedInEmployee.EmployeeCode

                        };
                        var SavedCenterLog = ultimateReportService.SaveCenterLog(param1);
                    }

                    centerService.Update(entity); // Finaly Save in Center Table
                    var Center_List = string.Format("CenterList_{0}", (int)SessionHelper.LoginUserOfficeID);
                    var centerList = new List<DBCenterDetailModel>();
                    Session[Center_List] = null;


                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }





        // POST: Center/Edit/5
        [HttpPost]
        public ActionResult Edit(CenterViewModel model)
        {
            try
            {
                var entity = centerService.GetById(model.CenterID);
                if (ModelState.IsValid)
                {
                    var currentDate = DateTime.Now;
                    var fullDate = Convert.ToDateTime(currentDate.ToString("yyyy-MM-dd")).Add(TimeSpan.Parse(model.CenterTimeOnly.Replace(" ", "")));
                    entity.CenterName = model.CenterName;
                    entity.CenterNameBng = model.CenterNameBng;
                    entity.EmployeeId = model.EmployeeId;
                    entity.GeoLocationID = model.GeoLocationID;
                    entity.CenterAddress = model.CenterAddress;
                    entity.CollectionDay = model.CollectionDay;
                    entity.CollectionDate = model.CollectionDate;
                    entity.OperationStartDate = model.OperationStartDate;
                    entity.CenterStatus = model.CenterStatus;
                    entity.Organizer = model.Organizer;
                    entity.CenterTime = fullDate;
                    centerService.Update(entity);
                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GetLastCenterCode(string centerType)
        {
            var vCenterCode = "";
            if (centerType == null)
            {
                centerType = "1";
            }
            var param1 = new { @OfficeID = LoginUserOfficeID, @CenterTypeID = centerType };
            var centerTypeData = ultimateReportService.GetLastCenterCode(param1);
            // var LoanInstallMent = specialLoanCollectionService.GetAll().Where(l => l.OrgID == LoggedInOrganizationID && l.OfficeID == Convert.ToInt16(officeId) && l.CenterID == Convert.ToInt16(centerId) && l.MemberID == Convert.ToInt64(MemId) && l.ProductID == productid && l.LoanTerm == vlOanTerm  && l.IsActive == true && l.TrxType==trxType).FirstOrDefault();
            if (centerTypeData.Tables[0].Rows.Count > 0)
            {
                vCenterCode = centerTypeData.Tables[0].Rows[0]["CenterCode"].ToString();
            }
            else
            {
                vCenterCode = "";
            }

            var result = new { centerCode = vCenterCode.ToString(), CenterTypeID = centerType };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Center/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: Center/Delete/5
        [HttpPost]
        public ActionResult Delete(CenterViewModel model)
        {
            try
            {
                var entity = centerService.GetById(model.CenterID);
                if (ModelState.IsValid)
                {
                    entity.InActiveDate = DateTime.Now;
                    entity.IsActive = false;
                    centerService.Update(entity);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}

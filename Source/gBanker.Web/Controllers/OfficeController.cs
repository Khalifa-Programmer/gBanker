
#region Usings

using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Web.Core.Extensions;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using System.Data;
using gBanker.Core.Utility;
using gBanker.Data.DBDetailModels;
using gBanker.Core.Filters;

#endregion

namespace gBanker.Web.Controllers
{
    public class OfficeController : BaseController
    {
        #region Private Members
        private readonly IOfficeService officeService;
        private readonly IGeoLocationService geoLocationService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IInvestorService investorService;
        private readonly IUnionService unionService;
        private readonly ICountryService countryService;
        public OfficeController(IOfficeService officeService, IGeoLocationService geoLocationService, IUltimateReportService ultimateReportService, IInvestorService investorService, IUnionService unionService, ICountryService countryService)
        {
            this.officeService = officeService;
            this.geoLocationService = geoLocationService;
            this.ultimateReportService = ultimateReportService;
            this.investorService = investorService;
            this.unionService = unionService;
            this.countryService = countryService;
        }
        #endregion
        #region Methods
        public JsonResult GetOfficeInfo(int jtStartIndex = 0, int jtPageSize = 20, string jtSorting = null)
        {
            try
            {
                //var officeDetail = officeService.GetOfficeDetail();

                var officeDetail = ultimateReportService.GetDataWithoutParameter("GetOfficeDetail").Tables[0];
                var officeDetailList = officeDetail.AsEnumerable().Select(s => new DBOfficeDetailModel
                {
                    OfficeID = s.Field<int>("OfficeID"),
                    OfficeCode = s.Field<string>("OfficeCode"),
                    OfficeName = s.Field<string>("OfficeName"),
                    OfficeLevel = s.Field<byte>("OfficeLevel"),
                    FirstLevel = s.Field<string>("FirstLevel"),
                    SecondLevel = s.Field<string>("SecondLevel"),
                    ThirdLevel = s.Field<string>("ThirdLevel"),
                    FourthLevel = s.Field<string>("FourthLevel"),
                    OperationStartDate = s.Field<DateTime>("OperationStartDate"),
                    OfficeAddress = s.Field<string>("OfficeAddress"),
                    PostCode = s.Field<string>("PostCode"),
                    GeoLocationID = s.Field<int?>("GeoLocationID"),
                    LocationName = s.Field<string>("LocationName"),
                    Email = s.Field<string>("Email"),
                    Phone = s.Field<string>("Phone"),
                    UnionID = s.Field<int?>("UnionID"),
                    UnionName = s.Field<string>("UnionName"),
                    //InvestorID = s.Field<byte?>("InvestorID"),
                    InvestorID = s.Field<int?>("InvestorID"),
                    InvestorName = s.Field<string>("InvestorName")
                });

                var detail = officeDetailList.ToList();
                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        [HttpPost]
        public JsonResult GetGeoLocation()
        {

            try
            {
                var geoList = geoLocationService.GetAll().Select(c => new { DisplayText = c.LocationName, Value = c.GeoLocationID }).OrderBy(s => s.DisplayText);
                return Json(new { Result = "OK", Options = geoList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetOfficeSecondLevel()
        {

            try
            {
                var geoList = officeService.GetAll().Select(c => new { DisplayText = c.SecondLevel, Value = c.SecondLevel }).Distinct().OrderBy(s => s.DisplayText);
                return Json(new { Result = "OK", Options = geoList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetOfficeThirdLevel()
        {

            try
            {
                var geoList = officeService.GetAll().Select(c => new { DisplayText = c.ThirdLevel, Value = c.ThirdLevel }).Distinct().OrderBy(s => s.DisplayText);
                return Json(new { Result = "OK", Options = geoList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetInvestors()
        {
            try
            {
                var investorList = investorService.GetAll().Select(c => new { DisplayText = c.InvestorName, Value = c.InvestorID }).Distinct().OrderBy(s => s.DisplayText);
                return Json(new { Result = "OK", Options = investorList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetUnions()
        {
            try
            {
                var unionList = unionService.GetAll().Select(c => new { DisplayText = c.UnionName, Value = c.UnionID}).Distinct().OrderBy(s => s.DisplayText); //;
                return Json(new { Result = "OK", Options = unionList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }       
        public JsonResult GetParentList(string OfficeCode)
        {
            int orgId = Convert.ToInt16(LoggedInOrganizationID);

            var offc = officeService.GetMany(m => m.OfficeLevel != 4 && m.OrgID == orgId).ToList();
            var officeList = new List<Office>();
            officeList = offc;
            var offce = officeList.Where(m => string.Format("{0} - {1}", m.OfficeCode, m.OfficeName).ToLower().Contains(OfficeCode.ToLower())).Select(m1 => new { m1.OfficeID, OfficeFullName = string.Format("{0} - {1}", m1.OfficeCode, m1.OfficeName), m1.FirstLevel, m1.SecondLevel, m1.ThirdLevel }).ToList();
            return Json(offce, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetParentCodeDetail(string ParentId)
        {

            if (ParentId != "")
            {
                var offc = officeService.GetById(Convert.ToInt32(ParentId));
                var result = new { FirstLevel = offc.FirstLevel, SecondLevel = offc.SecondLevel, ThirdLevel = offc.ThirdLevel };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var offc = officeService.GetAll().FirstOrDefault();
                var result = new { FirstLevel = offc.FirstLevel, SecondLevel = offc.SecondLevel, ThirdLevel = offc.ThirdLevel };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult GetUnionListByFilter(string UnionID)
        {
            var filter = new BaseSearchFilter
            {
                OrganizationId = LoggedInOrganizationID.Value,
                SearchTerm = UnionID
            };
            List<SelectListItem> UnionList = new List<SelectListItem>();
            var uniList = unionService.GetAll().Where(p => p.IsActive == true && p.UnionID == Convert.ToInt32(UnionID));
            var unions = uniList.Where(m => string.Format("{0} - {1}", m.UnionCode, (string.IsNullOrEmpty(m.UnionName) ? "" : m.UnionName)).ToLower().Contains(UnionID.ToLower())).Select(m1 => new { m1.UnionID, UnionName = string.Format("{0} - {1}", m1.UnionCode, (string.IsNullOrEmpty(m1.UnionName) ? "" : m1.UnionName)) }).ToList();
            return Json(unions, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Office New Mapping
        [HttpGet]
        public ActionResult RDOffice()
        {
            return View();
        }
        public JsonResult RegionDivisionCreate(RDOffice obj)
        {
            string msg = "";
            if (string.IsNullOrEmpty(obj.RDCode.Trim())
                && string.IsNullOrEmpty(obj.RDName.Trim()))
                msg = "check required field";
            else
            {
                using (gBankerDbContext db = new gBankerDbContext())
                {
                    var rdObj = db.RDOffice.Where(x => x.RDCode == obj.RDCode || x.RDName == obj.RDName).ToList();
                    if (rdObj.Any())
                    {
                        msg = "Duplicate Code or Name,Pls Check";
                    }
                    else
                    {
                        obj.IsActive = true;
                        obj.CreatedBy = SessionHelper.LoggedInEmployeeID;
                        obj.CreatedIn = DateTime.Now;
                        db.RDOffice.Add(obj);
                        int s = db.SaveChanges();
                        msg = s == 1 ? "Save Success" : "Failed Save";
                    }
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RegionList()
        {
            using (gBankerDbContext db = new gBankerDbContext())
            {
                return Json(db.RDOffice.Where(x => x.IsActive == true && x.ParentID == 0).ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult RegionGrid(int jtStartIndex = 0, int jtPageSize = 20, string jtSorting = null)
        {
            List<RegionDivisionViewModel> viewModel = new List<RegionDivisionViewModel>();
            using (gBankerDbContext db = new gBankerDbContext())
            {
                viewModel = db.Database.SqlQuery<RegionDivisionViewModel>("sp_RegionDivision").ToList();
            }
            var totCount = viewModel.Count();
            var currentPageRecords = viewModel.ToList().Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });
        }
        public JsonResult MappingGrid(int rdID)
        {
            List<RegionXMappingViewModel> viewModel = new List<RegionXMappingViewModel>();
            using (gBankerDbContext db = new gBankerDbContext())
            {
                viewModel = db.Database.SqlQuery<RegionXMappingViewModel>("sp_OfficeMapping " + rdID + "").ToList();
            }
            var totCount = viewModel.Count();
            var currentPageRecords = viewModel.ToList();
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });
        }
        public JsonResult RDOfficeMappingCreate(RDMappingCreateViewModel obj)
        {
            if (obj != null)
            {
                if (obj.rdView != null)
                {
                    try
                    {
                        using (gBankerDbContext db = new gBankerDbContext())
                        {
                            db.Database.ExecuteSqlCommand("UPDATE RDOfficeMapping SET IsActive=0,UpdatedBy=" + SessionHelper.LoggedInEmployeeID
                                + ",UpdatedIn=getdate() WHERE RDID=" + obj.RDID + " AND IsActive=1");
                            var v = obj.rdView;
                            foreach (var m in v)
                            {
                                db.Database.ExecuteSqlCommand("INSERT INTO RDOfficeMapping (RDID,OfficeID,IsActive,CreatedBy,CreatedIn) " +
                                    "VALUES(" + obj.RDID + "," + m.OfficeID + ",1," +
                                    SessionHelper.LoggedInEmployeeID + ",getdate())");
                            }
                        }
                        return Json(new { Result = "OK", Message = "Mapping Complete" });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Result = "Error", Message = ex.Message });
                    }
                }
                else
                    return Json(new { Result = "Error", Message = "Somethong is Wrong" });
            }
            else
                return Json(new { Result = "Error", Message = "Somethong is Wrong" });
        }
        public JsonResult DivisionShow()
        {
            var v = new gBankerDbContext().RDOffice.Where(X => X.IsActive == true && X.ParentID == 0).ToList();
            var viewOffice = v.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.RDID.ToString(),
                Text = x.RDCode.ToString() + " " + x.RDName.ToString()
            });
            var items = new List<SelectListItem>();

            items.AddRange(viewOffice);
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RegionShow(int rdID)
        {
            var items = new List<SelectListItem>();
            if (rdID >= 0)
            {
                var v = new gBankerDbContext().RDOffice.Where(X => X.IsActive == true && X.ParentID == rdID).ToList();
                var viewOffice = v.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.RDID.ToString(),
                    Text = x.RDCode.ToString() + " " + x.RDName.ToString()
                });

                items.AddRange(viewOffice);
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        private void MapDropDownList(OfficeViewModel model)
        {
            var getUnion = ultimateReportService.GetDataWithoutParameter("Get_UnionName_forOffice");
            var unionList = getUnion.Tables[0].AsEnumerable().Select(p => new SelectListItem
            {
                Value = p.Field<int>("UnionID").ToString(),
                Text = p.Field<string>("UnionNameF")
            });
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            items.AddRange(unionList);
            model.UnionList = items;

            var investors = investorService.GetAll();
            var mapInvestors = investors.AsEnumerable().Select(p=> new SelectListItem
            {
                Text = p.InvestorName,
                Value = p.InvestorID.ToString()
            });
            var investorList = new List<SelectListItem>();
            investorList.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            investorList.AddRange(mapInvestors);
            model.InvestorList = investorList;   
        }
        #endregion Office New Mapping
        #region Events

        // GET: Office
        public ActionResult Index()
        {
            //var allOffice = officeService.GetAll();
            //var viewOffice = Mapper.Map<IEnumerable<Office>, IEnumerable<OfficeViewModel>>(allOffice);
            //return View(viewOffice);
            return View();
        }

        // GET: Office/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Office/Create
        public ActionResult Create()
        {
            var model = new OfficeViewModel();
            MapDropDownList(model);            
            return View(model);
        }

        // POST: Office/Create
        [HttpPost]
        public ActionResult Create(OfficeViewModel model, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<OfficeViewModel, Office>(model);
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                    var errors = officeService.IsValidOffice(entity);

                    if (errors.ToList().Count == 0)
                    {
                        if (model.FirstLevel != null)
                        {
                            entity.FirstLevel = model.FirstLevel;
                            if (model.SecondLevel != null)
                            {
                                entity.SecondLevel = model.SecondLevel;
                                if (model.ThirdLevel != null)
                                {
                                    entity.ThirdLevel = model.ThirdLevel;
                                    entity.FourthLevel = model.OfficeCode;
                                    entity.OfficeLevel = 4;
                                }
                                else
                                {
                                    entity.ThirdLevel = model.OfficeCode;
                                    entity.OfficeLevel = 3;
                                }
                            }
                            else
                            {
                                entity.SecondLevel = model.OfficeCode;
                                entity.OfficeLevel = 2;
                            }
                        }
                        else
                        {
                            entity.FirstLevel = model.OfficeCode;
                            entity.OfficeLevel = 1;
                        }
                        entity.IsActive = true;
                        officeService.Create(entity);
                        var ent = new { OfficeID = entity.OfficeID };

                        var param = new { @OfficeID = ent.OfficeID, @TransacDate = System.DateTime.Now };
                        ultimateReportService.AddApplicationSetting(param);

                        return GetSuccessMessageResult();

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

        // GET: Office/Edit/5
        public ActionResult Edit(int id)
        {
            var offc = officeService.GetById(id);
            var offcModel = Mapper.Map<Office, OfficeViewModel>(offc);
            var officeParent = new Office();

            if(offc.OfficeLevel==4)
            officeParent = officeService.GetByOfficeCode(offc.ThirdLevel);
            if (offc.OfficeLevel == 3)
                officeParent = officeService.GetByOfficeCode(offc.SecondLevel);
            if (offc.OfficeLevel == 2)
                officeParent = officeService.GetByOfficeCode(offc.FirstLevel);

            if (officeParent != null)
            {
                offcModel.txtParentCode =$"{officeParent.OfficeCode} - {officeParent.OfficeName}";
                offcModel.ParentId = officeParent.OfficeID.ToString();
            }
            
            MapDropDownList(offcModel);
            return View(offcModel);
        }

        // POST: Office/Edit/5
        [HttpPost]
        public JsonResult Edit(OfficeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = officeService.GetById(model.OfficeID);

                    entity.OfficeName = model.OfficeName;
                    entity.OperationStartDate = model.OperationStartDate;
                    entity.PostCode = model.PostCode;
                    //entity.GeoLocationID = model.GeoLocationID;
                    entity.Email = model.Email;
                    entity.Phone = model.Phone;
                    entity.OfficeAddress = model.OfficeAddress;
                    entity.OfficeLevel = model.OfficeLevel;
                    entity.FirstLevel = model.FirstLevel;
                    entity.SecondLevel = model.SecondLevel;
                    entity.ThirdLevel = model.ThirdLevel;
                    entity.FourthLevel = model.FourthLevel;
                    entity.InvestorID = model.InvestorID;
                    entity.UnionID = model.UnionID;
                    officeService.Update(entity);

                    return GetSuccessMessageResult();
                }
                else {
                    return GetErrorMessageResult();
                }
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult();
            }
        }

        // GET: Office/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Office/Delete/5
        [HttpPost]
        public ActionResult Delete(OfficeViewModel model)
        {
            try
            {
                //model.IsActive = false;
                //model.InActiveDate = DateTime.Now;
                //var offcModel = Mapper.Map<OfficeViewModel, Office>(model);
                //officeService.Update(offcModel);

                var entity = officeService.GetById(model.OfficeID);
                if (ModelState.IsValid)
                {
                    entity.InActiveDate = DateTime.Now;
                    entity.IsActive = false;

                    officeService.Update(entity);
                }

                return Json(new { Result = "OK" });
            }
            catch
            {
                return View();
            }
        }
        #endregion
        #region Ajax Calls
        public JsonResult GetOfficesForDropdownList()
        {
            int officeID = 0;
            var param1 = new { @EmpID = LoggedInEmployeeID };
            var employeeInfo = ultimateReportService.GetCenterROleWise(param1);

            var empType = employeeInfo.Tables[0].Rows[0]["Name"].ToString();
            if (empType != UserRoleConstants.Administrator)
                officeID = (int)LoginUserOfficeID;

            var param = new { @OrgID = LoggedInOrganizationID, @OfficeID = officeID };
            var offices = ultimateReportService.GetOfficesForDropdownList(param);

            var listings = offices.Tables[0].AsEnumerable()
            .Select(row => new SelectListItem
            {
                Value = row.Field<string>("Value"),
                Text = row.Field<string>("Text"),
                Selected = (empType != UserRoleConstants.Administrator)
            }).ToList();

            var officeItems = new List<SelectListItem>();
            officeItems.AddRange(listings);
            return Json(officeItems, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}

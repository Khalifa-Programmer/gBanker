using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Service.ReportExecutionService;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Web.Controllers
{
    public class VillageController : BaseController
    {
        #region Variables
        private readonly ICenterService centerService;
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly ICountryService countryService;
        private readonly IDivisionService divisionService;
        private readonly IDistrictService districtService;
        private readonly IUpozillaService upozillaService;
        private readonly IUnionService unionService;
        private readonly ILgVillageService villageService;
        private readonly IUltimateReportService unlimitedReportService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IGroupwiseReportService groupwiseReportService;

        public VillageController(
            ICenterService centerService,
            IOfficeService officeService,
            IMemberService memberService,
            ICountryService countryService,
            IDivisionService divisionService,
            IDistrictService districtService,
            IUpozillaService upozillaService,
            IUnionService unionService,
            ILgVillageService villageService,
            IUltimateReportService unlimitedReportService,
            IUltimateReportService ultimateReportService,
            IGroupwiseReportService groupwiseReportService
        )
        {
            this.centerService = centerService;
            this.officeService = officeService;
            this.memberService = memberService;
            this.countryService = countryService;
            this.divisionService = divisionService;
            this.districtService = districtService;
            this.upozillaService = upozillaService;
            this.unionService = unionService;
            this.villageService = villageService;
            this.unlimitedReportService = unlimitedReportService;
            this.ultimateReportService = ultimateReportService;
            this.groupwiseReportService = groupwiseReportService;
        }
        #endregion


        public ActionResult Division()
        {
            var model = new DivisionViewModel();
            mapDropdownForDivision(model);
            return View(model);
        }
        public ActionResult District()
        {
            var model = new DistrictViewModel();
            mapDropdownForDistrict(model);
            return View(model);
        }

        public ActionResult Upozilla()
        {
            var model = new UpozillaViewModel();
            mapDropdownForUpozilla(model);
            return View(model);
        }

        public ActionResult Union()
        {
            var model = new UnionViewModel();
            mapDropdownForUnion(model);
            return View(model);
        }

        public ActionResult Village()
        {
            var model = new VillageViewModel();
            mapDropdownForVillage(model);
            return View(model);
        }

        public void mapDropdownForDivision(DivisionViewModel model)
        {
            bool IsActive = true;
            var pram = new { IsActive = IsActive };
            var countryList = groupwiseReportService.GetCountry(pram, "SP_GET_GetCountry"); //SP_GET_GetCountry
            var viewList = countryList.Tables[0].AsEnumerable()
                 .Select((row, index) => new SelectListItem
                 {
                     Text = row.Field<string>("CountryName"),
                     Value = row.Field<int?>("CountryId").ToString()
                 }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            list.AddRange(viewList);
            model.CountryList = list;
        }


        public void mapDropdownForDistrict(DistrictViewModel model)
        {
            bool IsActive = true;
            var pram = new { IsActive = IsActive };
            var officewiseDivisionList = groupwiseReportService.GetDivision(pram, "SP_GET_GetDivision"); //SP_GET_GetActiveCenter
            var viewList = officewiseDivisionList.Tables[0].AsEnumerable()
                 .Select((row, index) => new SelectListItem
                 {
                     Text = row.Field<string>("DivisionCode") + " " + row.Field<string>("DivisionName"),
                     Value = row.Field<byte>("DivisionID").ToString()
                 }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            list.AddRange(viewList);
            model.DivisionList = list;
        }

        public void mapDropdownForUpozilla(UpozillaViewModel model)
        {
            bool IsActive = true;
            var pram = new { IsActive = IsActive };
            var officewiseDistrictList = groupwiseReportService.GetDistrict(pram, "SP_GET_GetDistrict"); //SP_GET_GetActiveCenter
            var viewList = officewiseDistrictList.Tables[0].AsEnumerable()
                 .Select((row, index) => new SelectListItem
                 {
                     Text = row.Field<string>("DistrictCode") + " " + row.Field<string>("DistrictName"),
                     Value = row.Field<int?>("DistrictID").ToString()
                 }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            list.AddRange(viewList);
            model.DistrictList = list;
        }

        public void mapDropdownForUnion(UnionViewModel model)
        {
            bool IsActive = true;
            var pram = new { IsActive = IsActive };
            var officewiseDistrictList = groupwiseReportService.GetDistrict(pram, "SP_GET_GetUpozilla"); //SP_GET_GetActiveCenter
            var viewList = officewiseDistrictList.Tables[0].AsEnumerable()
                 .Select((row, index) => new SelectListItem
                 {
                     Text = row.Field<string>("UpozillaCode") + " " + row.Field<string>("UpozillaName"),
                     Value = row.Field<int?>("UpozillaID").ToString()
                 }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            list.AddRange(viewList);
            model.UpozillaList = list;
        }

        public void mapDropdownForVillage(VillageViewModel model)
        {
            bool cIsActive = true;
            var cpram = new { IsActive = cIsActive };
            var countryList = groupwiseReportService.GetCountry(cpram, "SP_GET_GetCountry"); //SP_GET_GetCountry
            var cviewList = countryList.Tables[0].AsEnumerable()
                 .Select((row, index) => new SelectListItem
                 {
                     Text = row.Field<string>("CountryName"),
                     Value = row.Field<int?>("CountryId").ToString()
                 }).ToList();
            var clist = new List<SelectListItem>();
            clist.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            clist.AddRange(cviewList);
            model.CountryList = clist;

            bool IsActive = true;
            var pram = new { IsActive = IsActive };
            var officewiseDivisionList = groupwiseReportService.GetDivision(pram, "SP_GET_GetDivision"); //SP_GET_GetActiveCenter
            var viewList = officewiseDivisionList.Tables[0].AsEnumerable()
                 .Select((row, index) => new SelectListItem
                 {
                     Text = row.Field<string>("DivisionName"),
                     //Text = row.Field<string>("DivisionCode") + " " + row.Field<string>("DivisionName"),
                     //Value = row.Field<byte>("DivisionID").ToString()
                     Value = row.Field<string>("DivisionCode").ToString()
                 }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            list.AddRange(viewList);
            model.DivisionList = list;

            bool disIsActive = true;
            var dispram = new { IsActive = disIsActive };
            var officewiseDistrictList = groupwiseReportService.GetDistrict(dispram, "SP_GET_GetDistrict"); //SP_GET_GetActiveCenter
            var disviewList = officewiseDistrictList.Tables[0].AsEnumerable()
                 .Select((row, index) => new SelectListItem
                 {
                     Text = row.Field<string>("DistrictName"),
                     //Text = row.Field<string>("DistrictCode") + " " + row.Field<string>("DistrictName"),
                     //Value = row.Field<int?>("DistrictID").ToString()
                     Value = row.Field<string>("DistrictCode").ToString()
                 }).ToList();
            var dislist = new List<SelectListItem>();
            dislist.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            dislist.AddRange(disviewList);
            model.DistrictList = dislist;


            bool upIsActive = true;
            var uppram = new { IsActive = upIsActive };
            var upofficewiseDistrictList = groupwiseReportService.GetDistrict(uppram, "SP_GET_GetUpozilla"); //SP_GET_GetActiveCenter
            var upviewList = upofficewiseDistrictList.Tables[0].AsEnumerable()
                 .Select((row, index) => new SelectListItem
                 {
                     Text = row.Field<string>("UpozillaName"),
                     //Text = row.Field<string>("UpozillaCode") + " " + row.Field<string>("UpozillaName"),
                     //Value = row.Field<int?>("UpozillaID").ToString()
                     Value = row.Field<string>("UpozillaCode").ToString()
                 }).ToList();
            var uplist = new List<SelectListItem>();
            uplist.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            uplist.AddRange(upviewList);
            model.UpozillaList = uplist;


            bool unIsActive = true;
            var unpram = new { IsActive = unIsActive };
            var unofficewiseDistrictList = groupwiseReportService.GetUnion(unpram, "SP_GET_GetUnion"); //SP_GET_GetActiveCenter
            var unviewList = unofficewiseDistrictList.Tables[0].AsEnumerable()
                 .Select((row, index) => new SelectListItem
                 {
                     Text = row.Field<string>("UnionName"),
                     //Text = row.Field<string>("UnionCode") + " " + row.Field<string>("UnionName"),
                     //Value = row.Field<int?>("UnionID").ToString()
                     Value = row.Field<string>("UnionCode").ToString()
                 }).ToList();
            var unlist = new List<SelectListItem>();
            unlist.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            unlist.AddRange(unviewList);
            model.UnionList = unlist;
        }
        


        // Division CRUD  ---------------------------------------------------------------------------------------

        public JsonResult SaveDivision(DivisionViewModel Division)
        {
            var result = string.Empty;
            try
            {

                //var isDuplicate =
                //    divisionService.GetAll()
                //        .Where(
                //            p =>
                //                p.IsActive == true &&
                //                p.DivisionCode.ToUpper().Trim() == Division.DivisionCode.ToUpper().Trim())
                //        .ToList();

                var isDuplicate =
                    divisionService.GetMany(p =>
                                p.IsActive == true &&
                                p.DivisionCode.ToUpper().Trim() == Division.DivisionCode.ToUpper().Trim());
                       

                if (isDuplicate.Any())
                {
                    result = "Duplicate Division Code found, Save denied";
                }
                else
                {
                    //var entity = new Division();
                    //entity.DivisionCode = Division.DivisionCode;
                    //entity.DivisionName = Division.DivisionName;
                    //entity.DivisionAddress = Division.DivisionAddress;
                    //entity.IsActive = true;
                    //entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    //entity.CreateDate = DateTime.UtcNow;
                    //divisionService.Create(entity);

                    var paramInsert = new
                    {
                        DivisionID = 0,
                        DivisionCode = Division.DivisionCode,
                        DivisionName = Division.DivisionName,
                        DivisionAddress = Division.DivisionAddress,
                        CountryId = Division.CountryId,

                        IsActive = true,
                        CreateUser = SessionHelper.LoginUserOfficeID,
                        CreateDate = DateTime.Now
                    };
                    var targetAchievementInsert = groupwiseReportService.GetDivisionInsert(paramInsert, "GetDivisionInsert"); //GetgetTargetAchievementBuroLatestInsert
                    result = "Save Successfull";
                }

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateDivision(DivisionViewModel Division)
        {
            var result = string.Empty;
            try
            {
                //var isDuplicate =
                //   divisionService.GetAll()
                //       .Where(
                //           p =>
                //               p.IsActive == true && p.DivisionID != Division.DivisionID &&
                //               p.DivisionCode.ToUpper().Trim() == Division.DivisionCode.ToUpper().Trim()).ToList();

                var isDuplicate =
                   divisionService.GetMany(p =>
                               p.IsActive == true && p.DivisionID != Division.DivisionID &&
                               p.DivisionCode.ToUpper().Trim() == Division.DivisionCode.ToUpper().Trim());

                if (isDuplicate.Any())
                {
                    result = "Duplicate Division Code found, Update denied";
                }
                else
                {
                    var entity = divisionService.GetById(Convert.ToInt32(Division.DivisionID));
                    //entity.DivisionID = Division.DivisionID;
                    //entity.OldMemberName = Division.OldMemberName;
                    //entity.OfficeID = Division.OfficeID;
                    //entity.CenterID = Division.CenterID;
                    //entity.FatherName = Division.FatherName;
                    //entity.MotherName = Division.MotherName;
                    //entity.SpouseName = Division.SpouseName;
                    //entity.PhoneNo = Division.PhoneNo;
                    //entity.NationalID = Division.NationalID;
                    //entity.Address = Division.Address;
                    //entity.DisburseDate = Division.DisburseDate;
                    //entity.DisburseAmount = Division.DisburseAmount;
                    //entity.WriteOffDate = Division.WriteOffDate;
                    //entity.WriteOffAmount = Division.WriteOffAmount;
                    //entity.WriteOffReceovery = Division.WriteOffReceovery;
                    //entity.OpeningDate = DateTime.Now;
                    //entity.IsActive = true;
                    //entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    //entity.CreateDate = DateTime.UtcNow;
                    //DivisionService.Update(entity);


                    var paramInsert = new
                    {
                        DivisionID = Division.DivisionID,
                        DivisionCode = Division.DivisionCode,
                        DivisionName = Division.DivisionName,
                        DivisionAddress = Division.DivisionAddress,
                        CountryId = Division.CountryId,

                        IsActive = true,
                        CreateUser = SessionHelper.LoginUserOfficeID,
                        CreateDate = DateTime.Now
                    };
                    var targetAchievementInsert = groupwiseReportService.GetDivisionInsert(paramInsert, "GetDivisionInsert"); //GetgetTargetAchievementBuroLatestInsert


                    result = "Update Successfull";
                }
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult ListDivision([DataSourceRequest]Kendo.Mvc.UI.DataSourceRequest request, string DivisionID)
        {
            try
            {
                bool IsActive = true;
                var pram = new { IsActive = IsActive };
                var officewiseDivisionList = groupwiseReportService.GetDivision(pram, "SP_GET_GetDivision"); //SP_GET_GetActiveCenter
                var officewiseDivisionListViewModel = officewiseDivisionList.Tables[0].AsEnumerable()
                   .Select(row => new DivisionViewModel
                   {
                       DivisionID = row.Field<byte?>("DivisionID"),
                       DivisionCode = row.Field<string>("DivisionCode"),
                       DivisionName = row.Field<string>("DivisionName"),
                       DivisionAddress = row.Field<string>("DivisionAddress"),
                       ContactNo = row.Field<string>("ContactNo"),
                       IsActive = row.Field<bool?>("IsActive"),
                       CountryId = row.Field<int?>("CountryId"),
                       CountryName = row.Field<string>("CountryName")
                   }).ToList();

                DataSourceResult result = officewiseDivisionListViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult InformationDeleteDivision(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = divisionService.GetById(Id);
                model.IsActive = false;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                divisionService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        // District CRUD ---------------------------------------------------------------------------------------

        public JsonResult SaveDistrict(DistrictViewModel District)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                    districtService.GetMany(p =>
                                p.IsActive == true &&
                                p.DistrictCode.ToUpper().Trim() == District.DistrictCode.ToUpper().Trim());

                //var isDuplicate =
                //    districtService.GetAll()
                //        .Where(
                //            p =>
                //                p.IsActive == true &&
                //                p.DistrictCode.ToUpper().Trim() == District.DistrictCode.ToUpper().Trim())
                //        .ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate District Code found, Save denied";
                }
                else
                {
                    var paramInsert = new
                    {
                        DistrictID = 0,
                        DistrictCode = District.DistrictCode,
                        DistrictName = District.DistrictName,
                        DivisionID = District.DivisionID,

                        IsActive = true,
                        CreateUser = SessionHelper.LoginUserOfficeID,
                        CreateDate = DateTime.Now
                    };
                    var targetAchievementInsert = groupwiseReportService.GetDistrictInsert(paramInsert, "GetDistrictInsert"); //GetgetTargetAchievementBuroLatestInsert
                    result = "Save Successfull";
                }

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateDistrict(DistrictViewModel District)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                   districtService.GetMany(p =>
                               p.IsActive == true && p.DistrictID != District.DistrictID &&
                               p.DistrictCode.ToUpper().Trim() == District.DistrictCode.ToUpper());

                //var isDuplicate =
                //   districtService.GetAll()
                //       .Where(
                //           p =>
                //               p.IsActive == true && p.DistrictID != District.DistrictID &&
                //               p.DistrictCode.ToUpper().Trim() == District.DistrictCode.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate District Code found, Update denied";
                }
                else
                {
                    var paramInsert = new
                    {
                        DistrictID = District.DistrictID,
                        DistrictCode = District.DistrictCode,
                        DistrictName = District.DistrictName,
                        DivisionID = District.DivisionID,

                        IsActive = true,
                        CreateUser = SessionHelper.LoginUserOfficeID,
                        CreateDate = DateTime.Now
                    };
                    var targetAchievementInsert = groupwiseReportService.GetDistrictInsert(paramInsert, "GetDistrictInsert"); //GetgetTargetAchievementBuroLatestInsert


                    result = "Update Successfull";
                }
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult ListDistrict([DataSourceRequest]Kendo.Mvc.UI.DataSourceRequest request, string DistrictID, string DivisionID)
        {
            try
            {
                bool IsActive = true;
                var pram = new { IsActive = IsActive, DivisionID = DivisionID };
                var officewiseDistrictList = groupwiseReportService.GetDistrict(pram, "SP_GET_GetDistrict"); //SP_GET_GetActiveCenter
                var officewiseDistrictListViewModel = officewiseDistrictList.Tables[0].AsEnumerable()
                   .Select(row => new DistrictViewModel
                   {
                       DistrictID = row.Field<int>("DistrictID"),
                       DistrictCode = row.Field<string>("DistrictCode"),
                       DivisionCode = row.Field<string>("DivisionCode"),
                       DistrictName = row.Field<string>("DistrictName"),
                       IsActive = row.Field<bool?>("IsActive"),
                       DivisionID = row.Field<int?>("DivisionID"),
                       DivisionName = row.Field<string>("DivisionName")
                   }).ToList();

                DataSourceResult result = officewiseDistrictListViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult InformationDeleteDistrict(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = districtService.GetById(Id);
                model.IsActive = false;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                districtService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        


        // Upozilla CRUD ---------------------------------------------------------------------------------------

        public JsonResult SaveUpozilla(UpozillaViewModel Upozilla)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                    upozillaService.GetMany(p =>
                                p.IsActive == true &&
                                p.UpozillaCode.ToUpper().Trim() == Upozilla.UpozillaCode.ToUpper().Trim());

                //var isDuplicate =
                //    UpozillaService.GetAll()
                //        .Where(
                //            p =>
                //                p.IsActive == true &&
                //                p.UpozillaCode.ToUpper().Trim() == Upozilla.UpozillaCode.ToUpper().Trim())
                //        .ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Upozilla Code found, Save denied";
                }
                else
                {
                    var paramInsert = new
                    {
                        UpozillaID = 0,
                        UpozillaCode = Upozilla.UpozillaCode,
                        UpozillaName = Upozilla.UpozillaName,
                        DistrictID = Upozilla.DistrictID,
                        

                        IsActive = true,
                        CreateUser = SessionHelper.LoginUserOfficeID,
                        CreateDate = DateTime.Now
                    };
                    var targetAchievementInsert = groupwiseReportService.GetUpozillaInsert(paramInsert, "GetUpozillaInsert"); //GetgetTargetAchievementBuroLatestInsert
                    result = "Save Successfull";
                }

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateUpozilla(UpozillaViewModel Upozilla)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                   upozillaService.GetMany(p =>
                               p.IsActive == true && p.UpozillaID != Upozilla.UpozillaID &&
                               p.UpozillaCode.ToUpper().Trim() == Upozilla.UpozillaCode.ToUpper());

                //var isDuplicate =
                //   UpozillaService.GetAll()
                //       .Where(
                //           p =>
                //               p.IsActive == true && p.UpozillaID != Upozilla.UpozillaID &&
                //               p.UpozillaCode.ToUpper().Trim() == Upozilla.UpozillaCode.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Upozilla Code found, Update denied";
                }
                else
                {
                    var paramInsert = new
                    {
                        UpozillaID = Upozilla.UpozillaID,
                        UpozillaCode = Upozilla.UpozillaCode,
                        UpozillaName = Upozilla.UpozillaName,
                        DistrictID = Upozilla.DistrictID,

                        IsActive = true,
                        CreateUser = SessionHelper.LoginUserOfficeID,
                        CreateDate = DateTime.Now
                    };
                    var targetAchievementInsert = groupwiseReportService.GetUpozillaInsert(paramInsert, "GetUpozillaInsert"); //GetgetTargetAchievementBuroLatestInsert


                    result = "Update Successfull";
                }
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult ListUpozilla([DataSourceRequest]Kendo.Mvc.UI.DataSourceRequest request, string UpozillaID, string DistrictID)
        {
            try
            {
                bool IsActive = true;
                var pram = new { IsActive = IsActive, DistrictID = DistrictID };
                var officewiseUpozillaList = groupwiseReportService.GetUpozilla(pram, "SP_GET_GetUpozilla"); //SP_GET_GetActiveCenter
                var officewiseUpozillaListViewModel = officewiseUpozillaList.Tables[0].AsEnumerable()
                   .Select(row => new UpozillaViewModel
                   {
                       UpozillaID = row.Field<int>("UpozillaID"),
                       UpozillaCode = row.Field<string>("UpozillaCode"),
                       UpozillaName = row.Field<string>("UpozillaName"),
                       IsActive = row.Field<bool?>("IsActive"),
                       DistrictID = row.Field<int?>("DistrictID"),
                       DistrictCode = row.Field<string>("DistrictCode"),
                       DistrictName = row.Field<string>("DistrictName")
                   }).ToList();

                DataSourceResult result = officewiseUpozillaListViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult InformationDeleteUpozilla(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = upozillaService.GetById(Id);
                model.IsActive = false;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                upozillaService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        // Union CRUD ---------------------------------------------------------------------------------------

        public JsonResult SaveUnion(UnionViewModel Union)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                    unionService.GetMany(p =>
                                p.IsActive == true &&
                                p.UnionCode.ToUpper().Trim() == Union.UnionCode.ToUpper().Trim());

                //var isDuplicate =
                //    UnionService.GetAll()
                //        .Where(
                //            p =>
                //                p.IsActive == true &&
                //                p.UnionCode.ToUpper().Trim() == Union.UnionCode.ToUpper().Trim())
                //        .ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Union Code found, Save denied";
                }
                else
                {
                    var paramInsert = new
                    {
                        UnionID = 0,
                        UnionCode = Union.UnionCode,
                        UnionName = Union.UnionName,
                        UpozillaID = Union.UpozillaID,

                        IsActive = true,
                        CreateUser = SessionHelper.LoginUserOfficeID,
                        CreateDate = DateTime.Now
                    };
                    var targetAchievementInsert = groupwiseReportService.GetUnionInsert(paramInsert, "GetUnionInsert"); //GetgetTargetAchievementBuroLatestInsert
                    result = "Save Successfull";
                }

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateUnion(UnionViewModel Union)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                   unionService.GetMany(p =>
                               p.IsActive == true && p.UnionID != Union.UnionID &&
                               p.UnionCode.ToUpper().Trim() == Union.UnionCode.ToUpper());

                //var isDuplicate =
                //   UnionService.GetAll()
                //       .Where(
                //           p =>
                //               p.IsActive == true && p.UnionID != Union.UnionID &&
                //               p.UnionCode.ToUpper().Trim() == Union.UnionCode.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Union Code found, Update denied";
                }
                else
                {
                    var paramInsert = new
                    {
                        UnionID = Union.UnionID,
                        UnionCode = Union.UnionCode,
                        UnionName = Union.UnionName,
                        UpozillaID = Union.UpozillaID,

                        IsActive = true,
                        CreateUser = SessionHelper.LoginUserOfficeID,
                        CreateDate = DateTime.Now
                    };
                    var targetAchievementInsert = groupwiseReportService.GetUnionInsert(paramInsert, "GetUnionInsert"); //GetgetTargetAchievementBuroLatestInsert


                    result = "Update Successfull";
                }
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult ListUnion([DataSourceRequest]Kendo.Mvc.UI.DataSourceRequest request, string UnionID, string UpozillaID)
        {
            try
            {
                bool IsActive = true;
                var pram = new { IsActive = IsActive, UpozillaID = UpozillaID };
                var officewiseUnionList = groupwiseReportService.GetUnion(pram, "SP_GET_GetUnion"); //SP_GET_GetActiveCenter
                var officewiseUnionListViewModel = officewiseUnionList.Tables[0].AsEnumerable()
                   .Select(row => new UnionViewModel
                   {
                       UnionID = row.Field<int>("UnionID"),
                       UnionCode = row.Field<string>("UnionCode"),
                       UnionName = row.Field<string>("UnionName"),
                       IsActive = row.Field<bool?>("IsActive"),
                       UpozillaID = row.Field<int?>("UpozillaID"),
                       UpozillaCode =  row.Field<string>("UpozillaCode"),
                       UpozillaName = row.Field<string>("UpozillaName")
                   }).ToList();

                DataSourceResult result = officewiseUnionListViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult InformationDeleteUnion(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = unionService.GetById(Id);
                model.IsActive = false;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                unionService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        

        // Village CRUD ---------------------------------------------------------------------------------------

        public JsonResult SaveVillage(VillageViewModel Village)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                    villageService.GetMany(p =>
                                p.IsActive == true &&
                                p.VillageCode.ToUpper().Trim() == Village.VillageCode.ToUpper().Trim());

                if (isDuplicate.Any())
                {
                    result = "Duplicate Village Code found, Save denied";
                }
                else
                {
                    var unionmodel = unionService.GetById(Convert.ToInt32(Village.UnionID));
                    var paramInsert = new
                    {
                        VillageID = 0,
                        VillageCode = Village.VillageCode,
                        VillageName = Village.VillageName,
                        DivisionCode = Village.DivisionCode,
                        DivisionName = Village.DivisionName,
                        DistrictCode = Village.DistrictCode,
                        DistrictName = Village.DistrictName,
                        UpozillaCode = Village.UpozillaCode,
                        UpozillaName = Village.UpozillaName,
                        UnionCode = Village.UnionCode,
                        UnionName = Village.UnionName,

                        CountryID = Village.CountryID,

                        IsActive = true,
                        CreateUser = SessionHelper.LoginUserOfficeID,
                        CreateDate = DateTime.Now
                    };
                    var targetAchievementInsert = groupwiseReportService.GetVillageInsert(paramInsert, "GetVillageInsertNew"); //GetgetTargetAchievementBuroLatestInsert
                    result = "Save Successfull";
                }

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateVillage(VillageViewModel Village)
        {
            var result = string.Empty;
            try
            {
                var isDuplicate =
                   villageService.GetMany(p =>
                               p.IsActive == true && p.LgVillageID != Village.LgVillageID &&
                               p.VillageCode.ToUpper().Trim() == Village.VillageCode.ToUpper());

                //var isDuplicate =
                //   VillageService.GetAll()
                //       .Where(
                //           p =>
                //               p.IsActive == true && p.VillageID != Village.VillageID &&
                //               p.VillageCode.ToUpper().Trim() == Village.VillageCode.ToUpper().Trim()).ToList();
                if (isDuplicate.Any())
                {
                    result = "Duplicate Village Code found, Update denied";
                }
                else
                {
                    var unionmodel = unionService.GetById(Convert.ToInt32(Village.UnionID));
                    var paramInsert = new
                    {
                        VillageID = Village.LgVillageID,
                        VillageCode = Village.VillageCode,
                        VillageName = Village.VillageName,
                        DivisionCode = Village.DivisionCode,
                        DivisionName = Village.DivisionName,
                        DistrictCode = Village.DistrictCode,
                        DistrictName = Village.DistrictName,
                        UpozillaCode = Village.UpozillaCode,
                        UpozillaName = Village.UpozillaName,
                        UnionCode = Village.UnionCode,
                        UnionName = Village.UnionName,

                        CountryID = Village.CountryID,

                        IsActive = true,
                        CreateUser = SessionHelper.LoginUserOfficeID,
                        CreateDate = DateTime.Now
                    };
                    var targetAchievementInsert = groupwiseReportService.GetVillageInsert(paramInsert, "GetVillageInsertNew"); //GetgetTargetAchievementBuroLatestInsert


                    result = "Update Successfull";
                }
            }

            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public JsonResult ListVillage([DataSourceRequest]Kendo.Mvc.UI.DataSourceRequest request, string VillageID, string UnionId = "")
        {
            try
            {
                bool IsActive = true;
                var pram = new { IsActive = IsActive, UnionId = UnionId };
                var officewiseVillageList = groupwiseReportService.GetVillage(pram, "SP_GET_GetVillage"); //SP_GET_GetActiveCenter
                var officewiseVillageListViewModel = officewiseVillageList.Tables[0].AsEnumerable()
                   .Select(row => new VillageViewModel
                   {
                       LgVillageID = row.Field<long>("LgVillageID"),
                       VillageCode = row.Field<string>("VillageCode"),
                       VillageName = row.Field<string>("VillageName"),
                       IsActive = row.Field<bool?>("IsActive"),
                       UnionID = row.Field<int?>("UnionID"),
                       UnionName = row.Field<string>("UnionName"),
                       UnionCode = row.Field<string>("UnionCode"),

                       UpozillaID = row.Field<int?>("UpozillaID"),
                       UpozillaName = row.Field<string>("UpozillaName"),
                       UpozillaCode = row.Field<string>("UpozillaCode"),

                       DistrictID = row.Field<int?>("DistrictID"),
                       DistrictName = row.Field<string>("DistrictName"),
                       DistrictCode = row.Field<string>("DistrictCode"),

                       //DivisionID = row.Field<int?>("DivisionID"),
                       DivisionName = row.Field<string>("DivisionName"),
                       DivisionCode = row.Field<string>("DivisionCode"),

                       CountryID = row.Field<int?>("CountryId"),
                       CountryName = row.Field<string>("CountryName")
                   }).ToList();

                DataSourceResult result = officewiseVillageListViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult InformationDeleteVillage(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = villageService.GetById(Id);
                model.IsActive = false;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                villageService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";
            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }






        // country mapping ------
        public JsonResult coucodewisedivinfo(int id)
        {
            var result = string.Empty;
            var ofc_items = new List<SelectListItem>();
            try
            {
                var couid = countryService.GetById(id);
                int cid = Convert.ToInt32(couid.CountryId);
                var allOffice = divisionService.GetAll().Where(m => m.CountryId == cid).ToList();
                var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.DivisionCode.ToString(),
                    Text = x.DivisionName.ToString()
                });

                ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                ofc_items.AddRange(viewOffice);
            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(ofc_items, JsonRequestBehavior.AllowGet);
        }
        // div mapping ------
        public JsonResult divcodewisedisinfo(string DivisionCode)
        {
            var result = string.Empty;
            var ofc_items = new List<SelectListItem>();
            try
            {
                var divisionid = divisionService.divcodewisedisinfo(DivisionCode).SingleOrDefault();
                int divid = Convert.ToInt32(divisionid.DivisionID);
                var allOffice = districtService.GetAll().Where(m => m.DivisionID == divid).ToList();
                var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.DistrictCode.ToString(),
                    Text = x.DistrictName.ToString()
                });
                
                ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                ofc_items.AddRange(viewOffice);

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(ofc_items, JsonRequestBehavior.AllowGet);
        }
        // dis mapping ------
        public JsonResult discodewiseupoinfo(string DistrictCode)
        {
            var result = string.Empty;
            var ofc_items = new List<SelectListItem>();
            try
            {
                var DistrictID = districtService.discodewiseupoinfo(DistrictCode).SingleOrDefault();
                int disid = Convert.ToInt32(DistrictID.DistrictID);
                var allOffice = upozillaService.GetAll().Where(m => m.DistrictID == disid).ToList();
                var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.UpozillaCode.ToString(),
                    Text = x.UpozillaName.ToString()
                });

                ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                ofc_items.AddRange(viewOffice);

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(ofc_items, JsonRequestBehavior.AllowGet);
        }
        // upo mapping ------
        public JsonResult upocodewiseuniinfo(string UpozillaCode)
        {
            var result = string.Empty;
            var ofc_items = new List<SelectListItem>();
            try
            {
                var UpozillaId = upozillaService.upocodewiseuniinfo(UpozillaCode).SingleOrDefault();
                int upoid = Convert.ToInt32(UpozillaId.UpozillaID);
                var allOffice = unionService.GetAll().Where(m => m.UpozillaID == upoid).ToList();
                var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.UnionCode.ToString(),
                    Text = x.UnionName.ToString()
                });

                ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                ofc_items.AddRange(viewOffice);

            }

            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }
            return Json(ofc_items, JsonRequestBehavior.AllowGet);
        }


    }
}

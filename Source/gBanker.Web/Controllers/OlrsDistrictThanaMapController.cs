using AutoMapper;
using gBanker.Data.CodeFirstMigration;
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
using System.Web.UI.WebControls;

namespace gBanker.Web.Controllers
{
    public class OlrsDistrictThanaMapController : BaseController
    {
        #region Variables       
        private readonly IUltimateReportService ultimateReportService;
        private readonly IDistrictService districtService;
        private readonly IUpozillaService upozillaService;
        private readonly ILgVillageService lgVillageService;

        public OlrsDistrictThanaMapController(IUltimateReportService ultimateReportService, IDistrictService districtService, ILgVillageService lgVillageService,
        IUpozillaService upozillaService)
        {
            this.ultimateReportService = ultimateReportService;
            this.districtService = districtService;
            this.upozillaService = upozillaService;
            this.lgVillageService = lgVillageService;

        }
        #endregion

        #region Event
        public ActionResult Map()
        {
            var model = new DistrictThanaMappingViewModel();
            MapDropdownDistrictThana(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Map(DistrictThanaMappingViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");
            try
            {
                var param = new { @DistrictCode = model.DistrictCode, @OlrsDistrictCode = model.OlrsDistrictCode, @ThanaCode = model.ThanaCode, @OlrsThanaCode = model.OlrsThanaCode, @CreatedBy = SessionHelper.LoggedInEmployeeID.Value };
                ultimateReportService.GetDataWithParameter(param, "[pksf].[DistrictThanaMapping_MapOlrsDistrictAndThana]");
                return GetSuccessMessageResult("District Thana Mapped Successfully");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult("Error! There was an error while adding.");
            }
        }

        #endregion

        #region AjaxCalls

        public JsonResult GetThanaByDistrict(string districtCode)
        {
            var param = new { @DistrictCode = districtCode };
            var thanas = ultimateReportService.GetDataWithParameter(param, "pksf.GetThanaByDistrict");
            var viewThana = thanas.Tables[0].AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.Field<string>("UpozillaName"),
                Value = p.Field<string>("UpozillaCode")
            });
            var thanaList = new List<SelectListItem>();
            thanaList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            thanaList.AddRange(viewThana);
            return Json(thanaList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOlrsThanaByOlrsDistrict(string DistrictCode)
        {
            var param = new { DistrictCode = DistrictCode };
            var olrsThanas = ultimateReportService.GetDataWithParameter(param, "pksf.GetOlrsThanaByOlrsDistrict");
            var viewOlrsThanas = olrsThanas.Tables[0].AsEnumerable().Select(p => new SelectListItem
            {
                Value = p.Field<string>("thana_code"),
                Text = p.Field<string>("ThanaName")
            });
            var olrsThanaList = new List<SelectListItem>();
            olrsThanaList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            olrsThanaList.AddRange(viewOlrsThanas);
            return Json(olrsThanaList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDistrictThanaMappingData(int jtStartIndex, int jtPageSize, string jtSorting, string filterValue, string DistrictCode, string OlrsDistrictCode, string ThanaCode, string OlrsThanaCode)
        {
            try
            {
                var param = new { DistrictCode = DistrictCode, OlrsDistrictCode = OlrsDistrictCode, ThanaCode = ThanaCode, OlrsThanaCode = OlrsThanaCode };
                var getData = ultimateReportService.GetDataWithParameter(param, "pksf.GetDistrictThanaMappingData");
                var detail = getData.Tables[0].AsEnumerable().Select(p => new DistrictThanaMappingViewModel
                {
                    Id = p.Field<int>("Id"),
                    DistrictName = p.Field<string>("District"),
                    OlrsDistrictCode = p.Field<string>("OlrsDistrict"),
                    ThanaName = p.Field<string>("Thana"),
                    OlrsThanaCode = p.Field<string>("OlrsThana")
                }).ToList();
                var currentPageRecords = detail.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = detail.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion

        #region Private Methods
        private void MapDropdownDistrictThana(DistrictThanaMappingViewModel model)
        {
            //var districts = districtService.GetMany(p => p.IsActive == true).OrderBy(p => p.DistrictName);
            var param = new { @DistrictCode = "" };
            var districts = ultimateReportService.GetDataWithParameter(param, "Proc_GetLgVillageDistrict");
            var districtList = districts.Tables[0].AsEnumerable().Select(p => new SelectListItem
            {
                //Text = p.DistrictCode + " - " + p.DistrictName,
                //Value = p.DistrictCode.ToString()
                Text = p.Field<string>("district_code") + " - " + p.Field<string>("district_name"),
                Value = p.Field<string>("district_code")
            });
            var disList = new List<SelectListItem>();
            disList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            disList.AddRange(districtList);
            model.DistrictList = disList;

            var olrsDistricts = ultimateReportService.GetDataWithoutParameter("pksf.GetOlrsDistrict");
            var viewOlrsDistricts = olrsDistricts.Tables[0].AsEnumerable().Select(p => new SelectListItem
            {
                Value = p.Field<string>("district_code"),
                Text = p.Field<string>("district_name"),
            }).ToList();
            var OlrsDistrictList = new List<SelectListItem>();
            OlrsDistrictList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            OlrsDistrictList.AddRange(viewOlrsDistricts);
            model.OlrsDistrictList = OlrsDistrictList;

            var thanaList = new List<SelectListItem>();
            thanaList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            model.ThanaList = thanaList;

            var olrsThanaList = new List<SelectListItem>();
            olrsThanaList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            model.OlrsThanaList = olrsThanaList;
        }

        #region Delete

        [HttpDelete]
        public JsonResult DeleteDistrictMap(int Id)
        {
            try
            {

                var param = new { @Id = Id };
                var getData = ultimateReportService.GetDataWithParameter(param, "pksf.DeleteDistrictThanaMappingData");
                return GetSuccessMessageResult("District deleted successfully");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion
    }
}

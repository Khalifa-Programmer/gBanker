using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class AccChartMappingController : BaseController
    {
        #region Variables
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IAccChartService accChartService;


        public AccChartMappingController(IOfficeService officeService, IAccChartService accChartService, IUltimateReportService ultimateReportService)
        {
            this.officeService = officeService;
            this.accChartService = accChartService;
            this.ultimateReportService = ultimateReportService;
        }
        #endregion

        // GET: ProductMapping
        public ActionResult Set()
        {
            ProdAccMappingViewModel model = new ProdAccMappingViewModel();

            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            //var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OfficeID == offc_id && m.OrgID == LoggedInOrganizationID).ToList();
            var allOffice = officeService.GetAll();

            // var allOffice = officeService.GetById(offc_id);
            var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            //var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = string.Format("{0}, {1}", x.OfficeCode.ToString(), x.OfficeName.ToString())
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.OfficeList = ofc_items;

            List<SelectListItem> items = new List<SelectListItem>();
            ViewData["KOfficeList"] = items;

            return View(model);
        }
      
        public ActionResult SetCash()
        {
            ProdAccMappingViewModel model = new ProdAccMappingViewModel();

            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            //var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OfficeID == offc_id && m.OrgID == LoggedInOrganizationID).ToList();
            var allOffice = officeService.GetAll();

            // var allOffice = officeService.GetById(offc_id);
            var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            //var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = string.Format("{0}, {1}", x.OfficeCode.ToString(), x.OfficeName.ToString())
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.OfficeList = ofc_items;

            List<SelectListItem> items = new List<SelectListItem>();
            ViewData["KOfficeList"] = items;

            return View(model);
        }



        #region Methods
        public JsonResult GetAvailableAccChartList(string OfficeID, string FirstLevel, string SecondLevel, string ThirdLevel)
        {
            try
            {
                List<AccChartViewModel> List_AccChartInfoViewModel = new List<AccChartViewModel>();
                if (Convert.ToInt32(OfficeID) > 0 && OfficeID != "")
                {


                    var param = new { OfficeID = Convert.ToInt32(OfficeID), FirstLevel = FirstLevel, SecondLevel = SecondLevel, ThirdLevel = ThirdLevel };
                    var officeList = ultimateReportService.GetAccChartListForMapping(param);

                    List_AccChartInfoViewModel = officeList.Tables[0].AsEnumerable()
                    .Select(row => new AccChartViewModel
                    {
                        Sl = row.Field<int>("Sl"),
                        AccID = row.Field<int>("AccID"),
                        AccCode = row.Field<string>("AccCode"),
                        AccName = row.Field<string>("AccName")
                    }).ToList();
                }
                var jsonResult = Json(List_AccChartInfoViewModel.ToList(), JsonRequestBehavior.AllowGet);                jsonResult.MaxJsonLength = int.MaxValue;                return jsonResult;
                //return Json(List_AccChartInfoViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOfficeByLevel(string Qtype = "1", string FirstLevel = "0", string SecondLevel = "0")
        {
            List<AccChartViewModel> List_AccChartInfoViewModel = new List<AccChartViewModel>();

            var param = new { Qtype = Convert.ToInt32(Qtype), FirstLevel = FirstLevel, SecondLevel = SecondLevel };
            var officeList = ultimateReportService.GetOfficeByLevel(param);

            List_AccChartInfoViewModel = officeList.Tables[0].AsEnumerable()
            .Select(row => new AccChartViewModel
            {

                AccID = row.Field<int>("AccID"),
                AccCode = row.Field<string>("AccCode"),
                AccName = row.Field<string>("AccName")
            }).ToList();


            var viewZOOffice = List_AccChartInfoViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.AccCode.ToString(),
                Text = string.Format("{0} - {1}", x.AccCode, x.AccName)
            });

            var zoOffice_items = new List<SelectListItem>();
            if (viewZOOffice.ToList().Count > 0)
            {
                zoOffice_items.Add(new SelectListItem() { Text = "SELECT ALL ", Value = "0", Selected = true });
            }
            zoOffice_items.AddRange(viewZOOffice);
            return Json(zoOffice_items, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetAvailableAccChartList(string OfficeID)
        //{
        //    try
        //    {
        //        List<AccChartViewModel> List_AccChartInfoViewModel = new List<AccChartViewModel>();
        //        if (Convert.ToInt32(OfficeID) > 0 && OfficeID != "")
        //        {


        //            var param = new { OfficeID = Convert.ToInt32(OfficeID), FirstLevel = "1", secondLevel="2000", thirdLevel="2007" };
        //            var officeList = ultimateReportService.GetAccChartListForMapping(param);

        //            List_AccChartInfoViewModel = officeList.Tables[0].AsEnumerable()
        //            .Select(row => new AccChartViewModel
        //            {
        //                Sl = row.Field<int>("Sl"),
        //                AccID = row.Field<int>("AccID"),
        //                AccCode = row.Field<string>("AccCode"),
        //                AccName = row.Field<string>("AccName")
        //            }).ToList();
        //        }
        //        return Json(List_AccChartInfoViewModel.ToList(), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public JsonResult GetSelectedAccChartList(string OfficeID)
        {
            try
            {
                List<AccChartViewModel> List_AccChartInfoViewModel = new List<AccChartViewModel>();
                if (Convert.ToInt32(OfficeID) > 0 && OfficeID != "")
                {
                    var param = new { OfficeID = Convert.ToInt32(OfficeID) };
                    var officeList = ultimateReportService.GetSelectedAccChartListForMapping(param);

                    List_AccChartInfoViewModel = officeList.Tables[0].AsEnumerable()
                    .Select(row => new AccChartViewModel
                    {
                        Sl = row.Field<int>("Sl"),
                        AccID = row.Field<int>("AccID"),
                        AccCode = row.Field<string>("AccCode"),
                        AccName = row.Field<string>("AccName")
                    }).ToList();
                }

                var jsonResult = Json(List_AccChartInfoViewModel, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;                return jsonResult;

                //return Json(List_AccChartInfoViewModel.ToList(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult OfficeWiseAccChartSave(List<string> allAccChartIds, string OfficeID)
        {
            int dealOfficeId = 0;
            if (OfficeID != "")
            {

                var officeID = Convert.ToInt32(OfficeID);
                dealOfficeId = officeID;
                foreach (var AccChartId in allAccChartIds)
                {
                    var param = new { OfficeID = Convert.ToInt32(OfficeID), AccID = AccChartId };
                    var officeList = ultimateReportService.SaveAccChartMapping(param);
                }
                return Json(dealOfficeId, JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }
        public JsonResult OfficeWiseAccChartDelete(List<string> AccChartIds, string OfficeID)
        {
            int dealOfficeId = 0;
            if (OfficeID != "")
            {

                var officeID = Convert.ToInt32(OfficeID);
                dealOfficeId = officeID;
                foreach (var AccChartId in AccChartIds)
                {
                    var param = new { OfficeID = Convert.ToInt32(OfficeID), AccID = AccChartId };
                    var officeList = ultimateReportService.DeleteAccChartMapping(param);

                }
                return Json(dealOfficeId, JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }



        #endregion Methods


        #region MethodsCash

        public JsonResult GetAvailableAccChartListCash(string OfficeID, string FirstLevel, string SecondLevel, string ThirdLevel)
        {
            try
            {
                List<AccChartViewModel> List_AccChartInfoViewModel = new List<AccChartViewModel>();
                if (Convert.ToInt32(OfficeID) > 0 && OfficeID != "")
                {


                    var param = new { OfficeID = Convert.ToInt32(OfficeID), FirstLevel = FirstLevel, SecondLevel = SecondLevel, ThirdLevel = ThirdLevel };
                    var officeList = ultimateReportService.GetAccChartListForMapping(param);

                    List_AccChartInfoViewModel = officeList.Tables[0].AsEnumerable()
                    .Select(row => new AccChartViewModel
                    {
                        Sl = row.Field<int>("Sl"),
                        AccID = row.Field<int>("AccID"),
                        AccCode = row.Field<string>("AccCode"),
                        AccName = row.Field<string>("AccName")
                    }).ToList();
                }
                return Json(List_AccChartInfoViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOfficeByLevelCash(string Qtype = "1", string FirstLevel = "0", string SecondLevel = "0")
        {
            List<AccChartViewModel> List_AccChartInfoViewModel = new List<AccChartViewModel>();

            var param = new { Qtype = Convert.ToInt32(Qtype), FirstLevel = FirstLevel, SecondLevel = SecondLevel };
            var officeList = ultimateReportService.GetOfficeByLevelCash(param);

            List_AccChartInfoViewModel = officeList.Tables[0].AsEnumerable()
            .Select(row => new AccChartViewModel
            {

                AccID = row.Field<int>("AccID"),
                AccCode = row.Field<string>("AccCode"),
                AccName = row.Field<string>("AccName")
            }).ToList();


            var viewZOOffice = List_AccChartInfoViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.AccCode.ToString(),
                Text = string.Format("{0} - {1}", x.AccCode, x.AccName)
            });

            var zoOffice_items = new List<SelectListItem>();
            if (viewZOOffice.ToList().Count > 0)
            {
                zoOffice_items.Add(new SelectListItem() { Text = "SELECT ALL ", Value = "0", Selected = true });
            }
            zoOffice_items.AddRange(viewZOOffice);
            return Json(zoOffice_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSelectedAccChartListCash(string OfficeID)
        {
            try
            {
                List<AccChartViewModel> List_AccChartInfoViewModel = new List<AccChartViewModel>();
                if (Convert.ToInt32(OfficeID) > 0 && OfficeID != "")
                {
                    var param = new { OfficeID = Convert.ToInt32(OfficeID) };
                    var officeList = ultimateReportService.GetSelectedAccChartListForMappingCash(param);

                    List_AccChartInfoViewModel = officeList.Tables[0].AsEnumerable()
                    .Select(row => new AccChartViewModel
                    {
                        Sl = row.Field<int>("Sl"),
                        AccID = row.Field<int>("AccID"),
                        AccCode = row.Field<string>("AccCode"),
                        AccName = row.Field<string>("AccName")
                    }).ToList();
                }
                return Json(List_AccChartInfoViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult OfficeWiseAccChartSaveCash(List<string> allAccChartIds, string OfficeID)
        {
            int dealOfficeId = 0;
            if (OfficeID != "")
            {

                var officeID = Convert.ToInt32(OfficeID);
                dealOfficeId = officeID;
                foreach (var AccChartId in allAccChartIds)
                {
                    var param = new { OfficeID = Convert.ToInt32(OfficeID), AccID = AccChartId };
                    var officeList = ultimateReportService.SaveAccChartMappingCash(param);
                }
                return Json(dealOfficeId, JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }
        public JsonResult OfficeWiseAccChartDeleteCash(List<string> AccChartIds, string OfficeID)
        {
            int dealOfficeId = 0;
            if (OfficeID != "")
            {

                var officeID = Convert.ToInt32(OfficeID);
                dealOfficeId = officeID;
                foreach (var AccChartId in AccChartIds)
                {
                    var param = new { OfficeID = Convert.ToInt32(OfficeID), AccID = AccChartId };
                    var officeList = ultimateReportService.DeleteAccChartMappingCash(param);

                }
                return Json(dealOfficeId, JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        #endregion MethodsCash



    }// End Class
}// End Namespace
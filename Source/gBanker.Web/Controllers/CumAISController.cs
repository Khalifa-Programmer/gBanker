using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using AutoMapper;

namespace gBanker.Web.Controllers
{
    public class CumAISController : BaseController
    {
        private readonly IOfficeService officeService;
        private readonly IAccChartService accChartService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ICumAISService CumAISService;
        public CumAISController(IOfficeService officeService, ICumAISService CumAISService, IUltimateReportService ultimateReportService, IAccChartService accChartService)
        {
            this.CumAISService = CumAISService;
            this.officeService = officeService;
            this.accChartService = accChartService;
            this.ultimateReportService = ultimateReportService;
          
        }

        private void MapDropDownList(CumAISViewModel model)
        {
            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OrgID == LoggedInOrganizationID && m.OfficeID == LoginUserOfficeID).ToList();
            var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.OfficeList = ofc_items;

            var type_item = new List<SelectListItem>();
            type_item.Add(new SelectListItem() { Text = "Debit", Value = "Dr" });
            type_item.Add(new SelectListItem() { Text = "Credit", Value = "Cr" });
            type_item.Add(new SelectListItem() { Text = "Journal", Value = "Jr" });
            type_item.Add(new SelectListItem() { Text = "Payable", Value = "Py" });
            model.VoucherTypeList = type_item;

            List<ReconPurpose> List_ProductViewModel = new List<ReconPurpose>();
            var param = new { OfficeID = LoginUserOfficeID };
            var div_items = ultimateReportService.GetReconPurposeList(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new ReconPurpose
            {

                ReconPurposeCode = row.Field<string>("ReconPurposeCode"),
                ReconPurposeName = row.Field<string>("ReconPurposeName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ReconPurposeCode.ToString(),
                Text = x.ReconPurposeCode.ToString() + " " + x.ReconPurposeName.ToString()
            });

            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            model.ReconPurposeList = d_items;


            var chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.ModuleID == 1).ToList();
            var viewChart = chart.Select(x => x).ToList().Select(x => new SelectListItem
             {
                 Value = x.AccCode.ToString(),
                 Text = x.AccCode.ToString() + " " + x.AccName.ToString()
             });
            var Acc_items = new List<SelectListItem>();
            Acc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            Acc_items.AddRange(viewChart);
            model.AccCodeList = Acc_items;
        }
        public JsonResult GetAccCode(string acc_code)
        {
            IEnumerable<AccChart> chart;

           
                chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true  && m.ModuleID == 1).ToList();
                var chartList = new List<AccChart>();
                chartList = chart.ToList();
                var acc = chartList.Where(m => string.Format("{0} - {1}", m.AccCode, m.AccName).ToLower().Contains(acc_code.ToLower())).Select(m1 => new { m1.AccCode, AccFullName = string.Format("{0} - {1}", m1.AccCode, m1.AccName) }).ToList();
                return Json(acc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCumAISInfo(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {

                var getCum = CumAISService.GetCumAISInfo(LoginUserOfficeID, filterColumn, filterValue);
                var detail = getCum.ToList();
                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<Proc_Get_CUMAIS_Result>, IEnumerable<CumAISViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });



            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        //
        // GET: /CumAIS/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /CumAIS/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /CumAIS/Create
        public ActionResult Create()
        {
            var model = new CumAISViewModel();
            if (IsDayInitiated)
                model.AISDate = TransactionDate;

            MapDropDownList(model);

            return View(model);
        }

        //
        // POST: /CumAIS/Create
        [HttpPost]
        public ActionResult Create(CumAISViewModel model, FormCollection form)
        {
            try
            {


                var entity = Mapper.Map<CumAISViewModel, CumAI>(model);
                //Add Validlation Logic.
                if (ModelState.IsValid)
                {

                    entity.OfficeID = Convert.ToString( LoginUserOfficeID);
                    CumAISService.Create(entity);
                    return GetSuccessMessageResult();

                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        //
        // GET: /CumAIS/Edit/5
        public ActionResult Edit(int id)
        {
            var product = CumAISService.GetById(id);
            var entity = Mapper.Map<CumAI, CumAISViewModel>(product);
            MapDropDownList(entity);
            return View(entity);
        }

        //
        // POST: /CumAIS/Edit/5
        [HttpPost]
        public ActionResult Edit(CumAISViewModel model)
        {
            try
            {

                var entity = Mapper.Map<CumAISViewModel, CumAI>(model);
                var getproduct = CumAISService.GetById(entity.CumAisID);
                //// TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    getproduct.AISDate = entity.AISDate;
                    getproduct.VoucherNo = entity.VoucherNo;
                    getproduct.OfficeID = Convert.ToString( LoginUserOfficeID);
                    getproduct.AccCode = entity.AccCode;
                    getproduct.Naration = entity.Naration;
                    getproduct.ReconPurposeCode = entity.ReconPurposeCode;
                    getproduct.Reference = entity.Reference;
                    getproduct.Debit = entity.Debit;
                    getproduct.Credit = entity.Credit;
                    getproduct.VoucherType = entity.VoucherType;
                    CumAISService.Update(getproduct);
                    return GetSuccessMessageResult();

                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        //
        // GET: /CumAIS/Delete/5
        public ActionResult Delete(int id)
        {
            CumAISService.Delete(id);
            return RedirectToAction("Index");
        }

        //
        // POST: /CumAIS/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                CumAISService.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}


#region Usings

using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace gBanker.Web.Controllers
{
    public class LoanSavingsRateMappingController : BaseController
    {
        #region Private Members

        private readonly IUltimateReportService ultimateReportService;

        #endregion

        #region Ctor

        public LoanSavingsRateMappingController(IUltimateReportService ultimateReportService)
        {
            this.ultimateReportService = ultimateReportService;
        }

        #endregion

        #region List
       
        public JsonResult GetLoanSavingsRateMappingList(int jtStartIndex, int jtPageSize, string jtSorting, string filterValue, string LoanSavingRateType,string searchTerm)
        {
            try
            {
                var param = new { LoanSavingRateType = LoanSavingRateType, searchTerm= searchTerm, OrgCode = LoggedInOrganizationCode };
                var getData = ultimateReportService.GetDataWithParameter(param, "pksf.GetLoanSavingsRateMappingList");
                var detail = getData.Tables[0].AsEnumerable().Select(p => new LoanSavingRateMappingViewModel
                {
                    ProductName = p.Field<string>("PRODUCT_NAME"),
                    Rate = p.Field<int>(LoanSavingRateType == "Loan Service Charge Rate" ? "SC_RATE" : "INT_RATE")
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

        #region Map

        [HttpGet]
        public ActionResult Map()
        {
            var model = new LoanSavingRateMappingViewModel { };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Map(LoanSavingRateMappingViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");
            try
            {
                var param = new
                {
                    @MappingType = model.LoanSavingRateType,
                    @PO_CODE = SessionHelper.LoggedInOrganizationCode,
                    @PRODUCT_NAME = model.ProductName,
                    @RATE = model.Rate,
                    @CREATED_BY = SessionHelper.LoginUserEmployeeID
                };

                //let's add into db
                ultimateReportService.GetDataWithParameter(param, "[pksf].[LoanSavingsRateMapping_InsertDataFromUI]");

                return GetSuccessMessageResult();
            }
            catch(Exception ex) 
            {
                return GetErrorMessageResult("Error! There was an error while adding.");
            }
        }
        #endregion

        #region Ajax Calls

        public JsonResult GetProductByType(string loanSavingRateType)
        {
            var param = new { MappingType = loanSavingRateType };
            var productList = ultimateReportService.GetDataWithParameter(param, "[pksf].[LoanSavingsRateMapping_GetProductsByType]");
            var viewProductList = productList.Tables[0].AsEnumerable().Select(p => new SelectListItem()
            {
                 Text = p.Field<string>("ProductName"),
                 Value = p.Field<string>("ProductName")
            });
            return Json(viewProductList, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
    }
}
#region Usings

using gBanker.Core.Filters;
using gBanker.Core.Utility;
using gBanker.Data.DBDetailModels.OLRSHubs.CommonModels;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace gBanker.Web.Controllers
{
    public class ProgramDataController : BaseController
    {
        #region Private Members

        private readonly IIndicatorService indicatorService;
        public readonly IOrganizationService organizationService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IPOProductMappingService pOProductMappingService;
        #endregion

        #region Ctor
        public ProgramDataController(IIndicatorService indicatorService, IOrganizationService organizationService, IUltimateReportService ultimateReportService, IPOProductMappingService pOProductMappingService)
        {
            this.indicatorService = indicatorService;
            this.organizationService = organizationService;
            this.ultimateReportService = ultimateReportService;
            this.pOProductMappingService = pOProductMappingService;
        }
        #endregion

        #region Add New
        public async Task<ActionResult> AddNew()
        {
            var model = new ProgramDataAddViewModel
            {
                IndicatorList = await GetIndicatorList(),
                LoanCodeWiseProductList = await GetLoanCodeWiseProductList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddNew(ProgramDataAddViewModel model)
        {
            bool isOperationSuccess = true;
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields.");

            if (!model.ProgramDataLoanCodeWiseValueList.Any())
                return GetErrorMessageResult("Warning! You must give atleast one product amount");

            //var totalAmount = model.ProgramDataLoanCodeWiseValueList.Sum(s => s.Amount);

            //if (totalAmount == 0)
            //return GetErrorMessageResult("Warning! You must give atleast one product amount");

            var pdLoanCodeWiseValueList = model.ProgramDataLoanCodeWiseValueList
                .GroupBy(g => g.LoanCode)
                .Select(s => new { LoanCode = s.FirstOrDefault().LoanCode, Amount = s.Sum(sm => sm.Amount) }).ToList();
           
            foreach (var item in pdLoanCodeWiseValueList)
            {
                var pdDataManualData = new ProgramDataManualDataModel
                {
                    OrganizationCode = LoggedInOrganizationCode,
                    MNYR = model.MNYR,
                    IndCode = model.IndicatorCode,
                    LoanCode = item.LoanCode,
                    M_F_FLAG = model.M_F_Flag,
                    Amount =item.Amount,
                    InsertUser = LoggedInEmployeeID.ToString()
                };

                //let's add into db for [PRA_MN_RPT_TAB_XL_FD]
                var isAdded = await indicatorService.AddManualProgramData(pdDataManualData);
                if (!isAdded) isOperationSuccess = false;
            }
                        
            if (!isOperationSuccess)
                return GetErrorMessageResult("Warning! There was an error while adding.");

            return GetSuccessMessageResult();
        }

        #endregion

        #region Delete
        public ActionResult DeleteProgramData(string IndicatorCode, string MNYR)
        {
            try
            {
                var param = new { @IndicatorCode = IndicatorCode, @MNYR = MNYR };
                ultimateReportService.GetDataWithParameter(param, "pksf.PRA_MN_RPT_TAB_XL_PD_DeleteData");
                return GetSuccessMessageResult("Scuccess! Data Deleted.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Ajax Call

        #endregion

        #region Private Methods

        private async Task<IEnumerable<SelectListItem>> GetIndicatorList()
        {
            var filter = new IndicatorSearchFilter { AssociatedTable = OLRSRelatedConstants.PRA_MN_RPT_TAB_XL_PD };

            var indicators = await indicatorService.GetIndicatorListByIsManual(filter);

            var indicatorDdlList = indicators.Select(x => new SelectListItem
            {
                Value = x.IndicatorCode,
                Text = $"{x.IndicatorCode} - {x.IndicatorName}"
            });
            var selectListItems = new List<SelectListItem>();
            selectListItems.AddRange(indicatorDdlList);

            return selectListItems;
        }
        private async Task<IEnumerable<POLoanCodeWithProductModel>> GetLoanCodeWiseProductList()
        {            
            var productMappings = await pOProductMappingService.GetPOLoanCodeWiseProductMappings();

            return productMappings;
        }
        #endregion
    }
}
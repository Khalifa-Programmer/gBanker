#region Usings

using gBanker.Core.Filters;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
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
    public class FinancialDataController : BaseController
    {
        #region Private Members

        private readonly IIndicatorService indicatorService;
        public readonly IOrganizationService organizationService;
        private readonly IUltimateReportService ultimateReportService;
        #endregion
         
        #region Ctor
        public FinancialDataController(IIndicatorService indicatorService, IOrganizationService organizationService, IUltimateReportService ultimateReportService)
        {
            this.indicatorService = indicatorService;
            this.organizationService = organizationService;
            this.ultimateReportService = ultimateReportService;
        }
        #endregion

        #region Map
        public async Task<ActionResult> Add()
        {
            var model = new FinancialDataAddViewModel
            {
                IndicatorList = await GetIndicatorList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(FinancialDataAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                bool isError = false;
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        return GetErrorMessageResult(error.ErrorMessage);
                        isError = true;
                        break;
                    }

                    if (isError)
                        break;
                }
            }           

            //if (model.Amount <= 0)
                //return GetErrorMessageResult("Warning! Amount must be greater than 0.");

            var financialData = new FinancialDataModel
            {
                POCode = LoggedInOrganizationCode,
                Ind_code = model.IndicatorCode,
                MNYR = model.MNYR,
                FD_PKSF_FUND = model.Amount,
                CreatedBy = (int)LoggedInEmployeeID,
            };

            //let's add into db for [PRA_MN_RPT_TAB_XL_FD]
            var isAdded = await indicatorService.AddFinancialData(financialData);
            if (!isAdded)
                return GetErrorMessageResult("Error! There was an error while adding.");

            return GetSuccessMessageResult();
        }
        #endregion

        #region Ajax Call

        #endregion


        #region Delete
        public ActionResult DeleteFinancialData(string IndicatorCode, string MNYR)
        {
            try
            {
                var param = new { @IndicatorCode = IndicatorCode, @MNYR = MNYR };
                ultimateReportService.GetDataWithParameter(param, "pksf.PRA_MN_RPT_TAB_XL_FD_DeleteData");
                return GetSuccessMessageResult("Success! Data Deleted.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion

        #region Private Methods

        private async Task<IEnumerable<SelectListItem>> GetIndicatorList()
        {
            var filter = new BaseSearchFilter { AssociatedTable = "PRA_MN_RPT_TAB_XL_FD" };

            var indicators = await indicatorService.GetIndicatorList(filter);

            var indicatorDdlList = indicators.Select(x => new SelectListItem
            {
                Value = x.IndicatorCode,
                Text = $"{x.IndicatorCode} - {x.IndicatorName}"
            });
            var selectListItems = new List<SelectListItem>();
            selectListItems.AddRange(indicatorDdlList);

            return selectListItems;
        }

        #endregion
    }
}
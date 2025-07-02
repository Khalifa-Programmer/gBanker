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
    public class BasicDataController : BaseController
    {
        #region Private Members

        private readonly IIndicatorService indicatorService;
        public readonly IOrganizationService organizationService;
        private readonly IUltimateReportService ultimateReportService;
        #endregion
         
        #region Ctor
        public BasicDataController(IIndicatorService indicatorService, IOrganizationService organizationService, IUltimateReportService ultimateReportService)
        {
            this.indicatorService = indicatorService;
            this.organizationService = organizationService;
            this.ultimateReportService = ultimateReportService;
        }
        #endregion

        #region Add
        public async Task<ActionResult> Add()
        {
            var model = new BasicDataAddViewModel
            {
                IndicatorList = await GetIndicatorList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(BasicDataAddViewModel model)
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

            //if ((model.bd_pksf_fund+model.bd_non_pksf_fund) <= 0)
                //return GetErrorMessageResult("Warning! Amount must be greater than 0.");

            var basicData = new BasicDataModel
            {
                po_code = LoggedInOrganizationCode,
                ind_code = model.IndicatorCode,
                mnyr = model.MNYR,
                m_f_flag=model.M_F_flag,
                bd_pksf_fund = model.bd_pksf_fund,
                bd_non_pksf_fund = model.bd_non_pksf_fund,
                created_by = LoggedInEmployeeID.ToString(),
            };

            //let's add into db for [PRA_MN_RPT_TAB_XL_BD]
            var isAdded = await indicatorService.AddBasicData(basicData);
            if (!isAdded)
                return GetErrorMessageResult("Error! There was an error while adding.");

            return GetSuccessMessageResult();
        }
        #endregion

        #region Ajax Call

        #endregion

        #region Delete
        public ActionResult DeleteBasicData(string IndicatorCode, string MNYR)
        {
            try
            {
                var param = new { @IndicatorCode = IndicatorCode, @MNYR = MNYR };
                ultimateReportService.GetDataWithParameter(param, "pksf.PRA_MN_RPT_TAB_XL_BD_DeleteData");
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
            var filter = new BaseSearchFilter { AssociatedTable = "PRA_MN_RPT_TAB_XL_BD" };

            var indicators = await indicatorService.GetIndicatorList(filter);

            var indicatorDdlList = indicators.Where(f=>f.AssociatedTable== filter.AssociatedTable).ToList().Select(x => new SelectListItem
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
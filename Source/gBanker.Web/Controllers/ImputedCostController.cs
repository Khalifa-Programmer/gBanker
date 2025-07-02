using gBanker.Core.Filters;
using gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads;
using gBanker.Service.OLRS;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using gHRM.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class ImputedCostController : BaseController
    {
        #region Private Members

        private readonly IUltimateReportService ultimateReportService;
        private readonly IOLRSHubService olrSHubService;
        private readonly IOLRSService oLRSService;

        #endregion

        #region Ctor

        public ImputedCostController(IUltimateReportService ultimateReportService,
            IOLRSService oLRSService,
            IOLRSHubService olrSHubService)
        {
            this.ultimateReportService = ultimateReportService;
            this.olrSHubService = olrSHubService;
            this.oLRSService = oLRSService;
        }

        #endregion

        #region Header Information

        [HttpGet]
        public ActionResult HeaderInformation()
        {
            var filter = new BaseSearchFilter();
            return View(filter);
        }

        public ActionResult AddHeaderInfo()
        {
            var model = new ImputedCost_HeaderInfoViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddHeaderInfo(ImputedCost_HeaderInfoViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");
            try
            {
                var param = new
                {
                    @po_code = SessionHelper.LoggedInOrganizationCode,
                    @mnyr = model.MNYR,
                    @market_rate = model.MarketRate,
                    @inf_rate = model.InflationRate,
                    @fy_month = model.FY_Month,
                    @imp_fs_npk = model.imp_fs_npk,
                    @imp_fs_pk = model.imp_fs_pk,
                    @imp_fsi_npk = model.imp_fsi_npk,
                    @imp_fsi_pk = model.imp_fsi_pk,
                    @ins_user = SessionHelper.LoginUserEmployeeID
                };

                //let's add into db
                ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_MN_IMP_COST_MASTER_InsertDataFromUI]");

                return GetSuccessMessageResult();
            }
            catch(Exception ex)
            {
                return GetErrorMessageResult("Error! There was an error while adding.");
            }
        }
        public JsonResult GetHeaderInformation(int jtStartIndex, int jtPageSize, string jtSorting, string filterValue, string mnyr, string fyMonth)
        {
            try
            {
                var param = new { mnyr = mnyr, fy_month = fyMonth, OrgCode = LoggedInOrganizationCode };
                var getData = ultimateReportService.GetDataWithParameter(param, "pksf.GetHeaderInformation");
                var detail = getData.Tables[0].AsEnumerable().Select(p => new ImputedCost_HeaderInfoViewModel
                {
                    MNYR = p.Field<string>("mnyr"),
                    FY_Month = p.Field<Int16>("fy_month").ToString(),
                    MarketRate = p.Field<decimal>("market_rate"),
                    InflationRate = p.Field<decimal>("inf_rate"),
                    imp_fs_npk = p.Field<decimal>("imp_fs_npk"),
                    imp_fs_pk = p.Field<decimal>("imp_fs_pk"),
                    imp_fsi_npk = p.Field<decimal>("imp_fsi_npk"),
                    imp_fsi_pk = p.Field<decimal>("imp_fsi_pk"),

                    SYNCED_STATUS = p.Field<string>("SYNCED_STATUS")
                }).ToList();
                var currentPageRecords = detail.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = detail.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpDelete]
        public ActionResult DeleteHeaderInfoData(string MNYR)
        {
            try
            {
                var param = new { @MNYR = MNYR };
                ultimateReportService.GetDataWithParameter(param, "pksf.PRA_MN_IMP_COST_MASTER_DeleteData");
                return GetSuccessMessageResult("Success! Data Deleted.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<JsonResult> SyncImputedCostHeaderInfoToPKSF()
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                var olrsHubToken = await olrSHubService.GetAccessToken();

                if (!olrsHubToken.isSuccess)
                    return GetErrorMessageResult(olrsHubToken.message);

                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    PostingFlag = "1"
                };

                var listing = await oLRSService.PRA_MN_IMP_COST_MASTER_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Posted data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;

                    var pra_MN_IMP_COST_MASTER_List = new List<PRA_MN_IMP_COST_MASTER_Model>();
                    pra_MN_IMP_COST_MASTER_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_MN_IMP_COST_MASTER(pra_MN_IMP_COST_MASTER_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_MASTER(item.PO_CODE, item.MNYR, updatedUser);
                    else
                        isErrorFound = true;
                }

                if (isErrorFound)
                    return GetErrorMessageResult("Warning! Operation Partially Completed");

                return GetSuccessMessageResult("Success! Add Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while sync to pksf.");
            }
        }

        #endregion

        #region INF Equity Information
        [HttpGet]
        public ActionResult InfEquities()
        {
            var model = new ImputedCost_INFEquityInfoViewModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult INFEquityInformation()
        {
            var model = new ImputedCost_INFEquityInfoViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> INFEquityInformation(ImputedCost_INFEquityInfoViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");

            if (model.NPK_LAST_YR == 0 && model.PK_LAST_YR == 0 && model.NPK_THIS_MONTH == 0
                    && model.PK_THIS_MONTH == 0 && model.NPK_AVG == 0 && model.NPK_AVG == 0 && model.NPK_INF_ADJ == 0
                    && model.PK_INF_ADJ == 0)
            {
                return GetErrorMessageResult("Warning! You must fill atleast one amount field greater than 0");
            }

            try
            {
                var param = new
                {
                    @po_code = SessionHelper.LoggedInOrganizationCode,
                    @mnyr = model.MNYR,

                    @npk_last_yr = model.NPK_LAST_YR,
                    @pk_last_yr = model.PK_LAST_YR,

                    @npk_this_month = model.NPK_THIS_MONTH,
                    @pk_this_month = model.PK_THIS_MONTH,

                    @npk_avg = model.NPK_AVG,
                    @pk_avg = model.PK_AVG,

                    @npk_inf_adj = model.NPK_INF_ADJ,
                    @pk_inf_adj = model.PK_INF_ADJ,

                    @ins_user = SessionHelper.LoginUserEmployeeID
                };

                //let's add into db
                ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_MN_INF_EQUITY_Insertion]");

                return GetSuccessMessageResult();
            }
            catch(Exception ex)
            {
                return GetErrorMessageResult("Error! There was an error while adding.");
            }
        }
        public JsonResult GetINFEquityInformation(int jtStartIndex, int jtPageSize, string jtSorting, string filterValue, string mnyr,string posting_flag)
        {
            try
            {
                var param = new { po_code=LoggedInOrganizationCode, mnyr = mnyr, posting_flag = posting_flag };
                var getData = ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_MN_INF_EQUITY_GetINFEquityInformation]");
                var detail = getData.Tables[0].AsEnumerable().Select(p => new ImputedCost_INFEquityInfoViewModel
                {
                    MNYR = p.Field<string>("mnyr"),
                    NPK_LAST_YR = p.Field<decimal>("npk_last_yr"),
                    PK_LAST_YR = p.Field<decimal>("pk_last_yr"),

                    NPK_THIS_MONTH = p.Field<decimal>("npk_this_month"),
                    PK_THIS_MONTH = p.Field<decimal>("pk_this_month"),

                    NPK_AVG = p.Field<decimal>("npk_avg"),
                    PK_AVG = p.Field<decimal>("pk_avg"),

                    NPK_INF_ADJ = p.Field<decimal>("npk_inf_adj"),
                    PK_INF_ADJ = p.Field<decimal>("pk_inf_adj"),

                    SYNCED_STATUS = p.Field<string>("SYNCED_STATUS")

                }).ToList();
                var currentPageRecords = detail.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = detail.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteInfEquity(string mmYYYY)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");
            try
            {
                var param = new
                {
                    @po_code = SessionHelper.LoggedInOrganizationCode,
                    @mnyr = mmYYYY
                };

                //let's add into db
                ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_MN_INF_EQUITY_Remove]");

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult("Error! There was an error while deleting.");
            }
        }

        [HttpGet]
        public async Task<JsonResult> SyncINFEquityToPKSF()
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                var olrsHubToken = await olrSHubService.GetAccessToken();

                if (!olrsHubToken.isSuccess)
                    return GetErrorMessageResult(olrsHubToken.message);

                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    PostingFlag = "1"
                };

                var listing = await oLRSService.PRA_MN_INF_EQUITY_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Posted data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;

                    var pra_MN_INF_EQUITY_List = new List<PRA_MN_INF_EQUITY_Model>();
                    pra_MN_INF_EQUITY_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_MN_INF_EQUITY(pra_MN_INF_EQUITY_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_INF_EQUITY(item.PO_CODE, item.MNYR, updatedUser);
                    else
                        isErrorFound = true;
                }

                if (isErrorFound)
                    return GetErrorMessageResult("Warning! Operation Partially Completed");

                return GetSuccessMessageResult("Success! Add Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while sync to pksf.");
            }
        }

        #endregion
    }
}
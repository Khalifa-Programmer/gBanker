
#region Usings

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
using System.Web.Mvc;

#endregion

namespace gBanker.Web.Controllers
{
    public class ImputedCostSavingInterestController : BaseController
    {
        #region Private Members

        private readonly IUltimateReportService ultimateReportService;
        private readonly IOLRSHubService olrSHubService;
        private readonly IOLRSService oLRSService;

        #endregion

        #region Ctor

        public ImputedCostSavingInterestController(
            IOLRSService oLRSService,
            IOLRSHubService olrSHubService,
           IUltimateReportService ultimateReportService)
        {
            this.ultimateReportService = ultimateReportService;
            this.olrSHubService = olrSHubService;
            this.oLRSService = oLRSService;
        }


        #endregion

        #region Create

        [HttpGet]
        public ActionResult Create()
        {
            var model = new ImputedCostSavingInterestViewModel { };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Create(ImputedCostSavingInterestViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");
            try
            {
                var param = new
                {
                    @po_code = SessionHelper.LoggedInOrganizationCode,
                    @mnyr = model.MNYR,
                    @int_rate = model.INT_RATE,
                    @npk = model.NPK,
                    @reg = model.Regular,
                    @vol = model.VOL,
                    @other = model.Other,
                    @ins_user = SessionHelper.LoginUserEmployeeID
                };

                //let's add into db
                ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_MN_IMP_COST_SV_INT_InsertDataFromUI]");

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult("Error! There was an error while adding.");
            }
        }
        public JsonResult GetImputedCostSavingInterestList(int jtStartIndex, int jtPageSize, string jtSorting, string filterValue, string mnyr)
        {
            try
            {
                var param = new { @MNYR = mnyr };
                var getData = ultimateReportService.GetDataWithParameter(param, "pksf.PRA_MN_IMP_COST_SV_INT_GET_SAVING_INTEREST");
                var detail = getData.Tables[0].AsEnumerable().Select(p => new ImputedCostSavingInterestViewModel
                {
                    MNYR = p.Field<string>("mnyr"),
                    INT_RATE = p.Field<decimal>("int_rate"),
                    NPK = p.Field<decimal>("npk"),
                    Regular = p.Field<decimal>("reg"),
                    VOL = p.Field<decimal>("vol"),
                    Other = p.Field<decimal>("other"),

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

        [HttpGet]
        public async Task<JsonResult> SyncImputedCostSavingInterestToPKSF()
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                var olrsHubToken = await olrSHubService.GetAccessToken();

                if (!olrsHubToken.isSuccess)
                    return GetErrorMessageResult(olrsHubToken.message);

                var filter = new BaseSearchFilter
                {
                    PoCode=LoggedInOrganizationCode,
                    PostingFlag = "1"
                };

                var listing = await oLRSService.PRA_MN_IMP_COST_SV_INT_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Posted data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;

                    var pra_MN_IMP_COST_SV_INT_List = new List<PRA_MN_IMP_COST_SV_INT_Model>();
                    pra_MN_IMP_COST_SV_INT_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_MN_IMP_COST_SV_INT(pra_MN_IMP_COST_SV_INT_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_SV_INT(item.PO_CODE, item.MNYR, updatedUser);
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

        #region Delete

        [HttpDelete]
        public JsonResult DeleteImputedCostSavingInterest(string mnyr)
        {
            try
            {
                var param = new { @mnyr = mnyr };
                var getData = ultimateReportService.GetDataWithParameter(param, "pksf.DeleteImputedCostSavingInterest");
                return GetSuccessMessageResult("Imputed Cost deleted successfully");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
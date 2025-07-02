
using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using gHRM.Service;
using gBanker.Service.OLRS;
using System.Threading.Tasks;
using gBanker.Core.Filters;
using gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads;
using gBanker.Service;
using gBanker.Data.DBDetailModels.OLRSHubs.CommonModels;

namespace gBanker.Web.Controllers
{
    public class LCdeWizSerCharController : BaseController
    {
        #region Private Members

        private readonly IUltimateReportService ultimateReportService;
        private readonly IOLRSHubService olrSHubService;
        private readonly IOLRSService oLRSService;
        public readonly IPOLoanCodeService poLoanCodeService;

        #endregion

        #region Ctor

        public LCdeWizSerCharController(IUltimateReportService ultimateReportService,
            IOLRSService oLRSService,
            IOLRSHubService olrSHubService,
            IPOLoanCodeService poLoanCodeService
            )
        {
            this.ultimateReportService = ultimateReportService;
            this.olrSHubService = olrSHubService;
            this.oLRSService = oLRSService;
            this.poLoanCodeService = poLoanCodeService;
        }

        #endregion

        #region IMP Loan SC Amount Data
        [HttpGet]
        public ActionResult IMPLoanSCAmountData()
        {
            var model = new PRA_MN_IMP_COST_LN_SC_ReportSearchFilter();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetIMPLoanSCAmountData(int jtStartIndex, int jtPageSize, string jtSorting,
            string filterValue, string mnyr, string syncedStatus, bool isCalculateTotal, int totalCount)
        {
            try
            {
                int pageNumber = jtStartIndex > 0 ? jtStartIndex : 1;
                if (pageNumber > 1) //if not first page                
                    pageNumber = (jtStartIndex / jtPageSize) + 1;

                var filter = new PRA_MN_IMP_COST_LN_SC_ReportSearchFilter
                {
                    OrgCode = LoggedInOrganizationCode,
                    MNYR = mnyr ?? string.Empty,
                    SYNCED_STATUS = syncedStatus ?? string.Empty,
                    IsCalculateTotal = isCalculateTotal,
                    PageNumber = pageNumber,
                    PageSize = jtPageSize
                };

                var filtedListing = await olrSHubService.Get_PRA_MN_IMP_COST_LN_SC_By_Filter(filter);

                if (filtedListing.Any() && isCalculateTotal)
                    totalCount = filtedListing[0].TotalCount;

                return Json(new { Result = "OK", Records = filtedListing, TotalRecordCount = totalCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("lcdewizserchar/loanserviceamountdata/mmyr/{mmyr}/sc/{sc}")]
        public async Task<ActionResult> LoanServiceAmountDataDetails(string mmyr, string sc)
        {
            try
            {
                var filter = new PRA_MN_IMP_COST_LN_SC_ReportSearchFilter
                {
                    OrgCode = LoggedInOrganizationCode,
                    MNYR = mmyr ?? string.Empty,
                    ServiceChargeRate = sc ?? string.Empty
                };

                var model = await olrSHubService.Get_PRA_MN_IMP_COST_LN_SC(filter);

                return View(model);
            }
            catch (Exception ex)
            {
                return View(new PRA_MN_IMP_COST_LN_SC_Model());
            }
        }

        #endregion

        #region Manage IMP Loan SC Amount

        public async Task<ActionResult> LCSerCharge()
        {
            var model = new LoanCodeWithRateViewModel
            {
                LoanCodeList = await GetLoandCodeDropDownList()
            };

            return View(model);    

            //changed
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> LCSerCharge(LoanCodeWithRateViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");

            try
            {
                var imp_COST_LN_SC_INSERT = new IMP_COST_LN_SC_INSERT_Model
                {
                    PO_CODE = LoggedInOrganizationCode,
                    MNYR = model.LoanDate,
                    Sc_Rate = model.ServiceChargeRate,
                    LoanCode = model.LoanCode,
                    LoanServiceAmount = model.ServiceAmount,
                    INS_USER = LoggedInOrganizationID.ToString()
                };

                bool isOperationSuccess = await poLoanCodeService.Manage_IMP_COST_LN_SC(imp_COST_LN_SC_INSERT);
                if (!isOperationSuccess)
                    return GetErrorMessageResult("Error, There was an error while adding. Try another");

                return GetSuccessMessageResult("Success, Changes Made");
            }
            catch (Exception ex)
            {
                string message = "Sorry for inconvenience! please try again later";
                return GetErrorMessageResult(message);
            }
        }

        [HttpGet]
        public async Task<JsonResult> SyncImputedCostLoanServiceChargeToPKSF(string syncMonth)
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
                    MNYR = syncMonth,
                    PostingFlag = "1"
                };

                var listing = await oLRSService.PRA_MN_IMP_COST_LN_SC_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Posted data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;

                    var pra_MN_IMP_COST_LN_SC_List = new List<PRA_MN_IMP_COST_LN_SC_Model>();
                    pra_MN_IMP_COST_LN_SC_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_MN_IMP_COST_LN_SC(pra_MN_IMP_COST_LN_SC_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_LN_SC(item.PO_CODE, syncMonth, (decimal)item.SC_RATE, updatedUser);
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

        #region Ajax Calls        

        [HttpGet]
        public JsonResult GetServiceRateList()
        {
            try
            {
                var _items = new List<SelectListItem>();
                _items.Add(new SelectListItem() { Text = "0.50", Value = "0.50" });
                _items.Add(new SelectListItem() { Text = "1.00", Value = "1.00" });
                _items.Add(new SelectListItem() { Text = "1.25", Value = "1.25" });
                _items.Add(new SelectListItem() { Text = "1.50", Value = "1.50" });
                _items.Add(new SelectListItem() { Text = "2.00", Value = "2.00" });
                _items.Add(new SelectListItem() { Text = "2.50", Value = "2.50" });
                _items.Add(new SelectListItem() { Text = "3.00", Value = "3.00" });
                _items.Add(new SelectListItem() { Text = "3.50", Value = "3.50" });
                _items.Add(new SelectListItem() { Text = "4.00", Value = "4.00" });
                _items.Add(new SelectListItem() { Text = "4.50", Value = "4.50" });
                _items.Add(new SelectListItem() { Text = "5.00", Value = "5.00" });
                _items.Add(new SelectListItem() { Text = "5.50", Value = "5.50" });
                _items.Add(new SelectListItem() { Text = "6.00", Value = "6.00" });
                _items.Add(new SelectListItem() { Text = "6.50", Value = "6.50" });
                _items.Add(new SelectListItem() { Text = "7.00", Value = "7.00" });
                _items.Add(new SelectListItem() { Text = "7.50", Value = "7.50" });
                _items.Add(new SelectListItem() { Text = "8.00", Value = "8.00" });
                _items.Add(new SelectListItem() { Text = "8.50", Value = "8.50" });
                _items.Add(new SelectListItem() { Text = "9.00", Value = "9.00" });
                _items.Add(new SelectListItem() { Text = "9.50", Value = "9.50" });
                _items.Add(new SelectListItem() { Text = "10.00", Value = "10.00" });
                _items.Add(new SelectListItem() { Text = "10.50", Value = "10.50" });
                _items.Add(new SelectListItem() { Text = "11.00", Value = "11.00" });
                _items.Add(new SelectListItem() { Text = "11.50", Value = "11.50" });
                _items.Add(new SelectListItem() { Text = "12.00", Value = "12.00" });
                _items.Add(new SelectListItem() { Text = "12.50", Value = "12.50" });
                _items.Add(new SelectListItem() { Text = "13.00", Value = "13.00" });

                return Json(new { Result = "OK", Options = _items }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Private Methods
        private async Task<IEnumerable<SelectListItem>> GetLoandCodeDropDownList()
        {
            //get loan codes
            var poLoanCodes = await poLoanCodeService.GetPOLoanCodes();

            var selectListItems = poLoanCodes.Select(x => new SelectListItem
            {
                Value = x.LoanCode,
                Text = $"{x.LoanCode} - {x.FunctionalitiesAndFeatures})"
            });

            return selectListItems;
        }


        #endregion

        #region Delete

        [HttpDelete]
        public JsonResult DeleteImputedCostSavingInterest(string mnyr, string SC_RATE)
        {
            try
            {
                var param = new { @mnyr = mnyr, @SC_RATE = SC_RATE };
                var getData = ultimateReportService.GetDataWithParameter(param, "pksf.Delete_ImpLoanServiceChargeAmount");
                return GetSuccessMessageResult("Success! Delete Operation Completed.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
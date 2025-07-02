
#region Usings

using gBanker.Core.Filters;
using gBanker.Core.Utility;
using gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads;
using gBanker.Service.OLRS;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using gHRM.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace gBanker.Web.Controllers
{
    public class OlrsProcessController : BaseController
    {
        #region Private Members

        private readonly IUltimateReportService ultimateReportService;
        private readonly IOLRSHubService olrSHubService;
        private readonly IOLRSService oLRSService;

        #endregion

        #region Ctor

        public OlrsProcessController(IUltimateReportService ultimateReportService,
            IOLRSService oLRSService,
           IOLRSHubService olrSHubService)
        {
            this.ultimateReportService = ultimateReportService;
            this.olrSHubService = olrSHubService;
            this.oLRSService = oLRSService;
        }

        #endregion

        #region Process

        [HttpGet]
        public ActionResult Process()
        {
            var model = new OlrsProcessViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Process(OlrsProcessViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");
            try
            {
                if (model.ProcessType == ProcessTypeConstants.Process_All)
                {
                    //program data
                    var paramPD = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(paramPD, "[pksf].[PRA_MN_RPT_TAB_XL_PD_Main]");


                    //basic data
                    var paramBD = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(paramBD, "[pksf].[PRA_MN_RPT_TAB_XL_BD_InsertBasicData]");

                    //upazilla loan
                    var paramUL = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(paramUL, "[pksf].[PRA_LN_DIST_WISE_DISB_InsertData]");

                    //balance sheet
                    var paramBS = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(paramBS, "[pksf].[PO_MONTHLY_ACC_BAL_BalanceSheet_InsertData]");

                    //trial (IE + RP) balance
                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[Process_TrailBalance_IE_RP]");
                                        
                    //Financial Data
                    var paramFD = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(paramFD, "[pksf].[PRA_MN_RPT_TAB_XL_FD_Main]");

                    //Top Sheet
                    var paramTS = new
                    {
                        @MNYR = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(paramTS, "[pksf].[TOP_SHEET_InsertData]");

                }
                else if (model.ProcessType == ProcessTypeConstants.Program_Data)
                {
                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_MN_RPT_TAB_XL_PD_Main]");
                }
                else if (model.ProcessType == ProcessTypeConstants.Basic_Data)
                {
                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User=$@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_MN_RPT_TAB_XL_BD_InsertBasicData]");
                }
                else if (model.ProcessType == ProcessTypeConstants.Financial_Data)
                {
                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_MN_RPT_TAB_XL_FD_Main]");
                }
                else if (model.ProcessType == ProcessTypeConstants.Upazilla_Loan)
                {
                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_LN_DIST_WISE_DISB_InsertData]");
                }
                else if (model.ProcessType == ProcessTypeConstants.Balance_Sheet)
                {
                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[PO_MONTHLY_ACC_BAL_BalanceSheet_InsertData]");
                }
                
                else if (model.ProcessType == ProcessTypeConstants.Trial_Balance)
                {
                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[Process_TrailBalance_IE_RP]");
                }
               
                else if (model.ProcessType == ProcessTypeConstants.Top_Sheet)
                {
                    var param = new
                    {
                        @MNYR = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[TOP_SHEET_InsertData]");
                }

                return GetSuccessMessageResult("Succcess! Process has been completed.");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult("Error! There was an error while processing.");
            }
        }

        [HttpPost]
        public async Task<JsonResult> ProcessMFIDataToPKSF(OlrsProcessViewModel model)
        {
            ModelState.Remove("SyncMonth");
            ModelState.Remove("NotSyncMonth");

            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");

            try
            {                
                if (model.ProcessType == ProcessTypeConstants.Program_Data)
                {
                    var filter =new BaseSearchFilter { MNYR= model.ProcessMonth};
                    var response= await oLRSService.PRA_MN_RPT_TAB_XL_PD_CHECK_DATA_SYNC_TO_PKSF(filter);
                    if (response.IS_SYNCHED_TO_PKSF) {
                       return GetErrorMessageResult(response.MESSAGE);
                    }

                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_MN_RPT_TAB_XL_PD_Main]");
                }
                else if (model.ProcessType == ProcessTypeConstants.Basic_Data)
                {
                    var filter = new BaseSearchFilter { MNYR = model.ProcessMonth };
                    var response = await oLRSService.PRA_MN_RPT_TAB_XL_BD_CHECK_DATA_SYNC_TO_PKSF(filter);
                    if (response.IS_SYNCHED_TO_PKSF)
                    {
                        return GetErrorMessageResult(response.MESSAGE);
                    }

                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_MN_RPT_TAB_XL_BD_InsertBasicData]");
                }
                else if (model.ProcessType == ProcessTypeConstants.Financial_Data)
                {
                    var filter = new BaseSearchFilter { MNYR = model.ProcessMonth };
                    var response = await oLRSService.PRA_MN_RPT_TAB_XL_FD_CHECK_DATA_SYNC_TO_PKSF(filter);
                    if (response.IS_SYNCHED_TO_PKSF)
                    {
                        return GetErrorMessageResult(response.MESSAGE);
                    }

                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_MN_RPT_TAB_XL_FD_Main]");
                }
                else if (model.ProcessType == ProcessTypeConstants.Upazilla_Loan)
                {
                    var filter = new BaseSearchFilter { MNYR = model.ProcessMonth };
                    var response = await oLRSService.PRA_LN_DIST_WISE_DISB_CHECK_DATA_SYNC_TO_PKSF(filter);
                    if (response.IS_SYNCHED_TO_PKSF)
                    {
                        return GetErrorMessageResult(response.MESSAGE);
                    }

                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_LN_DIST_WISE_DISB_InsertData]");
                }
                else if (model.ProcessType == ProcessTypeConstants.Balance_Sheet)
                {
                    var filter = new BaseSearchFilter { MNYR = model.ProcessMonth };
                    var response = await oLRSService.PO_MONTHLY_ACC_BAL_CHECK_DATA_SYNC_TO_PKSF(filter);
                    if (response.IS_SYNCHED_TO_PKSF)
                    {
                        return GetErrorMessageResult(response.MESSAGE);
                    }

                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[PO_MONTHLY_ACC_BAL_BalanceSheet_InsertData]");
                }

                else if (model.ProcessType == ProcessTypeConstants.Trial_Balance)
                {
                    var filter = new BaseSearchFilter { MNYR = model.ProcessMonth };
                    var response = await oLRSService.PO_MONTHLY_ACC_BAL_CHECK_DATA_SYNC_TO_PKSF(filter);
                    if (response.IS_SYNCHED_TO_PKSF)
                    {
                        return GetErrorMessageResult(response.MESSAGE);
                    }

                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[Process_TrailBalance_IE_RP]");
                }

                else if (model.ProcessType == ProcessTypeConstants.Employment_Data)
                {

                    var filter = new BaseSearchFilter { MNYR = model.ProcessMonth };
                    var response = await oLRSService.PRA_EMPLOYMENT_NEW_CHECK_DATA_SYNC_TO_PKSF(filter);
                    if (response.IS_SYNCHED_TO_PKSF)
                    {
                        return GetErrorMessageResult(response.MESSAGE);
                    }

                    var param = new
                    {
                        @FilteredDate = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[PRA_LN_WISE_Employment_InsertData]");
                }

                else if (model.ProcessType == ProcessTypeConstants.Top_Sheet)
                {
                    var filter = new BaseSearchFilter { MNYR = model.ProcessMonth };
                    var response = await oLRSService.TOP_SHEET_CHECK_DATA_SYNC_TO_PKSF(filter);
                    if (response.IS_SYNCHED_TO_PKSF)
                    {
                        return GetErrorMessageResult(response.MESSAGE);
                    }

                    var param = new
                    {
                        @MNYR = model.ProcessMonth,
                        @OrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        @Ins_User = $@"{SessionHelper.LoginUserEmployeeID}"
                    };
                    ultimateReportService.GetDataWithParameter(param, "[pksf].[TOP_SHEET_InsertData]");
                }

                return GetSuccessMessageResult("Succcess! Process has been completed.");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult("Error! There was an error while processing.");
            }
        }
        #endregion

        #region Sync Process Data to PKSF  

        [HttpPost]
        public async Task<JsonResult> SyncProcessDataToPKSF( string syncMonth, string syncToPKSFType)
        {
            try
            {
                if (syncToPKSFType == SyncToPKSFTypeConstants.Program_Data)
                {
                    return await SyncProgramDataToPKSF(syncMonth);
                }
                else if (syncToPKSFType == SyncToPKSFTypeConstants.Basic_Data)
                {
                    return await SyncBasicDataToPKSF(syncMonth);
                }
                else if (syncToPKSFType == SyncToPKSFTypeConstants.Financial_Data)
                {
                    return await SyncFinancialDataToPKSF(syncMonth);
                }
                else if (syncToPKSFType == SyncToPKSFTypeConstants.Upazilla_Loan)
                {
                    return await SyncUpazillaLoanToPKSF(syncMonth);
                }
                else if (syncToPKSFType == SyncToPKSFTypeConstants.Accounting_BS_IE_RP)
                {
                    return await SyncAccountingDataToPKSF(syncMonth);
                }

                //Imputed Cost
                else if (syncToPKSFType == SyncToPKSFTypeConstants.ImputedCost_Header_Info)
                {
                    return await SyncImputedCostHeaderInfoToPKSF(syncMonth);
                }
                else if (syncToPKSFType == SyncToPKSFTypeConstants.ImputedCost_Loan_Code_Wise_Service_Change)
                {
                    return await SyncImputedCostLoanServiceChargeToPKSF(syncMonth);
                }
                else if (syncToPKSFType == SyncToPKSFTypeConstants.ImputedCost_Savings_Interest_Info)
                {
                    return await SyncImputedCostSavingInterestToPKSF(syncMonth);
                }
                else if (syncToPKSFType == SyncToPKSFTypeConstants.ImputedCost_Inflation_Equity_Info)
                {
                    return await SyncINFEquityToPKSF(syncMonth);
                }

                //Employment
                else if (syncToPKSFType == SyncToPKSFTypeConstants.Employment_Related_Last_Data)
                {
                    return await SyncEmploymentAToPKSF(syncMonth);
                }
                else if (syncToPKSFType == SyncToPKSFTypeConstants.Employment_Related_Last_Retained_Data)
                {
                    return await SyncEmploymentBToPKSF(syncMonth);
                }
                else if (syncToPKSFType == SyncToPKSFTypeConstants.Employment_Related_Current_Year_New_Data)
                {
                    return await SyncEmploymentCToPKSF(syncMonth);
                }

                //TOP Sheet
                else if (syncToPKSFType == SyncToPKSFTypeConstants.Top_Sheet)
                {
                    return await SyncToSheetDataToPKSF(syncMonth);
                }

                return GetErrorMessageResult("Warning, No sync type found.");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult("Error! There was an error while adding.");
            }
        }


        [HttpPost]
        public async Task<JsonResult> NotSyncProcessDataToPKSF(string NotsyncMonth, string NotsyncToPKSFType)
        {
            try
            {
                if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.Program_Data)
                {
                    return await NOT_SyncProgramDataToPKSF(NotsyncMonth);
                }
                else if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.Basic_Data)
                {
                    return await NOT_SyncBasicDataToPKSF(NotsyncMonth);
                }
                else if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.Financial_Data)
                {
                    return await NOT_SyncFinancialDataToPKSF(NotsyncMonth);
                }
                else if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.Upazilla_Loan)
                {
                    return await NOT_SyncUpazillaLoanToPKSF(NotsyncMonth);
                }
                else if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.Accounting_BS_IE_RP)
                {
                    return await NOT_SyncAccountingDataToPKSF(NotsyncMonth);
                }

                //Imputed Cost
                else if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.ImputedCost_Header_Info)
                {
                    return await NOT_SyncImputedCostHeaderInfoToPKSF(NotsyncMonth);
                }
                else if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.ImputedCost_Loan_Code_Wise_Service_Change)
                {
                    return await NOT_SyncImputedCostLoanServiceChargeToPKSF(NotsyncMonth);
                }
                else if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.ImputedCost_Savings_Interest_Info)
                {
                    return await NOT_SyncImputedCostSavingInterestToPKSF(NotsyncMonth);
                }
                else if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.ImputedCost_Inflation_Equity_Info)
                {
                    return await NOT_SyncINFEquityToPKSF(NotsyncMonth);         
                }

                //Employment
                else if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.Employment_Related_Last_Data)
                {
                    return await NOT_SyncEmploymentAToPKSF(NotsyncMonth);
                }
                else if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.Employment_Related_Last_Retained_Data)
                {
                    return await NOT_SyncEmploymentBToPKSF(NotsyncMonth);
                }
                else if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.Employment_Related_Current_Year_New_Data)
                {
                    return await NOT_SyncEmploymentCToPKSF(NotsyncMonth);
                }

                //TOP Sheet
                else if (NotsyncToPKSFType == NotSyncToPKSFTypeConstants.Top_Sheet)
                {
                    return await NOT_SyncToSheetDataToPKSF(NotsyncMonth);
                }

                return GetErrorMessageResult("Warning, No sync type found.");
            }
            catch (Exception ex)
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

        #region Sync to PKSF 

        private async Task<JsonResult> SyncProgramDataToPKSF(string syncMonth)
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
                    MNYR=syncMonth,
                    PostingFlag = "1"
                };

                var listing = await oLRSService.PRA_MN_RPT_TAB_XL_PD_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                if (listing.Count != OLRSSyncDataCountConstants.PROGRAM_DATA)
                {
                    return GetErrorMessageResult($@"Warning! Required total Processed data {OLRSSyncDataCountConstants.PROGRAM_DATA}. (Found {listing.Count} records)");
                }

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

                    var pra_MN_RPT_TAB_XL_PD_List = new List<PRA_MN_RPT_TAB_XL_PD_Model>();
                    pra_MN_RPT_TAB_XL_PD_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_MN_RPT_TAB_XL_PD(pra_MN_RPT_TAB_XL_PD_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_PD(item.PO_CODE, item.MNYR, item.IND_CODE, item.M_F_FLAG, updatedUser);
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
        private async Task<JsonResult> SyncBasicDataToPKSF(string syncMonth)
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

                var listing = await oLRSService.PRA_MN_RPT_TAB_XL_BD_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                if (listing.Count != OLRSSyncDataCountConstants.BASIC_DATA)
                {
                    return GetErrorMessageResult($@"Warning! Required total Processed data {OLRSSyncDataCountConstants.FINANCIAL_DATA}. (Found {listing.Count} records)");
                }

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

                    var pra_MN_RPT_TAB_XL_BD_List = new List<PRA_MN_RPT_TAB_XL_BD_Model>();
                    pra_MN_RPT_TAB_XL_BD_List.Add(item);
                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_MN_RPT_TAB_XL_BD(pra_MN_RPT_TAB_XL_BD_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_BD(item.PO_CODE, item.MNYR, item.IND_CODE, item.M_F_FLAG, updatedUser);
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
        private async Task<JsonResult> SyncFinancialDataToPKSF(string syncMonth)
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

                var listing = await oLRSService.PRA_MN_RPT_TAB_XL_FD_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                if (listing.Count != OLRSSyncDataCountConstants.FINANCIAL_DATA)
                {
                    return GetErrorMessageResult($@"Warning! Required total Processed data {OLRSSyncDataCountConstants.FINANCIAL_DATA}. (Found {listing.Count} records)");
                }

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

                    var pra_MN_RPT_TAB_XL_FD_List = new List<PRA_MN_RPT_TAB_XL_FD_Model>();
                    pra_MN_RPT_TAB_XL_FD_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_MN_RPT_TAB_XL_FD(pra_MN_RPT_TAB_XL_FD_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_FD(item.PO_CODE, item.MNYR, item.IND_CODE, item.M_F_FLAG, updatedUser);
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
        private async Task<JsonResult> SyncUpazillaLoanToPKSF(string syncMonth)
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

                var listing = await oLRSService.PRA_LN_DIST_WISE_DISB_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

                    var pra_LN_DIST_WISE_DISB_List = new List<PRA_LN_DIST_WISE_DISB_Model>();
                    pra_LN_DIST_WISE_DISB_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_LN_DIST_WISE_DISB(pra_LN_DIST_WISE_DISB_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_LN_DIST_WISE_DISB(item.PO_CODE, item.MNYR, item.DIST_CODE, item.THANA_CODE, item.LOAN_CODE, updatedUser);
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
        private async Task<JsonResult> SyncAccountingDataToPKSF(string syncMonth)
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

                var listing = await oLRSService.PO_MONTHLY_ACC_BAL_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
                    
                    var po_MONTHLY_ACC_BAL_List = new List<PO_MONTHLY_ACC_BAL_Model>();
                    po_MONTHLY_ACC_BAL_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PO_MONTHLY_ACC_BAL(po_MONTHLY_ACC_BAL_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PO_MONTHLY_ACC_BAL(item.PO_CODE, item.MNYR,
                                            item.COMPANY_CODE,
                                            item.COMPANY_BRANCH_CODE,
                                            item.FINANCE_CODE,
                                            item.PROJECT_CODE,
                                            item.COMPONENT_CODE,
                                            item.ACCGROUP,
                                            item.COA_ID,
                                            item.L1_CODE,
                                            item.L2_CODE,
                                            item.L3_CODE,
                                            item.L4_CODE,
                                            item.L5_CODE,
                                            updatedUser);
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
        private async Task<JsonResult> SyncImputedCostHeaderInfoToPKSF(string syncMonth)
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

                var listing = await oLRSService.PRA_MN_IMP_COST_MASTER_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

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
            catch(Exception ex)
            {
                return GetErrorMessageResult("Error! There was an error while sync to pksf.");
            }
        }
        private async Task<JsonResult> SyncINFEquityToPKSF(string syncMonth)
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

                var listing = await oLRSService.PRA_MN_INF_EQUITY_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

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
        private async Task<JsonResult> SyncImputedCostSavingInterestToPKSF(string syncMonth)
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

                var listing = await oLRSService.PRA_MN_IMP_COST_SV_INT_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

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
        private async Task<JsonResult> SyncImputedCostLoanServiceChargeToPKSF(string syncMonth)
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
                    return GetErrorMessageResult("Warning! Processed data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

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
        private async Task<JsonResult> SyncEmploymentAToPKSF(string syncMonth)
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

                var listing = await oLRSService.PRA_EMPLOYMENT_OLD_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

                    var pra_EMPLOYMENT_OLD_List = new List<PRA_EMPLOYMENT_OLD_Model>();
                    pra_EMPLOYMENT_OLD_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_EMPLOYMENT_OLD(pra_EMPLOYMENT_OLD_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_OLD(item.PO_CODE, item.MNYR, item.LOAN_CODE, updatedUser);
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
        private async Task<JsonResult> SyncEmploymentBToPKSF(string syncMonth)
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

                var listing = await oLRSService.PRA_EMPLOYMENT_CLS_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

                    var pra_EMPLOYMENT_CLS_List = new List<PRA_EMPLOYMENT_CLS_Model>();
                    pra_EMPLOYMENT_CLS_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_EMPLOYMENT_CLS(pra_EMPLOYMENT_CLS_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_CLS(item.PO_CODE, item.MNYR, item.LOAN_CODE, updatedUser);
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
        private async Task<JsonResult> SyncEmploymentCToPKSF( string syncMonth)
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

                var listing = await oLRSService.PRA_EMPLOYMENT_NEW_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

                    var pra_EMPLOYMENT_NEW_List = new List<PRA_EMPLOYMENT_NEW_Model>();
                    pra_EMPLOYMENT_NEW_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_EMPLOYMENT_NEW(pra_EMPLOYMENT_NEW_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_NEW(item.PO_CODE, item.MNYR, item.LOAN_CODE, updatedUser);
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
        private async Task<JsonResult> SyncToSheetDataToPKSF( string syncMonth)
        {
            try
            {
                var updatedUser = $@"{LoggedInEmployeeID}";
                var olrsHubToken = await olrSHubService.GetAccessToken();

                if (!olrsHubToken.isSuccess)
                    return GetErrorMessageResult(olrsHubToken.message);

                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "1"
                };

                var listing = await oLRSService.TOP_SHEET_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

                    var top_SHEET_List = new List<TOP_SHEET_Model>();
                    top_SHEET_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_TOP_SHEET(top_SHEET_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_TOP_SHEET(item.PO_CODE, item.MNYR, updatedUser);
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

        #region NOT Sync 

        private async Task<JsonResult> NOT_SyncProgramDataToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                  
                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.PRA_MN_RPT_TAB_XL_PD_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                 
                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
                     
                    await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_PD_NOT_Sync(item.PO_CODE, item.MNYR, item.IND_CODE, item.M_F_FLAG, updatedUser);
                } 
                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while sync to pksf.");
            }
        
        }
        private async Task<JsonResult> NOT_SyncBasicDataToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";

                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.PRA_MN_RPT_TAB_XL_BD_LIST_BY_FILTER(filter);

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
                     
                   await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_BD_Not_Sync(item.PO_CODE, item.MNYR, item.IND_CODE, item.M_F_FLAG, updatedUser);
                }

                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error.");
            }
        }
        private async Task<JsonResult> NOT_SyncFinancialDataToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.PRA_MN_RPT_TAB_XL_FD_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");
                 
                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
 
                    await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_FD_NOT_Sync(item.PO_CODE, item.MNYR, item.IND_CODE, item.M_F_FLAG, updatedUser);
                } 
                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while sync to pksf.");
            }
        }
        private async Task<JsonResult> NOT_SyncUpazillaLoanToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                 
                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.PRA_LN_DIST_WISE_DISB_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");
                 
                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;

                    await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_LN_DIST_WISE_DISB_NOT_Sync(item.PO_CODE, item.MNYR, item.DIST_CODE, item.THANA_CODE, item.LOAN_CODE, updatedUser);
                }
                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while sync to pksf.");
            }
        }
        private async Task<JsonResult> NOT_SyncAccountingDataToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                 
                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.PO_MONTHLY_ACC_BAL_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");
                 
                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
                     
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PO_MONTHLY_ACC_BAL_NOT_Sync(item.PO_CODE, item.MNYR,
                                            item.COMPANY_CODE,
                                            item.COMPANY_BRANCH_CODE,
                                            item.FINANCE_CODE,
                                            item.PROJECT_CODE,
                                            item.COMPONENT_CODE,
                                            item.ACCGROUP,
                                            item.COA_ID,
                                            item.L1_CODE,
                                            item.L2_CODE,
                                            item.L3_CODE,
                                            item.L4_CODE,
                                            item.L5_CODE,
                                            updatedUser);
                    
                } 
                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while NOT sync .");
            }
        }
        private async Task<JsonResult> NOT_SyncImputedCostHeaderInfoToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                 
                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.PRA_MN_IMP_COST_MASTER_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");
 
                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
                     
                    await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_MASTER_NOT_Sync(item.PO_CODE, item.MNYR, updatedUser);
                     
                }
                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult("Error! There was an error while NOT sync.");
            }
        }
        private async Task<JsonResult> NOT_SyncINFEquityToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                 
                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.PRA_MN_INF_EQUITY_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");
                 
                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
                     
                  await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_INF_EQUITY_NOT_Sync(item.PO_CODE, item.MNYR, updatedUser);
                }
                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while NOT sync.");
            }
        }
        private async Task<JsonResult> NOT_SyncImputedCostSavingInterestToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                 
                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.PRA_MN_IMP_COST_SV_INT_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");
                 
                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
                     
                    await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_SV_INT_NOT_Sync(item.PO_CODE, item.MNYR, updatedUser);
                }
 
                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while NOT sync.");
            }
        }
        private async Task<JsonResult> NOT_SyncImputedCostLoanServiceChargeToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                 
                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.PRA_MN_IMP_COST_LN_SC_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");
 
                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
                      
                    await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_LN_SC_NOT_Sync(item.PO_CODE, syncMonth,(decimal)item.SC_RATE, updatedUser);
                }
                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while NOT sync.");
            }
        }
        private async Task<JsonResult> NOT_SyncEmploymentAToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.PRA_EMPLOYMENT_OLD_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");
                 
                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
                     
                    await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_OLD_NOT_Sync(item.PO_CODE, item.MNYR, item.LOAN_CODE, updatedUser);
                }
                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while NOT sync.");
            }
        }
        private async Task<JsonResult> NOT_SyncEmploymentBToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                 
                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.PRA_EMPLOYMENT_CLS_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");
                
                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
                     
                    await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_CLS_NOT_Sync(item.PO_CODE, item.MNYR, item.LOAN_CODE, updatedUser);
                }
                 
                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while NOT sync.");
            }
        }
        private async Task<JsonResult> NOT_SyncEmploymentCToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                 
                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.PRA_EMPLOYMENT_NEW_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                 
                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
                     
                    await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_NEW_NOT_Sync(item.PO_CODE, item.MNYR, item.LOAN_CODE, updatedUser);
                }
                 
                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while NOT sync.");
            }
        }
        private async Task<JsonResult> NOT_SyncToSheetDataToPKSF(string syncMonth)
        {
            try
            {
                var updatedUser = $@"{LoggedInEmployeeID}";
                 
                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    MNYR = syncMonth,
                    PostingFlag = "2"
                };

                var listing = await oLRSService.TOP_SHEET_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Processed data not found");

                 
                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;
                    item.UPD_DATE = null;
                     
                    await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_TOP_SHEET_NOT_Sync(item.PO_CODE, item.MNYR, updatedUser);
                }
                return GetSuccessMessageResult("Success! Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while NOT sync.");
            }
        }

        #endregion
    }
}
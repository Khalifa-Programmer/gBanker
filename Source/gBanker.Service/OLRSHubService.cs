using gBanker.Core.Filters;
using gBanker.Core.Utility;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels.OLRSHubs;
using gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads;
using gBanker.Data.DBDetailModels.OLRSHubs.Reports;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public class OLRSHubService : IOLRSHubService
    {
        #region Ctor
        public OLRSHubService()
        {

        }
        #endregion

        #region Public Methods

        #region Authentication
        public async Task<OLRSHubTokenResponse> GetAccessToken()
        {
            try
            {
                WebClient webClient = new WebClient();

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/authorize/token");
                webClient.Headers["Content-Type"] = "application/json";

                var model = new
                {
                    ClientId = OLRSAuthConstants.ClientId,
                    ClientSecret = OLRSAuthConstants.ClientSecret
                };

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(model);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var tokenResponse = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubTokenResponse>(responsebody);

                return tokenResponse;
            }
            catch (Exception ex)
            {
                return new OLRSHubTokenResponse { isSuccess = false, message = $"Error! Error on token generation." };
            }
        }
        #endregion

        #region Imputed Cost

        public async Task<PRA_MN_IMP_COST_LN_SC_Model> Get_PRA_MN_IMP_COST_LN_SC(PRA_MN_IMP_COST_LN_SC_ReportSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {                    
                    var sqlCommand = $@"[pksf].[PRA_MN_IMP_COST_LN_SC_GetDetails] '{filter.OrgCode}', '{filter.MNYR.Replace("_", "/")}',{filter.ServiceChargeRate}";
                    var imp_COST_LN_SCData = await db.Database.SqlQuery<PRA_MN_IMP_COST_LN_SC_Model>(sqlCommand).FirstOrDefaultAsync();
                    return imp_COST_LN_SCData;
                }
            }
            catch (Exception ex)
            {
                return new PRA_MN_IMP_COST_LN_SC_Model();
            }
        }
        public async Task<List<PRA_MN_IMP_COST_LN_SC_Model>> Get_PRA_MN_IMP_COST_LN_SC_By_Filter(PRA_MN_IMP_COST_LN_SC_ReportSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_IMP_COST_LN_SC_GetImpLoanServiceChargeAmount] '{filter.OrgCode}', '{filter.MNYR}','{filter.SYNCED_STATUS}','{filter.IsCalculateTotal}',{filter.PageNumber},{filter.PageSize}";
                    var filtedListing = await db.Database.SqlQuery<PRA_MN_IMP_COST_LN_SC_Model>(sqlCommand).ToListAsync();
                    return filtedListing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_MN_IMP_COST_LN_SC_Model>();
            }
        }

        public async Task<OLRSHubResponse> Add_PRA_MN_INF_EQUITY(List<PRA_MN_INF_EQUITY_Model> pra_MN_INF_EQUITY_List, string accessToken)
        {
            try
            { 
                WebClient webClient = new WebClient();

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/ImputedCost/add_pra_mn_inf_equity");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pra_MN_INF_EQUITY_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }

        public async Task<OLRSHubResponse> Add_PRA_MN_IMP_COST_MASTER(List<PRA_MN_IMP_COST_MASTER_Model> pra_MN_IMP_COST_MASTER_List, string accessToken)
        {
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/ImputedCost/add_pra_mn_imp_cost_master");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pra_MN_IMP_COST_MASTER_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }

        public async Task<OLRSHubResponse> Add_PRA_MN_IMP_COST_LN_SC(List<PRA_MN_IMP_COST_LN_SC_Model> pra_MN_IMP_COST_LN_SC_List, string accessToken)
        {
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/ImputedCost/add_pra_mn_imp_cost_ln_sc");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pra_MN_IMP_COST_LN_SC_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }

        public async Task<OLRSHubResponse> Add_PRA_MN_IMP_COST_SV_INT(List<PRA_MN_IMP_COST_SV_INT_Model> pra_MN_IMP_COST_SV_INT_List, string accessToken)
        {
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/ImputedCost/add_pra_mn_imp_cost_sv_int");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pra_MN_IMP_COST_SV_INT_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }
        #endregion

        #region Employment

        public async Task<List<PRA_EMPLOYMENT_Model>> Get_PRA_EMPLOYMENT_By_Filter(PRA_EMPLOYMENT_ReportSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    //Get employment stored procedure name
                    string storedProcedureName = GetEmploymentStoredProcedureName(filter);

                    var sqlCommand = $@"{storedProcedureName} '{filter.OrgCode}', '{filter.MNYR}','{filter.SYNCED_STATUS}','{filter.IsCalculateTotal}',{filter.PageNumber},{filter.PageSize}";
                    var filtedListing = await db.Database.SqlQuery<PRA_EMPLOYMENT_Model>(sqlCommand).ToListAsync();
                    return filtedListing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_EMPLOYMENT_Model>();
            }
        }       

        public async Task<OLRSHubResponse> Add_PRA_EMPLOYMENT_OLD(List<PRA_EMPLOYMENT_OLD_Model> pra_EMPLOYMENT_OLD_List, string accessToken)
        {
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/Employment/add_pra_employment_old");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pra_EMPLOYMENT_OLD_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }

        public async Task<OLRSHubResponse> Add_PRA_EMPLOYMENT_NEW(List<PRA_EMPLOYMENT_NEW_Model> pra_EMPLOYMENT_NEW_List, string accessToken)
        {
            try
            {
                WebClient webClient = new WebClient();  

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/Employment/add_pra_employment_new");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pra_EMPLOYMENT_NEW_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }

        public async Task<OLRSHubResponse> Add_PRA_EMPLOYMENT_CLS(List<PRA_EMPLOYMENT_CLS_Model> pra_MN_EMPLOYMENT_CLS_List, string accessToken)
        {
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/Employment/add_pra_employment_cls");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pra_MN_EMPLOYMENT_CLS_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }


        #endregion

        #region Program Data 
        public async Task<PRA_MN_RPT_TAB_XL_PD_Model> Get_PRA_MN_RPT_TAB_XL_PD(PRA_MN_RPT_TAB_XL_PD_ReportSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_PD_GetDetails] '{filter.OrgCode}', '{filter.MNYR.Replace("_","/")}','{filter.IND_CODE}','{filter.M_F_flag}'";
                    var programData = await db.Database.SqlQuery<PRA_MN_RPT_TAB_XL_PD_Model>(sqlCommand).FirstOrDefaultAsync();
                    return programData;
                }
            }
            catch (Exception ex)
            {
                return new PRA_MN_RPT_TAB_XL_PD_Model();
            }
        }
        public async Task<List<PRA_MN_RPT_TAB_XL_PD_Model>> Get_PRA_MN_RPT_TAB_XL_PD_By_Filter(PRA_MN_RPT_TAB_XL_PD_ReportSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_PD_GetProgramDataList] '{filter.OrgCode}', '{filter.MNYR}','{filter.IND_CODE}','{filter.M_F_flag}','{filter.SYNCED_STATUS}','{filter.IsCalculateTotal}',{filter.PageNumber},{filter.PageSize}";
                    var filtedListing = await db.Database.SqlQuery<PRA_MN_RPT_TAB_XL_PD_Model>(sqlCommand).ToListAsync();
                    return filtedListing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_MN_RPT_TAB_XL_PD_Model>();
            }
        }

        public async Task<OLRSHubResponse> Add_PRA_MN_RPT_TAB_XL_PD(List<PRA_MN_RPT_TAB_XL_PD_Model> pra_MN_RPT_TAB_XL_PD_List, string accessToken)
        {
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/ProgramData/add_pra_mn_rpt_tab_xl_pd");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pra_MN_RPT_TAB_XL_PD_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }

        #endregion

        #region Financial Data

        public async Task<List<PRA_MN_RPT_TAB_XL_FD_Report_Model>> Get_PRA_MN_RPT_TAB_XL_FD_By_Filter(PRA_MN_RPT_TAB_XL_FD_ReportSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_FD_GetFinancialDataList] '{filter.OrgCode}', '{filter.MNYR}','{filter.IND_CODE}','{filter.M_F_flag}','{filter.SYNCED_STATUS}','{filter.IsCalculateTotal}',{filter.PageNumber},{filter.PageSize}";
                    var filtedListing = await db.Database.SqlQuery<PRA_MN_RPT_TAB_XL_FD_Report_Model>(sqlCommand).ToListAsync();
                    return filtedListing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_MN_RPT_TAB_XL_FD_Report_Model>();
            }
        }

        public async Task<OLRSHubResponse> Add_PRA_MN_RPT_TAB_XL_FD(List<PRA_MN_RPT_TAB_XL_FD_Model> pra_MN_RPT_TAB_XL_FD_List, string accessToken)
        {
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/FinancialData/add_pra_mn_rpt_tab_xl_fd");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pra_MN_RPT_TAB_XL_FD_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }

        #endregion

        #region Basic Data

        public async Task<List<PRA_MN_RPT_TAB_XL_BD_Model>> Get_PRA_MN_RPT_TAB_XL_FD_By_Filter(PRA_MN_RPT_TAB_XL_BD_ReportSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_BD_GetBasicDataList] '{filter.OrgCode}', '{filter.MNYR}','{filter.IND_CODE}','{filter.M_F_flag}','{filter.SYNCED_STATUS}','{filter.IsCalculateTotal}',{filter.PageNumber},{filter.PageSize}";
                    var filtedListing = await db.Database.SqlQuery<PRA_MN_RPT_TAB_XL_BD_Model>(sqlCommand).ToListAsync();
                    return filtedListing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_MN_RPT_TAB_XL_BD_Model>();
            }
        }

        public async Task<OLRSHubResponse> Add_PRA_MN_RPT_TAB_XL_BD(List<PRA_MN_RPT_TAB_XL_BD_Model> pra_MN_RPT_TAB_XL_BD_List, string accessToken)
        {
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/BasicData/add_pra_mn_rpt_tab_xl_bd");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pra_MN_RPT_TAB_XL_BD_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }

        #endregion

        #region Upazilla Loan

        public async Task<List<PRA_LN_DIST_WISE_DISB_Model>> Get_PRA_LN_DIST_WISE_DISB_By_Filter(PRA_LN_DIST_WISE_DISB_ReportSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    //[pksf].[PRA_LN_DIST_WISE_DISB_GetUpazillaLoanDataList_Sandbox]
                    //var sqlCommand = $@"[pksf].[PRA_LN_DIST_WISE_DISB_GetUpazillaLoanDataList_Sandbox] '{filter.OrgCode}', '{filter.MNYR}','{filter.SYNCED_STATUS}','{filter.IsCalculateTotal}',{filter.PageNumber},{filter.PageSize}";
                    var sqlCommand = $@"[pksf].[PRA_LN_DIST_WISE_DISB_GetUpazillaLoanDataList] '{filter.OrgCode}', '{filter.MNYR}','{filter.MFI_District_Code}','{filter.MFI_Thana_Code}', '{filter.SYNCED_STATUS}','{filter.IsCalculateTotal}',{filter.PageNumber},{filter.PageSize}";

                    var filtedListing = await db.Database.SqlQuery<PRA_LN_DIST_WISE_DISB_Model>(sqlCommand).ToListAsync();
                    return filtedListing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_LN_DIST_WISE_DISB_Model>();
            }
        }
        public async Task<OLRSHubResponse> Add_PRA_LN_DIST_WISE_DISB(List<PRA_LN_DIST_WISE_DISB_Model> pra_LN_DIST_WISE_DISB_List, string accessToken)
        {
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;                
               
                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/UpazillaLoan/add_pra_ln_dist_wise_disb");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pra_LN_DIST_WISE_DISB_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }

        public async Task<OLRSHubResponse> Update_PRA_LN_DIST_WISE_DISB(PRA_LN_DIST_WISE_DISB_Model parameter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_LN_DIST_WISE_DISB_UpdateUpazillaLoanData] '{parameter.PO_CODE}', '{parameter.MNYR}','{parameter.CUM_DISB}','{parameter.MEMBER}', '{parameter.BORROWER}','{parameter.LOAN_FY}',{parameter.CUM_BORR},{parameter.CUM_DISB_HDN}, {parameter.MEMBER_HDN}, {parameter.BORROWER_HDN}, {parameter.LOAN_FY_HDN}, {parameter.CUM_BORR_HDN}";
                    var filtedListing = await db.Database.SqlQuery<PRA_LN_DIST_WISE_DISB_Model>(sqlCommand).ToListAsync();
                    return new OLRSHubResponse { isSuccess = true, message = "Data Update Successfully" };
                }

            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }


        #endregion

        #region Accounting


        public async Task<List<PO_MONTHLY_ACC_BAL_Model>> Get_PO_MONTHLY_ACC_BAL_By_Filter(PO_MONTHLY_ACC_BAL_ReportSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PO_MONTHLY_ACC_BAL_GetAccountingData] '{filter.OrgCode}', '{filter.MNYR}','{filter.ACCGROUP}','{filter.SearchTerm.Trim()}', '{filter.SYNCED_STATUS}','{filter.IsCalculateTotal}',{filter.PageNumber},{filter.PageSize}";
                    var filtedListing = await db.Database.SqlQuery<PO_MONTHLY_ACC_BAL_Model>(sqlCommand).ToListAsync();
                    return filtedListing;
                }
            }
            catch (Exception ex)
            {
                return new List<PO_MONTHLY_ACC_BAL_Model>();
            }
        }

        public async Task<PO_MONTHLY_ACC_BAL_SummaryModel> Get_PO_MONTHLY_ACC_BAL_Summary_By_Filter(PO_MONTHLY_ACC_BAL_ReportSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PO_MONTHLY_ACC_BAL_GetSummary] '{filter.MNYR}','{filter.ACCGROUP}','{filter.SearchTerm.Trim()}', '{filter.SYNCED_STATUS}'";
                    var filtedListing = await db.Database.SqlQuery<PO_MONTHLY_ACC_BAL_SummaryModel>(sqlCommand).FirstOrDefaultAsync();
                    return filtedListing;
                }
            }
            catch (Exception ex)
            {
                return new PO_MONTHLY_ACC_BAL_SummaryModel();
            }
        }

        public async Task<OLRSHubResponse> Add_PO_MONTHLY_ACC_BAL(List<PO_MONTHLY_ACC_BAL_Model> pra_LN_DIST_WISE_DISB_List, string accessToken)
        {
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/Accounting/add_po_monthly_acc_bal");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(pra_LN_DIST_WISE_DISB_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }

        #endregion

        #region Topsheet Data 
       
        public async Task<List<TOP_SHEET_Model>> Get_TOP_SHEET_By_Filter(TOP_SHEET_ReportSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[TOP_SHEET_GetTopSheetData] '{filter.OrgCode}', '{filter.MNYR}','{filter.SYNCED_STATUS}','{filter.IsCalculateTotal}',{filter.PageNumber},{filter.PageSize}";
                    var filtedListing = await db.Database.SqlQuery<TOP_SHEET_Model>(sqlCommand).ToListAsync();
                    return filtedListing;
                }
            }
            catch (Exception ex)
            {
                return new List<TOP_SHEET_Model>();
            }
        }

        public async Task<OLRSHubResponse> Add_TOP_SHEET(List<TOP_SHEET_Model> top_SHEET_List, string accessToken)
        {
            try
            {
                WebClient webClient = new WebClient();

                webClient.Headers["Content-Type"] = "application/json";
                webClient.Headers["Authorization"] = $"Bearer {accessToken}";
                webClient.Encoding = Encoding.UTF8;

                var uri = new Uri($"{OLRSAuthConstants.ApiBaseUrl}/api/topsheet/add_top_sheet");

                var requestData = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(top_SHEET_List);

                var responsebody = await webClient.UploadStringTaskAsync(uri, "POST", requestData);

                var response = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<OLRSHubResponse>(responsebody);

                return response;
            }
            catch (Exception ex)
            {
                return new OLRSHubResponse { isSuccess = false, message = ex.Message };
            }
        }

        #endregion

        #endregion

        #region Private Methods

        private string GetEmploymentStoredProcedureName(PRA_EMPLOYMENT_ReportSearchFilter filter)
        {
            string storedProcedureName = "";
            switch (filter.EmploymentType)
            {
                case "Employment up to Last Half Year":
                    storedProcedureName = "[pksf].[PRA_EMPLOYMENT_OLDData]";
                    break;

                case "Employment Retained From Last Half Year":
                    storedProcedureName = "[pksf].[PRA_EMPLOYMENT_CLSData]";
                    break;

                case "New Emloyee In Current Half Year":
                    storedProcedureName = "[pksf].[PRA_EMPLOYMENT_NEWData]";
                    break;
            }

            return storedProcedureName;
        }

        #endregion
    }
}

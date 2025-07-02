using gBanker.Core.Filters;
using gBanker.Data.DBDetailModels.OLRSHubs;
using gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads;
using gBanker.Data.DBDetailModels.OLRSHubs.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Service
{

    public interface IOLRSHubService
    {
        #region Authentication

        Task<OLRSHubTokenResponse> GetAccessToken();

        #endregion

        #region Imputed Cost

        Task<PRA_MN_IMP_COST_LN_SC_Model> Get_PRA_MN_IMP_COST_LN_SC(PRA_MN_IMP_COST_LN_SC_ReportSearchFilter filter);
        Task<List<PRA_MN_IMP_COST_LN_SC_Model>> Get_PRA_MN_IMP_COST_LN_SC_By_Filter(PRA_MN_IMP_COST_LN_SC_ReportSearchFilter filter);


        Task<OLRSHubResponse> Add_PRA_MN_INF_EQUITY(List<PRA_MN_INF_EQUITY_Model> pra_MN_INF_EQUITY_List, string accessToken);

        Task<OLRSHubResponse> Add_PRA_MN_IMP_COST_MASTER(List<PRA_MN_IMP_COST_MASTER_Model> pra_MN_IMP_COST_MASTER_List, string accessToken);

        Task<OLRSHubResponse> Add_PRA_MN_IMP_COST_LN_SC(List<PRA_MN_IMP_COST_LN_SC_Model> pra_MN_IMP_COST_LN_SC_List, string accessToken);

        Task<OLRSHubResponse> Add_PRA_MN_IMP_COST_SV_INT(List<PRA_MN_IMP_COST_SV_INT_Model> pra_MN_IMP_COST_SV_INT_List, string accessToken);

        #endregion

        #region Employment
        Task<List<PRA_EMPLOYMENT_Model>> Get_PRA_EMPLOYMENT_By_Filter(PRA_EMPLOYMENT_ReportSearchFilter filter);
        Task<OLRSHubResponse> Add_PRA_EMPLOYMENT_OLD(List<PRA_EMPLOYMENT_OLD_Model> pra_EMPLOYMENT_OLD_List, string accessToken);
        Task<OLRSHubResponse> Add_PRA_EMPLOYMENT_NEW(List<PRA_EMPLOYMENT_NEW_Model> pra_EMPLOYMENT_NEW_List, string accessToken);
        Task<OLRSHubResponse> Add_PRA_EMPLOYMENT_CLS(List<PRA_EMPLOYMENT_CLS_Model> pra_MN_EMPLOYMENT_CLS_List, string accessToken);

        #endregion

        #region Program Data
        Task<PRA_MN_RPT_TAB_XL_PD_Model> Get_PRA_MN_RPT_TAB_XL_PD(PRA_MN_RPT_TAB_XL_PD_ReportSearchFilter filter);
        Task<List<PRA_MN_RPT_TAB_XL_PD_Model>> Get_PRA_MN_RPT_TAB_XL_PD_By_Filter(PRA_MN_RPT_TAB_XL_PD_ReportSearchFilter filter);
        Task<OLRSHubResponse> Add_PRA_MN_RPT_TAB_XL_PD(List<PRA_MN_RPT_TAB_XL_PD_Model> pra_MN_RPT_TAB_XL_PD_List, string accessToken);

        #endregion

        #region Financial Data
        Task<List<PRA_MN_RPT_TAB_XL_FD_Report_Model>> Get_PRA_MN_RPT_TAB_XL_FD_By_Filter(PRA_MN_RPT_TAB_XL_FD_ReportSearchFilter filter);
        Task<OLRSHubResponse> Add_PRA_MN_RPT_TAB_XL_FD(List<PRA_MN_RPT_TAB_XL_FD_Model> pra_MN_RPT_TAB_XL_FD_List, string accessToken);

        #endregion

        #region Basic Data

        Task<List<PRA_MN_RPT_TAB_XL_BD_Model>> Get_PRA_MN_RPT_TAB_XL_FD_By_Filter(PRA_MN_RPT_TAB_XL_BD_ReportSearchFilter filter);

        Task<OLRSHubResponse> Add_PRA_MN_RPT_TAB_XL_BD(List<PRA_MN_RPT_TAB_XL_BD_Model> pra_MN_RPT_TAB_XL_BD_List, string accessToken);

        #endregion

        #region Upazilla Loan

        Task<List<PRA_LN_DIST_WISE_DISB_Model>> Get_PRA_LN_DIST_WISE_DISB_By_Filter(PRA_LN_DIST_WISE_DISB_ReportSearchFilter filter);
        Task<OLRSHubResponse> Add_PRA_LN_DIST_WISE_DISB(List<PRA_LN_DIST_WISE_DISB_Model> pra_LN_DIST_WISE_DISB_List, string accessToken);
        Task<OLRSHubResponse> Update_PRA_LN_DIST_WISE_DISB(PRA_LN_DIST_WISE_DISB_Model parameter);

        #endregion

        #region Accounting
        Task<List<PO_MONTHLY_ACC_BAL_Model>> Get_PO_MONTHLY_ACC_BAL_By_Filter(PO_MONTHLY_ACC_BAL_ReportSearchFilter filter);
        Task<OLRSHubResponse> Add_PO_MONTHLY_ACC_BAL(List<PO_MONTHLY_ACC_BAL_Model> pra_LN_DIST_WISE_DISB_List, string accessToken);
        Task<PO_MONTHLY_ACC_BAL_SummaryModel> Get_PO_MONTHLY_ACC_BAL_Summary_By_Filter(PO_MONTHLY_ACC_BAL_ReportSearchFilter filter);
        #endregion

        #region TopSheet Data

        Task<List<TOP_SHEET_Model>> Get_TOP_SHEET_By_Filter(TOP_SHEET_ReportSearchFilter filter);
        Task<OLRSHubResponse> Add_TOP_SHEET(List<TOP_SHEET_Model> top_SHEET_List, string accessToken);

        #endregion
    }
}

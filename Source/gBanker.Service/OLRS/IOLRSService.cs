using BasicDataAccess;
using gBanker.Core.Filters;
using gBanker.Data.DBDetailModels.OLRSHubs;
using gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.OLRS
{
    public interface IOLRSService
    {
        #region Imputed Cost
        Task<List<PRA_MN_INF_EQUITY_Model>> PRA_MN_INF_EQUITY_LIST_BY_FILTER(BaseSearchFilter filter);
        Task<List<PRA_MN_IMP_COST_MASTER_Model>> PRA_MN_IMP_COST_MASTER_LIST_BY_FILTER(BaseSearchFilter filter);
        Task<List<PRA_MN_IMP_COST_LN_SC_Model>> PRA_MN_IMP_COST_LN_SC_LIST_BY_FILTER(BaseSearchFilter filter);
        Task<List<PRA_MN_IMP_COST_SV_INT_Model>> PRA_MN_IMP_COST_SV_INT_BY_FILTER(BaseSearchFilter filter);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_INF_EQUITY(string po_Code, string mnyr, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_INF_EQUITY_NOT_Sync(string po_Code, string mnyr, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_MASTER(string po_Code, string mnyr, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_LN_SC(string po_Code, string mnyr, decimal sc_rate, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_SV_INT(string po_Code, string mnyr, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_MASTER_NOT_Sync(string po_Code, string mnyr, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_SV_INT_NOT_Sync(string po_Code, string mnyr, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_LN_SC_NOT_Sync(string po_Code, string mnyr, decimal sc_rate, string updatedUser);


        #endregion

        #region Employment
        Task<OlrsSyncResponse> PRA_EMPLOYMENT_NEW_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter);
        Task<List<PRA_EMPLOYMENT_OLD_Model>> PRA_EMPLOYMENT_OLD_LIST_BY_FILTER(BaseSearchFilter filter);
        Task<List<PRA_EMPLOYMENT_NEW_Model>> PRA_EMPLOYMENT_NEW_LIST_BY_FILTER(BaseSearchFilter filter);
        Task<List<PRA_EMPLOYMENT_CLS_Model>> PRA_EMPLOYMENT_CLS_LIST_BY_FILTER(BaseSearchFilter filter);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_OLD(string po_Code, string mnyr, string loanCode, string updatedUser);
        Task<bool>  UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_NEW(string po_Code, string mnyr, string loanCode, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_CLS(string po_Code, string mnyr, string loanCode, string updatedUser);
        Task<bool> DeleteEmploymentData(string mnyr, string type);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_OLD_NOT_Sync(string po_Code, string mnyr, string loanCode, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_CLS_NOT_Sync(string po_Code, string mnyr, string loanCode, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_NEW_NOT_Sync(string po_Code, string mnyr, string loanCode, string updatedUser);

        #endregion

        #region Program Data
        Task<OlrsSyncResponse> PRA_MN_RPT_TAB_XL_PD_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter);
        Task<List<PRA_MN_RPT_TAB_XL_PD_Model>> PRA_MN_RPT_TAB_XL_PD_LIST_BY_FILTER(BaseSearchFilter filter);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_PD(string po_Code, string mnyr, string indCode, string m_f_flag, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_PD_NOT_Sync(string po_Code, string mnyr, string indCode, string m_f_flag, string updatedUser);
        #endregion

        #region Financial Data
        Task<OlrsSyncResponse> PRA_MN_RPT_TAB_XL_FD_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter);
        Task<List<PRA_MN_RPT_TAB_XL_FD_Model>> PRA_MN_RPT_TAB_XL_FD_LIST_BY_FILTER(BaseSearchFilter filter);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_FD(string po_Code, string mnyr, string ind_code, string m_f_flag, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_FD_NOT_Sync(string po_Code, string mnyr, string ind_code, string m_f_flag, string updatedUser);
        #endregion

        #region Basic Data
        Task<OlrsSyncResponse> PRA_MN_RPT_TAB_XL_BD_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter);
        Task<List<PRA_MN_RPT_TAB_XL_BD_Model>> PRA_MN_RPT_TAB_XL_BD_LIST_BY_FILTER(BaseSearchFilter filter);

        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_BD(string po_Code, string mnyr, string ind_code, string m_f_flag, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_BD_Not_Sync(string po_Code, string mnyr, string ind_code, string m_f_flag, string updatedUser);

        #endregion

        #region Upazilla Loan
        Task<OlrsSyncResponse> PRA_LN_DIST_WISE_DISB_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter);
        Task<List<PRA_LN_DIST_WISE_DISB_Model>> PRA_LN_DIST_WISE_DISB_LIST_BY_FILTER(BaseSearchFilter filter);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_LN_DIST_WISE_DISB(string po_Code, string mnyr, string dist_code, string tha_code, string loan_code, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_LN_DIST_WISE_DISB_NOT_Sync(string po_Code, string mnyr, string dist_code, string tha_code, string loan_code, string updatedUser);
        #endregion

        #region Accounting
        Task<OlrsSyncResponse> PO_MONTHLY_ACC_BAL_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter);
        Task<List<PO_MONTHLY_ACC_BAL_Model>> PO_MONTHLY_ACC_BAL_LIST_BY_FILTER(BaseSearchFilter filter);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PO_MONTHLY_ACC_BAL(string po_Code, string mnyr,
        string company_code,
        string company_branch_code,
        string finance_code,
        string project_code,
        string component_code,
        string acc_group,
        string coa_id,
        string l1_code,
        string l2_code,
        string l3_code,
        string l4_code,
        string l5_code,
        string updatedUser);

        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PO_MONTHLY_ACC_BAL_NOT_Sync(string po_Code, string mnyr,
        string company_code,
        string company_branch_code,
        string finance_code,
        string project_code,
        string component_code,
        string acc_group,
        string coa_id,
        string l1_code,
        string l2_code,
        string l3_code,
        string l4_code,
        string l5_code,
        string updatedUser);

        #endregion

        #region Top Sheet Data
        Task<OlrsSyncResponse> TOP_SHEET_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter);
        Task<List<TOP_SHEET_Model>> TOP_SHEET_LIST_BY_FILTER(BaseSearchFilter filter);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_TOP_SHEET(string po_Code, string mnyr, string updatedUser);
        Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_TOP_SHEET_NOT_Sync(string po_Code, string mnyr, string updatedUser);
        #endregion
    }
}

using gBanker.Core.Filters;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels.OLRSHubs;
using gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gBanker.Service.OLRS
{
    public class OLRSService : IOLRSService
    {
        #region Imputed Cost

        public async Task<List<PRA_MN_INF_EQUITY_Model>> PRA_MN_INF_EQUITY_LIST_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using(var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_INF_EQUITY_GetINFEquityInformation] '{filter.PoCode}', '{filter.MNYR}','{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<PRA_MN_INF_EQUITY_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_MN_INF_EQUITY_Model>();
            }
        }

        public async Task<List<PRA_MN_IMP_COST_MASTER_Model>> PRA_MN_IMP_COST_MASTER_LIST_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_IMP_COST_MASTER_Get_PRA_MN_IMP_COST_MASTER_FOR_SYNC] '{filter.PoCode}','{filter.MNYR}', '{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<PRA_MN_IMP_COST_MASTER_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_MN_IMP_COST_MASTER_Model>();
            }
        }

        public async Task<List<PRA_MN_IMP_COST_LN_SC_Model>> PRA_MN_IMP_COST_LN_SC_LIST_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_IMP_COST_LN_SC_Get_PRA_MN_IMP_COST_LN_SC_FOR_SYNC] '{filter.PoCode}','{filter.MNYR}', '{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<PRA_MN_IMP_COST_LN_SC_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_MN_IMP_COST_LN_SC_Model>();
            }
        }

        public async Task<List<PRA_MN_IMP_COST_SV_INT_Model>> PRA_MN_IMP_COST_SV_INT_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_IMP_COST_SV_INT_Get_PRA_MN_IMP_COST_SV_INT_FOR_SYNC] '{filter.PoCode}','{filter.MNYR}','{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<PRA_MN_IMP_COST_SV_INT_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_MN_IMP_COST_SV_INT_Model>();
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_INF_EQUITY(string po_Code, string mnyr,string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_INF_EQUITY_UPDATE_FLAG_AND_REMOVE_DUMP] '{po_Code}','{mnyr}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_MASTER(string po_Code, string mnyr, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_IMP_COST_MASTER_UPDATE_FLAG_AND_REMOVE_DUMP] '{po_Code}','{mnyr}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_LN_SC(string po_Code, string mnyr, decimal sc_rate, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_IMP_COST_LN_SC_UPDATE_FLAG_AND_REMOVE_DUMP] '{po_Code}' ,'{mnyr}','{sc_rate}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_SV_INT(string po_Code, string mnyr,string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_IMP_COST_SV_INT_UPDATE_FLAG_AND_REMOVE_DUMP] '{po_Code}','{mnyr}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_MASTER_NOT_Sync(string po_Code, string mnyr, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_IMP_COST_MASTER_UPDATE_FLAG_AND_REMOVE_DUMP_NOT_Sync] '{po_Code}','{mnyr}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_INF_EQUITY_NOT_Sync(string po_Code, string mnyr, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_INF_EQUITY_UPDATE_FLAG_AND_REMOVE_DUMP_NOT_Sync] '{po_Code}','{mnyr}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_SV_INT_NOT_Sync(string po_Code, string mnyr, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_IMP_COST_SV_INT_UPDATE_FLAG_AND_REMOVE_DUMP_NOT_Sync] '{po_Code}','{mnyr}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_IMP_COST_LN_SC_NOT_Sync(string po_Code, string mnyr, decimal sc_rate, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_IMP_COST_LN_SC_UPDATE_FLAG_AND_REMOVE_DUMP_NOT_Sync] '{po_Code}' ,'{mnyr}' ,'{sc_rate}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        #endregion

        #region Employment

        public async Task<OlrsSyncResponse> PRA_EMPLOYMENT_NEW_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_LN_WISE_Employment_CHECK_DATA_SYNC_TO_PKSF] '{filter.MNYR}'";
                    var response = await db.Database.SqlQuery<OlrsSyncResponse>(sqlCommand).FirstOrDefaultAsync();

                    return response;
                }
            }
            catch (Exception ex)
            {
                return new OlrsSyncResponse();
            }
        }

        public async Task<List<PRA_EMPLOYMENT_OLD_Model>> PRA_EMPLOYMENT_OLD_LIST_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_EMPLOYMENT_OLD_Get_PRA_EMPLOYMENT_OLD_FOR_SYNC] '{filter.PoCode}','{filter.MNYR}' ,'{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<PRA_EMPLOYMENT_OLD_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_EMPLOYMENT_OLD_Model>();
            }
        }

        public async Task<List<PRA_EMPLOYMENT_NEW_Model>> PRA_EMPLOYMENT_NEW_LIST_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_EMPLOYMENT_NEW_Get_PRA_EMPLOYMENT_NEW_FOR_SYNC] '{filter.PoCode}','{filter.MNYR}', '{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<PRA_EMPLOYMENT_NEW_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_EMPLOYMENT_NEW_Model>();
            }
        }

        public async Task<List<PRA_EMPLOYMENT_CLS_Model>> PRA_EMPLOYMENT_CLS_LIST_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_EMPLOYMENT_CLS_Get_PRA_EMPLOYMENT_CLS_FOR_SYNC] '{filter.PoCode}','{filter.MNYR}', '{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<PRA_EMPLOYMENT_CLS_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_EMPLOYMENT_CLS_Model>();
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_OLD(string po_Code, string mnyr, string loanCode, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_EMPLOYMENT_OLD_UPDATE_FLAG_AND_REMOVE_DUMP] '{po_Code}','{mnyr}','{loanCode}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_NEW(string po_Code, string mnyr, string loanCode, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_EMPLOYMENT_NEW_UPDATE_FLAG_AND_REMOVE_DUMP] '{po_Code}','{mnyr}','{loanCode}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_CLS(string po_Code, string mnyr,string loanCode, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_EMPLOYMENT_CLS_UPDATE_FLAG_AND_REMOVE_DUMP] '{po_Code}','{mnyr}','{loanCode}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEmploymentData(string mnyr, string type)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_EMPLOYMENT_DeleteEmploymentData] '{mnyr}','{type}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_OLD_NOT_Sync(string po_Code, string mnyr, string loanCode, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_EMPLOYMENT_OLD_UPDATE_FLAG_AND_REMOVE_DUMP_NOT_Sync] '{po_Code}','{mnyr}','{loanCode}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_CLS_NOT_Sync(string po_Code, string mnyr, string loanCode, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_EMPLOYMENT_CLS_UPDATE_FLAG_AND_REMOVE_DUMP_NOT_Sync] '{po_Code}','{mnyr}','{loanCode}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_NEW_NOT_Sync(string po_Code, string mnyr, string loanCode, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_EMPLOYMENT_NEW_UPDATE_FLAG_AND_REMOVE_DUMP_NOT_Sync] '{po_Code}','{mnyr}','{loanCode}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion

        #region Program Data

        public async Task<OlrsSyncResponse> PRA_MN_RPT_TAB_XL_PD_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_PD_CHECK_DATA_SYNC_TO_PKSF] '{filter.MNYR}'";
                    var response = await db.Database.SqlQuery<OlrsSyncResponse>(sqlCommand).FirstOrDefaultAsync();

                    return response;
                }
            }
            catch (Exception ex)
            {
                return new OlrsSyncResponse();
            }
        }

        public async Task<List<PRA_MN_RPT_TAB_XL_PD_Model>> PRA_MN_RPT_TAB_XL_PD_LIST_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_PD_Get_PRA_MN_RPT_TAB_XL_PD_FOR_SYNC] '{filter.PoCode}', '{filter.MNYR}',  '{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<PRA_MN_RPT_TAB_XL_PD_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_MN_RPT_TAB_XL_PD_Model>();
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_PD(string po_Code, string mnyr, string indCode,string m_f_flag, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_PD_UPDATE_FLAG_AND_REMOVE_DUMP] '{po_Code}','{mnyr}','{indCode}','{m_f_flag}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);
                    

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_PD_NOT_Sync(string po_Code, string mnyr, string indCode, string m_f_flag, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_PD_UPDATE_FLAG_AND_REMOVE_DUMP_NOT_Sync] '{po_Code}','{mnyr}','{indCode}','{m_f_flag}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);


                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        #endregion

        #region Financial Data

        public async Task<OlrsSyncResponse> PRA_MN_RPT_TAB_XL_FD_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_FD_CHECK_DATA_SYNC_TO_PKSF] '{filter.MNYR}'";
                    var response = await db.Database.SqlQuery<OlrsSyncResponse>(sqlCommand).FirstOrDefaultAsync();

                    return response;
                }
            }
            catch (Exception ex)
            {
                return new OlrsSyncResponse();
            }
        }

        public async Task<List<PRA_MN_RPT_TAB_XL_FD_Model>> PRA_MN_RPT_TAB_XL_FD_LIST_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_FD_Get_PRA_MN_RPT_TAB_XL_FD_FOR_SYNC] '{filter.PoCode}', '{filter.MNYR}','{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<PRA_MN_RPT_TAB_XL_FD_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_MN_RPT_TAB_XL_FD_Model>();
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_FD(string po_Code, string mnyr, string ind_code,string m_f_flag, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_FD_UPDATE_FLAG_AND_REMOVE_DUMP] '{po_Code}','{mnyr}','{ind_code}','{m_f_flag}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_FD_NOT_Sync(string po_Code, string mnyr, string ind_code, string m_f_flag, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_FD_UPDATE_FLAG_AND_REMOVE_DUMP_NOT_Sync] '{po_Code}','{mnyr}','{ind_code}','{m_f_flag}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion

        #region Basic Data

        public async Task<OlrsSyncResponse> PRA_MN_RPT_TAB_XL_BD_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_BD_CHECK_DATA_SYNC_TO_PKSF] '{filter.MNYR}'";
                    var response = await db.Database.SqlQuery<OlrsSyncResponse>(sqlCommand).FirstOrDefaultAsync();

                    return response;
                }
            }
            catch (Exception ex)
            {
                return new OlrsSyncResponse();
            }
        }

        public async Task<List<PRA_MN_RPT_TAB_XL_BD_Model>> PRA_MN_RPT_TAB_XL_BD_LIST_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_BD_Get_PRA_MN_RPT_TAB_XL_BD_FOR_SYNC] '{filter.PoCode}','{filter.MNYR}','{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<PRA_MN_RPT_TAB_XL_BD_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_MN_RPT_TAB_XL_BD_Model>();
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_BD(string po_Code, string mnyr, string ind_code,string m_f_flag, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_BD_UPDATE_FLAG_AND_REMOVE_DUMP] '{po_Code}','{mnyr}','{ind_code}','{m_f_flag}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_MN_RPT_TAB_XL_BD_Not_Sync(string po_Code, string mnyr, string ind_code, string m_f_flag, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_BD_UPDATE_FLAG_AND_REMOVE_DUMP_Not_Sync] '{po_Code}','{mnyr}','{ind_code}','{m_f_flag}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion

        #region Upazilla Loan

        public async Task<OlrsSyncResponse> PRA_LN_DIST_WISE_DISB_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_LN_DIST_WISE_DISB_CHECK_DATA_SYNC_TO_PKSF] '{filter.MNYR}'";
                    var response = await db.Database.SqlQuery<OlrsSyncResponse>(sqlCommand).FirstOrDefaultAsync();

                    return response;
                }
            }
            catch (Exception ex)
            {
                return new OlrsSyncResponse();
            }
        }

        public async Task<List<PRA_LN_DIST_WISE_DISB_Model>> PRA_LN_DIST_WISE_DISB_LIST_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_LN_DIST_WISE_DISB_Get_PRA_LN_DIST_WISE_DISB_FOR_SYNC] '{filter.PoCode}','{filter.MNYR}','{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<PRA_LN_DIST_WISE_DISB_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<PRA_LN_DIST_WISE_DISB_Model>();
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_LN_DIST_WISE_DISB(string po_Code, string mnyr, string dist_code,string tha_code, string loan_code, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_LN_DIST_WISE_DISB_UPDATE_FLAG_AND_REMOVE_DUMP] '{po_Code}','{mnyr}','{dist_code}','{tha_code}','{loan_code}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PRA_LN_DIST_WISE_DISB_NOT_Sync(string po_Code, string mnyr, string dist_code, string tha_code, string loan_code, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PRA_LN_DIST_WISE_DISB_UPDATE_FLAG_AND_REMOVE_DUMP_NOT_Sync] '{po_Code}','{mnyr}','{dist_code}','{tha_code}','{loan_code}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region Accounting


        public async Task<OlrsSyncResponse> PO_MONTHLY_ACC_BAL_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PO_MONTHLY_ACC_BAL_CHECK_DATA_SYNC_TO_PKSF] '{filter.MNYR}'";
                    var response = await db.Database.SqlQuery<OlrsSyncResponse>(sqlCommand).FirstOrDefaultAsync();

                    return response;
                }
            }
            catch (Exception ex)
            {
                return new OlrsSyncResponse();
            }
        }

        public async Task<List<PO_MONTHLY_ACC_BAL_Model>> PO_MONTHLY_ACC_BAL_LIST_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PO_MONTHLY_ACC_BAL_Get_PO_MONTHLY_ACC_BAL_FOR_SYNC] '{filter.PoCode}','{filter.MNYR}','{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<PO_MONTHLY_ACC_BAL_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<PO_MONTHLY_ACC_BAL_Model>();
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PO_MONTHLY_ACC_BAL(string po_Code, string mnyr,
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
        string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PO_MONTHLY_ACC_BAL_UPDATE_FLAG_AND_REMOVE_DUMP] 
                                    '{po_Code}',
                                    '{mnyr}',
                                    '{company_code}',
                                    '{company_branch_code}',
                                    '{finance_code}',
                                    '{project_code}',
                                    '{component_code}',
                                    '{acc_group}',
                                    '{coa_id}',
                                    '{l1_code}',
                                    '{l2_code}',
                                    '{l3_code}',
                                    '{l4_code}',
                                    '{l5_code}',
                                    '{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_PO_MONTHLY_ACC_BAL_NOT_Sync(string po_Code, string mnyr,
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
        string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[PO_MONTHLY_ACC_BAL_UPDATE_FLAG_AND_REMOVE_DUMP_NOT_Sync] 
                                    '{po_Code}',
                                    '{mnyr}',
                                    '{company_code}',
                                    '{company_branch_code}',
                                    '{finance_code}',
                                    '{project_code}',
                                    '{component_code}',
                                    '{acc_group}',
                                    '{coa_id}',
                                    '{l1_code}',
                                    '{l2_code}',
                                    '{l3_code}',
                                    '{l4_code}',
                                    '{l5_code}',
                                    '{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }




        #endregion

        #region Top Sheet Data

        public async Task<OlrsSyncResponse> TOP_SHEET_CHECK_DATA_SYNC_TO_PKSF(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[TOP_SHEET_CHECK_DATA_SYNC_TO_PKSF] '{filter.MNYR}'";
                    var response = await db.Database.SqlQuery<OlrsSyncResponse>(sqlCommand).FirstOrDefaultAsync();

                    return response;
                }
            }
            catch (Exception ex)
            {
                return new OlrsSyncResponse();
            }
        }
        public async Task<List<TOP_SHEET_Model>> TOP_SHEET_LIST_BY_FILTER(BaseSearchFilter filter)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[TOP_SHEET_Get_TOP_SHEET_FOR_SYNC] '{filter.PoCode}','{filter.MNYR}','{filter.PostingFlag}'";
                    var listing = await db.Database.SqlQuery<TOP_SHEET_Model>(sqlCommand).ToListAsync();

                    return listing;
                }
            }
            catch (Exception ex)
            {
                return new List<TOP_SHEET_Model>();
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_TOP_SHEET(string po_Code, string mnyr, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[TOP_SHEET_UPDATE_FLAG_AND_REMOVE_DUMP] '{po_Code}','{mnyr}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UPDATE_FLAG_AND_REMOVE_DUMP_TOP_SHEET_NOT_Sync(string po_Code, string mnyr, string updatedUser)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[pksf].[TOP_SHEET_UPDATE_FLAG_AND_REMOVE_DUMP_NOT_Sync] '{po_Code}','{mnyr}','{updatedUser}'";
                    await db.Database.ExecuteSqlCommandAsync(sqlCommand);

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }
}

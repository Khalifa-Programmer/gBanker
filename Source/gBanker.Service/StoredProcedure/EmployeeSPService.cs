using BasicDataAccess;
using BasicDataAccess.Data;
using gBanker.Core.Filters;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.StoredProcedure
{
    public interface IEmployeeSPService
    {
        DataSet InsertSAVINGRegisterUpdateINFO<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetPromotionInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetTimeScaleInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataWithoutParameter(string storeProcedureName);
        DataSet EligibleEmployeeForPromotion<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetLeaveForApproveList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetLeaveForAdjustList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetLeaveSellList<TParamOType>(TParamOType target) where TParamOType : class;
        string BulkInsertHelper(DataTable dt); //in interface
        string BulkInsertCSVForGTT(DataTable dt);
        string BulkInsertCSVForGC(DataTable dt);


        DataSet GetCenterROleWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet SpecialDepositWithdraw<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductSavingList<TParamOType>(TParamOType target) where TParamOType : class;
       

        DataSet GetSpecialSavingLastBalance<TParamOType>(TParamOType target) where TParamOType : class;

        DataSet Proc_getSavingLastBalance<TParamOType>(TParamOType target) where TParamOType : class;

        List<SearchFilterData> GetStaffwiseStatementDSKNewByFilter(SearchFilterData filter);
        List<Rpt_DailyCollectionReceiptFoWise_New_tbl> GetDailyRecoverableReceiptByFilter_NEW(SearchFilterData filter);
        List<Rpt_Product_tbl> GetProduct_NEW(SearchFilterData filter);
        List<Rpt_DailyCollectionReceiptFoWise> GetDailyRecoverableReceiptByFilter(SearchFilterData filter);


        DataSet GetNIDVerificationDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

    }
    public class EmployeeSPService : IEmployeeSPService
    {
        /// <summary>
        /// RND TEST
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<SearchFilterData> GetStaffwiseStatementDSKNewByFilter(SearchFilterData filter)
        {
            var filterList = new List<SearchFilterData>();

            try
            {
                using (var db = new gBankerDbContext())
                {
                    var officeId = filter.OfficeID > 0 ? filter.OfficeID.ToString() : "NULL";
                    var DateFrom = !string.IsNullOrWhiteSpace(filter.DateFrom.ToString()) ? $"'{filter.DateFrom.ToString()}'" : "NULL";
                    var DateTo = !string.IsNullOrWhiteSpace(filter.DateTo.ToString()) ? $"'{filter.DateTo.ToString()}'" : "NULL";

                    var sqlCommand = $@"[dbo].[Rpt_StaffwiseStatementDSKNew]
                                {officeId},
                                {DateFrom},
                                {DateTo}";

                    filterList = db.Database.SqlQuery<SearchFilterData>(sqlCommand)
                        .AsParallel().ToList();
                }
            }
            catch (Exception ex)
            {
                filterList = new List<SearchFilterData>();
            }

            return filterList;
        }
        public List<Rpt_DailyCollectionReceiptFoWise_New_tbl> GetDailyRecoverableReceiptByFilter_NEW(SearchFilterData filter)
        {
            var filterList = new List<Rpt_DailyCollectionReceiptFoWise_New_tbl>();
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var Office = filter.OfficeID > 0 ? filter.OfficeID.ToString() : "NULL";
                    var Date = !string.IsNullOrWhiteSpace(filter.DateTo.ToString()) ? $"'{filter.DateTo.ToString()}'" : "NULL";
                    var QType = filter.QType > 0 ? filter.QType.ToString() : "NULL";
                    var EmployeeID = filter.EmployeeId > 0 ? filter.EmployeeId.ToString() : "NULL";


                    var sqlCommand = $@"[dbo].[Rpt_DailyCollectionReceiptFoWise_New]
                                {Office},
                                {Date},
                                {QType},
                                {EmployeeID}";

                    //var sqlCommand = $@"[dbo].[Rpt_DailyCollectionReceiptFoWise_New_TEST]
                    //            {Office},
                    //            {Date},
                    //            {QType},
                    //            {EmployeeID}";

                    filterList = db.Database.SqlQuery<Rpt_DailyCollectionReceiptFoWise_New_tbl>(sqlCommand)
                        .AsParallel().ToList();
                }
            }
            catch (Exception ex)
            {
                filterList = new List<Rpt_DailyCollectionReceiptFoWise_New_tbl>();
            }

            return filterList;
        }

        public List<Rpt_Product_tbl> GetProduct_NEW(SearchFilterData filter)
        {
            var filterList = new List<Rpt_Product_tbl>();
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var sqlCommand = $@"[dbo].[Rpt_Get_Products]";
                    filterList = db.Database.SqlQuery<Rpt_Product_tbl>(sqlCommand)
                        .AsParallel().ToList();
                }
            }
            catch (Exception ex)
            {
                filterList = new List<Rpt_Product_tbl>();
            }
            return filterList;
        }

        public List<Rpt_DailyCollectionReceiptFoWise> GetDailyRecoverableReceiptByFilter(SearchFilterData filter)
                {
                    var filterList = new List<Rpt_DailyCollectionReceiptFoWise>();
                    try
                    {
                        using (var db = new gBankerDbContext())
                        {
                            var Office = filter.OfficeID > 0 ? filter.OfficeID.ToString() : "NULL";
                            var Date = !string.IsNullOrWhiteSpace(filter.DateTo.ToString()) ? $"'{filter.DateTo.ToString()}'" : "NULL";
                            var QType = filter.QType > 0 ? filter.QType.ToString() : "NULL";
                            var EmployeeID = filter.EmployeeId > 0 ? filter.EmployeeId.ToString() : "NULL";

                    /*
                            var sqlCommand = $@"[dbo].[Rpt_DailyCollectionReceiptFoWise]
                                                {Office},
                                                {Date},
                                                {QType},
                                                {EmployeeID}";
                    */ 
                    var sqlCommand = $@"[dbo].[Rpt_DailyCollectionReceiptFoWise_TEST]
                                                {Office},
                                                {Date},
                                                {QType},
                                                {EmployeeID}";
                            filterList = db.Database.SqlQuery<Rpt_DailyCollectionReceiptFoWise>(sqlCommand)
                                        .AsParallel().ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        filterList = new List<Rpt_DailyCollectionReceiptFoWise>();
                    }

                    return filterList;
                }

        public DataSet Proc_getSavingLastBalance<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_get_getSavingLastBalance";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet InsertSAVINGRegisterUpdateINFO<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_InsertSavingRegisterUpdateInfo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetSpecialSavingLastBalance<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Get_SpecialSavingLastBalance";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetPromotionInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_GetPromotionInfo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }       

        public DataSet GetTimeScaleInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_GetTimeScaleInfo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet EligibleEmployeeForPromotion<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GET_EligibleEmployeeForPromotion";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetProductSavingList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getProductByMemberSavings";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet SpecialDepositWithdraw<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SpecialDepositWithdraw";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet SpecialDepositWithdraw<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetCenterROleWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getUserRole";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        //Changed by Mansur 
        public DataSet GetDataWithoutParameter(string storeProcedureName)
        {
            //var sPName = "SP_IPD_VisitorInfoAll";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDatesetWithoutParam(storeProcedureName);
            }
        }

        public DataSet GetLeaveForApproveList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_GetLeaveForApproveList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetLeaveForAdjustList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_GetLeaveForAdjustList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetLeaveSellList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_GetLeaveSellList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetNIDVerificationDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public class TempTime
        {
            public string EmployeeCode { get; set; }
        }

        public string BulkInsertHelper(DataTable dt)//Insert Attendance CSV Data
        {
            DataTable table = new DataTable();
            table.Columns.Add("SNo", typeof(int));
            table.Columns.Add("EmployeeCode", typeof(string));
            table.Columns.Add("EmployeeName", typeof(string));
            table.Columns.Add("AttendanceDate", typeof(DateTime));
            table.Columns.Add("AttendanceTime", typeof(string));
            table.Columns.Add("TimeStamp", typeof(DateTime));
            //table.Columns.Add("")
            DataRow row1 = table.NewRow();
            string empCode = string.Empty;
            string empName = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                row1 = table.NewRow();
                row1["SNo"] = 0;
                row1["EmployeeCode"] = row[1].ToString().Replace('"', ' ').Trim();
                row1["EmployeeName"] = row[2].ToString().Replace('"', ' ').Trim();
                var dateAtten = row[5].ToString().Replace('"', ' ').Trim();
                var dateTime = DateTime.ParseExact(dateAtten, "yyyy/M/d", CultureInfo.InvariantCulture);
                var dateOnly = dateTime.ToShortDateString();
                row1["AttendanceDate"] = dateTime;
                row1["AttendanceTime"] = row[6].ToString().Replace('"', ' ').Trim();
                var timeStamp = Convert.ToDateTime(dateOnly + " " + row[6].ToString().Replace('"', ' ').Trim());
                row1["TimeStamp"] = timeStamp;
                //1/1/2017 7:44

                //row1["TimeStamp"] = Convert.ToDateTime(row[5].ToString().Replace('"', ' ').Trim() + " " + row[6].ToString().Replace('"', ' ').Trim());

                table.Rows.Add(row1);
            }

            var gbData = new gBankerDataAccess();

            var ConnString = gbData.GetConnectionString();
            using (SqlConnection con = new SqlConnection(ConnString))
            {
                //return gbData.GetDataOnDateset(storeProcedureName, target);

                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name
                    sqlBulkCopy.DestinationTableName = "dbo.AttCSVData";

                    //[OPTIONAL]: Map the DataTable columns with that of the database table
                    //sqlBulkCopy.ColumnMappings.Add("User ID", "EmployeeCode");
                    //sqlBulkCopy.ColumnMappings.Add("TimeStamp", "TimeStamp");
                    ////sqlBulkCopy.ColumnMappings.Add("h3", "h3");
                    con.Open();
                    sqlBulkCopy.WriteToServer(table);
                    con.Close();
                }
            }
            return "ok";
        }  // End Of Method

        public string BulkInsertCSVForGTT(DataTable dt)//Insert Attendance CSV Data
        {
            DataTable table = new DataTable();
            table.Columns.Add("SNo", typeof(int));
            table.Columns.Add("EmployeeCode", typeof(string));
            table.Columns.Add("EmployeeName", typeof(string));
            table.Columns.Add("AttendanceDate", typeof(DateTime));
            table.Columns.Add("AttendanceTime", typeof(string));
            table.Columns.Add("TimeStamp", typeof(DateTime));
            DataRow row1 = table.NewRow();

            foreach (DataRow row in dt.Rows)
            {
                var employeeCode = row[0].ToString().Replace('"', ' ').Trim();
                var employeeName = row[1].ToString().Replace('"', ' ').Trim();
                var attendanceDate = row[2].ToString().Replace('"', ' ').Trim();
                var attendanceDateTime = Convert.ToDateTime(attendanceDate);
                //DateTime.ParseExact(attendanceDate, "yyyy/M/d", CultureInfo.InvariantCulture);
                //DateTime.ParseExact(attendanceDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                var attendanceDateOnly = attendanceDateTime.ToShortDateString();
                string attendanceTimeOnly = attendanceDateTime.ToString("hh:mm:ss");

                row1 = table.NewRow();
                row1["SNo"] = 0;
                row1["EmployeeCode"] = employeeCode;
                row1["EmployeeName"] = employeeName;
                row1["AttendanceDate"] = attendanceDateOnly;
                row1["AttendanceTime"] = attendanceTimeOnly;
                var timeStamp = attendanceDateTime;
                row1["TimeStamp"] = timeStamp;
                table.Rows.Add(row1);
            }

            var gbData = new gBankerDataAccess();

            var ConnString = gbData.GetConnectionString();
            using (SqlConnection con = new SqlConnection(ConnString))
            {
                SqlDataAdapter adp = new SqlDataAdapter();
                //adp.SelectCommand.CommandTimeout = 0;

                //return gbData.GetDataOnDateset(storeProcedureName, target);

                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    //Set the database table name
                    sqlBulkCopy.DestinationTableName = "dbo.AttCSVData";

                    //[OPTIONAL]: Map the DataTable columns with that of the database table
                    //sqlBulkCopy.ColumnMappings.Add("User ID", "EmployeeCode");
                    //sqlBulkCopy.ColumnMappings.Add("TimeStamp", "TimeStamp");
                    ////sqlBulkCopy.ColumnMappings.Add("h3", "h3");
                    con.Open();
                    sqlBulkCopy.WriteToServer(table);
                    con.Close();
                }
            }
            return "ok";
        }  // End Of Method



        public string BulkInsertCSVForGC(DataTable dt)//Insert Attendance CSV Data
        {
            DataTable table = new DataTable();
            table.Columns.Add("SNo", typeof(int));
            table.Columns.Add("EmployeeCode", typeof(string));
            table.Columns.Add("EmployeeName", typeof(string));
            table.Columns.Add("AttendanceDate", typeof(DateTime));
            table.Columns.Add("AttendanceTime", typeof(string));
            table.Columns.Add("TimeStamp", typeof(DateTime));
            DataRow row1 = table.NewRow();

            foreach (DataRow row in dt.Rows)
            {
                var employeeCode = row[1].ToString().Replace('"', ' ').Trim();
                if (employeeCode.Length != 4)
                {
                    if (employeeCode.Length == 2)
                    {
                        employeeCode = "00" + employeeCode;
                    }
                    if (employeeCode.Length == 1)
                    {
                        employeeCode = "000" + employeeCode;
                    }
                    if (employeeCode.Length == 3)
                    {
                        employeeCode = "0" + employeeCode;
                    }
                }
                var employeeName = row[2].ToString().Replace('"', ' ').Trim() + " " + row[3].ToString().Replace('"', ' ').Trim();
                var dateAtten = row[5].ToString().Replace('"', ' ').Trim();

                var dateTime = DateTime.ParseExact(dateAtten, "yyyy/M/d", CultureInfo.InvariantCulture);
                //DateTime.ParseExact(dateAtten, "d/M/yyyy", CultureInfo.InvariantCulture); //DateTime.ParseExact(dateAtten, "yyyy/M/d", CultureInfo.InvariantCulture);
                var dateOnly = dateTime.ToShortDateString();
                var attendanceTime = row[6].ToString().Replace('"', ' ').Trim();
                var timeStamp = Convert.ToDateTime(dateOnly + " " + attendanceTime);
                row1 = table.NewRow();
                row1["SNo"] = 0;
                row1["EmployeeCode"] = employeeCode;
                row1["EmployeeName"] = employeeName;
                row1["AttendanceDate"] = dateTime;
                row1["AttendanceTime"] = attendanceTime;
                row1["TimeStamp"] = timeStamp;
                table.Rows.Add(row1);
            }

            var gbData = new gBankerDataAccess();

            var ConnString = gbData.GetConnectionString();
            using (SqlConnection con = new SqlConnection(ConnString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {                    
                    sqlBulkCopy.DestinationTableName = "dbo.AttCSVData";                   
                    con.Open();
                    sqlBulkCopy.WriteToServer(table);
                    con.Close();
                }
            }
            return "ok";
        }

       
    }
}

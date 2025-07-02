using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System.Data;
using System.Data.SqlClient;

namespace gBanker.Web.Controllers
{
    public class SearchMemberController : BaseController
    {

        #region Variables
  
        private readonly IUltimateReportService unlimitedReportService;
        private readonly IUltimateReportServiceMemberPortal unlimitedReportServiceMemberPortal;

        public SearchMemberController(IUltimateReportService unlimitedReportService, IUltimateReportServiceMemberPortal unlimitedReportServiceMemberPortal)
        { 
            this.unlimitedReportService = unlimitedReportService;
            this.unlimitedReportServiceMemberPortal = unlimitedReportServiceMemberPortal;
        }
        #endregion

        // GET: SearchMember
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetMemberLoanInfo(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, string typeFilterColumn,
           string txtPhoneNo,
           string txtNationalId             ,
           string txtPassport               ,
           string txtDrivingLicense         ,
           string txtBirthCertificateNo     ,
           string txtOther                  ,
           string txtSmartCard              
             
           )
        {
            try
            {

                List<SearchMemberViewModel> List_EmployeeViewModel = new List<SearchMemberViewModel>();
                var param = new
                {
                    PhoneNo              = txtPhoneNo            ,
                    NationalId           = txtNationalId         ,
                    Passport             = txtPassport           ,
                    DrivingLicense       = txtDrivingLicense     ,
                    BirthCertificateNo   = txtBirthCertificateNo ,
                    Other                = txtOther              ,
                    SmartCard            = txtSmartCard

                };
                var DataList = new DataSet();
                
                DataList = unlimitedReportServiceMemberPortal.GetDataWithParameter(param, "GetMemberOrg"); // getVirtualImmatureList
            // END
             
                List_EmployeeViewModel = DataList.Tables[0].AsEnumerable()
                .Select(row => new SearchMemberViewModel
                {
                    MemberCode              = row.Field<string>("MemberCode"),
                    MemberName              = row.Field<string>("MemberName"),
                    RefereeName             = row.Field<string>("RefereeName"),
                    DistrictName            = row.Field<string>("DistrictName"),
                    DivisionName            = row.Field<string>("DivisionName"),
                    OrganaizationName       = row.Field<string>("OrganaizationName")

                }).ToList();

                var currentPageRecords = List_EmployeeViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords.OrderBy(x => x.InsNo).ToList(), TotalRecordCount = List_EmployeeViewModel.LongCount(), JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// list 


        public JsonResult SwitchServers(int orgId)  // RND With Connection String.
        {
            try
            {
                List<ServerLoginCredentialsViewModel> List_Members = new List<ServerLoginCredentialsViewModel>();

                var param = new { OrgId = orgId };
                var alldata = unlimitedReportServiceMemberPortal.GetDataWithParameter(param, "sp_ServerLoginCredentials");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new ServerLoginCredentialsViewModel
                {
                    ServerIP = row.Field<string>("ServerIP"),
                    User = row.Field<string>("User"),
                    Password = row.Field<string>("Password"),
                    DatabaseName = row.Field<string>("DatabaseName")
                    
                }).ToList();


                foreach (var v in List_Members)
                {
                    //string queryString = "SELECT tPatCulIntPatIDPk, tPatSFirstname, tPatSName, tPatDBirthday  FROM  [dbo].[TPatientRaw] WHERE tPatSName = @tPatSName";
                    //string connectionString = "Server=.\PDATA_SQLEXPRESS;Database=;User Id=sa;Password=2BeChanged!;"; 

                    string connectionString = @"Data Source=" + v.ServerIP + ";Initial Catalog=" + v.DatabaseName + ";User ID=" + v.User + ";Password=" + v.Password  ;

                    //using (SqlConnection connection = new SqlConnection(connectionString))
                    //{
                    //    SqlCommand command = new SqlCommand(queryString, connection);
                    //    //command.Parameters.AddWithValue("@tPatSName", "Your-Parm-Value");
                    //    connection.Open();
                    //    SqlDataReader reader = command.ExecuteReader();
                    //    try
                    //    {
                    //        //while (reader.Read())
                    //        //{
                    //        //    //Console.WriteLine(String.Format("{0}, {1}",
                    //        //    //reader["tPatCulIntPatIDPk"], reader["tPatSFirstname"]));// etc
                    //        //}
                    //    }
                    //    finally
                    //    {
                    //        // Always call Close when done reading.
                    //        reader.Close();
                    //        connection.Close();
                    //    }
                    //}// END

                    var ServerIP = @"192.192.192.188\MSSQLSERVER2016";
                    var DatabaseName = "gBankerSMSDb";
                    var User = "gBanker6";
                    var Password = "gBanker6";

                    string csDest = @"Data Source=" + ServerIP + ";Initial Catalog=" + DatabaseName + ";User ID=" + User + ";Password=" + Password;


                    // Create source connection
                    SqlConnection source = new SqlConnection(connectionString);
                    // Create destination connection
                    SqlConnection destination = new SqlConnection(csDest);

                    // Clean up destination table. Your destination database must have the
                    // table with schema which you are copying data to.
                    // Before executing this code, you must create a table BulkDataTable
                    // in your database where you are trying to copy data to.

                    //SqlCommand cmd = new SqlCommand("DELETE FROM MemberPortal_Bulk", destination);
                    SqlCommand cmd = new SqlCommand("SELECT 1 ", destination);
                    // Open source and destination connections.
                    source.Open();
                    destination.Open();
                    cmd.ExecuteNonQuery();
                    // Select data from Products table
                    cmd = new SqlCommand(@"SELECT TOP 5000 * FROM MemberPortal", source);
                    // Execute reader
                    SqlDataReader reader = cmd.ExecuteReader();
                    // Create SqlBulkCopy
                    SqlBulkCopy bulkData = new SqlBulkCopy(destination);
                    // Set destination table name
                    bulkData.DestinationTableName = "MemberPortal_Bulk";
                    // Write data
                    bulkData.WriteToServer(reader);
                    // Close objects
                    bulkData.Close();
                    destination.Close();
                    source.Close();

                    //using (SqlConnection connSource = new SqlConnection(connectionString))
                    //using (SqlCommand cmd = connSource.CreateCommand())
                    //using (SqlBulkCopy bcp = new SqlBulkCopy(csDest))
                    //{
                    //    bcp.DestinationTableName = "SomeTable";
                    //    cmd.CommandText = "myproc";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    connSource.Open();
                    //    using (SqlDataReader reader = cmd.ExecuteReader())
                    //    {
                    //        bcp.WriteToServer(reader);

                    //    }
                    //}
                }
                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }// END Saving Installment




    }// END Class
}// END Namespace
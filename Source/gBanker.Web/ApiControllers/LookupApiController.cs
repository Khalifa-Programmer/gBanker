using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.ApiControllers.ApiModels;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace gBanker.Web.ApiControllers
{
    public class LookupApiController : ApiController
    {
        private readonly IOfficeService officeService;
        private readonly ICenterService centerServivce;
        private readonly IProductService productService;
        private readonly IPurposeService purposeService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IEmployeeService employeeService;

        public LookupApiController(IOfficeService officeService, ICenterService centerServivce, IProductService productService, IPurposeService purposeService, IGroupwiseReportService groupwiseReportService, IUltimateReportService ultimateReportService, IEmployeeService employeeService)
        {
            this.officeService = officeService;
            this.centerServivce = centerServivce;
            this.productService = productService;
            this.purposeService = purposeService;
            this.groupwiseReportService = groupwiseReportService;
            this.ultimateReportService = ultimateReportService;
            this.employeeService = employeeService;
        }
        [System.Web.Mvc.Route("api/lookupapi/getalloffice")]
        public List<SelectListItem> GetAllOffice()
        {
            return officeService.GetAll().Where(w => w.IsActive).Select(s => new SelectListItem() { Text = s.OfficeName, Value = s.OfficeID.ToString() }).ToList();
        }
        [System.Web.Mvc.Route("api/lookupapi/getcenterbyoffice")]
        public List<SelectListItem> GetCenterByOffice(int officeID, string loginID)
        {

            LogToFile(string.Format("GetCenterByOffice - OfficeID:{0}, Loginid:{1} ", officeID, loginID));
            var empid = employeeService.GetByCode(loginID);
            if (empid == null)
                throw new Exception("Invalid Login ID");
            //var allcenter = centerServivce.GetByOfficeId(officeID, 4, empid.EmployeeID);

            ////var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt16(LoggedInEmployeeID));
            //return allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() }).ToList();


            try
            {
                var param = new { OfficeId = officeID, EmployeeId = empid.EmployeeID };

                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "getCenterByEmployee");

                var lst = alldata.Tables[0].AsEnumerable()
                    .Select(row => new SelectListItem()
                    {
                        Text = row.Field<string>("CenterName"),
                        Value = row.Field<int>("CenterID").ToString()
                    }).ToList();
                LogToFile(string.Format("GetCenterByOffice - Result Count: {0} ", lst.Count()));
                return lst;
            }
            catch (Exception ex)
            {
                LogToFile(string.Format("Error Occured on GetCenterByOffice. OfficeID: {1}, Employee: {2}, Error: {0}", ex.Message, officeID, loginID));
                throw ex;
            }
        }

        [System.Web.Mvc.Route("api/lookupapi/getofficesyncdata")]
        public List<MobileOfficeDataSyncModel> GetOfficeSyncData(int officeID, string loginID)
        {

            LogToFile(string.Format("GetOfficeSyncData - OfficeID:{0}, LoginID: {1}", officeID, loginID));
            var empid = employeeService.GetByCode(loginID);
            if (empid == null)
                throw new Exception("Invalid Login ID");
            try
            {
                var param = new { OfficeId = officeID, EmployeeID = empid.EmployeeID };

                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetOfficeMemberDetailAllSamity");

                var lst = alldata.Tables[0].AsEnumerable()
                    .Select(row => new MobileOfficeDataSyncModel()
                    {
                        ProductID = int.Parse(row["ProductID"].ToString()),
                        ProductName = row["ProductName"].ToString(),
                        ProductType = int.Parse(row["ProductType"].ToString()),
                        LoanRecovery = decimal.Parse(row["LoanRecovery"].ToString()),
                        Recoverable = decimal.Parse(row["Recoverable"].ToString()),
                        Balance = decimal.Parse(row["Balance"].ToString()),
                        PrinBalance = decimal.Parse(row["PrinBalance"].ToString()),
                        SerBalance = decimal.Parse(row["SerBalance"].ToString()),
                        InstallmentNo = row.Field<int>("installmentNo"),
                        MemberID = int.Parse(row["MemberID"].ToString()),
                        MemberCode = row["MemberCode"].ToString(),
                        MemberName = row["MemberName"].ToString(),
                        CenterID = int.Parse(row["CenterID"].ToString()),
                        CenterCode = row["CenterCode"].ToString(),
                        CenterName = row["CenterName"].ToString(),
                        InstallmentDate = row["InstallmentDate"].ToString(),
                        TrxType = int.Parse(row["TrxType"].ToString()),
                        EmployeeName = empid.EmpName,
                        SummaryID = long.Parse(row["SummaryID"].ToString()),
                        CumIntCharge = decimal.Parse(row["CumIntCharge"].ToString()),
                        CumInterestPaid = decimal.Parse(row["CumInterestPaid"].ToString()),
                        Duration = int.Parse(row["Duration"].ToString()),
                        DurationOverIntDue = decimal.Parse(row["DurationOverIntDue"].ToString()),
                        DurationOverLoanDue = decimal.Parse(row["DurationOverLoanDue"].ToString()),
                        IntDue = decimal.Parse(row["IntDue"].ToString()),
                        InterestCalculationMethod = row["InterestCalculationMethod"].ToString(),
                        LoanDue = decimal.Parse(row["LoanDue"].ToString()),
                        LoanRepaid = decimal.Parse(row["LoanRepaid"].ToString()),
                        PrincipalLoan = decimal.Parse(row["PrincipalLoan"].ToString()),
                        IntCharge = decimal.Parse(row["IntCharge"].ToString()),
                        NewDue = decimal.Parse(row["NewDue"].ToString()),
                        MainProductCode = row["MainProductCode"].ToString(),
                        Doc = int.Parse(row["doc"].ToString())

                    }).ToList();

                LogToFile(string.Format("GetOfficeSyncData - Result Count: {0} ", lst.Count()));

                try
                {
                    var json = new JavaScriptSerializer().Serialize(lst);
                    LogToFileJson(json);
                }
                catch (Exception e)
                {

                }
                return lst;
            }
            catch (Exception ex)
            {
                LogToFile(string.Format("Error Occured on GetOfficeSyncData. OfficeID: {1}, Error: {0}", ex.Message, officeID));
                throw ex;
            }
        }

        //[System.Web.Mvc.Route("api/lookupapi/getofficesyncdata")]
        //public List<MobileOfficeDataSyncModel> GetOfficeSyncData(int officeID, string loginID)
        //{

        //    LogToFile(string.Format("GetOfficeSyncData - OfficeID:{0}, LoginID: {1}", officeID, loginID));
        //    var empid = employeeService.GetByCode(loginID);
        //    if (empid == null)
        //        throw new Exception("Invalid Login ID");
        //    try
        //    {
        //        var param = new { OfficeId = officeID, EmployeeID = empid.EmployeeID };

        //        var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetOfficeMemberDetailAllSamity");

        //        var lst = alldata.Tables[0].AsEnumerable()
        //            .Select(row => new MobileOfficeDataSyncModel()
        //            {
        //                ProductID = int.Parse(row["ProductID"].ToString()),
        //                ProductName = row["ProductName"].ToString(),
        //                ProductType = int.Parse(row["ProductType"].ToString()),
        //                LoanRecovery = decimal.Parse(row["LoanRecovery"].ToString()),
        //                Recoverable = decimal.Parse(row["Recoverable"].ToString()),
        //                Balance = decimal.Parse(row["Balance"].ToString()),
        //                PrinBalance = decimal.Parse(row["PrinBalance"].ToString()),
        //                SerBalance = decimal.Parse(row["SerBalance"].ToString()),
        //                InstallmentNo = row.Field<int>("installmentNo"),
        //                MemberID = int.Parse(row["MemberID"].ToString()),
        //                MemberCode = row["MemberCode"].ToString(),
        //                MemberName = row["MemberName"].ToString(),
        //                CenterID = int.Parse(row["CenterID"].ToString()),
        //                CenterCode = row["CenterCode"].ToString(),
        //                CenterName = row["CenterName"].ToString(),
        //                InstallmentDate = row["InstallmentDate"].ToString(),
        //                TrxType = int.Parse(row["TrxType"].ToString()),
        //                EmployeeName = empid.EmpName,
        //                SummaryID = long.Parse(row["SummaryID"].ToString()),
        //                CumIntCharge = decimal.Parse(row["CumIntCharge"].ToString()),
        //                CumInterestPaid = decimal.Parse(row["CumInterestPaid"].ToString()),
        //                Duration = int.Parse(row["Duration"].ToString()),
        //                DurationOverIntDue = decimal.Parse(row["DurationOverIntDue"].ToString()),
        //                DurationOverLoanDue = decimal.Parse(row["DurationOverLoanDue"].ToString()),
        //                IntDue = decimal.Parse(row["IntDue"].ToString()),
        //                InterestCalculationMethod = row["InterestCalculationMethod"].ToString(),
        //                LoanDue = decimal.Parse(row["LoanDue"].ToString()),
        //                LoanRepaid = decimal.Parse(row["LoanRepaid"].ToString()),
        //                PrincipalLoan = decimal.Parse(row["PrincipalLoan"].ToString()),
        //                IntCharge = decimal.Parse(row["IntCharge"].ToString()),
        //                NewDue = decimal.Parse(row["NewDue"].ToString()),
        //                MainProductCode = row["MainProductCode"].ToString(),
        //                Doc = int.Parse(row["doc"].ToString()),
        //                OrgID = int.Parse(row["OrgID"].ToString()),
        //                accountNo = row["accountNo"].ToString(),
        //                fine = decimal.Parse(row["fine"].ToString()),
        //                SCPaid = decimal.Parse(row["SCPaid"].ToString()),
        //                PersonalWithdraw = decimal.Parse(row["PersonalWithdraw"].ToString()),
        //                PersonalSaving = decimal.Parse(row["PersonalSaving"].ToString())
        //            }).ToList();

        //        LogToFile(string.Format("GetOfficeSyncData - Result Count: {0} ", lst.Count()));

        //        try
        //        {
        //            var json = new JavaScriptSerializer().Serialize(lst);
        //            LogToFileJson(json);
        //        }
        //        catch (Exception e)
        //        {

        //        }
        //        return lst;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogToFile(string.Format("Error Occured on GetOfficeSyncData. OfficeID: {1}, Error: {0}", ex.Message, officeID));
        //        throw ex;
        //    }
        //}

        //[System.Web.Mvc.Route("api/lookupapi/getofficesyncdatanewsavings")]
        //public List<MobileOfficeDataSyncModel> GetOfficeSyncDataNewSavings(int officeID, string loginID)
        //{

        //    LogToFile(string.Format("GetOfficeSyncData - OfficeID:{0}, LoginID: {1}", officeID, loginID));
        //    var empid = employeeService.GetByCode(loginID);
        //    if (empid == null)
        //        throw new Exception("Invalid Login ID");
        //    try
        //    {
        //        var param = new { OfficeId = officeID, EmployeeID = empid.EmployeeID };

        //        var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetNewSavings");

        //        var lst = alldata.Tables[0].AsEnumerable()
        //            .Select(row => new MobileOfficeDataSyncModel()
        //            {
        //                ProductID = int.Parse(row["ProductID"].ToString()),
        //                ProductName = row["ProductName"].ToString(),
        //                ProductType = int.Parse(row["ProductType"].ToString()),
        //                LoanRecovery = decimal.Parse(row["LoanRecovery"].ToString()),
        //                Recoverable = decimal.Parse(row["Recoverable"].ToString()),
        //                Balance = decimal.Parse(row["Balance"].ToString()),
        //                PrinBalance = decimal.Parse(row["PrinBalance"].ToString()),
        //                SerBalance = decimal.Parse(row["SerBalance"].ToString()),
        //                InstallmentNo = row.Field<int>("installmentNo"),
        //                MemberID = int.Parse(row["MemberID"].ToString()),
        //                MemberCode = row["MemberCode"].ToString(),
        //                MemberName = row["MemberName"].ToString(),
        //                CenterID = int.Parse(row["CenterID"].ToString()),
        //                CenterCode = row["CenterCode"].ToString(),
        //                CenterName = row["CenterName"].ToString(),
        //                InstallmentDate = row["InstallmentDate"].ToString(),
        //                TrxType = int.Parse(row["TrxType"].ToString()),
        //                EmployeeName = empid.EmpName,
        //                SummaryID = long.Parse(row["SummaryID"].ToString()),
        //                CumIntCharge = decimal.Parse(row["CumIntCharge"].ToString()),
        //                CumInterestPaid = decimal.Parse(row["CumInterestPaid"].ToString()),
        //                Duration = int.Parse(row["Duration"].ToString()),
        //                DurationOverIntDue = decimal.Parse(row["DurationOverIntDue"].ToString()),
        //                DurationOverLoanDue = decimal.Parse(row["DurationOverLoanDue"].ToString()),
        //                IntDue = decimal.Parse(row["IntDue"].ToString()),
        //                InterestCalculationMethod = row["InterestCalculationMethod"].ToString(),
        //                LoanDue = decimal.Parse(row["LoanDue"].ToString()),
        //                LoanRepaid = decimal.Parse(row["LoanRepaid"].ToString()),
        //                PrincipalLoan = decimal.Parse(row["PrincipalLoan"].ToString()),
        //                IntCharge = decimal.Parse(row["IntCharge"].ToString()),
        //                NewDue = decimal.Parse(row["NewDue"].ToString()),
        //                MainProductCode = row["MainProductCode"].ToString(),
        //                Doc = int.Parse(row["doc"].ToString())
        //            }).ToList();

        //        LogToFile(string.Format("GetOfficeSyncData - Result Count: {0} ", lst.Count()));

        //        try
        //        {
        //            var json = new JavaScriptSerializer().Serialize(lst);
        //            LogToFileJson(json);
        //        }
        //        catch (Exception e)
        //        {

        //        }
        //        return lst;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogToFile(string.Format("Error Occured on GetOfficeSyncData. OfficeID: {1}, Error: {0}", ex.Message, officeID));
        //        throw ex;
        //    }
        //}
        [System.Web.Mvc.Route("api/lookupapi/getinfoapi")]
        public int GetInfoAPI(int appVersion, string user)
        {
            var apiVersion = VersionHelper.GetMobileAPIVersion();
            LogToFile(string.Format("GetAPIVersion - API version check. Current Mobile APP version: {0}, WebAPI version: {1}, User: {2} ", appVersion, apiVersion, user));
            return VersionHelper.GetMobileAPIVersion();
        }
        //[System.Web.Mvc.Route("api/lookupapi/getinfoapi")]
        //public string GetInfoAPI(string appVersion, string user)
        //{
        //    var apiVersion = VersionHelper.GetMobileAPIVersion();
        //    LogToFile(string.Format("GetAPIVersion - API version check. Current Mobile APP version: {0}, WebAPI version: {1}, User: {2} ", appVersion, apiVersion, user));
        //    return VersionHelper.GetMobileAPIVersion();
        //}
        [System.Web.Mvc.Route("api/lookupapi/getproduct")]
        public List<SelectListItem> GetProduct()
        {
            return productService.GetAll().Where(w => w.IsActive.HasValue && w.IsActive.Value == true && w.ProductID == 3).Select(s => new SelectListItem() { Text = string.Format("{0} - {1}", s.ProductCode, s.ProductName), Value = s.ProductID.ToString() }).ToList();
        }
        [System.Web.Mvc.Route("api/lookupapi/GetPurpose")]
        public List<SelectListItem> GetPurpose()
        {
            return purposeService.GetAll().Where(w => w.IsActive.HasValue && w.IsActive.Value == true && w.PurposeID == 3).Select(s => new SelectListItem() { Text = s.PurposeName, Value = s.PurposeID.ToString() }).ToList();
        }
        [System.Web.Mvc.Route("api/lookupapi/GetProductsForMember")]
        public List<MemberLoanModel> GetProductsForMember(long memberID)
        {
            try
            {
                var param = new { Qtype = 1, MemberID = memberID, ProductID = 0 };
                // var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberProducts");
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberCollectionDetail");

                var lst = alldata.Tables[0].AsEnumerable()
                    .Select(row => new MemberLoanModel()
                    {
                        MemberID = memberID,
                        ProductID = int.Parse(row["ProductID"].ToString()),
                        ProductName = row["ProductName"].ToString(),
                        ProductType = int.Parse(row["ProductType"].ToString()),
                        LoanRecovery = decimal.Parse(row["LoanRecovery"].ToString()),
                        Recoverable = decimal.Parse(row["Recoverable"].ToString()),
                        Balance = decimal.Parse(row["Balance"].ToString()),
                        InstallmentNo = row.Field<int>("installmentNo")
                    }).ToList();
                LogToFile(string.Format("GetProductsForMember Result. Member: {1}, Count: {0}", lst.Count(), memberID));
                return lst;
            }
            catch (Exception ex)
            {
                LogToFile(string.Format("Error Occured on GetProductsForMember. Member: {1}, Error: {0}", ex.Message, memberID));
                throw ex;
            }
        }
        [System.Web.Mvc.Route("api/lookupapi/GetMemberListbyCenter")]
        public List<SelectListItem> GetMemberListbyCenter(int OfficeID, int CenterID)
        {
            // List<GetMemberListViewModel> lst = new List<GetMemberListViewModel>();
            LogToFile(string.Format("GetMemberListbyCenter - OfficeID:{0}, CenterID:{1} ", OfficeID, CenterID));
            var param = new { OfficeID = OfficeID, CenterID = CenterID };
            var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberListDropdown");

            var lst = alldata.Tables[0].AsEnumerable()
           .Select(row => new SelectListItem()
           {
               Value = row.Field<string>("MemberID"),
               //LoanTerm = row.Field<string>("LoanTerm"),
               Text = row.Field<string>("MemberName")

           }).ToList();
            LogToFile(string.Format("GetMemberListbyCenter - Result Count: {0} ", lst.Count));

            return lst;
        }
        [System.Web.Mvc.Route("api/lookupapi/GetProductBymember")]
        public List<SelectListItem> GetProductBymember(int OfficeId, int CenterID, long MemberId)
        {
            LogToFile(string.Format("GetProductBymember - OfficeID:{0}, CenterID:{1}, MemberID: {2}", OfficeId, CenterID, MemberId));

            try
            {
                var param = new { OfficeId = OfficeId, CenterID = CenterID, MemberId = MemberId };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_get_ProductByMember");

                var lst = alldata.Tables[0].AsEnumerable()
               .Select(row => new SelectListItem()
               {
                   Value = row["ProductID"].ToString(),
                   Text = row.Field<string>("ProductCode")

               }).ToList();
                LogToFile(string.Format("GetProductBymember - Result Count: {0} ", lst.Count));

                return lst;
            }
            catch (Exception ex)
            {
                LogToFile(ex.Message);
                throw ex;
            }
        }
        private void LogToFile(string msg)
        {
            var orgName = ConfigurationManager.AppSettings["MobileAPIOrgName"].ToString();
            var fileName = string.Format(@"C:\APILog\{0}", orgName);
            if (!Directory.Exists(fileName))
                Directory.CreateDirectory(fileName);
            fileName = string.Format(@"{0}\{1}.txt", fileName, DateTime.Now.ToString("dd_MM_yyyy")); ;
            using (StreamWriter sr = File.AppendText(fileName))
            {
                sr.WriteLine(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt - ") + msg);
            }
        }
        private void LogToFileJson(string msg)
        {
            var orgName = ConfigurationManager.AppSettings["MobileAPIOrgName"].ToString();
            var fileName = string.Format(@"C:\APILog\{0}", orgName);
            if (!Directory.Exists(fileName))
                Directory.CreateDirectory(fileName);
            fileName = string.Format(@"{0}\Sync_Json_{1}.txt", fileName, DateTime.Now.ToString("dd_MM_yyyy")); ;
            using (StreamWriter sr = File.AppendText(fileName))
            {
                sr.WriteLine(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt - ") + msg);
            }
        }
    }
    //public class MobileOfficeDataSyncModel
    //{
    //    public long MemberID { get; set; }
    //    public string MemberCode { get; set; }
    //    public int ProductID { get; set; }
    //    public string ProductName { get; set; }
    //    public int ProductType { get; set; }
    //    public decimal LoanRecovery { get; set; }
    //    public decimal Recoverable { get; set; }
    //    public decimal Balance { get; set; }
    //    public decimal PrinBalance { get; set; }
    //    public decimal SerBalance { get; set; }
    //    //PrinBalance,SerBalance
    //    public int InstallmentNo { get; set; }
    //    public int CenterID { get; set; }
    //    public string CenterCode { get; set; }
    //    public string CenterName { get; set; }
    //    public string MemberName { get; set; }
    //    public string InstallmentDate { get; set; }
    //    public string EmployeeName { get; set; }
    //    public int TrxType { get; set; }
    //    public long SummaryID { get; set; }
    //    public string InterestCalculationMethod { get; set; }
    //    public int Duration { get; set; }
    //    public decimal DurationOverLoanDue { get; set; }
    //    public decimal DurationOverIntDue { get; set; }
    //    public decimal LoanDue { get; set; }

    //    public decimal IntDue { get; set; }
    //    public decimal CumIntCharge { get; set; }
    //    public decimal CumInterestPaid { get; set; }

    //    public decimal PrincipalLoan { get; set; }
    //    public decimal LoanRepaid { get; set; }
    //    public decimal IntCharge { get; set; }
    //    // public string CollectionType { get; set; }
    //    public decimal NewDue { get; set; }
    //    public string MainProductCode { get; set; }
    //    public int Doc { get; set; }
    //    //public int OrgID { get; set; }
    //    //public string accountNo { get; set; }
    //    //public decimal fine { get; set; }

    //    //public decimal SCPaid { get; set; }
    //    //public decimal PersonalWithdraw { get; set; }

    //    //public decimal PersonalSaving { get; set; }
    //}

    public class MobileOfficeDataSyncModel
    {
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductType { get; set; }
        public decimal LoanRecovery { get; set; }
        public decimal Recoverable { get; set; }
        public decimal Balance { get; set; }
        public decimal PrinBalance { get; set; }
        public decimal SerBalance { get; set; }
        //PrinBalance,SerBalance
        public int InstallmentNo { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public string MemberName { get; set; }
        public string InstallmentDate { get; set; }
        public string EmployeeName { get; set; }
        public int TrxType { get; set; }
        public long SummaryID { get; set; }
        public string InterestCalculationMethod { get; set; }
        public int Duration { get; set; }
        public decimal DurationOverLoanDue { get; set; }
        public decimal DurationOverIntDue { get; set; }
        public decimal LoanDue { get; set; }

        public decimal IntDue { get; set; }
        public decimal CumIntCharge { get; set; }
        public decimal CumInterestPaid { get; set; }

        public decimal PrincipalLoan { get; set; }
        public decimal LoanRepaid { get; set; }
        public decimal IntCharge { get; set; }
        // public string CollectionType { get; set; }
        public decimal NewDue { get; set; }
        public string MainProductCode { get; set; }
        public int Doc { get; set; }
        public int OrgID { get; set; }
        public string accountNo { get; set; }
        public decimal fine { get; set; }

        public decimal SCPaid { get; set; }
        public decimal PersonalWithdraw { get; set; }

        public decimal PersonalSaving { get; set; }
    }
}

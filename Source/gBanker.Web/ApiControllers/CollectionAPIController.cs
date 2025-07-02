using gBanker.Service;
using gBanker.Service.ReportServies;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Mvc;
namespace gBanker.Web.ApiControllers
{
    public class CollectionAPIController : ApiController
    {
        // GET api/<controller>
        private readonly IUltimateReportService ultimateReportService;
        private readonly IOfficeService officeService;
        public CollectionAPIController(IOfficeService officeService, IUltimateReportService ultimateReportService)
        {
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;

        }

        [System.Web.Mvc.Route("api/CollectionAPI/DataDistributeAmount")]
        public string DataDistributeAmount(int OfficeID, int CenterID, string collectionDate)
        {
            var param = new { @CollectionDate = collectionDate, @OfficeID = OfficeID, @CenterID = CenterID };
            ultimateReportService.GetDataDistributeAmount(param);
            return "Success";
        }

    //    [System.Web.Mvc.Route("api/CollectionAPI/PostMobileCollection")]
    //    [System.Web.Mvc.HttpPost]
    //    public List<long> PostMobileCollection([FromBody] CollectionAPIModel apiRequest) //(string apiRequest) //[FromBody] CollectionAPIModel apiRequest
    //    {
    //        var msg = "";
    //        if (apiRequest == null)
    //            return new List<long> { -1000 };

    //        var collections = apiRequest.Collections;
    //        var user = apiRequest.UserId;
    //        var apiVersion = apiRequest.APIVersion;
    //        var collectionDate = apiRequest.CollectionDate;

    //        // CollectionAPIModel item = CollectionAPIModel.De<TripObject>(jsonResult.ToString());
    //        //string jsonStrings = apiRequest.ToString();

    //        var jsonStrings = new JavaScriptSerializer().Serialize(apiRequest);
    //        var successList = new List<long>();
    //        try
    //        {

    //            var param = new { JsonString = jsonStrings };
    //            var execute = ultimateReportService.InsertTabUploadData(param, "InsertIntoTabCollection");
    //            successList.Add(200);
    //        }
    //        catch (Exception ex)
    //        {
    //            successList.Add(-700);
    //            LogToFile(string.Format("PostMobileCollection:: " + ex.ToString()));
    //        }

    //        /*
    //        var successList = new List<long>();
    //        var offices = collections.Select(s => new { OfficeID = s.OfficeID, User = user }).Distinct();
    //        LogToFile(string.Format("Loan Collection Sync Request - Total Record: {0}", collections == null ? 0 : collections.Count));
    //        try
    //        {
    //            foreach (var office in offices)
    //            {
    //                var param = new { @OfficeId = office.OfficeID };
    //                var lastInitialDate = ultimateReportService.GetLastInitialDate(param);
    //                var dt = DateTime.MinValue;
    //                if (lastInitialDate == null || !DateTime.TryParse(lastInitialDate.ToString(), out dt))
    //                    successList.Add(-100);
    //                else
    //                {
    //                    if (DateTime.Parse(collectionDate) != dt)
    //                    {
    //                        //Mobile colleciton date and system date are not same.
    //                        LogToFile(string.Format("Mobile colleciton date and system date are not same. Mobile Date: {0}, Database: {1}", collectionDate, dt.ToString("yyyy/MM/dd")));
    //                        successList.Add(-101);
    //                    }
    //                }
    //                if (successList.Count > 0)
    //                    return successList;
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            successList.Add(-102);
    //            LogToFile(ex.Message);
    //            return successList;
    //        }
    //        if (collections != null && collections.Count > 0)
    //        {
    //            // var version = collections.First().ApiVersion;
    //            if (VersionHelper.GetMobileAPIVersion() > apiVersion)
    //            {
    //                var logtext = string.Format("Mobile APP version is older. Mobile APP Version: {0}, WebAPI Version: {1}", apiVersion, VersionHelper.GetMobileAPIVersion());
    //                LogToFile(logtext);
    //                return new List<long>() { -99 };
    //            }
    //            foreach (var coll in collections)
    //            {
    //                var logtext = string.Format("    MemberID:{0}, amount:{1}, office:{2}, Center: {3}, Product: {4}, User: {5}, guid: {6}, summaryId:{7}, TrxType: {8}, Prod Type: {9}, Loan:{10}, Int: {11}, IntCh: {12} ", coll.MemberID, coll.Amount, coll.OfficeID, coll.CenterID, coll.ProductID, user, coll.Token, coll.Sid, coll.TType, coll.PType, coll.LoanInstallment, coll.IntInstallment, coll.IntCharge);
    //                LogToFile(logtext);
    //                var collId = coll.CollectionID;
    //                try
    //                {
    //                    var paramAdd = new { @OfficeID = coll.OfficeID, @CenterID = coll.CenterID, @ProductID = coll.ProductID, MemberID = coll.MemberID, @LoanPaid = coll.Amount, @loggedInUser = user, SummaryID = coll.Sid, CollectionGUID = coll.Token, TransType = coll.TType, ProdType = coll.PType, @LoanInstallment = coll.LoanInstallment, @IntInstallment = coll.IntInstallment, @IntCharge = coll.IntCharge };
    //                    var ds = ultimateReportService.GetDataMemberCollectionAdd(paramAdd);

    //                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //                        msg = ds.Tables[0].Rows[0][0].ToString();

    //                    if (msg == "Success")
    //                        successList.Add(collId);
    //                    else
    //                        LogToFile("Failed for this record: " + msg);
    //                }
    //                catch (Exception ex)
    //                {
    //                    LogToFile("Error on Loan collection: " + ex.Message);
    //                }
    //            }
    //            ////================call data distribution only once for each officeid=====


    //            LogToFile(string.Format("Data distribution started"));
    //            foreach (var office in offices)
    //            {
    //                try
    //                {
    //                    var param = new { @OfficeID = office.OfficeID, @User = office.User };
    //                    ultimateReportService.GetDataDistributeAmount(param);
    //                    LogToFile(string.Format("Succeed: - Office: {0}, User: {1}", office.OfficeID, office.User));
    //                }
    //                catch (Exception ex)
    //                {
    //                    LogToFile(string.Format("Error on data distribution- Office: {0}, User: {1}. Error: {2}", office.OfficeID, office.User, ex.Message));
    //                }
    //            }
    //        }

    //*/

    //        return successList;
    //    }
        [System.Web.Mvc.Route("api/CollectionAPI/PostMobileCollection")]
        [System.Web.Mvc.HttpPost]
        public List<long> PostMobileCollection([FromBody] CollectionAPIModel apiRequest)
        {
            var msg = "";
            if (apiRequest == null)
                return new List<long> { -1000 };

            var collections = apiRequest.Collections;
            var user = apiRequest.UserId;
            var apiVersion = apiRequest.APIVersion;
            var collectionDate = apiRequest.CollectionDate;
            var successList = new List<long>();
            var offices = collections.Select(s => new { OfficeID = s.OfficeID, User = user }).Distinct();
            LogToFile(string.Format("Loan Collection Sync Request - Total Record: {0}", collections == null ? 0 : collections.Count));
            try
            {
                foreach (var office in offices)
                {
                    var param = new { @OfficeId = office.OfficeID };
                    var lastInitialDate = ultimateReportService.GetLastInitialDate(param);
                    var dt = DateTime.MinValue;
                    if (lastInitialDate == null || !DateTime.TryParse(lastInitialDate.ToString(), out dt))
                        successList.Add(-100);
                    else
                    {
                        if (DateTime.Parse(collectionDate) != dt)
                        {
                            //Mobile colleciton date and system date are not same.
                            LogToFile(string.Format("Mobile colleciton date and system date are not same. Mobile Date: {0}, Database: {1}", collectionDate, dt.ToString("yyyy/MM/dd")));
                            successList.Add(-101);
                        }
                    }
                    if (successList.Count > 0)
                        return successList;
                }

            }
            catch (Exception ex)
            {
                successList.Add(-102);
                LogToFile(ex.Message);
                return successList;
            }
            if (collections != null && collections.Count > 0)
            {
                // var version = collections.First().ApiVersion;
                if (VersionHelper.GetMobileAPIVersion() > apiVersion)
                {
                    var logtext = string.Format("Mobile APP version is older. Mobile APP Version: {0}, WebAPI Version: {1}", apiVersion, VersionHelper.GetMobileAPIVersion());
                    LogToFile(logtext);
                    return new List<long>() { -99 };
                }
                foreach (var coll in collections)
                {
                    var logtext = string.Format("    MemberID:{0}, amount:{1}, office:{2}, Center: {3}, Product: {4}, User: {5}, guid: {6}, summaryId:{7}, TrxType: {8}, Prod Type: {9}, Loan:{10}, Int: {11}, IntCh: {12} ", coll.MemberID, coll.Amount, coll.OfficeID, coll.CenterID, coll.ProductID, user, coll.Token, coll.Sid, coll.TType, coll.PType, coll.LoanInstallment, coll.IntInstallment, coll.IntCharge);
                    LogToFile(logtext);
                    var collId = coll.CollectionID;
                    try
                    {
                        var paramAdd = new { @OfficeID = coll.OfficeID, @CenterID = coll.CenterID, @ProductID = coll.ProductID, MemberID = coll.MemberID, @LoanPaid = coll.Amount, @loggedInUser = user, SummaryID = coll.Sid, CollectionGUID = coll.Token, TransType = coll.TType, ProdType = coll.PType, @LoanInstallment = coll.LoanInstallment, @IntInstallment = coll.IntInstallment, @IntCharge = coll.IntCharge };
                        var ds = ultimateReportService.GetDataMemberCollectionAdd(paramAdd);

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            msg = ds.Tables[0].Rows[0][0].ToString();

                        if (msg == "Success")
                            successList.Add(collId);
                        else
                            LogToFile("Failed for this record: " + msg);
                    }
                    catch (Exception ex)
                    {
                        LogToFile("Error on Loan collection: " + ex.Message);
                    }
                }
                ////================call data distribution only once for each officeid=====


                LogToFile(string.Format("Data distribution started"));
                foreach (var office in offices)
                {
                    try
                    {
                        var param = new { @OfficeID = office.OfficeID, @User = office.User };
                        ultimateReportService.GetDataDistributeAmount(param);
                        LogToFile(string.Format("Succeed: - Office: {0}, User: {1}", office.OfficeID, office.User));
                    }
                    catch (Exception ex)
                    {
                        LogToFile(string.Format("Error on data distribution- Office: {0}, User: {1}. Error: {2}", office.OfficeID, office.User, ex.Message));
                    }
                }
            }

            return successList;
        }

        [System.Web.Mvc.Route("api/CollectionAPI/PostErrorLog")]
        [System.Web.Mvc.HttpPost]
        public void PostErrorLog([FromBody] SystemErrorModel[] errors)
        {
            var successList = new List<long>();
            LogToFile(string.Format("Error Log Sync Request - Total Record: {0}", errors == null ? 0 : errors.Length), "ErrorLog");
            if (errors != null && errors.Length > 0)
            {
                foreach (var error in errors)
                {
                    LogToFile(string.Format("Action: {0}, Error:{1}, Input: {2}, User: {3}, CreateDate: {4}", error.ActionName, error.ErrorText, error.InputParameters, error.UserID, error.CreateDate), "ErrorLog");
                }
            }
        }

        [System.Web.Mvc.Route("api/CollectionAPI/GetCreateCollectionStatus")]
        public string GetCreateCollectionStatus(string memberCode, decimal amount, int OfficeID, int CenterID, int ProductID, string loggedInUser)
        {
            var logtext = string.Format("Loan Collection: Member:{0}, amount:{1}, office:{2}, Center: {3}, Product: {4}, User: {5}", memberCode, amount, OfficeID, CenterID, ProductID, loggedInUser);
            LogToFile(logtext);
            var msg = "";
            try
            {
                var paramAdd = new { @OfficeID = OfficeID, @CenterID = CenterID, @ProductID = ProductID, MemberID = memberCode, @LoanPaid = amount, @loggedInUser = loggedInUser };
                var ds = ultimateReportService.GetDataMemberCollectionAdd(paramAdd);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    msg = ds.Tables[0].Rows[0][0].ToString();
                }
                if (msg == "Success")
                {
                    var param = new { @CollectionDate = DateTime.Now, @OfficeID = OfficeID, @CenterID = CenterID };
                    ultimateReportService.GetDataDistributeAmount(param);
                }
            }
            catch (Exception ex)
            {
                LogToFile(ex.Message);
                return "Error";
            }
            return msg;
        }
        private void LogToFile(string msg, string filePrefix = "Collection")
        {
            var orgName = ConfigurationManager.AppSettings["MobileAPIOrgName"].ToString();
            var fileName = string.Format(@"C:\APILog\{0}", orgName);
            if (!Directory.Exists(fileName))
                Directory.CreateDirectory(fileName);
            fileName = string.Format(@"{0}\{2}_{1}.txt", fileName, DateTime.Now.ToString("dd_MM_yyyy"), filePrefix); ;
            using (StreamWriter sr = File.AppendText(fileName))
            {
                sr.WriteLine(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt - ") + msg);
            }
        }
        public class CollectionAPIModel
        {
            public string CollectionDate { get; set; }
            public string UserId { get; set; }
            public int APIVersion { get; set; }
            public List<LoanCollectionModel> Collections { get; set; }
        }
        public class LoanCollectionModel
        {
            public long MemberID { get; set; }
            public decimal Amount { get; set; }
            public int OfficeID { get; set; }
            public int CenterID { get; set; }
            public int ProductID { get; set; }
            //   public string loggedInUser { get; set; }
            public string Token { get; set; }
            public long CollectionID { get; set; }
            public long Sid { get; set; }
            public int TType { get; set; }
            public int PType { get; set; }
            public decimal IntCharge { get; set; }
            public decimal LoanInstallment { get; set; }
            public decimal IntInstallment { get; set; }
            public double fine { get; set; }
            // public int ApiVersion { get; set; }
        }
       
        public class SystemErrorModel
        {
            public int ID { get; set; }
            public string ActionName { get; set; }
            public string ErrorText { get; set; }
            public string InputParameters { get; set; }
            public string UserID { get; set; }
            public string CreateDate { get; set; }
        }
    }
}

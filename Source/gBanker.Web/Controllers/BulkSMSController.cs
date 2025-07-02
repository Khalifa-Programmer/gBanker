#region Usings

using gBanker.Core.Filters;
using gBanker.Core.Utility;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Service.SMSSenderServices.Models;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace gBanker.Web.Controllers
{
    public class BulkSMSController : BaseController
    {
        #region Private Variabls

        private readonly ICenterService centerService;
        private readonly ILoanCollectionService loanCollectionService;
        private readonly ILoanSummaryService loansummaryService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly ILoanCollectionReportService loanCollectionReportService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ISmsLogService smsLogService;
        private readonly ISmsConfigService smsConfigService;
        private readonly ISMSSenderService smsSenderService;
        private static DataSet SMSList;
        private static List<SMSViewModel> List_ViewModel;
        private static string DataSearchType;

        #endregion

        #region Variables        

        public BulkSMSController(ILoanCollectionService loanCollectionService,
            ICenterService centerService, IMemberService memberService,
            IOfficeService officeService, IProductService productService,
            ILoanCollectionReportService loanCollectionReportService,
            ISMSSenderService smsSenderService,
            IUltimateReportService ultimateReportService, ISmsLogService smsLogService,
            ISmsConfigService smsConfigService)
        {
            this.loanCollectionService = loanCollectionService;
            this.centerService = centerService;
            this.memberService = memberService;
            this.officeService = officeService;
            this.productService = productService;
            this.loanCollectionReportService = loanCollectionReportService;
            this.ultimateReportService = ultimateReportService;
            this.smsLogService = smsLogService;
            this.smsConfigService = smsConfigService;
            this.smsSenderService = smsSenderService;
        }
        
        #endregion

        #region Send SMS

        public ActionResult SendSMS()
        {
            var trnDate = SessionHelper.TransactionDate.ToString();
            var bv = SessionHelper.TransactionDate.ToString();
            string fromDate = "", todate = "";
            if ("1/1/0001 12:00:00 AM" == bv)
            {
                trnDate = DateTime.Now.ToString("dd-MMM-yyyy");
                fromDate = trnDate; todate = trnDate;
            }
            else
            {
                var filterDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");
                fromDate = filterDate; todate = filterDate;
            }

            var model = new SMSSendBulkViewModel
            {
                DateFromValue = fromDate,
                DateToValue = todate,
                OfficeList = new List<SelectListItem> { new SelectListItem { Text = "Nothing Seleted" } }
            };
            return View(model);
        }

        #endregion

        #region SMS TransactionWise
        public ActionResult SMSTransactionWise()
        {
            return View();
        }
        #endregion

        #region Execute Send SMS


        public SMSSendResponse ExecuteSendSMSNw(SMSViewModel model)
        {
            var sMSSendResponse = new SMSSendResponse();

            // Force TLS 1.2 or TLS 1.3
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            // Define variables for the dynamic parts of the URL
            string message = model.MessageDetails;

            string contactNumber = model.PhoneNo;

            try
            {
                if (!contactNumber.StartsWith("88"))
                {
                    contactNumber = "88" + contactNumber;
                }
            }
            catch (Exception ex)
            {
                sMSSendResponse.IsError = true;
                sMSSendResponse.Message = $"General Error: {ex.Message}";

            }

            // Construct the URL using string interpolation
            string url = $"{SMSSenderConstants_Rtcom.ApiBaseUrl}?acode={SMSSenderConstants_Rtcom.AccountCode}&api_key={SMSSenderConstants_Rtcom.ApiKey}&msg={message}&senderid={SMSSenderConstants_Rtcom.SenderId}&type={SMSSenderConstants_Rtcom.MessageType}&contacts={contactNumber}&transactionType={SMSSenderConstants_Rtcom.TransactionType}";

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            // Add headers if needed
            request.Headers.Add("Cookie", "ci_session=mb1ip1mam84hmf7le3dqv17cjas3dito");

            // Send the request
            try
            {
                var responseX = client.SendAsync(request).GetAwaiter().GetResult();
                responseX.EnsureSuccessStatusCode();

                sMSSendResponse.IsError = false;
                sMSSendResponse.Message = $"Success";
            }
            catch (HttpRequestException ex)
            {
                sMSSendResponse.IsError = true;
                sMSSendResponse.Message = $"General Error: {ex.Message}";

                //Console.WriteLine($"Request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                sMSSendResponse.IsError = true;
                sMSSendResponse.Message = $"General Error: {ex.Message}";
            }

            return sMSSendResponse;
        }


        public SMSSendResponse ExecuteSendSMS(SMSViewModel model)
        {
            var response = new SMSSendResponse();
            try
            {
                var request = new SMSSendRequest
                {
                    RecipientMobile = model.PhoneNo,
                    Message = model.MessageDetails,
                    RequestType = SMSSendRequestTypeConstants.SINGLE_SMS,
                    MessageType = SMSMessageTypeConstants.UNICODE,
                    Organization = SessionHelper.LoggedInOrganizationCode
                };

                //let's send sms
                response = smsSenderService.SendSMS(request);

                if (response.IsError)
                {
                    //Let's insert into SMS_SentLog for NOT SENT
                    var param1 = new { @GrettingsText = model.MessageDetails, @PhoneNo = model.PhoneNo, @SMSType = "Grettings", @OfficeId = model.OfficeId, @OrgId = (int)SessionHelper.LoginUserOrganizationID };
                    ultimateReportService.GetDataWithParameter(param1, "Insert_SMS_FAILEDLogSMSLOGDetail");

                    return response;
                }

                var param = new
                {
                    @OfficeID = model.OfficeId,
                    @OrgId = (int)SessionHelper.LoginUserOrganizationID,
                    @SummaryId = model.SummaryId,
                    @DateSent = DateTime.Now,
                    @SMSType = 1,
                    @SMSPrice = 0.0,
                    @RecordId = model.RecordId,
                    @SMSDETAIL = model.MessageDetails,
                    @Length = model.Length,
                    @SMSCount = model.SMSCount
                };

                //let's insert into [SMS_SentLog]
                ultimateReportService.GetSMSDataWithParameter(param, "Insert_SMS_SentLog");
                return response;
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.Message = "There was an error on sending SMS or check your internet connection!";
            }

            return response;
        }
        #endregion

        #region Get SMS ListNew
        public JsonResult GetSMSListNew(string OptionId, List<int> officeIds, string DateFromValue, 
            string DateToValue, string MemberId, string SearchType, string SearchKey, int jtStartIndex, 
            int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                var newOfficeIDs = String.Join("_", officeIds);

                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeID.ToString());
                DateTime UpdateDate = DateTime.Now;

                var trnDate = SessionHelper.TransactionDate.ToString();
                var bv = SessionHelper.TransactionDate.ToString();

                if ("1/1/0001 12:00:00 AM" == bv)
                {
                    trnDate = DateTime.Now.ToString("dd-MMM-yyyy");
                }

                string fromDate = DateFromValue, todate = DateToValue;
                if (string.IsNullOrWhiteSpace(DateFromValue) || string.IsNullOrWhiteSpace(DateToValue))
                {
                    var filterDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");
                    fromDate = filterDate; todate = filterDate;
                }

                if (OptionId == "NotSend")
                {
                    //get sms not sent listing from SMSLOGDetail
                    SMSList = GetSMSNotSentList(SearchKey, newOfficeIDs, fromDate, todate);
                }
                else if (OptionId == "All")
                {
                    //Get sms sent and not sent list from SMSLOGDetail
                    SMSList = GetSMSSentAndNotSentList(SearchKey, newOfficeIDs, fromDate, todate);
                }
                else if (OptionId == "Success")
                {
                    //get sms sent success listing from SMS_SentLog and SMSLOGDetail
                    SMSList = GetSMSSentSuccessList(SearchKey, newOfficeIDs, fromDate, todate);
                }
                else if (OptionId == "Greetings")
                {
                    SMSList = GETSMSGreetingList(SearchKey, newOfficeIDs, fromDate, todate);
                }

                List_ViewModel = SMSList.Tables[0].AsEnumerable()
                .Select(row => new SMSViewModel
                {
                    rowSl = row.Field<Int64>("rowSl"),
                    RecordId = row.Field<Int64>("recordId"),
                    MessageDetails = row.Field<string>("lblMessage"),
                    PhoneNo = row.Field<string>("PhoneNo"),
                    DisburseDate = row.Field<string>("DisburseDate"),
                    MemberCode = row.Field<string>("MemberCode"),
                    SummaryId = row.Field<Int64>("SummaryId"),
                    OfficeId = row.Field<int>("OfficeId"),
                    TrxDate = trnDate,
                    Length = row.Field<int>("Length"),
                    SMSCount = row.Field<int>("SMSCount")
                }).ToList();

                if (MemberId != null)
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function 

        

        #endregion

        #region Transaction Get SMSList

        public JsonResult TransactionGetSMSList(string MemberId, string SearchType, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                DataSearchType = SearchType;

                StringBuilder sb = new StringBuilder();

                if (MemberId != null) //"0"
                    sb.Append(" AND M.MemberId =" + MemberId);

                List_ViewModel = new List<SMSViewModel>();
                var param = new { AndCondition = sb.ToString() };

                SMSList = new DataSet();
                if (SearchType == "TransactionAlert")
                {
                    sb.Append(" And dlt.OfficeID = " + (int)SessionHelper.LoginUserOfficeID);
                    param = new { AndCondition = sb.ToString() };
                    SMSList = ultimateReportService.GetDataWithParameter(param, "SP_PR_Get_Loan_SMS_List");
                }
                else if (SearchType == "ALL")
                {
                    StringBuilder sb1 = new StringBuilder();

                    int MyTestOfficeId = 4;

                    sb1.Append(" And dlt.OfficeID = " + MyTestOfficeId); //LoginUserOfficeID
                    var param1 = new { AndCondition = sb1.ToString() };
                    SMSList = ultimateReportService.GetDataWithParameter(param1, "SP_PR_Get_Loan_SMS_List");

                    StringBuilder sb2 = new StringBuilder();

                    if (TransactionDate != default(DateTime))
                    {
                        sb2.Append(" AND ls.DisburseDate ='" + TransactionDate.ToString("dd MMM, yyyy") + "'");
                    }



                    sb2.Append(" And ls.OfficeID = " + LoginUserOfficeID);
                    var param2 = new { AndCondition = sb2.ToString() };

                    var SMSList2 = ultimateReportService.GetDataWithParameter(param2, "SP_PR_Get_Disburse_SMS_List");

                    SMSList.Merge(SMSList2);
                }
                else
                {       // Disburse
                    //string TESTdisburseDate = "20 Oct, 2019";
                    if (TransactionDate != default(DateTime))
                    {
                        sb.Append(" AND ls.DisburseDate ='" + TransactionDate.ToString("dd MMM, yyyy") + "'");
                        //sb.Append(" AND ls.DisburseDate ='" + TESTdisburseDate + "'");
                    }

                    sb.Append(" And ls.OfficeID = " + LoginUserOfficeID);
                    param = new { AndCondition = sb.ToString() };
                    //SMSList = ultimateReportService.GetDataWithParameter(param, "SP_PR_Get_SMS_List");
                    SMSList = ultimateReportService.GetDataWithParameter(param, "SP_PR_Get_Disburse_SMS_List");
                }

                List_ViewModel = SMSList.Tables[0].AsEnumerable()
                .Select(row => new SMSViewModel
                {
                    rowSl = row.Field<Int64>("rowSl"),
                    RecordId = row.Field<Int64>("recordId"),
                    MessageDetails = row.Field<string>("lblMessage"),
                    PhoneNo = row.Field<string>("PhoneNo"),
                    DisburseDate = row.Field<string>("DisburseDate"),
                    MemberCode = row.Field<string>("MemberCode"),
                    Length = row.Field<int>("Length"),
                    SMSCount = row.Field<int>("SMSCount")

                }).ToList();

                if (MemberId != null)
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function 
        
        #endregion

        #region SMS Event Execute

        [HttpPost]
        public async Task<JsonResult> SMSEventExecute(SMSSendBulkViewModel model)
        {
            string result = "Message Sent Successfully";
            try
            {
                var trnDate = SessionHelper.TransactionDate.ToString();

                if (trnDate == "1/1/0001 12:00:00 AM")
                    trnDate = DateTime.Now.ToString("dd-MMM-yyyy");

                //populate sms not sent listing
                var smsNotSentList = PopulateSMSNotSentList(model, trnDate);

                if(!smsNotSentList.Any())
                    return Json("SMS Recipient not found. Please try another search criteria", JsonRequestBehavior.AllowGet);

                var smsNotSentTo = "";
                int count = 1;

                foreach (var itemSMS in smsNotSentList)
                {
                    if (string.IsNullOrWhiteSpace(itemSMS.PhoneNo)) continue;

                    /*
                    //::For Testing
                    if (count == 2)
                        break;

                    count= count+1;
                    itemSMS.PhoneNo = "01718055626";
                    */

                    //execute sms
                    //var response = ExecuteSendSMS(itemSMS);

                    var response = ExecuteSendSMSNw(itemSMS); //use this.

                    if (response.IsError) smsNotSentTo += $"{itemSMS.PhoneNo},";

                    if (!response.IsError)
                    {
                        var param = new { @RecordId = itemSMS.RecordId };

                        //update SMSLOGDetail to SET SendSMS = 0 and SentToAPI=1 SentOK=1
                        ultimateReportService.GetSMSDataWithParameter(param, "UpdateSMS");


                        var param2 = new
                        {
                            @OfficeID = itemSMS.OfficeId,
                            @OrgId = (int)SessionHelper.LoginUserOrganizationID,
                            @SummaryId = itemSMS.SummaryId,
                            @DateSent = DateTime.Now,
                            @SMSType = 1,
                            @SMSPrice = 0.0,
                            @RecordId = itemSMS.RecordId,
                            @SMSDETAIL = itemSMS.MessageDetails,
                            @Length = itemSMS.Length,
                            @SMSCount = itemSMS.SMSCount
                        };

                        //let's insert into [SMS_SentLog]
                        ultimateReportService.GetSMSDataWithParameter(param2, "Insert_SMS_SentLog");

                    }
                }

                var bulkResponseMessage = string.IsNullOrWhiteSpace(smsNotSentTo)
                    ? $@"{result}" : $@"SMS Send Partially Successful, But Not Sent Mobile Number(s) : {smsNotSentTo.TrimEnd(',')}";

                return Json(bulkResponseMessage, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                result = "Message Not Sent. Error Occured.";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

       

        #endregion

        #region SMS Event Generate
        public JsonResult SMSEventGenerate(int SMSType)
        {
            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeID.ToString());
                DateTime UpdateDate = DateTime.Now;

                var trnDate = SessionHelper.TransactionDate.ToString();

                var bv = SessionHelper.TransactionDate.ToString();
                if ("1/1/0001 12:00:00 AM" == bv)
                {
                    trnDate = DateTime.Now.ToString();
                }
                var param = new
                {
                    @OfficeID = SessionHelper.LoginUserOfficeID,
                    @SmsDate = trnDate
                };
                var v = ultimateReportService.GetSMSDataWithParameter(param, "genereateSMSList");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Private Methods

        private DataSet GetSMSNotSentList(string SearchKey, string newOfficeIDs, string fromDate, string todate)
        {
            var param = new
            {
                @OfficeID = newOfficeIDs,
                @SearchKey= SearchKey,               
                @FromDate = fromDate,
                @ToDate = todate,
            };

            SMSList = ultimateReportService.GetSMSDataWithParameter(param, "[dbo].[SMSLOGDetail_GetSMSNotSentList]");
            return SMSList;
        }

        private DataSet GetSMSSentSuccessList(string SearchKey, string newOfficeIDs, string fromDate, string todate)
        {
            var param = new
            {
                OfficeID = newOfficeIDs,
                SearchKey = SearchKey,
                StartDate = fromDate,
                EndDate = todate
            };

            SMSList = ultimateReportService.GetSMSDataWithParameter(param, "[dbo].[SMSLOGDetail_GetSMSSentSuccessList]");

            return SMSList;
        }

        private DataSet GetSMSSentAndNotSentList(string SearchKey, string newOfficeIDs, string fromDate, string todate)
        {
            var param = new
            {
                OfficeID = newOfficeIDs,
                SearchKey = SearchKey,
                FromDate = fromDate,
                ToDate = todate
            };

            SMSList = ultimateReportService.GetSMSDataWithParameter(param, "[dbo].[SMSLOGDetail_GetSMSSentAndNotSentList]");

            return SMSList;
        }

        private DataSet GETSMSGreetingList(string SearchKey, string newOfficeIDs, string fromDate, string todate)
        {
            var param = new
            {
                OfficeID = newOfficeIDs,
                SearchKey = SearchKey,
                FromDate = fromDate,
                ToDate = todate
            };

            SMSList = ultimateReportService.GetSMSDataWithParameter(param, "[dbo].[SMSSentLog_GETSMSGreetingList]");

            return SMSList;
        }


        private List<SMSViewModel> PopulateSMSNotSentList(SMSSendBulkViewModel model, string trnDate)
        {
            var newOfficeIDs = String.Join("_", model.OfficeIds);
            var newSMSList = GetSMSNotSentList(model.SearchKey, newOfficeIDs, model.DateFromValue, model.DateToValue);

            var smsNotSentList = newSMSList.Tables[0].AsEnumerable()
            .Select(row => new SMSViewModel
            {
                rowSl = row.Field<Int64>("rowSl"),
                RecordId = row.Field<Int64>("recordId"),
                MessageDetails = row.Field<string>("lblMessage"),
                PhoneNo = row.Field<string>("PhoneNo"),
                DisburseDate = row.Field<string>("DisburseDate"),
                MemberCode = row.Field<string>("MemberCode"),
                SummaryId = row.Field<Int64>("SummaryId"),
                OfficeId = row.Field<int>("OfficeId"),
                TrxDate = trnDate,
                Length = row.Field<int>("Length"),
                SMSCount = row.Field<int>("SMSCount")
            }).ToList();
            return smsNotSentList;
        }

        #endregion
    }
}
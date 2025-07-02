using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using Twilio;

namespace gBanker.Web.Controllers
{
    public class LoanDisburseSmsController : BaseController
    {
        #region Variables
        private readonly IBranchService branchService;
        private readonly ILoanSummaryService loansSummaryService;
        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;
        private readonly ICenterService centerService;
        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        private readonly ILoanDisburseService disburseservice;
        private readonly IPNMOrderService pnmOrderService;
        private readonly ISmsConfigService smsConfigService;
        private readonly ISmsLogService smsLogService;
        public LoanDisburseSmsController(ILoanDisburseService disburseservice, ILoanSummaryService loansSummaryService, IProductService productService, IMemberCategoryService membercategoryService, IBranchService branchService, ICenterService centerService, IPurposeService purposeService, IMemberService memberService, IPNMOrderService pnmOrderService, ISmsConfigService smsConfigService, ISmsLogService smsLogService)
        {
            this.loansSummaryService = loansSummaryService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.branchService = branchService;
            this.centerService = centerService;
            this.purposeService = purposeService;
            this.memberService = memberService;
            this.disburseservice = disburseservice;
            this.pnmOrderService = pnmOrderService;
            this.smsConfigService = smsConfigService;
            this.smsLogService = smsLogService;
        }
        #endregion
          
        #region Methods
        [HttpPost]
        public ActionResult GetLoanDisburseInfo(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                if (IsDayInitiated)
                {
                    var disburseDetail = disburseservice.GetLoanDisburseSms(LoggedInOrganizationID,LoginUserOfficeID, TransactionDate, filterColumn, filterValue);
                    var detail = disburseDetail.ToList();
                    var totalCount = detail.Count();
                    var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                    var currentPageRecords = Mapper.Map<IEnumerable<Proc_get_LoanDisburse_Result>, IEnumerable<LoanDisburseViewModel>>(entities);

                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
                }
                return Json(new { Result = "OK", Records = new List<LoanDisburseViewModel>(), TotalRecordCount = 0 });
                //var viewploansummary = Mapper.Map<IEnumerable<Proc_get_LoanDisburse_Result>, IEnumerable<LoanDisburseViewModel>>(detail);
                
                //return Json(new { Result = "OK", Records = viewploansummary });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        /*
        public Member GetMember(Int64 memberid)
        {
            var mbr = memberService.GetByMemberId(memberid);
            return mbr;
        }
        public Center GetEmployee(int employeeid)
        {
            var mbr = centerService.GetById(employeeid);
            return mbr;
        }
        public static double GetCurrentMilli()
        {
            DateTime Jan1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan javaSpan = DateTime.UtcNow - Jan1970;
            return javaSpan.TotalMilliseconds;
        }
        public T DeserializeToObject<T>(string objectData) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result as T;
        }
        public ActionResult SendSMS(string msgBody, string receiver, string memId, string disburseDt)
        {
            var result = "";
            try
            {
                var sms_config = smsConfigService.GetByOrgID(1);
                //string AccSid = "AC4b512e190c24c98270bdf2bc6d5c53b6";
                //string AuthToken = "750eca8f7ebdf6cc2713c490ab7d08b6";
                //string PhoneNo = "+16467592441";
                int msgLength = msgBody.Length;
                string AccSid = smsConfigService.Decrypt(sms_config.AccSID);
                string AuthToken = smsConfigService.Decrypt(sms_config.AuthToken);
                string PhoneNo = smsConfigService.Decrypt(sms_config.PhoneNo);
                var twilio = new TwilioRestClient(AccSid, AuthToken);

                ////var message = twilio.SendMessage("+16467592441", "[To]", null, null, null);
                var messsage = twilio.SendSmsMessage(PhoneNo, receiver, msgBody);                                            

                if (messsage.Sid != null)
                {                    
                    SmsLogViewModel log = new SmsLogViewModel();
                    log.OrgID = 1;
                    log.MemberID = Convert.ToInt64(memId);
                    log.SmsType = "D";
                    log.SmsFrom = PhoneNo;
                    log.SmsTo = receiver;
                    log.SmsBody = msgBody;
                    log.DateSent = Convert.ToDateTime(disburseDt);
                    log.SmsStatus = "Sent";
                    log.IsActive = true;
                    var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
                    smsLogService.Create(entity);
                    result = "1";
                }
                else
                {
                    SmsLogViewModel log = new SmsLogViewModel();
                    log.OrgID = 1;
                    log.MemberID = Convert.ToInt64(memId);
                    log.SmsType = "D";
                    log.SmsFrom = PhoneNo;
                    log.SmsTo = receiver;
                    log.SmsBody = msgBody;
                    log.DateSent = Convert.ToDateTime(disburseDt);
                    log.SmsStatus = "Fail";
                    log.IsActive = true;
                    var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
                    smsLogService.Create(entity);
                }

                //result = messsage.Sid.ToString();
                
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        */
        #endregion

        #region Events
        // GET: LoanDisburse
        public ActionResult Index()
        {
            //ViewData["SmsOption"] = ApplicationSettings.SmsOption;            
            return View();
        }
        // GET: LoanDisburse/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoanDisburse/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoanDisburse/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: LoanDisburse/Edit/5
        public ActionResult Edit(int id, LoanDisburseViewModel Model)
        {
           
            return View();
        }

        
        // POST: LoanDisburse/Edit/5
        [HttpPost]
        public ActionResult Edit(LoanDisburseViewModel Model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                var loanser = loansSummaryService.GetByIdLong(Convert.ToInt64(Model.LoanSummaryID));
                if (ModelState.IsValid)
                {
                    string msg = "";
                    if (disburseservice.IsValidLoan(loanser, out msg))
                    {
                       // loansSummaryService.updateDisburseCharge(loanser.LoanSummaryID,loanser.OfficeID,loanser.CenterID,loanser.MemberID,loanser.ProductID,loanser.LoanTerm,loanser.PrincipalLoan,Convert.ToDateTime(loanser.InstallmentStartDate),Convert.ToDateTime( loanser.DisburseDate));
                        loanser.IsActive = true;
                        loanser.InstallmentStartDate = Convert.ToDateTime(Model.InstallmentStartDate);
                        loanser.LoanStatus = 1;
                        loanser.DisburseDate = TransactionDate;
                        loanser.InstallmentDate = TransactionDate;
                        var proid = productService.GetById(loanser.ProductID);
                        if (proid != null)
                        {
                            if (proid.InterestCalculationMethod == "F")
                            {
                                loanser.IntCharge = Convert.ToDecimal((loanser.PrincipalLoan * proid.InterestRate) / 100);
                            }
                            else
                            {
                                loanser.IntCharge = 0;
                            }
                        }
                        loansSummaryService.Update(loanser);
                        //
                        #region PayNearMe
                        //if (ApplicationSettings.SmsOption == "true")
                        //{
                        //    var pnm = pnmOrderService.GetByLoanSummaryId(loanser.LoanSummaryID);
                        //    if (pnm == null)
                        //    {
                        //        using (var api = new PnmApiClient("https://sandbox-pro.paynearme.com/", "cf87f3b1fab61f81"))
                        //        {
                        //            api.setParam("order_currency", "USD");
                        //            api.setParam("order_type", "exact");
                        //            api.setParam("site_identifier", "S9230481873");
                        //            api.setParam("site_customer_identifier", "GM");
                        //            api.setParam("order_amount", loanser.PrincipalLoan.ToString());
                        //            api.setParam("version", "2.0");
                        //            api.setParam("timestamp", (GetCurrentMilli() / 1000).ToString());
                        //            var response = api.Execute("api/create_order");
                        //            if (response.IsSuccessStatusCode)
                        //            {
                        //                var content = response.Content.ReadAsStringAsync().Result;
                        //                var resultObj = DeserializeToObject<OrderCreate>(content);
                        //                var pnmOrder = new PNMOrder();
                        //                pnmOrder.loan_disburse_id = Convert.ToInt64(loanser.LoanSummaryID);
                        //                pnmOrder.pnm_customer_address = resultObj.order.customer.pnm_customer_addressee;
                        //                pnmOrder.pnm_customer_identifier = resultObj.order.customer.pnm_customer_identifier;
                        //                pnmOrder.pnm_customer_postal_code = resultObj.order.customer.pnm_customer_postal_code;
                        //                pnmOrder.site_customer_identifier = resultObj.order.customer.site_customer_identifier;
                        //                pnmOrder.latitude = resultObj.order.locations.latitude.ToString();
                        //                pnmOrder.longitude = resultObj.order.locations.longitude.ToString();
                        //                pnmOrder.minimum_payment_amount = resultObj.order.minimum_payment_amount;
                        //                pnmOrder.minimum_payment_currency = resultObj.order.minimum_payment_currency;
                        //                pnmOrder.order_amount = resultObj.order.order_amount;
                        //                pnmOrder.order_created = Convert.ToDateTime(resultObj.order.order_created);
                        //                pnmOrder.order_currency = resultObj.order.order_currency;
                        //                pnmOrder.order_is_standing = Convert.ToBoolean(resultObj.order.order_is_standing);
                        //                pnmOrder.order_status = resultObj.order.order_status;
                        //                pnmOrder.order_tracking_url = resultObj.order.order_tracking_url;
                        //                pnmOrder.order_type = resultObj.order.order_type;
                        //                pnmOrder.pnm_balance_due_amount = resultObj.order.pnm_balance_due_amount;
                        //                pnmOrder.pnm_balance_due_currency = resultObj.order.pnm_balance_due_currency;
                        //                pnmOrder.pnm_customer_language = resultObj.order.pnm_customer_language;
                        //                pnmOrder.pnm_order_crid = resultObj.order.pnm_order_crid;
                        //                pnmOrder.pnm_order_identifier = resultObj.order.pnm_order_identifier.ToString();
                        //                pnmOrder.pnm_order_short_identifier = resultObj.order.pnm_order_short_identifier;
                        //                pnmOrder.require_auth_tracker = resultObj.order.require_auth_tracker;
                        //                pnmOrder.retailer_name = resultObj.order.retailers.retailer.retailer_name;
                        //                pnmOrder.slip_id = resultObj.order.retailers.retailer.slip.slip_id.ToString();
                        //                pnmOrder.site_identifier = resultObj.order.site_identifier;
                        //                pnmOrder.site_name = resultObj.order.site_name;
                        //                pnmOrder.site_order_key = resultObj.order.site_order_key;
                        //                pnmOrder.is_active = true;
                        //                pnmOrder.create_user = LoggedInEmployeeID;
                        //                pnmOrder.create_date = DateTime.Now;
                        //                pnmOrderService.Create(pnmOrder);
                        //            }
                        //            else
                        //            {


                        //            }
                        //        }
                        //    }
                        //}
                        #endregion
                        //
                       // loansSummaryService.Proc_Set_RepaymentSchedule(loanser.LoanSummaryID, loanser.OfficeID, loanser.MemberID, loanser.ProductID, loanser.CenterID, loanser.MemberCategoryID, loanser.LoanTerm, loanser.Duration, loanser.InstallmentStartDate, loanser.CreateUser, loanser.CreateDate);
                        return Json(new { Result = "OK" });
                        //return View(emtpy);
                    }
                    else
                        return Json(new { Result = "ERROR", Message = msg });
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
          
        }
        [HttpPost]
        public JsonResult DeleteDisburse(long LoanSummaryID)
        {
            try
            {
                var loanser = loansSummaryService.GetByIdLong(Convert.ToInt64(LoanSummaryID));
                loanser.InstallmentStartDate = null;
                loanser.LoanStatus = 1;
                loanser.DisburseDate = null;
                loansSummaryService.Update(loanser);
                //disburseservice.Delete(LoanSummaryID);
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion
    }
}

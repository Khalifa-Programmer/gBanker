using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Web.Core.Extensions;
using gBanker.Web.Helpers;
using gBanker.Service.ReportServies;
using System.Data;
using gBanker.Data.CodeFirstMigration;
using Twilio;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.IO.Ports;
using System.Configuration;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace gBanker.Web.Controllers
{
    public class LoanDisburseController : BaseController
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
        private readonly ILoanCollectionReportService loanCollectionReportService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IAccChartService accChartService;
        private readonly ISpecialLoanCollectionService specialLoanCollectionService;
        private readonly IApplicationSettingsService applicationSettingsService;
        private readonly IWeeklyReportService weeklyReportService;
        public LoanDisburseController(ILoanDisburseService disburseservice, IAccChartService accChartService, ILoanSummaryService loansSummaryService, IProductService productService, IMemberCategoryService membercategoryService, IBranchService branchService, ICenterService centerService, IPurposeService purposeService, IMemberService memberService, IPNMOrderService pnmOrderService, ISmsConfigService smsConfigService, ISmsLogService smsLogService, ILoanCollectionReportService loanCollectionReportService, IUltimateReportService ultimateReportService, ISpecialLoanCollectionService specialLoanCollectionService, IWeeklyReportService weeklyReportService, IApplicationSettingsService applicationSettingsService)
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
            this.loanCollectionReportService = loanCollectionReportService;
            this.ultimateReportService = ultimateReportService;
            this.accChartService = accChartService;
            this.specialLoanCollectionService = specialLoanCollectionService;
            this.weeklyReportService = weeklyReportService;
            this.applicationSettingsService = applicationSettingsService;
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
                    var disburseDetail = disburseservice.GetLoanDisburse(LoggedInOrganizationID,LoginUserOfficeID, TransactionDate, filterColumn, filterValue);
                    var detail = disburseDetail.ToList();
                    var totalCount = detail.Count();
                    var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                    var currentPageRecords = Mapper.Map<IEnumerable<Proc_get_LoanDisburse_Result>, IEnumerable<LoanDisburseViewModel>>(entities);

                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
                }
                return Json(new { Result = "OK", Records = new List<LoanDisburseViewModel>(), TotalRecordCount = 0 });
               
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public ActionResult GetFirstLoanDisburseInfo(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                if (IsDayInitiated)
                {
                    var disburseDetail = disburseservice.GetFirstLoanDisburse(LoggedInOrganizationID, LoginUserOfficeID, TransactionDate, filterColumn, filterValue);
                    var detail = disburseDetail.ToList();
                    var totalCount = detail.Count();
                    var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                    var currentPageRecords = Mapper.Map<IEnumerable<Proc_get_LoanDisburse_Result>, IEnumerable<LoanDisburseViewModel>>(entities);

                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
                }
                return Json(new { Result = "OK", Records = new List<LoanDisburseViewModel>(), TotalRecordCount = 0 });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        //public ActionResult SendSMS(string msgBody, string receiver, string memId, string collDt)
        //{
        //    var result = "";
        //    try
        //    {
        //        clsSMS objclsSMS = new clsSMS();
        //        ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
        //        var portname = ConfigurationManager.AppSettings["PortNumber"];
        //        using (SerialPort port = objclsSMS.OpenPort(portname, 9600, 8, 300, 300))
        //        {

        //            if (port != null)
        //            {
        //                if (objclsSMS.sendMsg(port, receiver, msgBody))
        //                {
        //                    SmsLogViewModel log = new SmsLogViewModel();
        //                    log.OrgID = 1;
        //                    log.MemberID = Convert.ToInt64(memId);
        //                    log.SmsType = "D";
        //                    log.SmsFrom = "Server";
        //                    log.SmsTo = receiver;
        //                    log.SmsBody = msgBody;
        //                    log.DateSent = Convert.ToDateTime(collDt);
        //                    log.SmsStatus = "Sent";
        //                    log.IsActive = true;
        //                    var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
        //                    smsLogService.Create(entity);
        //                    result = "1";
        //                }
        //                else
        //                {
        //                    SmsLogViewModel log = new SmsLogViewModel();
        //                    log.OrgID = 1;
        //                    log.MemberID = Convert.ToInt64(memId);
        //                    log.SmsType = "D";
        //                    log.SmsFrom = "Server";
        //                    log.SmsTo = receiver;
        //                    log.SmsBody = msgBody;
        //                    log.DateSent = Convert.ToDateTime(collDt);
        //                    log.SmsStatus = "Fail";
        //                    log.IsActive = true;
        //                    var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
        //                    smsLogService.Create(entity);
        //                }
        //            }

        //            return Json(result, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public ActionResult SendSMS(string msgBody, string receiver, string memId, string disburseDt)
        {
            var result = "";
            try
            {
                //HttpClient httpClient = new HttpClient();
                //httpClient.BaseAddress = new Uri(Configuration.GetConnectionString("BaseUrl"));
                //httpClient.DefaultRequestHeaders.Clear();
                
                clsSMS objclsSMS = new clsSMS();
                ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
                var portname = ConfigurationManager.AppSettings["PortNumber"];
                HttpClient Client = new HttpClient();
                Client.BaseAddress = new Uri("http://192.192.192.233:8091/"); //api/nonmaskingsms/easysend?sender=01713140127&message=hello%20bangladesh)
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Client.DefaultRequestHeaders.Add("Accept", "application/json");
                //Client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                HttpResponseMessage message = Client.GetAsync($"api/nonmaskingsms/easysend?sender={receiver}&message={msgBody}").Result;
                if (message.IsSuccessStatusCode)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(0, JsonRequestBehavior.AllowGet);
                
                //using (SerialPort port = objclsSMS.OpenPort(portname, 9600, 8, 300, 300))
                //{

                //    if (port != null)
                //    {
                //        if (objclsSMS.sendMsg(port, receiver, msgBody))
                //        {
                //            SmsLogViewModel log = new SmsLogViewModel();
                //            log.OrgID = 1;
                //            log.MemberID = Convert.ToInt64(memId);
                //            log.SmsType = "D";
                //            log.SmsFrom = "Server";
                //            log.SmsTo = receiver;
                //            log.SmsBody = msgBody;
                //            log.DateSent = Convert.ToDateTime(disburseDt);
                //            log.SmsStatus = "Sent";
                //            log.IsActive = true;
                //            var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
                //            smsLogService.Create(entity);
                //            result = "1";
                //        }
                //        else
                //        {
                //            SmsLogViewModel log = new SmsLogViewModel();
                //            log.OrgID = 1;
                //            log.MemberID = Convert.ToInt64(memId);
                //            log.SmsType = "D";
                //            log.SmsFrom = "Server";
                //            log.SmsTo = receiver;
                //            log.SmsBody = msgBody;
                //            log.DateSent = Convert.ToDateTime(disburseDt);
                //            log.SmsStatus = "Fail";
                //            log.IsActive = true;
                //            var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
                //            smsLogService.Create(entity);
                //        }
                //    }

                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception ex)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
           
        }
        private void MapDropDownList(LoanDisburseViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }


            List<AccChartViewModel> List_ProductViewModel = new List<AccChartViewModel>();
            var param = new { SecondLevel= "1100", OfficeID = LoginUserOfficeID };
            var div_items = ultimateReportService.GetAccCodetListAccordingToOffice(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new AccChartViewModel
            {
                AccID = row.Field<Int16>("AccID"),
                AccCode = row.Field<string>("AccCode"),
                AccName = row.Field<string>("AccName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.AccCode.ToString(),
                Text = x.AccCode.ToString() + " " + x.AccName.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            model.GetAccountCodeList = d_items;
        }
        public JsonResult GetAccCode()
        {

            try
            {
                List<AccChartViewModel> List_ProductViewModel = new List<AccChartViewModel>();

                var param = new { SecondLevel = "1100", OfficeID = LoginUserOfficeID };
                var div_items = ultimateReportService.GetAccCodetListAccordingToOffice(param);

                List_ProductViewModel = div_items.Tables[0].AsEnumerable()
                .Select(row => new AccChartViewModel
                {
                    AccID = row.Field<int>("AccID"),
                    AccCode = row.Field<string>("AccCode"),
                    AccName = row.Field<string>("AccName")
                }).ToList();

                var accLIst = List_ProductViewModel.Select(c => new { DisplayText = c.AccCode + " " + c.AccName, Value = c.AccCode }).OrderBy(s => s.DisplayText);
                return Json(new { Result = "OK", Options = accLIst });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateRepaymentReport(string MemberID, string ProductID)
        {
            var param = new { OfficeId = LoginUserOfficeID, MemberID = MemberID, ProductID = ProductID };
            var allproducts = loanCollectionReportService.GetRepaymentInfo(param);
            var reportParam = new Dictionary<string, object>();
            reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
            if (allproducts.Tables[0].Rows.Count>0)
            {
                ReportHelper.PrintReport("RepaymentSchedule.rpt", allproducts.Tables[0], reportParam);
            }
            else
                return GetErrorMessageResult("Record not found");
           
            return Content(string.Empty);
        }
        public Member GetMember(long memberid)
        {
            var mbr = memberService.GetByMemberId(memberid);
            return mbr;
        }
        public Center GetEmployee(int employeeid)
        {
            var mbr = centerService.GetById(employeeid);
            return mbr;
        }
        /*
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
         public static double GetCurrentMilli()
        {
            DateTime Jan1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan javaSpan = DateTime.UtcNow - Jan1970;
            return javaSpan.TotalMilliseconds;
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

        public ActionResult FirstInstallmentStartDate()
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

                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                var loanser = loansSummaryService.GetByIdLong(Convert.ToInt64(Model.LoanSummaryID));
                if (ModelState.IsValid)
                {
                    string msg = "";
                    if (disburseservice.IsValidLoan(loanser, out msg))
                    {
                        var proid = productService.GetById(loanser.ProductID);
                       // loansSummaryService.updateDisburseCharge(loanser.LoanSummaryID,loanser.OfficeID,loanser.CenterID,loanser.MemberID,loanser.ProductID,loanser.LoanTerm,loanser.PrincipalLoan,Convert.ToDateTime(loanser.InstallmentStartDate),Convert.ToDateTime( loanser.DisburseDate));
                        loanser.IsActive = true;

                        //string sMonth = DateTime.Parse(Convert.ToString(loanser.InstallmentStartDate)).Month.ToString();
                        //int vMonth = 0;
                        if (proid.PaymentFrequency=="M")
                        {

                            loanser.InstallmentStartDate = TransactionDate.AddMonths(proid.GracePeriod);
                        }
                        else
                        {
                            loanser.InstallmentStartDate = TransactionDate.AddDays(proid.GracePeriod);
                        }

                        loanser.FirstInstallmentStartDate = loanser.InstallmentStartDate;
                        loanser.LoanStatus = 1;
                        loanser.DisburseDate = TransactionDate;
                        loanser.InstallmentDate = TransactionDate;
                      
                        if (loanser.InstallmentStartDate <= TransactionDate)
                        {
                            return GetErrorMessageResult("Pls. enter Valid Date");
                        }
                      
                        if (proid != null)
                        {
                            if (loanser.TransType == 102)
                            {
                                if (Model.BankName == null)
                                {

                                    return GetErrorMessageResult("Please put the BankName");
                                }
                                if (Model.ChequeNo == null)
                                {

                                    return GetErrorMessageResult("Please put the ChequeNo");
                                }
                                if (Model.ChequeIssueDate == null)
                                {

                                    return GetErrorMessageResult("Please put the ChequeIssueDate");
                                }
                            }
                            if (proid.InterestCalculationMethod == "F")
                            {

                                loanser.IntCharge = Math.Round(Convert.ToDecimal((loanser.PrincipalLoan * proid.InterestRate) / 100), 0);
                            }
                            else
                            {
                                loanser.IntCharge = 0;
                            }
                        }
                        loanser.BankName = Model.BankName;
                        loanser.ChequeNo = Model.ChequeNo;
                        loanser.ChequeIssueDate = Model.ChequeIssueDate;
                        loansSummaryService.Update(loanser);

                        var paramSLC = new { @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Convert.ToInt64(loanser.CenterID), @MemberID = loanser.MemberID, @ProductID = loanser.ProductID, @LoanTerm = loanser.LoanTerm, @DisburseDate = TransactionDate, @EmployeeId = loanser.EmployeeId, @MemberCategoryID = loanser.MemberCategoryID, @OrgID = SessionHelper.LoginUserOrganizationID, @CreateUser = LoggedInEmployeeID, @PrincipalAmount = loanser.PrincipalLoan };
                        var paramSA = new { @LoansummaryID = Convert.ToInt64(Model.LoanSummaryID), @PrincipalAmount = loanser.PrincipalLoan };

                        if (loanser.OrgID == 4 || loanser.OrgID == 94)
                        {
                            ultimateReportService.RiskDataTransferIntoSavings(paramSLC);
                        }

                        if (loanser.OrgID == 218)
                        {
                            ultimateReportService.RiskDataTransferIntoSavings(paramSA);
                        }
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
        public ActionResult EditFirstInsDate(LoanDisburseViewModel Model)
        {
            try
            {
               var loanser = loansSummaryService.GetByIdLong(Convert.ToInt64(Model.LoanSummaryID));
                if (ModelState.IsValid)
                {
                    string msg = "";
                    if (disburseservice.IsValidLoan(loanser, out msg))
                    {
                       

                        loanser.FirstInstallmentStartDate = Model.FirstInstallmentStartDate;
                       

                        if (loanser.FirstInstallmentStartDate < loanser.InstallmentStartDate)
                        {
                            return GetErrorMessageResult("Pls. enter Valid Date");
                        }

                       
                        loansSummaryService.Update(loanser);

                      


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
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                var loanser = loansSummaryService.GetByIdLong(Convert.ToInt64(LoanSummaryID));
                loanser.InstallmentStartDate = null;
                loanser.LoanStatus = 1;
                loanser.DisburseDate = null;
                loanser.IsApproved = false;
                loansSummaryService.Update(loanser);

                var paramSLC = new { @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Convert.ToInt16(loanser.CenterID), @ProductID = Convert.ToInt16(loanser.ProductID), @MemberID = loanser.MemberID };

                if (loanser.OrgID == 3)
                {
                    ultimateReportService.DelRiskDataTransferIntoSavings(paramSLC);
                }

              
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetRepaymentSchedule(string MemberID, string ProductID)
        {
            List<RepaymentScheduleViewModel> List_LoanRepaymentScheduleModel = new List<RepaymentScheduleViewModel>();
            //MemberID = "7485";
            //ProductID = "69";
            var param = new { officeId = SessionHelper.LoginUserOfficeID.Value, MemberID = Convert.ToInt64(MemberID), ProductId = Convert.ToInt16(ProductID) };
            var loanInfo = ultimateReportService.GetRepaymentSchedule(param);
            List_LoanRepaymentScheduleModel = loanInfo.Tables[0].AsEnumerable()
                .Select(row => new RepaymentScheduleViewModel
                {
                    CenterCode = row.Field<string>("CenterCode"),
                    LoanTerm = row.Field<int>("LoanTerm"),
                    RepaymentDate = row.Field<string>("RepaymentDate"),
                    InstallmentNo = row.Field<int>("InstallmentNo"),
                    PrincipalLoan = row.Field<decimal>("PrincipalLoan"),//Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("PrincipalLoan")) ? "0" : row.Field<string>("PrincipalLoan")),
                    LoanInstallment = row.Field<decimal>("LoanInstallment"), //Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("LoanRepaid")) ? "0" : row.Field<string>("LoanRepaid")),
                    LoanBalnce = row.Field<decimal>("LoanBalnce"),
                    IntInstallment = row.Field<decimal>("IntPaid"),
                    IntCharge = row.Field<decimal>("IntCharge"),
                    InterestBalance = row.Field<decimal>("InterestBalance")//Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("IntPaid")) ? "0" : row.Field<string>("IntPaid"))
                }).ToList();
            return Json(List_LoanRepaymentScheduleModel, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GenerateReport(string fromDate, string toDate)
        //{
        //    var param = new { Qtype = 1, Office = LoginUserOfficeID, Center = "", DateFrom = TransactionDate, DateTo = TransactionDate, Org=LoggedInOrganizationID };
        //    var allproducts = loanCollectionReportService.GetDataDisbursementInfoDisburePage(param);
        //    var reportParam = new Dictionary<string, object>();
        //    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //    ReportHelper.PrintReport("rptDisburseHistory.rpt", allproducts.Tables[0], reportParam);
        //    return Content(string.Empty);
        //}

        public ActionResult GenerateReport(string fromDate, string toDate)        {            DataSet allproducts;            var param = new { Qtype = 1, Office = LoginUserOfficeID, Center = "", DateFrom = TransactionDate, DateTo = TransactionDate, Org = LoggedInOrganizationID };

            if (LoggedInOrganizationID == 3) //====== Mostak : 09 Jul, 2024 (PIDIM) ========//
            {                allproducts = ultimateReportService.GetDataWithParameter(param, "GetDisburseListDisbursepage__New");            }            else            {                allproducts = loanCollectionReportService.GetDataDisbursementInfoDisburePage(param);            }

            var reportParam = new Dictionary<string, object>();            if (LoggedInOrganizationID == 3) //====== Mostak : 09 Jul, 2024 (PIDIM) ========//
            {                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);                reportParam.Add("TransactionDate", TransactionDate);                ReportHelper.PrintReport("DisbursementTopSheet__PIDIM.rpt", allproducts.Tables[0], reportParam);            }            else            {                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);                ReportHelper.PrintReport("rptDisburseHistory.rpt", allproducts.Tables[0], reportParam);            }            return Content(string.Empty);        }
        #endregion
    }
}

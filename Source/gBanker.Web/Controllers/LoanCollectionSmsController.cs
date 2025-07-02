using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;

namespace gBanker.Web.Controllers
{
    public class LoanCollectionSmsController : BaseController
    {
        #region Variables
        private readonly ICenterService centerService;
        private readonly ILoanCollectionService loanCollectionService;
        private readonly ILoanSummaryService loansummaryService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly ILoanCollectionReportService loanCollectionReportService;
        private readonly ISmsLogService smsLogService;
        private readonly ISmsConfigService smsConfigService;
        public LoanCollectionSmsController(ILoanCollectionService loanCollectionService, ICenterService centerService, IMemberService memberService, IOfficeService officeService, IProductService productService, ILoanCollectionReportService loanCollectionReportService, ISmsLogService smsLogService, ISmsConfigService smsConfigService)
        {
            this.loanCollectionService = loanCollectionService;
            this.centerService = centerService;
            this.memberService = memberService;
            this.officeService = officeService;
            this.productService = productService;
            this.loanCollectionReportService = loanCollectionReportService;
            this.smsLogService = smsLogService;
            this.smsConfigService = smsConfigService;
        }
        #endregion

        #region Methods
        [HttpPost]
        public ActionResult SaveLoanTransaction(Dictionary<string, string> allTrx, List<string> allLoanTrxId)
        {
            try
            {
                loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);

                if (!IsDayInitiated)
                {
                    return Json(new { Result = "ERROR", Message = "Please run the start work process" });
                }
                var trx = allTrx;

                var trxId = 1;
                var loanTrxIds = allLoanTrxId.Where(w => int.TryParse(w, out trxId));

                var loanTrxViewCollection = new List<DailyLoanTrx>();
                foreach (var id in loanTrxIds)
                {
                    var totalId = "Total" + id;

                    var intPaidId = "IntPaid" + id;

                    var loanDueId = "LoanDue" + id;

                    var loanPaidId = "LoanPaid" + id;

                    var loanTernId = "LoanTerm" + id;

                    var intDueId = "IntDue" + id;

                    var princialloanId = "PrincipalLoan" + id;

                    var loanrepaidId = "LoanRepaid" + id;

                    var MemberID = "MemberID" + id;

                    var MemberCodeID = "MemberCode" + id;

                    var MemberNameID = "MemberName" + id;

                    decimal total = 0;
                    decimal loanPaid = 0;
                    decimal intPaid = 0;
                    decimal loanDue = 0;
                    decimal intDue = 0;
                    decimal princialloan = 0;
                    decimal loanrepaid = 0;
                    int mem = 0;
                    string memCode = "";
                    string memName = "";

                    if (allTrx.ContainsKey(totalId))
                        decimal.TryParse(allTrx[totalId], out total);
                    if (allTrx.ContainsKey(loanPaidId))
                        decimal.TryParse(allTrx[loanPaidId], out loanPaid);
                    if (allTrx.ContainsKey(intPaidId))
                        decimal.TryParse(allTrx[intPaidId], out intPaid);
                    if (allTrx.ContainsKey(loanDueId))
                        decimal.TryParse(allTrx[loanDueId], out loanDue);
                    if (allTrx.ContainsKey(intDueId))
                        decimal.TryParse(allTrx[intDueId], out intDue);
                    if (allTrx.ContainsKey(princialloanId))
                        decimal.TryParse(allTrx[princialloanId], out princialloan);
                    if (allTrx.ContainsKey(loanrepaidId))
                        decimal.TryParse(allTrx[loanrepaidId], out loanrepaid);
                    if (allTrx.ContainsKey(MemberID))
                        int.TryParse(allTrx[MemberID], out mem);
                    if (allTrx.ContainsKey(MemberCodeID))
                        memCode = allTrx[MemberCodeID];
                    if (allTrx.ContainsKey(MemberNameID))
                        memName = allTrx[MemberNameID];
                    //if (loanPaid > (princialloan - loanrepaid))
                    //{
                    //    return Json(new { Result = "ERROR", Message = "Balance Loan Amount Cann't be greter than Principal Loan" });
                    //}
                    var loanTrx = new DailyLoanTrx() { DailyLoanTrxID = long.Parse(id), TotalPaid = total, LoanDue = loanDue, IntPaid = intPaid, LoanPaid = loanPaid, IntDue = intDue, MemberID = mem, MemberCode = memCode, MemberName = memName, CreateUser = LoggedInEmployee.EmployeeCode, CollectionStatus = "1" };
                    loanTrxViewCollection.Add(loanTrx);

                }

                loanCollectionService.SaveDailyLoanCollection(loanTrxViewCollection);

                //Auto SMS Send
                foreach (var loan in loanTrxViewCollection)
                {
                    string loanPaid = (loan.LoanPaid + loan.IntPaid).ToString();
                    string loanBalance = ((loan.PrincipalLoan - loan.LoanRepaid) - loan.LoanPaid).ToString();
                    string smsBody = "Thank you" + loan.MemberName + ", for your repayment of " + loanPaid + " on " + TransactionDate.ToString("dd-MMM-yyyy") +
                                     ". Your Loan balance is " + loanBalance + " and you have " + loan.TotalPaid.ToString() + " remaining installments.";
                    var mem = memberService.GetByMemberId(Convert.ToInt64(loan.MemberID));
                    string result = TwilioSms(smsBody, mem.PhoneNo, loan.MemberID.ToString(), loan.TrxDate.ToString("dd-MMM-yyyy"));

                }

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        public ActionResult GenerateReport(string fromDate, string toDate)
        {
            var param = new { OfficeID = LoginUserOfficeID };
            var allproducts = loanCollectionReportService.GetDataCollectionInfo(param);
            var reportParam = new Dictionary<string, object>();
            reportParam.Add("Header1", ApplicationSettings.OrganiztionName);
            ReportHelper.PrintReport("rptDailyLoanCollection.rpt", allproducts.Tables[0], reportParam);
            return Content(string.Empty);
        }
        private void MapDropDownList(DailyLoanTrxViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }

            var allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID));

            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;
            //var allcenter = centerService.GetByOfficeId(LoginUserOfficeID.Value);
            //var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            //model.centerListItems = viewCenter;


            var Transtype = new List<SelectListItem>();
            Transtype.Add(new SelectListItem() { Text = "Transfer", Value = "202", Selected = true });
            Transtype.Add(new SelectListItem() { Text = "Bad Debt", Value = "104" });
            model.cashListItems = Transtype.AsEnumerable();

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, m.FirstName + '-' + m.MiddleName + '-' + m.LastName), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;


            var alloffice = officeService.GetAll().Where(l => l.OfficeID == LoginUserOfficeID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;


            var allSearchProd = productService.SearchProduct(0, Convert.ToInt16(LoggedInOrganizationID),"L");
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            proditems.AddRange(viewProdList);
            model.productListItems = proditems;


        }

        public ActionResult GetInstallment(string officeId, string centerId, string MemId, int productid, int loanTerm)
        {
            decimal vLoanInstallment = 0;
            decimal vInterestInstallment = 0;
            decimal vTotalIns = 0;
            var LoanInstallMent = loanCollectionService.GetAll().Where(l => l.OfficeID == Convert.ToInt16(officeId) && l.MemberID == Convert.ToInt64(MemId) && l.ProductID == productid && l.LoanTerm == loanTerm && l.IsActive == true).FirstOrDefault(); ;

            //var LoanInstallMent = loanCollectionService.GetAll().Where(l => l.OfficeID == Convert.ToInt16(officeId) && l.CenterID == Convert.ToInt16(centerId) && l.MemberID == Convert.ToInt16(MemId) && l.ProductID == productid && l.LoanTerm == loanTerm && l.IsActive == true).FirstOrDefault(); ;
            if (LoanInstallMent != null)
            {
                vLoanInstallment = LoanInstallMent.LoanDue;
                vInterestInstallment = LoanInstallMent.IntDue;
                vTotalIns = vLoanInstallment + vInterestInstallment;

            }
            else
            {
                vLoanInstallment = 0;
                vInterestInstallment = 0;
                vTotalIns = 0;
            }
            //  var pbr = productService.GetById(productid);


            var result = new { loan = vLoanInstallment.ToString(), interest = vInterestInstallment.ToString(), total = vTotalIns.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMaxLoanTerm(string officeId, string centerId, string MemId, string ProdId)
        {
            int vLoanTerm;
            var model = new DailyLoanTrxViewModel();
            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt16(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);


            var entity = Mapper.Map<DailyLoanTrxViewModel, DailyLoanTrx>(model);
            var mlt = loanCollectionService.getMaxLoanterm(entity);
            //Session[ProductSessionKey] = pbr;
            vLoanTerm = Convert.ToInt16(mlt);

            var result = new { LoanTerm = vLoanTerm.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMemberList(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, m.FirstName + " " + m.MiddleName + " " + m.LastName).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, m1.FirstName + " " + m1.MiddleName + " " + m1.LastName) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDailyLoanCollectionSheet(int centerId, int productId, string filterColumn, string filterValue, string sortColumn, string sortOrder)
        {
            try
            {
                var collectionList = loanCollectionService.GetDailyLoanCollectionByCenter(centerId, filterColumn, filterValue, sortColumn, sortOrder).ToList();

                var members = collectionList.Where(c => c.ProductID == productId && c.CenterID == centerId).ToList();
                var memberModels = Mapper.Map<IEnumerable<DailyLoanTrx>, IEnumerable<DailyLoanTrxViewModel>>(members);


                List<DailyLoanTrxViewModel> detail = new List<DailyLoanTrxViewModel>();
                foreach (var vd in memberModels)
                {
                    var memberInfo = memberService.GetByMemberId(Convert.ToInt64(vd.MemberID));
                    var smsInfo = smsLogService.GetLoanCollectionSms(1, vd.MemberID, vd.TrxDate);
                    string sms_status = "";
                    if (smsInfo != null)
                        sms_status = smsInfo.SmsStatus;
                    var loans = new DailyLoanTrxViewModel() { DailyLoanTrxID = vd.DailyLoanTrxID, TrxDate = vd.TrxDate, TrxDateMsg = vd.TrxDate.ToString("dd-MMM-yyyy"), LoanSummaryID = vd.LoanSummaryID, OfficeID = vd.OfficeID, MemberID = vd.MemberID, MemberCode = vd.MemberCode, MemberName = vd.MemberName, PhoneNo = memberInfo.PhoneNo.ToString(), ProductID = vd.ProductID, ProductCode = vd.ProductCode, ProductName = vd.ProductName, InterestCalculationMethod = vd.InterestCalculationMethod, CenterID = vd.CenterID, MemberCategoryID = vd.MemberCategoryID, LoanTerm = vd.LoanTerm, PurposeID = vd.PurposeID, InstallmentDate = vd.InstallmentDate, PrincipalLoan = vd.PrincipalLoan, LoanRepaid = vd.LoanRepaid, LoanDue = vd.LoanDue, LoanPaid = vd.LoanPaid, IntPaid = vd.IntPaid, CumIntCharge = vd.CumIntCharge, IntCharge = vd.IntCharge, IntDue = vd.IntDue, Advance = vd.Advance, DueRecovery = vd.DueRecovery, TrxType = vd.TrxType, InstallmentNo = vd.InstallmentNo, EmployeeID = vd.EmployeeID, TotalPaid = vd.TotalPaid, InvestorID = vd.InvestorID, memName = vd.memName, vMaxLoanTerm = vd.vMaxLoanTerm, DueLoanSummary = vd.DueLoanSummary, LoanCollectionSummary = vd.LoanCollectionSummary, DueInterestSummary = vd.DueInterestSummary, InterestCollectionSummary = vd.InterestCollectionSummary, TotalDueSummary = vd.TotalDueSummary, TotalCollectionSummary = vd.TotalCollectionSummary, CollectionStatus = vd.CollectionStatus, SmsStatus = sms_status, LoanNo = vd.LoanNo };
                    detail.Add(loans);
                }

                return Json(new { Result = "OK", Records = detail });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult GetDailyLoanCollectionProductList(int jtStartIndex, int jtPageSize, string jtSorting, int centerId, string filterColumn, string filterValue)
        {
            try
            {
                // long totalCount;

                var collectionList = loanCollectionService.GetDailyLoanCollectionByCenter(centerId, filterColumn, filterValue, "", "").ToList();
                // var collectionList = loanCollectionService.GetLoanCollectionDetailPaged(centerId, filterColumn, filterValue, jtStartIndex, jtPageSize, out totalCount).ToList();
                var products = new List<DailyLoanTrxViewModel>();
                foreach (var tr in collectionList)
                {
                    if (products.Where(p => p.ProductID == tr.ProductID && tr.CenterID == centerId).FirstOrDefault() == null)
                    {
                        var prodViewModel = Mapper.Map<DailyLoanTrx, DailyLoanTrxViewModel>(tr);
                        // var v = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.LoanDue);
                        prodViewModel.DueLoanSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.LoanDue);
                        prodViewModel.LoanCollectionSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.LoanPaid);
                        prodViewModel.DueInterestSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.IntDue);
                        prodViewModel.InterestCollectionSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.IntPaid);
                        prodViewModel.TotalDueSummary = prodViewModel.DueLoanSummary + prodViewModel.DueInterestSummary;
                        prodViewModel.TotalCollectionSummary = prodViewModel.LoanCollectionSummary + prodViewModel.InterestCollectionSummary;
                        products.Add(prodViewModel);
                    }

                }

                var currentPageProducts = products.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageProducts, TotalRecordCount = products.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        private List<DailyLoanTrxViewModel> GetProductList()
        {
            var collectionList = new List<DailyLoanTrxViewModel>();
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 1, CenterID = 1, MemberID = 10000, ProductID = 222, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 1, CenterID = 1, MemberID = 10000, ProductID = 225, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 2, CenterID = 1, MemberID = 10000, ProductID = 333, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 3, CenterID = 1, MemberID = 10002, ProductID = 222, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 4, CenterID = 1, MemberID = 10003, ProductID = 222, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 5, CenterID = 1, MemberID = 10003, ProductID = 333, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 6, CenterID = 1, MemberID = 10004, ProductID = 222, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 7, CenterID = 1, MemberID = 10004, ProductID = 555, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 8, CenterID = 1, MemberID = 10006, ProductID = 222, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });
            collectionList.Add(new DailyLoanTrxViewModel() { DailyLoanTrxID = 9, CenterID = 1, MemberID = 10007, ProductID = 222, LoanDue = 100, IntDue = 10, LoanPaid = 100, IntPaid = 10, LoanTerm = 1 });

            return collectionList.Select(s => new DailyLoanTrxViewModel() { ProductID = s.ProductID }).Distinct().ToList();
        }

        private string TwilioSms(string msgBody, string receiver, string memId, string collDt)
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
                    log.SmsType = "C";
                    log.SmsFrom = PhoneNo;
                    log.SmsTo = receiver;
                    log.SmsBody = msgBody;
                    log.DateSent = Convert.ToDateTime(collDt);
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
                    log.SmsType = "C";
                    log.SmsFrom = PhoneNo;
                    log.SmsTo = receiver;
                    log.SmsBody = msgBody;
                    log.DateSent = Convert.ToDateTime(collDt);
                    log.SmsStatus = "Fail";
                    log.IsActive = true;
                    var entity = Mapper.Map<SmsLogViewModel, SmsLog>(log);
                    smsLogService.Create(entity);
                }
                return result;
            }
            catch (Exception ex)
            {
                return result = string.Empty;
            }
        }
        public ActionResult SendSMS(string msgBody, string receiver, string memId, string collDt)
        {
            var result = TwilioSms(msgBody, receiver, memId, collDt);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Events
        // GET: LoanCollection
        public ActionResult Index()
        {

            var model = new DailyLoanTrxViewModel();
            if (IsDayInitiated)
                MapDropDownList(model);
            else
            {
                model.centerListItems = new List<SelectListItem>() { new SelectListItem() { Text = "Select Center", Value = "0" } };
            }
            //if (!IsDayInitiated)
            //{
            //    return Json(new { Result = "ERROR", Message = "Please run the start work process" });
            //}

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(Dictionary<string, string> allTrx)
        {
            var trx = allTrx;
            //var formvalues = form;
            //return View();
            var model = new DailyLoanTrxViewModel();


            MapDropDownList(model);

            return View(model);
        }
        // GET: LoanCollection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoanCollection/Create
        public ActionResult Create()
        {

            var model = new DailyLoanTrxViewModel();

            if (IsDayInitiated)
            {
                model.TrxDate = TransactionDate;
            }

            MapDropDownList(model);

            return View(model);
        }

        // POST: LoanCollection/Create
        [HttpPost]
        public ActionResult Create(DailyLoanTrxViewModel model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                var entity = Mapper.Map<DailyLoanTrxViewModel, DailyLoanTrx>(model);

                //Add Validlation Logic.

                if (ModelState.IsValid)
                {

                    loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);
                    var summary = loanCollectionService.GetAll().Where(s =>s.OrgID==LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.MemberID == entity.MemberID && s.CenterID == entity.CenterID && s.ProductID == entity.ProductID && s.LoanTerm == entity.LoanTerm && s.IsActive == true).FirstOrDefault();

                    //var getSummaryId = LoanSummaryService.GetAll();
                    if (summary != null)
                    {

                        var getLoanCol = loanCollectionService.GetById(Convert.ToInt16(summary.DailyLoanTrxID));
                        getLoanCol.LoanPaid = entity.LoanPaid;
                        getLoanCol.IntPaid = entity.IntPaid;
                        getLoanCol.TotalPaid = entity.TotalPaid;
                        getLoanCol.InstallmentDate = model.TrxDate;
                        getLoanCol.TrxType = 0;
                        getLoanCol.Advance = 0;
                        getLoanCol.CumIntCharge = 0;
                        getLoanCol.DueRecovery = 0;
                        getLoanCol.IntCharge = 0;
                        getLoanCol.IntDue = 0;
                        getLoanCol.LoanDue = 0;
                        getLoanCol.LoanRepaid = 0;
                        getLoanCol.PrincipalLoan = 0;
                        getLoanCol.TotalPaid = 0;
                        getLoanCol.TrxType = 11;
                        var errors = loanCollectionService.IsValidLoan(getLoanCol);

                        if (errors.ToList().Count == 0)
                        {

                            loanCollectionService.Create(getLoanCol);
                            return GetSuccessMessageResult();
                            //ModelState.Clear();
                            //var emtpy = new DailyLoanTrxViewModel();
                            //if (IsDayInitiated)
                            //{
                            //    emtpy.TrxDate = TransactionDate;
                            //}
                            //MapDropDownList(emtpy);

                            //return View(emtpy);
                        }
                        else
                            return GetErrorMessageResult();

                    }

                }
                //MapDropDownList(model);
                //return View(model);
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: LoanCollection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoanCollection/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: LoanCollection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoanCollection/Delete/5
        [HttpPost]
        public ActionResult Delete(int DailyLoanTrxID, DailyLoanTrxViewModel model)
        {

            try
            {
                loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);
                var sp = loanCollectionService.GetById(DailyLoanTrxID);
                var entity = Mapper.Map<DailyLoanTrxViewModel, DailyLoanTrx>(model);
                if (sp.TrxType == 11)
                {
                    loanCollectionService.Delete(DailyLoanTrxID);
                }

                // TODO: Add delete logic here
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

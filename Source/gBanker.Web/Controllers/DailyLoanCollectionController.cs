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

namespace gBanker.Web.Controllers
{
    public class DailyLoanCollectionController :  BaseController
    {
        #region Variables
        private readonly ICenterService centerService;
        private readonly ILoanCollectionService loanCollectionService;
        private readonly ILoanSummaryService loansummaryService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly ILoanCollectionReportService loanCollectionReportService;
        private readonly IUltimateReportService ultimateReportService;
        public DailyLoanCollectionController(ILoanCollectionService loanCollectionService, ICenterService centerService, IMemberService memberService, IOfficeService officeService, IProductService productService, ILoanCollectionReportService loanCollectionReportService, IUltimateReportService ultimateReportService)
        {
            this.loanCollectionService = loanCollectionService;
            this.centerService = centerService;
            this.memberService = memberService;
            this.officeService = officeService;
            this.productService = productService;
            this.loanCollectionReportService = loanCollectionReportService;
            this.ultimateReportService = ultimateReportService;
        }
        #endregion
        #region Methods
        [HttpPost]
        public ActionResult SaveLoanTransaction(Dictionary<string, string> allTrx, List<string> allLoanTrxId)
        {

            try
            {
                loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);

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

                    decimal total = 0;
                    decimal loanPaid = 0;
                    decimal intPaid = 0;
                    decimal loanDue = 0;
                    decimal intDue = 0;
                    decimal princialloan = 0;
                    decimal loanrepaid = 0;
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
                   
                    var loanTrx = new DailyLoanTrx() { DailyLoanTrxID = long.Parse(id), TotalPaid = total, LoanDue = loanDue, IntPaid = intPaid, LoanPaid = loanPaid, IntDue = intDue };
                    loanTrxViewCollection.Add(loanTrx);

                }

                loanCollectionService.SaveDailyLoanCollection(loanTrxViewCollection);

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        public ActionResult GenerateReport(string fromDate, string toDate, string CenterID)
        {
            var param = new { OfficeID = LoginUserOfficeID, CenterId = CenterID };
            var allproducts = loanCollectionReportService.GetDataCollectionInfo(param);
            var reportParam = new Dictionary<string, object>();
            reportParam.Add("Header1", ApplicationSettings.OrganiztionName);
            ReportHelper.PrintReport("rptDailyLoanCollection.rpt", allproducts.Tables[0], reportParam);
            return Content(string.Empty);
        }
        public ActionResult UpdateDataLessFiftyPercent(string officeId, string CenterID)
        {
            var result = loanCollectionService.setLoanAndSavingingLessFiftyPercent(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(CenterID), 1);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private void MapDropDownList(DailyLoanTrxViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }

            var allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, LoggedInOrganizationID);
            var viewCenList = allcenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = string.Format("{0} - {1}", x.CenterCode.ToString(), x.CenterName.ToString())
            });
            var cenitems = new List<SelectListItem>();
            cenitems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            cenitems.AddRange(viewCenList);
            model.centerListItems = cenitems;


            var Transtype = new List<SelectListItem>();
            Transtype.Add(new SelectListItem() { Text = "Transfer", Value = "11", Selected = true });
            Transtype.Add(new SelectListItem() { Text = "Bad Debt", Value = "104" });
            model.cashListItems = Transtype.AsEnumerable();

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));
  
            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, m.FirstName + '-' + m.MiddleName + '-' + m.LastName), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;


            var alloffice = officeService.GetAll().Where(l => l.OfficeID == LoginUserOfficeID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;


            var allSearchProd = loanCollectionService.getDailyProduct(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID)).Distinct();
            var viewProdList = allSearchProd.Select(x => x).Distinct().ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), (string.IsNullOrEmpty(x.ProductName) ? "" : x.ProductName))
            }).Distinct();
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            proditems.AddRange(viewProdList);
            model.productListItems = proditems.Distinct();


        }

        public ActionResult GetInstallment(string officeId, string centerId, string MemId, int productid, int loanTerm)
        {
            decimal vLoanInstallment = 0;
            decimal vInterestInstallment = 0;
            decimal vTotalIns = 0;
            string vInterestCalcMethod = string.Empty, vPaymentFreq = string.Empty;
            decimal vLoanDue = 0;
            decimal vInterestDue = 0;
            decimal vPrincipalLoan = 0;
            decimal vLoanRepaid = 0;
            Int64 vDailyLoanTrxID = 0;
            var LoanInstallMent = loanCollectionService.GetAll().Where(l => l.OfficeID == Convert.ToInt16(officeId) && l.MemberID == Convert.ToInt64(MemId) && l.ProductID == productid && l.LoanTerm == loanTerm && l.IsActive == true).FirstOrDefault(); ;

            if (LoanInstallMent != null)
            {
                vLoanInstallment = LoanInstallMent.LoanDue;
                vInterestInstallment = LoanInstallMent.IntDue;
                vTotalIns = vLoanInstallment + vInterestInstallment;
                var prod = productService.GetById(productid);
                vInterestCalcMethod = prod.InterestCalculationMethod;
                vPaymentFreq = prod.PaymentFrequency;
                vLoanDue = LoanInstallMent.LoanDue;
                vInterestDue = LoanInstallMent.IntDue;
                vPrincipalLoan = LoanInstallMent.PrincipalLoan;
                vLoanRepaid = LoanInstallMent.LoanRepaid;
                vDailyLoanTrxID = LoanInstallMent.DailyLoanTrxID;

            }
            else
            {
                vLoanInstallment = 0;
                vInterestInstallment = 0;
                vTotalIns = 0;
                var prod = productService.GetById(productid);
                vInterestCalcMethod = prod.InterestCalculationMethod;
                vPaymentFreq = prod.PaymentFrequency;
            }
  
            var result = new { loan = vLoanInstallment.ToString(), interest = vInterestInstallment.ToString(), interestCalcMethod = vInterestCalcMethod, PaymentFreq = vPaymentFreq, LoanDue = vLoanDue, InterestDue = vInterestDue, PrincipalLoan = vPrincipalLoan, LoanRepaid = vLoanRepaid, DailyLoanTrxID = vDailyLoanTrxID, total = vTotalIns.ToString() };
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
            entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
            var mlt = loanCollectionService.getMaxLoanterm(entity);
            vLoanTerm = Convert.ToInt16(mlt);

            var result = new { LoanTerm = vLoanTerm.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMemberList(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<getDailyMember_Result>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<getDailyMember_Result>;
            else
            {
                 var mbr = loanCollectionService.getDailyMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();
                var dmbr = mbr.Where(m => m.CenterID == int.Parse(centerId)).ToList();
                Session[MemberByCenterSessionKey] = dmbr;
                memberList = dmbr;
                
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, m.MemberName).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, m1.MemberName) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDailyLoanCollectionSheet(int centerId, int productId, long MemberID)
        {
            try
            {

                var collectionList = loanCollectionService.GetDailyLoanCollectionByCenterMember(centerId, productId, MemberID).ToList();
                IEnumerable<DailyLoanTrx> members ;
                if (MemberID>0)
                {
                    members = collectionList.Where(c => c.ProductID == productId && c.CenterID == centerId && c.OfficeID == LoginUserOfficeID && c.MemberID==MemberID).ToList().OrderBy(c => c.ProductCode);
                }
                else
                 members = collectionList.Where(c => c.ProductID == productId && c.CenterID == centerId && c.OfficeID == LoginUserOfficeID).ToList().OrderBy(c => c.ProductCode);
               
                
                var memberModels = Mapper.Map<IEnumerable<DailyLoanTrx>, IEnumerable<DailyLoanTrxViewModel>>(members);


                List<DailyLoanTrxViewModel> detail = new List<DailyLoanTrxViewModel>();
                int rowSl = 0;
                foreach (var vd in memberModels)
                {
                    var loans = new DailyLoanTrxViewModel() { rowSl = rowSl, DailyLoanTrxID = vd.DailyLoanTrxID, TrxDate = vd.TrxDate, TrxDateMsg = vd.TrxDate.ToString("dd-MMM-yyyy"), LoanSummaryID = vd.LoanSummaryID, OfficeID = vd.OfficeID, MemberID = vd.MemberID, MemberCode = vd.MemberCode, MemberName = vd.MemberName, ProductID = vd.ProductID, ProductCode = vd.ProductCode, ProductName = vd.ProductName, InterestCalculationMethod = vd.InterestCalculationMethod, CenterID = vd.CenterID, MemberCategoryID = vd.MemberCategoryID, LoanTerm = vd.LoanTerm, PurposeID = vd.PurposeID, InstallmentDate = vd.InstallmentDate, PrincipalLoan = vd.PrincipalLoan, LoanRepaid = vd.LoanRepaid, LoanDue = vd.LoanDue, LoanPaid = vd.LoanPaid, IntPaid = vd.IntPaid, CumIntCharge = vd.CumIntCharge, IntCharge = vd.IntCharge, IntDue = vd.IntDue, Advance = vd.Advance, DueRecovery = vd.DueRecovery, TrxType = vd.TrxType, InstallmentNo = vd.InstallmentNo, EmployeeID = vd.EmployeeID, TotalPaid = vd.TotalPaid, InvestorID = vd.InvestorID, memName = vd.memName, vMaxLoanTerm = vd.vMaxLoanTerm, DueLoanSummary = vd.DueLoanSummary, LoanCollectionSummary = vd.LoanCollectionSummary, DueInterestSummary = vd.DueInterestSummary, InterestCollectionSummary = vd.InterestCollectionSummary, TotalDueSummary = vd.TotalDueSummary, TotalCollectionSummary = vd.TotalCollectionSummary };
                    detail.Add(loans);
                    rowSl++;
                }



                return Json(new { Result = "OK", Records = detail });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        [HttpPost]
         public ActionResult GetDailyLoanCollectionProductList(int jtStartIndex, int jtPageSize, string jtSorting, int centerId, string MemberID, string productID)
        {
            try
            {
                if (productID == "")
                {
                    productID = "0";
                }
                var collectionList = loanCollectionService.GetDailyLoanCollectionByCenterMember(centerId,Convert.ToInt16(productID), Convert.ToInt64(MemberID)).ToList();
                var products = new List<DailyLoanTrxViewModel>();
                foreach (var tr in collectionList)
                {
                    if (products.Where(p => p.ProductID == tr.ProductID && tr.CenterID == centerId ).OrderBy(p => p.ProductCode).FirstOrDefault() == null)
                    {
                        var prodViewModel = Mapper.Map<DailyLoanTrx, DailyLoanTrxViewModel>(tr);
                        prodViewModel.DueLoanSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID ).Sum(s => s.LoanDue);
                        prodViewModel.LoanCollectionSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.LoanPaid);
                        prodViewModel.DueInterestSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID ).Sum(s => s.IntDue);
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

        #endregion
        // GET: DailyLoanCollection
        public ActionResult Index()
        {
            var model = new DailyLoanTrxViewModel();
            if (IsDayInitiated)
                MapDropDownList(model);
            else
            {
                model.centerListItems = new List<SelectListItem>() { new SelectListItem() { Text = "Select Center", Value = "0" } };
                model.productListItems = new List<SelectListItem>() { new SelectListItem() { Text = "Select Product", Value = "0" } };
            }
              
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(Dictionary<string, string> allTrx)
        {
            var trx = allTrx;
             var model = new DailyLoanTrxViewModel();
            MapDropDownList(model);

            return View(model);
        }
        // GET: DailyLoanCollection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DailyLoanCollection/Create
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

        // POST: DailyLoanCollection/Create
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

                    loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                    var summary = loanCollectionService.GetAll().Where(s => s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.MemberID == entity.MemberID && s.CenterID == entity.CenterID && s.ProductID == entity.ProductID && s.LoanTerm == entity.LoanTerm && s.IsActive == true).FirstOrDefault();

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
                           
                        }
                        else
                            return GetErrorMessageResult();

                    }

                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: DailyLoanCollection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DailyLoanCollection/Edit/5
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

        // GET: DailyLoanCollection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DailyLoanCollection/Delete/5
        [HttpPost]
        public ActionResult Delete(int DailyLoanTrxID, DailyLoanTrxViewModel model)
        {
            try
            {
                loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
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

            //////////////Bappa

        }
        public ActionResult LoanRegisterForChequeReceiveReturn()
        {
            return View();
        }
        public ActionResult GetLoanRegisterForChequeReceiveReturnReport(string dateFrom, string dateTo)
        {
            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;
                var officeId = SessionHelper.LoginUserOfficeID;
                var param = new { OrgID = orgID, OfficeID = officeId, DateFrom = dateFrom, DateTo = dateTo };
                var data = ultimateReportService.GetDataWithParameter(param, "SP_LoanRegister_ChequeReceiveReturn");
                var reportParam = new Dictionary<string, object>();
                ReportHelper.PrintReport("LoanRegister_ChequeReceiveReturn.rpt", data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

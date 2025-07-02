using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Web.Core.Extensions;
using gBanker.Web.Helpers;
using gBanker.Data.DBDetailModels;
using gBanker.Service.ReportServies;

namespace gBanker.Web.Controllers
{
    public class DailySavingCollectionController : BaseController
    {
          private readonly ICenterService centerService;
          private readonly ISavingCollectionService savingCollectionService;
          private readonly ILoanCollectionService loanCollectionService;
          private readonly IMemberService memberService;
          private readonly IOfficeService officeService;
          private readonly IProductService productService;
          private readonly ILoanCollectionReportService loanCollectionReportService;
        public DailySavingCollectionController(ISavingCollectionService savingCollectionService, ICenterService centerService, IProductService productService, IOfficeService officeService, IMemberService memberService, ILoanCollectionService loanCollectionService, ILoanCollectionReportService loanCollectionReportService)
          {
              this.savingCollectionService = savingCollectionService;
              this.centerService = centerService;
              this.productService = productService;
             
              this.officeService = officeService;
             
              this.memberService = memberService;
              this.loanCollectionService = loanCollectionService;
              this.loanCollectionReportService = loanCollectionReportService;
             
          }
        // GET: DailySavingCollection
        public ActionResult Index()
        {
            var model = new DailySavingCollectionViewModel();
            if (IsDayInitiated)
                MapDropDownList(model);
            else
            {
                model.centerListItems = new List<SelectListItem>() { new SelectListItem() { Text = "Select Center", Value = "0" } };
                model.productListItems = new List<SelectListItem>() { new SelectListItem() { Text = "Select Product", Value = "0" } };
            }

            return View(model);
        }
        private void MapDropDownList(DailySavingCollectionViewModel model)
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
            Transtype.Add(new SelectListItem() { Text = "Transfer", Value = "0", Selected = true });
            Transtype.Add(new SelectListItem() { Text = "Cash", Value = "1" });
            model.cashListItems = Transtype.AsEnumerable();

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, m.FirstName + '-' + m.MiddleName + '-' + m.LastName), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;


            var alloffice = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

            var allSearchProd = savingCollectionService.getDailySavingProduct(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID)).Distinct();
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
        public ActionResult GetInstallment(string officeId, string centerId, string MemId, string ProdId, string NoOfAccount, decimal SavingInstallment, decimal WithDrawal, decimal penalty)
        {
            decimal vLoanInstallment = 0;
            var model = new DailySavingCollectionViewModel();
            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt16(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);
            model.NoOfAccount = Convert.ToInt16(NoOfAccount);
            var entity = Mapper.Map<DailySavingCollectionViewModel, DailySavingTrx>(model);

            var mlt = savingCollectionService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == model.OfficeID && s.MemberID == model.MemberID && s.ProductID == model.ProductID && s.NoOfAccount == model.NoOfAccount && s.IsActive == 1).FirstOrDefault();

            if (mlt != null)
            {

                vLoanInstallment = (mlt.Balance + SavingInstallment + penalty) - WithDrawal;


            }
            else
            {
                var sm = savingCollectionService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.CenterID == model.CenterID && s.MemberID == model.MemberID && s.ProductID == model.ProductID && s.NoOfAccount == model.NoOfAccount && s.IsActive == 1).FirstOrDefault();

                if (sm != null)
                {
                    vLoanInstallment = (sm.Balance + SavingInstallment + penalty) - WithDrawal;
                }

            }
            var result = new { loan = vLoanInstallment.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNoOfAccount(string officeId, string centerId, string MemId, string ProdId)
        {
            int vLoanTerm;
            int Loantrm = 0;
            decimal vBalance = 0;
            var model = new DailySavingCollectionViewModel();
            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt16(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);


            var entity = Mapper.Map<DailySavingCollectionViewModel, DailySavingTrx>(model);
            var mlt = savingCollectionService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.MemberID == entity.MemberID && s.ProductID == entity.ProductID && s.IsActive == 1).FirstOrDefault();

            if (mlt != null)
            {

                Loantrm = mlt.NoOfAccount;
                vBalance = mlt.Balance;

            }
            vLoanTerm = Loantrm;
  
            var result = new { LoanTerm = vLoanTerm.ToString(), Balance = vBalance };
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
                var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, m.FirstName + " " + m.MiddleName + " " + m.LastName).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, m1.FirstName + " " + m1.MiddleName + " " + m1.LastName) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDailySavingCollectionSheet(int centerId, int productId, long memberID)
        {
            try
            {
                var collectionList = savingCollectionService.GetDailySavingCollectionByCenterMember(centerId, productId, memberID).ToList();


                var members = collectionList.Where(c => c.ProductID == productId && c.CenterID == centerId && c.MemberID==memberID).ToList().OrderBy(c => c.ProductCode);
                var memberModels = Mapper.Map<IEnumerable<DailySavingTrx>, IEnumerable<DailySavingCollectionViewModel>>(members);

                List<DailySavingCollectionViewModel> detail = new List<DailySavingCollectionViewModel>();
                int rowSl = 0;
                foreach (var vd in memberModels)
                {
                    var loans = new DailySavingCollectionViewModel() { rowSl = rowSl, DailySavingTrxID = vd.DailySavingTrxID, TransactionDate = vd.TransactionDate, TrxDateMsg = vd.TransactionDate.ToString("dd-MMM-yyyy"), SavingSummaryID = vd.SavingSummaryID, OfficeID = vd.OfficeID, MemberID = vd.MemberID, MemberCode = vd.MemberCode, MemberName = vd.MemberName, ProductID = vd.ProductID, ProductCode = vd.ProductCode, ProductName = vd.ProductName, NoOfAccount = vd.NoOfAccount, CenterID = vd.CenterID, SavingInstallment = vd.SavingInstallment, Deposit = vd.Deposit, Withdrawal = vd.Withdrawal, Balance = vd.Balance, Penalty = vd.Penalty, TransType = vd.TransType, MonthlyInterest = vd.MonthlyInterest, PresenceInd = vd.PresenceInd, TransferDeposit = vd.TransferDeposit, TransferWithdrawal = vd.TransferWithdrawal, DueSavingSummary = vd.DueSavingSummary, SavingCollectionSummary = vd.SavingCollectionSummary, WithDrawalSummary = vd.WithDrawalSummary, PenaltySummary = vd.PenaltySummary, memName = vd.memName, vMaxLoanTerm = vd.vMaxLoanTerm };
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
        public ActionResult GenerateReport(string fromDate, string toDate, string CenterID)
        {

            if (CenterID == "0")
            {
                var param = new { Qtype = 1, Org = LoggedInOrganizationID, Office = LoginUserOfficeID, Center = CenterID };
                var allproducts = loanCollectionReportService.GetDataSavingCollectionInfo(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptDailySavingsCollection.rpt", allproducts.Tables[0], reportParam);
            }
            else
            {
                var param = new { Qtype = 2, Org = LoggedInOrganizationID, Office = LoginUserOfficeID, Center = CenterID };
                var allproducts = loanCollectionReportService.GetDataSavingCollectionInfo(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptDailySavingsCollection.rpt", allproducts.Tables[0], reportParam);
            }


            return Content(string.Empty);
        }
        public ActionResult UpdateDataLessFiftyPercent(string officeId, string CenterID)
        {
            var result = loanCollectionService.setLoanAndSavingingLessFiftyPercent(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(CenterID), 2);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDailySavingCollectionProductList(int jtStartIndex, int jtPageSize, string jtSorting, int centerId, string MemberID, string productID)
        {
            try
            {
                 if (productID == "")
                {
                    productID = "0";
                }
                var collectionList = savingCollectionService.GetDailySavingCollectionByCenterMember(centerId, Convert.ToInt16(productID), Convert.ToInt64(MemberID)).ToList();
                var products = new List<DailySavingCollectionViewModel>();
                foreach (var tr in collectionList)
                {
                    if (products.Where(p => p.ProductID == tr.ProductID && tr.CenterID == centerId).OrderBy(p => p.ProductCode).FirstOrDefault() == null)
                    {

                        var prodViewModel = Mapper.Map<DailySavingTrx, DailySavingCollectionViewModel>(tr);
                        prodViewModel.DueSavingSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.DueSavingInstallment);
                        prodViewModel.SavingCollectionSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.SavingInstallment);
                        prodViewModel.WithDrawalSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.Withdrawal);
                        prodViewModel.PenaltySummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.Penalty);
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
        private List<DailySavingCollectionViewModel> GetProductList()
        {
            var collectionList = new List<DailySavingCollectionViewModel>();
            collectionList.Add(new DailySavingCollectionViewModel() { DailySavingTrxID = 1, CenterID = 1, MemberID = 10000, ProductID = 222, Deposit = 100, Withdrawal = 10, SavingInstallment = 100, NoOfAccount = 1 });
            collectionList.Add(new DailySavingCollectionViewModel() { DailySavingTrxID = 1, CenterID = 1, MemberID = 10000, ProductID = 225, Deposit = 100, Withdrawal = 10, SavingInstallment = 100, NoOfAccount = 1 });
            collectionList.Add(new DailySavingCollectionViewModel() { DailySavingTrxID = 2, CenterID = 1, MemberID = 10000, ProductID = 333, Deposit = 100, Withdrawal = 10, SavingInstallment = 100, NoOfAccount = 1 });
            collectionList.Add(new DailySavingCollectionViewModel() { DailySavingTrxID = 3, CenterID = 1, MemberID = 10002, ProductID = 222, Deposit = 100, Withdrawal = 10, SavingInstallment = 100, NoOfAccount = 1 });
            collectionList.Add(new DailySavingCollectionViewModel() { DailySavingTrxID = 4, CenterID = 1, MemberID = 10003, ProductID = 222, Deposit = 100, Withdrawal = 10, SavingInstallment = 100, NoOfAccount = 1 });
            collectionList.Add(new DailySavingCollectionViewModel() { DailySavingTrxID = 5, CenterID = 1, MemberID = 10003, ProductID = 333, Deposit = 100, Withdrawal = 10, SavingInstallment = 100, NoOfAccount = 1 });
            collectionList.Add(new DailySavingCollectionViewModel() { DailySavingTrxID = 6, CenterID = 1, MemberID = 10004, ProductID = 222, Deposit = 100, Withdrawal = 10, SavingInstallment = 100, NoOfAccount = 1 });
            collectionList.Add(new DailySavingCollectionViewModel() { DailySavingTrxID = 7, CenterID = 1, MemberID = 10004, ProductID = 555, Deposit = 100, Withdrawal = 10, SavingInstallment = 100, NoOfAccount = 1 });
            collectionList.Add(new DailySavingCollectionViewModel() { DailySavingTrxID = 8, CenterID = 1, MemberID = 10006, ProductID = 222, Deposit = 100, Withdrawal = 10, SavingInstallment = 100, NoOfAccount = 1 });
            collectionList.Add(new DailySavingCollectionViewModel() { DailySavingTrxID = 9, CenterID = 1, MemberID = 10007, ProductID = 222, Deposit = 100, Withdrawal = 10, SavingInstallment = 100, NoOfAccount = 1 });

            return collectionList.Select(s => new DailySavingCollectionViewModel() { ProductID = s.ProductID }).Distinct().ToList();
        }
        // GET: SavingCollection/Details/5
        [HttpPost]
        public ActionResult SaveSavingTransaction(Dictionary<string, string> allTrx, List<string> allLoanTrxId)
        {

            try
            {
                if (!IsDayInitiated)
                {
                    return Json(new { Result = "ERROR", Message = "Please run the start work process" });
                }

                savingCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                var trx = allTrx;

                var trxId = 1;
                var loanTrxIds = allLoanTrxId.Where(w => int.TryParse(w, out trxId));

                var savingTrxViewCollection = new List<DailySavingTrx>();
                foreach (var id in loanTrxIds)
                {
                    var BalanceId = "Balance" + id;

                    var SavingInstallmentId = "SavingInstallment" + id;

                    var WithdrawalId = "Withdrawal" + id;

                    var DepositId = "Deposit" + id;

                    var PenaltyId = "Penalty" + id;

                    decimal balance = 0;
                    decimal savingInstallment = 0;
                    decimal withdrawal = 0;
                    decimal deposit = 0;
                    decimal penalty = 0;

                    if (allTrx.ContainsKey(BalanceId))
                        decimal.TryParse(allTrx[BalanceId], out balance);
                    if (allTrx.ContainsKey(SavingInstallmentId))
                        decimal.TryParse(allTrx[SavingInstallmentId], out savingInstallment);
                    if (allTrx.ContainsKey(WithdrawalId))
                        decimal.TryParse(allTrx[WithdrawalId], out withdrawal);
                    if (allTrx.ContainsKey(DepositId))
                        decimal.TryParse(allTrx[DepositId], out deposit);
                    if (allTrx.ContainsKey(PenaltyId))
                        decimal.TryParse(allTrx[PenaltyId], out penalty);

                    var savingTrx = new DailySavingTrx() { DailySavingTrxID = long.Parse(id), Balance = balance, Deposit = deposit, SavingInstallment = savingInstallment, Withdrawal = withdrawal, Penalty = penalty };
                    savingTrxViewCollection.Add(savingTrx);

                }

                savingCollectionService.SaveDailysavingCollection(savingTrxViewCollection);

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        // GET: DailySavingCollection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: DailySavingCollection/Create
        public ActionResult Create()
        {
            var model = new DailySavingCollectionViewModel();
            if (IsDayInitiated)
            {
                model.TransactionDate = TransactionDate;
            }


            MapDropDownList(model);

            return View(model);
        }
        // POST: DailySavingCollection/Create
        [HttpPost]
        public ActionResult Create(DailySavingCollectionViewModel model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                var entity = Mapper.Map<DailySavingCollectionViewModel, DailySavingTrx>(model);

                 if (ModelState.IsValid)
                {
                    savingCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                    var summary = savingCollectionService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.MemberID == entity.MemberID && s.ProductID == entity.ProductID && s.NoOfAccount == entity.NoOfAccount && s.IsActive == 1).FirstOrDefault();

                       if (summary != null)
                    {

                        var getLoanCol = savingCollectionService.GetByIdLong(Convert.ToInt64(summary.DailySavingTrxID));
                        getLoanCol.SavingInstallment = entity.SavingInstallment;
                        getLoanCol.Withdrawal = entity.Withdrawal;
                        getLoanCol.Balance = 0;
                        getLoanCol.Deposit = 0;
                        getLoanCol.DueSavingInstallment = 0;
                        getLoanCol.MonthlyInterest = 0;
                        getLoanCol.Penalty = entity.Penalty;
                        getLoanCol.TransferDeposit = 0;
                        getLoanCol.TransferWithdrawal = 0;
                        getLoanCol.TransactionDate = entity.TransactionDate;
                        getLoanCol.TransType = 11;
                        getLoanCol.OrgID = Convert.ToInt32(LoggedInOrganizationID);
                        var errors = savingCollectionService.IsValidLoan(getLoanCol);

                        if (errors.ToList().Count == 0)
                        {

                            savingCollectionService.Create(getLoanCol);
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
        // GET: DailySavingCollection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: DailySavingCollection/Edit/5
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
        // GET: DailySavingCollection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: DailySavingCollection/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

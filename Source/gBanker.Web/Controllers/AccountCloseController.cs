using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using Microsoft.AspNet.Identity.EntityFramework;
namespace gBanker.Web.Controllers
{
    public class AccountCloseController : BaseController
    {
        private readonly IAccountCloseService accountCLoseService;
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly ICenterService centerService;
        private readonly IProductService productService;
        private readonly ISavingSummaryService savingSummaryService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ISpecialSavingCollectionService specialSavingCollectionService;
        public AccountCloseController(ISpecialSavingCollectionService specialSavingCollectionService, IAccountCloseService accountCLoseService, IUltimateReportService ultimateReportService, IOfficeService officeService, IMemberService memberService, ICenterService centerService, IProductService productService, ISavingSummaryService savingSummaryService)
        {
            this.accountCLoseService = accountCLoseService;
            this.officeService = officeService;
            this.memberService = memberService;
            this.centerService = centerService;
            this.productService = productService;
            this.savingSummaryService = savingSummaryService;
            this.ultimateReportService = ultimateReportService;
            this.specialSavingCollectionService = specialSavingCollectionService;
        }
        public JsonResult GetProductList(string Member_id, string center_id)
        {
            List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
            var param = new { OfficeId = LoginUserOfficeID, MemberId = Member_id, CenterId = center_id };
            var div_items = ultimateReportService.GetProductSavingList(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new ProductViewModel
            {
                ProductID = row.Field<Int16>("ProductID"),
                ProductCode = row.Field<string>("ProductCode"),
                ProductName = row.Field<string>("ProductName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductCode.ToString() + " " + x.ProductName.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetConsentProductList(string Member_id, string center_id)
        {
            List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
            var param = new { OfficeId = LoginUserOfficeID, MemberId = Member_id, CenterId = center_id };
            var div_items = ultimateReportService.GetConsentProductSavingList(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new ProductViewModel
            {
                ProductID = row.Field<Int16>("ProductID"),
                ProductCode = row.Field<string>("ProductCode"),
                ProductName = row.Field<string>("ProductName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductCode.ToString() + " " + x.ProductName.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountNo(string officeId, string MemId, string centerId, String ProdId)
        {
            List<SavingSummaryViewModel> List_ProductViewModel = new List<SavingSummaryViewModel>();
            var param = new { OfficeId = LoginUserOfficeID, MemberId = MemId, CenterId = centerId, ProductID = ProdId };
            var div_items = ultimateReportService.GetNoAccount(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new SavingSummaryViewModel
            {
                NoOfAccount = row.Field<int>("NoOfAccount"),

            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.NoOfAccount.ToString(),
                Text = x.NoOfAccount.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }
        private void MapDropDownList(AccountCloseViewModel model)
        {

            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }


            //var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));
            //var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, m.FirstName), Value = m.MemberID.ToString() });
            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allcenter = centerService.GetByOfficeId(LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });
            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll().Where(a => a.OfficeID == LoginUserOfficeID.Value && a.OrgID == LoggedInOrganizationID.Value);
            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });
            model.officeListItems = viewOffice;

            var allSearchProd = productService.SearchProduct(0, Convert.ToInt32(LoggedInOrganizationID), "S");
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });
            //var proditems = new List<SelectListItem>();
            //proditems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //proditems.AddRange(viewProdList);
            //model.productListItems = proditems;
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.productListItems = proditems;

        }
        public JsonResult GetStopSavingBalance(string officeId, string centerId, string MemId, string ProdId, string NoOfAccount)
        {
            int vLoanTerm;
            int Loantrm = 0;
            decimal vBalance = 0;

            decimal savInsall = 0;
            decimal with = 0;
            decimal penal = 0;

            var model = new SpecialSavingCollectionViewModel();
            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt16(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);


            var entity = Mapper.Map<SpecialSavingCollectionViewModel, DailySavingTrx>(model);

            List<Proc_get_SavingLastBalance> getBal = new List<Proc_get_SavingLastBalance>();
            var param = new
            {
                @OfficeID = LoginUserOfficeID,
                @CenterID = Convert.ToInt32(entity.CenterID),
                @MemberID = Convert.ToInt64(entity.MemberID),
                @ProductID = Convert.ToInt16(entity.ProductID),
                @NoOfAccount = Convert.ToInt16(NoOfAccount),

                @DailySavinInstallment = Convert.ToDecimal(entity.SavingInstallment),
                @WithDrawal = Convert.ToDecimal(entity.Withdrawal),
                @lcl_BusinessDate = TransactionDate,
                @TransType = Convert.ToInt16(entity.TransType)

            };
            var div_items = ultimateReportService.GetSpecialSavingLastBalance(param);
            getBal = div_items.Tables[0].AsEnumerable()
           .Select(row => new Proc_get_SavingLastBalance
           {
               officeid = row.Field<Int32>("officeid"),
               Centerid = row.Field<Int32>("Centerid"),
               MemberID = row.Field<long>("MemberID"),
               ProductID = row.Field<short>("ProductID"),
               NoOfAccount = row.Field<Int32>("NoOfAccount"),
               SavingSummaryID = row.Field<long>("SavingSummaryID"),
               Balance = row.Field<decimal>("Balance"),
               SavingInstallment = row.Field<decimal>("SavingInstallment")
           }).ToList();
            if (getBal.Count() > 0)
            {
                var getdetail = getBal.ToList();
                var GetBalance = getdetail.Where(g => g.officeid == Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();
                //savInsall = GetBalance.SavingInstallment;
                savInsall = 0;
                with = 0;
                penal = GetBalance.Penalty;
                //Loantrm = sm.NoOfAccount;
                Loantrm = GetBalance.NoOfAccount;
                vBalance = (Convert.ToDecimal(GetBalance.Balance) + savInsall + penal) - with;

                vLoanTerm = Loantrm;

            }
            else
            {
                savInsall = Convert.ToDecimal(0.00);
                with = Convert.ToDecimal(0.00);
                penal = Convert.ToDecimal(0.00);

                vLoanTerm = 0;
                vBalance = Convert.ToDecimal(0.00);
                vLoanTerm = 0;

            }
            var result = new { LoanTerm = vLoanTerm.ToString(), Balance = vBalance, savInstall = savInsall.ToString(), withdrawal = with.ToString(), penalty = penal.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNoOfAccount(string officeId, string centerId, string MemId, string ProdId)
        {
            int vLoanTerm;
            int Loantrm = 0;
            decimal vBalance = 0;

            decimal savInsall = 0;
            decimal with = 0;
            decimal penal = 0;

            var model = new SpecialSavingCollectionViewModel();
            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt16(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);


            var entity = Mapper.Map<SpecialSavingCollectionViewModel, DailySavingTrx>(model);
            var mlt = specialSavingCollectionService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.CenterID == entity.CenterID && s.MemberID == entity.MemberID && s.ProductID == entity.ProductID && s.NoOfAccount == entity.NoOfAccount && s.IsActive == 1).FirstOrDefault();
            if (mlt != null)
            {


                var getBal = specialSavingCollectionService.getSavingLastBalance(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(entity.CenterID), Convert.ToInt64(entity.MemberID), Convert.ToInt16(entity.ProductID), Convert.ToInt16(mlt.NoOfAccount), Convert.ToDecimal(entity.SavingInstallment), Convert.ToDecimal(entity.Withdrawal), TransactionDate, Convert.ToInt16(entity.TransType));
                var getdetail = getBal.ToList();
                var GetBalance = getdetail.Where(g => g.officeid == Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();

                // vLoanInstallment = (mlt.Balance + savInsall + penal) - with;
                //  vLoanInstallment = Convert.ToDecimal(GetBalance.Balance);



                savInsall = mlt.SavingInstallment;
                with = mlt.Withdrawal;
                penal = mlt.Penalty;
                Loantrm = mlt.NoOfAccount;
                //vBalance = (mlt.Balance + savInsall + penal) - with;
                vBalance = (Convert.ToDecimal(GetBalance.Balance) + savInsall + penal) - with;

            }
            else
            {
                var sm = savingSummaryService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.CenterID == entity.CenterID && s.MemberID == entity.MemberID && s.ProductID == entity.ProductID && s.SavingStatus == 1 && s.IsActive == true).FirstOrDefault();
                if (sm != null)
                {
                    var getBal = specialSavingCollectionService.getSavingLastBalance(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(entity.CenterID), Convert.ToInt64(entity.MemberID), Convert.ToInt16(entity.ProductID), Convert.ToInt16(sm.NoOfAccount), Convert.ToDecimal(entity.SavingInstallment), Convert.ToDecimal(entity.Withdrawal), TransactionDate, Convert.ToInt16(entity.TransType));
                    var getdetail = getBal.ToList();
                    var GetBalance = getdetail.Where(g => g.officeid == Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();


                    savInsall = sm.SavingInstallment;
                    with = 0;
                    penal = 0;
                    Loantrm = sm.NoOfAccount;
                    //vBalance = (sm.Balance + savInsall + penal) - with;
                    vBalance = (Convert.ToDecimal(GetBalance.Balance) + savInsall + penal) - with;
                }
            }
            //Session[ProductSessionKey] = pbr;
            vLoanTerm = Loantrm;
            vBalance = vBalance;

            var result = new { LoanTerm = vLoanTerm.ToString(), Balance = vBalance, savInstall = savInsall.ToString(), withdrawal = with.ToString(), penalty = penal.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetAccountClose(int jtStartIndex, int jtPageSize)
        {
            try
            {


                var getBal = accountCLoseService.getLastDayEndDate(Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();

                //  TransactionDate = getBal.Column1;

                var specialLoandetail = savingSummaryService.GetSavingAccountCloseInfo(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value), getBal.BusinessDate);
                var detail = specialLoandetail.ToList();


                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<getSavingCloseAccountInfo_Result>, IEnumerable<AccountCloseViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });



            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult GetStopInterest(int jtStartIndex, int jtPageSize)
        {
            try
            {


                var getBal = accountCLoseService.getLastDayEndDate(Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();
                var specialLoandetail = savingSummaryService.GetSavingStopInterestInfo(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value), getBal.BusinessDate);
                var detail = specialLoandetail.ToList();
                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<getSavingCloseAccountInfo_Result>, IEnumerable<AccountCloseViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });



            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        [HttpPost]
        public ActionResult GetClaimableInterest(int jtStartIndex, int jtPageSize)
        {
            try
            {


                var getBal = accountCLoseService.getLastDayEndDate(Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();

                //  TransactionDate = getBal.Column1;

                var specialLoandetail = savingSummaryService.GetClaimableStopInterestInfo(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value), getBal.BusinessDate);
                var detail = specialLoandetail.ToList();


                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<getSavingCloseAccountInfo_Result>, IEnumerable<AccountCloseViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });



            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

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
                //Session[MemberByCenterSessionKey] = List_MemberViewModel;
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            //var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, m.FirstName).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, m1.FirstName) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        // GET: AccountClose
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StopInterestIndex()
        {
            return View();
        }
        public ActionResult ClaimableInterestIndex()
        {
            return View();
        }
        // GET: AccountClose/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountClose/Create
        public ActionResult Create()
        {
            var model = new AccountCloseViewModel();
            if (IsDayInitiated)
                model.TransactionDate = TransactionDate;
            MapDropDownList(model);
            return View(model);
        }
        public ActionResult StopInterestCreate()
        {
            var model = new AccountCloseViewModel();
            if (IsDayInitiated)
                model.TransactionDate = TransactionDate;
            MapDropDownList(model);
            return View(model);
        }
        public ActionResult ClaimableInterestCreate()
        {
            var model = new AccountCloseViewModel();
            if (IsDayInitiated)
                model.TransactionDate = TransactionDate;
            MapDropDownList(model);
            return View(model);
        }
        // POST: AccountClose/Create
        [HttpPost]
        public ActionResult Create(AccountCloseViewModel model)
        {
            try
            {

                var entity = Mapper.Map<AccountCloseViewModel, SavingSummary>(model);

                //Add Validlation Logic.
                if (ModelState.IsValid)
                {
                    var getBal = accountCLoseService.getLastDayEndDate(Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();
                    savingSummaryService.AccountClose(LoggedInOrganizationID, LoginUserOfficeID.Value, entity.CenterID, entity.MemberID, entity.ProductID, entity.NoOfAccount, getBal.BusinessDate);
                    var emtpy = new AccountCloseViewModel();
                    MapDropDownList(emtpy);
                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        [HttpPost]
        public ActionResult StopInterestCreate(AccountCloseViewModel model)
        {
            try
            {
                var entity = Mapper.Map<AccountCloseViewModel, SavingSummary>(model);
                //Add Validlation Logic.
                if (ModelState.IsValid)
                {


                    DataSet LoanInstallMent;
                    if (LoggedInOrganizationID == 12)
                    {
                        var paramSLC = new { @OfficeID = SessionHelper.LoginUserOfficeID, @MemberID = entity.MemberID };
                        LoanInstallMent = ultimateReportService.GetStopClaimableInterestValidationGUK(paramSLC);



                        if (LoanInstallMent.Tables[0].Rows.Count > 0)
                        {
                            var getBal = accountCLoseService.getLastDayEndDate(Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();
                            savingSummaryService.StopInterestAccount(LoggedInOrganizationID, LoginUserOfficeID.Value, entity.CenterID, entity.MemberID, entity.ProductID, entity.NoOfAccount, getBal.BusinessDate, 1, entity.SavingSummaryID, 1, Convert.ToString(LoggedInEmployeeID));
                            var emtpy = new AccountCloseViewModel();
                            MapDropDownList(emtpy);
                            return GetSuccessMessageResult();

                        }
                        else
                        {
                            return GetErrorMessageResult("Invalid MemberID");
                            //var getBal = accountCLoseService.getLastDayEndDate(Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();
                            //savingSummaryService.StopInterestAccount(LoggedInOrganizationID, LoginUserOfficeID.Value, entity.CenterID, entity.MemberID, entity.ProductID, entity.NoOfAccount, getBal.BusinessDate, 1, entity.SavingSummaryID, 1, Convert.ToString(LoggedInEmployeeID));
                            //var emtpy = new AccountCloseViewModel();
                            //MapDropDownList(emtpy);
                            //return GetSuccessMessageResult();
                        }

                    }
                    else
                    {
                        var getBal = accountCLoseService.getLastDayEndDate(Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();
                        savingSummaryService.StopInterestAccount(LoggedInOrganizationID, LoginUserOfficeID.Value, entity.CenterID, entity.MemberID, entity.ProductID, entity.NoOfAccount, getBal.BusinessDate, 1, entity.SavingSummaryID, 1, Convert.ToString(LoggedInEmployeeID));
                        var emtpy = new AccountCloseViewModel();
                        MapDropDownList(emtpy);
                        return GetSuccessMessageResult();
                    }

                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        [HttpPost]
        public ActionResult ClaimableInterestCreate(AccountCloseViewModel model)
        {
            try
            {

                var entity = Mapper.Map<AccountCloseViewModel, SavingSummary>(model);

                //Add Validlation Logic.
                if (ModelState.IsValid)
                {
                    var getBal = accountCLoseService.getLastDayEndDate(Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();
                    savingSummaryService.ClaimableInterestAccount(LoggedInOrganizationID, LoginUserOfficeID.Value, entity.CenterID, entity.MemberID, entity.ProductID, entity.NoOfAccount, getBal.BusinessDate, 1, entity.SavingSummaryID, 1, Convert.ToString(LoggedInEmployeeID));
                    var emtpy = new AccountCloseViewModel();
                    MapDropDownList(emtpy);
                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();
            }

            catch (Exception ex)
            {

                return GetErrorMessageResult(ex);

            }
        }

        // GET: AccountClose/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountClose/Edit/5
        [HttpPost]
        public ActionResult Edit(AccountCloseViewModel model)
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
        [HttpPost]
        public ActionResult StopInterestEdit(AccountCloseViewModel model)
        {
            try
            {
                // TODO: Add update logic here

                var entity = Mapper.Map<AccountCloseViewModel, SavingSummary>(model);

                //Add Validlation Logic.
                if (ModelState.IsValid)
                {

                    var getBal = accountCLoseService.getLastDayEndDate(Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();
                    savingSummaryService.StartInterestAccount(LoggedInOrganizationID, LoginUserOfficeID.Value, entity.CenterID, entity.MemberID, entity.ProductID, entity.NoOfAccount, getBal.BusinessDate, 0, entity.SavingSummaryID, 1, Convert.ToString(LoggedInEmployeeID));
                    var emtpy = new AccountCloseViewModel();
                    MapDropDownList(emtpy);
                    return GetSuccessMessageResult();

                }
                return GetErrorMessageResult();
                // TODO: Add delete logic here
            }
            catch
            {
                return View();
            }
        }
        // GET: AccountClose/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountClose/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult StopInterestDelete(int id, AccountCloseViewModel model)
        {
            try
            {
                var entity = Mapper.Map<AccountCloseViewModel, SavingSummary>(model);

                //Add Validlation Logic.
                if (ModelState.IsValid)
                {

                    var getBal = accountCLoseService.getLastDayEndDate(Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();
                    savingSummaryService.StartInterestAccount(LoggedInOrganizationID, LoginUserOfficeID.Value, entity.CenterID, entity.MemberID, entity.ProductID, entity.NoOfAccount, getBal.BusinessDate, 0, entity.SavingSummaryID, 1, Convert.ToString(LoggedInEmployeeID));
                    var emtpy = new AccountCloseViewModel();
                    MapDropDownList(emtpy);
                    return GetSuccessMessageResult();

                }
                return GetErrorMessageResult();
                // TODO: Add delete logic here


            }
            catch
            {
                return View();
            }
        }

        // Consent Form

        public ActionResult ConsentForm()
        {
            var model = new AccountCloseViewModel();
            if (IsDayInitiated)
                model.TransactionDate = TransactionDate;
            MapDropDownList(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult ConsentForm(AccountCloseViewModel model)
        {
            try
            {
                var entity = Mapper.Map<AccountCloseViewModel, SavingSummary>(model);
                if (ModelState.IsValid)
                {
                    var getBal = accountCLoseService.getLastDayEndDate(Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();
                    savingSummaryService.InsertConsentForm(LoggedInOrganizationID, LoginUserOfficeID.Value, entity.CenterID, entity.MemberID, entity.ProductID, entity.NoOfAccount, getBal.BusinessDate, 1, entity.SavingSummaryID, 1, Convert.ToString(LoggedInEmployeeID));
                    var emtpy = new AccountCloseViewModel();
                    MapDropDownList(emtpy);
                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
    }
}

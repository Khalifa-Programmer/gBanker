using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class SavingsBankInterestUpdateController : BaseController
    {
        private readonly ICenterService centerService;
        private readonly ISpecialSavingCollectionService specialSavingCollectionService;
        private readonly IBranchService branchService;
        private readonly IOfficeService officeService;

        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;

        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        private readonly ISavingSummaryService savingSummaryService;
        private readonly IAccTrxMasterService accMasterService;
        private readonly IAccTrxDetailService accDetailService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ILoanCollectionReportService loanCollectionReportService;
        private readonly ILoanCollectionService loanCollectionService;
        private readonly ISavingCollectionService savingCollectionService;

        // GET: SavingsBankInterestUpdate

        public SavingsBankInterestUpdateController(ISpecialSavingCollectionService specialSavingCollectionService, ISavingSummaryService savingSummaryService, ICenterService centerService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, IPurposeService purposeService, IMemberService memberService, IAccTrxMasterService accMasterService, IAccTrxDetailService accDetailService, IUltimateReportService ultimateReportService, ILoanCollectionReportService loanCollectionReportService, ILoanCollectionService loanCollectionService, ISavingCollectionService savingCollectionService)
        {
            this.specialSavingCollectionService = specialSavingCollectionService;
            this.centerService = centerService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.officeService = officeService;
            this.purposeService = purposeService;
            this.memberService = memberService;
            this.savingSummaryService = savingSummaryService;
            this.accMasterService = accMasterService;
            this.accDetailService = accDetailService;
            this.ultimateReportService = ultimateReportService;
            this.loanCollectionReportService = loanCollectionReportService;
            this.loanCollectionService = loanCollectionService;
            this.savingCollectionService = savingCollectionService;

        }
        [HttpPost]
        public ActionResult GetSpecialCollection(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {


                string vday = string.Empty;
                if (IsDayInitiated)
                    vday = TransactionDay;
                var specialLoandetail = specialSavingCollectionService.GetSpecialsavingCollectionDetail(LoggedInOrganizationID, SessionHelper.LoginUserOfficeID.Value, vday, filterColumn, filterValue);
                var detail = specialLoandetail.ToList();

                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<proc_get_SpecialSavingCollection_Result>, IEnumerable<SpecialSavingCollectionViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

                //var viewploansummary = Mapper.Map<IEnumerable<proc_get_SpecialSavingCollection_Result>, IEnumerable<SpecialSavingCollectionViewModel>>(detail);
                //return Json(new { Result = "OK", Records = viewploansummary });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult GetMemberBalanceInfoByProduct(string officeId, string centerId, string MemId, string ProdId)
        {

            int vLoanTerm;
                int Loantrm = 0;
            decimal vMonthlyInterest = 0;

            decimal savInsall = 0;
            decimal with = 0;
            decimal penal = 0;
            decimal CumInterest = 0;

            var model = new SpecialSavingCollectionViewModel();
            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt32(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);


            var entity = Mapper.Map<SpecialSavingCollectionViewModel, DailySavingTrx>(model);


            List<Proc_get_SavingLastBalance> getBal = new List<Proc_get_SavingLastBalance>();
            var param = new
            {
                @Org = LoggedInOrganizationID.Value,
                @Office = LoginUserOfficeID,
                @Center = Convert.ToInt32(entity.CenterID),
                @Member = Convert.ToInt64(entity.MemberID),
                @DateTo = DateTime.Now,
                @productId = Convert.ToInt32(ProdId),

            };
            var div_items = ultimateReportService.GetMemberBalanceInfoByProduct(param);
            getBal = div_items.Tables[0].AsEnumerable()
           .Select(row => new Proc_get_SavingLastBalance
           {
               officeid = row.Field<Int32>("officeid"),
               Centerid = row.Field<Int32>("Centerid"),
               CumInterest = row.Field<decimal>("bankinterest"),

           }).ToList();
            if (getBal.Count() > 0)
            {
                var getdetail = getBal.ToList();
                var GetBalance = getdetail.Where(g => g.officeid == Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();
                //savInsall = GetBalance.SavingInstallment;

                vMonthlyInterest = (Convert.ToDecimal(GetBalance.CumInterest));
            }
            else
            {
                vMonthlyInterest = Convert.ToDecimal(0.00);
            }

            var result = new { MonthlyInterest = vMonthlyInterest };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductList(string Member_id, string center_id)
        {
            List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
            DataSet div_items;
            var param = new { OfficeId = LoginUserOfficeID, MemberId = Member_id, CenterId = center_id };
            if (LoggedInOrganizationID == 105)
            {
                var paramd = new { OfficeId = LoginUserOfficeID, MemberId = Member_id, CenterId = center_id, EmpId = LoggedInEmployeeID };
                div_items = ultimateReportService.GetProductSavingList_SDC(paramd);
            }
            else
            div_items = ultimateReportService.GetProductSavingList(param);

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
        public ActionResult GetInstallment(decimal SavingInstallment, decimal WithDrawal, decimal Penalty, decimal balance, string officeId, string centerId, string MemId, string ProdId, string noAccount)
        {

            decimal vLoanInstallment = 0;
            decimal savInsall;
            decimal with = 0;
            decimal penal = 0;
            decimal vWithDrawal;
            decimal vBal = 0;
            decimal CumInterest = 0;
            var model = new SpecialSavingCollectionViewModel();



            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt16(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);
            model.NoOfAccount = Convert.ToInt16(noAccount);
            var entity = Mapper.Map<SpecialSavingCollectionViewModel, DailySavingTrx>(model);
            savInsall = SavingInstallment;
            with = WithDrawal;
            penal = Penalty;


            var getBal = specialSavingCollectionService.Proc_getSavingLastBalance(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(entity.CenterID), Convert.ToInt64(entity.MemberID), Convert.ToInt16(entity.ProductID), Convert.ToInt16(noAccount), Convert.ToDecimal(entity.SavingInstallment), Convert.ToDecimal(entity.Withdrawal), TransactionDate, Convert.ToInt16(entity.TransType));
            var getdetail = getBal.ToList();
            var GetBalance = getdetail.Where(g => g.officeid == Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();


            //savInsall = sm.SavingInstallment;
            savInsall = SavingInstallment;
            with = WithDrawal;
            penal = Penalty;
            //Loantrm = sm.NoOfAccount;

            vLoanInstallment = (Convert.ToDecimal(GetBalance.Balance) + savInsall + Penalty - with);
            savInsall = 0;
            CumInterest = GetBalance.CumInterest;
            // var result = new { loan = vLoanInstallment.ToString(), savInstall = savInsall.ToString(), withdrawal = vWithDrawal.ToString(), penalty = penal.ToString() };
            var result = new { loan = vLoanInstallment.ToString(), savInstall = savInsall.ToString(), withdrawal = with.ToString(), penalty = penal.ToString(),CumInterest= CumInterest.ToString() };
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
            decimal CumInterest = 0;

            var model = new SpecialSavingCollectionViewModel();
            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt32(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);


            var entity = Mapper.Map<SpecialSavingCollectionViewModel, DailySavingTrx>(model);


            //var getBal = specialSavingCollectionService.Proc_getSavingLastBalance(LoginUserOfficeID, entity.CenterID, Convert.ToInt64(entity.MemberID), entity.ProductID, 0, Convert.ToDecimal(entity.SavingInstallment), Convert.ToDecimal(entity.Withdrawal), TransactionDate, Convert.ToInt16(entity.TransType));
            //var getdetail = getBal.ToList();
            //var GetBalance = getdetail.Where(g => g.officeid == Convert.ToInt16(LoginUserOfficeID)).FirstOrDefault();

            List<Proc_get_SavingLastBalance> getBal = new List<Proc_get_SavingLastBalance>();
            var param = new
            {
                @OfficeID = LoginUserOfficeID,
                @CenterID = Convert.ToInt32(entity.CenterID),
                @MemberID = Convert.ToInt64(entity.MemberID),
                @ProductID = Convert.ToInt16(entity.ProductID),
                @NoOfAccount = Convert.ToInt16(model.NoOfAccount),

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
               SavingInstallment = row.Field<decimal>("SavingInstallment"),
               CumInterest = row.Field<decimal>("CumInterest")
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
                CumInterest = GetBalance.CumInterest;
            }
            else
            {
                savInsall = Convert.ToDecimal(0.00);
                with = Convert.ToDecimal(0.00);
                penal = Convert.ToDecimal(0.00);

                vLoanTerm = 0;
                vBalance = Convert.ToDecimal(0.00);
                vLoanTerm = 0;
                CumInterest = 0;

            }
            ////savInsall = sm.SavingInstallment;
            //savInsall = GetBalance.SavingInstallment;
            //with = 0;
            //penal = 0;
            ////Loantrm = sm.NoOfAccount;
            //Loantrm = GetBalance.NoOfAccount;
            //vBalance = (Convert.ToDecimal(GetBalance.Balance) + savInsall + penal) - with;

            //vLoanTerm = Loantrm;
            //vBalance = vBalance;
            //savInsall = 0;
            var result = new { LoanTerm = vLoanTerm.ToString(), Balance = vBalance, savInstall = savInsall.ToString(), withdrawal = with.ToString(), penalty = penal.ToString(),CumInterest=CumInterest.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);


        }
        private void MapDropDownList(SpecialSavingCollectionViewModel model)
        {

            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var Transtype = new List<SelectListItem>();
            //Transtype.Add(new SelectListItem() { Text = "Cash", Value = "20", Selected = true });
            Transtype.Add(new SelectListItem() { Text = "Transfer", Value = "23" });

            model.cashListItems = Transtype.AsEnumerable();

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allpurpose = purposeService.SearchPurpose(Convert.ToInt16(LoggedInOrganizationID));

            var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });

            model.purposeListItems = viewPurpose;


            string vCoday = TransactionDay;

            //// var allcenter = centerService.SearchSpecialCenter(vCoday, SessionHelper.LoginUserOfficeID.Value);
            //var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID));

            //var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            //model.centerListItems = viewCenter;

            var param1 = new { @EmpID = LoggedInEmployeeID };
            var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);

            IEnumerable<Center> allcenter;
            if (LoanInstallMent != null)
            {
                var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
                if (empType == "FO")
                {
                    allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt16(LoggedInEmployeeID));
                }
                else

                    allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));
            }

            else
                allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));


            //var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoggedInEmployeeID));

            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

            var allSearchProd = productService.SearchProduct(0, Convert.ToInt16(LoggedInOrganizationID), "S");
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.productListItems = proditems;
            //var proditems = new List<SelectListItem>();
            //proditems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //proditems.AddRange(viewProdList);
            //model.productListItems = proditems;

            var allmembercategory = membercategoryService.GetAll().Where(m => m.IsActive == true && m.OrgID == LoggedInOrganizationID);

            var viewmembercategory = allmembercategory.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCategoryID, m.CategoryName), Value = m.MemberCategoryID.ToString() });

            model.membercategoryListItems = viewmembercategory;



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
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public Member GetMember(long memberid)
        {
            var mbr = memberService.GetByIdLong(memberid);
            return mbr;
        }
        public Center GetEmployee(int employeeid)
        {
            var mbr = centerService.GetById(employeeid);
            return mbr;
        }

        public Product GetProduct(int productid)
        {
            var mbr = productService.GetById(productid);
            return mbr;
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: SavingsBankInterestUpdate/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SavingsBankInterestUpdate/Create
        public ActionResult Create()
        {
            var model = new SpecialSavingCollectionViewModel();

            if (IsDayInitiated)
                model.TransactionDate = TransactionDate;
            MapDropDownList(model);

            return View(model);
        }

        // POST: SavingsBankInterestUpdate/Create
        [HttpPost]
        public ActionResult Create(SpecialSavingCollectionViewModel model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                specialSavingCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);

                var entity = Mapper.Map<SpecialSavingCollectionViewModel, DailySavingTrx>(model);

                //Add Validlation Logic.

                if (ModelState.IsValid)
                {


                    var member = GetMember(Convert.ToInt64(entity.MemberID));
                    int membercaregoryid = member.MemberCategoryID;

                    var employee = GetEmployee(entity.CenterID);
                    int employeeid = employee.EmployeeId;

                    var prod = GetProduct(entity.ProductID);
                    decimal intrate = Convert.ToDecimal(prod.InterestRate);
                    entity.EmployeeID = Convert.ToInt16(employeeid);
                    entity.MemberCategoryID = Convert.ToByte(membercaregoryid);
                    entity.MemberCode = member.MemberCode;
                    entity.MemberName = member.FirstName;
                    entity.ProductCode = prod.ProductCode;
                    entity.ProductName = prod.ProductName;
                    
                    var param1 = new { @OrgID = LoggedInOrganizationID, @OfficeID = LoginUserOfficeID, @MemberID = entity.MemberID, @CenterID = entity.CenterID, @ProductID = entity.ProductID, @NoOfAccount = entity.NoOfAccount };
                    var LoanInstallMent = ultimateReportService.ValidateSavingSummary(param1);
                    if (LoanInstallMent != null && LoanInstallMent.Tables.Count > 0 && LoanInstallMent.Tables[0].Rows.Count > 0)
                    {
                        entity.SavingSummaryID = Convert.ToInt64(LoanInstallMent.Tables[0].Rows[0]["SavingSummaryID"].ToString());

                        entity.Deposit = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["Deposit"].ToString());
                        entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);

                        var errors = specialSavingCollectionService.IsValidLoanSave(entity);

                        if (errors.ToList().Count == 0)
                        {

                            var param2 = new { @OfficeID = LoginUserOfficeID, @CenterID = entity.CenterID, @MemberID = entity.MemberID, @ProductID = entity.ProductID, @NoOfAccount = entity.NoOfAccount, @monthlyInt = entity.MonthlyInterest, @DateTo = TransactionDate };
                            var LoanInstallMent1 = ultimateReportService.ValidateSavingSummaryMonthlyInterest(param2);

                            var err= Convert.ToDecimal(LoanInstallMent1.Tables[0].Rows[0]["error"].ToString());
                            if (err == 1)
                            {
                                return GetErrorMessageResult();
                            }
                            else
                            {
                                entity.TransactionDate = TransactionDate;
                                entity.IsActive = 1;
                                specialSavingCollectionService.Create(entity);
                                specialSavingCollectionService.setUpdateSavingBalance(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(entity.CenterID), Convert.ToInt64(entity.MemberID), Convert.ToInt16(entity.ProductID), Convert.ToInt16(entity.NoOfAccount), Convert.ToDecimal(entity.MonthlyInterest));
                                return GetSuccessMessageResult();
                            }
                          

                        }
                        else
                            return GetErrorMessageResult();
                    }
                    //var summary = savingSummaryService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == LoginUserOfficeID && s.MemberID == entity.MemberID && s.CenterID == entity.CenterID && s.ProductID == entity.ProductID && s.NoOfAccount == entity.NoOfAccount && s.SavingStatus == 1 && s.IsActive == true).FirstOrDefault();
                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: SavingsBankInterestUpdate/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SavingsBankInterestUpdate/Edit/5
        [HttpPost]
        public ActionResult Edit(SpecialSavingCollectionViewModel Model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                specialSavingCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);

                var loanser = specialSavingCollectionService.GetByIdLong(Convert.ToInt64(Model.DailySavingTrxID));
               // Model.NoOfAccount = loanser.NoOfAccount;
                var entity = Mapper.Map<SpecialSavingCollectionViewModel, DailySavingTrx>(Model);
                //if (ModelState.IsValid)
                //{

                    var errors = specialSavingCollectionService.IsValidLoan(loanser);

                    if (errors.ToList().Count == 0)
                    {

                        loanser.MonthlyInterest = Model.MonthlyInterest;

                        specialSavingCollectionService.Update(loanser);
                        specialSavingCollectionService.setUpdateSavingBalance(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(loanser.CenterID), Convert.ToInt64(loanser.MemberID), Convert.ToInt16(loanser.ProductID), Convert.ToInt16(loanser.NoOfAccount), Convert.ToDecimal(loanser.MonthlyInterest));
                        return GetSuccessMessageResult();
                    }
                    else
                    return GetErrorMessageResult();
                //return Json(new { Result = "ERROR" });
                //}
                //return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        // GET: SavingsBankInterestUpdate/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SavingsBankInterestUpdate/Delete/5
        [HttpPost]
        public ActionResult Delete(int DailySavingTrxID, SpecialSavingCollectionViewModel model)
        {
            try
            {
                // TODO: Add delete logic here
                specialSavingCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                var param1 = new { @Qtype = 3, @DailyLoanTrxID = DailySavingTrxID, @OrgID = LoggedInOrganizationID };
                var LoanInstallMent = ultimateReportService.DelSpecialLoanCollection(param1);
                // TODO: Add delete logic here
                return Json(new { Result = "OK" });
               
            }
            catch
            {
                return View();
            }
        }

        #region SavingInterestUpdate
        public ActionResult SavingInterestUpdate()
        {
            var model = new DailySavingCollectionViewModel();

            MapDropDownList(model);

            //if (!IsDayInitiated)
            //{
            //    return Json(new { Result = "ERROR", Message = "Please run the start work process" });
            //}

            return View(model);
        }
        private void MapDropDownList(DailySavingCollectionViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }

            //  string vCoday = TransactionDay;

            //var allcenter = centerService.SearchOffCenter(vCoday, SessionHelper.LoginUserOfficeID.Value);
            //var allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value);

            //var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            //model.centerListItems = viewCenter;


            var param1 = new { @EmpID = LoggedInEmployeeID };
            var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);


            
            IEnumerable<Center> allcenter;
            if (LoanInstallMent != null)
            {
                var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
                if (empType == "FO")
                {
                    allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt16(LoggedInEmployeeID));
                }
                else

                    allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));
            }

            else
                allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));

            //if (LoanInstallMent != null)
            //{
            //    allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, LoggedInOrganizationID, Convert.ToInt16(LoggedInEmployeeID), LoanInstallMent.Tables[0].Rows[0]["Name"].ToString());
            //}

            //else
            //    allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, LoggedInOrganizationID, Convert.ToInt16(LoggedInEmployeeID), LoanInstallMent.Tables[0].Rows[0]["Name"].ToString());

            // var allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value,LoggedInOrganizationID,Convert.ToInt16(LoggedInEmployeeID));
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


            var allSearchProd = productService.SearchProduct(0, Convert.ToInt32(LoggedInOrganizationID), "S");
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


        public ActionResult GenerateReport(string fromDate, string toDate, string CenterID)
        {

            if (CenterID == "0")
            {
                var param = new { Qtype = 1, Org = LoggedInOrganizationID, Office = LoginUserOfficeID, Center = CenterID };
                var allproducts = loanCollectionReportService.GetDataSavingCollectionInfo(param);
                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("Header1", ApplicationSettings.OrganiztionName);
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptDailySavingsCollection.rpt", allproducts.Tables[0], reportParam);
            }
            else
            {
                var param = new { Qtype = 2, Org = LoggedInOrganizationID, Office = LoginUserOfficeID, Center = CenterID };
                var allproducts = loanCollectionReportService.GetDataSavingCollectionInfo(param);
                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("Header1", ApplicationSettings.OrganiztionName);
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptDailySavingsCollection.rpt", allproducts.Tables[0], reportParam);
            }


            return Content(string.Empty);
        }


        public ActionResult UpdateDataLessFiftyPercent(string officeId, string CenterID)
        {
            var result = loanCollectionService.setLoanAndSavingingLessFiftyPercent(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(CenterID), 2);
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ExecuteAppend(string officeId, string CenterID)
        {

            var result = "OK";
            var param1 = new
            {
                @OfficeId = SessionHelper.LoginUserOfficeID,
                @lcl_BusinessDate = TransactionDate,
                @CreateUser = LoggedInEmployeeID,
                @OrgID = SessionHelper.LoginUserOrganizationID
            };
            var LoanInstallMent = ultimateReportService.ExecuteSPBankINterestRate(param1);


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetDailySavingCollectionProductList(int jtStartIndex, int jtPageSize, string jtSorting, int centerId, string filterColumn, string filterValue, string append = "")
        {
            try
            {
                jtPageSize = 1;
                var collectionList = savingCollectionService.GetDailySavingBankInterestCollectionByCenter(centerId, filterColumn, filterValue).ToList();



                var products = new List<DailySavingCollectionViewModel>();
                foreach (var tr in collectionList)
                {
                    if (products.Where(p => p.ProductID == tr.ProductID && tr.CenterID == centerId).OrderBy(p => p.ProductCode).OrderBy(p => p.MemberCode).FirstOrDefault() == null)
                    {

                        var prodViewModel = Mapper.Map<DailySavingTrx, DailySavingCollectionViewModel>(tr);

                        prodViewModel.DueSavingSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.DueSavingInstallment);
                        prodViewModel.SavingCollectionSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.SavingInstallment);
                        prodViewModel.WithDrawalSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.Withdrawal);
                        prodViewModel.PenaltySummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.Penalty);
                        products.Add(prodViewModel);
                        //  products.Add(tr);
                    }

                }

                var currentPageProducts = products.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageProducts, TotalRecordCount = products.Count });
                //// var collectionList = savingCollectionService.GetDailySavingCollectionByCenter(centerId, filterColumn, filterValue).ToList();
                //var collectionList = savingCollectionService.GetDailySavingBankInterestCollectionByCenter(centerId, filterColumn, filterValue).ToList();

                //var products = new List<DailySavingCollectionViewModel>();
                //foreach (var tr in collectionList)
                //{
                //    if (products.Where(p => p.ProductID == tr.ProductID && tr.CenterID == centerId).OrderBy(p => p.ProductCode).OrderBy(p => p.MemberCode).FirstOrDefault() == null)
                //    {

                //        var prodViewModel = Mapper.Map<DailySavingTrx, DailySavingCollectionViewModel>(tr);

                //        prodViewModel.DueSavingSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.DueSavingInstallment);
                //        prodViewModel.SavingCollectionSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.SavingInstallment);
                //        prodViewModel.WithDrawalSummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.Withdrawal);
                //        prodViewModel.PenaltySummary = collectionList.Where(s => s.CenterID == tr.CenterID && s.ProductID == tr.ProductID).Sum(s => s.Penalty);
                //        products.Add(prodViewModel);
                //        //  products.Add(tr);
                //    }

                //}
                ////  var productModels = Mapper.Map<IEnumerable<DailySavingTrx>, IEnumerable<DailySavingCollectionViewModel>>(products);

                //var currentPageProducts = products.Skip(jtStartIndex).Take(jtPageSize);
                //return Json(new { Result = "OK", Records = currentPageProducts, TotalRecordCount = products.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }


        [HttpPost]
        public ActionResult SaveSavingTransaction(Dictionary<string, string> allTrx, List<string> allLoanTrxId, string CenterId, string ProductCode)
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



                    var MonthlyInterestId = "MonthlyInterest" + id;


                    var DepositId = "Deposit" + id;



                    decimal balance = 0;
                    decimal MonthlyInterest = 0;
                    decimal savingInstallment = 0;
                    decimal withdrawal = 0;
                    decimal deposit = 0;
                    decimal penalty = 0;

                    if (allTrx.ContainsKey(BalanceId))
                        decimal.TryParse(allTrx[BalanceId], out balance);

                    if (allTrx.ContainsKey(MonthlyInterestId))
                        decimal.TryParse(allTrx[MonthlyInterestId], out MonthlyInterest);
                    if (allTrx.ContainsKey(DepositId))
                        decimal.TryParse(allTrx[DepositId], out deposit);
       
                    var savingTrx = new DailySavingTrx() { DailySavingTrxID = long.Parse(id), Balance = balance, Deposit = deposit, MonthlyInterest = MonthlyInterest };
                    if (savingTrx.MonthlyInterest > 0)
                    {
                        savingTrxViewCollection.Add(savingTrx);
                    }
                }

                savingCollectionService.SaveMonthlyInterestCollection(savingTrxViewCollection);


                var param1 = new
                {
                    @OfficeId = SessionHelper.LoginUserOfficeID,
                    @CenterId = CenterId,
                    @lcl_BusinessDate = TransactionDate,
                    @CreateUser = LoggedInEmployeeID,
                    @OrgID = SessionHelper.LoginUserOrganizationID,
                    @ProductCode = ProductCode
                };
                var LoanInstallMent = ultimateReportService.ExecuteSPBankINterestClear(param1);



                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }


        [HttpPost]
        public ActionResult GetDailySavingCollectionSheet(int centerId, int productId, string filterColumn, string filterValue)
        {
            try
            {
                var collectionList = savingCollectionService.GetDailySavingBankInterestCollectionByCenterQueryable(centerId, filterColumn, filterValue);



                var members = collectionList.Where(c => c.ProductID == productId && c.CenterID == centerId).ToList().OrderBy(c => c.ProductCode).OrderBy(c => c.MemberCode);
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

            //    return Json(new { Result = "OK", Records = memberModels });
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { Result = "ERROR", Message = ex.Message });
            //}

        }




        #endregion SavingInterestUpdate






    }// END Class
}// End Namespace

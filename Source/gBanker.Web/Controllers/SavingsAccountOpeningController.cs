using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
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
    public class SavingsAccountOpeningController : BaseController
    {
        private readonly IOfficeService officeService;
        private readonly IEmployeeService employeeService;
        private readonly ISavingsAccountOpeningService savingsAccountOpeningService;
        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;
        private readonly ICenterService centerService;
        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        private readonly ISavingSummaryService savingsummaryService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IAccReportService accReportService;
        private readonly IDailySavingTrxService dailySavingTrxService;
            // GET: SavingSummary
        public SavingsAccountOpeningController(ISavingsAccountOpeningService savingsAccountOpeningService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, ICenterService centerService, IPurposeService purposeService, IMemberService memberService,ISavingSummaryService savingsummaryService,IUltimateReportService ultimateReportService, IAccReportService accReportService, IDailySavingTrxService dailySavingTrxService,IEmployeeService employeeService)
        {
            this.savingsAccountOpeningService = savingsAccountOpeningService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.officeService = officeService;
            this.centerService = centerService;
            this.purposeService = purposeService;
            this.memberService = memberService;
            this.savingsummaryService = savingsummaryService;
            this.ultimateReportService = ultimateReportService;
            this.accReportService = accReportService;
            this.dailySavingTrxService = dailySavingTrxService;
            this.employeeService = employeeService;
        }
        public JsonResult GetRate(int productid, long memberId, int centerID)
        {
            decimal vrate = 0;
            string vdate ;

            var pbr = productService.GetById(productid);
            var mbr = memberService.GetByIdLong(memberId);
            vrate = Convert.ToDecimal(pbr.InterestRate);
            //vdate = mbr.JoinDate.ToString("dd-MMM-yyyy");
            vdate = TransactionDate.ToString("dd-MMM-yyyy"); 
            int vLoanTerm;
            int Loantrm = 0;
            decimal vBalance = 0;

            decimal savInsall = 0;
            decimal with = 0;
            decimal penal = 0;
            var prCode = "";
            var model = new SavingsAccountOpeningViewModel();
            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt32(centerID);
            model.MemberID = Convert.ToInt64(memberId);
            model.ProductID = Convert.ToInt16(productid);


            var entity = Mapper.Map<SavingsAccountOpeningViewModel, DBSavingSummaryDetails>(model);


            if (LoggedInOrganizationID == 54)
            {
                var param = new { OfficeID = LoginUserOfficeID, MemberID = entity.MemberID, ProductId = entity.ProductID };
                var div_items = ultimateReportService.GetMaxLoanTermMainCodeWise(param);
                vLoanTerm = Convert.ToInt16(div_items.Tables[0].Rows[0]["LoanTerm"].ToString());

            }
            else
            {
                var MaxNo = savingsummaryService.Get_MaxNoOfAccount(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt32(entity.CenterID), Convert.ToInt64(entity.MemberID), Convert.ToInt16(entity.ProductID));
                var hh = MaxNo.Where(o => o.officeID == LoginUserOfficeID).FirstOrDefault();
                Loantrm = Convert.ToInt16(hh.NoOfAccount);

                vLoanTerm = Loantrm;
            }

            
                vBalance = 0;


            var result = new { rate = vrate.ToString(), loanterm = vLoanTerm.ToString(), jdate = vdate };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private void MapDropDownList(SavingsAccountOpeningViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;



            //var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));

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


            //var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt16(LoggedInEmployeeID));
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;
            var allbranch = officeService.GetAll().Where(o => o.OfficeID == SessionHelper.LoginUserOfficeID.Value && o.OrgID==LoggedInOrganizationID);

            var viewbranch = allbranch.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewbranch;


            //var allSearchProd = productService.SearchProduct(0, Convert.ToInt32(LoggedInOrganizationID),"S");
            //var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.ProductID.ToString(),
            //    Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            //});
            //var proditems = new List<SelectListItem>();
            //proditems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //proditems.AddRange(viewProdList);
            //model.productListItems = proditems;

            List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
            var param = new { Prodtype=0, OrgID=LoggedInOrganizationID, ItemType="S", OfficeID = LoginUserOfficeID };
            var div_items = ultimateReportService.GetProductListAccordingToOffice(param);

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
            model.productListItems = d_items;

        }
        public JsonResult GetNoOfAccount(string officeId, string centerId, string MemId, string ProdId)
        {
            int vLoanTerm;
            int Loantrm = 0;
            decimal vBalance = 0;

            decimal savInsall = 0;
            decimal with = 0;
            decimal penal = 0;

            var model = new SavingsAccountOpeningViewModel();
            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt32(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);


            var entity = Mapper.Map<SavingsAccountOpeningViewModel, DBSavingSummaryDetails>(model);
           
                //var sm = savingSummaryService.GetAll().Where(s => s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.CenterID == model.CenterID && s.MemberID == model.MemberID && s.ProductID == model.ProductID && s.SavingStatus == 1).FirstOrDefault();
                var sm = savingsummaryService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.CenterID == entity.CenterID && s.MemberID == entity.MemberID && s.ProductID == entity.ProductID && s.SavingStatus == 1 && s.IsActive == true).FirstOrDefault();
                if (sm != null)
                {
                    savInsall = sm.SavingInstallment;
                    with = 0;
                    penal = 0;
                    vLoanTerm = sm.NoOfAccount;
                    vBalance = (sm.Balance + savInsall + penal) - with;

                }
                else
                    vLoanTerm = 1;
                vBalance =0;
           
            

            var result = new { LoanTerm = vLoanTerm.ToString(), Balance = vBalance, savInstall = savInsall.ToString(), withdrawal = with.ToString(), penalty = penal.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSavingAccountsOpening(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                var allSavingsummary = savingsAccountOpeningService.GetSavingSummaryDetail(Convert.ToInt16(LoggedInOrganizationID),SessionHelper.LoginUserOfficeID.Value);
                var totalCount = allSavingsummary.Count();
                var entities = allSavingsummary.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<DBSavingSummaryDetails>, IEnumerable<SavingSummaryViewModel>>(entities);
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
                var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json (members, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeList(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("EmployeeByOfficeSessionKey_{0}", LoginUserOfficeID);
            var memberList = new List<Employee>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Employee>;
            else
            {
                //var emr = employeeService.SearchEmployeeByOffice(Convert.ToInt16(LoginUserOfficeID)).ToList();
                var emr = employeeService.SearchEmployee().ToList();
                Session[MemberByCenterSessionKey] = emr;
                memberList = emr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.EmployeeCode, (string.IsNullOrEmpty(m.EmpName) ? "" : m.EmpName) ).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.EmployeeID, EmployeeName = string.Format("{0} - {1}", m1.EmployeeCode, (string.IsNullOrEmpty(m1.EmpName) ? "" : m1.EmpName) ) }).ToList();

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
        // GET: SavingsAccountOpening
        public ActionResult Index()
        {
            var businessDate = SessionHelper.TransactionDate.Year;

            // 01 - Jan - 0001

            ViewData["IsDayOpen"] = businessDate == 1 ? false : true;

            return View();

        }
        // GET: SavingsAccountOpening/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: SavingsAccountOpening/Create
        public ActionResult Create()
        {
            var model = new SavingsAccountOpeningViewModel();
          

            MapDropDownList(model);
            model.OpeningDate = TransactionDate;
            return View(model);
        }
        private string GetlastWorkingDay(int offc_id)
        {
            var param = new { OfficeID = offc_id };
            var workingDay = accReportService.LastWorkingDate(param);

            //var workingDt = Convert.ToDateTime(string.IsNullOrEmpty(workingDay.Tables[0].Rows[0]["Column1"].ToString()) ? DateTime.Now.ToString() : workingDay.Tables[0].Rows[0]["Column1"]);
            var workingDt = workingDay.Tables[0].Rows[0]["BusinessDate"].ToString();
            return workingDt;
        }
        // POST: SavingsAccountOpening/Create
        [HttpPost]
        public ActionResult Create(SavingsAccountOpeningViewModel model)
        {
            try
            {

                var entity = Mapper.Map<SavingsAccountOpeningViewModel, SavingSummary>(model);

                //Add Validlation Logic.

                if (ModelState.IsValid)
                {

                    
                    var member = GetMember(Convert.ToInt64(entity.MemberID));
                    int membercaregoryid = member.MemberCategoryID;

                    var employee = GetEmployee(entity.CenterID);
                    int employeeid = employee.EmployeeId;
                    entity.EmployeeId = Convert.ToInt16(employeeid);


                    entity.MemberCategoryID = Convert.ToByte(membercaregoryid);
                    //var mbr=memberService.GetById(employeeid);
                    var pbr = productService.GetById(entity.ProductID);

                    entity.IsActive = true;
                    entity.SavingStatus = 1;
                    entity.TransType = 2;
                    //entity.OpeningDate = mbr.JoinDate;
                    entity.TransactionDate = TransactionDate;



                    var param = new { @OfficeID = SessionHelper.LoginUserOfficeID, @MemberID = Convert.ToInt64(entity.MemberID), @ProductID = Convert.ToInt16(entity.ProductID) };
                    var getDD = ultimateReportService.ValidateMainItemCode_21_list(param);
                    if (getDD.Tables[0].Rows.Count>0)
                    {
                        return GetSuccessMessageResult();
                    }
                    entity.OrgID = Convert.ToInt32(LoggedInOrganizationID);

                    if (IsDayInitiated)
                    {
                        entity.OpeningDate = TransactionDate;
                        if (pbr.PaymentFrequency == "W")
                        {
                            entity.MaturedDate = entity.OpeningDate.AddMonths(Convert.ToInt16((pbr.Duration / 52)) * 12);
                        }
                        else
                            entity.MaturedDate = TransactionDate.AddMonths(Convert.ToInt16(pbr.Duration));

                    }
                    else
                    {
                        var param1 = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                        var allProducts = accReportService.GetLastInitialDate(param1);

                        var detail = allProducts.ToString();

                        if (!IsDayInitiated)
                        {
                            if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                            {
                                model.OpeningDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());

                                if (pbr.PaymentFrequency == "W")
                                {
                                    entity.MaturedDate = entity.OpeningDate.AddMonths(Convert.ToInt16((pbr.Duration / 52)) * 12);
                                }
                                else
                                    entity.MaturedDate = entity.OpeningDate.AddMonths(Convert.ToInt16(pbr.Duration));
                            }
                            else
                            {
                                entity.OpeningDate = System.DateTime.Now;
                                entity.MaturedDate = entity.OpeningDate.AddMonths(Convert.ToInt16(pbr.Duration));
                            }
                        
                        }
                        else
                        {
                            model.OpeningDate  = TransactionDate;
                        }

                    }

                    var errors = savingsAccountOpeningService.IsValidSaving(entity);
                    if (errors.ToList().Count == 0)
                    {


                        if (LoggedInOrganizationID == 6)
                        {
                            var LoanAccount = new { Qtype = 2, OfficeID = LoginUserOfficeID, MemberID = entity.MemberID, ProductID = entity.ProductID, loanTerm = entity.NoOfAccount };
                            var Loan_items = ultimateReportService.GenerateLoanAndSavingAccount(LoanAccount);
                            if (Loan_items.Tables[0].Rows.Count > 0)
                            {
                                entity.SavingAccountNo = Loan_items.Tables[0].Rows[0][0].ToString();
                            }

                        }


                        if (entity.NoOfAccount == 0 )
                        {
                            return GetErrorMessageResult("Please Check Savings AccountNo.");
                        }
                        //entity.Ref_EmployeeID=
                        entity.Duration = Convert.ToInt16(pbr.Duration);
                        entity.InstallmentNo = 1;
                        if (LoggedInOrganizationID != 54)
                        {

                            entity.Ref_EmployeeID = Convert.ToInt16(employeeid);
                        }
                        if (LoggedInOrganizationID == 54)
                        {
                            if (entity.Ref_EmployeeID == 0)
                            {
                                entity.Ref_EmployeeID = Convert.ToInt16(employeeid);
                            }
                        }
                         savingsAccountOpeningService.Create(entity);
                        return GetSuccessMessageResult();

                    }

                    else
                        return GetErrorMessageResult(errors);
                }
                //MapDropDownList(model);
                //return View(model);
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
                //MapDropDownList(model);
                //return View(model);
            }
        }
        [HttpPost]
        public JsonResult SavingsAccountOpening(List<SavingsAccountOpeningWithNomineeViewModel> obj)
        {
            try
            {
                SavingsAccountOpeningViewModel samodel = new SavingsAccountOpeningViewModel();
               
                
              

                if (obj == null)
                    return GetErrorMessageResult();
                else
                {
                    if (obj.Where(x => x.NAlocation <= 0).Any())
                        return GetErrorMessageResult("Alocation 0 not allow");
                    else
                    {
                        if (obj.Sum(x => x.NAlocation) != 100)
                            return GetErrorMessageResult("Alocation required 100");
                        else
                        {




                            SavingsAccountOpeningViewModel model = new SavingsAccountOpeningViewModel()
                            {
                                CenterID = obj[0].CenterID,
                                MemberID = obj[0].MemberID,
                                OfficeID = obj[0].OfficeID,
                                ProductID = obj[0].ProductID,
                                NoOfAccount = obj[0].NoOfAccount,
                                InterestRate = obj[0].InterestRate,
                                SavingInstallment = obj[0].SavingInstallment,
                           
                                Ref_EmployeeID = obj[0].Ref_EmployeeID,
                            };
                            var entity = Mapper.Map<SavingsAccountOpeningViewModel, SavingSummary>(model);

                            var pbr = productService.GetById(entity.ProductID);
                            var employee = GetEmployee(entity.CenterID);
                            int employeeid = employee.EmployeeId;
                            
                            if (model.Ref_EmployeeID==0)
                            {
                                model.Ref_EmployeeID = employeeid;
                            }
                            //if (!ModelState.IsValid)
                            //    return GetErrorMessageResult();
                            //else
                            //{
                                var member = GetMember(Convert.ToInt64(entity.MemberID));
                                int membercaregoryid = member.MemberCategoryID;

                                
                                entity.EmployeeId = Convert.ToInt16(employeeid);
                                entity.MemberCategoryID = Convert.ToByte(membercaregoryid);
                                //var mbr=memberService.GetById(employeeid);
                               

                                entity.IsActive = true;
                                entity.SavingStatus = 1;
                                entity.TransType = 2;
                                //entity.OpeningDate = mbr.JoinDate;
                                entity.TransactionDate = TransactionDate;

                                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID, @MemberID = Convert.ToInt64(entity.MemberID), @ProductID = Convert.ToInt16(entity.ProductID) };
                                var getDD = ultimateReportService.ValidateMainItemCode_21_list(param);
                                if (getDD.Tables[0].Rows.Count > 0)
                                    return GetSuccessMessageResult();
                                entity.OrgID = Convert.ToInt32(LoggedInOrganizationID);

                                if (IsDayInitiated)
                                {
                                    entity.OpeningDate = TransactionDate;
                                    if (pbr.PaymentFrequency == "W")
                                        entity.MaturedDate = entity.OpeningDate.AddMonths(Convert.ToInt16((pbr.Duration / 52)) * 12);
                                    else
                                        entity.MaturedDate = TransactionDate.AddMonths(Convert.ToInt16(pbr.Duration));
                                }
                                else
                                {
                                    var param1 = new { @OfficeID = SessionHelper.LoginUserOfficeID };
                                    var allProducts = accReportService.GetLastInitialDate(param1);

                                    var detail = allProducts.ToString();

                                if (!IsDayInitiated)
                                {
                                    if (allProducts.Tables[0].Rows.Count > 0) // check if there is any data.
                                    {

                                        entity.OpeningDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                                        entity.TransactionDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                                        if (pbr.PaymentFrequency == "W")
                                            entity.MaturedDate = entity.OpeningDate.AddMonths(Convert.ToInt16((pbr.Duration / 52)) * 12);
                                        else
                                            entity.MaturedDate = entity.OpeningDate.AddMonths(Convert.ToInt16(pbr.Duration));
                                    }
                                    else
                                    {
                                        entity.OpeningDate = System.DateTime.Now;
                                        entity.MaturedDate = entity.OpeningDate.AddMonths(Convert.ToInt16(pbr.Duration));
                                    }
                                }
                                else
                                {
                                    model.OpeningDate = TransactionDate;
                                    entity.MaturedDate = entity.OpeningDate.AddMonths(Convert.ToInt16(pbr.Duration));
                                }
                                       

                                }

                                var errors = savingsAccountOpeningService.IsValidSaving(entity);
                                if (errors.ToList().Count == 0)
                                {
                                    if (LoggedInOrganizationID == 54)
                                    {
                                        var LoanAccount = new { Qtype = 2, OfficeID = LoginUserOfficeID, MemberID = entity.MemberID, ProductID = entity.ProductID, loanTerm = entity.NoOfAccount };
                                        var Loan_items = ultimateReportService.GenerateLoanAndSavingAccount(LoanAccount);
                                        if (Loan_items.Tables[0].Rows.Count > 0)
                                            entity.SavingAccountNo = Loan_items.Tables[0].Rows[0][0].ToString();

                                    }
                                    if (entity.NoOfAccount == 0)
                                        return GetErrorMessageResult("Please Check Savings AccountNo.");
                                    if ((pbr.MaxLimit < entity.SavingInstallment))
                                    {
                                        return GetErrorMessageResult("Please Check Savings Amount.");
                                    }

                                    if ((pbr.MinLimit > entity.SavingInstallment))
                                    {
                                        return GetErrorMessageResult("Please Check Savings Amount.");
                                    }
                                    if (LoggedInOrganizationID != 54)
                                    {

                                        entity.Ref_EmployeeID = Convert.ToInt16(employeeid);
                                    }
                                    if (LoggedInOrganizationID == 54)
                                    {
                                        if (entity.Ref_EmployeeID == 0)
                                        {
                                            entity.Ref_EmployeeID = Convert.ToInt16(employeeid);
                                        }
                                    }
                                    entity.Duration = Convert.ToInt16(pbr.Duration);
                                    entity.InstallmentNo = 1;
                                    savingsAccountOpeningService.Create(entity);

                                if (LoggedInOrganizationID == 54)
                                {
                                    using (gBankerDbContext db = new gBankerDbContext())
                                    {
                                        foreach (var n in obj)
                                        {
                                            db.Database.ExecuteSqlCommand("INSERT INTO NomineeXSavingSummary VALUES(" + entity.SavingSummaryID +
                                                ",'" + n.NomineeName + "','" + n.NFatherName + "','" + n.NRelationName + "','" + n.NAddressName + "'," + n.NAlocation + ")");
                                        }
                                    }
                                }
                                   
                                    return GetSuccessMessageResult();
                                }

                                else
                                    return GetErrorMessageResult(errors);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        public ActionResult LedgerPost(SavingSummaryViewModel model)
        {
            var members = "Success";
            var val = savingsummaryService.SetOpeningSavingEntry(LoggedInOrganizationID,SessionHelper.LoginUserOfficeID.Value);
            return Json(members, JsonRequestBehavior.AllowGet);

        }
        // GET: SavingsAccountOpening/Edit/5
        public ActionResult Edit(long id)
        {
            if (savingsAccountOpeningService.IsContinued(id))
            {
                var savingsummary = savingsAccountOpeningService.GetByIdLong(id);

                var member = GetMember(Convert.ToInt64(savingsummary.MemberID));
                var entity = Mapper.Map<SavingSummary, SavingsAccountOpeningViewModel>(savingsummary);
                ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName);
                MapDropDownList(entity);

                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Discontinued ID, please enter a diferent id and name.");
                return RedirectToAction("Index");
            
        }
        // POST: SavingsAccountOpening/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, SavingsAccountOpeningViewModel model)
        {
            try
            {


                var entity = Mapper.Map<SavingsAccountOpeningViewModel, SavingSummary>(model);
                var savingSum = savingsAccountOpeningService.GetByIdLong(Convert.ToInt64(entity.SavingSummaryID));
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    string msg = "";

                    var errors = savingsAccountOpeningService.IsValidSavingEdit(entity);
                    if (errors.ToList().Count == 0)
                    {
                        savingSum.CenterID = entity.CenterID;
                        savingSum.ProductID = entity.ProductID;
                        savingSum.NoOfAccount = entity.NoOfAccount;
                        savingSum.InterestRate = entity.InterestRate;
                        savingSum.SavingInstallment = entity.SavingInstallment;
                        savingSum.OpeningDate = entity.OpeningDate;
                        savingSum.MaturedDate = entity.MaturedDate;
                        savingsAccountOpeningService.Update(savingSum);
                        return GetSuccessMessageResult();

                    }
                    else
                        return GetErrorMessageResult();
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        // GET: SavingsAccountOpening/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: SavingsAccountOpening/Delete/5
        [HttpPost]
        public ActionResult Delete(long id, SavingsAccountOpeningViewModel model)
        {
            try
            {
                var entity = Mapper.Map<SavingsAccountOpeningViewModel, SavingSummary>(model);
                // TODO: Add delete logic here
                entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                var errors = savingsAccountOpeningService.IsValidSavingDelete(entity);
                if (errors.ToList().Count == 0)
                {
                    var param = new { @SavingSummaryID = id };
                    ultimateReportService.DelSavingSummary(param);
                    
                    
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
    }
}

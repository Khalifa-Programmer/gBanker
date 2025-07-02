using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using gBanker.Web.Core.Extensions;
using gBanker.Web.Helpers;
using gBanker.Data.DBDetailModels;
using gBanker.Service.ReportServies;
namespace gBanker.Web.Controllers
{
    public class SavingCollectionController : BaseController
    {
          private readonly ICenterService centerService;
          private readonly ISavingCollectionService savingCollectionService;
          private readonly ILoanCollectionService loanCollectionService;
          private readonly IMemberService memberService;
          private readonly IOfficeService officeService;
          private readonly IProductService productService;
          private readonly ILoanCollectionReportService loanCollectionReportService;
          private readonly IUltimateReportService ultimateReportService;
          public SavingCollectionController(ISavingCollectionService savingCollectionService, ICenterService centerService, IProductService productService, IOfficeService officeService, IMemberService memberService, ILoanCollectionService loanCollectionService, ILoanCollectionReportService loanCollectionReportService, IUltimateReportService ultimateReportService)
          {
              this.savingCollectionService = savingCollectionService;
              this.centerService = centerService;
              this.productService = productService;
             
              this.officeService = officeService;
             
              this.memberService = memberService;
              this.loanCollectionService = loanCollectionService;
              this.loanCollectionReportService = loanCollectionReportService;
              this.ultimateReportService = ultimateReportService;
          }
        // GET: SavingCollection
        public ActionResult Index()
        {
            var model = new DailySavingCollectionViewModel();


            MapDropDownList(model);

  
            return View(model);
        }
        public ActionResult BuroIndex()
        {
            var model = new DailySavingCollectionViewModel();


            MapDropDownList(model);


            return View(model);
        }
        public ActionResult MonthlySavings()
        {
            var model = new DailySavingCollectionViewModel();


            MapDropDownList(model);


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
            //var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);


            IEnumerable<Center> allcenter;
            // if (LoanInstallMent != null)
            // {
            //     allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, LoggedInOrganizationID, Convert.ToInt16(LoggedInEmployeeID), LoanInstallMent.Tables[0].Rows[0]["Name"].ToString());
            // }

            // else
            //     allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, LoggedInOrganizationID, Convert.ToInt16(LoggedInEmployeeID), LoanInstallMent.Tables[0].Rows[0]["Name"].ToString());

            //// var allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value,LoggedInOrganizationID,Convert.ToInt16(LoggedInEmployeeID));
            // var viewCenList = allcenter.Select(x => x).ToList().Select(x => new SelectListItem
            // {
            //     Value = x.CenterID.ToString(),
            //     Text = string.Format("{0} - {1}", x.CenterCode.ToString(), x.CenterName.ToString())
            // });
            // var cenitems = new List<SelectListItem>();
            // cenitems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            // cenitems.AddRange(viewCenList);
            //model.centerListItems = cenitems;
            var EmployeeType = "";
            var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);
            if (LoanInstallMent.Tables[0].Rows.Count > 0)
            {
                EmployeeType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
            }
            else
            {
                EmployeeType = "";
            }
            var param = new { OrgID = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, EmpId = Convert.ToInt16(LoggedInEmployeeID), EmpType = EmployeeType.ToString(), ColDay = TransactionDay };

            List<CenterViewModel> List_ProductViewModel = new List<CenterViewModel>();
            DataSet div_items;
            if (LoggedInOrganizationID == 54)
            {
                //if (LoanInstallMent.Tables[0].Rows.Count > 0)
                //{
                div_items = ultimateReportService.GetDailySavingCenterList(param);

                //     //allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, LoggedInOrganizationID, Convert.ToInt16(LoggedInEmployeeID), LoanInstallMent.Tables[0].Rows[0]["Name"].ToString());
                // }

                // else
                //     div_items = ultimateReportService.GetDailyCenterList(param);
                //// allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, LoggedInOrganizationID, Convert.ToInt16(LoggedInEmployeeID), LoanInstallMent.Tables[0].Rows[0]["Name"].ToString());

            }
            else
            {
                //if (LoanInstallMent.Tables[0].Rows.Count > 0)
                //{

                div_items = ultimateReportService.GetDailySavingCenterList(param);

                //allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, LoggedInOrganizationID, Convert.ToInt16(LoggedInEmployeeID), LoanInstallMent.Tables[0].Rows[0]["Name"].ToString());
                //}

                //else
                //    div_items = ultimateReportService.GetDailyCenterList(param);
                ////allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, LoggedInOrganizationID, Convert.ToInt16(LoggedInEmployeeID), LoanInstallMent.Tables[0].Rows[0]["Name"].ToString());

            }




            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new CenterViewModel
            {
                CenterID = row.Field<Int32>("CenterID"),
                CenterCode = row.Field<string>("CenterCode"),
                CenterName = row.Field<string>("CenterName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + " - " + x.CenterName.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            model.centerListItems = d_items;
            var Transtype = new List<SelectListItem>();
            Transtype.Add(new SelectListItem() { Text = "Transfer", Value = "0", Selected = true });
            Transtype.Add(new SelectListItem() { Text = "Cash", Value = "1" });
            model.cashListItems = Transtype.AsEnumerable();

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));
            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, m.FirstName + '-' + m.MiddleName + '-' + m.LastName), Value = m.MemberID.ToString() });
            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;


            var alloffice = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.OrgID==LoggedInOrganizationID);
            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });
            model.officeListItems = viewOffice;


            var allSearchProd = productService.SearchProduct(0, Convert.ToInt32(LoggedInOrganizationID),"S");
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
        public ActionResult ExecuteAppend(string officeId, string CenterID)
        {

            var result = "OK";
            var param1 = new
            {
                @OfficeID = SessionHelper.LoginUserOfficeID,
                @OrgID = SessionHelper.LoginUserOrganizationID,
                @lcl_BusinessDate = TransactionDate,
                @CreateUser = LoggedInEmployeeID,
                @Qtype=1,
                @ProductCode='1',
                @CenterID=CenterID

            };
            var LoanInstallMent = ultimateReportService.ExecuteSPMonthlySaving(param1);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInstallment(string officeId, string centerId, string MemId, string ProdId, string NoOfAccount, decimal SavingInstallment, decimal WithDrawal,decimal penalty)
        {
            decimal vLoanInstallment = 0;
            decimal vWithDrawal=0;
            decimal vDeposit = 0;
            var model = new DailySavingCollectionViewModel();
            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt16(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);
            model.NoOfAccount = Convert.ToInt16(NoOfAccount);
            var entity = Mapper.Map<DailySavingCollectionViewModel, DailySavingTrx>(model);

          //  var mlt = savingCollectionService.GetAll().Where(s => s.OfficeID == model.OfficeID && s.CenterID == model.CenterID && s.MemberID == model.MemberID && s.ProductID == model.ProductID && s.NoOfAccount == model.NoOfAccount).FirstOrDefault();
            var mlt = savingCollectionService.GetAll().Where(s => s.OrgID==LoggedInOrganizationID && s.OfficeID == model.OfficeID  && s.MemberID == model.MemberID && s.ProductID == model.ProductID && s.NoOfAccount == model.NoOfAccount && s.IsActive == 1).FirstOrDefault();

            if (mlt != null)
            {
                
                var vsav = savingCollectionService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == model.OfficeID && s.MemberID == model.MemberID && s.ProductID == model.ProductID && s.NoOfAccount == model.NoOfAccount && s.IsActive == 1).Sum(s => s.SavingInstallment);
                var vwith = savingCollectionService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == model.OfficeID && s.MemberID == model.MemberID && s.ProductID == model.ProductID && s.NoOfAccount == model.NoOfAccount && s.IsActive == 1).Sum(s => s.Withdrawal);
              
                //vLoanInstallment = (mlt.Deposit + mlt.SavingInstallment+mlt.TransferDeposit+SavingInstallment + penalty) - WithDrawal-mlt.Withdrawal-mlt.TransferWithdrawal;
                vLoanInstallment = (mlt.Deposit + Convert.ToDecimal(vsav) + mlt.TransferDeposit + SavingInstallment + penalty) - WithDrawal - Convert.ToDecimal(vwith) - mlt.TransferWithdrawal;
                if (vLoanInstallment < 0)
                {
                    vWithDrawal = 0;

                    //vLoanInstallment = (Convert.ToDecimal(mlt.Deposit) + Convert.ToDecimal(mlt.SavingInstallment) + Convert.ToDecimal(mlt.TransferDeposit) + Convert.ToDecimal(SavingInstallment) + Convert.ToDecimal(penalty)) - (vWithDrawal + Convert.ToDecimal(mlt.Withdrawal) + Convert.ToDecimal(mlt.TransferWithdrawal));
                    vLoanInstallment = (Convert.ToDecimal(mlt.Deposit) + Convert.ToDecimal(vsav) + Convert.ToDecimal(mlt.TransferDeposit) + Convert.ToDecimal(SavingInstallment) + Convert.ToDecimal(penalty)) - (vWithDrawal + Convert.ToDecimal(vwith) + Convert.ToDecimal(mlt.TransferWithdrawal));

                }
                else
                {

                    vWithDrawal = WithDrawal;

                    //vLoanInstallment = (Convert.ToDecimal(mlt.Deposit) + Convert.ToDecimal(mlt.SavingInstallment) + Convert.ToDecimal(mlt.TransferDeposit) + Convert.ToDecimal(SavingInstallment) + Convert.ToDecimal(penalty)) - (vWithDrawal + Convert.ToDecimal(mlt.Withdrawal) + Convert.ToDecimal(mlt.TransferWithdrawal));
                    vLoanInstallment = (Convert.ToDecimal(mlt.Deposit) + Convert.ToDecimal(vsav) + Convert.ToDecimal(mlt.TransferDeposit) + Convert.ToDecimal(SavingInstallment) + Convert.ToDecimal(penalty)) - (vWithDrawal + Convert.ToDecimal(vwith) + Convert.ToDecimal(mlt.TransferWithdrawal));

                }
            }
            else
            {
                var sm = savingCollectionService.GetAll().Where(s => s.OrgID==LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.CenterID == model.CenterID && s.MemberID == model.MemberID && s.ProductID == model.ProductID && s.NoOfAccount == model.NoOfAccount && s.IsActive==1).FirstOrDefault();

                //var sm = savingCollectionService.GetAll().Where(s => s.OfficeID == SessionHelper.LoginUserOfficeID.Value  && s.MemberID == model.MemberID && s.ProductID == model.ProductID && s.NoOfAccount == model.NoOfAccount).FirstOrDefault();
                if (sm != null)
                {
                    var vsav = savingCollectionService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == model.OfficeID && s.MemberID == model.MemberID && s.ProductID == model.ProductID && s.NoOfAccount == model.NoOfAccount && s.IsActive == 1).Sum(s => s.SavingInstallment);
                    var vwith = savingCollectionService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == model.OfficeID && s.MemberID == model.MemberID && s.ProductID == model.ProductID && s.NoOfAccount == model.NoOfAccount && s.IsActive == 1).Sum(s => s.Withdrawal);
       
                   // vLoanInstallment = (sm.Balance + SavingInstallment + penalty) - WithDrawal;
                  //  vLoanInstallment=(mlt.Deposit + mlt.SavingInstallment + mlt.TransferDeposit + SavingInstallment + penalty) - WithDrawal - mlt.Withdrawal - mlt.TransferWithdrawal;
                    vLoanInstallment = (sm.Deposit + Convert.ToDecimal(vsav)  + SavingInstallment + penalty) - WithDrawal - Convert.ToDecimal(vwith) ;
                    if (vLoanInstallment < 0)
                    {
                        vWithDrawal = 0;
                        vLoanInstallment = (Convert.ToDecimal(sm.Deposit) + Convert.ToDecimal(vsav)  + Convert.ToDecimal(SavingInstallment) + Convert.ToDecimal(penalty)) - (vWithDrawal + Convert.ToDecimal(vwith));
                        //vLoanInstallment = (Convert.ToDecimal(mlt.Deposit) + Convert.ToDecimal(mlt.SavingInstallment) + Convert.ToDecimal(mlt.TransferDeposit) + Convert.ToDecimal(SavingInstallment) + Convert.ToDecimal(penalty)) - (vWithDrawal + Convert.ToDecimal(mlt.Withdrawal) + Convert.ToDecimal(mlt.TransferWithdrawal));

                    }
                    else
                    {
                        vWithDrawal = WithDrawal ;
                        vLoanInstallment = (Convert.ToDecimal(sm.Deposit) + Convert.ToDecimal(vsav) + Convert.ToDecimal(SavingInstallment) + Convert.ToDecimal(penalty)) - (vWithDrawal + Convert.ToDecimal(vwith) );
                        //vLoanInstallment = (Convert.ToDecimal(mlt.Deposit) + Convert.ToDecimal(mlt.SavingInstallment) + Convert.ToDecimal(mlt.TransferDeposit) + Convert.ToDecimal(SavingInstallment) + Convert.ToDecimal(penalty)) - (vWithDrawal + Convert.ToDecimal(mlt.Withdrawal) + Convert.ToDecimal(mlt.TransferWithdrawal));

                    }

                }

            }
            var result = new {with=vWithDrawal.ToString(), loan = vLoanInstallment.ToString() };
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
            var mlt = savingCollectionService.GetAll().Where(s => s.OrgID==LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.MemberID == entity.MemberID && s.ProductID == entity.ProductID && s.IsActive == 1).FirstOrDefault();

           // var mlt = savingCollectionService.GetAll().Where(s => s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.CenterID == entity.CenterID && s.MemberID == entity.MemberID && s.ProductID == entity.ProductID).FirstOrDefault();
            if (mlt != null)
            {

                Loantrm = mlt.NoOfAccount;
                vBalance = mlt.Balance;

            }
            //Session[ProductSessionKey] = pbr;
            vLoanTerm = Loantrm;
            //vBalance = vBalance;

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
        [HttpPost]
        public ActionResult GetDailySavingCollectionSheet(int centerId, int productId,string filterColumn,string filterValue)
        {
            try
            {


                //var collectionList = savingCollectionService.GetDailySavingCollectionByCenter(centerId, filterColumn, filterValue).ToList();
                var collectionList = savingCollectionService.GetDailySavingCollectionByCenterQueryable(centerId, filterColumn, filterValue);
                //IQueryable<DailySavingTrx> members = null;
                //if (LoggedInOrganizationID==54)
                //{
                var members = collectionList.Where(c => c.ProductID == productId && c.CenterID == centerId && c.OfficeID == LoginUserOfficeID).OrderBy(c => c.MemberCode.Substring(c.MemberCode.Length-5));

                //}
                //else
                //{
                //    members = collectionList.Where(c => c.ProductID == productId && c.CenterID == centerId && c.OfficeID == LoginUserOfficeID).OrderBy(c => c.MemberCode);

                //}
                //var members = collectionList.Where(c => c.ProductID == productId && c.CenterID == centerId).ToList().OrderBy(c=>c.ProductCode).OrderBy(c=>c.MemberCode);
                var memberModels = Mapper.Map<IEnumerable<DailySavingTrx>, IEnumerable<DailySavingCollectionViewModel>>(members);

                List<DailySavingCollectionViewModel> detail = new List<DailySavingCollectionViewModel>();
                int rowSl = 0;
                if(LoggedInOrganizationID==54)
                {
                    foreach (var vd in memberModels)
                    {
                        var loans = new DailySavingCollectionViewModel()
                        {
                            rowSl = rowSl,
                            DailySavingTrxID = vd.DailySavingTrxID,
                            TransactionDate = vd.TransactionDate,
                            TrxDateMsg = vd.TransactionDate.ToString("dd-MMM-yyyy"),
                            SavingSummaryID = vd.SavingSummaryID,
                            OfficeID = vd.OfficeID,
                            MemberID = vd.MemberID,
                            MemberCode = vd.MemberCode,
                            MemberName = vd.MemberName,
                            ProductID = vd.ProductID,
                            ProductCode = vd.ProductCode,
                            ProductName = vd.ProductName,
                            NoOfAccount = vd.NoOfAccount,
                            CenterID = vd.CenterID,
                            SavingInstallment = vd.SavingInstallment,
                            Deposit = vd.Deposit,
                            Withdrawal = vd.Withdrawal,
                            Balance = vd.Balance,
                            Penalty = vd.Penalty,
                            TransType = vd.TransType,
                            MonthlyInterest = vd.MonthlyInterest,
                            PresenceInd = vd.PresenceInd,
                            TransferDeposit = vd.TransferDeposit,
                            TransferWithdrawal = vd.TransferWithdrawal,
                            DueSavingSummary = vd.DueSavingSummary,
                            SavingCollectionSummary = vd.SavingCollectionSummary,
                            WithDrawalSummary = vd.WithDrawalSummary,
                            PenaltySummary = vd.PenaltySummary,
                            memName = vd.memName,
                            vMaxLoanTerm = vd.vMaxLoanTerm,
                            SubMainCategory = vd.SubMainCategory,
                            SavingAccountNo = vd.SavingAccountNo.Substring(4, (vd.SavingAccountNo.Length - 4)),
                            DueSavingInstallment = vd.DueSavingInstallment,
                            OrgID=vd.OrgID
                        };
                        detail.Add(loans);
                        rowSl++;
                    }
                }
                else
                {
                    foreach (var vd in memberModels)
                    {
                        var loans = new DailySavingCollectionViewModel()
                        {
                            rowSl = rowSl,
                            DailySavingTrxID = vd.DailySavingTrxID,
                            TransactionDate = vd.TransactionDate,
                            TrxDateMsg = vd.TransactionDate.ToString("dd-MMM-yyyy"),
                            SavingSummaryID = vd.SavingSummaryID,
                            OfficeID = vd.OfficeID,
                            MemberID = vd.MemberID,
                            MemberCode = vd.MemberCode,
                            MemberName = vd.MemberName,
                            ProductID = vd.ProductID,
                            ProductCode = vd.ProductCode,
                            ProductName = vd.ProductName,
                            NoOfAccount = vd.NoOfAccount,
                            CenterID = vd.CenterID,
                            SavingInstallment = vd.SavingInstallment,
                            Deposit = vd.Deposit,
                            Withdrawal = vd.Withdrawal,
                            Balance = vd.Balance,
                            Penalty = vd.Penalty,
                            TransType = vd.TransType,
                            MonthlyInterest = vd.MonthlyInterest,
                            PresenceInd = vd.PresenceInd,
                            TransferDeposit = vd.TransferDeposit,
                            TransferWithdrawal = vd.TransferWithdrawal,
                            DueSavingSummary = vd.DueSavingSummary,
                            SavingCollectionSummary = vd.SavingCollectionSummary,
                            WithDrawalSummary = vd.WithDrawalSummary,
                            PenaltySummary = vd.PenaltySummary,
                            memName = vd.memName,
                            vMaxLoanTerm = vd.vMaxLoanTerm,
                            SubMainCategory = vd.SubMainCategory,
                            SavingAccountNo = vd.SavingAccountNo,
                            DueSavingInstallment = vd.DueSavingInstallment,
                            OrgID=vd.OrgID
                        };
                        detail.Add(loans);
                        rowSl++;
                    }
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
        public ActionResult GetDailySavingCollectionSheetForMonthly(int centerId, int productId, string filterColumn, string filterValue)
        {
            try
            {


                //var collectionList = savingCollectionService.GetDailySavingCollectionByCenter(centerId, filterColumn, filterValue).ToList();
                var collectionList = savingCollectionService.GetDailySavingCollectionByCenterQueryableForMonthly(centerId, filterColumn, filterValue);

                var members = collectionList.Where(c => c.ProductID == productId && c.CenterID == centerId).ToList().OrderBy(c => c.ProductCode).OrderBy(c => c.MemberCode);
                var memberModels = Mapper.Map<IEnumerable<DailySavingTrx>, IEnumerable<DailySavingCollectionViewModel>>(members);

                List<DailySavingCollectionViewModel> detail = new List<DailySavingCollectionViewModel>();
                int rowSl = 0;
                foreach (var vd in memberModels)
                {
                    var loans = new DailySavingCollectionViewModel() { rowSl = rowSl, DailySavingTrxID = vd.DailySavingTrxID, TransactionDate = vd.TransactionDate, TrxDateMsg = vd.TransactionDate.ToString("dd-MMM-yyyy"), SavingSummaryID = vd.SavingSummaryID, OfficeID = vd.OfficeID, MemberID = vd.MemberID, MemberCode = vd.MemberCode, MemberName = vd.MemberName, ProductID = vd.ProductID, ProductCode = vd.ProductCode, ProductName = vd.ProductName, NoOfAccount = vd.NoOfAccount, CenterID = vd.CenterID, SavingInstallment = vd.SavingInstallment, Deposit = vd.Deposit, Withdrawal = vd.Withdrawal, Balance = vd.Balance, Penalty = vd.Penalty, TransType = vd.TransType, MonthlyInterest = vd.MonthlyInterest, PresenceInd = vd.PresenceInd, TransferDeposit = vd.TransferDeposit, TransferWithdrawal = vd.TransferWithdrawal, DueSavingSummary = vd.DueSavingSummary, SavingCollectionSummary = vd.SavingCollectionSummary, WithDrawalSummary = vd.WithDrawalSummary, PenaltySummary = vd.PenaltySummary, memName = vd.memName, vMaxLoanTerm = vd.vMaxLoanTerm, SubMainCategory = vd.SubMainCategory, SavingAccountNo = vd.SavingAccountNo, DueSavingInstallment = vd.DueSavingInstallment };
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
        public ActionResult GenerateReport(string fromDate, string toDate, string CenterID, int? Qtype)
        {
            
            if (CenterID == "0")
            {
                var param = new { Qtype = (Qtype ?? 1), Org = LoggedInOrganizationID, Office = LoginUserOfficeID, Center = CenterID };
                //var param = new { Qtype = 1, Org = LoggedInOrganizationID, Office = LoginUserOfficeID, Center = CenterID };
                var allproducts = loanCollectionReportService.GetDataSavingCollectionInfo(param);
                 var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                string rptName = "rptDailySavingsCollection";
                if (Qtype.HasValue)
                    if (Qtype.Value == 3 || Qtype.Value == 4)
                        rptName = "rptDailySavingsCollectionSpecial";
                ReportHelper.PrintReport(rptName + ".rpt", allproducts.Tables[0], reportParam);
                //ReportHelper.PrintReport("rptDailySavingsCollection.rpt", allproducts.Tables[0], reportParam);
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
            var result = loanCollectionService.setLoanAndSavingingLessFiftyPercent(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(CenterID), 2);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDailySavingCollectionProductList(int jtStartIndex, int jtPageSize, string jtSorting, int centerId, string filterColumn, string filterValue)
        {
            try
            {
                jtPageSize = 1;

                //List<DailySavingCollectionViewModel> collectionList = new List<DailySavingCollectionViewModel>();
                //var param1 = new { @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Convert.ToInt16(centerId), @filterColumn = filterColumn, @filterValue = filterValue };
                //var div_items = ultimateReportService.GetDailySavinglist(param1);

                ////var param = new { OfficeId = LoginUserOfficeID, MemberId = Member_id, CenterId = center_id };
                ////var div_items = ultimateReportService.GetProductList(param);

                //collectionList = div_items.Tables[0].AsEnumerable()
                //.Select(row => new DailySavingCollectionViewModel
                //{

                //    DailySavingTrxID = row.Field<long>("DailySavingTrxID"),
                //    SavingSummaryID = row.Field<long>("SavingSummaryID"),
                //    OfficeID = row.Field<Int16>("OfficeID"),
                //    MemberID = row.Field<long>("MemberID"),
                //    MemberCode = row.Field<string>("MemberCode"),
                //    MemberName = row.Field<string>("MemberName"),
                //    ProductID = row.Field<Int16>("ProductID"),
                //    ProductCode = row.Field<string>("ProductCode"),
                //    ProductName = row.Field<string>("ProductName"),
                //    CenterID = row.Field<Int16>("CenterID"),
                //    NoOfAccount = row.Field<Int16>("NoOfAccount"),
                //     TransactionDate = row.Field<DateTime>("TransactionDate"),
                //    DueSavingInstallment = row.Field<decimal>("DueSavingInstallment"),
                //    SavingInstallment = row.Field<decimal>("SavingInstallment"),
                //    Deposit = row.Field<decimal>("Deposit"),
                //    Withdrawal = row.Field<decimal>("Withdrawal"),
                //    Balance = row.Field<decimal>("Balance"),
                //    Penalty = row.Field<decimal>("Penalty"),
                //    TransType = row.Field<byte>("TransType"),
                //    MonthlyInterest = row.Field<decimal>("MonthlyInterest"),
                //    PresenceInd = row.Field<bool>("PresenceInd"),
                //    TransferDeposit = row.Field<decimal>("TransferDeposit"),
                //    TransferWithdrawal = row.Field<decimal>("TransferWithdrawal"),
                //    IsActive = row.Field<long>("IsActive"),
                //    InActiveDate = row.Field<DateTime>("InActiveDate"),

                //}).ToList();


                var collectionList = savingCollectionService.GetDailySavingCollectionByCenter(centerId, filterColumn, filterValue).ToList();



                var products = new List<DailySavingCollectionViewModel>();
                foreach (var tr in collectionList)
                {
                    if (products.Where(p => p.ProductID == tr.ProductID && tr.CenterID == centerId ).OrderBy(p=>p.ProductCode).OrderBy(p=>p.MemberCode).FirstOrDefault() == null)
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
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public ActionResult GetDailySavingCollectionProductListForMonthly(int jtStartIndex, int jtPageSize, string jtSorting, int centerId, string filterColumn, string filterValue)
        {
            try
            {
                jtPageSize = 1;



                var collectionList = savingCollectionService.GetDailySavingCollectionByCenterForMonthly(centerId, filterColumn, filterValue).ToList();



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
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        private List<DailySavingCollectionViewModel> GetProductList()
        {
            var collectionList = new List<DailySavingCollectionViewModel>();
            collectionList.Add(new DailySavingCollectionViewModel() { DailySavingTrxID = 1, CenterID = 1, MemberID = 10000, ProductID = 222, Deposit = 100, Withdrawal = 10, SavingInstallment = 100,  NoOfAccount = 1 });
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

                savingCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);
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
        [HttpPost]
        public ActionResult SaveSavingTransactionForMonthly(Dictionary<string, string> allTrx, List<string> allLoanTrxId, string CenterId, string ProductCode)
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
                var param1 = new
                {
                    @OfficeID = SessionHelper.LoginUserOfficeID,
                    @OrgID = SessionHelper.LoginUserOrganizationID,
                    @lcl_BusinessDate = TransactionDate,
                    @CreateUser = LoggedInEmployeeID,
                    @Qtype = 0,
                    @ProductCode = ProductCode,
                    @CenterID = CenterId

                };
                var LoanInstallMent = ultimateReportService.ExecuteSPMonthlySaving(param1);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: SavingCollection/Create
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
        // POST: SavingCollection/Create
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

                //Add Validlation Logic.

                if (ModelState.IsValid)
                {
                    savingCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);
                    var summary = savingCollectionService.GetAll().Where(s => s.OrgID==LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.MemberID == entity.MemberID && s.ProductID == entity.ProductID && s.NoOfAccount == entity.NoOfAccount && s.IsActive == 1).FirstOrDefault();

                   // var summary = savingCollectionService.GetAll().Where(s => s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.MemberID == entity.MemberID && s.CenterID == entity.CenterID && s.ProductID == entity.ProductID && s.NoOfAccount == entity.NoOfAccount).FirstOrDefault();

                    //var getSummaryId = LoanSummaryService.GetAll();
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
                            //ModelState.Clear();
                            //var emtpy = new DailySavingCollectionViewModel();
                            //if (IsDayInitiated)
                            //{
                            //    emtpy.TransactionDate = TransactionDate;
                            //}
                            //MapDropDownList(emtpy);

                            //return View(emtpy);
                        }
                        else
                            return GetErrorMessageResult();
                            //ModelState.AddModelErrors(errors);
                    }
                   
                }
                return GetErrorMessageResult();
                //MapDropDownList(model);
                //return View(model);

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        // GET: SavingCollection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: SavingCollection/Edit/5
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
        // GET: SavingCollection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: SavingCollection/Delete/5
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

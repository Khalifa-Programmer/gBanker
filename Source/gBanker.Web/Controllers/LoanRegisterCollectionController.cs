using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gBanker.Service.ReportExecutionService;
using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;

namespace gBanker.Web.Controllers
{
    public class LoanRegisterCollectionController : BaseController
    {
        private readonly ICenterService centerService;
        private readonly ILoanTrxService loanTrxService;
        private readonly ILoanCollectionService loanCollectionService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly ILoanCollectionReportService loanCollectionReportService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ILoanSummaryService loanSummaryService;
        private readonly IInvestorService investorService;
        //KHALID AHMED
        //29 March, 2018

        public LoanRegisterCollectionController(ILoanTrxService loanTrxService, ICenterService centerService, IProductService productService, IOfficeService officeService, IMemberService memberService, ILoanCollectionService loanCollectionService, ILoanCollectionReportService loanCollectionReportService, IUltimateReportService ultimateReportService, ILoanSummaryService loanSummaryService, IInvestorService investorService)
        {
            this.loanTrxService = loanTrxService;
            this.centerService = centerService;
            this.productService = productService;
            this.officeService = officeService;
            this.memberService = memberService;
            this.loanCollectionService = loanCollectionService;
            this.loanCollectionReportService = loanCollectionReportService;
            this.ultimateReportService = ultimateReportService;
            this.loanSummaryService = loanSummaryService;
            this.investorService = investorService;
        }



        // GET: LoanLedgerCorrection
        public ActionResult LoanRegisterCorrect()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["LoggedInUser"] = LoggedInEmployee.EmpName;
            ViewData["LoggedInOfficeID"] = LoggedInEmployee.OfficeID;

            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;
            return View();

        }


        public JsonResult GETLoanRegisterList(int jtStartIndex, int jtPageSize, string jtSorting, int CenterID = 0, long MemberID = 0, int ProductID = 0, int LoanTerm = 0, string Option = "")
        {
            try
            {
                var detail = loanTrxService.GetLoanTrxList(CenterID, MemberID, LoanTerm, ProductID, Option).ToList();

                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<LoanTrx>, IEnumerable<LoanTrxViewModel>>(entities);
                ////////////////NE

                List<LoanTrxViewModel> ViewDetail = new List<LoanTrxViewModel>();
                int rowSl = 0;
                foreach (var vd in currentPageRecords.OrderBy(step => step.TrxDate))
                {
                    var loans = new LoanTrxViewModel() { LoanTrxID = vd.LoanTrxID, rowSl = rowSl, TrxDate = vd.TrxDate, InstallmentDate = vd.InstallmentDate, PrincipalLoan = vd.PrincipalLoan, LoanDue = vd.LoanDue, LoanPaid = vd.LoanPaid, IntCharge = vd.IntCharge, IntDue = vd.IntDue, IntPaid = vd.IntPaid, TrxType = vd.TrxType, CreateUser = vd.CreateUser, LoanSummaryID = vd.LoanSummaryID, InstallmentNo = vd.InstallmentNo };
                    ViewDetail.Add(loans);
                    rowSl++;
                }
                ///////////////////////////////////////


                return Json(new { Result = "OK", Records = ViewDetail, TotalRecordCount = totalCount });
                //return Json(new { Result = "OK",  TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult SaveLoanTrx(Dictionary<string, string> allTrx, List<string> allLoanTrxId,
            string center,
            string member,
            string product,
            string loanterm
            )
        {

            try
            {
                //if (!IsDayInitiated)
                //{
                //    return Json(new { Result = "ERROR", Message = "Please run the start work process" });
                //}

                //savingCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                var trx = allTrx;

                var trxId = 1;
                var loanTrxIds = allLoanTrxId.Where(w => int.TryParse(w, out trxId));

                var loanTrxRegisterCollection = new List<LoanTrx>();
                LoanTrxViewModel LoanTrxVM = new LoanTrxViewModel();
                foreach (var id in trx)  //foreach (var id in loanTrxIds)
                {
                    string[] v = id.Key.Split(',');
                    var LoanTrxId = "";
                    var FieldName = "";
                    if (v.Length > 1)
                    {
                        LoanTrxId = v[1];
                        FieldName = v[0];
                    }
                    else
                    {
                        LoanTrxId = "0";
                        FieldName = v[0];
                    }

                    var value = id.Value;
                    LoanTrxVM.LoanTrxID = Convert.ToInt64(LoanTrxId);

                    if (FieldName.Contains("TransactionDate"))
                    {
                        LoanTrxVM.InstallmentDate = Convert.ToDateTime(value);
                    }


                    if (FieldName == "PrincipalLoan")// Last one
                    {
                        LoanTrxVM.PrincipalLoan = Convert.ToDecimal(value);
                    }
                    if (FieldName == "LoanDue")// Last one
                    {
                        LoanTrxVM.LoanDue = Convert.ToDecimal(value);
                    }
                    if (FieldName == "LoanPaid")// Last one
                    {
                        LoanTrxVM.LoanPaid = Convert.ToDecimal(value);
                    }
                    if (FieldName == "IntCharge")// Last one
                    {
                        LoanTrxVM.IntCharge = Convert.ToDecimal(value);
                    }
                    if (FieldName == "IntDue")// Last one
                    {
                        LoanTrxVM.IntDue = Convert.ToDecimal(value);
                    }
                    if (FieldName == "IntPaid")// Last one
                    {
                        LoanTrxVM.IntPaid = Convert.ToDecimal(value);
                    }
                    if (FieldName == "InstallmentNo")// Last one
                    {
                        LoanTrxVM.InstallmentNo = Convert.ToInt16(value); //Convert.ToInt32(value);
                    }
                    if (FieldName == "TrxType")// Last one
                    {
                        LoanTrxVM.TrxType = Convert.ToByte(value); //Convert.ToInt32(value);
                    }

                    if (FieldName == "CreateUser")// Last one
                    {
                        // Create New Object to Save
                        // Update if Primary key available Otherwise Save it.
                        LoanTrxVM.CreateUser = value;

                        if (LoanTrxId == "0")// Create New
                        {   // Save Method. NEW
                            var entity = Mapper.Map<LoanTrxViewModel, LoanTrx>(LoanTrxVM);
                            entity.CenterID = Convert.ToInt32(center);
                            entity.MemberID = Convert.ToInt64(member);
                            entity.LoanTerm = Convert.ToInt32(loanterm);
                            entity.ProductID = Convert.ToInt16(product);
                            entity.OrgID = (int)LoggedInOrganizationID;
                            entity.OfficeID = (int)SessionHelper.LoginUserOfficeID;

                            // var getProduct = productService.GetAll().Where(s => s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.ProductCode);

                            //var LoanSummaryObj = loanSummaryService.GetAll().Where(s => s.IsActive == true && s.OfficeID == SessionHelper.LoginUserOfficeID && s.ProductID == entity.ProductID && s.CenterID == entity.CenterID && s.MemberID == entity.MemberID && s.LoanTerm == entity.LoanTerm).FirstOrDefault(); //.SingleOrDefault();
                            var LoanSummaryObj = loanSummaryService.GetSingleRow((int)SessionHelper.LoginUserOfficeID, entity.ProductID, entity.CenterID, entity.MemberID, entity.LoanTerm); //.SingleOrDefault();

                            entity.MemberCategoryID = LoanSummaryObj.MemberCategoryID;
                            entity.LoanSummaryID = LoanSummaryObj.LoanSummaryID;

                            entity.InvestorID = LoanSummaryObj.InvestorID;
                            entity.PrincipalLoan = LoanTrxVM.PrincipalLoan;
                            entity.LoanDue = LoanTrxVM.LoanDue;
                            entity.LoanPaid = LoanTrxVM.LoanPaid;
                            entity.IntCharge = LoanTrxVM.IntCharge;
                            entity.IntDue = LoanTrxVM.IntDue;
                            entity.IntPaid = LoanTrxVM.IntPaid;
                            entity.TrxType = LoanTrxVM.TrxType;
                            entity.EmployeeID = (Int16)SessionHelper.LoggedInEmployeeID;


                            //entity.TrxDate = DateTime.Now;
                            //entity.InstallmentDate = DateTime.Now;

                            entity.TrxDate = LoanTrxVM.InstallmentDate;
                            entity.InstallmentDate = LoanTrxVM.InstallmentDate;
                            entity.CreateDate = DateTime.Now;
                            entity.CreateUser = LoanTrxVM.CreateUser;
                            if (entity.PrincipalLoan == 0 && entity.LoanDue == 0 && entity.LoanPaid == 0 && entity.IntCharge == 0 && entity.IntDue == 0 && entity.IntPaid == 0)
                            {

                            }
                            else
                            {
                                loanTrxService.Create(entity);
                            }

                            // loanTrxService.CreateLoanTrx(entity);
                        }
                        else
                        {       //Update Method

                            if (LoanTrxVM.CreateUser == "update")
                            {

                                //var loanTrxUpdat = loanTrxService.GetById((int)LoanTrxVM.LoanTrxID);
                                //loanTrxUpdat.PrincipalLoan = LoanTrxVM.PrincipalLoan;
                                //loanTrxUpdat.LoanDue = LoanTrxVM.LoanDue;
                                //loanTrxUpdat.LoanPaid = LoanTrxVM.LoanPaid;
                                //loanTrxUpdat.IntCharge = LoanTrxVM.IntCharge;
                                //loanTrxUpdat.IntDue = LoanTrxVM.IntDue;
                                //loanTrxUpdat.IntPaid = LoanTrxVM.IntPaid;
                                //loanTrxUpdat.TrxType = LoanTrxVM.TrxType;


                                //loanTrxService.Update(loanTrxUpdat);


                                /// insert into Table
                                var param = new
                                {

                                    LoanTrxID = LoanTrxVM.LoanTrxID
                                    ,
                                    PrincipalLoan = LoanTrxVM.PrincipalLoan
                                    ,
                                    LoanDue = LoanTrxVM.LoanDue
                                    ,
                                    LoanPaid = LoanTrxVM.LoanPaid
                                    ,
                                    IntCharge = LoanTrxVM.IntCharge
                                    ,
                                    IntDue = LoanTrxVM.IntDue
                                    ,
                                    IntPaid = LoanTrxVM.IntPaid
                                    ,
                                    TrxType = LoanTrxVM.TrxType
                                    ,
                                    CreateUser = SessionHelper.LoginUserEmployeeID
                                    ,
                                    CreateDate = DateTime.Now
                                    ,
                                    InstallmentNo = LoanTrxVM.InstallmentNo

                                };

                                var val = ultimateReportService.InsertLOANRegisterUpdateINFO(param);


                            }// END Update

                            //  loanTrxService.UpdateLoanTrx(entity);
                        }

                        LoanTrxVM = new LoanTrxViewModel(); // After Save and Update Create New Object

                    }


                    /*
                    if (allTrx.ContainsKey(BalanceId))
                        decimal.TryParse(allTrx[BalanceId], out balance);
                   */


                    //var errors = new { DailySavingTrxID = long.Parse(id), DailySavinInstallment = savingInstallment, WithDrawal = withdrawal };
                    //var div_items = ultimateReportService.validateSavingBalance(errors);
                    //if (div_items.Tables[0].Rows.Count > 0)
                    //{
                    //    string vErr = div_items.Tables[0].Rows[0]["ErrorCode"].ToString();
                    //    if (vErr == "2")
                    //    {
                    //        return GetErrorMessageResult(div_items.Tables[0].Rows[0]["ErrorName"].ToString());
                    //    }
                    //}

                    //;/;//  var savingTrx = new DailySavingTrx() { DailySavingTrxID = long.Parse(id), Balance = balance, Deposit = deposit, SavingInstallment = savingInstallment, Withdrawal = withdrawal, Penalty = penalty };
                    ///;;/;///;/ savingTrxViewCollection.Add(savingTrx);

                }

                // savingCollectionService.SaveDailysavingCollection(savingTrxViewCollection);

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }// END



    }// Class
}// End Of Namespace
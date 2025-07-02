using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System.Data;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using BasicDataAccess;


namespace gBanker.Web.Controllers
{
    //sdfdsf
    public class DisbursementController : BaseController
    {
        #region Variables
        private readonly IEmployeeService employeeService;
        private readonly IProductService productService;
        private readonly ICenterService centerService;
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IUltimateReportService unlimitedReportService;
        private readonly IPOMISReportService pomisReportService;
        private readonly IWeeklyReportService weeklyReportService;
        private readonly ILoanTrxService loanTrxService;
        private readonly ILoanSummaryService loanSummaryService;
        private readonly ISavingSummaryService savingSummaryService;
        private readonly IDailyLoanTrxService dailyLoanTrxService;
        private readonly ISpecialLoanCollectionService specialLoanCollectionService;
        public DisbursementController(IEmployeeService employeeService, IOfficeService officeService, 
            IProductService productService, ICenterService centerService, 
            IMemberService memberService, IGroupwiseReportService groupwiseReportService, 
            IUltimateReportService unlimitedReportService, 
            IPOMISReportService pomisReportService, IWeeklyReportService weeklyReportService,
            ILoanTrxService loanTrxService, ILoanSummaryService loanSummaryService, 
            IDailyLoanTrxService dailyLoanTrxService, 
            ISpecialLoanCollectionService specialLoanCollectionService, ISavingSummaryService savingSummaryService)
        {
            this.employeeService = employeeService;
            this.officeService = officeService;
            this.productService = productService;
            this.centerService = centerService;
            this.memberService = memberService;
            this.groupwiseReportService = groupwiseReportService;
            this.unlimitedReportService = unlimitedReportService;
            this.pomisReportService = pomisReportService;
            this.weeklyReportService = weeklyReportService;
            this.loanTrxService = loanTrxService;
            this.loanSummaryService = loanSummaryService;
            this.dailyLoanTrxService = dailyLoanTrxService;
            this.specialLoanCollectionService = specialLoanCollectionService;
            this.savingSummaryService = savingSummaryService;
        }
        #endregion

        #region Methods
        public JsonResult GetDisbursementCorrectionData(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                List<LoanSummaryViewModel> DisbursementCorrectionData = new List<LoanSummaryViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID};
                var alldata = groupwiseReportService.GetDisbursementCorrectionData(param, "Rpt_DisbursementCorrectionData");

                DisbursementCorrectionData = alldata.Tables[0].AsEnumerable()
                .Select(row => new LoanSummaryViewModel
                {
                    LoanSummaryID = row.Field<long>("LoanSummaryID"),
                     MemberCode = row.Field<string>("MemberCode"),
                    MemberName = row.Field<string>("MemberName"),
                    ProductCode = row.Field<string>("ProductCode"),
                    ProductName = row.Field<string>("ProductName"),
                    LoanTerm = row.Field<byte>("LoanTerm"),
                    DisburseDate = row.Field<DateTime>("DisburseDate"),
                    PrincipalLoan = row.Field<decimal>("PrincipalLoan"),
                    LoanRepaid = row.Field<decimal>("LoanRepaid")
                }).ToList();

                return Json(new { Result = "OK", Records = DisbursementCorrectionData, TotalRecordCount = DisbursementCorrectionData.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSavingSummaryCorrectionData(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                List<SavingInstallmentCorrectionViewModel> SavingSummaryCorrectionData = new List<SavingInstallmentCorrectionViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "GetSavingSummaryCorrectionData");

                SavingSummaryCorrectionData = alldata.Tables[0].AsEnumerable()
                .Select(row => new SavingInstallmentCorrectionViewModel
                {
                    SavingSummaryID = row.Field<long>("SavingSummaryID"),
                    MemberCode = row.Field<string>("MemberCode"),
                    //MemberName = row.Field<string>("MemberName"),
                    ProductCode = row.Field<string>("ProductCode"),
                    //ProductName = row.Field<string>("ProductName"),
                    NoOfAccount = row.Field<Int32>("NoOfAccount"),
                    ////OpeningDate = row.Field<DateTime>("OpeningDate"),
                    Deposit = row.Field<decimal>("Deposit"),
                    Withdrawal = row.Field<decimal>("Withdrawal"),
                    ProductCodeTo = row.Field<string>("NewProductCode"),
                    MemberCodeTo = row.Field<string>("NewMemberCode"),
                }).ToList();

                return Json(new { Result = "OK", Records = SavingSummaryCorrectionData, TotalRecordCount = SavingSummaryCorrectionData.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetMemberListForSInstallment(int centerId)
        {
            try
            {
                List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "GetMemberList_Dropdown_SavingInstallment");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new GetMemberListViewModel
                {
                    MemberID = row.Field<string>("MemberID"),
                     MemberName = row.Field<string>("MemberName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }// END Saving Installment
        public JsonResult GetCenterList()
        {
            List<CenterViewModel> List_CenterViewModel = new List<CenterViewModel>();
            var paramFOWISE = new { OfficeId = LoginUserOfficeID };
            var div_items = groupwiseReportService.GetDataDataseAccess(paramFOWISE, "GetOnlyCenter");

            List_CenterViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new CenterViewModel
            {
                CenterID = row.Field<int>("CenterID"),
                CenterCode = row.Field<string>("CenterCode"),
                CenterName = row.Field<string>("CenterName")
            }).ToList();

            var viewCenter = List_CenterViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + " " + x.CenterName.ToString()
            });
            var center_items = new List<SelectListItem>();
            center_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            center_items.AddRange(viewCenter);

            return Json(center_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemberList(int centerId)
        {
            try
            {
                List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "GetMemberList_Dropdown_LoanSummary");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new GetMemberListViewModel
                {
                    MemberID = row.Field<string>("MemberID"),
                    MemberName = row.Field<string>("MemberName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetMemberListAuto(string memberid, string centerId)
        {
            var MemberByCenterSessionKey_dis = string.Format("MemberByCenterSessionKey_dis_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey_dis] != null)
                memberList = Session[MemberByCenterSessionKey_dis] as List<Member>;
            else
            {
                // var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();

                List<Member> List_Members = new List<Member>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "GetMemberListLoanAutoCompleteDisburseMent_WriteOff");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new Member
                {
                    MemberID = row.Field<Int64>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    FirstName = row.Field<string>("MemberName"),
                    MemberCode = row.Field<string>("MemberCode"),
                }).ToList();

                Session[MemberByCenterSessionKey_dis] = List_Members; // mbr;
                memberList = List_Members; // mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSavingMemberList(int centerId)
        {
            try
            {
                List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "GetMemberList_Dropdown_SavingSummary");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new GetMemberListViewModel
                {
                    MemberID = row.Field<string>("MemberID"),
                    MemberName = row.Field<string>("MemberName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetMemberListAll(int centerId)
        {
            try
            {
                List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataDisbursementTransferMemberList(param, "GetMemberList_Dropdown_Member");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new GetMemberListViewModel
                {
                    MemberID = row.Field<string>("MemberID"),
                     MemberName = row.Field<string>("MemberName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
//============================================================
public JsonResult GetProductListByProc()
        {
            try
            {
                    var param = new { OrgId = SessionHelper.LoginUserOrganizationID };
                var result_set = groupwiseReportService.GetDataDisbursementTransferMemberList(param, "getProductByOrg");

                var getProduct = result_set.Tables[0].AsEnumerable().ToList();

                var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.Field<short>("ProductID").ToString(),
                    Text = x.Field<string>("ProductCode").ToString() + ", " + x.Field<string>("ProductName")
                });

                var prod_items = new List<SelectListItem>();
                if (viewProduct.ToList().Count > 0)
                {
                    prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
                }
                prod_items.AddRange(viewProduct);

                return Json(prod_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProductList()
        {
            //List<MemberwiseProductAndLoanTermViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndLoanTermViewModel>();

            //var param = new { Qtype = 1 };
            //var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndNoOfAccountforDropDown_SavingAccountCorrection");

            //List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
            //.Select(row => new MemberwiseProductAndLoanTermViewModel
            //{
            //    ProductID = row.Field<string>("ProductID"),
            //    ProductName = row.Field<string>("ProductName")

            //}).ToList();

            //return Json(List_MemberwiseProduct, JsonRequestBehavior.AllowGet);

            var getProduct = productService.GetAll().Where(s => s.IsActive == true && s.ProductType == 1 && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.ProductCode);
            var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductCode.ToString() + ", " + x.ProductName.ToString()
            });
            var prod_items = new List<SelectListItem>();
            if (viewProduct.ToList().Count > 0)
            {
                prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            prod_items.AddRange(viewProduct);
            return Json(prod_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSavingProductList()
        {
            var getProduct = productService.GetAll().Where(s => s.IsActive == true && s.ProductType == 0 && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.ProductCode);
            var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductCode.ToString() + ", " + x.ProductName.ToString()
            });
            var prod_items = new List<SelectListItem>();
            if (viewProduct.ToList().Count > 0)
            {
                prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            prod_items.AddRange(viewProduct);
            return Json(prod_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductListByMemberWithProcedure(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndLoanTermforDropDown_DisbursementCorrection");

                List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    ProductName = row.Field<string>("ProductName")

                }).ToList();

                return Json(List_MemberwiseProduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSavingProductListByMemberWithProcedure(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndNoOfAccountforDropDown_SavingAccountCorrection");

                List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    ProductName = row.Field<string>("ProductName")

                }).ToList();

                return Json(List_MemberwiseProduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLoanTermListByProductandMemberWithProcedure(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> List_LoanTermMemberandProductwise = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { Qtype = 2, MemberID = MemberID, ProductID = ProductID };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndLoanTermforDropDown_LoanSummary");

                List_LoanTermMemberandProductwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    LoanTerm = row.Field<string>("LoanTerm"),

                }).ToList();

                return Json(List_LoanTermMemberandProductwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetNoOfAccountListByProductandMemberWithProcedure(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> List_LoanTermMemberandProductwise = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { Qtype = 2, MemberID = MemberID, ProductID = ProductID };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndNoOfAccountforDropDown_SavingSummary");

                List_LoanTermMemberandProductwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    noAccount = row.Field<string>("LoanTerm"),

                }).ToList();

                return Json(List_LoanTermMemberandProductwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLoanSummaryIDListByProductandLoanTermWithProcedure(string CenterID, string MemberID, string ProductID, string LoanTerm)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> LoanSummaryIDLoantermwise = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = CenterID, MemberID = MemberID, ProductID = ProductID, LoanTerm = LoanTerm };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndLoanTermLoanSummaryIDforDropDown_LoanSummary");

                LoanSummaryIDLoantermwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    LoanSummaryID = row.Field<long>("LoanSummaryID")

                }).ToList();

                return Json(LoanSummaryIDLoantermwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSavingSummaryIDListByProductandLoanTermWithProcedure(string CenterID, string MemberID, string ProductID, string LoanTerm)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> LoanSummaryIDLoantermwise = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = CenterID, MemberID = MemberID, ProductID = ProductID, LoanTerm = LoanTerm };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndNOOFAccountSavingSummaryIDforDropDown_LoanSummary");

                LoanSummaryIDLoantermwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    LoanSummaryID = row.Field<long>("LoanSummaryID")

                }).ToList();

                return Json(LoanSummaryIDLoantermwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetInstallmentByProductandMemberWithProcedure(string CenterID, string MemberID, string ProductID, string Loanterm, string TrxDate)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> InstallmentAmount = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = CenterID, MemberID = MemberID, ProductID = ProductID, Loanterm = Loanterm, TrxDate = TrxDate };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "Member_Product_LoanTermwise_Disburse_Amount");

                InstallmentAmount = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    LoanSummaryIDPre = row.Field<long>("LoanSummaryIDPre"),
                    PrincipalLoan = row.Field<decimal>("PrincipalLoan"),
                    LoanRepaid = row.Field<decimal>("LoanRepaid")
                    

                }).ToList();

                return Json(InstallmentAmount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetInstallmentBySavingProductandMemberWithProcedure(string CenterID, string MemberID, string ProductID, string Loanterm, string TrxDate)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> InstallmentAmount = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = CenterID, MemberID = MemberID, ProductID = ProductID, Loanterm = Loanterm, TrxDate = TrxDate };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "Member_Product_NoOfAccountwise_SavingSummaryID");

                InstallmentAmount = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    LoanSummaryIDPre = row.Field<long>("LoanSummaryIDPre"),
                    PrincipalLoan = row.Field<decimal>("Deposit"),
                    LoanRepaid = row.Field<decimal>("Withdrawal")


                }).ToList();

                return Json(InstallmentAmount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DisburseAmountSave(string CenterIDTo, string ProductIDTo, string MemberIDTo, string PrincipalLoan, string LoanRepaid, string LoanSummaryID, string LoanSummaryIDPre, string ErrorDate)
                                                
        {
            try
            {
                var SummaryFrom = loanSummaryService.GetByLoanSummaryId(Convert.ToInt64(LoanSummaryIDPre));

                var storeProcedureName = "Proc_Set_MemberDisbursementTransfer";
                var param = new
                {
                    @LoanSummaryID = LoanSummaryIDPre,
                    @OfficeID = SummaryFrom.OfficeID,
                    @CenterID = SummaryFrom.CenterID,
                    @MemberID = SummaryFrom.MemberID,
                    @ProductId = SummaryFrom.ProductID,
                    @LoanTerm = SummaryFrom.LoanTerm,
                    @TrLoanSummaryID = LoanSummaryID,
                    @TrCenterID = CenterIDTo,
                    @TrMemberID = MemberIDTo,
                    @TrProductId = ProductIDTo,
                    @TrLoanTerm = SummaryFrom.LoanTerm,
                    @CreateUser = SessionHelper.LoginUserEmployeeID,
                    @CreateDate = DateTime.Now,
                    @transtype = 41
                };
                using (var gbData = new gBankerDataAccess())
                {
                    int query_result = gbData.ExecuteNonQuery(storeProcedureName, param);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SavingSummarySave(string CenterIDTo, string ProductIDTo, string MemberIDTo, string PrincipalLoan, string LoanRepaid, string LoanSummaryID, string LoanSummaryIDPre, string ErrorDate)

        {
            try
            {
                var SummaryFrom = savingSummaryService.GetBySavingSummaryId(Convert.ToInt64(LoanSummaryID));

                var storeProcedureName = "Proc_Set_SavingAccountTransfer";
                //var param = new
                //{
                //    @LoanSummaryID = LoanSummaryID,
                //    @OfficeID = SummaryFrom.OfficeID,
                //    @CenterID = SummaryFrom.CenterID,
                //    @MemberID = SummaryFrom.MemberID,
                //    @ProductId = SummaryFrom.ProductID,
                //    @LoanTerm = SummaryFrom.NoOfAccount,
                //    @TrLoanSummaryID = LoanSummaryID,
                //    @TrCenterID = CenterIDTo,
                //    @TrMemberID = MemberIDTo,
                //    @TrProductId = ProductIDTo,
                //    @TrLoanTerm = SummaryFrom.NoOfAccount,
                //    @CreateUser = SessionHelper.LoginUserEmployeeID,
                //    @CreateDate = DateTime.Now,
                //    @transtype = 41
                //};
                var param = new
                {
                    @LoanSummaryID = LoanSummaryID,
                    @OfficeID = SummaryFrom.OfficeID,
                    @CenterID = SummaryFrom.CenterID,
                    @MemberID = SummaryFrom.MemberID,
                    @ProductId = SummaryFrom.ProductID,
                    @LoanTerm = SummaryFrom.NoOfAccount,
                    @TrLoanSummaryID = LoanSummaryID,
                    @TrCenterID = CenterIDTo,
                    @TrMemberID = MemberIDTo,
                    @TrProductId = ProductIDTo,
                    @TrLoanTerm = SummaryFrom.NoOfAccount,
                    @CreateUser = SessionHelper.LoginUserEmployeeID,
                    @CreateDate = DateTime.Now,
                    @transtype = 41
                };
                using (var gbData = new gBankerDataAccess())
                {
                    int query_result = gbData.ExecuteNonQuery(storeProcedureName, param);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Events
        public ActionResult DisbursementCorrection()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;

            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult SavingSummaryCorrection()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["NoAccount"] = items;

            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        //
        // GET: /Disbursement/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SavingSummaryIndex()
        {
            return View();
        }
        //
        // GET: /Disbursement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        //
        // GET: /Disbursement/Create
        public ActionResult Create()
        {
            return View();
        }
        //
        // POST: /Disbursement/Create
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
        //
        // GET: /Disbursement/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        //
        // POST: /Disbursement/Edit/5
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
        //
        // GET: /Disbursement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        //
        // POST: /Disbursement/Delete/5
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
        #endregion

    }
}

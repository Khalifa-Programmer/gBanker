using AutoMapper;
using AutoMapper.Internal;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Windows;

namespace gBanker.Web.Controllers
{
    public class SavingUpdateController : BaseController
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
        private readonly IUltimateReportService ultimateReportService;
        public SavingUpdateController
            (
            IEmployeeService employeeService,
            IOfficeService officeService,
            IProductService productService,
            ICenterService centerService,
            IMemberService memberService,
            IGroupwiseReportService groupwiseReportService,
            IUltimateReportService unlimitedReportService,
            IPOMISReportService pomisReportService,
            IWeeklyReportService weeklyReportService,
            ILoanTrxService loanTrxService,
            ILoanSummaryService loanSummaryService,
            IDailyLoanTrxService dailyLoanTrxService,
            ISpecialLoanCollectionService specialLoanCollectionService,
            ISavingSummaryService savingSummaryService,
            IUltimateReportService ultimateReportService
            )
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
            this.ultimateReportService = ultimateReportService;
        }
        #endregion
        //===================================================================================//
        //=============================== Saving Summary Update =============================//
        //===================================================================================//

        // Saving Summary Update View Page
        public ActionResult SavingSummaryUpdate()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["LoggedInUser"] = LoggedInEmployee.EmpName;
            ViewData["LoggedInOfficeID"] = LoggedInEmployee.OfficeID;

            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["NoOAccList"] = items;
            return View();
        }

        // GET Saving Summary List According to Center,Center,Product & NoOfAccount Wise
        public JsonResult GETSavingUpdateList(int jtStartIndex, int jtPageSize, string jtSorting, int CenterID = 0, long MemberID = 0, int ProductID = 0, int NoofAccount = 0, string Option = "")
        {
            try
            {
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = CenterID, @MemberID = MemberID, @ProductID = ProductID, @NoOfAccount = NoofAccount};
                var spData = ultimateReportService.GetDataWithParameter(param, "Proc_GetSavingSummaryListForUpdate");
                var detail = spData.Tables[0].AsEnumerable()
                    .Select(x => new SavingUpdateViewModel
                    {
                        SavingSummaryID = x.Field<long>("SavingSummaryID"),
                        CenterID = x.Field<int>("CenterID"),
                        CenterCode = x.Field<string>("CenterCode"),
                        CenterName = x.Field<string>("CenterName"),
                        MemberID = x.Field<long>("MemberID"),
                        MemberCode = x.Field<string>("MemberCode"),
                        MemberName = x.Field<string>("MemberName"),
                        ProductID = x.Field<short>("ProductID"),
                        ProductCode = x.Field<string>("ProductCode"),
                        ProductName = x.Field<string>("ProductName"),
                        NoOfAccount = x.Field<int>("NoOfAccount"),
                        TransactionDate = x.Field<string>("TransactionDate"),
                        Deposit = x.Field<decimal>("Deposit"),
                        Withdrawal = x.Field<decimal>("Withdrawal"),
                        InterestRate = x.Field<decimal>("InterestRate"),
                        SavingsInstallment = x.Field<decimal>("SavingsInstallment"),
                        CumInterest = x.Field<decimal>("CumInterest"),
                        MonthlyInterest = x.Field<decimal>("MonthlyInterest"),
                        Penalty = x.Field<decimal>("Penalty"),
                        ClosingDate = x.Field<string>("ClosingDate"),
                        SavingStatus = x.Field<byte>("SavingStatus")
                    }).ToList();

                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);

                List<SavingUpdateViewModel> viewList = new List<SavingUpdateViewModel>();
                int rowSl = 0;
                foreach (var item in detail.OrderBy(x => x.TransactionDate))
                {
                    var lists = new SavingUpdateViewModel()
                    {
                        SavingSummaryID = item.SavingSummaryID,
                        CenterID = item.CenterID,
                        CenterCode = item.CenterCode,
                        CenterName = item.CenterName,
                        MemberID = item.MemberID,
                        MemberCode = item.MemberCode,
                        MemberName = item.MemberName,
                        ProductID = item.ProductID,
                        ProductCode = item.ProductCode,
                        ProductName = item.ProductName,
                        NoOfAccount = item.NoOfAccount,
                        TransactionDate = item.TransactionDate,
                        Deposit = item.Deposit,
                        Withdrawal = item.Withdrawal,
                        InterestRate = item.InterestRate,
                        SavingsInstallment = item.SavingsInstallment,
                        CumInterest = item.CumInterest,
                        MonthlyInterest = item.MonthlyInterest,
                        Penalty = item.Penalty,
                        ClosingDate = item.ClosingDate,
                        SavingStatus = item.SavingStatus
                    };
                    viewList.Add(lists);
                    rowSl++;
                }

                return Json(new { Result = "OK", Records = viewList, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        //Member Auto Complete
        public ActionResult GetMemberListAuto(string memberid, string centerId)
        {
            var MemberByCenterSessionKeySavings = string.Format("MemberByCenterSessionKeySavings_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKeySavings] != null)
                memberList = Session[MemberByCenterSessionKeySavings] as List<Member>;
            else
            {

                List<Member> List_Members = new List<Member>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = ultimateReportService.GetDataWithParameter(param, "Proc__Get_MemberList_AutoComplete");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new Member
                {
                    MemberID = row.Field<Int64>("MemberID"),
                    FirstName = row.Field<string>("MemberName"),
                    MemberCode = row.Field<string>("MemberCode"),
                }).ToList();

                Session[MemberByCenterSessionKeySavings] = List_Members; // mbr;

                memberList = List_Members; // mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }



        #region Methods (JSON return)=========================================================

        //Center List For DropDown
        public JsonResult GetCenterList()
        {
            var CenterByCenterSessionKeySavings = string.Format("CenterByCenterSessionKeySavings_{0}", LoginUserOfficeID);
            var centerList = new List<CenterViewModel>();
            if (Session[CenterByCenterSessionKeySavings] != null)
                centerList = Session[CenterByCenterSessionKeySavings] as List<CenterViewModel>;
            else
            {
                List<CenterViewModel> List_Center = new List<CenterViewModel>();

                // Qtype = 1 is for center list from the procedure
                var param = new { @QType = 1, @OrgID = LoggedInOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID };
                var alldata = ultimateReportService.GetDataWithParameter(param, "Proc__Get_Center_MemberCategory_Purpose_Product_Only___QTypeWise");

                List_Center = alldata.Tables[0].AsEnumerable()
                .Select(row => new CenterViewModel
                {
                    CenterID = row.Field<int>("CenterID"),
                    CenterCode = row.Field<string>("CenterCode"),
                    CenterName = row.Field<string>("CenterName")

                }).ToList();

                Session[CenterByCenterSessionKeySavings] = List_Center; // mbr;

                centerList = List_Center; // mbr;
            }

            var viewCenter = centerList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + " " + x.CenterName.ToString()
            });
            var center_items = new List<SelectListItem>();
            center_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            center_items.AddRange(viewCenter);

            return Json(center_items, JsonRequestBehavior.AllowGet);
        }
        
        //Member Category List For DropDown
        public JsonResult GetMemberCategoryList()
        {
            var MemberCategoryByCenterSessionKeySavings = string.Format("MemberCategoryByCenterSessionKeySavings_{0}", LoginUserOfficeID);
            var memberCategoryList = new List<MemberCategoryViewModel>();
            if (Session[MemberCategoryByCenterSessionKeySavings] != null)
                memberCategoryList = Session[MemberCategoryByCenterSessionKeySavings] as List<MemberCategoryViewModel>;
            else
            {
                List<MemberCategoryViewModel> List_memberCategory = new List<MemberCategoryViewModel>();

                var param = new { @QType = 2, @OrgID = LoggedInOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID };
                var alldata = ultimateReportService.GetDataWithParameter(param, "Proc__Get_Center_MemberCategory_Purpose_Product_Only___QTypeWise");

                List_memberCategory = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberCategoryViewModel
                {
                    MemberCategoryID = row.Field<byte>("MemberCategoryID"),
                    MemberCategoryCode = row.Field<string>("MemberCategoryCode"),
                    CategoryName = row.Field<string>("CategoryName")

                }).ToList();

                Session[MemberCategoryByCenterSessionKeySavings] = List_memberCategory; // mbr;

                memberCategoryList = List_memberCategory; // mbr;
            }

            var viewCenter = memberCategoryList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MemberCategoryID.ToString(),
                Text = x.MemberCategoryCode.ToString() + " " + x.CategoryName.ToString()
            });
            var center_items = new List<SelectListItem>();
            center_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            center_items.AddRange(viewCenter);

            return Json(center_items, JsonRequestBehavior.AllowGet);
        }

        //// Get Member List by CenterID
        //public JsonResult GetMemberList(int centerId)
        //{
        //    try
        //    {
        //        List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

        //        var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
        //        var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "GetMemberList_Dropdown_LoanSummary");

        //        List_Members = alldata.Tables[0].AsEnumerable()
        //        .Select(row => new GetMemberListViewModel
        //        {
        //            MemberID = row.Field<string>("MemberID"),
        //            MemberName = row.Field<string>("MemberName")

        //        }).ToList();

        //        return Json(List_Members, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //Get Product List by Member 
        public JsonResult GetProductListByMemberWithProcedure(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<SavingUpdateViewModel> List_MemberwiseProduct = new List<SavingUpdateViewModel>();

                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "Proc_GetProduct_NoOfAccount__SavingSummary");

                List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
                .Select(row => new SavingUpdateViewModel
                {
                    ProductID = row.Field<short>("ProductID"),
                    ProductName = row.Field<string>("ProductName")

                }).ToList();

                return Json(List_MemberwiseProduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Get Product List For Grid DDL
        public JsonResult GetProductListProcedure()
        {
            try
            {
                var ProductByProductSessionKeySavings = string.Format("ProductByProductSessionKeySavings_{0}", LoginUserOfficeID);

                var productList = new List<MemberwiseProductAndLoanTermViewModel>();
                
                if(Session[ProductByProductSessionKeySavings] != null)
                {
                    productList = Session[ProductByProductSessionKeySavings] as List<MemberwiseProductAndLoanTermViewModel>;
                }
                else
                {
                    List<MemberwiseProductAndLoanTermViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndLoanTermViewModel>();
                    var param = new { Qtype = 1 };
                    var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "GET_Product_List__SavingSummary");
                    List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
                    .Select(row => new MemberwiseProductAndLoanTermViewModel
                    {
                        ProductID = row.Field<string>("ProductID"),
                        ProductName = row.Field<string>("ProductName")

                    }).ToList();

                    Session[ProductByProductSessionKeySavings] = List_MemberwiseProduct;
                    productList = List_MemberwiseProduct;
                }
                

                var viewCenter = productList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.ProductID.ToString(),
                    Text = x.ProductName.ToString()
                });
                var product_items = new List<SelectListItem>();
                //product_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
                product_items.AddRange(viewCenter);

                return Json(product_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }




        //Get No Of Account by Member & Product wise
        public JsonResult GetNoOfAccountWithMemberAndProduct(string MemberID, string ProductID)
        {
            try
            {
                List<SavingUpdateViewModel> List_NoOfAccount = new List<SavingUpdateViewModel>();

                var param = new {QType = 2, MemberID = MemberID, ProductID = ProductID };
                var alldata = ultimateReportService.GetDataWithParameter(param, "Proc_GetProduct_NoOfAccount__SavingSummary");

                List_NoOfAccount = alldata.Tables[0].AsEnumerable()
                .Select(row => new SavingUpdateViewModel
                {
                    NoOfAccountValue = row.Field<string>("NoOfAccountValue"),
                    NoOfAccountText = row.Field<string>("NoOfAccountText")

                }).ToList();

                return Json(List_NoOfAccount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion =================================================================



        [HttpPost]
        public ActionResult SaveSavingummaryUpdate(Dictionary<string, string> allTrx, List<string> allSavingTrxId,
            string center,
            string member,
            string product,
            string noOfAccount
            )
        {
            try
            {
                var trx = allTrx;

                var trxId = 1;
                var loanTrxIds = allSavingTrxId.Where(w => int.TryParse(w, out trxId));

                //var loanTrxRegisterCollection = new List<LoanTrx>();
                SavingUpdateViewModel obj = new SavingUpdateViewModel();
                foreach (var id in trx)  //foreach (var id in loanTrxIds)
                {
                    string[] v = id.Key.Split(',');
                    var SavingSummaryID = "";
                    var FieldName = "";
                    if (v.Length > 1)
                    {
                        SavingSummaryID = v[1];
                        FieldName = v[0];
                    }
                    else
                    {
                        SavingSummaryID = "0";
                        FieldName = v[0];
                    }

                    var value = id.Value;
                    obj.SavingSummaryID = Convert.ToInt64(SavingSummaryID);

                    if (FieldName == "txtSelectedMemberId")
                    {
                        obj.MemberID = Convert.ToInt64(value);
                    }
                    if (FieldName == "txtSelectedProductId")
                    {
                        obj.ProductID = Convert.ToInt16(value);
                    }
                    if (FieldName == "txtSelectedCenterId")
                    {
                        obj.CenterID = Convert.ToInt32(value);
                    }

                    if (FieldName == "NoOfAccount")
                    {
                        obj.NoOfAccount = Convert.ToInt32(value);
                    }
                    if (FieldName == "TransactionDate")
                    {
                        obj.TransactionDate = Convert.ToString(value);
                    }
                    if (FieldName == "Deposit")
                    {
                        obj.Deposit = Convert.ToDecimal(value);
                    }
                    if (FieldName == "Withdrawal")
                    {
                        obj.Withdrawal = Convert.ToDecimal(value);
                    }
                    if (FieldName == "InterestRate")
                    {
                        obj.InterestRate = Convert.ToDecimal(value);
                    }
                    if (FieldName == "SavingsInstallment")
                    {
                        obj.SavingsInstallment = Convert.ToDecimal(value);
                    }
                    if (FieldName == "CumInterest")
                    {
                        obj.CumInterest = Convert.ToDecimal(value);
                    }
                    if (FieldName == "MonthlyInterest")
                    {
                        obj.MonthlyInterest = Convert.ToDecimal(value);
                    }
                    if (FieldName == "Penalty")
                    {
                        obj.Penalty = Convert.ToDecimal(value);
                    }
                    if (FieldName == "ClosingDate")
                    {
                        obj.ClosingDate = Convert.ToString(value);
                    }
                    if (FieldName == "SavingStatus")
                    {
                        obj.SavingStatus = Convert.ToByte(value);

                        // insert into Table
                        var param = new
                        {
                            SavingSummaryID = obj.SavingSummaryID,
                            OfficeID = SessionHelper.LoginUserOfficeID,
                            MemberID = obj.MemberID,
                            ProductID = obj.ProductID,
                            CenterID = obj.CenterID,

                            NoOfAccount = obj.NoOfAccount,
                            TransactionDate = obj.TransactionDate == "null" ? "" : obj.TransactionDate,

                            Deposit = obj.Deposit,
                            Withdrawal = obj.Withdrawal,
                            InterestRate = obj.InterestRate,
                            SavingsInstallment = obj.SavingsInstallment,
                            CumInterest = obj.CumInterest,
                            MonthlyInterest = obj.MonthlyInterest,
                            Penalty = obj.Penalty,
                            ClosingDate = obj.ClosingDate == "null" ? "" : obj.ClosingDate,
                            SavingStatus = obj.SavingStatus,
                            CreateUser = SessionHelper.LoginUserEmployeeID,
                            CreateDate = DateTime.Now.Date.ToString("dd-MMM-yyyy")

                        };

                        var val = ultimateReportService.GetDataWithParameter(param, "Proc_InsertSavingSummaryUpdate");
                        //var val = ultimateReportService.InsertLOANRegisterUpdateINFO(param);

                        obj = new SavingUpdateViewModel(); // After Save and Update Create New Object
                    }

                }

                // savingCollectionService.SaveDailysavingCollection(savingTrxViewCollection);

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }// END




        //===================================================================================//
        //================================= Saving Trx Update ===============================//
        //===================================================================================//

        public ActionResult SavingTrxUpdate()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["LoggedInUser"] = LoggedInEmployee.EmpName;
            ViewData["LoggedInOfficeID"] = LoggedInEmployee.OfficeID;
            ViewData["TransactionDate"] = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["NoOAccList"] = items;
            return View();
        }

        //Load Saving Trx Data Into Grid
        public JsonResult GETSavingTrxUpdateList(int jtStartIndex, int jtPageSize, string jtSorting, int CenterID = 0, long MemberID = 0, int ProductID = 0, int NoOfAccount = 0, string Option = "")
        {
            try
            {
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = CenterID, @MemberID = MemberID, @ProductID = ProductID, @NoOfAccount = NoOfAccount };
                var spData = ultimateReportService.GetDataWithParameter(param, "Proc_GetSavingTrxListForUpdate");
                var detail = spData.Tables[0].AsEnumerable()
                    .Select(x => new SavingUpdateViewModel
                    {
                        SavingTrxID = x.Field<Int64>("SavingTrxID"),
                        SavingSummaryID = x.Field<Int64>("SavingSummaryID"),
                        CenterID = x.Field<int>("CenterID"),
                        CenterCode = x.Field<string>("CenterCode"),
                        CenterName = x.Field<string>("CenterName"),
                        OfficeID = x.Field<int>("OfficeID"),
                        OfficeCode = x.Field<string>("OfficeCode"),
                        OfficeName = x.Field<string>("OfficeName"),
                        MemberID = x.Field<Int64>("MemberID"),
                        MemberCode = x.Field<string>("MemberCode"),
                        MemberName = x.Field<string>("MemberName"),
                        ProductID = x.Field<short>("ProductID"),
                        ProductCode = x.Field<string>("ProductCode"),
                        ProductName = x.Field<string>("ProductName"),
                        NoOfAccount = x.Field<int>("NoOfAccount"),
                        TransactionDate = x.Field<string>("TransactionDate"),
                        Deposit = x.Field<decimal>("Deposit"),
                        Withdrawal = x.Field<decimal>("Withdrawal"),
                        Balance = x.Field<decimal>("Balance"),
                        Penalty = x.Field<decimal>("Penalty"),
                        TransType = x.Field<byte>("TransType"),
                        MonthlyInterest = x.Field<decimal>("MonthlyInterest"),
                        PresenceInd = x.Field<bool>("PresenceInd"),
                        TransferDeposit = x.Field<decimal>("TransferDeposit"),
                        TransferWithdrawal = x.Field<decimal>("TransferWithdrawal"),
                        EmployeeID = x.Field<short>("EmployeeID"),
                        MemberCategoryID = x.Field<byte>("MemberCategoryID"),
                        MemberCategoryCode = x.Field<string>("MemberCategoryCode"),
                        CategoryName = x.Field<string>("CategoryName"),
                        CreateUser = x.Field<string>("CreateUser"),
                        CreateDate = x.Field<string>("CreateDate")
                    }).ToList();

                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);

                List<SavingUpdateViewModel> viewList = new List<SavingUpdateViewModel>();
                int rowSl = 0;
                foreach (var item in detail.OrderBy(x => x.TransactionDate))
                {
                    var lists = new SavingUpdateViewModel()
                    {
                        SavingTrxID = item.SavingTrxID,
                        SavingSummaryID = item.SavingSummaryID,
                        CenterID = item.CenterID,
                        CenterCode = item.CenterCode,
                        CenterName = item.CenterName,
                        OfficeID = item.OfficeID,
                        OfficeCode = item.OfficeCode,
                        OfficeName = item.OfficeName,
                        MemberID = item.MemberID,
                        MemberCode = item.MemberCode,
                        MemberName = item.MemberName,
                        ProductID = item.ProductID,
                        ProductCode = item.ProductCode,
                        ProductName = item.ProductName,
                        NoOfAccount = item.NoOfAccount,
                        TransactionDate = item.TransactionDate,
                        Deposit = item.Deposit,
                        Withdrawal = item.Withdrawal,
                        Balance = item.Balance,
                        Penalty = item.Penalty,
                        TransType = item.TransType,
                        MonthlyInterest = item.MonthlyInterest,
                        PresenceInd = item.PresenceInd,
                        TransferDeposit = item.TransferDeposit,
                        TransferWithdrawal = item.TransferWithdrawal,
                        EmployeeID = item.EmployeeID,
                        MemberCategoryID = item.MemberCategoryID,
                        MemberCategoryCode = item.MemberCategoryCode,
                        CategoryName = item.CategoryName,
                        CreateUser = item.CreateUser,
                        CreateDate = item.CreateDate
                    };
                    viewList.Add(lists);
                    rowSl++;
                }

                return Json(new { Result = "OK", Records = viewList, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }




        // Saving Trx Update ----- POST

        [HttpPost]
        public ActionResult SaveSavingTrxUpdate(Dictionary<string, string> allTrx, List<string> allSavingTrxId,
            string center,
            string member,
            string product,
            string noOfAccount
            )
        {
            try
            {
                var trx = allTrx;

                var trxId = 1;
                var loanTrxIds = allSavingTrxId.Where(w => int.TryParse(w, out trxId));

                SavingUpdateViewModel obj = new SavingUpdateViewModel();
                foreach (var id in trx)  //foreach (var id in loanTrxIds)
                {
                    string[] v = id.Key.Split(',');
                    var SavingTrxID = "";
                    var FieldName = "";
                    if (v.Length > 1)
                    {
                        SavingTrxID = v[1];
                        FieldName = v[0];
                    }
                    else
                    {
                        SavingTrxID = "0";
                        FieldName = v[0];
                    }

                    var value = id.Value;
                    obj.SavingTrxID = Convert.ToInt64(SavingTrxID);
                    if(FieldName == "SavingSummaryID")
                    {
                        obj.SavingSummaryID = Convert.ToInt64(value);
                    }

                    if (FieldName == "txtSelectedMemberId")
                    {
                        obj.MemberID = Convert.ToInt64(value);
                    }
                    if (FieldName == "txtSelectedProductId")
                    {
                        obj.ProductID = Convert.ToInt16(value);
                    }
                    if (FieldName == "txtSelectedCenterId")
                    {
                        obj.CenterID = Convert.ToInt32(value);
                    }
                    if (FieldName == "txtSelectedMemberCategoryId")
                    {
                        obj.MemberCategoryID = Convert.ToByte(value);
                    }

                    if (FieldName == "NoOfAccount")
                    {
                        obj.NoOfAccount = Convert.ToInt32(value);
                    }
                    if (FieldName == "TransactionDate")
                    {
                        obj.TransactionDate = Convert.ToString(value);
                    }
                    if (FieldName == "Deposit")
                    {
                        obj.Deposit = Convert.ToDecimal(value);
                    }
                    if (FieldName == "Withdrawal")
                    {
                        obj.Withdrawal = Convert.ToDecimal(value);
                    }
                    if (FieldName == "Balance")
                    {
                        obj.Balance = Convert.ToDecimal(value);
                    }
                    if (FieldName == "TransferDeposit")
                    {
                        obj.TransferDeposit = Convert.ToDecimal(value);
                    }
                    if (FieldName == "MonthlyInterest")
                    {
                        obj.MonthlyInterest = Convert.ToDecimal(value);
                    }
                    if (FieldName == "Penalty")
                    {
                        obj.Penalty = Convert.ToDecimal(value);
                    }
                    if (FieldName == "TransType")
                    {
                        obj.TransType = Convert.ToByte(value);
                    }
                    if (FieldName == "TransferWithdrawal")
                    {
                        obj.TransferWithdrawal = Convert.ToDecimal(value);

                        // insert into Table
                        var param = new
                        {
                            SavingTrxID = obj.SavingTrxID,
                            SavingSummaryID = obj.SavingSummaryID,
                            OfficeID = SessionHelper.LoginUserOfficeID,
                            MemberID = obj.MemberID,
                            ProductID = obj.ProductID,
                            CenterID = obj.CenterID,
                            MemberCategoryID = obj.MemberCategoryID,

                            NoOfAccount = obj.NoOfAccount,
                            TransactionDate = obj.TransactionDate == "null" ? "" : obj.TransactionDate,

                            Deposit = obj.Deposit,
                            Withdrawal = obj.Withdrawal,
                            Balance = obj.Balance,
                            TransferDeposit = obj.TransferDeposit,
                            TransferWithdrawal = obj.TransferWithdrawal,
                            MonthlyInterest = obj.MonthlyInterest,
                            Penalty = obj.Penalty,
                            TransType = obj.TransType,
                            CreateUser = SessionHelper.LoginUserEmployeeID,
                            CreateDate = DateTime.Now.Date.ToString("dd-MMM-yyyy")

                        };

                       var val = ultimateReportService.GetDataWithParameter(param, "Proc_InsertSavingTrxUpdate");

                        obj = new SavingUpdateViewModel(); // After Save and Update Create New Object
                    }

                }

                // savingCollectionService.SaveDailysavingCollection(savingTrxViewCollection);

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }// END

    }
}
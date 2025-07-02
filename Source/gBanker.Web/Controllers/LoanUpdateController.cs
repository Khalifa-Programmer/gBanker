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
    public class LoanUpdateController : BaseController
    {
        #region Variables
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IUltimateReportService ultimateReportService;
        public LoanUpdateController( IGroupwiseReportService groupwiseReportService, IUltimateReportService ultimateReportService)
        {
            this.groupwiseReportService = groupwiseReportService;
            this.ultimateReportService = ultimateReportService;
        }
        #endregion
        //========== View Page
        public ActionResult LoanSummaryUpdate()
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
        //========= Data Loan Into Grid From SP
        public JsonResult GETLoanUpdateList(int jtStartIndex, int jtPageSize, string jtSorting, int CenterID = 0, long MemberID = 0, int ProductID = 0, int LoanTerm = 0, string Option = "")
        {
            try
            {
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = CenterID, @MemberID = MemberID, @ProductID = ProductID, @LoanTerm = LoanTerm };
                var spData = ultimateReportService.GetDataWithParameter(param, "Proc_GetLoanSummaryListForUpdate");
                var detail = spData.Tables[0].AsEnumerable()
                    .Select(x => new LoanUpdateViewModel
                    {
                        LoanSummaryID = x.Field<Int64>("LoanSummaryID"),
                        MemberID = x.Field<Int64>("MemberID"),
                        MemberCode = x.Field<string>("MemberCode"),
                        MemberName = x.Field<string>("MemberName"),
                        ProductID = x.Field<Int16>("ProductID"),
                        ProductCode = x.Field<string>("ProductCode"),
                        ProductName = x.Field<string>("ProductName"),
                        CenterID = x.Field<int>("CenterID"),
                        CenterCode = x.Field<string>("CenterCode"),
                        CenterName = x.Field<string>("CenterName"),
                        MemberCategoryID = x.Field<byte>("MemberCategoryID"),
                        MemberCategoryCode = x.Field<string>("MemberCategoryCode"),
                        CategoryName = x.Field<string>("CategoryName"),
                        PurposeID = x.Field<Int16>("PurposeID"),
                        PurposeCode = x.Field<string>("PurposeCode"),
                        PurposeName = x.Field<string>("PurposeName"),
                        LoanTerm = x.Field<byte>("LoanTerm"),
                        PrincipalLoan = x.Field<decimal>("PrincipalLoan"),
                        ApproveDate = x.Field<string>("ApproveDate"),
                        DisburseDate = x.Field<string>("DisburseDate"),
                        Duration = x.Field<int>("Duration"),
                        LoanRepaid = x.Field<decimal>("LoanRepaid"),
                        IntCharge = x.Field<decimal>("IntCharge"),
                        IntPaid = x.Field<decimal>("IntPaid"),
                        LoanInstallment = x.Field<decimal>("LoanInstallment"),
                        IntInstallment = x.Field<decimal>("IntInstallment"),
                        InterestRate = x.Field<decimal>("InterestRate"),
                        InstallmentNo = x.Field<int>("InstallmentNo"),
                        InstallmentDate = x.Field<string>("InstallmentDate"),
                        LoanStatus = x.Field<byte>("LoanStatus"),
                        PartialAmount = x.Field<decimal>("PartialAmount"),
                        PartialIntCharge = x.Field<decimal>("PartialIntCharge"),
                        PartialIntPaid = x.Field<decimal>("PartialIntPaid")
                    }).ToList();

                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);

                List<LoanUpdateViewModel> viewList = new List<LoanUpdateViewModel>();
                int rowSl = 0;
                foreach (var item in detail.OrderBy(x => x.DisburseDate))
                {
                    var lists = new LoanUpdateViewModel()
                    {
                        LoanSummaryID = item.LoanSummaryID,
                        MemberID = item.MemberID,
                        MemberCode = item.MemberCode,
                        MemberName = item.MemberName,
                        ProductID = item.ProductID,
                        ProductCode = item.ProductCode,
                        ProductName = item.ProductName,
                        CenterID = item.CenterID,
                        CenterCode = item.CenterCode,
                        CenterName = item.CenterName,
                        MemberCategoryID = item.MemberCategoryID,
                        MemberCategoryCode = item.MemberCategoryCode,
                        CategoryName = item.CategoryName,
                        PurposeID = item.PurposeID,
                        PurposeCode = item.PurposeCode,
                        PurposeName = item.PurposeName,
                        LoanTerm = item.LoanTerm,
                        PrincipalLoan = item.PrincipalLoan,
                        ApproveDate = item.ApproveDate,
                        DisburseDate = item.DisburseDate,
                        Duration = item.Duration,
                        LoanRepaid = item.LoanRepaid,
                        IntCharge = item.IntCharge,
                        IntPaid = item.IntPaid,
                        LoanInstallment = item.LoanInstallment,
                        IntInstallment = item.IntInstallment,
                        InterestRate = item.InterestRate,
                        InstallmentNo = item.InstallmentNo,
                        InstallmentDate = item.InstallmentDate,
                        LoanStatus = item.LoanStatus,
                        PartialAmount = item.PartialAmount,
                        PartialIntCharge = item.PartialIntCharge,
                        PartialIntPaid = item.PartialIntPaid
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
            var MemberByCenterSessionKey__Loan = string.Format("MemberByCenterSessionKey__Loan_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey__Loan] != null)
                memberList = Session[MemberByCenterSessionKey__Loan] as List<Member>;
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

                Session[MemberByCenterSessionKey__Loan] = List_Members; // mbr;

                memberList = List_Members; // mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }



        #region Methods (JSON return)=========================================================

        public JsonResult GetCenterList()
        {
            var CenterByCenterSessionKey__Loan = string.Format("CenterByCenterSessionKey__Loan_{0}", LoginUserOfficeID);
            var centerList = new List<CenterViewModel>();
            if (Session[CenterByCenterSessionKey__Loan] != null)
                centerList = Session[CenterByCenterSessionKey__Loan] as List<CenterViewModel>;
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

                Session[CenterByCenterSessionKey__Loan] = List_Center; // mbr;

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

        //Purpose List For DropDown
        public JsonResult GetPurposeList()
        {
            var PurposeByCenterSessionKey_Loan = string.Format("PurposeByCenterSessionKey_Loan_{0}", LoginUserOfficeID);
            var purposeList = new List<PurposeViewModel>();
            if (Session[PurposeByCenterSessionKey_Loan] != null)
                purposeList = Session[PurposeByCenterSessionKey_Loan] as List<PurposeViewModel>;
            else
            {
                List<PurposeViewModel> List_Purpose = new List<PurposeViewModel>();

                var param = new { @QType = 3, @OrgID = LoggedInOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID };
                var alldata = ultimateReportService.GetDataWithParameter(param, "Proc__Get_Center_MemberCategory_Purpose_Product_Only___QTypeWise");

                List_Purpose = alldata.Tables[0].AsEnumerable()
                .Select(row => new PurposeViewModel
                {
                    PurposeID = row.Field<int>("PurposeID"),
                    PurposeCode = row.Field<string>("PurposeCode"),
                    PurposeName = row.Field<string>("PurposeName")

                }).ToList();

                Session[PurposeByCenterSessionKey_Loan] = List_Purpose; // mbr;

                purposeList = List_Purpose; // mbr;
            }

            var viewCenter = purposeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.PurposeID.ToString(),
                Text = x.PurposeCode.ToString() + " " + x.PurposeName.ToString()
            });
            var center_items = new List<SelectListItem>();
            center_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            center_items.AddRange(viewCenter);

            return Json(center_items, JsonRequestBehavior.AllowGet);
        }

        //Member Category List For DropDown
        public JsonResult GetMemberCategoryList()
        {
            var MemberCategoryByCenterSessionKey_Loan = string.Format("MemberCategoryByCenterSessionKey_Loan_{0}", LoginUserOfficeID);
            var memberCategoryList = new List<MemberCategoryViewModel>();
            if (Session[MemberCategoryByCenterSessionKey_Loan] != null)
                memberCategoryList = Session[MemberCategoryByCenterSessionKey_Loan] as List<MemberCategoryViewModel>;
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

                Session[MemberCategoryByCenterSessionKey_Loan] = List_memberCategory; // mbr;

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


        //Get Product List by Member 
        public JsonResult GetProductListByMemberWithProcedure(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = ultimateReportService.GetDataWithParameter(param, "Proc__Get_Product_And_LoanTerm___QTypeWise");

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

        //Get Loan Term by Member & Product wise
        public JsonResult GetLoanTermListByProductandMemberWithProcedure(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> List_LoanTermMemberandProductwise = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { Qtype = 2, MemberID = MemberID, ProductID = ProductID };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "Proc__Get_Product_And_LoanTerm___QTypeWise");

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

        //Get Product List For Grid DDL
        public JsonResult GetProductListProcedure()
        {
            try
            {
                var ProductByProductSessionKey_Loan = string.Format("ProductByProductSessionKey_Loan_{0}", LoginUserOfficeID);
                var productList = new List<MemberwiseProductAndLoanTermViewModel>();

                if (Session[ProductByProductSessionKey_Loan] != null)
                    productList = Session[ProductByProductSessionKey_Loan] as List<MemberwiseProductAndLoanTermViewModel>;
                else
                {
                    List<MemberwiseProductAndLoanTermViewModel> List_Product = new List<MemberwiseProductAndLoanTermViewModel>();
                    var param = new { @QType = 4, @OrgID = LoggedInOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID };
                    var alldata = ultimateReportService.GetDataWithParameter(param, "Proc__Get_Center_MemberCategory_Purpose_Product_Only___QTypeWise");

                    List_Product = alldata.Tables[0].AsEnumerable()
                    .Select(row => new MemberwiseProductAndLoanTermViewModel
                    {
                        ProductID = row.Field<string>("ProductID"),
                        ProductName = row.Field<string>("ProductName")
                    }).ToList();

                    Session[ProductByProductSessionKey_Loan] = List_Product;
                    productList = List_Product;
                }
                
                

                var viewCenter = productList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.ProductID.ToString(),
                    Text = x.ProductName.ToString()
                });
                var product_items = new List<SelectListItem>();
                product_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
                product_items.AddRange(viewCenter);

                return Json(product_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion =================================================================



        [HttpPost]
        public ActionResult SaveLoanSummaryUpdate(Dictionary<string, string> allTrx, List<string> allLoanTrxId,
            string center,
            string member,
            string product,
            string loanterm
            )
        {
            try
            {
                var trx = allTrx;

                var trxId = 1;
                var loanTrxIds = allLoanTrxId.Where(w => int.TryParse(w, out trxId));

                var loanTrxRegisterCollection = new List<LoanTrx>();
                LoanUpdateViewModel obj = new LoanUpdateViewModel();
                foreach (var id in trx)  //foreach (var id in loanTrxIds)
                {
                    string[] v = id.Key.Split(',');
                    var LoanSummaryID = "";
                    var FieldName = "";
                    if (v.Length > 1)
                    {
                        LoanSummaryID = v[1];
                        FieldName = v[0];
                    }
                    else
                    {
                        LoanSummaryID = "0";
                        FieldName = v[0];
                    }

                    var value = id.Value;
                    obj.LoanSummaryID = Convert.ToInt64(LoanSummaryID);

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
                    if (FieldName == "txtSelectedPurposeCode")
                    {
                        obj.PurposeID = Convert.ToInt16(value);
                    }
                    if (FieldName == "LoanTerm")
                    {
                        obj.LoanTerm = Convert.ToByte(value);
                    }

                    if (FieldName == "PrincipalLoan")
                    {
                        obj.PrincipalLoan = Convert.ToDecimal(value);
                    }
                    if (FieldName == "ApproveDate")
                    {
                        obj.ApproveDate = Convert.ToString(value);
                    }
                    if (FieldName == "DisburseDate")
                    {
                        obj.DisburseDate = Convert.ToString(value);
                    }
                    if (FieldName == "Duration")
                    {
                        obj.Duration = Convert.ToInt32(value);
                    }
                    if (FieldName == "LoanRepaid")
                    {
                        obj.LoanRepaid = Convert.ToDecimal(value);
                    }
                    if (FieldName == "IntCharge")
                    {
                        obj.IntCharge = Convert.ToDecimal(value);
                    }
                    if (FieldName == "IntPaid")
                    {
                        obj.IntPaid = Convert.ToDecimal(value);
                    }
                    if (FieldName == "LoanInstallment")
                    {
                        obj.LoanInstallment = Convert.ToDecimal(value);
                    }
                    if (FieldName == "IntInstallment")
                    {
                        obj.IntInstallment = Convert.ToDecimal(value);
                    }
                    if (FieldName == "InterestRate")
                    {
                        obj.InterestRate = Convert.ToDecimal(value);
                    }
                    if (FieldName == "InstallmentNo")
                    {
                        obj.InstallmentNo = Convert.ToInt32(value);
                    }
                    if (FieldName == "InstallmentDate")
                    {
                        obj.InstallmentDate = Convert.ToString(value);
                    }
                    if (FieldName == "LoanStatus")
                    {
                        obj.LoanStatus = Convert.ToByte(value);
                    }
                    if (FieldName == "PartialAmount")
                    {
                        obj.PartialAmount = Convert.ToDecimal(value);
                    }
                    if (FieldName == "PartialIntCharge")
                    {
                        obj.PartialIntCharge = Convert.ToDecimal(value);
                    }
                    if (FieldName == "PartialIntPaid") // Last Column Data
                    {
                        obj.PartialIntPaid = Convert.ToDecimal(value);

                        /// insert into Table
                        var param = new
                        {
                            LoanSummaryID = obj.LoanSummaryID,
                            OfficeID = SessionHelper.LoginUserOfficeID,
                            MemberID = obj.MemberID,
                            ProductID = obj.ProductID,
                            CenterID = obj.CenterID,
                            MemberCategoryID = obj.MemberCategoryID,
                            PurposeID = obj.PurposeID,
                            LoanTerm = obj.LoanTerm,
                            PrincipalLoan = obj.PrincipalLoan,
                            ApproveDate = obj.ApproveDate == "null" ? "" : obj.ApproveDate,
                            DisburseDate = obj.DisburseDate == "null" ? "" : obj.DisburseDate,

                            Duration = obj.Duration,
                            LoanRepaid = obj.LoanRepaid,
                            IntCharge = obj.IntCharge,
                            IntPaid = obj.IntPaid,
                            LoanInstallment = obj.LoanInstallment,
                            IntInstallment = obj.IntInstallment,
                            InterestRate = obj.InterestRate,
                            InstallmentNo = obj.InstallmentNo,
                            InstallmentDate = obj.InstallmentDate == "null" ? "" : obj.InstallmentDate,
                            LoanStatus = obj.LoanStatus,
                            PartialAmount = obj.PartialAmount,
                            PartialIntCharge = obj.PartialIntCharge,
                            PartialIntPaid = obj.PartialIntPaid,
                            CreateUser = SessionHelper.LoginUserEmployeeID,
                            CreateDate = DateTime.Now.Date.ToString("dd-MMM-yyyy")

                        };

                        var val = ultimateReportService.GetDataWithParameter(param, "Proc_InsertLoanSummaryUpdate");

                        obj = new LoanUpdateViewModel(); 

                    }

                }

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }// END



        //===================================================================================//
        //================================= Loan Trx Update ===============================//
        //===================================================================================//

        public ActionResult LoanTrxUpdate()
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


        // Load Data Into Grid

        public JsonResult GETLoanTrxList(int jtStartIndex, int jtPageSize, string jtSorting, int CenterID = 0, long MemberID = 0, int ProductID = 0, int LoanTerm = 0, string Option = "")
        {
            try
            {
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = CenterID, @MemberID = MemberID, @ProductID = ProductID, @LoanTerm = LoanTerm };
                var spData = ultimateReportService.GetDataWithParameter(param, "Proc_GetLoanTrxListForUpdate");
                var detail = spData.Tables[0].AsEnumerable()
                    .Select(x => new LoanUpdateViewModel
                    {
                        LoanTrxID           = x.Field<Int64>("LoanTrxID"),
                        TrxDate             = x.Field<string>("TrxDate"),
                        LoanSummaryID       = x.Field<Int64>("LoanSummaryID"),
                        OfficeID            = x.Field<int>("OfficeID"),
                        OfficeCode          = x.Field<string>("OfficeCode"),
                        OfficeName          = x.Field<string>("OfficeName"),
                        MemberID            = x.Field<Int64>("MemberID"),
                        MemberCode          = x.Field<string>("MemberCode"),
                        MemberName          = x.Field<string>("MemberName"),
                        ProductID           = x.Field<short>("ProductID"),
                        ProductCode         = x.Field<string>("ProductCode"),
                        ProductName         = x.Field<string>("ProductName"),
                        CenterID            = x.Field<int>("CenterID"),
                        CenterCode          = x.Field<string>("CenterCode"),
                        CenterName          = x.Field<string>("CenterName"),
                        MemberCategoryID    = x.Field<byte>("MemberCategoryID"),
                        MemberCategoryCode  = x.Field<string>("MemberCategoryCode"),
                        CategoryName        = x.Field<string>("CategoryName"),
                        LoanTerm            = x.Field<int>("LoanTerm"),
                        InstallmentDate     = x.Field<string>("InstallmentDate"),
                        PrincipalLoan       = x.Field<decimal>("PrincipalLoan"),
                        LoanDue             = x.Field<decimal>("LoanDue"),
                        LoanPaid            = x.Field<decimal>("LoanPaid"),
                        IntCharge           = x.Field<decimal>("IntCharge"),
                        IntDue              = x.Field<decimal>("IntDue"),
                        IntPaid             = x.Field<decimal>("IntPaid"),
                        Advance             = x.Field<decimal>("Advance"),
                        DueRecovery         = x.Field<decimal>("DueRecovery"),
                        TrxType             = x.Field<byte>("TrxType"),
                        InstallmentNo       = x.Field<short>("InstallmentNo"),
                        EmployeeID          = x.Field<short>("EmployeeID")
                    }).ToList();

                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);

                List<LoanUpdateViewModel> viewList = new List<LoanUpdateViewModel>();
                int rowSl = 0;
                foreach (var item in detail.OrderBy(x => x.DisburseDate))
                {
                    var lists = new LoanUpdateViewModel()
                    {
                        LoanTrxID = item.LoanTrxID,
                        TrxDate = item.TrxDate,
                        LoanSummaryID = item.LoanSummaryID,
                        OfficeID = item.OfficeID,
                        OfficeCode = item.OfficeCode,
                        OfficeName = item.OfficeName,
                        MemberID = item.MemberID,
                        MemberCode = item.MemberCode,
                        MemberName = item.MemberName,
                        ProductID = item.ProductID,
                        ProductCode = item.ProductCode,
                        ProductName = item.ProductName,
                        CenterID = item.CenterID,
                        CenterCode = item.CenterCode,
                        CenterName = item.CenterName,
                        MemberCategoryID = item.MemberCategoryID,
                        MemberCategoryCode = item.MemberCategoryCode,
                        CategoryName = item.CategoryName,
                        LoanTerm = item.LoanTerm,
                        InstallmentDate = item.InstallmentDate,
                        PrincipalLoan = item.PrincipalLoan,
                        LoanDue = item.LoanDue,
                        LoanPaid = item.LoanPaid,
                        IntCharge = item.IntCharge,
                        IntDue = item.IntDue,
                        IntPaid = item.IntPaid,
                        Advance = item.Advance,
                        DueRecovery = item.DueRecovery,
                        TrxType = item.TrxType,
                        InstallmentNo = item.InstallmentNo,
                        EmployeeID = item.EmployeeID
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



        //=========== Save LoanTrxLog and Update LoanTrx 

        [HttpPost]
        public ActionResult SaveLoanTrxUpdate(Dictionary<string, string> allTrx, List<string> allLoanTrxId,
            string center,
            string member,
            string product,
            string loanterm
            )
        {
            try
            {
                var trx = allTrx;

                var trxId = 1;
                var loanTrxIds = allLoanTrxId.Where(w => int.TryParse(w, out trxId));

                var loanTrxRegisterCollection = new List<LoanTrx>();
                LoanUpdateViewModel obj = new LoanUpdateViewModel();
                foreach (var id in trx)  //foreach (var id in loanTrxIds)
                {
                    string[] v = id.Key.Split(',');
                    var LoanTrxID = "";
                    var FieldName = "";
                    if (v.Length > 1)
                    {
                        LoanTrxID = v[1];
                        FieldName = v[0];
                    }
                    else
                    {
                        LoanTrxID = "0";
                        FieldName = v[0];
                    }

                    var value = id.Value;
                    obj.LoanTrxID = Convert.ToInt64(LoanTrxID);

                    if (FieldName == "TrxDate")
                    {
                        obj.TrxDate = Convert.ToString(value);
                    }

                    if (FieldName == "LoanSummaryID")
                    {
                        obj.LoanSummaryID = Convert.ToInt64(value);
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
                    if (FieldName == "LoanTerm")
                    {
                        obj.LoanTerm = Convert.ToByte(value);
                    }
                    if (FieldName == "InstallmentDate")
                    {
                        obj.InstallmentDate = Convert.ToString(value);
                    }
                    if (FieldName == "PrincipalLoan")
                    {
                        obj.PrincipalLoan = Convert.ToDecimal(value);
                    }
                    if (FieldName == "LoanDue")
                    {
                        obj.LoanDue = Convert.ToDecimal(value);
                    }
                    if (FieldName == "LoanPaid")
                    {
                        obj.LoanPaid = Convert.ToDecimal(value);
                    }
                    if (FieldName == "IntCharge")
                    {
                        obj.IntCharge = Convert.ToDecimal(value);
                    }
                    if (FieldName == "IntDue")
                    {
                        obj.IntDue = Convert.ToDecimal(value);
                    }
                    if (FieldName == "IntPaid")
                    {
                        obj.IntPaid = Convert.ToDecimal(value);
                    }
                    if (FieldName == "Advance")
                    {
                        obj.Advance = Convert.ToDecimal(value);
                    }
                    if (FieldName == "DueRecovery")
                    {
                        obj.DueRecovery = Convert.ToDecimal(value);
                    }
                    
                    if (FieldName == "TrxType")
                    {
                        obj.TrxType = Convert.ToByte(value);
                    }
                    if (FieldName == "InstallmentNo")
                    {
                        obj.InstallmentNo = Convert.ToInt32(value);


                        /// insert into Table
                        var param = new
                        {
                            LoanTrxID = obj.LoanTrxID,
                            TrxDate = obj.TrxDate == "null" ? "" : obj.TrxDate,
                            LoanSummaryID = obj.LoanSummaryID,
                            OfficeID = SessionHelper.LoginUserOfficeID,
                            MemberID = obj.MemberID,
                            ProductID = obj.ProductID,
                            CenterID = obj.CenterID,
                            MemberCategoryID = obj.MemberCategoryID,
                            LoanTerm = obj.LoanTerm,
                            InstallmentDate = obj.InstallmentDate == "null" ? "" : obj.InstallmentDate,
                            PrincipalLoan = obj.PrincipalLoan,
                            LoanDue = obj.LoanDue,
                            LoanPaid = obj.LoanPaid,
                            IntCharge = obj.IntCharge,
                            IntDue = obj.IntDue,
                            IntPaid = obj.IntPaid,
                            Advance = obj.Advance,
                            DueRecovery = obj.DueRecovery,
                            TrxType = obj.TrxType,
                            InstallmentNo = obj.InstallmentNo,
                            CreateUser = SessionHelper.LoginUserEmployeeID,
                            CreateDate = DateTime.Now.Date.ToString("dd-MMM-yyyy")

                        };

                        var val = ultimateReportService.GetDataWithParameter(param, "Proc_InsertLoanTrxUpdate");

                        obj = new LoanUpdateViewModel(); // After Save and Update Create New Object

                    }
                }


                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }// END
    }
}
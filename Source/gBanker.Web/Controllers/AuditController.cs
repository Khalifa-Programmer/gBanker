using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
namespace gBanker.Web.Controllers
{
    public class AuditController : BaseController
    {
        private readonly IEmployeeService employeeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IGroupwiseReportService groupwiseReportService;
        public AuditController(IEmployeeService employeeService, IUltimateReportService ultimateReportService, IGroupwiseReportService groupwiseReportService)
        {
            this.employeeService = employeeService;
            this.ultimateReportService = ultimateReportService;
            this.groupwiseReportService = groupwiseReportService;
        }
        public JsonResult GetEmpList()
        {
            try
            {
                List<AccCorrectionViewModel> List_Members = new List<AccCorrectionViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Qtype = 2 };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetOfficeAndUser");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccCorrectionViewModel
                {
                    CreateUser = row.Field<string>("CreateUser"),
                    EmpName =  row.Field<string>("EmpName")
 
                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOffList()
        {
            try
            {
                List<AccCorrectionViewModel> List_Members = new List<AccCorrectionViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Qtype = 1 };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetOfficeAndUser");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccCorrectionViewModel
                {
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName") 

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEmpListLOanSummary()
        {
            try
            {
                List<AccCorrectionViewModel> List_Members = new List<AccCorrectionViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Qtype = 4 };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetOfficeAndUser");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccCorrectionViewModel
                {
                    CreateUser = row.Field<string>("CreateUser"),
                    EmpName = row.Field<string>("EmpName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEmpListSavingInstallmentCorrection()
        {
            try
            {
                List<AccCorrectionViewModel> List_Members = new List<AccCorrectionViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Qtype = 9 };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetOfficeAndUser");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccCorrectionViewModel
                {
                    CreateUser = row.Field<string>("CreateUser"),
                    EmpName = row.Field<string>("EmpName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEmpListLOanInstallment()
        {
            try
            {
                List<AccCorrectionViewModel> List_Members = new List<AccCorrectionViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Qtype = 7 };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetOfficeAndUser");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccCorrectionViewModel
                {
                    CreateUser = row.Field<string>("CreateUser"),
                    EmpName = row.Field<string>("EmpName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOffListLOanSummary()
        {
            try
            {
                List<AccCorrectionViewModel> List_Members = new List<AccCorrectionViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Qtype = 3 };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetOfficeAndUser");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccCorrectionViewModel
                {
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOffListSavingInstallmentCorrection()
        {
            try
            {
                List<AccCorrectionViewModel> List_Members = new List<AccCorrectionViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Qtype = 10 };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetOfficeAndUser");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccCorrectionViewModel
                {
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOffListLOanInstallment()
        {
            try
            {
                List<AccCorrectionViewModel> List_Members = new List<AccCorrectionViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Qtype = 8 };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetOfficeAndUser");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccCorrectionViewModel
                {
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEmpListMember()
        {
            try
            {
                List<AccCorrectionViewModel> List_Members = new List<AccCorrectionViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Qtype = 5 };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetOfficeAndUser");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccCorrectionViewModel
                {
                    CreateUser = row.Field<string>("CreateUser"),
                    EmpName = row.Field<string>("EmpName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetOffListMember()
        {
            try
            {
                List<AccCorrectionViewModel> List_Members = new List<AccCorrectionViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Qtype = 6 };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetOfficeAndUser");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccCorrectionViewModel
                {
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDisbursementLogRecords(int jtStartIndex, int jtPageSize, string jtSorting, string dateFrom, string dateTo, string Office, string user)
        {

            try
            {
                if (Office == "0")
                {

                    Office = "";
                }
                if (user == "0")
                {

                    user = "";
                }
                List<LoanSummaryCorrectionViewModel> List_ProductViewModel = new List<LoanSummaryCorrectionViewModel>();
                var param = new object();
                if (Office == "" && user == "")
                {
                    param = new { Qtype = 1, Office = SessionHelper.LoginUserOfficeID, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }

                else if (Office == "" && user != "")
                {
                    param = new { Qtype = 2, Office = SessionHelper.LoginUserOfficeID, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                else if (Office != "" && user != "")
                {
                    param = new { Qtype = 3, Office = Office, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                else if (Office != "" && user == "")
                {
                    param = new { Qtype = 4, Office = Office, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                var model = ultimateReportService.GetDisbursementCorrectionLog(param);

                List_ProductViewModel = model.Tables[0].AsEnumerable()
                .Select(row => new LoanSummaryCorrectionViewModel
                {
                    OldMemberCode = row.Field<string>("OldMemberCode"),
                    MemberCode = row.Field<string>("MemberCode"),
                    OldName = row.Field<string>("OldName"),
                    MemberName = row.Field<string>("MemberName"),
                    OldCenterCode = row.Field<string>("OldCenterCode"),
                    CenterCode = row.Field<string>("CenterCode"),
                    OldCenterName = row.Field<string>("OldCenterName"),
                    CenterName = row.Field<string>("CenterName"),
                    OldProduct = row.Field<string>("OldProduct"),
                    OLdProdName = row.Field<string>("OLdProdName"),
                    Product = row.Field<string>("Product"),
                    ProdName = row.Field<string>("ProdName"),

                    OldPrin = row.Field<decimal>("OldPrin"),
                    UpdatedDIsbursedLoan = row.Field<decimal>("UpdatedDIsbursedLoan"),
                    OldDisDate = row.Field<DateTime>("OldDisDate"),
                    CorrectionDate = row.Field<DateTime>("CorrectionDate"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmpName = row.Field<string>("EmpName"),
                    OfficeCode= row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName")

                }).ToList();



                var detail = List_ProductViewModel.ToList();
                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult GetLoanInstallmentLogRecords(int jtStartIndex, int jtPageSize, string jtSorting, string dateFrom, string dateTo, string Office, string user)
        {

            try
            {
                if (Office == "0")
                {

                    Office = "";
                }
                if (user == "0")
                {

                    user = "";
                }
                List<LoanInsCorrectionViewModel> List_ProductViewModel = new List<LoanInsCorrectionViewModel>();
                var param = new object();
                if (Office == "" && user == "")
                {
                    param = new { Qtype = 1, Office = SessionHelper.LoginUserOfficeID, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }

                else if (Office == "" && user != "")
                {
                    param = new { Qtype = 2, Office = SessionHelper.LoginUserOfficeID, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                else if (Office != "" && user != "")
                {
                    param = new { Qtype = 3, Office = Office, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                else if (Office != "" && user == "")
                {
                    param = new { Qtype = 4, Office = Office, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                var model = ultimateReportService.GetLoanInstallmentmentCorrectionLog(param);

                List_ProductViewModel = model.Tables[0].AsEnumerable()
                .Select(row => new LoanInsCorrectionViewModel
                {
                    OldMemCode = row.Field<string>("OldMemCode"),
                    NewMemberCode = row.Field<string>("NewMemberCode"),
                    OldName = row.Field<string>("OldName"),
                    NewMemName = row.Field<string>("NewMemName"),
                   
                    CenterCode = row.Field<string>("CenterCode"),
                   
                    CenterName = row.Field<string>("CenterName"),
                    ProductCode = row.Field<string>("ProductCode"),
                    NewProductCode = row.Field<string>("NewProductCode"),
                    NewProdName = row.Field<string>("NewProdName"),
                    ProductFullNameEng = row.Field<string>("ProductFullNameEng"),
                    LoanPaid= row.Field<decimal>("LoanPaid"),
                    IntPaid = row.Field<decimal>("IntPaid"),
                    OldLoanPaid = row.Field<decimal>("OldLoanPaid"),
                    OldIntPaid = row.Field<decimal>("OldIntPaid"),

                    OldTrxDate = row.Field<DateTime>("OldTrxDate"),
                    CorrectionDate = row.Field<DateTime>("CorrectionDate"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmpName = row.Field<string>("EmpName"),
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName")

                }).ToList();



                var detail = List_ProductViewModel.ToList();
                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult GetLogRecordsMember(int jtStartIndex, int jtPageSize, string jtSorting, string dateFrom, string dateTo, string Office, string user)
        {

            try
            {
                if (Office == "0")
                {

                    Office = "";
                }
                if (user == "0")
                {

                    user = "";
                }
                List<MemberCorrectionViewModel> List_ProductViewModel = new List<MemberCorrectionViewModel>();
                var param = new object();
                if (Office == "" && user == "")
                {
                    param = new { Qtype = 1, Office = SessionHelper.LoginUserOfficeID, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }

                else if (Office == "" && user != "")
                {
                    param = new { Qtype = 2, Office = SessionHelper.LoginUserOfficeID, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                else if (Office != "" && user != "")
                {
                    param = new { Qtype = 3, Office = Office, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                else if (Office != "" && user == "")
                {
                    param = new { Qtype = 4, Office = Office, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                var model = ultimateReportService.GetMemberCorrectionLog(param);

                List_ProductViewModel = model.Tables[0].AsEnumerable()
                .Select(row => new MemberCorrectionViewModel
                {
                    OfficeCode= row.Field<string>("OfficeCode"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmpName = row.Field<string>("EmpName"),
                    CreateDate = row.Field<DateTime>("CreateDate"),
                   CenterCode = row.Field<string>("CenterCode"),
                    CenterName = row.Field<string>("CenterName"),
                    NewCenterCode = row.Field<string>("NewCenterCode"),
                    NewCenterName = row.Field<string>("NewCenterName"),
                    MemberCode= row.Field<string>("MemberCode"),
                    MemName = row.Field<string>("MemName"),
                    NewMemName = row.Field<string>("NewMemName"),
                    Address = row.Field<string>("Address"),
                    NewAddress = row.Field<string>("NewAddress"),
                    MemberCategoryCode = row.Field<string>("MemberCategoryCode"),
                    NewMemberCategory = row.Field<string>("NewMemberCategory"),
                    Gender= row.Field<string>("Gender"),
                    NewGender = row.Field<string>("NewGender"),
                    RefereeName = row.Field<string>("RefereeName"),
                    NewRef = row.Field<string>("NewRef"),
                    NationalID = row.Field<string>("NationalID"),
                    NewNationalID = row.Field<string>("NewNationalID"),
                    BirthDate = row.Field<DateTime>("BirthDate"),
                    NewBirthDate = row.Field<DateTime>("NewBirthDate"),
                    JoinDate = row.Field<DateTime>("JoinDate"),
                    NewJoinDate = row.Field<DateTime>("JoinDate"),
                    PhoneNo = row.Field<string>("PhoneNo"),
                    NewPhoneNo = row.Field<string>("NewPhoneNo")

                }).ToList();



                var detail = List_ProductViewModel.ToList();
                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult GetLogRecords(int jtStartIndex, int jtPageSize, string jtSorting, string dateFrom, string dateTo, string Office, string user)
        {

            try
            {
                if (Office=="0")
                {

                    Office = "";
                }
                if (user == "0")
                {

                    user = "";
                }
                List<AccCorrectionViewModel> List_ProductViewModel = new List<AccCorrectionViewModel>();
                var param = new object();
                if (Office == "" && user == "")
                {
                    param = new { Qtype = 1, Office = SessionHelper.LoginUserOfficeID, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo) , User = user };
                }
               
                else if (Office == "" && user != "")
                {
                    param = new { Qtype = 2, Office = SessionHelper.LoginUserOfficeID, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                else if (Office != "" && user != "")
                {
                    param = new { Qtype = 3, Office = Office, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                else if (Office != "" && user == "")
                {
                    param = new { Qtype = 4, Office = Office, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                var model = ultimateReportService.GetAccountCorrectionLog(param);



                List_ProductViewModel = model.Tables[0].AsEnumerable()
                .Select(row => new AccCorrectionViewModel
                {
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmpName = row.Field<string>("EmpName"),
                    CreateDate = row.Field<DateTime>("CreateDate"),
                    AccCode = row.Field<string>("AccCode"),
                    AccName = row.Field<string>("AccName"),
                    Debit = row.Field<decimal>("Debit"),
                    Credit = row.Field<decimal>("Credit"),
                    CurrentAccCode = row.Field<string>("CurrentAccCode"),
                    CurrentAccName = row.Field<string>("CurrentAccName"),
                    CurrentDebit = row.Field<decimal>("CurrentDebit"),
                    CurrentCredit = row.Field<decimal>("CurrentCredit"),
                    OfficeCode = row.Field<string>("OfficeCode"),
                   

                }).ToList();



                var detail = List_ProductViewModel.ToList();
                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult GetSavingInstallmentCorrectionRecords(int jtStartIndex, int jtPageSize, string jtSorting, string dateFrom, string dateTo, string Office, string user)
        {

            try
            {
                if (Office == "0")
                {

                    Office = "";
                }
                if (user == "0")
                {

                    user = "";
                }
                List<SavingInstallmentCorrectionViewModel> List_ProductViewModel = new List<SavingInstallmentCorrectionViewModel>();
                var param = new object();
                if (Office == "" && user == "")
                {
                    param = new { Qtype = 1, Office = SessionHelper.LoginUserOfficeID, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }

                else if (Office == "" && user != "")
                {
                    param = new { Qtype = 2, Office = SessionHelper.LoginUserOfficeID, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                else if (Office != "" && user != "")
                {
                    param = new { Qtype = 3, Office = Office, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                else if (Office != "" && user == "")
                {
                    param = new { Qtype = 4, Office = Office, DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), User = user };
                }
                var model = ultimateReportService.GetSavingInstallmentCorrectionLog(param);

                List_ProductViewModel = model.Tables[0].AsEnumerable()
                .Select(row => new SavingInstallmentCorrectionViewModel
                {
                    MemberCodeTo = row.Field<string>("MemberCodeTo"),
                    MemberCode = row.Field<string>("MemberCode"),
                    MemberNameTo = row.Field<string>("MemberNameTo"),
                    MemberName = row.Field<string>("MemberName"),

                    CenterCode = row.Field<string>("CenterCode"),

                    CenterName = row.Field<string>("CenterName"),
                    ProductCodeTo = row.Field<string>("ProductCodeTo"),
                    ProductCode = row.Field<string>("ProductCode"),
                    ProductNameTo = row.Field<string>("ProductNameTo"),
                    ProductName = row.Field<string>("ProductName"),
                    SavingInstallment = row.Field<decimal>("SavingInstallment"),
                    Withdrawal = row.Field<decimal>("Withdrawal"),
                    DepositTrans = row.Field<decimal>("DepositTrans"),
                    WithdrawalTrans = row.Field<decimal>("WithdrawalTrans"),

                    TransactionDate = row.Field<DateTime>("TransactionDate"),
                    TrxDate = row.Field<DateTime>("TrxDate"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmpName = row.Field<string>("EmpName"),
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OfficeName = row.Field<string>("OfficeName")

                }).ToList();



                var detail = List_ProductViewModel.ToList();
                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        // GET: Audit
        public ActionResult Index()
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmpList"] = items;

           // IEnumerable<SelectListItem> items1 = new SelectList(" ");
            ViewData["OffList"] = items;


            return View();
        }
        public ActionResult LoanSummaryCorrectionIndex()
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmpList"] = items;

            // IEnumerable<SelectListItem> items1 = new SelectList(" ");
            ViewData["OffList"] = items;


            return View();
        }
        public ActionResult LoanInstallmentCorrectionIndex()
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmpList"] = items;

            // IEnumerable<SelectListItem> items1 = new SelectList(" ");
            ViewData["OffList"] = items;


            return View();
        }
        public ActionResult SavingInstallmentCorrectionIndex()
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmpList"] = items;

            // IEnumerable<SelectListItem> items1 = new SelectList(" ");
            ViewData["OffList"] = items;


            return View();
        }
        public ActionResult MemberCorrectionIndex()
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmpList"] = items;

            // IEnumerable<SelectListItem> items1 = new SelectList(" ");
            ViewData["OffList"] = items;


            return View();
        }
        // GET: Audit/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Audit/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Audit/Create
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
        // GET: Audit/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: Audit/Edit/5
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
        // GET: Audit/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: Audit/Delete/5
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

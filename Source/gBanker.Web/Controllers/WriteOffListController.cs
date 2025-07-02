using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
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

    public class WriteOffListController : BaseController
    {
        private readonly ILoanTrxService loantrxService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ILoanSummaryService loanSummaryService;


        
        public WriteOffListController(ILoanTrxService loantrxService, IMemberService memberService, IOfficeService officeService, IUltimateReportService ultimateReportService, IGroupwiseReportService groupwiseReportService, ILoanSummaryService loanSummaryService)
        {
            this.loantrxService = loantrxService;
            this.memberService = memberService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.groupwiseReportService = groupwiseReportService;
            this.loanSummaryService = loanSummaryService;

        }
        private void MapDropDownList(getWriteOffListViewModel model)
         {
             if (!SessionHelper.LoginUserOfficeID.HasValue)
             {
                 RedirectToAction("Login", "Account");
                 return;
             }
             var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));

             var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + '-' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName))), Value = m.MemberID.ToString() });

             model.memberListItems = viewMember;
             ViewData["Member"] = viewMember;



             var alloffice = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value && l.OrgID==LoggedInOrganizationID);

             var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

             model.officeListItems = viewOffice;

        
         }
        public ActionResult GetMemberList(string memberid, string oficeId)
         {
             //var MemberByOfficeSessionKey = string.Format("MemberByOfficeSessionKey_{0}", oficeId);
             var memberList = new List<Member>();
             //if (Session[MemberByOfficeSessionKey] != null)
             //    memberList = Session[MemberByOfficeSessionKey] as List<Member>;
             //else
             //{

             var mbr = memberService.SearchMember(int.Parse(oficeId), Convert.ToInt32(LoggedInOrganizationID));
             // Session[MemberByOfficeSessionKey] = mbr;
             memberList = mbr.ToList();
             //}
             var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + " " + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + " " + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + " " + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + " " + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

             return Json(members, JsonRequestBehavior.AllowGet);
         }
        // GET: WriteOffList
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            


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
         
            var model = new getWriteOffListViewModel();
            MapDropDownList(model);
            return View(model);
        }
        public JsonResult GenerateWriteOffList(int jtStartIndex, int jtPageSize, string jtSorting, string DateFrom, long MemberId, int Writeoffyear)
        {
            try
            {
                //int writeoffyear = 0;


                var param1 = new { @OrgID = LoggedInOrganizationID, @Office = LoginUserOfficeID, @MemberID = MemberId, @Trandate = Convert.ToDateTime(DateFrom), @writeoffyear = Writeoffyear };
                //var LoanInstallMent = ultimateReportService.GetPreWriteOffHistory(param1);

                List<getWriteOffList_Result> List_AccTrxMasterViewModel = new List<getWriteOffList_Result>();
                //var param = new { OrgID = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, TrxDate = TransactionDate };
                var empList = ultimateReportService.GetPreWriteOffHistory(param1);

                List_AccTrxMasterViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new getWriteOffList_Result
                {
                    OfficeID = row.Field<int>("OfficeID"),
                    CenterCode = row.Field<string>("CenterCode"),
                    MemberCode = row.Field<string>("MemberCode"),
                    FirstName = row.Field<string>("FirstName"),
                    ProductCode = row.Field<string>("ProductCode"),
                    LoanTerm = row.Field<int>("LoanTerm"),
                    PrincipalLoan = row.Field<decimal>("PrincipalLoan"),
                    LoanPaid = row.Field<decimal>("LoanPaid"),
                    LoanBalance = row.Field<decimal>("LoanBalance"),
                    IntCharge = row.Field<decimal>("IntCharge"),
                    intPaid = row.Field<decimal>("intPaid"),
                    intBal = row.Field<decimal>("intBal"),
                    DisburseDatestg = row.Field<string>("DisburseDatestg"),
                    LoanSummaryID = row.Field<long>("LoanSummaryID"), //DisburseDate
                    isProcessed = row.Field<bool>("isProcessed")

                }).ToList();
                //IEnumerable<getWriteOffList_Result> allHistory;
                //if (LoanInstallMent != null)
                //{


                //}


                //var allHistory = loantrxService.GetwriteOfList(Convert.ToInt16(LoggedInOrganizationID), SessionHelper.LoginUserOfficeID.Value, MemberId, Convert.ToDateTime(DateFrom), Writeoffyear);
                //var detail = allHistory.ToList();
                //var totCount = detail.Count();

                //var detail = LoanInstallMent.ToString();
                //var totCount = detail.Count();
                var currentPageRecords = List_AccTrxMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_AccTrxMasterViewModel.LongCount() });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        //public JsonResult GenerateWriteOffList(int jtStartIndex, int jtPageSize, string jtSorting, string DateFrom, int MemberId,int writeoffyear)
        //{
        //    try
        //    {

        //        var allHistory = loantrxService.GetwriteOfList(Convert.ToInt16(LoggedInOrganizationID),SessionHelper.LoginUserOfficeID.Value, MemberId, Convert.ToDateTime(DateFrom), writeoffyear);
        //        var detail = allHistory.ToList();
        //        var totCount = detail.Count();
        //        var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
        //        return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }

        //}
        //public JsonResult GenerateWriteOffList(int jtStartIndex, int jtPageSize, string jtSorting, string DateFrom, long MemberId, int Writeoffyear)
        //{
        //    try
        //    {
        //        //int writeoffyear = 0;


        //        var param1 = new { @OrgID = LoggedInOrganizationID, @Office = LoginUserOfficeID, @MemberID = MemberId, @Trandate = Convert.ToDateTime(DateFrom), @writeoffyear = Writeoffyear };
        //        //var LoanInstallMent = ultimateReportService.GetPreWriteOffHistory(param1);

        //        List<getWriteOffList_Result> List_AccTrxMasterViewModel = new List<getWriteOffList_Result>();
        //        //var param = new { OrgID = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, TrxDate = TransactionDate };
        //        var empList = ultimateReportService.GetPreWriteOffHistory(param1);

        //        List_AccTrxMasterViewModel = empList.Tables[0].AsEnumerable()
        //        .Select(row => new getWriteOffList_Result
        //        {
        //            OfficeID = row.Field<int>("OfficeID"),
        //            CenterCode = row.Field<string>("CenterCode"),
        //            MemberCode = row.Field<string>("MemberCode"),
        //            FirstName = row.Field<string>("FirstName"),
        //            ProductCode = row.Field<string>("ProductCode"),
        //            LoanTerm = row.Field<int>("LoanTerm"),
        //            PrincipalLoan = row.Field<decimal>("PrincipalLoan"),
        //            LoanPaid = row.Field<decimal>("LoanPaid"),
        //            LoanBalance = row.Field<decimal>("LoanBalance"),
        //            IntCharge = row.Field<decimal>("IntCharge"),
        //            intPaid = row.Field<decimal>("intPaid"),
        //            intBal = row.Field<decimal>("intBal"),
        //            DisburseDatestg = row.Field<string>("DisburseDatestg"),
        //            LoanSummaryID = row.Field<long>("LoanSummaryID") //DisburseDate

        //        }).ToList();
        //        //IEnumerable<getWriteOffList_Result> allHistory;
        //        //if (LoanInstallMent != null)
        //        //{


        //        //}


        //        //var allHistory = loantrxService.GetwriteOfList(Convert.ToInt16(LoggedInOrganizationID), SessionHelper.LoginUserOfficeID.Value, MemberId, Convert.ToDateTime(DateFrom), Writeoffyear);
        //        //var detail = allHistory.ToList();
        //        //var totCount = detail.Count();

        //        //var detail = LoanInstallMent.ToString();
        //        //var totCount = detail.Count();
        //        var currentPageRecords = List_AccTrxMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);
        //        return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_AccTrxMasterViewModel.LongCount() });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }

        //}

        /*
        
        */

        public JsonResult AddTemp(
             string LoanSummaryID    
           , string OfficeID
           , string CenterCode
           , string MemberCode
           , string FirstName
           , string ProductCode
           , string LoanTerm
           , string PrincipalLoan
           , string LoanPaid
           , string LoanBalance
           , string IntCharge
           , string intPaid
           , string intBal
           , string DisburseDatestg
 
            )
        {
            try
            {
                var param = new {

                    LoanSummaryID    = LoanSummaryID    
                  , OfficeID         = OfficeID
                  , CenterCode       = CenterCode
                  , MemberCode       = MemberCode
                  , FirstName        = FirstName
                  , ProductCode      = ProductCode
                  , LoanTerm         = LoanTerm
                  , PrincipalLoan    = PrincipalLoan
                  , LoanPaid         = LoanPaid
                  , LoanBalance      = LoanBalance
                  , IntCharge        = IntCharge
                  , intPaid          = intPaid
                  , intBal           = intBal
                  , DisburseDatestg  = DisburseDatestg
 
                };
           var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "CreateWriteOffList");


                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }// END FUNCTION




        public JsonResult FinalApprove(
             string LoanSummaryID
           , string OfficeID
           , string CenterCode
           , string MemberCode
           , string FirstName
           , string ProductCode
           , string LoanTerm
           , string PrincipalLoan
           , string LoanPaid
           , string LoanBalance
           , string IntCharge
           , string intPaid
           , string intBal
           , string DisburseDatestg

            )
        {
            try
            {

                var LoanSummary = loanSummaryService.GetByIdLong(Convert.ToInt64(LoanSummaryID));
                var param = new
                {
                     @OrgID = SessionHelper.LoginUserOrganizationID 
                   , @Office = SessionHelper.LoginUserOfficeID 
                   , @MemberID = LoanSummary.MemberID
                   , @centerID  = LoanSummary.CenterID
                   , @productID = LoanSummary.ProductID
                   , @LoanTerm  = LoanTerm
                   , @Trandate = SessionHelper.TransactionDate 
                   , @writeOffLOan    = LoanBalance
                   , @writeOffInterest = intBal
 
                };
                
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "setWriteOffList");


                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }// END FUNCTION






        public JsonResult GenerateWriteOffListFinal(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {

                var param1 = new { @OrgID = LoggedInOrganizationID, @Office = LoginUserOfficeID };
                //var LoanInstallMent = ultimateReportService.GetPreWriteOffHistory(param1);

                List<getWriteOffList_Result> List_AccTrxMasterViewModel = new List<getWriteOffList_Result>();

                //  var empList = ultimateReportService.GetPreWriteOffHistory(param1);
                var empList = groupwiseReportService.GetDataUltimateReleaseReport(param1, "GetFinalList");
                List_AccTrxMasterViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new getWriteOffList_Result
                {
                    OfficeID = row.Field<int>("OfficeID"),
                    CenterCode = row.Field<string>("CenterCode"),
                    MemberCode = row.Field<string>("MemberCode"),
                    FirstName = row.Field<string>("FirstName"),
                    ProductCode = row.Field<string>("ProductCode"),
                    LoanTerm = row.Field<int>("LoanTerm"),
                    PrincipalLoan = row.Field<decimal>("PrincipalLoan"),
                    LoanPaid = row.Field<decimal>("LoanPaid"),
                    LoanBalance = row.Field<decimal>("LoanBalance"),
                    IntCharge = row.Field<decimal>("IntCharge"),
                    intPaid = row.Field<decimal>("intPaid"),
                    intBal = row.Field<decimal>("intBal"),
                    DisburseDatestg = row.Field<string>("DisburseDatestg"),
                    LoanSummaryID = row.Field<long>("LoanSummaryID"), //DisburseDate,
                    isProcessed = row.Field<bool>("isProcessed"),

                }).ToList();
                //IEnumerable<getWriteOffList_Result> allHistory;
                //if (LoanInstallMent != null)
                //{


                //}


                //var allHistory = loantrxService.GetwriteOfList(Convert.ToInt16(LoggedInOrganizationID), SessionHelper.LoginUserOfficeID.Value, MemberId, Convert.ToDateTime(DateFrom), Writeoffyear);
                //var detail = allHistory.ToList();
                //var totCount = detail.Count();

                //var detail = LoanInstallMent.ToString();
                //var totCount = detail.Count();
                var currentPageRecords = List_AccTrxMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_AccTrxMasterViewModel.LongCount() });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        //public JsonResult GenerateWriteOffListFinal(int jtStartIndex, int jtPageSize, string jtSorting)
        //{
        //    try
        //    {

        //        var param1 = new { @OrgID = LoggedInOrganizationID, @Office = LoginUserOfficeID };
        //        //var LoanInstallMent = ultimateReportService.GetPreWriteOffHistory(param1);

        //        List<getWriteOffList_Result> List_AccTrxMasterViewModel = new List<getWriteOffList_Result>();

        //        //  var empList = ultimateReportService.GetPreWriteOffHistory(param1);
        //        var empList = groupwiseReportService.GetDataUltimateReleaseReport(param1, "GetFinalList");
        //        List_AccTrxMasterViewModel = empList.Tables[0].AsEnumerable()
        //        .Select(row => new getWriteOffList_Result
        //        {
        //            OfficeID = row.Field<int>("OfficeID"),
        //            CenterCode = row.Field<string>("CenterCode"),
        //            MemberCode = row.Field<string>("MemberCode"),
        //            FirstName = row.Field<string>("FirstName"),
        //            ProductCode = row.Field<string>("ProductCode"),
        //            LoanTerm = row.Field<int>("LoanTerm"),
        //            PrincipalLoan = row.Field<decimal>("PrincipalLoan"),
        //            LoanPaid = row.Field<decimal>("LoanPaid"),
        //            LoanBalance = row.Field<decimal>("LoanBalance"),
        //            IntCharge = row.Field<decimal>("IntCharge"),
        //            intPaid = row.Field<decimal>("intPaid"),
        //            intBal = row.Field<decimal>("intBal"),
        //            DisburseDatestg = row.Field<string>("DisburseDatestg"),
        //            LoanSummaryID = row.Field<long>("LoanSummaryID") //DisburseDate

        //        }).ToList();
        //        //IEnumerable<getWriteOffList_Result> allHistory;
        //        //if (LoanInstallMent != null)
        //        //{


        //        //}


        //        //var allHistory = loantrxService.GetwriteOfList(Convert.ToInt16(LoggedInOrganizationID), SessionHelper.LoginUserOfficeID.Value, MemberId, Convert.ToDateTime(DateFrom), Writeoffyear);
        //        //var detail = allHistory.ToList();
        //        //var totCount = detail.Count();

        //        //var detail = LoanInstallMent.ToString();
        //        //var totCount = detail.Count();
        //        var currentPageRecords = List_AccTrxMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);
        //        return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_AccTrxMasterViewModel.LongCount() });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }

        //}



        public ActionResult WriteOffListFinal()
        {
            return View();
        }





        // GET: WriteOffList/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: WriteOffList/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: WriteOffList/Create
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
        // GET: WriteOffList/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: WriteOffList/Edit/5
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
        // GET: WriteOffList/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: WriteOffList/Delete/5
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

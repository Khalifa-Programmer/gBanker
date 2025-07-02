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
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Service.StoredProcedure;

namespace gBanker.Web.Controllers
{
    public class SavingRegisterCollectionController : BaseController
    {
        private readonly ICenterService centerService;
        private readonly ISavingTrxService savingTrxService;
        private readonly ILoanCollectionService loanCollectionService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly ILoanCollectionReportService loanCollectionReportService;
        private readonly IEmployeeSPService ultimateReportService;
        private readonly ISavingSummaryService savingSummaryService;
        private readonly IGroupwiseReportService groupwiseReportService;

        public SavingRegisterCollectionController(ISavingTrxService savingTrxService, ICenterService centerService, IProductService productService, IOfficeService officeService, IMemberService memberService, ILoanCollectionService loanCollectionService, ILoanCollectionReportService loanCollectionReportService, IEmployeeSPService ultimateReportService, ISavingSummaryService savingSummaryService, IGroupwiseReportService groupwiseReportService)
        {
            this.savingTrxService = savingTrxService;
            this.centerService = centerService;
            this.productService = productService;
            this.officeService = officeService;
            this.memberService = memberService;
            this.loanCollectionService = loanCollectionService;
            this.loanCollectionReportService = loanCollectionReportService;
            this.ultimateReportService = ultimateReportService;
            this.savingSummaryService = savingSummaryService;
            this.groupwiseReportService = groupwiseReportService;

        }

        // GET: SavingRegisterCollection
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetMemberListAuto(string memberid, string centerId)
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

            return Json(members, JsonRequestBehavior.AllowGet);
        }

        // GET: LoanLedgerCorrection
        public ActionResult SavingRegisterCollection()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["LoggedInUser"] = LoggedInEmployee.EmpName;
            ViewData["LoggedInOfficeID"] = LoggedInEmployee.OfficeID;
            ViewData["TransactionDate"] = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;
            return View();

        }

        public JsonResult GETSavingRegisterList(int jtStartIndex, int jtPageSize, string jtSorting, int CenterID = 0, long MemberID = 0, int ProductID = 0, int NoOfAccount = 0, string Option = "")
        {
            try
            {
                var detail = savingTrxService.GetSavingTrxList(CenterID, MemberID, NoOfAccount, ProductID, Option).ToList();

                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<SavingTrx>, IEnumerable<SavingTrxViewModel>>(entities);
                ////////////////NNE

                List<SavingTrxViewModel> ViewDetail = new List<SavingTrxViewModel>();
                int rowSl = 0;
                foreach (var vd in currentPageRecords)
                {
                    var loans = new SavingTrxViewModel() { SavingTrxID = vd.SavingTrxID, rowSl = rowSl, TransactionDate = vd.TransactionDate, Deposit = vd.Deposit, Withdrawal = vd.Withdrawal, Penalty = vd.Penalty, Balance = vd.Balance, NoOfAccount = vd.NoOfAccount, MonthlyInterest = vd.MonthlyInterest, TransType = vd.TransType, CreateUser = vd.CreateUser, SavingSummaryID = vd.SavingSummaryID };
                    ViewDetail.Add(loans);
                    rowSl++;
                }

                return Json(new { Result = "OK", Records = ViewDetail, TotalRecordCount = totalCount });
                //return Json(new { Result = "OK",  TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult SaveSavingTrx(Dictionary<string, string> allTrx, List<string> allLoanTrxId,
           string center,
           string member,
           string product,
           string noOfAccount
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
                var savingTrxIds = allLoanTrxId.Where(w => int.TryParse(w, out trxId));

                var savingTrxRegisterCollection = new List<SavingTrx>();
                SavingTrxViewModel SavingTrxVM = new SavingTrxViewModel();
                //var SavingTrxId = "";
                foreach (var id in trx)  //foreach (var id in loanTrxIds)
                {
                    string[] v = id.Key.Split(',');
                    var SavingTrxId = "";
                    var FieldName = "";
                    if (v.Length > 1)
                    {
                        SavingTrxId = v[1];
                        FieldName = v[0];
                    }
                    else
                    {
                        SavingTrxId = "0";
                        FieldName = v[0];
                    }

                    var value = id.Value;
                    SavingTrxVM.SavingTrxID = Convert.ToInt64(SavingTrxId);

                    if (FieldName == "Deposit")// Last one
                    {
                        SavingTrxVM.Deposit = Convert.ToDecimal(value);
                    }
                    if (FieldName == "Withdrawal")// Last one
                    {
                        SavingTrxVM.Withdrawal = Convert.ToDecimal(value);
                    }
                    if (FieldName == "Penalty")// Last one
                    {
                        SavingTrxVM.Penalty = Convert.ToDecimal(value);
                    }
                    if (FieldName == "MonthlyInterest")// Last one
                    {
                        SavingTrxVM.MonthlyInterest = Convert.ToDecimal(value);
                    }
                    if (FieldName == "TransType")// Last one
                    {
                        SavingTrxVM.TransType = Convert.ToByte(value); //Convert.ToInt32(value);
                    }

                    if (FieldName == "CreateUser")// Last one
                    {
                        SavingTrxVM.CreateUser = value;


                        if (SavingTrxId == "0")// Create New
                        {   // Save Method. NEW
                            var entity = Mapper.Map<SavingTrxViewModel, SavingTrx>(SavingTrxVM);
                            entity.CenterID = Convert.ToInt32(center);
                            entity.MemberID = Convert.ToInt64(member);
                            entity.NoOfAccount = Convert.ToInt32(noOfAccount);
                            entity.ProductID = Convert.ToInt16(product);
                            entity.OrgID = (int)LoggedInOrganizationID;
                            entity.OfficeID = (int)SessionHelper.LoginUserOfficeID;

                            var SavingSummaryObj = savingSummaryService.GetSingleRow((int)SessionHelper.LoginUserOfficeID, entity.ProductID, entity.CenterID, entity.MemberID, entity.NoOfAccount); //.SingleOrDefault();

                            entity.MemberCategoryID = SavingSummaryObj.MemberCategoryID;
                            entity.SavingSummaryID = SavingSummaryObj.SavingSummaryID;
                            entity.Deposit = SavingTrxVM.Deposit;
                            entity.Withdrawal = SavingTrxVM.Withdrawal;
                            entity.Penalty = SavingTrxVM.Penalty;
                            entity.MonthlyInterest = SavingTrxVM.MonthlyInterest;
                            entity.TransType = SavingTrxVM.TransType;
                            entity.TransactionDate = SessionHelper.TransactionDate;  //DateTime.Now;
                            entity.CreateDate = DateTime.Now;
                            entity.CreateUser = LoggedInEmployee.EmpName; //SavingTrxVM.CreateUser;
                            entity.EmployeeID = 123;

                            if (entity.Deposit == 0 && entity.Withdrawal == 0 && entity.Penalty == 0)
                            {

                            }
                            else
                            {
                                savingTrxService.Create(entity);
                            }
                        }
                        else
                        {
                            //if (SavingTrxVM.CreateUser == "update")
                            //{
                            //disabled as Rais bhai Asked For// 
                            //var savingTrxUpdat = savingTrxService.GetById((int)SavingTrxVM.SavingTrxID);
                            //savingTrxUpdat.Deposit = SavingTrxVM.Deposit;
                            //savingTrxUpdat.Withdrawal = SavingTrxVM.Withdrawal;
                            //savingTrxUpdat.Penalty = SavingTrxVM.Penalty;
                            //savingTrxUpdat.MonthlyInterest = SavingTrxVM.MonthlyInterest;
                            //savingTrxUpdat.TransType = SavingTrxVM.TransType;
                            //savingTrxService.Update(savingTrxUpdat);
                            //disabled as Rais bhai Asked For//

                            /// insert into Table
                            var param = new
                            {
                                SavingTRXId = SavingTrxVM.SavingTrxID,
                                Deposit = SavingTrxVM.Deposit,
                                Withdrawal = SavingTrxVM.Withdrawal,
                                Penalty = SavingTrxVM.Penalty,
                                MonthlyInterest = SavingTrxVM.MonthlyInterest,
                                TransType = SavingTrxVM.TransType,
                                CreateUser = SessionHelper.LoginUserEmployeeID,
                                CreateDate = DateTime.Now
                            };
                            var val = ultimateReportService.InsertSAVINGRegisterUpdateINFO(param);
                        }
                        SavingTrxVM = new SavingTrxViewModel(); // After Save and Update Create New Object
                    }
                }

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }

        }// END

        public JsonResult GetMemberList(int centerId)
        {
            try
            {
                List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberList_ddl_SavingRegisterCorrection");

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


    } // End Class
}// END NameSpace
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class CollectionController : BaseController
    {
        #region variables
        
        private readonly ICenterService centerService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IGroupwiseReportService groupwiseReportService;

        public CollectionController(ICenterService centerService, IMemberService memberService, IOfficeService officeService,IUltimateReportService ultimateReportService,IGroupwiseReportService groupwiseReportService)
        {

            this.centerService = centerService;
            this.memberService = memberService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.groupwiseReportService = groupwiseReportService;
         
        }

        #endregion
        public ActionResult GetInstallment(string officeId, string centerId, string MemId)
        {
            decimal LoanPaid = 0;
            decimal vLoanDue = 0;
            decimal IntDue = 0;
            decimal IntPaid = 0;
          
           
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Convert.ToInt16(centerId), @MemberID = Convert.ToInt64(MemId) };
            var model = ultimateReportService.GetTodaysCollection(param);

            var lst = new List<CollectionViewModel>();
            //  var model = specialLoanCollectionService.GetAll().Where(l => l.OrgID == LoggedInOrganizationID && l.OfficeID == Convert.ToInt16(officeId) && l.CenterID == Convert.ToInt16(centerId) && l.MemberID == Convert.ToInt64(MemId) && l.ProductID == productid  && l.IsActive == true).FirstOrDefault();
            if (model.Tables[0].Rows.Count>0 )
            {
                // vlOanTerm = model.LoanTerm;

                foreach (DataRow dr in model.Tables[0].Rows)
                {
                    // ProductID, LoanSummaryID,LoanDue,LoanPaid,IntDue,IntPaid 
                    var obj = new CollectionViewModel() {
                        ProductId = int.Parse(dr["Productid"].ToString()),
                        TotalPaid = decimal.Parse(dr["TotalPaid"].ToString()),
                        LoanPaid = decimal.Parse(dr["LoanPaid"].ToString()),
                        LoanDue = decimal.Parse(dr["LoanDue"].ToString()),
                        IntPaid = decimal.Parse(dr["IntPaid"].ToString()),
                        IntDue = decimal.Parse(dr["IntDue"].ToString()),
                        //ProductID = short.Parse(dr["ProductID"].ToString()),
                        ProductCode = dr["ProductCode"].ToString(),
                        ProductName = dr["ProductName"].ToString(),
                        Balance = decimal.Parse(dr["Balance"].ToString()),
                        installmentNo = int.Parse(dr["installmentNo"].ToString()),
                        SummaryID = long.Parse(dr["LoanSummaryID"].ToString()),

                        Duration = int.Parse(dr["Duration"].ToString()),
                        DurationOverLoanDue = decimal.Parse(dr["DurationOverLoanDue"].ToString()),
                        DurationOverIntDue = decimal.Parse(dr["DurationOverIntDue"].ToString()),
                        CumInterestPaid = decimal.Parse(dr["CumInterestPaid"].ToString()),
                        CumIntCharge = decimal.Parse(dr["CumIntCharge"].ToString()),
                        CumLoanDue = decimal.Parse(dr["CumLoanDue"].ToString()),
                        CumIntDue = decimal.Parse(dr["CumIntDue"].ToString()),
                        DOC = decimal.Parse(dr["DOC"].ToString()),
                        OrgID = int.Parse(dr["OrgID"].ToString()),
                        principalLoan = decimal.Parse(dr["principalLoan"].ToString()),
                        loanRepaid = decimal.Parse(dr["loanRepaid"].ToString()),

                    };
                    lst.Add(obj);
                }

            }           
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        private void MapDropDownList(CollectionViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }

            var param1 = new { @EmpID = LoggedInEmployeeID };
            var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);


            IEnumerable<Center> allcenter;
            
            if (LoanInstallMent.Tables[0].Rows.Count > 0)
            {
                allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, LoggedInOrganizationID, Convert.ToInt16(LoggedInEmployeeID), LoanInstallMent.Tables[0].Rows[0]["Name"].ToString());
            }

            else
                allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, LoggedInOrganizationID, Convert.ToInt16(LoggedInEmployeeID), LoanInstallMent.Tables[0].Rows[0]["Name"].ToString());
            var viewCenList = allcenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = string.Format("{0} - {1}", x.CenterCode.ToString(), x.CenterName.ToString())
            });
            var cenitems = new List<SelectListItem>();
            cenitems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            cenitems.AddRange(viewCenList);
            model.CenterListItems = cenitems;           

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, m.FirstName + '-' + m.MiddleName + '-' + m.LastName), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var alloffice = officeService.GetAll().Where(l => l.OfficeID == LoginUserOfficeID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;
        }
        public JsonResult GetMemberList(int centerId)
        {
            try
            {
                List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberList_Dropdown_LoanSummary");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new GetMemberListViewModel
                {
                    MemberID = row.Field<string>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    MemberName = row.Field<string>("MemberName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult InsertData(
            string SummaryID        = "",
            string officeId         = "",
            string CenterID         = "",
            string MemberID         = "",
            string ProductCode      = "",
            string LoanPaid         = "",
            string Deposit          = "",
            string WithDrawal       = "",
            string ProductId        = "",
            string LoanDue          = "",
            string IntDue           = "",
            string Balance          = "",
            string InstallmentNo    = ""
            )
        {

            if (!IsDayInitiated)
            {
                return GetErrorMessageResult("Pls Run Start work Process");
            }
         string result = "OK";

        //int OrgID = Convert.ToInt16( LoggedInOrganizationID);

        // @OrgID = SessionHelper.LoginUserOrganizationID,
        //@OfficeID = SessionHelper.LoginUserOfficeID,

            var param = new { 
                
                SummaryID       = SummaryID                         ,
                OfficeID        = LoginUserOfficeID                 ,     //SessionHelper.LoginUserOfficeID
                CenterID        = CenterID                          ,
                MemberID        = MemberID                          ,
                ProductCode     = ProductCode                       ,
                LoanPaid        = LoanPaid                          ,
                Deposit         = Deposit                           ,
                WithDrawal      = WithDrawal                        ,
                CollectionDate  = TransactionDate                   ,
                ProductID       = ProductId                         ,
                CreateUser      = LoggedInEmployeeID                ,
                CreateDate      = DateTime.Now                      ,
                LoanDue         = LoanDue                           ,
                IntDue          = IntDue                            ,
                Balance         = Balance                           ,
                InstallmentNo   = InstallmentNo
             
            };
            var empList = groupwiseReportService.GetDataUltimateReleaseReport(param, "insertLoanSaving");

            
            return Json(result, JsonRequestBehavior.AllowGet);
        }//End Function

        //
        // GET: /Collection/
        public ActionResult Index()
        {
            if (!IsDayInitiated)
            {
                return GetErrorMessageResult("Pls. run start work process");
            }
            var model = new CollectionViewModel();
            //model.CenterListItems = new List<SelectListItem>() { new SelectListItem() { Text = "Select Center", Value = "0" } };

            MapDropDownList(model);

            List<CollectionViewModel> obj = new List<CollectionViewModel>();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["MemberList"] = items;

            return View(model);
        }

        //public JsonResult GetMemberList(int centerId)
        //{
        //    try
        //    {
        //        var getMember = memberService.GetAll().Where(s => s.CenterID == centerId && s.IsActive == true ).OrderBy(e => e.CenterID);
        //        var viewMember = getMember.Select(x => x).ToList().Select(x => new SelectListItem
        //        {
        //            Value = x.MemberID.ToString(),
        //            Text = x.MemberCode.ToString() + ", " + x.MiddleName.ToString()
        //            //Text = x.CenterName.ToString()
        //        });
        //        var member_items = new List<SelectListItem>();
        //        if (viewMember.ToList().Count > 0)
        //        {
        //            member_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
        //        }
        //        member_items.AddRange(viewMember);
        //        return Json(member_items, JsonRequestBehavior.AllowGet);
                
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //
        // GET: /Collection/Create


        public JsonResult GetList(string MemberCode, string centerId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (MemberCode != null)
                {
                    if (MemberCode != "")
                        sb.Append(" AND m.MemberCode ='" + MemberCode.Trim() + "'");
                }

                if (centerId != null)
                {
                    if (centerId != "")
                        sb.Append(" AND ls.CenterID ='" + centerId.Trim() + "'");
                }

                if (centerId == null)
                {
                    sb.Append(" AND ls.officeid ='" + LoginUserOfficeID + "' And ls.CenterID ='0'");
                }
                else
                    sb.Append(" AND ls.officeid ='" + LoginUserOfficeID + "'");
                

                List<CollectionViewModel> List_ViewModel = new List<CollectionViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = groupwiseReportService.GetDataUltimateReleaseReport(param, "SP_GetCollectionList");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new CollectionViewModel
                {
                    
                    Id = row.Field<long>("ID"),
                    ProductId = row.Field<int>("productId"),
                    SummaryID = row.Field<long>("SummaryID"),
                    OfficeID = row.Field<int>("OfficeID"),
                    CenterCode = row.Field<string>("CenterCode"),
                    MemberCode = row.Field<string>("MemberCode"),
                    MemberName = row.Field<string>("MemberName"),
                    ProductCode = row.Field<string>("ProductCode"),
                    LoanPaid = row.Field<decimal>("LoanPaid"),
                    Deposit = row.Field<decimal>("Deposit"),
                    WithDrawal = row.Field<decimal>("WithDrawal"),
                    Balance = row.Field<decimal>("Balance"),
                    installmentNo = row.Field<int>("installmentNo")

                }).ToList();

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End of Function



        public JsonResult GetByProductID(string ProductId)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                //string ProductIds = Convert.ToString(ProductId);

                if (ProductId != null)
                {
                    if (ProductId != "")
                    {
                        sb.Append(" AND cls.ID =" + ProductId);
                    }
                }
                List<CollectionViewModel> List_ViewModel = new List<CollectionViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = groupwiseReportService.GetDataUltimateReleaseReport(param, "SP_GetCollection");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new CollectionViewModel
                {
                    //ID	ProductID	LoanPaid	Deposit	WithDrawal
                    Id          = row.Field<long>("ID"),
                    OfficeID    = row.Field<int>("OfficeID"),
                    SummaryID   = row.Field<long>("SummaryID"),
                    MemberID    = row.Field<long>("MemberID"),
                    CenterID    = row.Field<int>("CenterID"),
                    ProductCode = row.Field<string>("ProductCode"),
                    ProductName = row.Field<string>("ProductName"),
                    ProductId   = row.Field<int>("productId"),
                    LoanPaid    = row.Field<decimal>("LoanPaid"),
                    Balance = row.Field<decimal>("Balance"),
                    installmentNo = row.Field<int>("installmentNo"),
                    Deposit     = row.Field<decimal>("Deposit"),
                    WithDrawal  = row.Field<decimal>("WithDrawal"),
                    TotalDue = row.Field<decimal>("TotalDue")

                }).ToList();

                
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function



        //GETSumBalance
        public JsonResult GetBalance(string centerId = null, string memberId=null )
        {
            try
            {
                List<CollectionViewModel> List_ViewModel = new List<CollectionViewModel>();

                StringBuilder sb = new StringBuilder();
                 
                if (centerId != null) //"0"
                {
                    if (centerId != "0")
                    {
                        sb.Append(" AND cls.CenterID =" + centerId);
                    }
                }

                if (memberId != null) //"0"
                {
                    if (memberId != "")
                    {
                        sb.Append(" AND cls.MemberID =" + memberId);
                    }
                }
                sb.Append(" AND cls.OfficeID =" + LoginUserOfficeID);
                var param = new { @AndCondition = sb.ToString()};
                var empList = groupwiseReportService.GetDataUltimateReleaseReport(param, "GETSumBalance");

                if (empList.Tables[0].Rows.Count > 0)
                {
                    List_ViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new CollectionViewModel
                    {

                        LoadPaid = row.Field<decimal>("LoadPaid"),
                        Deposit = row.Field<decimal>("Deposit"),
                        WithDrawal = row.Field<decimal>("WithDrawal"),
                        Balance = row.Field<decimal>("Balance")


                    }).ToList();
                }


                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function

        //Process
        public JsonResult Process(string centerId)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return Json(new { Result = "ERROR", Message = "Please run the start work process" });
                }

                if (centerId=="0")
                {
                    return Json(new { Result = "ERROR", Message = "CenterID Found Empty" });
                }

                var param = new { @CollectionDate = TransactionDate, @OfficeID = LoginUserOfficeID, @CenterID = centerId };
                // var empList = groupwiseReportService.GetDataUltimateReleaseReport(param, "UpdateDailyLoan_SavingsTRx");
                var empList = groupwiseReportService.GetDataUltimateReleaseReport(param, "UpdateDailyLoan_SavingsTRX_CenterWise");
                var result = 1;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        // End Function

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Collection/Create
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
        // GET: /Collection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Collection/Edit/5
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
        // GET: /Collection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Collection/Delete/5
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

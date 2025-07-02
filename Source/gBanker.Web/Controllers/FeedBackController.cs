using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Controllers;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{


    public class FeedBackController : BaseController
    {
        private readonly IMiscellaneouService MiscellaneouService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly ICenterService centerService;
        private readonly ISpecialLoanCollectionService specialLoanCollectionService;
        private readonly IMemberService memberService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IEmployeeService employeeService;

        private object sb;
        private static DataSet dt;

        public FeedBackController(IMiscellaneouService MiscellaneouService, IProductService productService, IOfficeService officeService, ICenterService centerService, ISpecialLoanCollectionService specialLoanCollectionService, IMemberService memberService, IUltimateReportService ultimateReportService, IEmployeeService employeeService)
        {
            this.MiscellaneouService = MiscellaneouService;
            this.productService = productService;
            this.officeService = officeService;
            this.centerService = centerService;
            this.specialLoanCollectionService = specialLoanCollectionService;
            this.memberService = memberService;
            this.ultimateReportService = ultimateReportService;
            this.employeeService = employeeService;

        }

        // GET: FeedBack
        public ActionResult Index(string FeedbackId = "", string CreateUser = "0")
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;

            ViewData["SameUser"] = false;
            if (Convert.ToInt32(CreateUser) == SessionHelper.LoggedInEmployeeID)
            {
                ViewData["SameUser"] = true;
            }

            var Officeinfo = officeService.GetById((int)SessionHelper.LoginUserOfficeID);
            ViewData["LoginOfficeName"] = Officeinfo.OfficeName;

            ViewData["OfficeEmail"] = Officeinfo.Email;
            ViewData["OfficeMobile"] = Officeinfo.Phone;

            var employee = employeeService.GetById((int)SessionHelper.LoggedInEmployeeID);
            ViewData["LoginEmployeeName"] = employee.EmpName;

            ViewData["FeedbackId"] = 0;

            if (FeedbackId != "" && FeedbackId != null)
            {
                ViewData["FeedbackId"] = FeedbackId;
            }


            return View();
        }


        public JsonResult GetStatusType()
        {

            StringBuilder sb = new StringBuilder();
             
            var param = new { AndCondition = sb.ToString() };
            var MessageList = ultimateReportService.GetDataWithParameter(param, "SP_Get_FeedBackStatus_List");

            List<FeedBackViewModel> List_ViewModel = new List<FeedBackViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new FeedBackViewModel
            {
                StatusId = row.Field<int>("FeedbackStatusId"),
                StatusType = row.Field<string>("StatusName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.StatusId.ToString(),
                Text = x.StatusType.ToString() , //+ " " + x.OfficeName.ToString()
                Selected = x.StatusId == 1 ? true : false
            });
            var List_items = new List<SelectListItem>();
            
            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSolvedById()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = ultimateReportService.GetDataWithParameter(param, "SP_Get_FeedBackSolvedBy_List");

            List<FeedBackViewModel> List_ViewModel = new List<FeedBackViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new FeedBackViewModel
            {
                EmployeeId = row.Field<Int32>("EmployeeId"),
                EmployeeName = row.Field<string>("EmployeeName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.EmployeeId.ToString(),
                Text = x.EmployeeName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetNecessityType()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = ultimateReportService.GetDataWithParameter(param, "SP_Get_FeedBackNecessity_List");

            List<FeedBackViewModel> List_ViewModel = new List<FeedBackViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new FeedBackViewModel
            {
                NecessityId = row.Field<int>("FeedbackNecessityId"),
                NecessityType = row.Field<string>("NecessityName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.NecessityId.ToString(),
                Text = x.NecessityType.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProblemType()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = ultimateReportService.GetDataWithParameter(param, "SP_Get_FeedBackProblem_List");

            List<FeedBackViewModel> List_ViewModel = new List<FeedBackViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new FeedBackViewModel
            {
                ProblemId = row.Field<int>("FeedbackProblemId"),
                ProblemType = row.Field<string>("ProblemName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProblemId.ToString(),
                Text = x.ProblemType.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetOfficeList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = ultimateReportService.GetDataWithParameter(param, "SP_Get_OfficeList");

            List<FeedBackViewModel> List_ViewModel = new List<FeedBackViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new FeedBackViewModel
            {
                OfficeId = row.Field<int>("OfficeId"),
                OfficeName = row.Field<string>("OfficeName")
                

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SolvedByList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = ultimateReportService.GetDataWithParameter(param, "SP_Get_OfficeList");

            List<FeedBackViewModel> List_ViewModel = new List<FeedBackViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new FeedBackViewModel
            {
                EmployeeId = row.Field<int>("OfficeId"),
                OfficeName = row.Field<string>("OfficeName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeId.ToString(),
                Text = x.OfficeName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }


        [ValidateInput(false)]
        public JsonResult SetFeedBackInfo(string FeedBackId = "0", string CorrectionStatusId = "0", string ProblemDate = "0", string SolutionRequire = "0", string Email = "0", string Mobile = "0", string NecessityId = "0", string ProblemTypeId = "0", string ProblemDetail = "0", string SolvedDetail = "0", string SolvedBy = "0")
        {
            string result = "OK";
            try
            {
                if (FeedBackId == "0")
                {
                    //ProblemDate = DateTime.Now.Date.ToString("MMMM dd, yyyy");
                }
                 
                var param = new { FeedBackId= FeedBackId, OfficeId = SessionHelper.LoginUserOfficeID, ProblemName = " Problem Name ", CorrectionStatusId = CorrectionStatusId, ProblemDate = ProblemDate, SolutionRequire = SessionHelper.LoginUserOfficeID, Email = Email, Mobile = Mobile, NecessityId = NecessityId, ProblemTypeId = ProblemTypeId, ProblemDetail = ProblemDetail, SolvedDetail = SolvedDetail, EmployeeId= LoggedInEmployeeID, SolvedBy = SolvedBy, CreateUser = LoggedInEmployeeID};
                var empList = ultimateReportService.GetDataWithParameter(param, "FeedBack_Log");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;
            return View();
        }

        //public JsonResult GetList(string Zone, string Status, string Necessity, string Problem, string ProblemDetail, string SolvedDetail, string ProblemDate, string EntryDate, string SolvedBy)
        //{
        //    string result = "OK";
        //    try
        //    {
        //        var param = new { OfficeId = SessionHelper.LoginUserOfficeID, Zone= Zone, Status= Status, Necessity= Necessity, Problem= Problem, ProblemDetail= ProblemDetail, SolvedDetail= SolvedDetail, SolvedBy= SolvedBy, ProblemDate = ProblemDate, EntryDate = EntryDate };
        //        var empList = ultimateReportService.GetDataWithParameter(param, "FeedBack_Log");

        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 403;
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        public JsonResult GetCorrectionList(string CorrectionId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, string OfficeId = "", string NecessityId = "", string CorrectionStatusId ="")
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                var officeinfo = officeService.GetById((int)SessionHelper.LoginUserOfficeID);
                var Officelevel = officeinfo.OfficeLevel;

                var EmployeeInfo = SessionHelper.LoggedInEmployeeID;


                if (CorrectionId != null) //"0"
                {
                    if (CorrectionId != "0")
                        sb.Append("AND fdl.FeedBackId =" + CorrectionId);
                }

                if (NecessityId != null) //"0"
                {
                    if (NecessityId != "0" && NecessityId !="")
                        sb.Append("AND fdl.NecessityId =" + NecessityId);
                }


                if (CorrectionStatusId != null) //"0"
                {
                    if (CorrectionStatusId != "0" && CorrectionStatusId != "")
                        sb.Append("AND fdl.CorrectionStatusId =" + CorrectionStatusId);
                }


                if (OfficeId != "0" && OfficeId != "") //"0"
                {
                   
                        var officeinfo2 = officeService.GetById(Convert.ToInt32(OfficeId));
                        sb.Append("AND fdl.OfficeId =" + officeinfo2.OfficeID);
                   
                }
                else
                {
                    if (Officelevel == 1)
                    {

                    }
                    else
                    {
                       // sb.Append("AND fdl.OfficeId =" + officeinfo.OfficeID);
                    }
                }

                if (EmployeeInfo > 0)
                {
                    sb.Append("AND fdl.OfficeId IN ( SELECT OfficeID FROM EmployeeOfficeMapping WHERE EmployeeId = " + EmployeeInfo + " ) ");
                }
                 
                List<FeedBackViewModel> List_ViewModel = new List<FeedBackViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = ultimateReportService.GetDataWithParameter(param, "SP_Get_FeedBackList");

                dt = new DataSet();
                dt = empList;

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new FeedBackViewModel
                {

                    CorrectionId = row.Field<Int64>("FeedBackId"),
                    Zone = row.Field<string>("ZoneName"),
                    Branch = row.Field<string>("ThirdLevel"),
                    Status = row.Field<string>("StatusName"),
                    Necessity = row.Field<string>("NecessityName"),              
                    NecessityId = row.Field<int>("NecessityId"),
                    Problem = row.Field<string>("ProblemName"),
                    ProblemTypeId = row.Field<int>("ProblemTypeId"),
                    ProblemDetail = row.Field<string>("ProblemDetail"),
                    SolvedDetail = row.Field<string>("SolvedDetail"),
                    SolvedBy = row.Field<string>("SolvedBy"),
                    ProblemDate = row.Field<string>("ProblemDateMsg"),
                    SolvedDate = row.Field<string>("SolvedDateMsg"),
                    EntryDate = row.Field<string>("EntryDateMsg"),
                    CorrectionStatusId = row.Field<int>("CorrectionStatusId"),
                    Email = row.Field<string>("Email"),
                    Mobile = row.Field<string>("Mobile"),
                    CreateUser = row.Field<int>("CreateUser")

                }).ToList();

                if (CorrectionId != null) //"0"
                {
                    if (CorrectionId != "0")
                    {
                        return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                    }
                }

                //var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                //return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });


                var detail = List_ViewModel.ToList();
                var TotCount = detail.Count();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function


       
        public JsonResult Create_NEW_NOTE
       (
             string FeedBackId
           , string Comment
           , string Date

       )
        {
            string result = "Data Saved Successfully"; ;
            try
            {
                
                var param = new {
                    @FeedbackId   = FeedBackId,
                    @EmployeeId   = SessionHelper.LoggedInEmployeeID,
                    @Comment     = Comment,
                    @CreateUser = SessionHelper.LoginUserEmployeeID
                  
                };
                var empList = ultimateReportService.GetDataWithParameter(param, "FeedBack_Note");
 
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }// ENd of Function


        public JsonResult GetNoteList(string FeedbackId, string NoteId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue )
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (NoteId != null) //"0"
                {
                    if (NoteId != "0")
                        sb.Append(" AND M.NoteId =" + NoteId);
                }

                if (FeedbackId != null) //"0"
                {
                    if (FeedbackId != "" && FeedbackId != "0")
                        sb.Append(" AND M.FeedBackId =" + FeedbackId);
                }

                List<FeedBackViewModel> List_ViewModel = new List<FeedBackViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = ultimateReportService.GetDataWithParameter(param, "SP_Get_NoteList");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new FeedBackViewModel
                {
                    rowSl = row.Field<Int64>("rowSl"),
                    User = row.Field<string>("EmpName"),
                    Comment = row.Field<string>("Comment"),
                    NoteId = row.Field<Int64>("NoteID"),                  
                    Date = row.Field<string>("Date")

                }).ToList();

                //if (MessageId != null)
                //{
                //    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                //}

                //var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                //return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });


                var detail = List_ViewModel.ToList();
                var TotCount = detail.Count();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function


    }// END Class
}// END Namespace
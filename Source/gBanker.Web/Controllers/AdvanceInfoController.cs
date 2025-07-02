using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class AdvanceInfoController : BaseController
    {

        #region Variables
        private readonly ILoanTrxService loantrxService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ILoanSummaryService loanSummaryService;

        public AdvanceInfoController(ILoanTrxService loantrxService, IMemberService memberService, IOfficeService officeService, IUltimateReportService ultimateReportService, IGroupwiseReportService groupwiseReportService, ILoanSummaryService loanSummaryService)
        {
            this.loantrxService = loantrxService;
            this.memberService = memberService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.groupwiseReportService = groupwiseReportService;
            this.loanSummaryService = loanSummaryService;

        }
        #endregion Variables

        // GET: EmployeeSalInfo
        /// <summary>
        /// KHALID
        /// #22 Oct 2018
        /// </summary>
        /// <returns>View</returns>
        /// 
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ddlList"] = items;

            return View();
        }

        public ActionResult AdvanceSector()
        {
             
            return View();
        }

        


        /// <summary>
        /// GET AdvanceInfoList
        /// </summary>
        /// <param name="jtStartIndex"></param>
        /// <param name="jtPageSize"></param>
        /// <param name="jtSorting"></param>
        /// <param name="hdnEmployeeID"></param>
        /// <returns></returns>
        public JsonResult GenerateAdvanceInfoList(int jtStartIndex, int jtPageSize, string jtSorting, string hdnEmployeeID = "")
        {
            try
            {

                StringBuilder sb = new StringBuilder();
                if (hdnEmployeeID != "" && hdnEmployeeID != "0")
                {
                    sb.Append("AND ai.EmployeeID = " + hdnEmployeeID);
                }

                var param1 = new { @AndCondition = sb.ToString() };

                List<AdvanceInfoViewModel> List_AdvanceInfoViewModel = new List<AdvanceInfoViewModel>();
                var empList = ultimateReportService.GetAdvanceInfo(param1);

                List_AdvanceInfoViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new AdvanceInfoViewModel
                {
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName"),
                    EmployeeID = row.Field<int>("EmployeeID"),

                    AdvanceDateMSG = row.Field<string>("AdvanceDate"),
                    AdvanceAmount = row.Field<decimal>("AdvanceAmount"),
                    AdvanceSectorId = row.Field<int>("AdvanceSectorId"),
                    SectorName = row.Field<string>("SectorName"),
                    Remarks = row.Field<string>("Remarks"),
                    preparedBy = row.Field<decimal>("preparedBy"),
                    checkedBy = row.Field<decimal>("checkedBy"),
                    approvedBy = row.Field<decimal>("approvedBy"),
                    ShowInSalarySheet = row.Field<int>("ShowInSalarySheet"),
                    CreateDate = row.Field<DateTime>("CreateDate"),
                    CreateUser = row.Field<string>("CreateUser")

                }).ToList();

                var currentPageRecords = List_AdvanceInfoViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_AdvanceInfoViewModel.LongCount() });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult GetEmployeeList()
        {
            try
            {
                var param1 = new { @AndCondition = "" };

                List<EmployeeSalViewModel> List_EmployeeSalViewModel = new List<EmployeeSalViewModel>();
                var empList = ultimateReportService.GetEmployeeList(param1);


                List_EmployeeSalViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeSalViewModel
                {
                    EmployeeID = row.Field<int>("EmployeeID"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    EmployeeName = row.Field<string>("EmployeeName")

                }).ToList();

                var Components = List_EmployeeSalViewModel.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.EmployeeID.ToString(),
                    Text = string.Format("{0} - {1}", x.EmployeeCode, x.EmployeeName)
                });

                var ddl_items = new List<SelectListItem>();
                if (Components.ToList().Count > 0)
                {
                    ddl_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                ddl_items.AddRange(Components);
                return Json(ddl_items, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult GetSectorList()
        {
            try
            {
                var param1 = new { @AndCondition = "" };

                List<AdvanceInfoViewModel> List_EmployeeSalViewModel = new List<AdvanceInfoViewModel>();
                var empList = ultimateReportService.GetSectorList(param1);


                List_EmployeeSalViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new AdvanceInfoViewModel
                {
                    AdvanceSectorId = row.Field<int>("AdvanceSectorId"),
                    SectorName = row.Field<string>("SectorName") 
                }).ToList();

                var Components = List_EmployeeSalViewModel.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.AdvanceSectorId.ToString(),
                    //Text = string.Format("{0} - {1}", x.EmployeeCode, x.EmployeeName)
                    Text = string.Format("{0}", x.SectorName)
                });

                var ddl_items = new List<SelectListItem>();
                if (Components.ToList().Count > 0)
                {
                    ddl_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                ddl_items.AddRange(Components);
                return Json(ddl_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }






        /// <summary>
        /// Access: Create Or Update AdvanceInfo
        /// </summary>
        /// <param name="hdnEmployeeID"></param>
        /// <param name="txtAdvanceDate"></param>
        /// <param name="txtAdvanceAmount"></param>
        /// <param name="txtAdvanceSectorId"></param>
        /// <param name="txtRemarks"></param>
        /// <param name="txtpreparedBy"></param>
        /// <param name="txtcheckedBy"></param>
        /// <param name="txtapprovedBy"></param>
        /// <param name="txtShowInSalarySheet"></param>
        /// <returns></returns>
        public JsonResult CreateUpdate(

              string hdnEmployeeID
            , string txtAdvanceDate = "0"
            , string txtAdvanceAmount = "0"
            , string txtAdvanceSectorId = "0"
            , string txtRemarks = "0"
            , string txtpreparedBy = "0"
            , string txtcheckedBy = "0"
            , string txtapprovedBy = "0"
            , string txtShowInSalarySheet = "0"

            )
        {
            string result = "Data Saved Successfully.";
            try
            {
                int EmpID = 0;
                if (hdnEmployeeID != "")
                {
                    if (hdnEmployeeID != null)
                    {
                        EmpID = Convert.ToInt32(hdnEmployeeID);
                    }
                }

                var param1 = new
                {
                     @hdnEmployeeID = EmpID 
                    ,@txtAdvanceDate = txtAdvanceDate
                    ,@txtAdvanceAmount = txtAdvanceAmount
                    ,@txtAdvanceSectorId = txtAdvanceSectorId
                    ,@txtRemarks = txtRemarks
                    ,@txtpreparedBy = txtpreparedBy
                    ,@txtcheckedBy = txtcheckedBy
                    ,@txtapprovedBy = txtapprovedBy
                    ,@txtShowInSalarySheet = txtShowInSalarySheet
                    ,@txtCreateUser = LoggedInEmployee.EmployeeID

                };
                var empList = ultimateReportService.AccessAdvanceInfo(param1);

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        } // END FUNCTION






        public JsonResult GenerateAdvanceSectorList(int jtStartIndex, int jtPageSize, string jtSorting, string txtAdvanceSectorId = "")
        {
            try
            {

                StringBuilder sb = new StringBuilder();
                if (txtAdvanceSectorId != "" && txtAdvanceSectorId != "0")
                {
                    sb.Append("AND asi.AdvanceSectorId = " + txtAdvanceSectorId);
                }

                var param1 = new { @AndCondition = sb.ToString() };

                List<AdvanceInfoViewModel> List_AdvanceInfoViewModel = new List<AdvanceInfoViewModel>();
                var empList = ultimateReportService.GetAdvanceSector(param1);

                List_AdvanceInfoViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new AdvanceInfoViewModel
                {
                    AdvanceSectorId = row.Field<int>("AdvanceSectorId"),
                    SectorName = row.Field<string>("SectorName")
                    
                }).ToList();

                var currentPageRecords = List_AdvanceInfoViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_AdvanceInfoViewModel.LongCount() });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult CreateUpdateAdvanceSector(
             string txtAdvanceSectorId
           , string txtSectorName = "0"
           )
        {
            string result = "Data Saved Successfully.";
            try
            {
                int hdAdvanceSectorId = 0;
                if (txtAdvanceSectorId != "")
                {
                    if (txtAdvanceSectorId != null)
                    {
                        hdAdvanceSectorId = Convert.ToInt32(txtAdvanceSectorId);
                    }
                }

                var param1 = new
                {
                    @txtAdvanceSectorId = txtAdvanceSectorId
                    ,
                    txtSectorName = txtSectorName
                    ,
                    @txtCreateUser = LoggedInEmployee.EmployeeID

                };
                var empList = ultimateReportService.AccessAdvanceSector(param1);

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        } // END FUNCTION


    } //End Of Controller.
}// END NameSpace
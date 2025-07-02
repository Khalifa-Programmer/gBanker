using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service.ReportServies;
using gBanker.Service.StoredProcedure;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class BankInformationController : BaseController
    {

        // GET: Member
        #region Variables

        private readonly IUltimateReportService ultimateReportService;

        public BankInformationController(IUltimateReportService ultimateReportService)
        {
            this.ultimateReportService = ultimateReportService;
        }
        #endregion

        #region Methods

        // GET: BankInformation
        public ActionResult ManageBankInfo()
        {
            //List<SelectListItem> items = new List<SelectListItem>();
            // ViewData["component"] = items;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ComponentList"] = items;


            return View();
        }

        #endregion

        #region Events

        public JsonResult CreateUpdateBankInfo(BankInformationViewModel bankInfo)
        {
            string result = "Data Saved Successfully";
            try
            {
                //Check If Same Work area Name
                List<BankInformationViewModel> List_ViewModel = new List<BankInformationViewModel>();
                if (bankInfo.BankInfoID == 0)
                {
                    var param2 = new { @BankName = bankInfo.BankName };
                    var empList = ultimateReportService.GetDataWithParameter(param2, "SP_PR_Get_BankInfo_ByName");

                    List_ViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new BankInformationViewModel
                    {
                        BankInfoID = row.Field<long>("BankInfoID"),
                        BankName = row.Field<string>("BankName")
                    }).ToList();

                    if (List_ViewModel.Count > 0)
                    {
                        Response.StatusCode = 403;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                //End of Check

                Int64 CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                DateTime CreateDate = DateTime.Now;
                //(@WorkAreaName varchar(200), @CreateUser varchar(100), @CreateDate datetime)
                var param = new { BankInfoID = bankInfo.BankInfoID, AccId = bankInfo.AccID, BankName = bankInfo.BankName, AccountNumber = bankInfo.AccountNumber, BankAddress = bankInfo.BankAddress, officeId = SessionHelper.LoginUserOfficeID, CreateUser = CreateUser };
                var val = ultimateReportService.GetDataWithParameter(param, "SP_CreateUpdateBank");

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Show List
        public JsonResult GetBankInfoList(string BankInfoID, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string PRWorkAreaIDs = Convert.ToString(BankInfoID);

                if (BankInfoID != null) //"0"
                    sb.Append(" AND bi.BankInfoID =" + PRWorkAreaIDs);

                List<BankInformationViewModel> List_ViewModel = new List<BankInformationViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = ultimateReportService.GetDataWithParameter(param, "SP_Get_BankInfo_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new BankInformationViewModel
                {
                    rowSl = row.Field<Int64>("rowSl"),
                    BankInfoID = row.Field<long>("BankInfoID"),
                    AccID = row.Field<int>("AccId"),
                    BankName = row.Field<string>("BankName"),
                    AccountNumber = row.Field<string>("AccountNumber"),
                    BankAddress = row.Field<string>("BankAddress"),
                    AccCode = row.Field<string>("AccCode"),
                    BankCode = row.Field<string>("BankCode")

                }).ToList();

                if (BankInfoID != null)
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function

        public JsonResult GetAccCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" AND AccCode IS NOT NULL ");
            sb.Append(" AND SecondLevel IS NOT NULL ");
            sb.Append(" AND asett.OfficeID = " + SessionHelper.LoginUserOfficeID.Value.ToString());

            // SessionHelper.LoginUserOfficeID.Value
            var officeId = SessionHelper.LoginUserOfficeID;

            List<BankInformationViewModel> List_ViewModel = new List<BankInformationViewModel>();
            var param = new { AndCondition = sb.ToString() };
            var List = ultimateReportService.GetDataWithParameter(param, "SP_Get_AccCode");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new BankInformationViewModel
            {
                AccID = row.Field<int>("AccID"),
                AccCode = row.Field<string>("AccCode"),
                AccName = row.Field<string>("AccName"),
                BankCode = row.Field<string>("BankCode")

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.AccID.ToString(), // string.Format("{0} - {1}", x.AccID.ToString(), x.BankCode.ToString()) ,
                Text = string.Format("{0} - {1}", x.AccCode, x.AccName)
            });

            var Component_items = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items.AddRange(Components);
            return Json(Component_items, JsonRequestBehavior.AllowGet);

        }

        public JsonResult DeleteBankInfo(BankInformationViewModel bankInfo)
        {
            string result = "Data Saved Successfully";
            try
            {


                Int64 CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                DateTime CreateDate = DateTime.Now;
                //(@WorkAreaName varchar(200), @CreateUser varchar(100), @CreateDate datetime)
                var param = new { BankInfoID = bankInfo.BankInfoID, CreateUser = CreateUser };
                var val = ultimateReportService.GetDataWithParameter(param, "SP_DeleteBankInfo");

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
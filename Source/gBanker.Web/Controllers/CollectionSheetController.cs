using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class CollectionSheetController : BaseController
    {
        #region Private Members

        private readonly IUltimateReportService ultimateReportService;
        private readonly ICenterService centerService;

        #endregion

        #region Ctor
        public CollectionSheetController(
            IUltimateReportService ultimateReportService,
            ICenterService centerService
           )
        {

            this.ultimateReportService = ultimateReportService;
            this.centerService = centerService;
        }
        #endregion

        #region Collection Sheet
        public ActionResult CollectionSheet()
        {
            var model = new CollectionSheetViewModel { LoggedInOrganizationID = LoggedInOrganizationID };
            MapDropDownList(model);
            return View(model);
        }

        #endregion

        #region Ajax Calls

        [HttpPost]
        public JsonResult ApproveBatchCollectSheet(CollectionSheetStatusViewModel model)
        {
            try
            {
                foreach (var item in model.CSTrxIdXTransTypes)
                {
                    decimal savingInstallment = 0;
                    //get savings installment
                    savingInstallment = GetSavingsInstallment(item);

                    var param = new
                    {
                        TrxID = item.TrxId,
                        MemberID = item.MemberID,
                        TransType = item.TransType,
                        TotalLPaid = item.TotalLPaid,
                        LoanPaid = item.LoanPaid,
                        IntPaid = item.IntPaid,
                        SavingInstallment = savingInstallment
                    };

                    ultimateReportService.GetDataWithParameter(param, "[dbo].[CollectionSheet_UpdateCollectionSheetWithStatus]");
                }

                return GetSuccessMessageResult("Success! Operation Completed");
            }
            catch(Exception ex)
            {
                return GetErrorMessageResult("Error! There was an error while processing operation");
            }
        }       

        [HttpPost]
        public JsonResult ApproveCollectionSheet(int memberID)
        {
            try
            {
                var param = new { MemberID = memberID };
                ultimateReportService.GetDataWithParameter(param, "[dbo].[CollectionSheet_ApproveCollectionSheet]");

                return GetSuccessMessageResult("Success! Operation Completed");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while processing operation");
            }
        }

        public ActionResult GetCollectionSheetDetails(int CenterID, string memberCode)
        {
            try
            {
                var param = new { OfficeID = Convert.ToInt16(LoginUserOfficeID), CenterID = CenterID, MemberCode = memberCode };
                var getData = ultimateReportService.GetDataWithParameter(param, "[dbo].[DailySavingTrx_GetCollection_By_MemberCode]");

                //Populate Collection Sheet
                var collectionSheet = PopulateCollectionSheet(getData);
                var model = new CollectionSheetListViewModel { CollectionSheets = collectionSheet };
                return PartialView("_CollectionSheetDetailsPartial", model);
            }
            catch (Exception ex)
            {
                return PartialView("_CollectionSheetDetailsPartial", new CollectionSheetListViewModel { });
            }
        }

        public JsonResult GetCollectionSheetList(string jtSorting, string filterValue, int CenterID, int? jtStartIndex, int? jtPageSize /*, bool isCalculateTotal,int totalCount*/)
        {
            try
            {

                var param = new { OfficeID = Convert.ToInt16(LoginUserOfficeID), CenterID = CenterID, MemberCode = filterValue /*, IsCalculateTotal= isCalculateTotal, PageNumber= pageNumber, PageSize= jtPageSize */};
                var getData = ultimateReportService.GetDataWithParameter(param, "getCollectionEntryScreen");

                //Populate Collection Sheet
                var collectionSheet = PopulateCollectionSheet(getData);

                jtStartIndex = 0; jtPageSize = 999999;
                var currentPageRecords = collectionSheet.Skip((int)jtStartIndex).Take((int)jtPageSize);

                return Json(new { Result = "OK", Records = collectionSheet, TotalRecordCount = currentPageRecords, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Private Methods
        private decimal GetSavingsInstallment(CSTrxIdXTransTypeViewModel item)
        {
            decimal savingInstallment;
            switch (item.nType)
            {
                case "G":
                    {
                        savingInstallment = item.GSavings;
                        break;
                    }                
                case "SSP":
                    {
                        savingInstallment = item.SSP;
                        break;
                    }
                case "VS":
                    {
                        savingInstallment = item.VS;
                        break;
                    }
                default:
                    {
                        savingInstallment = 0;
                        break;
                    }
            }

            return savingInstallment;
        }
        private static List<CollectionSheetViewModel> PopulateCollectionSheet(DataSet getData)
        {
            return getData.Tables[0].AsEnumerable().Select(p => new CollectionSheetViewModel
            {
                OfficeID = p.Field<int>("OfficeID"),
                TrxID = p.Field<Int64?>("TrxID"),
                CenterID = p.Field<int>("CenterID"),
                LoanDue = p.Field<decimal>("LoanDue"),
                LoanPaid = p.Field<decimal>("LoanPaid"),
                IntDue = p.Field<decimal>("IntDue"),
                IntPaid = p.Field<decimal>("IntPaid"),

                PrincipalLoan = p.Field<decimal>("PrincipalLoan"),
                LoanRepaid = p.Field<decimal>("LoanRepaid"),
                CumIntCharge = p.Field<decimal>("CumIntCharge"),
                CumIntPaid = p.Field<decimal>("CumIntPaid"),

                DurationOverLoanDue = p.Field<decimal>("DurationOverLoanDue"),
                DurationOverIntDue = p.Field<decimal>("DurationOverIntDue"),

                SavingInstallment = p.Field<decimal>("SavingInstallment"),
                CollectionStatus = p.Field<string>("CollectionStatus"),

                TransType = p.Field<string>("TransType"),
                nType = p.Field<string>("nType"),
                GSavings = p.Field<decimal>("GSavings"),
                VS = p.Field<decimal>("VS"),
                SSP = p.Field<decimal>("SSP"),
                MemberID = p.Field<long>("MemberID"),
                MemberCode = p.Field<string>("MemberCode"),
                MemberName = p.Field<string>("MemberName"),

                ProductCode = p.Field<string>("ProductCode"),
                ProductName = p.Field<string>("ProductName"),
                AccountNo = p.Field<string>("AccountNo"),

                installmentNo = p.Field<Int32?>("InstallmentNo"),
                Duration = p.Field<Int32?>("Duration"),

                Balance = p.Field<decimal>("Balance"),
                DueSavingInstallment = p.Field<decimal>("DueSavingInstallment"),

            }).ToList();
        }

        private void MapDropDownList(CollectionSheetViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }

            var param1 = new { @EmpID = LoggedInEmployeeID };
            var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);

            IEnumerable<Center> allcenter;
            if (LoanInstallMent != null)
                allcenter = centerService.SearchOffCenter(TransactionDay, SessionHelper.LoginUserOfficeID.Value, LoggedInOrganizationID, Convert.ToInt16(LoggedInEmployeeID), LoanInstallMent.Tables[0].Rows[0]["Name"].ToString());
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
        }

        #endregion
    }
}
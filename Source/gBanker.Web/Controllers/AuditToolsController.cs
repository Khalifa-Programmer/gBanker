using System.Dynamic;
using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Web.Core.Extensions;
using gBanker.Data.CodeFirstMigration;
using System.Configuration;
using gBanker.Web.Helpers;
using Microsoft.AspNet.Identity;
using System.Data;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using gBanker.Service.ReportServies;
using System.Text;

namespace gBanker.Web.Controllers
{
    public class AuditToolsController : BaseController
    {

        List<ValidateResultModel> WrongCenterInfoList = new List<ValidateResultModel>();
        private static DataSet empList;//= new DataSet();

        

        private readonly ICenterService centerService;
        private readonly ISpecialLoanCollectionService specialLoanCollectionService;
        private readonly IBranchService branchService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;
        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        private readonly ILoanSummaryService LoanSummaryService;
        private readonly IUltimateReportService ultimateReportService;
        public AuditToolsController(ISpecialLoanCollectionService specialLoanCollectionService, IUltimateReportService ultimateReportService, ILoanSummaryService LoanSummaryService, ICenterService centerService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, IPurposeService purposeService, IMemberService memberService)
        {
            this.specialLoanCollectionService = specialLoanCollectionService;
            this.centerService = centerService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.officeService = officeService;
            this.purposeService = purposeService;
            this.memberService = memberService;
            this.LoanSummaryService = LoanSummaryService;
            this.ultimateReportService = ultimateReportService;

        }

        // GET: AuditTools
        public ActionResult Index()
        {
            specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);

            var model = new AuditToolsViewModel();

            if (IsDayInitiated)
                model.TrxDate = TransactionDate;
            MapDropDownList(model);

            model.CheckStaffDataTable = new List<dynamic>();
            var v = model.CheckStaffDataTable.Count;

            model.CheckStaffDataTable2 = new List<dynamic>();
            var v2 = model.CheckStaffDataTable2.Count;

            model.CheckStaffDataTable3 = new List<dynamic>();
            var v3 = model.CheckStaffDataTable3.Count;


            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;


            return View(model);
        }

        [HttpPost]
        public ActionResult Index(AuditToolsViewModel Model)
        {
            try
            {
                //PART 1
                ListToDataTableHelper convertDT = new ListToDataTableHelper();
                DataTable dd = new DataTable();

                var param2 = new
                {
                    @OfficeId = Model.OfficeId,
                    @MemberId = Model.MemberID
                };
                empList = ultimateReportService.GetDataWithParameter(param2, "AuditToolsSavingsInfo");


                dd = empList.Tables[0];
                var resultset = convertDT.ConvertToDictionary(dd);

                var result = new List<dynamic>();

                foreach (var emprow in resultset)
                {
                    var row = (IDictionary<string, object>)new ExpandoObject();
                    Dictionary<string, object> eachRow = (Dictionary<string, object>)emprow;

                    foreach (KeyValuePair<string, object> keyValuePair in eachRow)
                    {
                        row.Add(keyValuePair);
                    }
                    result.Add(row);
                }

                Model.CheckStaffDataTable = result.Count > 0 ? result : new List<dynamic>();


                // Part 2
                //var param  = new
                //{
                //    @OfficeId = Model.OfficeId,
                //    @MemberId = Model.MemberID
                //};
                empList = ultimateReportService.GetDataWithParameter(param2, "AuditToolsLoanInfo");


                dd = empList.Tables[0];
                resultset = convertDT.ConvertToDictionary(dd);

                result = new List<dynamic>();

                foreach (var emprow in resultset)
                {
                    var row = (IDictionary<string, object>)new ExpandoObject();
                    Dictionary<string, object> eachRow = (Dictionary<string, object>)emprow;

                    foreach (KeyValuePair<string, object> keyValuePair in eachRow)
                    {
                        row.Add(keyValuePair);
                    }
                    result.Add(row);
                }

                Model.CheckStaffDataTable2 = result.Count > 0 ? result : new List<dynamic>();

                //Part 3

                 
                //var param  = new
                //{
                //    @OfficeId = Model.OfficeId,
                //    @MemberId = Model.MemberID
                //};
                empList = ultimateReportService.GetDataWithParameter(param2, "AuditToolsMemberInfo");


                dd = empList.Tables[0];
                resultset = convertDT.ConvertToDictionary(dd);

                result = new List<dynamic>();

                foreach (var emprow in resultset)
                {
                    var row = (IDictionary<string, object>)new ExpandoObject();
                    Dictionary<string, object> eachRow = (Dictionary<string, object>)emprow;

                    foreach (KeyValuePair<string, object> keyValuePair in eachRow)
                    {
                        row.Add(keyValuePair);
                    }
                    result.Add(row);
                }

                Model.CheckStaffDataTable3 = result.Count > 0 ? result : new List<dynamic>();






                MapDropDownList(Model);

                IEnumerable<SelectListItem> items = new SelectList(" ");
                ViewData["comtype"] = items;

                return View(Model);
                 

            }
            catch (Exception ex)
            {
                Model.Result = 2;
                Model.ReturnMessage = "You provided wrong parameter for this check.";

               return GetErrorMessageResult(ex);
            }// END Catch
        }


        public JsonResult LoadAccountList(string OfficeId, string MemberId)
        {

            
            var param = new { @OfficeId = OfficeId, @MemberId = MemberId };
            var List = ultimateReportService.GetDataWithParameter(param, "AuditToolsCheckAccountNo");

           

            List<AuditToolsViewModel> List_ViewModel = new List<AuditToolsViewModel>();
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new AuditToolsViewModel
            {
                SummaryID = row.Field<string>("SummaryID"),
                AccountNo = row.Field<string>("AccountNo")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.SummaryID.ToString(),
                Text = x.AccountNo.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 

        public JsonResult AuditToolsCheckInsert(string SummaryID, string CheckDate)
        {
            try
            {

                var param = new { @SummaryID = SummaryID, @CheckDate = CheckDate };
                var List = ultimateReportService.GetDataWithParameter(param, "AuditToolsCheckInsert");

                return Json("Data Saved Successfully", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //return Json(new { Result = "ERROR", Message = ex.Message });
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }


        }//  END  Function 

        private void MapDropDownList(AuditToolsViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var Transtype = new List<SelectListItem>();

            if (LoggedInOrganizationID == 5 || LoggedInOrganizationID == 8 || LoggedInOrganizationID == 23 || LoggedInOrganizationID == 54)
            {
                //  Transtype.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
                Transtype.Add(new SelectListItem() { Text = "Cash", Value = "20", Selected = true });

            }
            else
            {
                // Transtype.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
                Transtype.Add(new SelectListItem() { Text = "Cash", Value = "20", Selected = true });
                Transtype.Add(new SelectListItem() { Text = "Transfer", Value = "21" });
                Transtype.Add(new SelectListItem() { Text = "Rebate", Value = "31" });
            }


            model.cashListItems = Transtype.AsEnumerable();

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allpurpose = purposeService.SearchPurpose(Convert.ToInt32(LoggedInOrganizationID));

            var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });

            model.purposeListItems = viewPurpose;


            string vCoday = TransactionDay;



            var param1 = new { @EmpID = LoggedInEmployeeID };
            var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);

            IEnumerable<Center> allcenter;
            if (LoanInstallMent != null)
            {
                var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
                if (empType == "FO")
                {
                    allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt16(LoggedInEmployeeID));
                }
                else
                    allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));
            }

            else
                allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));


            //var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt16(LoggedInEmployeeID));
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;
             
            //var allSearchProd = productService.SearchProduct(0, Convert.ToInt32(LoggedInOrganizationID), "L");
            //var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.ProductID.ToString(),
            //    Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            //});

            //var proditems = new List<SelectListItem>();
            //proditems.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            //model.productListItems = proditems;

            ////model.MemberProductItemsSelected = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(allproduct);

            //var allmembercategory = membercategoryService.GetAll().Where(a => a.OrgID == LoggedInOrganizationID);

            //var viewmembercategory = allmembercategory.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCategoryID, m.CategoryName), Value = m.MemberCategoryID.ToString() });

            //model.membercategoryListItems = viewmembercategory;

        }


    }// END of Class
}// End of Namespace

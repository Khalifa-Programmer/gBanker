using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
//using gBanker.Web.Models;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Web.Core.Extensions;
using CrystalDecisions.Shared;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace gBanker.Web.Controllers
{
    //[Authorize]
    public class ProductController : BaseController
    {
         private readonly IProductService productService;
         private readonly IInvestorService investorService;
         private readonly IMemberCategoryService memberCategoryService;
         private readonly IProductReportService productReportService;
        public ProductController(IProductService productService, IInvestorService investorService, IMemberCategoryService memberCategoryService, IProductReportService productReportService)
        {
            this.productService = productService;
            this.investorService = investorService;
            this.memberCategoryService = memberCategoryService;
            this.productReportService = productReportService;
        }
        public ActionResult ExportData()
         {
             GridView gv = new GridView();
             var allRepaymentSchedule = productService.GetAll().Where(p=>p.OrgID==LoggedInOrganizationID);
             var detail = allRepaymentSchedule.ToList();
             gv.DataSource = detail;
             gv.DataBind();
             Response.ClearContent();
             Response.Buffer = true;
             Response.AddHeader("content-disposition", "attachment; filename=Product.xls");
             Response.ContentType = "application/ms-excel";
             Response.Charset = "";
             StringWriter sw = new StringWriter();
             HtmlTextWriter htw = new HtmlTextWriter(sw);
             gv.RenderControl(htw);
             Response.Output.Write(sw.ToString());
             Response.Flush();
             Response.End();

             return RedirectToAction("ProductDetails");
         }
        public ActionResult Index()
         
         {
             return View();
         }
         [HttpPost]
        public ActionResult GetProducts(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {

                long totalCount;
               var allproduct = productService.GetProductDetailPaged(filterColumn, filterValue, jtStartIndex, jtSorting, jtPageSize, out totalCount,LoggedInOrganizationID);
               var currentPageRecords = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(allproduct);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        
        }
        public ActionResult GenerateReport(string fromDate, string toDate)
         {             
             var param = new { Status = 1 };
             var allproducts = productReportService.GetDataProductInfo(param);

             var reportParam = new Dictionary<string, object>();
             reportParam.Add("Header1", ApplicationSettings.OrganiztionName);
             ReportHelper.PrintReport("rptProductInfo.rpt", allproducts.Tables[0], reportParam);
             return Content(string.Empty); ;

             //ReportHelper.PrintReport("rptProductInfo.rpt", allproducts.Tables[0], new Dictionary<string, object>());
             //return Content(string.Empty); ;
         }
        public void UpdateMethod(int Id, DateTime newValue)
        {
            //using (gBankerEntities ctx = new gBankerEntities())
            //{
            //    var query = (from q in ctx.Products
            //                 where q.ProductID == Id
            //                 select q).First();
            //    query.IsActive = false;
            //    query.InActiveDate  = newValue;
            //    ctx.SaveChanges();

            //}
        }
        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Product/Create
        public ActionResult Create()
        {

           // ViewBag.investorList = viewInvestor;

            var model = new ProductViewModel();
            MapDropDownListCreate(model);            
        
            return View(model);
            //////////////////===================================================/////////////////////////////////////////////////
            //Below line of code will set the default value in the view....
            //var defaultCategory = new InvestorViewModel() { in = "", ParentMemberCategoryID = 1, CategoryList = viewCategory.Select(s => new SelectListItem() { Text = s.CategoryName, Value = s.MemberCategoryID.ToString() }) };
            //return View(defaultCategory);


        }
        private void MapDropDownList(ProductViewModelEditMode model)
        {
            var frequency = new List<SelectListItem>();
            frequency.Add(new SelectListItem() { Text = "Weekly", Value = "W", Selected = true });
            frequency.Add(new SelectListItem() { Text = "Monthly", Value = "M" });
            frequency.Add(new SelectListItem() { Text = "Fortnightly", Value = "F" });

            var calcList = new List<SelectListItem>();
            calcList.Add(new SelectListItem() { Text = "Flat", Value = "F" });
            calcList.Add(new SelectListItem() { Text = "Declined", Value = "D", Selected = true });
            calcList.Add(new SelectListItem() { Text = "Amortaization", Value = "A" });
            calcList.Add(new SelectListItem() { Text = "Amortaization-Fixed", Value = "E" });
            calcList.Add(new SelectListItem() { Text = "Abasan", Value = "H" });
            calcList.Add(new SelectListItem() { Text = "Variance", Value = "V" });
            var prodType = new List<SelectListItem>();
            prodType.Add(new SelectListItem() { Text = "Savings", Value = "0" });
            prodType.Add(new SelectListItem() { Text = "Loan", Value = "1", Selected = true });

            //var allinvestor = investorService.SearchInvestor();

            // var viewInvestor = allinvestor.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.InvestorCode, m.InvestorName), Value = m.InvestorID.ToString() });

            model.PInvestorListItems = prodType;
            model.PCalcuationMethodListItems = calcList.AsEnumerable();
            model.PFrequencyListItems = frequency.AsEnumerable();
            model.MemberCategoryList = memberCategoryService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID).Select(s => new SelectionViewModel() { Code = s.MemberCategoryCode, Id = s.MemberCategoryID, DisplayName = string.Format("{0} - {1}", s.MemberCategoryCode, s.CategoryName), IsSelected = false }).ToList();

            var YearlyInterestPayable = new List<SelectListItem>();
            YearlyInterestPayable.Add(new SelectListItem() { Text = "NO", Value = "0" });
            YearlyInterestPayable.Add(new SelectListItem() { Text = "YES", Value = "1", Selected = true });
            model.YearlyInterestPayable_List = YearlyInterestPayable;

        }

        private void MapDropDownListCreate(ProductViewModel model)
        {
            var frequency = new List<SelectListItem>();
            frequency.Add(new SelectListItem() { Text = "Weekly", Value = "W", Selected = true });
            frequency.Add(new SelectListItem() { Text = "Monthly", Value = "M" });

            var calcList = new List<SelectListItem>();
            calcList.Add(new SelectListItem() { Text = "Flat", Value = "F" });
            calcList.Add(new SelectListItem() { Text = "Declined", Value = "D", Selected = true });
            calcList.Add(new SelectListItem() { Text = "Amortaization", Value = "A" });
            calcList.Add(new SelectListItem() { Text = "Amortaization-Fixed", Value = "E" });
            calcList.Add(new SelectListItem() { Text = "Abasan", Value = "H" });
            calcList.Add(new SelectListItem() { Text = "Variance", Value = "V" });
            var prodType = new List<SelectListItem>();
            prodType.Add(new SelectListItem() { Text = "Savings", Value = "0" });
            prodType.Add(new SelectListItem() { Text = "Loan", Value = "1", Selected = true });

            //var allinvestor = investorService.SearchInvestor();

            // var viewInvestor = allinvestor.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.InvestorCode, m.InvestorName), Value = m.InvestorID.ToString() });

            model.PInvestorListItems = prodType;
            model.PCalcuationMethodListItems = calcList.AsEnumerable();
            model.PFrequencyListItems = frequency.AsEnumerable();
            model.MemberCategoryList = memberCategoryService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID).Select(s => new SelectionViewModel() { Code = s.MemberCategoryCode, Id = s.MemberCategoryID, DisplayName = string.Format("{0} - {1}", s.MemberCategoryCode, s.CategoryName), IsSelected = false }).ToList();


            var YearlyInterestPayable = new List<SelectListItem>();
            YearlyInterestPayable.Add(new SelectListItem() { Text = "NO", Value = "0" });
            YearlyInterestPayable.Add(new SelectListItem() { Text = "YES", Value = "1", Selected = true });
            model.YearlyInterestPayable_List = YearlyInterestPayable;


        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductViewModel model)
        {
            try
            {
                // model.CreateDate = System.DateTime.Now;
                model.IsActive = true;
                //var selectedMemberCategory = model.MemberCategoryList.Where(w => w.IsSelected).ToList();

                var entity = Mapper.Map<ProductViewModel, Product>(model);
                //Add Validlation Logic.
                if (ModelState.IsValid)
                {
                    var errors = productService.IsValidProduct(entity);
                    if (errors.ToList().Count == 0)
                    {
                        //if (entity.GracePeriod > 31)
                        //{
                        //    return GetErrorMessageResult("Grace Period exceeds");
                        //}
                        entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                        productService.Create(entity);
                        return GetSuccessMessageResult();
                    }
                    else
                        return GetErrorMessageResult(errors);
                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            if (productService.IsContinued(id))
            {
                var product = productService.GetById(id);
                var entity = Mapper.Map<Product, ProductViewModelEditMode>(product);
             
            
                MapDropDownList(entity);
                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Discontinued Product, please enter a diferent product id and name.");
            return RedirectToAction("Index");
        }
        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(ProductViewModel model)
        {
            try
            {
                model.IsActive = true;
                var entity = Mapper.Map<ProductViewModel, Product>(model);
                var getproduct = productService.GetById(entity.ProductID);
                //// TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    getproduct.Duration = entity.Duration;
                    getproduct.InsuranceItemCode = entity.InsuranceItemCode;
                    getproduct.InsuranceItemRate = entity.InsuranceItemRate;
                    getproduct.InterestCalculationMethod = entity.InterestCalculationMethod;
                    getproduct.InterestInstallment = entity.InterestInstallment;
                    getproduct.InterestRate = entity.InterestRate;
                    getproduct.LoanInstallment = entity.LoanInstallment;
                    getproduct.MainItemName = entity.MainItemName;
                    getproduct.MainProductCode = entity.MainProductCode;
                    getproduct.MaxLimit = entity.MaxLimit;
                    getproduct.MinLimit = entity.MinLimit;
                    getproduct.PaymentFrequency = entity.PaymentFrequency;
                    getproduct.ProductCode = entity.ProductCode;
                    getproduct.ProductFullNameBng = entity.ProductFullNameBng;
                    getproduct.ProductFullNameEng = entity.ProductFullNameEng;
                    getproduct.ProductName = entity.ProductName;
                    getproduct.ProductShortNameBng = entity.ProductShortNameBng;
                    getproduct.ProductType = entity.ProductType;
                    getproduct.SavingsInstallment = entity.SavingsInstallment;
                    getproduct.GracePeriod = entity.GracePeriod;
                    getproduct.SubMainCategory = entity.SubMainCategory; //Khalid Added.
                    getproduct.LateFeePercentage = entity.LateFeePercentage;
                    getproduct.YearlyInterestPayable = entity.YearlyInterestPayable;
                    getproduct.NextDisbursementDuration = entity.NextDisbursementDuration;

                    productService.Update(getproduct);
                    return GetSuccessMessageResult();

                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            //var product = productService.GetById(id);
            //var entity = Mapper.Map<Product, ProductViewModel>(product);
            //return View(entity);
            productService.Inactivate(id, null);
            return RedirectToAction("Index");
        }
        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                productService.Inactivate(id, null);
                // TODO: Add delete logic here
               // UpdateMethod(id, System.DateTime.Now);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }       

    }
}

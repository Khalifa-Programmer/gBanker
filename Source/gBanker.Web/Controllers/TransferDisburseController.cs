using AutoMapper;
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
namespace gBanker.Web.Controllers
{
    public class TransferDisburseController : BaseController
    {
        #region Variables
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;        
        private readonly ICenterService centerService;
        private readonly IProductService productService;
        private readonly ILoanSummaryService loanSummaryService;
        private readonly IPurposeService purposeService;
        private readonly IAccReportService accReportService;
        public TransferDisburseController(IOfficeService officeService, IMemberService memberService, ICenterService centerService, IProductService productService, ILoanSummaryService loanSummaryService, IPurposeService purposeService, IAccReportService accReportService)
        {
            this.memberService = memberService;  
            this.officeService = officeService;
            this.centerService = centerService;
            this.productService = productService;
            this.loanSummaryService = loanSummaryService;
            this.purposeService = purposeService;
            this.accReportService = accReportService;
        }
        #endregion

        #region Methods
        private void MapDropDownList(TransferDisburseViewModel model)
        {
            var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OrgID==LoggedInOrganizationID).ToList();
            var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.officeList = ofc_items;

            //var gender_item = new List<SelectListItem>();
            //gender_item.Add(new SelectListItem() { Text = "Male", Value = "Male" });
            //gender_item.Add(new SelectListItem() { Text = "Female", Value = "Female" });
            //model.GenderList = gender_item;

           
            //switch (s.InvestorID) { case 11: { return true; } default: return false; }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetProductList(string mem_id)
        {
            var loanProdId = loanSummaryService.GetAll().Where(p => p.MemberID == Convert.ToInt64(mem_id) && p.OrgID==LoggedInOrganizationID);
            //var getProdByMemId = productService.GetAll().Where(p => p.ProductID == loanProdId);

            var viewProduct = loanProdId.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = productService.GetById(x.ProductID).ProductName
            });
            var prod_items = new List<SelectListItem>();
            if (viewProduct.ToList().Count > 0)
            {
                prod_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            prod_items.AddRange(viewProduct);
            return Json(prod_items, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetNewProductList(string prod_id)
        {
            //var loanProdId = loanSummaryService.GetAll().Where(p => p.MemberID == Convert.ToInt64(mem_id));
            var getNewProd = productService.GetAll().Where(p => p.ProductID != Convert.ToInt16(prod_id) && p.OrgID==LoggedInOrganizationID);
            var viewProduct = getNewProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductName
            });
            var cate_items = new List<SelectListItem>();
            if (viewProduct.ToList().Count > 0)
            {
                cate_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            cate_items.AddRange(viewProduct);
            return Json(cate_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLoanSummaryDetail(string mem_id,string prod_id)
        {
            var loan = loanSummaryService.GetByMemProdId(Convert.ToInt32(mem_id), Convert.ToInt32(prod_id));
            var result = new { LoanTerm = loan.LoanTerm, PurposeID = loan.PurposeID, PurposeName = string.Format("{0}, {1}", purposeService.GetById(loan.PurposeID).PurposeCode, purposeService.GetById(loan.PurposeID).PurposeName), PrincipalLoan = loan.PrincipalLoan - loan.LoanRepaid, IntCharge = loan.IntCharge - loan.IntPaid, Duration = loan.Duration, DisburseDate = Convert.ToDateTime(loan.DisburseDate).ToString("dd-MMM-yyyy"), LoanInstallment = loan.LoanInstallment, InstallmentStartDate = Convert.ToDateTime(loan.InstallmentStartDate).ToString("dd-MMM-yyyy"), IntInstallment = loan.IntInstallment };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CalcTransaction(string prod_id, string pr_loan)
        {
            var prod = productService.GetById(Convert.ToInt32(prod_id));
            //var loan = loanSummaryService.GetByMemProdId(Convert.ToInt32(mem_id), Convert.ToInt32(prod_id));
            //var result = new { LoanTerm = loan.LoanTerm, PurposeID = loan.PurposeID, PurposeName = string.Format("{0}, {1}", purposeService.GetById(loan.PurposeID).PurposeCode, purposeService.GetById(loan.PurposeID).PurposeName), PrincipalLoan = loan.PrincipalLoan - loan.LoanRepaid, IntCharge = loan.IntCharge - loan.IntPaid, Duration = loan.Duration, DisburseDate = Convert.ToDateTime(loan.DisburseDate).ToString("dd-MMM-yyyy"), LoanInstallment = loan.LoanInstallment, InstallmentStartDate = Convert.ToDateTime(loan.InstallmentStartDate).ToString("dd-MMM-yyyy"), IntInstallment = loan.IntInstallment };
            var result = new { LoanInstallment = prod.LoanInstallment * Convert.ToDecimal(pr_loan), IntInstallment = prod.InterestInstallment * Convert.ToDecimal(pr_loan) };
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTransferDisburse(string CenterID, string MemberID, string ProductID, string LoanTerm, string PurposeID, string DisburseDate, string PrincipalLoan, string LoanInstallment, string IntInstallment, string InstallmentStartDate, string Duration, string TransferProductID, string TransferInterest, string TransType)
        {
            var OfficeID = Convert.ToString(SessionHelper.LoginUserOfficeID);
            var param = new { OfficeID = OfficeID, CenterID = CenterID, MemberID = MemberID, ProductID = ProductID, LoanTerm = LoanTerm, PurposeID = PurposeID, DisburseDate = DisburseDate, PrincipalLoan = PrincipalLoan, LoanInstallment = LoanInstallment, IntInstallment = IntInstallment, InstallmentStartDate = InstallmentStartDate, Duration = Duration, TransferProductID = TransferProductID, TransferInterest = TransferInterest, TransType = TransType };
            int status = accReportService.SaveTransferDisburse(param);
            string Result = "";
            if (status > 0)
            {
                Result = "OK";
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Result = "CANCEL";
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Events
        // GET: TransferDisburse
        public ActionResult Index()
        {
            var model = new TransferDisburseViewModel();
            MapDropDownList(model);
            var blnk_items = new List<SelectListItem>();
            //blnk_items.Add(new SelectListItem() { Text = "", Value = "0", Selected = true });            
            model.centerList = blnk_items;
            model.productList = blnk_items;
            model.trProductList = blnk_items;
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            return View(model);
        }

        // GET: TransferDisburse/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TransferDisburse/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TransferDisburse/Create
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

        // GET: TransferDisburse/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TransferDisburse/Edit/5
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

        // GET: TransferDisburse/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TransferDisburse/Delete/5
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
        #endregion
    }
}

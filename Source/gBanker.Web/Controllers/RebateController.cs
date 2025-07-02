
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
using gBanker.Service.ReportExecutionService;

namespace gBanker.Web.Controllers
{
    public class RebateController : BaseController
    {
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

        public RebateController(ISpecialLoanCollectionService specialLoanCollectionService, IUltimateReportService ultimateReportService, ILoanSummaryService LoanSummaryService, ICenterService centerService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, IPurposeService purposeService, IMemberService memberService)
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

        private void MapDropDownList(RebateViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
          


       

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allpurpose = purposeService.SearchPurpose(Convert.ToInt32(LoggedInOrganizationID));

            var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });

            model.purposeListItems = viewPurpose;


            string vCoday = TransactionDay;

            //var allcenter = centerService.SearchSpecialCenter(vCoday, SessionHelper.LoginUserOfficeID.Value);
            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;




            var allSearchProd = productService.SearchProduct(0, Convert.ToInt32(LoggedInOrganizationID), "L");
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });

            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.productListItems = proditems;

            //model.MemberProductItemsSelected = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(allproduct);

            var allmembercategory = membercategoryService.GetAll().Where(a => a.OrgID == LoggedInOrganizationID);

            var viewmembercategory = allmembercategory.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCategoryID, m.CategoryName), Value = m.MemberCategoryID.ToString() });

            model.membercategoryListItems = viewmembercategory;



        }
        public JsonResult GetProductList(string Member_id, string center_id)
        {
            List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
            var param = new { OfficeId = LoginUserOfficeID, MemberId = Member_id, CenterId = center_id };
            var div_items = ultimateReportService.GetProductList(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new ProductViewModel
            {
                ProductID = row.Field<Int16>("ProductID"),
                ProductCode = row.Field<string>("ProductCode"),
                ProductName = row.Field<string>("ProductName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductCode.ToString() + " " + x.ProductName.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMemberList(string memberid, string centerId)
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
        public JsonResult GetMaxLoanTerm(string officeId, string centerId, string MemId, string ProdId)
        {
            int vLoanTerm;
            var model = new RebateViewModel();
            model.OfficeID = Convert.ToInt16(officeId);
            model.CenterID = Convert.ToInt32(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);


            var entity = Mapper.Map<RebateViewModel, LoanSummary>(model);
            entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
            var mlt = specialLoanCollectionService.Get_MaxLoanTerm(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), entity.CenterID, entity.MemberID, entity.ProductID).FirstOrDefault();
            vLoanTerm = Convert.ToInt16(mlt.LoanTerm);

            var result = new { LoanTerm = vLoanTerm.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInstallment(string officeId, string centerId, string MemId, int productid, int loanTerm)
        {
            decimal vLoanInstallment = 0;
            decimal vInterestInstallment = 0;
            decimal vTotalIns = 0;
            string vInterestCalcMethod = string.Empty, vPaymentFreq = string.Empty;
            decimal vLoanDue = 0;
            decimal vInterestDue = 0;
            decimal vPrincipalLoan = 0;
            decimal vLoanRepaid = 0;
            Int64 vDailyLoanTrxID = 0;
            int vproductID = 0;
            decimal vcumIntcharge = 0;
            decimal vcumIntPaid = 0;
            decimal vcumIntDue = 0;
            decimal vLoanBalance = 0;
            decimal vIntBalance = 0;
            decimal vNewRebate = 0;
            decimal vPreIntPaid = 0;
            decimal vIntColection = 0;
            decimal vLOanBalIntColection = 0;
            decimal totalRebate = 0;
            int vDUration = 0;
            decimal durationOverLoanDue = 0;
            decimal duartionOverIntDue = 0;
            Int16 NoOfInstallment = 0;


            var param1 = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = Convert.ToInt32(centerId), MemberID = Convert.ToInt64(MemId), ProductID = productid, LoanTerm = loanTerm };
                var LoanInstallMent = ultimateReportService.GetRebateInfo(param1);



            // var LoanInstallMent = specialLoanCollectionService.GetAll().Where(l => l.OrgID == LoggedInOrganizationID && l.OfficeID == Convert.ToInt16(officeId) && l.CenterID == Convert.ToInt16(centerId) && l.MemberID == Convert.ToInt64(MemId) && l.ProductID == productid && l.LoanTerm == vlOanTerm  && l.IsActive == true && l.TrxType==trxType).FirstOrDefault();
            if (LoanInstallMent != null)
            {

                vPrincipalLoan = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["PrincipalLoan"].ToString());
                vLoanRepaid = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["LoanRepaid"].ToString());
                vLoanBalance = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["LoanBalance"].ToString());
                vcumIntcharge = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["IntCharge"].ToString());
                vPreIntPaid= Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["PrevIntPaid"].ToString());
                vIntBalance = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["IntBalance"].ToString());
                vcumIntDue = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["CumIntDue"].ToString());
               
                
                vNewRebate = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["NewRebate"].ToString());
                totalRebate= Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["TotalRebate"].ToString());
                vIntColection = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["IntCollection"].ToString());
                vLOanBalIntColection = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["LoanBalance"].ToString()) + Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["IntCollection"].ToString());

            }
            else
            {
                vPrincipalLoan = 0;
                vLoanRepaid = 0;
                vcumIntcharge = 0;
                vcumIntDue = 0;
                vLoanBalance = 0;
                vIntBalance = 0;
                vNewRebate = 0;
                totalRebate = 0;
                vLOanBalIntColection = 0;
              }

            var result = new { PrincipalLoan = vPrincipalLoan, LoanRepaid = vLoanRepaid, LoanBalance = vLoanBalance, CumIntCharge =vcumIntcharge, PrevIntPaid=vPreIntPaid, IntBalance = vIntBalance, CumIntDue =vcumIntDue,   NewRebate =vNewRebate, TotalRebate = totalRebate, IntCollection=vIntColection, LOanBalIntColection=vLOanBalIntColection };
            return Json(result, JsonRequestBehavior.AllowGet);
         
        }
        public ActionResult GetSpecialCollection(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {


                string vday = string.Empty;
                if (IsDayInitiated)
                    vday = TransactionDay;
                var specialLoandetail = specialLoanCollectionService.GetRebateDetails(LoggedInOrganizationID, LoginUserOfficeID, vday, filterColumn, filterValue);
                var detail = specialLoandetail.ToList();



                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<proc_get_SpecialLoanCollection_Result>, IEnumerable<RebateViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        // GET: Rebate
        public ActionResult Index()
        {
            return View();
        }

        // GET: Rebate/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Rebate/Create
        public ActionResult Create()
        {
            var model = new RebateViewModel();

            if (IsDayInitiated)
                model.TrxDate = TransactionDate;
            MapDropDownList(model);
           
            return View(model);
        }
        public ActionResult RebateView(string CenterID, string MemberID, string ProductID, string LoanTerm)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
              
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = LoginUserOfficeID.ToString() });
                paramValues.Add(new ParameterValue() { Name = "CenterID", Value = CenterID.ToString() });
                paramValues.Add(new ParameterValue() { Name = "MemberID", Value = MemberID.ToString() });
                paramValues.Add(new ParameterValue() { Name = "ProductID", Value = ProductID.ToString() });
                paramValues.Add(new ParameterValue() { Name = "LoanTerm", Value = LoanTerm.ToString() });
                PrintSSRSReport("/gBanker_Reports/RPT_Member_Rebate_Info", paramValues.ToArray(), "gBankerDbContext");
                
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: Rebate/Create
        [HttpPost]
        public ActionResult Create(RebateViewModel Model)
        {
            try
            {
                long vlOansummaryID;
                long vDailyTrxID;
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }

                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);


                var entity = Mapper.Map<RebateViewModel, DailyLoanTrx>(Model);

                if (ModelState.IsValid)
                {




                    var param1 = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = Convert.ToInt32(Model.CenterID), MemberID = Convert.ToInt64(Model.MemberID), ProductID = Convert.ToInt16(Model.ProductID), LoanTerm = Convert.ToInt16(Model.LoanTerm), LoanBalance=Convert.ToDecimal(Model.LoanBalance), NewRebate = Convert.ToDecimal(Model.NewRebate), TotalRebate = Convert.ToDecimal(Model.TotalRebate), IntCollectoin= Convert.ToDecimal(Model.IntCollection), lcl_BusinessDate=Convert.ToDateTime(TransactionDate), CreateUser=LoggedInEmployeeID, Qtype =1};
                    ultimateReportService.SetRebateInfo(param1);

                    //specialLoanCollectionService.UpdateSpecialLOan(loanser.DailyLoanTrxID, loanser.OfficeID, loanser.CenterID, loanser.MemberID, loanser.ProductID, loanser.LoanTerm, Model.LoanPaid, Model.IntPaid, Model.TotalPaid, Model.TrxType, loanser.OrgID);
                    return GetSuccessMessageResult();

                }
                return GetErrorMessageResult();



            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: Rebate/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Rebate/Edit/5
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

        // GET: Rebate/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Rebate/Delete/5
        [HttpPost]
        public ActionResult Delete(int DailyLoanTrxID, RebateViewModel model)
        {
            try
            {
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                var sp = specialLoanCollectionService.GetByIdLong(DailyLoanTrxID);
                var entity = Mapper.Map<DailyLoanTrx, RebateViewModel>(sp);
                specialLoanCollectionService.Delete(DailyLoanTrxID);


                var param1 = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = Convert.ToInt32(entity.CenterID), MemberID = Convert.ToInt64(entity.MemberID), ProductID = Convert.ToInt16(entity.ProductID), LoanTerm = Convert.ToInt16(entity.LoanTerm), LoanBalance = Convert.ToDecimal(entity.LoanBalance), NewRebate = Convert.ToDecimal(entity.NewRebate), TotalRebate = Convert.ToDecimal(entity.TotalRebate), IntCollectoin = Convert.ToDecimal(entity.IntCollection), lcl_BusinessDate = Convert.ToDateTime(TransactionDate), CreateUser = LoggedInEmployeeID, Qtype = 2 };

                ultimateReportService.SetRebateInfo(param1);


                // TODO: Add delete logic here
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}

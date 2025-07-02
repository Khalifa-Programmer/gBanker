using AutoMapper;
using gBanker.Data.CodeFirstMigration;
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
using System.Data;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
namespace gBanker.Web.Controllers
{
    public class CategoryTransferController : BaseController
    {
        private readonly ICenterService centerService;
        private readonly ICategoryTransferService categorytransferService;
        private readonly IBranchService branchService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;
        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        private readonly ISavingSummaryService savingSummaryService;
        private readonly IUltimateReportService ultimateReportService;
        public CategoryTransferController(ICategoryTransferService categorytransferService, IUltimateReportService ultimateReportService, ISavingSummaryService savingSummaryService, ICenterService centerService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, IMemberService memberService)
        {
            this.categorytransferService = categorytransferService;
            this.centerService = centerService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.officeService = officeService;
            this.memberService = memberService;
            this.savingSummaryService = savingSummaryService;
            this.ultimateReportService = ultimateReportService;
          
        }
        public ActionResult GetMemberList(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                var mbr = memberService.GetByCenterId(int.Parse(centerId),Convert.ToInt16(LoginUserOfficeID),Convert.ToInt16(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        private void MapDropDownList(CategoryTransferViewModel model)
        {

            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            //var allmember = memberService.GetAll().Where(w => w.IsActive == true && w.MemberStatus == "1" && w.OfficeID == LoginUserOfficeID);

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

 
            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

           

            // var allcenter = centerService.SearchSpecialCenter(vCoday, SessionHelper.LoginUserOfficeID.Value);
            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID));

            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.OrgID==LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;




            //var allSearchProd = productService.SearchAllProduct(Convert.ToInt16(LoggedInOrganizationID));
            var allSearchProd = productService.CategoryProduct(0,Convert.ToInt16(LoggedInOrganizationID));
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            proditems.AddRange(viewProdList);
            model.productListItems = proditems;


            var allmembercategory = membercategoryService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsActive == true).OrderBy(m => m.MemberCategoryCode);

            var viewmembercategory = allmembercategory.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MemberCategoryID.ToString(),
                Text = string.Format("{0} - {1}", x.MemberCategoryCode.ToString(), x.CategoryName.ToString())
            }); ;

            var categoryListItems = new List<SelectListItem>();
            categoryListItems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            categoryListItems.AddRange(viewmembercategory);
            model.membercategoryListItems = categoryListItems;


            //var allmembercategory = membercategoryService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsActive == true).OrderBy(m => m.MemberCategoryCode);

            //var viewmembercategory = allmembercategory.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCategoryCode, m.CategoryName), Value = m.MemberCategoryID.ToString() });

            //model.membercategoryListItems = viewmembercategory;
        }
        public ActionResult GetSavBalance(string officeId, string centerId, string MemId, string productid)
        {
            decimal vproncipal;
            var model = new CategoryTransferViewModel();
            model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
            model.CenterID = Convert.ToInt32(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(productid);


            var entity = Mapper.Map<CategoryTransferViewModel, getTransferHistory_Result>(model);
            //var mlt = savingSummaryService.GetAll().Where(s => s.OrgID==LoggedInOrganizationID && s.OfficeID == LoginUserOfficeID && s.CenterID == model.CenterID && s.MemberID == model.MemberID && s.ProductID == model.ProductID && s.IsActive==true && s.SavingStatus==1);
            //if (mlt != null)
            //{
            //    var vDeposit = mlt.Sum(m => m.Deposit);
            //    var vCumInterest = mlt.Sum(m => m.CumInterest);
            //    var vWithDrawal = mlt.Sum(m => m.Withdrawal);
            //    vproncipal = (vDeposit + vCumInterest - vWithDrawal);
            //}
            //else
            //    vproncipal = 0;
            var mlt = savingSummaryService.GetSavingBalanceForCate(LoginUserOfficeID, entity.CenterID, entity.MemberID, entity.ProductID).FirstOrDefault();
            vproncipal = Convert.ToDecimal(mlt.Balance);
            


            var result = new { Principal = vproncipal.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]

        //public JsonResult GetProductList(string mem_id)
        //{
        //    var loanProdId = savingSummaryService.GetAll().Where(p => p.MemberID == Convert.ToInt64(mem_id) && p.IsActive == true && p.OrgID == LoggedInOrganizationID && p.OfficeID == LoginUserOfficeID);
        //    //var getProdByMemId = productService.GetAll().Where(p => p.ProductID == loanProdId);

        //    var viewProduct = loanProdId.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.ProductID.ToString(),
        //        Text = productService.GetById(x.ProductID).ProductCode+" - "+productService.GetById(x.ProductID).ProductName
        //    });
        //    var prod_items = new List<SelectListItem>();
        //    if (viewProduct.ToList().Count > 0)
        //    {
        //        prod_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
        //    }
        //    prod_items.AddRange(viewProduct);
        //    return Json(prod_items, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetProductList(string Member_id, string center_id)
        {
            List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
            var param = new { OfficeId = LoginUserOfficeID, MemberId = Member_id, CenterId = center_id };
            var div_items = ultimateReportService.GetProductListFromSavingSummary(param);

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
        public JsonResult GetTrhfMemberCategoryID(string TrMemberCategoryID)
        {
            List<MemberCategoryViewModel> List_ProductViewModel = new List<MemberCategoryViewModel>();
            var param = new { TrMemberCategoryID = TrMemberCategoryID };
            var div_items = ultimateReportService.getCategoryByCategoryID(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new MemberCategoryViewModel
            {
                ProductCategoryID = row.Field<Int16>("ProductCategoryID"),
                MemberCategoryCode = row.Field<string>("MemberCategoryCode"),
                CategoryName = row.Field<string>("CategoryName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductCategoryID.ToString(),
                Text = x.MemberCategoryCode.ToString() + " " + x.CategoryName.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetNewcategoryList(string Member_id, string center_id)
        {
            List<MemberCategoryViewModel> List_ProductViewModel = new List<MemberCategoryViewModel>();
            var param = new { OfficeId = LoginUserOfficeID, MemberId = Member_id, CenterId = center_id };
            var div_items = ultimateReportService.GetNewcategoryList(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new MemberCategoryViewModel
            {
                MemberCategoryID = row.Field<byte>("MemberCategoryID"),
                MemberCategoryCode = row.Field<string>("MemberCategoryCode"),
                CategoryName = row.Field<string>("CategoryName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MemberCategoryID.ToString(),
                Text = x.MemberCategoryCode.ToString() + " " + x.CategoryName.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetNewcategoryList(string mem_id)
        //{
        //    //var loanProdId = savingSummaryService.GetAll().Where(p => p.MemberID == Convert.ToInt64(mem_id) && p.OfficeID==LoginUserOfficeID && p.IsActive == true && p.OrgID==LoggedInOrganizationID);

        //    var loanProdId = memberService.GetAll().Where(p => p.MemberID == Convert.ToInt64(mem_id) && p.OfficeID == LoginUserOfficeID && p.IsActive == true && p.OrgID == LoggedInOrganizationID && p.MemberStatus == Convert.ToString(1));

        //    var viewProduct = loanProdId.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.MemberCategoryID.ToString(),
        //        Text = membercategoryService.GetById(x.MemberCategoryID).MemberCategoryCode + " - " + membercategoryService.GetById(x.MemberCategoryID).CategoryName
        //    });
        //    var cate_items = new List<SelectListItem>();
        //    if (viewProduct.ToList().Count > 0)
        //    {
        //        cate_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
        //    }
        //    cate_items.AddRange(viewProduct);
        //    return Json(cate_items, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetCategoryTransfer(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                var alltransfer = categorytransferService.GetTransferDetail(Convert.ToInt16(LoggedInOrganizationID),LoginUserOfficeID);
                var detail = alltransfer.ToList();
                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<getTransferHistory_Result>, IEnumerable<CategoryTransferViewModel>>(entities);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
                //return Json(new { Result = "OK",  TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public ActionResult DeleteProcess(string officeId)
        {
            try
            {
                var param = new { @OfficeId = SessionHelper.LoginUserOfficeID };
                var allProducts = ultimateReportService.DeleteProcessCheck(param);
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        // GET: CategoryTransfer
        public ActionResult Index()
        {
            return View();
        }
        // GET: CategoryTransfer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: CategoryTransfer/Create
        public ActionResult Create()
        {
            var model = new CategoryTransferViewModel();
            if (IsDayInitiated)
            {
                model.TransferDate = TransactionDate;
            }


            MapDropDownList(model);
            var blnk_items = new List<SelectListItem>();
            model.productList = blnk_items;
            model.categoryList = blnk_items;
            return View(model);
        }
        // POST: CategoryTransfer/Create
        [HttpPost]
        public ActionResult Create(CategoryTransferViewModel model)
        {
            try
            {

                var entity = Mapper.Map<CategoryTransferViewModel, TransferHistory>(model);

                //Add Validlation Logic.
                if (ModelState.IsValid)
                {
             

                    var param = new { orgID = SessionHelper.LoginUserOrganizationID, OfficeID = LoginUserOfficeID, CenterID = entity.CenterID, MemberID = entity.MemberID, MemberCategoryID = entity.MemberCategoryId, ProductID = entity.ProductID, SavBalance = entity.Principal, NewMemberCategoryID = entity.TrMemberCategoryID, Transdate = System.DateTime.Now,NewProductID=entity.TrProductID };
                    ultimateReportService.setCateGoryTransfer(param);

                    var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", entity.CenterID);
                    Session.Remove(MemberByCenterSessionKey);
                    //categorytransferService.CateGoryTransfer(LoggedInOrganizationID,LoginUserOfficeID,entity.CenterID,entity.MemberID,entity.MemberCategoryId,entity.ProductID,entity.Principal,entity.TrMemberCategoryID,System.DateTime.Now,entity.TrProductID);
                    //var emtpy = new CategoryTransferViewModel();
                    //MapDropDownList(emtpy);
                    //return RedirectToAction("Index");
                    return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);

                }

                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }

            catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: CategoryTransfer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: CategoryTransfer/Edit/5
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
        // GET: CategoryTransfer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: CategoryTransfer/Delete/5
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

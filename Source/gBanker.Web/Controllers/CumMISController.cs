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

namespace gBanker.Web.Controllers
{
    public class CumMISController :  BaseController
    {
        private readonly ICumMISService CumMISService;
        private readonly IOfficeService officeService;
        private readonly ICenterService centerService;
        private readonly IProductService productService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IInvestorService investorService;
        private readonly ICumMisItemService CumMisItemService;
        public CumMISController(IOfficeService officeService, ICumMISService CumMISService, ICenterService centerService, IProductService productService, IUltimateReportService ultimateReportService,IInvestorService investorService, ICumMisItemService CumMisItemService)
        {
            this.centerService = centerService;
            this.officeService = officeService;
            this.CumMISService = CumMISService;
            this.centerService = centerService;
            this.productService = productService;
            this.ultimateReportService = ultimateReportService;
            this.investorService = investorService;
            this.CumMisItemService = CumMisItemService;
        }

        private void MapDropDownList(CumMISViewModel model)
        {
            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OrgID == LoggedInOrganizationID && m.OfficeID==LoginUserOfficeID).ToList();
            var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.OfficeList = ofc_items;

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


           var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;


            var allSearchProd = productService.SearchProduct(1, Convert.ToInt16(LoggedInOrganizationID), "");
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            proditems.AddRange(viewProdList);
            model.productListItems = proditems;

            var allInvestor = investorService.GetAll().Where(i => i.IsActive == true && i.OrgID == LoggedInOrganizationID).OrderBy(i => i.InvestorCode);

            var viewInvestor = allInvestor.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.InvestorCode, m.InvestorName), Value = m.InvestorID.ToString() });

            model.investorListItems = viewInvestor;

            var gender_item = new List<SelectListItem>();
            gender_item.Add(new SelectListItem() { Text = "Male", Value = "M" });
            gender_item.Add(new SelectListItem() { Text = "Female", Value = "F", Selected = true });
            model.GenderList = gender_item;


            var allSearchMisItem = CumMisItemService.GetAll();
            var viewCumMisList = allSearchMisItem.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CumMisItemID.ToString(),
                Text = string.Format("{0}", x.CumMisItemName.ToString())
            });
            var cumMisitems = new List<SelectListItem>();
            cumMisitems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            cumMisitems.AddRange(viewCumMisList);
            model.CumMisItemList = cumMisitems;



        }
        public ActionResult GetCumMISInfo(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                
                    var getCum = CumMISService.GetCumMISInfo(LoginUserOfficeID, filterColumn, filterValue);
                    var detail = getCum.ToList();
                    var totalCount = detail.Count();
                    var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                    var currentPageRecords = Mapper.Map<IEnumerable<Proc_Get_CUMMIS_Result>, IEnumerable<CumMISViewModel>>(entities);

                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
                
               

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        //
        // GET: /CumMIS/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /CumMIS/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /CumMIS/Create
        public ActionResult Create()
        {
            var model = new CumMISViewModel();
            if (IsDayInitiated)
                model.MisDate = TransactionDate;

            MapDropDownList(model);

            return View(model);
        }
        public Product GetProduct(int productid)
        {
            var mbr = productService.GetById(productid);
            return mbr;
        }
        //
        // POST: /CumMIS/Create
        [HttpPost]
        public ActionResult Create(CumMISViewModel model, FormCollection form)
        {
            try
            {
               var entity = Mapper.Map<CumMISViewModel, CumMi>(model);
                //Add Validlation Logic.
                if (ModelState.IsValid)
                {


                    entity.OfficeID =Convert.ToInt16( LoginUserOfficeID);
                        CumMISService.Create(entity);
                        return GetSuccessMessageResult();
                   
                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        //
        // GET: /CumMIS/Edit/5
        public ActionResult Edit(int id)
        {
            var product = CumMISService.GetById(id);
            var entity = Mapper.Map<CumMi, CumMISViewModel>(product);


            MapDropDownList(entity);
            return View(entity);
        }

        //
        // POST: /CumMIS/Edit/5
        [HttpPost]
        public ActionResult Edit(CumMISViewModel model)
        {
            try
            {
               
                var entity = Mapper.Map<CumMISViewModel, CumMi>(model);
                var getproduct = CumMISService.GetById(entity.CumMisID);
                //// TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    getproduct.CenterID = entity.CenterID;
                    getproduct.ProductID = entity.ProductID;
                    getproduct.NoOfLoanee = entity.NoOfLoanee;
                    getproduct.UpToLoanDis = entity.UpToLoanDis;
                    getproduct.UptoDisburseMent = entity.UptoDisburseMent;
                    getproduct.UpToRecovery = entity.UpToRecovery;
                    getproduct.UptoAdmission = entity.UptoAdmission;
                    getproduct.UpToDropOut = entity.UpToDropOut;
                    getproduct.UptoFullyRepaid = entity.UptoFullyRepaid;
                    getproduct.UptoDeposit = entity.UptoDeposit;
                    getproduct.UptoInterest = entity.UptoInterest;
                    getproduct.WriteOffLoan = entity.WriteOffLoan;
                    getproduct.WriteOffInterest = entity.WriteOffInterest;
                    getproduct.uptowithdrawal = entity.uptowithdrawal;
                    getproduct.CumMisItemID = entity.CumMisItemID;
                    CumMISService.Update(getproduct);
                    return GetSuccessMessageResult();

                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        //
        // GET: /CumMIS/Delete/5
        public ActionResult Delete(int id)
        {
            CumMISService.Delete(id);
            return RedirectToAction("Index");
        }

        //
        // POST: /CumMIS/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                CumMISService.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

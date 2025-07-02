using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using gBanker.Data.CodeFirstMigration;
using gBanker.Web.ViewModels;
using System.Text;

namespace gBanker.Web.Controllers
{
    public class AccountCodeMappingController : BaseController
    {
        #region Variables

        private readonly IEmployeeService employeeService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        public AccountCodeMappingController(IEmployeeService employeeService, IOfficeService officeService, IUltimateReportService ultimateReportService)
        {
            this.employeeService = employeeService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
        }
        #endregion Variables

        // GET: AccountCodeMapping
        public ActionResult AccountCodeProductMapping()
        { 
            IEnumerable<SelectListItem> item = new SelectList(" ");
            ViewData["AccountingInterfaceMaster"] = item;

            return View();
        }

        public ActionResult GetActiveProductListAuto(string productCode )
        {
            string OrgId = SessionHelper.LoginUserOfficeID.ToString();
            var ProductListSessionKey_Active = string.Format("ProductList_{0}", OrgId);
            var productList = new List<Product>();
            if (Session[ProductListSessionKey_Active] != null)
                productList = Session[ProductListSessionKey_Active] as List<Product>;
            else
            {
                List<Product> List_Product = new List<Product>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = 0 };
                var alldata = ultimateReportService.GetDataWithParameter(param, "GetProductListActiveAutoComplete");
                
                List_Product = alldata.Tables[0].AsEnumerable()
                .Select(row => new Product
                {
                    ProductID = row.Field<Int16>("ProductId"),
                    ProductCode = row.Field<string>("ProductCode"),
                    ProductName = row.Field<string>("ProductName"),
                    ProductFullNameEng = row.Field<string>("ProductFullNameEng"),
                    ProductType = row.Field<byte?>("ProductType")

                }).ToList();

                Session[ProductListSessionKey_Active] = List_Product; // mbr;
                productList = List_Product; // mbr;
            }
            var products = productList.Where(m => string.Format("{0} - {1}", m.ProductCode, (string.IsNullOrEmpty(m.ProductName) ? "" : m.ProductName) + ' ' + (string.IsNullOrEmpty(m.ProductFullNameEng) ? "" : m.ProductFullNameEng)).ToLower().Contains(productCode.ToLower())).Select(m1 => new { ProductID = m1.ProductID.ToString() + "-" + m1.ProductType.ToString(), ProductName = string.Format("{0} - {1}", m1.ProductCode, (string.IsNullOrEmpty(m1.ProductName) ? "" : m1.ProductName), m1.ProductType)}).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAccCodeListAuto(string AccCode)
        {
            string OrgId = SessionHelper.LoginUserOfficeID.ToString();
            var AccListSessionKey_Active = string.Format("AccList_{0}", OrgId);
            var productList = new List<AccChart>();
            if (Session[AccListSessionKey_Active] != null)
                productList = Session[AccListSessionKey_Active] as List<AccChart>;
            else
            {
                List<AccChart> List_Product = new List<AccChart>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = 0 };
                var alldata = ultimateReportService.GetDataWithParameter(param, "GetAccListActiveAutoComplete");

                List_Product = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccChart
                {
                    AccID = row.Field<int>("AccID"),
                    AccCode = row.Field<string>("AccCode"),
                    AccName = row.Field<string>("AccName")
                }).ToList();

                Session[AccListSessionKey_Active] = List_Product; // mbr;
                productList = List_Product; // mbr;
            }
            var products = productList.Where(m => string.Format("{0} - {1}", m.AccCode, (string.IsNullOrEmpty(m.AccName) ? "" : m.AccName)).ToLower().Contains(AccCode.ToLower())).Select(m1 => new { m1.AccCode, AccName = string.Format("{0} - {1}", m1.AccCode, (string.IsNullOrEmpty(m1.AccName) ? "" : m1.AccName)) }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTrxIndList(byte? productType)
        {
            try
            {
                string OrgId = SessionHelper.LoginUserOfficeID.ToString();
                var TrxSessionKey_Active = string.Format("TrxList_{0}", OrgId);
                var trxList = new List<AccountingInterfaceMasterViewModel>();
                if (Session[TrxSessionKey_Active] != null)
                    trxList = Session[TrxSessionKey_Active] as List<AccountingInterfaceMasterViewModel>;
                else
                {
                    List<AccountingInterfaceMasterViewModel> List = new List<AccountingInterfaceMasterViewModel>();

                    var tList = ultimateReportService.GetDataWithoutParameter("GETtrx_List");

                    List = tList.Tables[0].AsEnumerable()
                   .Select(row => new AccountingInterfaceMasterViewModel
                   {
                       trx_ind = row.Field<string>("trx_ind"),
                       trx_type = row.Field<string>("trx_type"),
                       trx_ind_FullName = row.Field<string>("trx_ind_FullName"),
                       ProductType = row.Field<byte?>("ProductType"),

                   }).ToList();

                    Session[TrxSessionKey_Active] = List; // mbr;
                }

                trxList = Session[TrxSessionKey_Active] as List<AccountingInterfaceMasterViewModel>;

                var mList = trxList.Where(m => m.ProductType == productType);
                var viewUni = mList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.trx_ind.ToString(),
                    Text = x.trx_ind_FullName.ToString() + " " + x.trx_type.ToString()
                });
                var uni_items = new List<SelectListItem>();
                uni_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
                uni_items.AddRange(viewUni);
                return Json(uni_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTrxIndProductMappingList(string ProductID, string AccCode, string trx_ind, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                

                List<AccountingInterfaceMasterViewModel> List_ViewModel = new List<AccountingInterfaceMasterViewModel>();
                var param = new { @ProductID = ProductID, @AccCode = 0, @trx_ind = 0, @OrgID = LoggedInOrganizationID };
                var empList = ultimateReportService.GetDataWithParameter(param, "SP_GETInterfaceData");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new AccountingInterfaceMasterViewModel
                {
                    AccountingInterfaceID = row.Field<int>("AccountingInterfaceID"),
                    rowSl = row.Field<Int64>("rowSl"),
                    ProductName = row.Field<string>("ProductName"),
                    AccCode = row.Field<string>("AccCode"),
                    voucher_category = row.Field<string>("voucher_category"),
                    voucher_type = row.Field<string>("voucher_type") ,
                    trx_ind_FullName = row.Field<string>("trx_ind_FullName")

                }).ToList();

 
                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function


        public JsonResult Delete(string Id)
        {
            try {
                string Result = "Mapping Deleted Successfully.";

                var param = new { AccountingInterfaceID = Id, @UpdateUser = LoggedInEmployeeID, @UpdateDate = DateTime.Now };
                var empList = ultimateReportService.GetDataWithParameter(param, "DeleteAccountingInterface");

                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }

        }

        public JsonResult Create(string TrxInd, string ProductID, string AccCode) 
        {
            try
            {
                string Result = "Mapping Create successfully.";
                var param = new { @ProductID = ProductID, @AccCode = AccCode, @trx_ind = TrxInd, @OrgID = LoggedInOrganizationID, @CreateUser = LoggedInEmployeeID, @CreateDate = DateTime.Now };
                var empList = ultimateReportService.GetDataWithParameter(param, "CreateAccountingInterface");

             
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }

        }



    }// END oc Class
}// END Namespace
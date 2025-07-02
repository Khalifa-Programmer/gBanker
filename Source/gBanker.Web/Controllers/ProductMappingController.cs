using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace gBanker.Web.Controllers
{
    public class ProductMappingController : BaseController
    {
        #region Variables
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IProductService productService;


        public ProductMappingController(IOfficeService officeService,  IProductService productService,  IUltimateReportService ultimateReportService)
        {
            this.officeService = officeService;
            this.productService = productService;
            this.ultimateReportService = ultimateReportService;
        }
#endregion

        // GET: ProductMapping
        public ActionResult Set()
        {
            ProdAccMappingViewModel model = new ProdAccMappingViewModel();

            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            //var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OfficeID == offc_id && m.OrgID == LoggedInOrganizationID).ToList();
            var allOffice = officeService.GetAll();
            // var allOffice = officeService.GetById(offc_id);
            var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            //var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = string.Format("{0}, {1}", x.OfficeCode.ToString(), x.OfficeName.ToString())
            });
            var ofc_items = new List<SelectListItem>();
            ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            ofc_items.AddRange(viewOffice);
            model.OfficeList = ofc_items;


            return View(model);
        }

        #region ProductMappingEdit


        public ActionResult NoteFourProductMapping()
        {
            ProdAccMappingViewModel model = new ProdAccMappingViewModel();

            List<ProdAccMappingViewModel> List_ViewModel = new List<ProdAccMappingViewModel>();

            var param = new { AndCondition = "" };
            var empList = ultimateReportService.GetDataWithParameter(param, "SP_Get_Product_List");

            List_ViewModel = empList.Tables[0].AsEnumerable()
            .Select(row => new ProdAccMappingViewModel
            {
                ProductCode = row.Field<string>("ProductCode"),
                ProductName = row.Field<string>("ProductName")

            }).ToList();

            var viewProduct = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductCode.ToString(),
                Text = string.Format("{0}, {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });

            var prod_items = new List<SelectListItem>();
            prod_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            prod_items.AddRange(viewProduct);
            model.ProductList = prod_items;


            //////// Component
            ///

            ProdAccMappingViewModel componentmodel = new ProdAccMappingViewModel();

            List<ProdAccMappingViewModel> List_ViewModelcomponent = new List<ProdAccMappingViewModel>();

            var param2 = new { AndCondition = "" };
            var empList2 = ultimateReportService.GetDataWithParameter(param, "SP_Get_ProductComponent_List");

            List_ViewModelcomponent = empList2.Tables[0].AsEnumerable()
            .Select(row => new ProdAccMappingViewModel
            {
                ProductCodeComponent = row.Field<string>("ProductCode"),
                ProductNameComponent = row.Field<string>("ProductName")

            }).ToList();

            var viewProductcomponent = List_ViewModelcomponent.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductCodeComponent.ToString(),
                Text = string.Format("{0}, {1}", x.ProductCodeComponent.ToString(), x.ProductNameComponent.ToString())
            });

            var prod_items_component = new List<SelectListItem>();
            prod_items_component.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            prod_items_component.AddRange(viewProductcomponent);
            model.ProductListComponent = prod_items_component;

            return View(model);
        }

        public JsonResult CreateMapping(string productcode, string ProductCodeComponent, string hdnMappingId = "")
        {
            string result = "OK";
            try
            {
                //Check If Same Work area Name
                List<ProdAccMappingViewModel> List_ViewModel = new List<ProdAccMappingViewModel>();
                var param2 = new { productcode = productcode, ProductCodeComponent = ProductCodeComponent };
                var empList = ultimateReportService.GetDataWithParameter(param2, "SP_PR_Get_Mapping_ByCodes");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new ProdAccMappingViewModel
                {
                    ProductCode = row.Field<string>("productcode")
                }).ToList();

                if (List_ViewModel.Count > 0)
                {
                    Response.StatusCode = 403;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                //End of Check

                if (hdnMappingId != "")
                {
                    //Update
                    var param = new { productcode = productcode, ProductCodeComponent = ProductCodeComponent, CreateUser = LoggedInEmployee.EmployeeID, hdnMappingId = hdnMappingId };
                    var val = ultimateReportService.GetDataWithParameter(param, "SP_PR_Update_Mapping");

                }
                else
                {
                    //Save
                    var param = new { productcode = productcode, ProductCodeComponent = ProductCodeComponent, CreateUser = LoggedInEmployee.EmployeeID };
                    var val = ultimateReportService.GetDataWithParameter(param, "SP_PR_CREATE_Mapping");
                }



            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Delete Area
        public JsonResult DeleteMap(int MapId)
        {
            string result = "OK";
            try
            {
                var param2 = new { MapId = MapId, UpdateUser = LoggedInEmployee.EmployeeID };
                var val = ultimateReportService.GetDataWithParameter(param2, "SP_PR_Delete_Map");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        // Show List
        public JsonResult GetMappingList(string MappingId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string MappingIdIds = Convert.ToString(MappingId);

                if (MappingId != null) //"0"
                    sb.Append(" AND  Id =" + MappingIdIds);

                List<ProdAccMappingViewModel> List_ViewModel = new List<ProdAccMappingViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = ultimateReportService.GetDataWithParameter(param, "SP_Get_Mapping_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new ProdAccMappingViewModel
                {
                    rowSl = row.Field<Int64>("rowSl"),
                    MappingId = row.Field<int>("MappingId"),
                    ProductCode = row.Field<string>("ProductCode"),
                    ProductCodeComponent = row.Field<string>("ProductCodeComponent"),
                    ProductName = row.Field<string>("ProductName"),
                    LoanComponent = row.Field<string>("LoanComponent")

                }).ToList();

                if (MappingId != null)
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





        #endregion


        #region Methods
        public JsonResult GetAvailableProductList(string OfficeID)
        {
            try
            {
                List<ProductViewModel> List_ProductInfoViewModel = new List<ProductViewModel>();
                if (Convert.ToInt32(OfficeID) > 0 && OfficeID != "")
                {
                    var param = new { OfficeID = Convert.ToInt32(OfficeID) };
                    var officeList = ultimateReportService.GetProductListForMapping(param);

                    List_ProductInfoViewModel = officeList.Tables[0].AsEnumerable()
                    .Select(row => new ProductViewModel
                    {
                        Sl = row.Field<int>("Sl"),
                        MyProductID = row.Field<Int16>("ProductId"),
                        ProductCode = row.Field<string>("ProductCode"),
                        ProductName = row.Field<string>("ProductFullNameEng")
                    }).ToList();
                }
                return Json(List_ProductInfoViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSelectedProductList(string OfficeID)
        {
            try
            {
                List<ProductViewModel> List_ProductInfoViewModel = new List<ProductViewModel>();
                if (Convert.ToInt32(OfficeID) > 0 && OfficeID != "")
                {
                    var param = new { OfficeID = Convert.ToInt32(OfficeID) };
                    var officeList = ultimateReportService.GetSelectedProductListForMapping(param);

                    List_ProductInfoViewModel = officeList.Tables[0].AsEnumerable()
                    .Select(row => new ProductViewModel
                    {
                        Sl = row.Field<int>("Sl"),
                        MyProductID = row.Field<Int16>("ProductId"),
                        ProductCode = row.Field<string>("ProductCode"),
                        ProductName = row.Field<string>("ProductFullNameEng")
                    }).ToList();
                }
                return Json(List_ProductInfoViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
         
        public JsonResult OfficeWiseProductSave(List<string> allProductIds, string OfficeID)
        {
            int dealOfficeId = 0;
            if (OfficeID != "")
            {
                
                var officeID = Convert.ToInt32(OfficeID);
                dealOfficeId = officeID;
                foreach (var ProdId in allProductIds)
                {
                    var param = new { OfficeID = Convert.ToInt32(OfficeID), ProductId = ProdId };
                    var officeList = ultimateReportService.SaveProductMapping(param);
                }
                return Json(dealOfficeId, JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }


        
        public JsonResult OfficeWiseProductDelete(List<string> ProductIds, string OfficeID)
        {
            int dealOfficeId = 0;
            if (OfficeID != "")
            {

                var officeID = Convert.ToInt32(OfficeID);
                dealOfficeId = officeID;
                foreach (var ProdId in ProductIds)
                {
                    var param = new { OfficeID = Convert.ToInt32(OfficeID), ProductId = ProdId };
                    var officeList = ultimateReportService.DeleteProductMapping(param);
        
                }
                return Json(dealOfficeId, JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }



        #endregion Methods



    }// End Class
}// End Namespace
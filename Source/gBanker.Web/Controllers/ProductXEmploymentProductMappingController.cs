using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class ProductXEmploymentProductMappingController : BaseController
    {
        // GET: ProductXEmploymentProductMapping
        private readonly IProductService productService;
        private readonly IProductXEmploymentProductMappingService productXEmploymentProductMappingService;

        public ProductXEmploymentProductMappingController(IProductService productService, IProductXEmploymentProductMappingService productXEmploymentProductMappingService)
        {
            this.productService = productService;
            this.productXEmploymentProductMappingService = productXEmploymentProductMappingService;
         
        }
        public ActionResult Create()
        {
            var model = new ProductXEmploymentProductMappingViewModel();
            MapDropDownList(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(ProductXEmploymentProductMappingViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");

            try
            {
                if (model.MappingId > 0)
                {
                    var employMappingRes = productXEmploymentProductMappingService.GetById(model.MappingId);

                    employMappingRes.EmploymentProductName = model.EmploymentProductName;
                    employMappingRes.MainProductCode = model.MainProductCode;
                    employMappingRes.MainProductName = model.MainProductName;
                    employMappingRes.DisplayOrder = model.DisplayOrder;

                    productXEmploymentProductMappingService.Update(employMappingRes);
                }
                else
                {
                    var entity = Mapper.Map<ProductXEmploymentProductMappingViewModel, ProductXEmploymentProductMapping>(model);
                    productXEmploymentProductMappingService.Create(entity);
                }

                return GetSuccessMessageResult("Data Saved Successfully");          
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        public ActionResult Edit(int id)
        {
            var employMappingRes = productXEmploymentProductMappingService.GetById(id);
            var entity = Mapper.Map<ProductXEmploymentProductMapping, ProductXEmploymentProductMappingViewModel>(employMappingRes);

            MapDropDownList(entity);

            return View("Create",entity);
        }
    
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                productXEmploymentProductMappingService.DeleteById(id);
                return GetSuccessMessageResult("Data Deleted Successfully");
            }
            catch(Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        private void MapDropDownList(ProductXEmploymentProductMappingViewModel model)
        {

            var productList = productService.GetProductMainCodeList();

            var viewproductList = productList.OrderBy(x => x.MainItemName).Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MainProductCode.ToString(),
                Text = x.MainItemName.ToString()
            });

            var productList_items = new List<SelectListItem>();
            productList_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            productList_items.AddRange(viewproductList);
            model.ProductList = productList_items;
        }
        [HttpPost]
        public JsonResult GetEmploymentMappingList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                List<ProductXEmploymentProductMappingViewModel> List_ProductXEmploymentProductMappingViewModel = new List<ProductXEmploymentProductMappingViewModel>();
                var param = new { };
                var employmentMappingList = productXEmploymentProductMappingService.GetEmploymentMappingList();
         
                var currentPageRecords = employmentMappingList;
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = employmentMappingList.Count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
    }
}
#region Usings

using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc; 

#endregion

namespace gBanker.Web.Controllers
{
    public class ProductMappingWithPOController : BaseController
    {
        #region Private Members

        public readonly IPOProductMappingService poProductMappingService;
        public readonly IPOLoanCodeService poLoanCodeService;
        public readonly IUltimateReportService ultimateReportService;
        public readonly IOrganizationService organizationService;
        #endregion

        #region Ctor
        public ProductMappingWithPOController(
            IPOProductMappingService poProductMappingService,
            IPOLoanCodeService poLoanCodeService,
            IUltimateReportService ultimateReportService, 
            IOrganizationService organizationService
            
            )
        {
            this.poProductMappingService = poProductMappingService;
            this.poLoanCodeService = poLoanCodeService;
            this.ultimateReportService = ultimateReportService;
            this.organizationService = organizationService;
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            var model = new AddOrEditLoanCodeXProductMappingViewModel { };

            return View(model);
        }

        #endregion

        #region Ajax Call
        public async Task<JsonResult> GetAvailableProductList(string OfficeID)
        {
            try
            {
                //populate products
                var productList = PopulateProducts(OfficeID);

                //get loan codes
                var poLoanCodes = await poLoanCodeService.GetPOLoanCodes();

                //get product mappings
                var poProductMappings = await poProductMappingService.GetPOProductMappings();

                //get loans product 
                var loanProducts = await poProductMappingService.GetLOAN_PRODUCT_List();

                var model = new LoanCodeXProductMappingViewModel
                {
                    ProductList = productList,
                    POLoanCodes = poLoanCodes,
                    POProductMappings = poProductMappings,
                    LoanProducts = loanProducts,
                };

                //get loan codexproduct mapping html
                var loanCodeXProductMappingHtml = GetLoanCodeXProductMappingHtml(model);

                JsonResult result = Json(loanCodeXProductMappingHtml);
                result.MaxJsonLength = Int32.MaxValue;
                result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return result;
                //return Json(loanCodeXProductMappingHtml, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetPOLoanCode()
        {
            var codeList = new List<SelectListItem>();
            var loanCodeList = poLoanCodeService.GetAll().Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = string.Format("{0}-{1}", p.LoanCode, p.FunctionalitiesAndFeatures)
            });
            return Json(loanCodeList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Manage(AddOrEditLoanCodeXProductMappingViewModel model)
        {
            var listing = new List<POProductMapping>();

            if (!model.LoanXProductMappings.Any(f => f.LoanCode != "Select One"))            
                return GetErrorMessageResult("Warning! Please select atleast one loan code for each dropdown.");            
            
            foreach (var item in model.LoanXProductMappings.Where(f=>f.LoanCode!= "Select One"))
            {
                var newPOProductMapping = new POProductMapping
                {
                    POCode = LoggedInOrganizationCode,
                    POId=(int)LoggedInOrganizationID,
                    LoanCode=item.LoanCode,
                    ProductCode=item.ProductCode,
                    ProductName = item.ProductName,
                    AssociatedLoanCode=item.AssociatedLoanCode!= "Select One"? item.AssociatedLoanCode:"",

                    LoanServiceChargeRate =item.LoanServiceChargeRate,
                    IsActive =true,
                    CreateUser=LoggedInEmployeeID,
                    CreateDate=DateTime.Now
                };

                listing.Add(newPOProductMapping);
            }

            //let's add into db for [POProductMapping]
            var isAdded = poProductMappingService.ManageProductXLoanMapping(listing);
            if(!isAdded)
                return GetErrorMessageResult("Error! There was an error while configure this mapping loan code with product.");

            return GetSuccessMessageResult();
        }
        #endregion

        #region Private Methods

        private string GetLoanCodeXProductMappingHtml(LoanCodeXProductMappingViewModel model)
        {
            string loanCodeXProductMappingHtml = "";
            try
            {
                int index = 0;
                foreach (var item in model.ProductList)
                {
                    index = index + 1;

                    var newlnCodXPrdMappingHtml = $@"
                            <tr> 
                                 <td>
                                    {index}
                                    <input type='hidden' name='LoanXProductMappings.Index' value='{index}' />
                                 </td> 
                                 <td> 
                                    {item.ProductCode} 
                                    <input type='hidden'  value='{item.ProductCode}' name='LoanXProductMappings[{index}].ProductCode' id='LoanXProductMappings[{index}]_ProductCode' /> 
                                 <td> 
                                    {item.ProductName}
                                    <input type='hidden'  value='{item.ProductName}' name='LoanXProductMappings[{index}].ProductName' id='LoanXProductMappings[{index}]_ProductName' /> 
                                 </td>
                                 <td> 
                                    {GetLoanCodeHtml(model, item, index)} 
                                </td> 
                                <td> 
                                    { GetAssociatedLoanCodeHtml(model, item, index)} 
                                </td>
                                <td> 
                                    {GetLoanCodeProductWiseSCRateHtml(model, item, index)} 
                                </td>
                            </tr>
                    ";

                    loanCodeXProductMappingHtml = loanCodeXProductMappingHtml + newlnCodXPrdMappingHtml;
                }
                return loanCodeXProductMappingHtml;
            }
            catch
            {
                return "";
            }
        }

        private string GetLoanCodeHtml(LoanCodeXProductMappingViewModel model, ProductViewModel product, int index)
        {
            string loanCodeHtml =
                $@"
                     <select name='LoanXProductMappings[{index}].LoanCode' id='LoanXProductMappings[{index}]_LoanCode' class='form-control chosen'>                                         
                        <option > Select One </option>           
                ";
            try
            {
                foreach (var item in model.POLoanCodes)
                {
                    var toggleSelected = model.POProductMappings.Any(a => a.LoanCode == item.LoanCode && a.ProductCode == product.ProductCode)
                        ? "selected='selected'" : "";

                    var newLoanCodeHtml = $@"<option {toggleSelected} value={item.LoanCode}> {item.LoanCode} - {item.FunctionalitiesAndFeatures} </option>";

                    loanCodeHtml = loanCodeHtml + newLoanCodeHtml;
                }

                loanCodeHtml = $"{loanCodeHtml} </select>";

                return loanCodeHtml;
            }
            catch
            {
                loanCodeHtml = $@"
                    <select name='LoanXProductMappings[{index}].LoanCode' id='LoanXProductMappings[{index}]_LoanCode' class='form-control chosen'>                                       
                        <option > Select One </option>
                    </select>";
            }

            return loanCodeHtml;
        }

        private string GetAssociatedLoanCodeHtml(LoanCodeXProductMappingViewModel model, ProductViewModel product, int index)
        {
            string loanCodeHtml =
                $@"
                     <select name='LoanXProductMappings[{index}].AssociatedLoanCode' id='LoanXProductMappings[{index}]_AssociatedLoanCode' class='form-control chosen'>                                         
                        <option > Select One </option>           
                ";
            try
            {
                foreach (var item in model.LoanProducts)
                {
                    var toggleSelected = model.POProductMappings.Any(a => a.ProductCode == product.ProductCode && model.POLoanCodes.Any(f=>f.LoanCode ==a.LoanCode && f.AssociatedLoanCode== item.LOAN_CODE))
                        ? "selected='selected'" : "";

                    var newLoanCodeHtml = $@"<option {toggleSelected} value='{item.LOAN_CODE}'> {item.LOAN_CODE} - {item.LOAN_NAME} </option>";

                    loanCodeHtml = loanCodeHtml + newLoanCodeHtml;
                }

                loanCodeHtml = $"{loanCodeHtml} </select>";

                return loanCodeHtml;
            }
            catch
            {
                loanCodeHtml = $@"
                    <select name='LoanXProductMappings[{index}].AssociatedLoanCode' id='LoanXProductMappings[{index}]_AssociatedLoanCode' class='form-control chosen'>                                      
                        <option> Select One </option>
                    </select>";
            }

            return loanCodeHtml;
        }

        private string GetLoanCodeProductWiseSCRateHtml(LoanCodeXProductMappingViewModel model, ProductViewModel product,int index)
        {
            decimal loanServiceChargeRate = 0;
            var singleMapping = model.POProductMappings.FirstOrDefault(f => f.ProductCode == product.ProductCode);
            if (singleMapping != null) loanServiceChargeRate = singleMapping.LoanServiceChargeRate;

            string loanServiceChargeRateHtml =
                $@"<input type='number' autocomplete='off' min='0' oninput='this.value = !!this.value && Math.abs(this.value) >= 0 ? Math.abs(this.value) : null' class='form-control input-sm' id='LoanXProductMappings[{index}]_LoanServiceChargeRate' name='LoanXProductMappings[{index}].LoanServiceChargeRate' value='{loanServiceChargeRate}' />";           

            return loanServiceChargeRateHtml;
        }
        private List<ProductViewModel> PopulateProducts(string OfficeID)
        {
            var param = new { OfficeID = Convert.ToInt32(OfficeID) };
            var officeList = ultimateReportService.GetProductListForMapping(param);

            var productList = officeList.Tables[0].AsEnumerable().Select(row => new ProductViewModel
            {
                Sl = row.Field<int>("Sl"),
                MyProductID = row.Field<Int16>("ProductId"),
                ProductCode = row.Field<string>("ProductCode"),
                ProductName = row.Field<string>("ProductFullNameEng")
            }).ToList();
            return productList;
        }


        #endregion
    }
}
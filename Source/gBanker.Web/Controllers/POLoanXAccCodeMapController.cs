#region Usings

using gBanker.Core.Filters;
using gBanker.Core.Utility;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.DBDetailModels.OLRSHubs.CommonModels;
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
    public class POLoanXAccCodeMapController : BaseController
    {
        #region Private Members
        public readonly IPOProductMappingService poProductMappingService;
        public readonly IPOLoanCodeService poLoanCodeService;
        public readonly IUltimateReportService ultimateReportService;
        public readonly IOrganizationService organizationService;
        #endregion

        #region Ctor
        public POLoanXAccCodeMapController(IPOProductMappingService poProductMappingService,
           IPOLoanCodeService poLoanCodeService,
           IUltimateReportService ultimateReportService, IOrganizationService organizationService)
        {
            this.poProductMappingService = poProductMappingService;
            this.poLoanCodeService = poLoanCodeService;
            this.ultimateReportService = ultimateReportService;
            this.organizationService = organizationService;
        }
        #endregion       

        #region Map
        public ActionResult Map()
        {
            var model = new LoanCodeXAccCodeMappingViewModel { };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Map(LoanCodeXAccCodeMappingViewModel model)
        {
            try
            {
                var listing = new List<POProductMapping>();

                if (!model.LoanXAccCodeMappings.Any(f => f.LoanCode != "Select One"))
                    return GetErrorMessageResult("Warning! Please select atleast one loan code for each dropdown.");

                var updatedPOLoanCodes = new List<POLoanCode>();

                foreach (var item in model.LoanXAccCodeMappings)
                {
                    var newPOLoanCode = new POLoanCode
                    {
                        LoanCode = item.LoanCode,
                        AssociatedAccCodeFA = item.AssociatedAccCodeFA == null ? "" : string.Join(",", item.AssociatedAccCodeFA),
                        AssociatedAccCodeSCP = item.AssociatedAccCodeSCP == null ? "" : string.Join(",", item.AssociatedAccCodeSCP),
                        IsActive = true,
                        UpdateUser = LoggedInEmployeeID,
                        UpdateDate = DateTime.Now
                    };
                    updatedPOLoanCodes.Add(newPOLoanCode);
                }

                //let's add into db for [POLoanCode]              
                var isAdded = await poLoanCodeService.GetPOLoanCodes(updatedPOLoanCodes);
                if (!isAdded)
                    return GetErrorMessageResult("Error! There was an error while configure this mapping loan code with acc code.");

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            { 
                return GetErrorMessageResult("Error! There was an error while configure this mapping loan code with acc code.");

            }
             
        }

        #endregion

        #region Ajax Call
        public async Task<JsonResult> GetToPopulateLoanCodeWiseAcc()
        {
            try
            {
                //get loan codes
                var poLoanCodes = await poLoanCodeService.GetPOLoanCodes();

                //get loan code x acc mapping html
                var loanCodeXAccCodeMappingHtml = await GetLoanCodeXAccCodeMappingHtml(poLoanCodes);
                return Json(loanCodeXAccCodeMappingHtml, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }       
               
        #endregion       

        #region Private Methods

        private async Task<string> GetLoanCodeXAccCodeMappingHtml(IEnumerable<POLoanCode> pOLoanCodes)
        {
            string loanCodeXAccCodeMappingHtml = "";
            try
            {
                int index = 0;
                foreach (var item in pOLoanCodes)
                {
                    index = index + 1;
                    var fnAndFeature = item.FunctionalitiesAndFeatures.Replace("Number field for entering Loan amount of ", "")
                        .Replace("Number field for entering ", "");

                    var newlnCodXAccCodeMappingHtml = $@"
                            <tr> 
                                 <td>
                                    {index}
                                    <input type='hidden' name='LoanXAccCodeMappings.Index' value='{index}' />
                                 </td> 
                                 <td> 
                                    {item.LoanCode} 
                                    <input type='hidden'  value='{item.LoanCode}' name='LoanXAccCodeMappings[{index}].LoanCode' id='LoanXAccCodeMappings[{index}]_LoanCode' /> 
                                 <td> 
                                    {fnAndFeature} 
                                 </td>
                                 <td> 
                                    {await GetLoanCodeXAssociatedAccCodeFAHtml(item, index)} 
                                </td>
                                <td> 
                                    {await GetLoanCodeXAssociatedAccCodeSCPHtml(item, index)} 
                                </td>
                            </tr>
                    ";

                    loanCodeXAccCodeMappingHtml = loanCodeXAccCodeMappingHtml + newlnCodXAccCodeMappingHtml;
                }
                return loanCodeXAccCodeMappingHtml;
            }
            catch
            {
                return "";
            }
        }

        private async Task<string> GetLoanCodeXAssociatedAccCodeFAHtml(POLoanCode pOLoanCode, int index)
        {
            //get po loan code related acc codes
            var filter = new BaseSearchFilter { ReportName = ConsolidateReportConstants.FA, ReportType = MFIReportTypeConstants.Consolidate };
            var accCodes = await poLoanCodeService.GetPOAccCodes(filter);           

            string loanCodeHtml =
                $@"
                     <select multiple='multiple' name='LoanXAccCodeMappings[{index}].AssociatedAccCodeFA' id='LoanXAccCodeMappings[{index}]_AssociatedAccCodeFA' class='form-control chosen'>                                         
                        <option > Select One </option>           
                ";
            try
            {
                List<string> accCodeList = new List<string>();
                if (!string.IsNullOrWhiteSpace(pOLoanCode.AssociatedAccCodeFA))                
                    accCodeList = pOLoanCode.AssociatedAccCodeFA.Split(',').ToList();
                
                foreach (var item in accCodes)
                {
                    var toggleSelected = accCodeList.Any(accCode => accCode == item.AccCode)
                        ? "selected='selected'" : "";

                    var newLoanCodeHtml = $@"<option {toggleSelected} value={item.AccCode}> {item.AccName} </option>";

                    loanCodeHtml = loanCodeHtml + newLoanCodeHtml;
                }

                loanCodeHtml = $"{loanCodeHtml} </select>";

                return loanCodeHtml;
            }
            catch
            {
                loanCodeHtml = $@"
                    <select multiple='multiple' class='form-control chosen'>                                         
                        <option > Select One </option>
                    </select>";
            }

            return loanCodeHtml;
        }

        private async Task<string> GetLoanCodeXAssociatedAccCodeSCPHtml(POLoanCode pOLoanCode, int index)
        {
            //get po loan code related acc codes
            var filter = new BaseSearchFilter { ReportName = ConsolidateReportConstants.SCP, ReportType = MFIReportTypeConstants.Consolidate };
            var accCodes = await poLoanCodeService.GetPOAccCodes(filter);
            
            string loanCodeHtml =
                $@"
                     <select multiple='multiple' name='LoanXAccCodeMappings[{index}].AssociatedAccCodeSCP' id='LoanXAccCodeMappings[{index}]_AssociatedAccCodeSCP' class='form-control chosen'>                                         
                        <option > Select One </option>           
                ";
            try
            {
                List<string> accCodeList =new List<string>();
                if (!string.IsNullOrWhiteSpace(pOLoanCode.AssociatedAccCodeSCP))
                    accCodeList = pOLoanCode.AssociatedAccCodeSCP.Split(',').ToList();

                foreach (var item in accCodes)
                {
                    var toggleSelected = accCodeList.Any(accCode => accCode == item.AccCode)
                        ? "selected='selected'" : "";

                    var newLoanCodeHtml = $@"<option {toggleSelected} value={item.AccCode}> {item.AccName} </option>";

                    loanCodeHtml = loanCodeHtml + newLoanCodeHtml;
                }

                loanCodeHtml = $"{loanCodeHtml} </select>";

                return loanCodeHtml;
            }
            catch
            {
                loanCodeHtml = $@"
                    <select multiple='multiple' class='form-control chosen'>                                         
                        <option > Select One </option>
                    </select>";
            }

            return loanCodeHtml;
        }
        #endregion
    }
}
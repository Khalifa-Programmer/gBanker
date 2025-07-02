#region Usings

using gBanker.Core.Filters;
using gBanker.Core.Utility;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace gBanker.Web.Controllers
{
    public class IndXAccCodeMapController : BaseController
    {
        #region Private Members
        public readonly IIndicatorService indicatorService;
        #endregion

        #region Ctor
        public IndXAccCodeMapController( IIndicatorService indicatorService)
        {
            this.indicatorService = indicatorService;
        }
        #endregion       

        #region Map
        public ActionResult Map()
        {
            var model = new IndicatorMappingViewModel { };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Map(IndicatorMappingViewModel model)
        {
            try
            {
                var updateIndicators = new List<Indicator>();

                foreach (var item in model.IndicatorMappings)
                {
                    var newIndicator = new Indicator
                    {
                        IndicatorCode = item.IndicatorCode,
                        AssociatedAccCodeFD = item.AssociatedAccCodeFD == null ? "" : string.Join(",", item.AssociatedAccCodeFD),
                        IsActive = true,
                        UpdateUser = LoggedInEmployeeID,
                        UpdateDate = DateTime.Now
                    };
                    updateIndicators.Add(newIndicator);
                }

                //let's add into db for [Indicator]              
                var isAdded = await indicatorService.UpdateIndicators(updateIndicators);
                if (!isAdded)
                    return GetErrorMessageResult("Error! There was an error while configure this mapping Indicator with acc code.");

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            { 
                return GetErrorMessageResult("Error! There was an error while configure this mapping Indicator with acc code.");

            }
             
        }

        #endregion

        #region Ajax Call
        public async Task<JsonResult> GetToPopulateIndicatorWiseAcc()
        {
            try
            {
                //get Indicators
                var indicators = await indicatorService.GetIndicatorsByFD();

                //get Indicator x acc mapping html
                var indicatorXAccCodeMappingHtml = await GetIndicatorXAccCodeMappingHtml(indicators);
                return Json(indicatorXAccCodeMappingHtml, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }       
               
        #endregion       

        #region Private Methods

        private async Task<string> GetIndicatorXAccCodeMappingHtml(IEnumerable<Indicator> indicators)
        {
            string indicatorXAccCodeMappingHtml = "";
            try
            {
                int index = 0;
                foreach (var item in indicators)
                {
                    index = index + 1;
                    var fnAndFeature = item.AssociatedAccCodeFD;

                    var newlnCodXAccCodeMappingHtml = $@"
                           <tr>
                                <td>
                                    {index}
                                    <input type='hidden' name='IndicatorMappings.Index' value='{index}' />
                                </td>
                                <td>
                                    {item.IndicatorName}
                                </td>
                                <td>
                                    {item.IndicatorCode}
                                    <input type='hidden' value='{item.IndicatorCode}' name='IndicatorMappings[{index}].IndicatorCode' id='IndicatorMappings[{index}]_IndicatorCode' />
                                </td>
                                <td>
                                    {await GetIndicatorXAssociatedAccCodeFAHtml(item, index)}
                                </td>
                            </tr>
                    ";

                    indicatorXAccCodeMappingHtml = indicatorXAccCodeMappingHtml + newlnCodXAccCodeMappingHtml;
                }
                return indicatorXAccCodeMappingHtml;
            }
            catch
            {
                return "";
            }
        }

        private async Task<string> GetIndicatorXAssociatedAccCodeFAHtml(Indicator indicator, int index)
        {
            //get po Indicator related acc codes
            var filter = new BaseSearchFilter { };
            var accCodes = await indicatorService.GetPOAccCodesByFilter(filter);           

            string indicatorHtml =
                $@"
                     <select multiple='multiple' name='IndicatorMappings[{index}].AssociatedAccCodeFD' id='IndicatorMappings[{index}]_AssociatedAccCodeFA' class='form-control chosen'>                                         
                        <option > Select One </option>           
                ";
            try
            {
                List<string> accCodeList = new List<string>();
                if (!string.IsNullOrWhiteSpace(indicator.AssociatedAccCodeFD))                
                    accCodeList = indicator.AssociatedAccCodeFD.Split(',').ToList();
                
                foreach (var item in accCodes)
                {
                    var toggleSelected = accCodeList.Any(accCode => accCode == item.AccCode)
                        ? "selected='selected'" : "";

                    var newIndicatorHtml = $@"<option {toggleSelected} value={item.AccCode}> {item.AccName} </option>";

                    indicatorHtml = indicatorHtml + newIndicatorHtml;
                }

                indicatorHtml = $"{indicatorHtml} </select>";

                return indicatorHtml;
            }
            catch
            {
                indicatorHtml = $@"
                    <select multiple='multiple' class='form-control chosen'>                                         
                        <option > Select One </option>
                    </select>";
            }

            return indicatorHtml;
        }

        #endregion
    }
}
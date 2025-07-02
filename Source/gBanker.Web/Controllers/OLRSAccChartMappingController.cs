#region Usings

using gBanker.Core.Filters;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
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
    public class OLRSAccChartMappingController : BaseController
    {
        #region Private Members

        public readonly IOLRSAccChartMappingService olrsAccChartMappingService;
        public readonly IUltimateReportService ultimateReportService;
        public readonly IOrganizationService organizationService;
        #endregion
         
        #region Ctor
        public OLRSAccChartMappingController(IOLRSAccChartMappingService olrsAccChartMappingService,
           IUltimateReportService ultimateReportService, IOrganizationService organizationService)
        {
            this.olrsAccChartMappingService = olrsAccChartMappingService;
            this.ultimateReportService = ultimateReportService;
            this.organizationService = organizationService;
        }
        #endregion

        #region List
        public ActionResult List()
        {
            var model = new AccountCodeMappingSearchFilter();
            return View(model);
        }

        public JsonResult GetAccChartWithOLRSAccCodeMapList(int jtStartIndex, int jtPageSize, string jtSorting, string filterValue, string searchTerm)
        {
            try
            {
                var param = new { OrgCode = LoggedInOrganizationCode, searchTerm = searchTerm };
                var getData = ultimateReportService.GetDataWithParameter(param, "pksf.GetAccChartWithOLRSAccCodeMapList");
                var detail = getData.Tables[0].AsEnumerable().Select(p => new OLRSAccChartMappingViewModel
                {
                    Id = p.Field<int>("Id"),
                    AccCodeOLRS = p.Field<string>("AccCodeOLRS"),
                    AccChartCode = p.Field<string>("AccCode")
                }).ToList();
                var currentPageRecords = detail.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = detail.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Map
        public async Task<ActionResult> Map()
        {
            var model = new OLRSAccChartMappingViewModel
            {
                AccCodeOLRSList = await GetOLRSAccChartDropDownList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Map(OLRSAccChartMappingViewModel model)
        {
            if(!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");

            var param = new OLRSAccChartMapping { POCode = LoggedInOrganizationCode, AccCodeOLRS = model.AccCodeOLRS, AccCode = model.AccChartCode };
            var checkDuplicacy = olrsAccChartMappingService.CheckAccChartMappingDuplicacy(param);
            if (checkDuplicacy == true)
                return GetErrorMessageResult("Duplicate mapping found, save denied");

            var layersCode = model.AccCodeOLRS.Split('-');

            var newOLRSAccChartMapping = new OLRSAccChartMapping
            {
                POCode = LoggedInOrganizationCode,
                POId = (int)LoggedInOrganizationID,
                AccCodeOLRS = model.AccCodeOLRS,
                AccCode = model.ConfigAccChartCode,
                l1_code= layersCode[0],
                l2_code = layersCode[1],
                l3_code = layersCode[2],
                l4_code = layersCode[3],
                l5_code = layersCode[4],
                IsActive = true,
                CreateUser = LoggedInEmployeeID,
                CreateDate = DateTime.Now
            };        

            //let's add into db for [OLRSAccChartMapping]
            var isAdded = await olrsAccChartMappingService.AddAccChartMapping(newOLRSAccChartMapping);
            if (!isAdded)
                return GetErrorMessageResult("Error! There was an error while configure this mapping.");

            return GetSuccessMessageResult();
        }

        #endregion

        #region Ajax Call

        [HttpPost]
        public async Task<JsonResult> GetAccChartMappingHtml(AccChartSearchFilter filter)
        {
            try
            {
                var olrsAccChartMappings = await olrsAccChartMappingService.GetOLRSAccChartMappingList();

                //get acc charts
                var acccCharts = await olrsAccChartMappingService.GetAccChartList(filter);

                //get po acc charts (olrs)
                var poAccCharts = await olrsAccChartMappingService.GetPOAccChartList();

                var model = new AccChartXOlrsAccCodeMappingViewModel
                {
                    POAccCharts = poAccCharts,
                    AccCharts = acccCharts,
                    OLRSAccChartMappings = olrsAccChartMappings
                };

                //get acc chart x olrs acc code mapping html
                var accChartXOlrsAccCodeMappingHtml = GetAccChartXOlrsAccCodeMappingHtml(model);

                return Json(accChartXOlrsAccCodeMappingHtml, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAccChartDDLByLavel(string accChartLevel)
        {
            try
            {
                var filter = new AccChartSearchFilter { AccChartLevel = accChartLevel };
                var listing = await olrsAccChartMappingService.GetAccChartListByLevel(filter);

                var listItems = listing.Select(item => new SelectListItem
                {
                    Value = item.AccCode,
                    Text = $"{item.AccCode} - {item.AccName}"

                }).ToList();

                return Json(new { listItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var items = new List<SelectListItem>();
                return Json(new { items }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetConfigAccChartDDLByLavelAndCode(string accChartLevel, string accCode)
        {
            try
            {
                var filter = new AccChartSearchFilter { AccChartLevel = accChartLevel, AccCode= accCode };
                // get acc charts
                var listing = await olrsAccChartMappingService.GetAccChartList(filter);

                var listItems = listing.Select(item => new SelectListItem
                {
                    Value = item.AccCode,
                    Text = $"{item.AccCode} - {item.AccName}"

                }).ToList();

                return Json(new { listItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var items = new List<SelectListItem>();
                return Json(new { items }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Delete

        [HttpDelete]
        public JsonResult DeleteAccMap(int Id)
        {
            try
            {
                olrsAccChartMappingService.Delete(Id);
                return GetSuccessMessageResult("Account Mapping deleted successfully");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Private Methods

        private async Task<IEnumerable<SelectListItem>> GetOLRSAccChartDropDownList()
        {
            //get po acc charts (olrs)
            var poAccCharts = await olrsAccChartMappingService.GetPOAccChartList();

            var selectListItems = poAccCharts.Select(x => new SelectListItem
            {
                Value = x.AccLayers,
                Text = $"{x.accgroup} - {x.AccountType} - {x.acchead} ({x.AccLayers})"
            });         

            return selectListItems;
        }

        private string GetAccChartXOlrsAccCodeMappingHtml(AccChartXOlrsAccCodeMappingViewModel model)
        {
            string olrsAccChartMappingHtml = "";
            try
            {
                int index = 0;
                foreach (var item in model.AccCharts)
                {
                    index = index + 1;

                    var newOLRSAccChartMappingHtml = $@"
                            <tr> 
                                 <td>
                                    {index}
                                    <input type='hidden' name='OLRSAccChartMappings.Index' value='{index}' />
                                 </td> 
                                 <td> 
                                    {item.AccCode} 
                                    <input type='hidden'  value='{item.AccCode}' name='OLRSAccChartMappings[{index}].AccCode' id='OLRSAccChartMappings[{index}]_AccCode' /> 
                                 <td> 
                                    {item.AccName}
                                 </td>
                                 <td> 
                                    {GetOlrsAccCodeHtml(model, item, index)} 
                                </td> 
                            </tr>
                    ";

                    olrsAccChartMappingHtml = olrsAccChartMappingHtml + newOLRSAccChartMappingHtml;
                }
                return olrsAccChartMappingHtml;
            }
            catch
            {
                return "";
            }
        }

        private string GetOlrsAccCodeHtml(AccChartXOlrsAccCodeMappingViewModel model, AccChart accChart, int index)
        {
            string olrsAccChartHtml =
                $@"
                     <select name='OLRSAccChartMappings[{index}].AccCodeOLRS' id='OLRSAccChartMappings[{index}]_AccCodeOLRS' class='form-control'>                                         
                        <option > Select One </option>           
                ";
            try
            {
                /*
                foreach (var item in model.POAccCharts)
                {
                    var toggleSelected = model.OLRSAccChartMappings.Any(a => a.AccCodeOLRS == item.AccCode && a.AccCode == accChart.AccCode)
                        ? "selected='selected'" : "";

                    var newLoanCodeHtml = $@"<option {toggleSelected} value='{item.AccCode}'> {item.AccCode} - {item.FunctionalitiesAndFeatures} </option>";

                    olrsAccChartHtml = olrsAccChartHtml + newLoanCodeHtml;
                }
                */

                olrsAccChartHtml = $"{olrsAccChartHtml} </select>";

                return olrsAccChartHtml;
            }
            catch
            {
                olrsAccChartHtml = $@"
                    <select class='form-control'>                                         
                        <option > Select One </option>
                    </select>";
            }
            return olrsAccChartHtml;
        }
               
        #endregion
    }
}
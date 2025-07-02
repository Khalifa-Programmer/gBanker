
#region Usings

using gBanker.Core.Filters;
using gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads;
using gBanker.Service.OLRS;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using gHRM.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace gBanker.Web.Controllers
{
    public class OLRSEmploymentController : BaseController
    {
        #region Private Members

        private readonly IUltimateReportService ultimateReportService;
        private readonly IOLRSHubService olrSHubService;
        private readonly IOLRSService oLRSService;

        #endregion

        #region Ctor

        public OLRSEmploymentController(
           IUltimateReportService ultimateReportService,
           IOLRSService oLRSService,
           IOLRSHubService olrSHubService
           )
        {
            this.ultimateReportService = ultimateReportService;
            this.olrSHubService = olrSHubService;
            this.oLRSService = oLRSService;
        }


        #endregion

        #region Create

        [HttpGet]
        public ActionResult Create()
        {
            var model = new OlrsEmploymentViewModel { };
            return View(model);
        }

        public async Task<JsonResult> Create(OlrsEmploymentViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");
            try
            {
                var param = new
                {
                    @employmentType = model.EmploymentType,
                    @po_code = SessionHelper.LoggedInOrganizationCode,
                    @mnyr = model.MNYR,
                    @loan_code = model.LOAN_CODE,
                    @self_full_m = model.SELF_FULL_M,
                    @self_full_f = model.SELF_FULL_F,
                    @self_part_m = model.SELF_PART_M,
                    @self_part_f = model.SELF_PART_F,
                    @wage_full_m = model.WAGE_FULL_M,
                    @wage_full_f = model.WAGE_FULL_F,
                    @wage_part_m = model.WAGE_PART_M,
                    @wage_part_f = model.WAGE_PART_F,
                    @ins_user = SessionHelper.LoginUserEmployeeID
                };

                //let's add into db
                ultimateReportService.GetDataWithParameter(param, "[pksf].PRA_EMPLOYMENT_InsertData");

                return GetSuccessMessageResult();
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while adding.");
            }
        }        

        #endregion

        #region Sync Employment A ToPKSF

        [HttpGet]
        public async Task<JsonResult> SyncEmploymentAToPKSF()
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                var olrsHubToken = await olrSHubService.GetAccessToken();

                if (!olrsHubToken.isSuccess)
                    return GetErrorMessageResult(olrsHubToken.message);

                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    PostingFlag = "1"
                };

                var listing = await oLRSService.PRA_EMPLOYMENT_OLD_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Posted data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;

                    var pra_EMPLOYMENT_OLD_List = new List<PRA_EMPLOYMENT_OLD_Model>();
                    pra_EMPLOYMENT_OLD_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_EMPLOYMENT_OLD(pra_EMPLOYMENT_OLD_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_OLD(item.PO_CODE, item.MNYR, item.LOAN_CODE,updatedUser);
                    else
                        isErrorFound = true;
                }

                if (isErrorFound)
                    return GetErrorMessageResult("Warning! Operation Partially Completed");

                return GetSuccessMessageResult("Success! Add Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while sync to pksf.");
            }
        }

        #endregion

        #region Sync Employment B ToPKSF

        [HttpGet]
        public async Task<JsonResult> SyncEmploymentBToPKSF()
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                var olrsHubToken = await olrSHubService.GetAccessToken();

                if (!olrsHubToken.isSuccess)
                    return GetErrorMessageResult(olrsHubToken.message);

                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    PostingFlag = "1"
                };

                var listing = await oLRSService.PRA_EMPLOYMENT_CLS_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Posted data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;

                    var pra_EMPLOYMENT_CLS_List = new List<PRA_EMPLOYMENT_CLS_Model>();
                    pra_EMPLOYMENT_CLS_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_EMPLOYMENT_CLS(pra_EMPLOYMENT_CLS_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_CLS(item.PO_CODE, item.MNYR,item.LOAN_CODE, updatedUser);
                    else
                        isErrorFound = true;
                }

                if (isErrorFound)
                    return GetErrorMessageResult("Warning! Operation Partially Completed");

                return GetSuccessMessageResult("Success! Add Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while sync to pksf.");
            }
        }

        #endregion

        #region Sync Employment C ToPKSF

        [HttpGet]
        public async Task<JsonResult> SyncEmploymentCToPKSF()
        {
            try
            {
                var updatedUser = $@"{UserFullName}({LoggedInEmployeeID})";
                var olrsHubToken = await olrSHubService.GetAccessToken();

                if (!olrsHubToken.isSuccess)
                    return GetErrorMessageResult(olrsHubToken.message);

                var filter = new BaseSearchFilter
                {
                    PoCode = LoggedInOrganizationCode,
                    PostingFlag = "1"
                };

                var listing = await oLRSService.PRA_EMPLOYMENT_NEW_LIST_BY_FILTER(filter);

                if (!listing.Any())
                    return GetErrorMessageResult("Warning! Posted data not found");

                var isErrorFound = false;

                foreach (var item in listing)
                {
                    item.INS_DATE = null;
                    item.STATUS_DATE = null;

                    var pra_EMPLOYMENT_NEW_List = new List<PRA_EMPLOYMENT_NEW_Model>();
                    pra_EMPLOYMENT_NEW_List.Add(item);

                    //let's sync to pksf db
                    var response = await olrSHubService.Add_PRA_EMPLOYMENT_NEW(pra_EMPLOYMENT_NEW_List, olrsHubToken.token);

                    if (response.isSuccess)
                        await oLRSService.UPDATE_FLAG_AND_REMOVE_DUMP_PRA_EMPLOYMENT_NEW(item.PO_CODE, item.MNYR,item.LOAN_CODE, updatedUser);
                    else
                        isErrorFound = true;
                }

                if (isErrorFound)
                    return GetErrorMessageResult("Warning! Operation Partially Completed");

                return GetSuccessMessageResult("Success! Add Operation Completed.");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while sync to pksf.");
            }
        }

        #endregion

        #region Delete

        [HttpDelete]
        public async Task<ActionResult> DeleteEmploymentData(string mnyr, string type)
        {
            try
            {
                var isDeleted = await oLRSService.DeleteEmploymentData(mnyr, type);
                if(!isDeleted)
                    return GetErrorMessageResult("Warning! Delete Operation not completed");

                return GetSuccessMessageResult("Success! Data Deleted");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
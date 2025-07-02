
#region Usings

using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using gBankerCodeFirstMigration.Db;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace gBanker.Web.Controllers
{
    public class CompanyController : BaseController
    {
        #region Private Members
                
        public readonly IOrganizationService organizationService;
        public readonly IPO_INFOService po_INFOService;

        #endregion

        #region Ctor

        public CompanyController(                     
            IOrganizationService organizationService,
            IPO_INFOService po_INFOService
           )
        {           
            this.organizationService = organizationService;
            this.po_INFOService = po_INFOService;
        }

        #endregion

        #region Manage

        [HttpGet]
        public async Task<ActionResult> Manage()
        {
            var orgInfo = await organizationService.GetOrganizationById((int)LoggedInOrganizationID);

            var pksfPOCodeMapping = await po_INFOService.Get_PO_INFO_MAPPING(orgInfo.OrganizationCode);
            string pksfPOCode = "";

            if (pksfPOCodeMapping != null)
            {
                pksfPOCode = pksfPOCodeMapping.PKSF_PO_CODE;
            }

            var model = new ManageOrgViewModel
            {
                OrganaizationName = orgInfo.OrganizationName,
                OrganaizationCode = orgInfo.OrganizationCode,
                OrganaizationAddress = orgInfo.OrgAddress,
                PKSFPOCode= pksfPOCode,

                POCodeList = await GetPKSFPOCodeDropDownList(pksfPOCode)
            };

            return View(model);
        }

        public async Task<JsonResult> Manage(ManageOrgViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");

            try
            {              
                var orgInfo = await organizationService.GetOrganizationById((int)LoggedInOrganizationID);
                if(orgInfo==null)
                    return GetErrorMessageResult("Warning! Organization not exists.");

                var updateOrganization = new Organization { OrgID= (int)LoggedInOrganizationID, OrganizationName=model.OrganaizationName,OrgAddress=model.OrganaizationAddress};

                //let's updated organization
                var isUpdated = await organizationService.UpdateOrganization(updateOrganization);
                if(!isUpdated)
                    return GetErrorMessageResult("Error! There was an error while managing organization settings.");

                //let's manage po info mapping with mfi [pksf.PO_INFO_MAPPING]
                var isManaged = await po_INFOService.Manage_PO_INFO_MAPPING(model.OrganaizationCode, model.PKSFPOCode);
                
                if(!isManaged)
                    return GetErrorMessageResult("Error! There was an error while managing organization settings.");

                return GetSuccessMessageResult("Success! Organization Settings Updated");
            }
            catch
            {
                return GetErrorMessageResult("Error! There was an error while managing organization settings.");
            }
        }

        #endregion

        #region Private Methods
        private async Task<IEnumerable<SelectListItem>> GetPKSFPOCodeDropDownList(string po_Code="")
        {
            //get po codes
            var poCodes = await po_INFOService.GetPO_INFOCodes();

            var selectListItems = poCodes.Select(x => new SelectListItem
            {               
                Value = x.po_code,
                Text = $"{x.po_code} - {x.po_name}",
                Selected=  x.po_code== po_Code
            });

            return selectListItems;
        }


        #endregion
    }
}
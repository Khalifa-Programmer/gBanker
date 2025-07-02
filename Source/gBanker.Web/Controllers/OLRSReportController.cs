using gBanker.Core.Filters;
using gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads;
using gBanker.Data.DBDetailModels.OLRSHubs.Reports;
using gBanker.Service;
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
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class OLRSReportController : BaseController
    {
        #region Private Members

        private readonly IUltimateReportService ultimateReportService;
        private readonly IOLRSHubService olrSHubService;
        private readonly IOLRSService oLRSService;
        private readonly IDistrictService districtService;


        #endregion

        #region Ctor

        public OLRSReportController(IUltimateReportService ultimateReportService,
            IDistrictService districtService,
            IOLRSService oLRSService,
            IOLRSHubService olrSHubService)
        {
            this.ultimateReportService = ultimateReportService;
            this.olrSHubService = olrSHubService;
            this.oLRSService = oLRSService;
            this.districtService = districtService;
        }

        #endregion        

        #region Program Data
        [HttpGet]
        public ActionResult ProgramData()
        {
            var model = new PRA_MN_RPT_TAB_XL_PD_ReportSearchFilter();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetProgramData(int jtStartIndex, int jtPageSize, string jtSorting,
            string filterValue, string mnyr, string ind_code, string m_f_flag, string syncedStatus, bool isCalculateTotal, int totalCount)
        {
            try
            {
                int pageNumber = jtStartIndex > 0 ? jtStartIndex : 1;
                if (pageNumber > 1) //if not first page                
                    pageNumber = (jtStartIndex / jtPageSize) + 1;

                var filter = new PRA_MN_RPT_TAB_XL_PD_ReportSearchFilter
                {
                    OrgCode = LoggedInOrganizationCode,
                    MNYR = mnyr ?? string.Empty,
                    IND_CODE = ind_code ?? string.Empty,
                    M_F_flag = m_f_flag ?? string.Empty,
                    SYNCED_STATUS = syncedStatus ?? string.Empty,
                    IsCalculateTotal = isCalculateTotal,
                    PageNumber = pageNumber,
                    PageSize = jtPageSize
                };

                var filtedListing = await olrSHubService.Get_PRA_MN_RPT_TAB_XL_PD_By_Filter(filter);

                if (filtedListing.Any() && isCalculateTotal)
                    totalCount = filtedListing[0].TotalCount;

                return Json(new { Result = "OK", Records = filtedListing, TotalRecordCount = totalCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Route("olrsreport/programdata/mmyr/{mmyr}/indcode/{indcode}/mfflag/{mfflag}")]
        public async Task<ActionResult> ProgramDataDetails(string mmyr, string indcode, string mfflag)
        {
            try
            {
                var filter = new PRA_MN_RPT_TAB_XL_PD_ReportSearchFilter
                {
                    OrgCode = LoggedInOrganizationCode,
                    MNYR = mmyr ?? string.Empty,
                    IND_CODE = indcode ?? string.Empty,
                    M_F_flag = mfflag ?? string.Empty
                };

                var model = await olrSHubService.Get_PRA_MN_RPT_TAB_XL_PD(filter);

                return View(model);
            }
            catch (Exception ex)
            {
                return View(new PRA_MN_RPT_TAB_XL_PD_Model());
            }
        }

        #endregion

        #region Financial Data
        [HttpGet]
        public ActionResult FinancialData()
        {
            var model = new PRA_MN_RPT_TAB_XL_FD_ReportSearchFilter();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetFinancialData(int jtStartIndex, int jtPageSize, string jtSorting,
            string filterValue,string mnyr,string ind_code,string m_f_flag,string syncedStatus, bool isCalculateTotal, int totalCount)
        {
            try
            {
                int pageNumber = jtStartIndex > 0 ? jtStartIndex : 1;
                if (pageNumber > 1) //if not first page                
                    pageNumber = (jtStartIndex / jtPageSize) + 1;

                var filter = new PRA_MN_RPT_TAB_XL_FD_ReportSearchFilter
                {
                    OrgCode=LoggedInOrganizationCode,
                    MNYR = mnyr ?? string.Empty,
                    IND_CODE = ind_code ?? string.Empty,
                    M_F_flag = m_f_flag ?? string.Empty,
                    SYNCED_STATUS = syncedStatus ?? string.Empty,
                    IsCalculateTotal = isCalculateTotal,
                    PageNumber = pageNumber,
                    PageSize = jtPageSize
                };

                var filtedListing = await olrSHubService.Get_PRA_MN_RPT_TAB_XL_FD_By_Filter(filter);

                if (filtedListing.Any() && isCalculateTotal)
                    totalCount = filtedListing[0].TotalCount;

                return Json(new { Result = "OK", Records = filtedListing, TotalRecordCount = totalCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Basic Data
        [HttpGet]
        public ActionResult BasicData()
        {
            var model = new PRA_MN_RPT_TAB_XL_BD_ReportSearchFilter();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetBasicData(int jtStartIndex, int jtPageSize, string jtSorting,
            string filterValue, string mnyr, string ind_code, string m_f_flag, string syncedStatus, bool isCalculateTotal, int totalCount)
        {
            try
            {
                int pageNumber = jtStartIndex > 0 ? jtStartIndex : 1;
                if (pageNumber > 1) //if not first page                
                    pageNumber = (jtStartIndex / jtPageSize) + 1;

                var filter = new PRA_MN_RPT_TAB_XL_BD_ReportSearchFilter
                {
                    OrgCode = LoggedInOrganizationCode,
                    MNYR = mnyr ?? string.Empty,
                    IND_CODE = ind_code ?? string.Empty,
                    M_F_flag = m_f_flag ?? string.Empty,
                    SYNCED_STATUS = syncedStatus ?? string.Empty,
                    IsCalculateTotal = isCalculateTotal,
                    PageNumber = pageNumber,
                    PageSize = jtPageSize
                };

                var filtedListing = await olrSHubService.Get_PRA_MN_RPT_TAB_XL_FD_By_Filter(filter);

                if (filtedListing.Any() && isCalculateTotal)
                    totalCount = filtedListing[0].TotalCount;

                return Json(new { Result = "OK", Records = filtedListing, TotalRecordCount = totalCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Upazilla Loan
        [HttpGet]
        public ActionResult UpazillaLoan()
        {
            var model = new PRA_LN_DIST_WISE_DISB_ReportSearchFilter();

            //upazilla loan init
            UpazillaLoanInit(model);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetUpazillaLoan(int jtStartIndex, int jtPageSize, string jtSorting,
            string filterValue, string mnyr,string mfi_District_Code, string mfi_Thana_Code,   string syncedStatus, bool isCalculateTotal, int totalCount)
        {
            try
            {
                int pageNumber = jtStartIndex > 0 ? jtStartIndex : 1;
                if (pageNumber > 1) //if not first page                
                    pageNumber = (jtStartIndex / jtPageSize) + 1;

                var filter = new PRA_LN_DIST_WISE_DISB_ReportSearchFilter
                {
                    OrgCode = LoggedInOrganizationCode,
                    MNYR = mnyr ?? string.Empty,
                    MFI_District_Code = mfi_District_Code ?? string.Empty,
                    MFI_Thana_Code = mfi_Thana_Code ?? string.Empty,
                    SYNCED_STATUS = syncedStatus ?? string.Empty,
                    IsCalculateTotal = isCalculateTotal,
                    PageNumber = pageNumber,
                    PageSize = jtPageSize
                };

                var filtedListing = await olrSHubService.Get_PRA_LN_DIST_WISE_DISB_By_Filter(filter);

                if (filtedListing.Any() && isCalculateTotal)
                    totalCount = filtedListing[0].TotalCount;

                return Json(new { Result = "OK", Records = filtedListing, TotalRecordCount = totalCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public async Task<ActionResult> UpdateUpazillaLoan(string po_code, string mnyr, decimal cum_disb, decimal member,
            decimal borrower, decimal loan_fy, decimal cum_borr, decimal cum_disb_hdn, decimal member_hdn,
            decimal borrower_hdn, decimal loan_fy_hdn, decimal cum_borr_hdn)
        {
            try
            {
                 
                var parameter = new PRA_LN_DIST_WISE_DISB_Model
                {
                    PO_CODE     =   po_code ?? string.Empty ,
                    MNYR        =   mnyr ?? string.Empty,
                    CUM_DISB    =   cum_disb,
                    MEMBER      =   member,
                    BORROWER    =   borrower,
                    LOAN_FY     =   loan_fy,
                    CUM_BORR    =   cum_borr,

                    CUM_DISB_HDN = cum_disb_hdn,
                    MEMBER_HDN = member_hdn,
                    BORROWER_HDN = borrower_hdn,
                    LOAN_FY_HDN = loan_fy_hdn,
                    CUM_BORR_HDN = cum_borr_hdn,

                };

                var filtedListing = await olrSHubService.Update_PRA_LN_DIST_WISE_DISB(parameter);
                return Json(new { Result = "Data Update Successfully.", Records = filtedListing, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }






        #endregion

        #region Accounting Data
        [HttpGet]
        public ActionResult AccountingData()
        {
            var model = new PO_MONTHLY_ACC_BAL_ReportSearchFilter();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetAccountingData(int jtStartIndex, int jtPageSize, string jtSorting,
            string filterValue, string mnyr, string accGroup, string syncedStatus, bool isCalculateTotal, int totalCount, string searchTerm)
        {
            try
            {
                int pageNumber = jtStartIndex > 0 ? jtStartIndex : 1;
                if (pageNumber > 1) //if not first page                
                    pageNumber = (jtStartIndex / jtPageSize) + 1;

                var filter = new PO_MONTHLY_ACC_BAL_ReportSearchFilter
                {
                    OrgCode = LoggedInOrganizationCode,
                    MNYR = mnyr ?? string.Empty,
                    ACCGROUP = accGroup ?? string.Empty,
                    SYNCED_STATUS = syncedStatus ?? string.Empty,
                    IsCalculateTotal = isCalculateTotal,
                    PageNumber = pageNumber,
                    PageSize = jtPageSize,
                    SearchTerm = searchTerm,
                };

                var filtedListing = await olrSHubService.Get_PO_MONTHLY_ACC_BAL_By_Filter(filter);

                if (filtedListing.Any() && isCalculateTotal)
                    totalCount = filtedListing[0].TotalCount;

                return Json(new { Result = "OK", Records = filtedListing, TotalRecordCount = totalCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAccountingDataSummary(string mnyr, string accGroup, string syncedStatus, string searchTerm)
        {
            try
            {
               
                var filter = new PO_MONTHLY_ACC_BAL_ReportSearchFilter
                {
                    OrgCode = LoggedInOrganizationCode,
                    MNYR = mnyr ?? string.Empty,
                    ACCGROUP = accGroup ?? string.Empty,
                    SYNCED_STATUS = syncedStatus ?? string.Empty,                   
                    SearchTerm = searchTerm,
                };
                
                var summary = await olrSHubService.Get_PO_MONTHLY_ACC_BAL_Summary_By_Filter(filter);
                               

                return Json(new { Result = "OK", Data = summary }, JsonRequestBehavior.AllowGet );
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteAccountingData(string MNYR, string l1_code, string l2_code, string l3_code, string l4_code, string l5_code)
        {
            try
            {
                var param = new { 
                    @MNYR = MNYR,
                    @l1_code = l1_code,
                    @l2_code = l2_code,
                    @l3_code = l3_code,
                    @l4_code = l4_code,
                    @l5_code = l5_code
                };
                ultimateReportService.GetDataWithParameter(param, "pksf.PRA_MN_RPT_TAB_XL_PD_DeleteData");
                return GetSuccessMessageResult("Scuccess! Data Deleted.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        #endregion

        #region Employment Data
        [HttpGet]
        public ActionResult EmploymentData()
        {
            var model = new PRA_EMPLOYMENT_ReportSearchFilter();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetEmploymentData(int jtStartIndex, int jtPageSize, string jtSorting,
            string filterValue, string mnyr, string employmentType, string syncedStatus, bool isCalculateTotal, int totalCount)
        {
            try
            {
                int pageNumber = jtStartIndex > 0 ? jtStartIndex : 1;
                if (pageNumber > 1) //if not first page                
                    pageNumber = (jtStartIndex / jtPageSize) + 1;

                var filter = new PRA_EMPLOYMENT_ReportSearchFilter
                {
                    OrgCode = LoggedInOrganizationCode,
                    MNYR = mnyr ?? string.Empty,
                    EmploymentType = employmentType ?? string.Empty,
                    SYNCED_STATUS = syncedStatus ?? string.Empty,
                    IsCalculateTotal = isCalculateTotal,
                    PageNumber = pageNumber,
                    PageSize = jtPageSize
                };

                var filtedListing = await olrSHubService.Get_PRA_EMPLOYMENT_By_Filter(filter);

                if (filtedListing.Any() && isCalculateTotal)
                {
                    filtedListing.ForEach(f => f.EmploymentType = filter.EmploymentType);

                    totalCount = filtedListing[0].TotalCount;
                }

                return Json(new { Result = "OK", Records = filtedListing, TotalRecordCount = totalCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region TopSheet Data

        [HttpGet]
        public ActionResult TopSheetData()
        {
            var model = new TOP_SHEET_ReportSearchFilter();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetTopSheetData(int jtStartIndex, int jtPageSize, string jtSorting,
            string filterValue, string mnyr,  string syncedStatus, bool isCalculateTotal, int totalCount)
        {
            try
            {
                int pageNumber = jtStartIndex > 0 ? jtStartIndex : 1;
                if (pageNumber > 1) //if not first page                
                    pageNumber = (jtStartIndex / jtPageSize) + 1;

                var filter = new TOP_SHEET_ReportSearchFilter
                {
                    OrgCode = LoggedInOrganizationCode,
                    MNYR = mnyr ?? string.Empty,                    
                    SYNCED_STATUS = syncedStatus ?? string.Empty,
                    IsCalculateTotal = isCalculateTotal,
                    PageNumber = pageNumber,
                    PageSize = jtPageSize
                };

                var filtedListing = await olrSHubService.Get_TOP_SHEET_By_Filter(filter);

                if (filtedListing.Any() && isCalculateTotal)
                    totalCount = filtedListing[0].TotalCount;

                return Json(new { Result = "OK", Records = filtedListing, TotalRecordCount = totalCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Private Methods
        private void UpazillaLoanInit(PRA_LN_DIST_WISE_DISB_ReportSearchFilter model)
        {
            var districts = districtService.GetMany(p => p.IsActive == true).OrderBy(p => p.DistrictName);
            var districtList = districts.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.DistrictCode + " - " + p.DistrictName,
                Value = p.DistrictCode.ToString()
            });
            var disList = new List<SelectListItem>();
            disList.Add(new SelectListItem() { Text = "Any", Value = "", Selected = true });
            disList.AddRange(districtList);
            model.MFI_DistrictList = disList;
           
            var thanaList = new List<SelectListItem>();
            thanaList.Add(new SelectListItem() { Text = "Any", Value = "", Selected = true });
            model.MFI_ThanaList = thanaList;
        }
        #endregion
    }
}
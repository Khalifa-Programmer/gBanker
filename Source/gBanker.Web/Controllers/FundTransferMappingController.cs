using gBanker.Service;
using AutoMapper;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Data.CodeFirstMigration.Db;

namespace gBanker.Web.Controllers
{
    public class FundTransferMappingController : BaseController
    {
        #region Variables
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IProductService productService;
        private readonly IMemberService memberService;
        private readonly IEmployeeService employeeService;
        private readonly IFundTransferMappingService fundTransferMappingService;

        public FundTransferMappingController(IOfficeService officeService, IProductService productService, IUltimateReportService ultimateReportService, IMemberService memberService, IEmployeeService employeeService, IFundTransferMappingService fundTransferMappingService
            )
        {
            this.officeService = officeService;
            this.productService = productService;
            this.ultimateReportService = ultimateReportService;
            this.memberService = memberService;
            this.employeeService = employeeService;
            this.fundTransferMappingService = fundTransferMappingService;
        }
        #endregion

        // GET: ProductMapping
        public ActionResult Create()
        {
            FundTransferMappingViewModel model = new FundTransferMappingViewModel();

            //var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            ////var allOffice = officeService.GetAll().Where(m => m.OfficeLevel == 4 && m.OfficeID == offc_id && m.OrgID == LoggedInOrganizationID).ToList();
            //var allOffice = officeService.GetAll();
            //// var allOffice = officeService.GetById(offc_id);
            //var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            ////var viewOffice = allOffice.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.OfficeID.ToString(),
            //    Text = string.Format("{0}, {1}", x.OfficeCode.ToString(), x.OfficeName.ToString())
            //});
            //var ofc_items = new List<SelectListItem>();
            //ofc_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //ofc_items.AddRange(viewOffice);
            //model.OfficeList = ofc_items;


            return View(model);
        }
        [HttpPost]
        public ActionResult Create(FundTransferMappingViewModel model)
        {
            if (!ModelState.IsValid)
                return GetErrorMessageResult("Warning! You must fill all the required fields");

            try
            {

                if (model.FundTransferMappingId > 0)
                {
                    var entity = Mapper.Map<FundTransferMappingViewModel, FundTransferMapping>(model);

                    entity.OfficeId = model.OfficeId;
                    entity.EmployeeId = model.EmployeeId;
                 
                    fundTransferMappingService.Update(entity);
                }

                else
                {
                    var entity = Mapper.Map<FundTransferMappingViewModel, FundTransferMapping>(model);

                    entity.CreateUser = LoggedInEmployeeID.ToString();
                    entity.CreateDate = DateTime.Now;
                    fundTransferMappingService.Create(entity);
                }

                //return GetSuccessMessageResult("Data Saved Successfully");
                return Json("Data Saved Successfully");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        public ActionResult Entry()
        {
            MapDropdownValues();
            MapDropdownHeadValues();
            return View();

        }
        private void MapDropdownValues()
        {
            ViewBag.ZoneList = officeService.GetAllZoneOffice("100000", Convert.ToInt16(LoggedInOrganizationID)).Select(s => new SelectListItem() { Value = s.OfficeCode, Text = string.Format("{0} - {1}", s.OfficeCode, s.OfficeName) }).ToList();
            /// ViewBag.HeadList = officeService.GetHeadOffice("100000", Convert.ToInt16(LoggedInOrganizationID)).Select(s => new SelectListItem() { Value = s.OfficeCode, Text = string.Format("{0} - {1}", s.OfficeCode, s.OfficeName) }).ToList();
        }
        private void MapDropdownHeadValues()
        {
            ViewBag.HeadList = officeService.GetAllZoneOffice1("100000", Convert.ToInt16(LoggedInOrganizationID)).Select(s => new SelectListItem() { Value = s.OfficeCode, Text = string.Format("{0} - {1}", s.OfficeCode, s.OfficeName) }).ToList();
        }
        #region Methods
        public JsonResult GetAvailableOfficeList(string officeName)
        {
            try
            {   int sl = 1;
                List<OfficeViewModel> List_OfficeViewModel = new List<OfficeViewModel>();

                var officeList = officeService.GetAll().Where(m => m.IsActive && m.OfficeLevel == 4 && m.OfficeName.Contains(officeName)).ToList();

                List_OfficeViewModel = officeList.AsEnumerable()
                    .Select(row => new OfficeViewModel
                    {
                        Sl = sl.ToString(),
                        OfficeID = row.OfficeID,
                        OfficeCode = row.OfficeCode,
                        OfficeName = row.OfficeName
                    }).ToList();
                
                return Json(List_OfficeViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAreaList(string zoneCode)
        {
            try
            {
                var areaOffices = officeService.GetAllAreaOfficeForZone("100000", zoneCode, Convert.ToInt16(LoggedInOrganizationID)).Select(s => new SelectListItem() { Value = s.OfficeCode, Text = string.Format("{0} - {1}", s.OfficeCode, s.OfficeName) }).ToList();
                return Json(new { Result = "OK", Options = areaOffices }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult LoadOffice(string zoneCode, string areaCode, string employeeCode)
        {
            try
            {
                var offices = officeService.GetAllBranchesForArea("100000", zoneCode, areaCode, Convert.ToInt16(LoggedInOrganizationID)).Select(s => new SelectListItem() { Value = s.OfficeID.ToString(), Text = string.Format("{0} - {1}", s.OfficeCode, s.OfficeName) }).ToList();
                ////var employeeOfficeMappings = employeeOfficeService.GetEmployeeOfficeMappings(employeeCode).ToList();
                //foreach (var empoff in employeeOfficeMappings)
                //{
                //    var mappingExits = offices.Where(w => w.Value == empoff.Office.OfficeID.ToString()).FirstOrDefault();
                //    if (mappingExits != null)
                //    {
                //        mappingExits.Selected = true;
                //    }
                //}
                return Json(new { Result = "OK", Options = offices }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    
    
        public ActionResult GetEmployeeList(string empid)
        {

            var employeeSessionKey = string.Format("EmployeeSessionKey{0}", empid);
            var employeeList = new List<Employee>();
            //if (Session[MemberByCenterSessionKey] != null)
            //    memberList = Session[MemberByCenterSessionKey] as List<Member>;
            //else
            //{
            var emp = employeeService.GetAll().Where(m => m.IsActive == true).ToList();
            Session[employeeSessionKey] = emp;
            employeeList = emp;
            //}
            var employees = employeeList.Where(m => string.Format("{0} - {1}", m.EmployeeCode, (string.IsNullOrEmpty(m.EmpName) ? "" : m.EmpName)).ToLower().Contains(empid.ToLower())).Select(m1 => new { m1.EmployeeID, EmployeeName = string.Format("{0} - {1}", m1.EmployeeCode, (string.IsNullOrEmpty(m1.EmpName) ? "" : m1.EmpName))}).ToList();
            return Json(employees, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOfficeList(string officeid)
        {

            var officeSessionKey = string.Format("officeSessionKey{0}", officeid);
            var officeList = new List<Office>();
       
            var emp = officeService.GetAll().Where(m => m.IsActive == true && (m.OfficeName.Contains(officeid) || m.OfficeCode.Contains(officeid))).ToList();
            Session[officeSessionKey] = emp;
            officeList = emp;
            //}
            var offices = officeList.Where(m => string.Format("{0} - {1}", m.OfficeCode, (string.IsNullOrEmpty(m.OfficeName) ? "" : m.OfficeName)).ToLower().Contains(officeid.ToLower())).Select(m1 => new { m1.OfficeID, OfficeName = string.Format("{0} - {1}", m1.OfficeCode, (string.IsNullOrEmpty(m1.OfficeName) ? "" : m1.OfficeName)) }).ToList();
            return Json(offices, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EmployeeOfficeSave(List<string> allOfficeIds, string empId)
        {
            int deaEmpId = 0;
            if (empId != "")
            {

                var empID = Convert.ToInt32(empId);
                deaEmpId = empID;
                foreach (var officeId in allOfficeIds)
                {
                    var entity = new FundTransferMapping();

                    entity.OfficeId = Convert.ToInt32(officeId);
                    entity.EmployeeId = empID;

                    entity.CreateUser = LoggedInEmployeeID.ToString();
                    entity.CreateDate = DateTime.Now;
                    try
                    {
                        fundTransferMappingService.Create(entity);
                    }
                    catch(Exception ex)
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                  
                }
                return Json(deaEmpId, JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectedOfficeList(string EmpID)
        {
            try
            {
                List<OfficeViewModel> List_OfficeInfoViewModel = new List<OfficeViewModel>();
                if (Convert.ToInt32(EmpID) > 0 && EmpID != "")
                {
                    var param = new { EmpID = Convert.ToInt32(EmpID) };
                    var officeList = ultimateReportService.GetSelectedOfficeListForMapping(param);

                    List_OfficeInfoViewModel = officeList.Tables[0].AsEnumerable()
                    .Select(row => new OfficeViewModel
                    {
                        Sl = row.Field<string>("Sl"),
                        OfficeID = row.Field<Int32>("OfficeId"),
                        OfficeCode = row.Field<string>("OfficeCode"),
                        OfficeName = row.Field<string>("OfficeName")
                    }).ToList();
                }
                return Json(List_OfficeInfoViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult EmpWiseOfficeDelete(List<string> OfficeIds, string EmpId)
        {
            int dealOfficeId = 0;
            if (EmpId != "")
            {

                var EmpID = Convert.ToInt32(EmpId);
                dealOfficeId = EmpID;
                foreach (var OfficeId in OfficeIds)
                {
                    var param = new { EmpId = Convert.ToInt32(EmpId), OfficeId = OfficeId };
                    var officeList = ultimateReportService.DeleteEmpOfficeMapping(param);

                }
                return Json(dealOfficeId, JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        #endregion Methods



    }// End Class
}// End Namespace
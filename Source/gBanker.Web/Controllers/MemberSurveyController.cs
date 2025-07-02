using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Utility.Enums;

namespace gBanker.Web.Controllers
{

    public class MemberSurveyController : BaseController
    {
        #region Variables
        private readonly ICenterService centerService;
        private readonly ILoanCollectionService loanCollectionService;
        private readonly ISurveyMemberMasterService surveyMemberMasterService;
        private readonly ICountryService countryService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ISurveyMemberVerificationService surveyMemberVerificationService;
        private readonly ISurveyMemberFamilyInformationService surveyMemberFamilyInformationService;
        private readonly ISurveyMemberAccomodationInformationService surveyMemberAccomodationInformationService;
        private readonly IView_SurveyMemberMasterService viewSurveyMemberMasterService;
        private readonly ISurveyMemberAssetService surveyMemberAssetService;
        private readonly ISurveyMemberExpenditureService surveyMemberExpenditureService;
        private readonly ISurveyMemberOperationwithOtherNGOInformationService surveyMemberOperationwithOtherNgoInformationService;
        private readonly ISurveyMemberFamilyEducationInformationService surveyMemberFamilyEducationInformationService;
        private readonly IInstituteTypeService instituteTypeService;
        private readonly IInstituteService instituteService;
        private readonly ISurveyKnownMemberService surveyKnownMemberService;
        private readonly IView_SurveyMemberFamilyInformationService viewSurveyMemberFamilyInformationService;
        private readonly IView_SurveyMemberAccomodationInformationService viewSurveyMemberAccomodationInformationService;
        private readonly IView_SurveyMemberAssetService viewSurveyMemberAssetService;
        private readonly IView_SurveyMemberExpenditureService viewSurveyMemberExpenditureService;
        private readonly IView_SurveyMemberOperationwithOtherNGOInformationService viewSurveyMemberOperationwithOtherNgoInformationService;
        private readonly IView_SurveyMemberFamilyEducationInformationService viewSurveyMemberFamilyEducationInformationService;
        private readonly IView_SurveyKnownMemberService viewSurveyKnownMemberService;
        public MemberSurveyController(ILoanCollectionService loanCollectionService, ICenterService centerService, ISurveyMemberMasterService surveyMemberMasterService, ICountryService countryService, IUltimateReportService ultimateReportService, ISurveyMemberVerificationService surveyMemberVerificationService, ISurveyMemberFamilyInformationService surveyMemberFamilyInformationService, ISurveyMemberAccomodationInformationService surveyMemberAccomodationInformationService, IView_SurveyMemberMasterService viewSurveyMemberMasterService, ISurveyMemberAssetService surveyMemberAssetService, ISurveyMemberExpenditureService surveyMemberExpenditureService, ISurveyMemberOperationwithOtherNGOInformationService surveyMemberOperationwithOtherNgoInformationService, ISurveyMemberFamilyEducationInformationService surveyMemberFamilyEducationInformationService, IInstituteTypeService instituteTypeService, IInstituteService instituteService, ISurveyKnownMemberService surveyKnownMemberService, IView_SurveyMemberFamilyInformationService viewSurveyMemberFamilyInformationService, IView_SurveyMemberAccomodationInformationService viewSurveyMemberAccomodationInformationService, IView_SurveyMemberAssetService viewSurveyMemberAssetService, IView_SurveyMemberExpenditureService viewSurveyMemberExpenditureService, IView_SurveyMemberOperationwithOtherNGOInformationService viewSurveyMemberOperationwithOtherNgoInformationService, IView_SurveyMemberFamilyEducationInformationService viewSurveyMemberFamilyEducationInformationService, IView_SurveyKnownMemberService viewSurveyKnownMemberService)
        {
            this.loanCollectionService = loanCollectionService;
            this.centerService = centerService;
            this.surveyMemberMasterService = surveyMemberMasterService;
            this.countryService = countryService;
            this.ultimateReportService = ultimateReportService;
            this.surveyMemberVerificationService = surveyMemberVerificationService;
            this.surveyMemberFamilyInformationService = surveyMemberFamilyInformationService;
            this.surveyMemberAccomodationInformationService = surveyMemberAccomodationInformationService;
            this.viewSurveyMemberMasterService = viewSurveyMemberMasterService;
            this.surveyMemberAssetService = surveyMemberAssetService;
            this.surveyMemberExpenditureService = surveyMemberExpenditureService;
            this.surveyMemberOperationwithOtherNgoInformationService = surveyMemberOperationwithOtherNgoInformationService;
            this.surveyMemberFamilyEducationInformationService = surveyMemberFamilyEducationInformationService;
            this.instituteTypeService = instituteTypeService;
            this.instituteService = instituteService;
            this.surveyKnownMemberService = surveyKnownMemberService;
            this.viewSurveyMemberFamilyInformationService = viewSurveyMemberFamilyInformationService;
            this.viewSurveyMemberAccomodationInformationService = viewSurveyMemberAccomodationInformationService;
            this.viewSurveyMemberAssetService = viewSurveyMemberAssetService;
            this.viewSurveyMemberExpenditureService = viewSurveyMemberExpenditureService;
            this.viewSurveyMemberOperationwithOtherNgoInformationService = viewSurveyMemberOperationwithOtherNgoInformationService;
            this.viewSurveyMemberFamilyEducationInformationService = viewSurveyMemberFamilyEducationInformationService;
            this.viewSurveyKnownMemberService = viewSurveyKnownMemberService;
        }


        private void MapDropdownForProfileById(SurveyMemberMasterViewModel model)
        {
            var MeritalStatus = new List<SelectListItem>();
            MeritalStatus.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            MeritalStatus.Add(new SelectListItem() { Text = "Married", Value = "M" });
            MeritalStatus.Add(new SelectListItem() { Text = "Unmarried", Value = "U" });
            MeritalStatus.Add(new SelectListItem() { Text = "Seperation", Value = "S" });
            model.MaritalStatusList = MeritalStatus;

            var Education = new List<SelectListItem>();
            Education.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            Education.Add(new SelectListItem() { Text = "SSC", Value = "SSC" });
            Education.Add(new SelectListItem() { Text = "HSC", Value = "HSC" });
            Education.Add(new SelectListItem() { Text = "BSC", Value = "BSC" });
            Education.Add(new SelectListItem() { Text = "MSC", Value = "MSC" });
            model.EducationList = Education;

            var Center = new List<SelectListItem>();
            Center.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            Center.Add(new SelectListItem() { Text = "Dhaka", Value = "Dhaka" });
            Center.Add(new SelectListItem() { Text = "Chittagong", Value = "CTG" });
            Center.Add(new SelectListItem() { Text = "Sylhet", Value = "SYL" });
            Center.Add(new SelectListItem() { Text = "Rangpur", Value = "RAN" });
            model.CenterList = Center;

            var OccupationId = new List<SelectListItem>();
            OccupationId.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            OccupationId.Add(new SelectListItem() { Text = "job", Value = "1" });
            OccupationId.Add(new SelectListItem() { Text = "Business", Value = "2" });
            OccupationId.Add(new SelectListItem() { Text = "Student", Value = "3" });
            model.OccupationIdList = OccupationId;
        }
        private void MapSurveyMemberFamilyInformationById(SurveyMemberMasterViewModel model)
        {
            var RelationshipId = new List<SelectListItem>();
            RelationshipId.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            RelationshipId.Add(new SelectListItem() { Text = "Father", Value = "1" });
            RelationshipId.Add(new SelectListItem() { Text = "Son", Value = "2" });
            RelationshipId.Add(new SelectListItem() { Text = "Mother", Value = "3" });
            model.RelationshipIdList = RelationshipId;

            var OccupationId = new List<SelectListItem>();
            OccupationId.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            OccupationId.Add(new SelectListItem() { Text = "job", Value = "1" });
            OccupationId.Add(new SelectListItem() { Text = "Business", Value = "2" });
            OccupationId.Add(new SelectListItem() { Text = "Student", Value = "3" });
            model.OccupationIdList = OccupationId;

            var IsEarningCapable = new List<SelectListItem>();
            IsEarningCapable.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            IsEarningCapable.Add(new SelectListItem() { Text = "Yes", Value = "true" });
            IsEarningCapable.Add(new SelectListItem() { Text = "No", Value = "false" });
            model.IsEarningCapableList = IsEarningCapable;

            var IsAnyOtherPrivateBusiness = new List<SelectListItem>();
            IsAnyOtherPrivateBusiness.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            IsAnyOtherPrivateBusiness.Add(new SelectListItem() { Text = "Yes", Value = "true" });
            IsAnyOtherPrivateBusiness.Add(new SelectListItem() { Text = "No", Value = "false" });
            model.IsAnyOtherPrivateBusinessList = IsAnyOtherPrivateBusiness;
        }

        private void MapSurveyMemberAccomodationInformationById(SurveyMemberMasterViewModel model)
        {
            var IsOwnHome = new List<SelectListItem>();
            IsOwnHome.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            IsOwnHome.Add(new SelectListItem() { Text = "Yes", Value = "true" });
            IsOwnHome.Add(new SelectListItem() { Text = "No", Value = "false" });
            model.IsOwnHomeList = IsOwnHome;

            var IsRentPaymentRegular = new List<SelectListItem>();
            IsRentPaymentRegular.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            IsRentPaymentRegular.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            IsRentPaymentRegular.Add(new SelectListItem() { Text = "No", Value = "0" });
            model.IsRentPaymentRegularList = IsRentPaymentRegular;

            var IsUseRentalMemberForLoanPurpose = new List<SelectListItem>();
            IsUseRentalMemberForLoanPurpose.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            IsUseRentalMemberForLoanPurpose.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            IsUseRentalMemberForLoanPurpose.Add(new SelectListItem() { Text = "No", Value = "0" });
            model.IsUseRentalMemberForLoanPurposeList = IsUseRentalMemberForLoanPurpose;
        }

        private void MapSurveyMemberAssetById(SurveyMemberMasterViewModel model)
        {
            var AssetId = new List<SelectListItem>();
            AssetId.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            AssetId.Add(new SelectListItem() { Text = "Table", Value = "1" });
            AssetId.Add(new SelectListItem() { Text = "desk", Value = "2" });
            model.AssetIdList = AssetId;
        }
        private void MapSurveyMemberExpenditureById(SurveyMemberMasterViewModel model)
        {
            var ExpenditureId = new List<SelectListItem>();
            ExpenditureId.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            ExpenditureId.Add(new SelectListItem() { Text = "employee salary", Value = "1" });
            ExpenditureId.Add(new SelectListItem() { Text = "office cost", Value = "2" });
            model.ExpenditureIdList = ExpenditureId;
        }
        private void MapCreateSurveyMemberNgoById(SurveyMemberMasterViewModel model)
        {
            var NGOId = new List<SelectListItem>();
            NGOId.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            NGOId.Add(new SelectListItem() { Text = "Test NGO", Value = "1" });
            NGOId.Add(new SelectListItem() { Text = "Main NGO", Value = "2" });
            model.NGOIdList = NGOId;
        }
        private void MapFamilyEducationInformationById(SurveyMemberMasterViewModel model)
        {
            var InstituteTypeId = instituteTypeService.GetAll().Where(p => p.IsActive == true);
            var vieweInstituteTypeIdLists = InstituteTypeId.Select(a => new SelectListItem()
            {
                Value = a.InstituteTypeId.ToString(),
                Text = a.InstituteTypeName
            });
            var extrapartInstituteTypeId = new List<SelectListItem>();
            extrapartInstituteTypeId.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            extrapartInstituteTypeId.AddRange(vieweInstituteTypeIdLists);
            model.InstituteTypeIdList = extrapartInstituteTypeId;


            var InstituteId = instituteService.GetAll().Where(p => p.IsActive == true);
            var vieweInstituteIdLists = InstituteId.Select(a => new SelectListItem()
            {
                Value = a.InstituteId.ToString(),
                Text = a.InstituteName
            });
            var extrapartInstituteId = new List<SelectListItem>();
            extrapartInstituteId.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            extrapartInstituteId.AddRange(vieweInstituteIdLists);
            model.InstituteIdList = extrapartInstituteId;
        }

        private void MapSurveyKnownMemberById(SurveyMemberMasterViewModel model)
        {
            var IsBloodRelated = new List<SelectListItem>();
            IsBloodRelated.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            IsBloodRelated.Add(new SelectListItem() { Text = "Yes", Value = "1" });
            IsBloodRelated.Add(new SelectListItem() { Text = "No", Value = "0" });
            model.IsBloodRelatedList = IsBloodRelated;
        }
        #endregion



        #region Methods
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {

            var model = new SurveyMemberMasterViewModel();
            MapDropdownForProfileById(model);
            MapSurveyMemberFamilyInformationById(model);
            MapSurveyMemberAccomodationInformationById(model);
            return View(model);
        }

        public ActionResult CreateBasicInfo()
        {

            var model = new SurveyMemberMasterViewModel();
            MapDropdownForProfileById(model);
            return View(model);
        }



        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult InActive()
        {
            return View();
        }





        public JsonResult SaveMemberSurvey(SurveyMemberMaster MemberSurvey)
        {
            var result = string.Empty;
            try
            {
                var entity = new SurveyMemberMaster();
                entity.SurveyId = MemberSurvey.SurveyId;
                entity.SurveyCode = MemberSurvey.SurveyCode;
                entity.Center = MemberSurvey.Center;
                entity.SurveyDate = MemberSurvey.SurveyDate;
                entity.FirstName = MemberSurvey.FirstName;
                entity.MiddleName = MemberSurvey.MiddleName;
                entity.LastName = MemberSurvey.LastName;
                entity.MemberFullName = MemberSurvey.MemberFullName;
                entity.PresentAddress = MemberSurvey.PresentAddress;
                entity.PermanentAddress = MemberSurvey.PermanentAddress;
                entity.PresentCountryId = MemberSurvey.PresentCountryId;
                entity.PermanentCountryId = MemberSurvey.PermanentCountryId;
                entity.PresentAddressPOBCode = MemberSurvey.PresentAddressPOBCode;
                entity.PermanentAddressPOBCode = MemberSurvey.PermanentAddressPOBCode;
                entity.CityzenshipId = MemberSurvey.CityzenshipId;
                entity.IsAnyRelationwithOtherNGO = MemberSurvey.IsAnyRelationwithOtherNGO;
                entity.RefereeName = MemberSurvey.RefereeName;
                entity.PlaceOfBirth = MemberSurvey.PlaceOfBirth;
                entity.BirthDate = MemberSurvey.BirthDate;
                entity.Ocupation = MemberSurvey.Ocupation;
                entity.Education = MemberSurvey.Education;
                entity.MeritalStatus = MemberSurvey.MeritalStatus;
                entity.IsActive = true;
                entity.InActiveDate = DateTime.UtcNow;
                entity.CreateDate = DateTime.UtcNow;
                entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                surveyMemberMasterService.Create(entity);
                result = "Save Successfull";
            }
            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult ListMemberSurvey(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var VMcar = viewSurveyMemberMasterService.GetAll().Where(t => t.IsActive == true).ToList();
            var currentPageRecords = VMcar.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = VMcar.LongCount(), JsonRequestBehavior.AllowGet });
        }



        public JsonResult ListSurveyMemberFamilyInformation(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var VMcar = viewSurveyMemberFamilyInformationService.GetAll().Where(t => t.IsActive == true).ToList();
            var currentPageRecords = VMcar.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = VMcar.LongCount(), JsonRequestBehavior.AllowGet });
        }



        public JsonResult ListSurveyAccomodationInformation(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var VMcar = viewSurveyMemberAccomodationInformationService.GetAll().Where(t => t.IsActive == true).ToList();
            var currentPageRecords = VMcar.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = VMcar.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult ListSurveyMemberAsset(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var VMcar = viewSurveyMemberAssetService.GetAll().Where(t => t.IsActive == true).ToList();
            var currentPageRecords = VMcar.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = VMcar.LongCount(), JsonRequestBehavior.AllowGet });
        }
        public JsonResult ListSurveyMemberExpenditure(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var VMcar = viewSurveyMemberExpenditureService.GetAll().Where(t => t.IsActive == true).ToList();
            var currentPageRecords = VMcar.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = VMcar.LongCount(), JsonRequestBehavior.AllowGet });
        }
        public JsonResult ListSurveyMemberOperationwithOtherNGO(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var VMcar = viewSurveyMemberOperationwithOtherNgoInformationService.GetAll().Where(t => t.IsActive == true).ToList();
            var currentPageRecords = VMcar.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = VMcar.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult ListSurveyMemberFamilyEducationInformation(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var VMcar = viewSurveyMemberFamilyEducationInformationService.GetAll().Where(t => t.IsActive == true).ToList();
            var currentPageRecords = VMcar.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = VMcar.LongCount(), JsonRequestBehavior.AllowGet });
        }
        public JsonResult ListSurveyKnownMember(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            var VMcar = viewSurveyKnownMemberService.GetAll().Where(t => t.IsActive == true).ToList();
            var currentPageRecords = VMcar.Skip(jtStartIndex).Take(jtPageSize);
            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = VMcar.LongCount(), JsonRequestBehavior.AllowGet });
        }

        public JsonResult InformationDeleteMemberFamilyInformation(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = surveyMemberFamilyInformationService.GetById(Id);
                model.IsActive = false;
                model.InActiveDate = DateTime.UtcNow;
                model.CreateDate = DateTime.UtcNow;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                surveyMemberFamilyInformationService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult InformationDeleteMemberAccomodationInfo(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = surveyMemberAccomodationInformationService.GetById(Id);
                model.IsActive = false;
                model.InActiveDate = DateTime.UtcNow;
                model.CreateDate = DateTime.UtcNow;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                surveyMemberAccomodationInformationService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult InformationDeleteMemberAsset(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = surveyMemberAssetService.GetById(Id);
                model.IsActive = false;
                model.InActiveDate = DateTime.UtcNow;
                model.CreateDate = DateTime.UtcNow;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                surveyMemberAssetService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult InformationDeleteMemberExpenditure(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = surveyMemberExpenditureService.GetById(Id);
                model.IsActive = false;
                model.InActiveDate = DateTime.UtcNow;
                model.CreateDate = DateTime.UtcNow;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                surveyMemberExpenditureService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult InformationDeleteMemberNgo(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = surveyMemberOperationwithOtherNgoInformationService.GetById(Id);
                model.IsActive = false;
                model.InActiveDate = DateTime.UtcNow;
                model.CreateDate = DateTime.UtcNow;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                surveyMemberOperationwithOtherNgoInformationService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult InformationDeleteEducationInformation(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = surveyMemberFamilyEducationInformationService.GetById(Id);
                model.IsActive = false;
                model.InActiveDate = DateTime.UtcNow;
                model.CreateDate = DateTime.UtcNow;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                surveyMemberFamilyEducationInformationService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult InformationDeleteKnownMember(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = surveyKnownMemberService.GetById(Id);
                model.IsActive = false;
                model.InActiveDate = DateTime.UtcNow;
                model.CreateDate = DateTime.UtcNow;
                model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                surveyKnownMemberService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult InformationDeleteMemberBasicInfo(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var model = surveyMemberMasterService.GetById(Id);
                var SID = model.SurveyId;

                var isDuplicateFamily = surveyMemberFamilyInformationService.GetAll().Where(p => p.SurveyId == SID);
                var isDuplicateAccomodation = surveyMemberAccomodationInformationService.GetAll().Where(p => p.SurveyId == SID);
                var isDuplicateAsset = surveyMemberAssetService.GetAll().Where(p => p.SurveyId == SID);
                var isDuplicateExpenditure = surveyMemberExpenditureService.GetAll().Where(p => p.SurveyId == SID);
                var isDuplicateKnownMember = surveyKnownMemberService.GetAll().Where(p => p.SurveryId == SID);
                var isDuplicatengo = surveyMemberOperationwithOtherNgoInformationService.GetAll().Where(p => p.SurveyId == SID);
                var isDuplicateEducationIn = surveyMemberFamilyEducationInformationService.GetAll().Where(p => p.SurveyId == SID);
                var isDuplicateVerification = surveyMemberVerificationService.GetAll().Where(p => p.SurveyId == SID);

                if(
                           isDuplicateFamily.Any()
                        || isDuplicateAccomodation.Any()
                        || isDuplicateAsset.Any()
                        || isDuplicateExpenditure.Any()
                        || isDuplicateKnownMember.Any()
                        || isDuplicatengo.Any()
                        || isDuplicateEducationIn.Any()
                        || isDuplicateVerification.Any()
                )
                {
                    message = "Delete Failed Because Relation with other table";
                }
                else
                {
                    model.IsActive = false;
                    model.InActiveDate = DateTime.UtcNow;
                    model.CreateDate = DateTime.UtcNow;
                    model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberMasterService.Update(model);
                    result = 1;
                    message = "Deleted Successfully";
                }

            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }





        public ActionResult CreateSurveyMemberFamilyInformation(string sCode)
        {
            var model = new SurveyMemberMasterViewModel();
            model.SurveyCode = sCode;
            MapSurveyMemberFamilyInformationById(model);
            return View(model);
        }
        public ActionResult CreateAccomodationInfo(string sCode)
        {
            var model = new SurveyMemberMasterViewModel();
            model.SurveyCode = sCode;
            MapSurveyMemberAccomodationInformationById(model);
            return View(model);
        }

        public ActionResult CreateSurveyMemberAsset(string sCode)
        {
            var model = new SurveyMemberMasterViewModel();
            model.SurveyCode = sCode;
            MapSurveyMemberAssetById(model);
            return View(model);
        }

        public ActionResult CreateSurveyMemberExpenditure(string sCode)
        {
            var model = new SurveyMemberMasterViewModel();
            model.SurveyCode = sCode;
            MapSurveyMemberExpenditureById(model);
            return View(model);
        }
        public ActionResult CreateSurveyMemberNgo(string sCode)
        {
            var model = new SurveyMemberMasterViewModel();
            model.SurveyCode = sCode;
            MapCreateSurveyMemberNgoById(model);
            return View(model);
        }
        public ActionResult CreateFamilyEducationInformation(string sCode)
        {
            var model = new SurveyMemberMasterViewModel();
            model.SurveyCode = sCode;
            MapFamilyEducationInformationById(model);
            return View(model);
        }

        public ActionResult CreateSurveyKnownMember(string sCode)
        {
            var model = new SurveyMemberMasterViewModel();
            model.SurveyCode = sCode;
            MapSurveyKnownMemberById(model);
            return View(model);
        }

        public JsonResult GetsCodeWiseId(string sCode)
        {
            var result = "";
            int RelationshipId = 0;
            int NoOfPerson = 0;
            int OccupationId = 0;
            bool IsEarningCapable = false;
            var IncomeMonthly = "";
            bool IsAnyOtherPrivateBusiness = false;
            var IncomeMonthlyFromPrivateBusiness = "";
            var SurveyId = "";
            long SurveyFamilyId = 0;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var detail =
                    surveyMemberFamilyInformationService.GetAll()
                        .Where(p => p.SurveyId == codetoid.SurveyId)
                        .FirstOrDefault();
                if (detail != null)
                {
                    result = detail.Remarks;
                    RelationshipId = detail.RelationshipId;
                    NoOfPerson = detail.NoOfPerson;
                    OccupationId = detail.OccupationId;
                    IsEarningCapable = detail.IsEarningCapable;
                    IncomeMonthly = Convert.ToString(detail.IncomeMonthly);
                    IsAnyOtherPrivateBusiness = detail.IsAnyOtherPrivateBusiness;
                    IncomeMonthlyFromPrivateBusiness = Convert.ToString(detail.IncomeMonthlyFromPrivateBusiness);
                    SurveyId = Convert.ToString(detail.SurveyId);
                    SurveyFamilyId = detail.SurveyFamilyId;
                }

            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return Json(new
            {
                result = result,
                RelationshipId = RelationshipId,
                NoOfPerson = NoOfPerson,
                OccupationId = OccupationId,
                IsEarningCapable = IsEarningCapable,
                IncomeMonthly = IncomeMonthly,
                IsAnyOtherPrivateBusiness = IsAnyOtherPrivateBusiness,
                IncomeMonthlyFromPrivateBusiness = IncomeMonthlyFromPrivateBusiness,
                SurveyFamilyId = SurveyFamilyId
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetsCodeWiseIdforAccomodationInfo(string sCode)
        {
            var result = "";
            bool IsOwnHome = false;
            var ResidenceAddress = "";
            var ResideFrom = "";
            var ResideTo = "";
            var HomeOwnerName = "";
            var IsRentPaymentRegular = "";
            var IsUseRentalMemberForLoanPurpose = "";
            long SMAccomodationId = 0;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var detail =
                    surveyMemberAccomodationInformationService.GetAll()
                        .Where(p => p.SurveyId == codetoid.SurveyId)
                        .FirstOrDefault();
                if (detail != null)
                {
                    IsOwnHome = detail.IsOwnHome;
                    ResidenceAddress = detail.ResidenceAddress;
                    ResideFrom = Convert.ToString(detail.ResideFrom);
                    ResideTo = Convert.ToString(detail.ResideTo);
                    HomeOwnerName = detail.HomeOwnerName;
                    IsRentPaymentRegular = Convert.ToString(detail.IsRentPaymentRegular);
                    IsUseRentalMemberForLoanPurpose = Convert.ToString(detail.IsUseRentalMemberForLoanPurpose);
                    SMAccomodationId = detail.SMAccomodationId;
                }

            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return Json(new
            {
                IsOwnHome = IsOwnHome,
                ResidenceAddress = ResidenceAddress,
                ResideFrom = ResideFrom,
                ResideTo = ResideTo,
                HomeOwnerName = HomeOwnerName,
                IsRentPaymentRegular = IsRentPaymentRegular,
                IsUseRentalMemberForLoanPurpose = IsUseRentalMemberForLoanPurpose,
                SMAccomodationId = SMAccomodationId
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetsCodeWiseIdforSurveyMemberAsset(string sCode)
        {
            var result = "";
            var AssetId = "";
            var AssetAmount = "";
            var Remarks = "";
            long SMAssetId = 0;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var detail =
                    surveyMemberAssetService.GetAll()
                        .Where(p => p.SurveyId == codetoid.SurveyId)
                        .FirstOrDefault();
                if (detail != null)
                {
                    AssetId = Convert.ToString(detail.AssetId);
                    AssetAmount = Convert.ToString(detail.AssetAmount);
                    Remarks = detail.Remarks;
                    SMAssetId = detail.SMAssetId;
                }

            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return Json(new
            {
                AssetId = AssetId,
                AssetAmount = AssetAmount,
                Remarks = Remarks,
                SMAssetId = SMAssetId
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetsCodeWiseIdforSurveyMemberExpenditure(string sCode)
        {
            var result = "";
            var ExpenditureId = "";
            var ExpendetureAmount = "";
            var Remarks = "";
            long SurveyExpenditureId = 0;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var detail =
                    surveyMemberExpenditureService.GetAll()
                        .Where(p => p.SurveyId == codetoid.SurveyId)
                        .FirstOrDefault();
                if (detail != null)
                {
                    ExpenditureId = Convert.ToString(detail.ExpenditureId);
                    ExpendetureAmount = Convert.ToString(detail.ExpendetureAmount);
                    Remarks = detail.Remarks;
                    SurveyExpenditureId = detail.SurveyExpenditureId;
                }

            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return Json(new
            {
                ExpenditureId = ExpenditureId,
                ExpendetureAmount = ExpendetureAmount,
                Remarks = Remarks,
                SurveyExpenditureId = SurveyExpenditureId
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetsCodeWiseIdforSurveyMemberNgo(string sCode)
        {

            var result = "";
            var NGOId = "";
            var LoanAmount = "";
            var Remarks = "";
            long SMNGOId = 0;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var detail =
                    surveyMemberOperationwithOtherNgoInformationService.GetAll()
                        .Where(p => p.SurveyId == codetoid.SurveyId)
                        .FirstOrDefault();
                if (detail != null)
                {
                    NGOId = Convert.ToString(detail.NGOId);
                    LoanAmount = Convert.ToString(detail.LoanAmount);
                    Remarks = detail.Remarks;
                    SMNGOId = detail.SMNGOId;
                }

            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return Json(new
            {
                NGOId = NGOId,
                LoanAmount = LoanAmount,
                Remarks = Remarks,
                SMNGOId = SMNGOId
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetsCodeWiseIdforSurveyMemberFamilyEducationInformation(string sCode)
        {
            var result = "";
            var SurveyFamilyId = "";
            var InstituteId = "";
            var ClassName = "";
            var DateFrom = "";
            var DateTo = "";
            var Remarks = "";
            long SMEducationId = 0;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var detail =
                    surveyMemberFamilyEducationInformationService.GetAll()
                        .Where(p => p.SurveyId == codetoid.SurveyId)
                        .FirstOrDefault();
                if (detail != null)
                {
                    SurveyFamilyId = Convert.ToString(detail.SurveyFamilyId);
                    InstituteId = Convert.ToString(detail.InstituteId);
                    ClassName = Convert.ToString(detail.ClassName);
                    DateFrom = Convert.ToString(detail.DateFrom);
                    DateTo = Convert.ToString(detail.DateTo);
                    Remarks = detail.Remarks;
                    SMEducationId = detail.SMEducationId;
                }

            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return Json(new
            {
                SurveyFamilyId = SurveyFamilyId,
                InstituteId = InstituteId,
                ClassName = ClassName,
                DateFrom = DateFrom,
                DateTo = DateTo,
                Remarks = Remarks,
                SMEducationId = SMEducationId
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SurveyKnownMemberByMemberCode(string MemberCode)
        {
            var result = string.Empty;
            try
            {
                var param = new { @MemberCode = MemberCode };
                var AutoChequeNumber =
                    ultimateReportService.GetDataWithParameter(param, "SP_SurveyKnownMemberByMemberCode");
                var AutoChequeNumberList = AutoChequeNumber.Tables[0].AsEnumerable().Select(row => new SurveyMemberMasterViewModel
                {
                    Name = row.Field<string>("FirstName"),
                    Address = row.Field<string>("AddressLine1"),
                    Contact = row.Field<string>("PhoneNo"),
                    MemberCode = row.Field<string>("MemberCode")
                }).ToList();
                return Json(new { AutoChequeNumberList = AutoChequeNumberList, result = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetsCodeWiseIdforSurveyKnownMember(string sCode)
        {

            var result = "";
            var IsBloodRelated = "";
            var MemberCode = "";
            var Remarks = "";
            long KnownMemberId = 0;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var detail =
                    surveyKnownMemberService.GetAll()
                        .Where(p => p.SurveryId == codetoid.SurveyId)
                        .FirstOrDefault();
                if (detail != null)
                {
                    IsBloodRelated = Convert.ToString(detail.IsBloodRelated);
                    MemberCode = Convert.ToString(detail.MemberCode);
                    Remarks = detail.Remarks;
                    KnownMemberId = detail.KnownMemberId;
                }

            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return Json(new
            {
                IsBloodRelated = IsBloodRelated,
                MemberCode = MemberCode,
                Remarks = Remarks,
                KnownMemberId = KnownMemberId
            }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult SaveVarificationInfo(SurveyMemberVerification MemberSurvey)
        {
            long Data = 0;
            var result = string.Empty;
            try
            {
                var entity = new SurveyMemberVerification();
                entity.SurveyId = MemberSurvey.SurveyId;
                entity.VarificationNo = MemberSurvey.VarificationNo;
                entity.VarificationTypeId = MemberSurvey.VarificationTypeId;
                entity.VarificationDocument = MemberSurvey.VarificationDocument;
                entity.Remarks = MemberSurvey.Remarks;
                entity.IsActive = true;
                entity.InActiveDate = DateTime.UtcNow;
                entity.CreateDate = DateTime.UtcNow;
                entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                var savedEntity = surveyMemberVerificationService.Create(entity);
                result = "Save Successfull";
                Data = savedEntity.SMVerificationId;
            }
            catch (Exception ex)
            {
                result = ex.InnerException.Message.ToString();
            }

            return Json(new { result = result, data = Data }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SaveSurveyMemberFamilyInformation(SurveyMemberFamilyInformation MemberSurvey, string sCode)
        {
            var result = string.Empty;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var codetosurveyid = codetoid.SurveyId;

                var isDuplicate = surveyMemberFamilyInformationService.GetAll().Where(p => p.SurveyId == codetosurveyid);
                if (isDuplicate.Any())
                {
                    var model = surveyMemberFamilyInformationService.GetById(Convert.ToInt32(MemberSurvey.SurveyFamilyId));
                    model.SurveyFamilyId = MemberSurvey.SurveyFamilyId;
                    model.SurveyId = codetosurveyid;
                    model.RelationshipId = MemberSurvey.RelationshipId;
                    model.OccupationId = MemberSurvey.OccupationId;
                    model.NoOfPerson = MemberSurvey.NoOfPerson;
                    model.IsEarningCapable = MemberSurvey.IsEarningCapable;
                    model.IncomeMonthly = MemberSurvey.IncomeMonthly;
                    model.IsAnyOtherPrivateBusiness = MemberSurvey.IsAnyOtherPrivateBusiness;
                    model.IncomeMonthlyFromPrivateBusiness = MemberSurvey.IncomeMonthlyFromPrivateBusiness;
                    model.Remarks = MemberSurvey.Remarks;
                    model.IsActive = true;
                    model.InActiveDate = DateTime.UtcNow;
                    model.CreateDate = DateTime.UtcNow;
                    model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberFamilyInformationService.Update(model);
                    result = "Updated successfully";
                }
                else
                {
                    var entity = new SurveyMemberFamilyInformation();
                    entity.SurveyId = codetosurveyid;
                    entity.RelationshipId = MemberSurvey.RelationshipId;
                    entity.OccupationId = MemberSurvey.OccupationId;
                    entity.NoOfPerson = MemberSurvey.NoOfPerson;
                    entity.IsEarningCapable = MemberSurvey.IsEarningCapable;
                    entity.IncomeMonthly = MemberSurvey.IncomeMonthly;
                    entity.IsAnyOtherPrivateBusiness = MemberSurvey.IsAnyOtherPrivateBusiness;
                    entity.IncomeMonthlyFromPrivateBusiness = MemberSurvey.IncomeMonthlyFromPrivateBusiness;
                    entity.Remarks = MemberSurvey.Remarks;
                    entity.IsActive = true;
                    entity.InActiveDate = DateTime.UtcNow;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberFamilyInformationService.Create(entity);
                    result = "Save Successfull";
                }

            }
            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveSurveyMemberAccomodationInformation(SurveyMemberAccomodationInformation MemberSurvey, string sCode)
        {
            var result = string.Empty;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var codetosurveyid = codetoid.SurveyId;

                var isDuplicate = surveyMemberAccomodationInformationService.GetAll().Where(p => p.SurveyId == codetosurveyid);
                if (isDuplicate.Any())
                {
                    var model = surveyMemberAccomodationInformationService.GetById(Convert.ToInt32(MemberSurvey.SMAccomodationId));
                    model.SMAccomodationId = MemberSurvey.SMAccomodationId;
                    model.SurveyId = codetosurveyid;
                    model.IsOwnHome = MemberSurvey.IsOwnHome;
                    model.ResidenceAddress = MemberSurvey.ResidenceAddress;
                    model.ResideFrom = MemberSurvey.ResideFrom;
                    model.ResideTo = MemberSurvey.ResideTo;
                    model.HomeOwnerName = MemberSurvey.HomeOwnerName;
                    model.HomeOwnerOccupationId = MemberSurvey.HomeOwnerOccupationId;
                    model.IsRentPaymentRegular = MemberSurvey.IsRentPaymentRegular;
                    model.IsRentPaymentRegular = MemberSurvey.IsRentPaymentRegular;
                    model.IsUseRentalMemberForLoanPurpose = MemberSurvey.IsUseRentalMemberForLoanPurpose;
                    model.IsActive = true;
                    model.InActiveDate = DateTime.UtcNow;
                    model.CreateDate = DateTime.UtcNow;
                    model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberAccomodationInformationService.Update(model);
                    result = "Updated successfully";
                }
                else
                {
                    var entity = new SurveyMemberAccomodationInformation();
                    entity.SurveyId = codetosurveyid;
                    entity.IsOwnHome = MemberSurvey.IsOwnHome;
                    entity.ResidenceAddress = MemberSurvey.ResidenceAddress;
                    entity.ResideFrom = MemberSurvey.ResideFrom;
                    entity.ResideTo = MemberSurvey.ResideTo;
                    entity.HomeOwnerName = MemberSurvey.HomeOwnerName;
                    entity.HomeOwnerOccupationId = MemberSurvey.HomeOwnerOccupationId;
                    entity.IsRentPaymentRegular = MemberSurvey.IsRentPaymentRegular;
                    entity.IsUseRentalMemberForLoanPurpose = MemberSurvey.IsUseRentalMemberForLoanPurpose;
                    entity.IsActive = true;
                    entity.InActiveDate = DateTime.UtcNow;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberAccomodationInformationService.Create(entity);
                    result = "Save Successfull";
                }

            }
            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SaveSurveyMemberAsset(SurveyMemberAsset MemberSurvey, string sCode)
        {
            var result = string.Empty;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var codetosurveyid = codetoid.SurveyId;

                var isDuplicate = surveyMemberAssetService.GetAll().Where(p => p.SurveyId == codetosurveyid);
                if (isDuplicate.Any())
                {
                    var model = surveyMemberAssetService.GetById(Convert.ToInt32(MemberSurvey.SMAssetId));
                    model.SMAssetId = MemberSurvey.SMAssetId;
                    model.SurveyId = codetosurveyid;
                    model.AssetId = MemberSurvey.AssetId;
                    model.AssetAmount = MemberSurvey.AssetAmount;
                    model.Remarks = MemberSurvey.Remarks;
                    model.IsActive = true;
                    model.InActiveDate = DateTime.UtcNow;
                    model.CreateDate = DateTime.UtcNow;
                    model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberAssetService.Update(model);
                    result = "Updated successfully";
                }
                else
                {
                    var entity = new SurveyMemberAsset();
                    entity.SurveyId = codetosurveyid;
                    entity.AssetId = MemberSurvey.AssetId;
                    entity.AssetAmount = MemberSurvey.AssetAmount;
                    entity.Remarks = MemberSurvey.Remarks;
                    entity.IsActive = true;
                    entity.InActiveDate = DateTime.UtcNow;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberAssetService.Create(entity);
                    result = "Save Successfull";
                }

            }
            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveSurveyMemberExpenditure(SurveyMemberExpenditure MemberSurvey, string sCode)
        {
            var result = string.Empty;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var codetosurveyid = codetoid.SurveyId;

                var isDuplicate = surveyMemberExpenditureService.GetAll().Where(p => p.SurveyId == codetosurveyid);
                if (isDuplicate.Any())
                {
                    var model = surveyMemberExpenditureService.GetById(Convert.ToInt32(MemberSurvey.SurveyExpenditureId));
                    model.SurveyExpenditureId = MemberSurvey.SurveyExpenditureId;
                    model.SurveyId = codetosurveyid;
                    model.ExpenditureId = MemberSurvey.ExpenditureId;
                    model.ExpendetureAmount = MemberSurvey.ExpendetureAmount;
                    model.Remarks = MemberSurvey.Remarks;
                    model.IsActive = true;
                    model.InActiveDate = DateTime.UtcNow;
                    model.CreateDate = DateTime.UtcNow;
                    model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberExpenditureService.Update(model);
                    result = "Updated successfully";
                }
                else
                {
                    var entity = new SurveyMemberExpenditure();
                    entity.SurveyId = codetosurveyid;
                    entity.ExpenditureId = MemberSurvey.ExpenditureId;
                    entity.ExpendetureAmount = MemberSurvey.ExpendetureAmount;
                    entity.Remarks = MemberSurvey.Remarks;
                    entity.IsActive = true;
                    entity.InActiveDate = DateTime.UtcNow;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberExpenditureService.Create(entity);
                    result = "Save Successfull";
                }

            }
            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveSurveyMemberNgo(SurveyMemberOperationwithOtherNGOInformation MemberSurvey, string sCode)
        {
            var result = string.Empty;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var codetosurveyid = codetoid.SurveyId;

                var isDuplicate = surveyMemberOperationwithOtherNgoInformationService.GetAll().Where(p => p.SurveyId == codetosurveyid);
                if (isDuplicate.Any())
                {
                    var model = surveyMemberOperationwithOtherNgoInformationService.GetById(Convert.ToInt32(MemberSurvey.SMNGOId));
                    model.SMNGOId = MemberSurvey.SMNGOId;
                    model.SurveyId = codetosurveyid;
                    model.NGOId = MemberSurvey.NGOId;
                    model.LoanAmount = MemberSurvey.LoanAmount;
                    model.Remarks = MemberSurvey.Remarks;
                    model.IsActive = true;
                    model.InActiveDate = DateTime.UtcNow;
                    model.CreateDate = DateTime.UtcNow;
                    model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberOperationwithOtherNgoInformationService.Update(model);
                    result = "Updated successfully";
                }
                else
                {
                    var entity = new SurveyMemberOperationwithOtherNGOInformation();
                    entity.SurveyId = codetosurveyid;
                    entity.NGOId = MemberSurvey.NGOId;
                    entity.LoanAmount = MemberSurvey.LoanAmount;
                    entity.Remarks = MemberSurvey.Remarks;
                    entity.IsActive = true;
                    entity.InActiveDate = DateTime.UtcNow;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberOperationwithOtherNgoInformationService.Create(entity);
                    result = "Save Successfull";
                }

            }
            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveSurveyMemberFamilyEducationInformation(SurveyMemberFamilyEducationInformation MemberSurvey, string sCode)
        {
            var result = string.Empty;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var codetosurveyid = codetoid.SurveyId;
                var familyinfoidbycodes = surveyMemberFamilyInformationService.GetAll().Where(p => p.SurveyId == codetosurveyid).FirstOrDefault();
                var familyinfoidbycode = familyinfoidbycodes.SurveyFamilyId;

                var isDuplicate = surveyMemberFamilyEducationInformationService.GetAll().Where(p => p.SurveyId == codetosurveyid);
                if (isDuplicate.Any())
                {
                    var model = surveyMemberFamilyEducationInformationService.GetById(Convert.ToInt32(MemberSurvey.SMEducationId));
                    model.SMEducationId = MemberSurvey.SMEducationId;
                    model.SurveyId = codetosurveyid;
                    model.SurveyFamilyId = familyinfoidbycode;
                    model.InstituteId = MemberSurvey.InstituteId;
                    model.ClassName = MemberSurvey.ClassName;
                    model.DateFrom = MemberSurvey.DateFrom;
                    model.DateTo = MemberSurvey.DateTo;
                    model.Remarks = MemberSurvey.Remarks;
                    model.IsActive = true;
                    model.InActiveDate = DateTime.UtcNow;
                    model.CreateDate = DateTime.UtcNow;
                    model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberFamilyEducationInformationService.Update(model);
                    result = "Updated successfully";
                }
                else
                {
                    var entity = new SurveyMemberFamilyEducationInformation();
                    entity.SurveyId = codetosurveyid;
                    entity.SurveyFamilyId = familyinfoidbycode;
                    entity.InstituteId = MemberSurvey.InstituteId;
                    entity.ClassName = MemberSurvey.ClassName;
                    entity.DateFrom = MemberSurvey.DateFrom;
                    entity.DateTo = MemberSurvey.DateTo;
                    entity.Remarks = MemberSurvey.Remarks;
                    entity.IsActive = true;
                    entity.InActiveDate = DateTime.UtcNow;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyMemberFamilyEducationInformationService.Create(entity);
                    result = "Save Successfull";
                }

            }
            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveSurveyKnownMember(SurveyKnownMember MemberSurvey, string sCode)
        {
            var result = string.Empty;
            try
            {
                var codetoid = surveyMemberMasterService.GetAll().Where(p => p.SurveyCode == sCode).FirstOrDefault();
                var codetosurveyid = codetoid.SurveyId;

                var isDuplicate = surveyKnownMemberService.GetAll().Where(p => p.SurveryId == codetosurveyid);
                if (isDuplicate.Any())
                {
                    var model = surveyKnownMemberService.GetById(Convert.ToInt32(MemberSurvey.KnownMemberId));
                    model.KnownMemberId = MemberSurvey.KnownMemberId;
                    model.SurveryId = codetosurveyid;
                    model.MemberCode = MemberSurvey.MemberCode;
                    model.IsBloodRelated = MemberSurvey.IsBloodRelated;
                    model.Remarks = MemberSurvey.Remarks;
                    model.IsActive = true;
                    model.InActiveDate = DateTime.UtcNow;
                    model.CreateDate = DateTime.UtcNow;
                    model.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyKnownMemberService.Update(model);
                    result = "Updated successfully";
                }
                else
                {
                    var entity = new SurveyKnownMember();
                    entity.SurveryId = codetosurveyid;
                    entity.MemberCode = MemberSurvey.MemberCode;
                    entity.IsBloodRelated = MemberSurvey.IsBloodRelated;
                    entity.Remarks = MemberSurvey.Remarks;
                    entity.IsActive = true;
                    entity.InActiveDate = DateTime.UtcNow;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                    surveyKnownMemberService.Create(entity);
                    result = "Save Successfull";
                }

            }
            catch (Exception ex)
            {

                result = ex.InnerException.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }




        public JsonResult GetSurveyCodeWishID(string SurveyCode)
        {
            var result = 0;
            try
            {
                var SurveyID = surveyMemberMasterService.GetSurveyCodeWishID(SurveyCode).Select(w => w.SurveyId).FirstOrDefault();
                result = 1;
                return Json(new { result = result, SurveyID = SurveyID }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { result = result }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion


        #region BirthPlaceDistrict
        public JsonResult GetCountry()
        {
            try
            {
                var countrys = countryService.GetActiveRecords();
                var countrytems = new List<SelectListItem>();
                countrytems.AddRange(countrys.Select(x => new SelectListItem
                {
                    Value = x.CountryId.ToString(),
                    Text = x.CountryName
                }));
                return Json(countrytems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBirthDivision(string id)
        {
            try
            {
                var birthDivisions = new List<SurveyMemberMasterViewModel>();
                if (id != null && id != "0")
                {
                    var param = new { @CountryID = Convert.ToInt32(id) };
                    var birthPlace = ultimateReportService.GetDataWithParameter(param, "dbo.GetBirthPlaceDivision");
                    birthDivisions = birthPlace.Tables[0].AsEnumerable()
                   .Select(row => new SurveyMemberMasterViewModel
                   {
                       BirthDivisionName = row.Field<string>("DivisionName"),
                       BirthDivisionCode = row.Field<string>("DivisionCode"),

                   }).ToList();
                    var birthPlaceItems = new List<SelectListItem>();
                    birthPlaceItems.AddRange(birthDivisions.Select(x => new SelectListItem
                    {
                        Value = x.BirthDivisionCode.ToString(),
                        Text = x.BirthDivisionName
                    }));
                    return Json(birthPlaceItems, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBirthDistrict(string id)
        {
            try
            {
                var birthDistricts = new List<SurveyMemberMasterViewModel>();
                if (id != null && id != "0")
                {
                    var param = new { @DivisionCode = id };
                    var birthPlace = ultimateReportService.GetDataWithParameter(param, "dbo.GetBirthPlaceDistrict");

                    birthDistricts = birthPlace.Tables[0].AsEnumerable()
                   .Select(row => new SurveyMemberMasterViewModel
                   {
                       BirthDistrictName = row.Field<string>("DistrictName"),
                       BirthDistrictCode = row.Field<string>("DistrictCode"),

                   }).ToList();
                }

                var districtItems = new List<SelectListItem>();
                districtItems.AddRange(birthDistricts.Select(x => new SelectListItem
                {
                    Value = x.BirthDistrictCode.ToString(),
                    Text = x.BirthDistrictName
                }));
                return Json(districtItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBirthPlaceUnion(string id)
        {
            try
            {
                var birthPlaces = new List<SurveyMemberMasterViewModel>();
                if (id != null && id != "0")
                {
                    var param = new { @DistrictCode = id };
                    var birthPlace = ultimateReportService.GetDataWithParameter(param, "dbo.GetBirthPlaceUnion");

                    birthPlaces = birthPlace.Tables[0].AsEnumerable()
                   .Select(row => new SurveyMemberMasterViewModel
                   {
                       UnionCode = row.Field<string>("UnionCode"),
                       UnionName = row.Field<string>("UnionName")
                   }).ToList();
                }

                var unionItems = new List<SelectListItem>();
                unionItems.AddRange(birthPlaces.Select(x => new SelectListItem
                {
                    Value = x.UnionCode.ToString(),
                    Text = x.UnionName
                }));
                return Json(unionItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBirthPlaces(string id)
        {
            try
            {
                var birthPlaces = new List<SurveyMemberMasterViewModel>();
                if (id != null && id != "0")
                {
                    var param = new { @UnionCode = id };
                    var birthPlace = ultimateReportService.GetDataWithParameter(param, "dbo.GetBirthPlaceVillage");

                    birthPlaces = birthPlace.Tables[0].AsEnumerable()
                   .Select(row => new SurveyMemberMasterViewModel
                   {
                       BirthVillageCode = row.Field<string>("VillageCode"),
                       BirthVillageName = row.Field<string>("VillageName")
                   }).ToList();
                }

                var districtItems = new List<SelectListItem>();
                districtItems.AddRange(birthPlaces.Select(x => new SelectListItem
                {
                    Value = x.BirthVillageCode.ToString(),
                    Text = x.BirthVillageName
                }));
                return Json(districtItems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }



        public JsonResult RemoveDepositAccountScanCopy(long depositAccountId)
        {
            var result = 0;
            var message = "";
            try
            {
                var depositAccount = surveyMemberVerificationService.GetById(Convert.ToInt32(depositAccountId));
                if (depositAccount != null)
                {

                    depositAccount.VarificationDocument = null;
                    surveyMemberVerificationService.Update(depositAccount);
                    result = 1;
                    message = "Removed Successfully";

                }
                else
                {
                    result = 0;
                    message = "Nothing Found to Delete";
                };
            }
            catch (Exception ex)
            {
                result = 0;
                message = ex.InnerException.Message.ToString();

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult UploadGuarantorImage(HttpPostedFileBase file, long SurveyId)
        {

            var Result = 0;
            var entity = surveyMemberVerificationService.GetByGurId(SurveyId);

            if (file != null)
            {
                byte[] data = new byte[file.ContentLength];
                file.InputStream.Read(data, 0, file.ContentLength);
                entity.VarificationDocument = data;
                surveyMemberVerificationService.Update(entity);
                Result = 1;
            }
            else
            {
                //entity.EmployeeImage = null;
                //employeeService.Update(entity);
                Result = 2;
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        #endregion




    }
}
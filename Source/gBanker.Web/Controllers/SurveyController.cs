using gBanker.Data;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using System.IO;
using gBanker.Service.StoredProcedure;
using System.Web.Services;

namespace gBanker.Web.Controllers
{
    public class SurveyController : BaseController
    {

        #region Variables


        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IGeoLocationService geoLocationService;
        private readonly ICenterService centerService;
        private readonly IGroupService groupService;
        private readonly IMemberCategoryService memberCategoryService;
        private readonly IProductService productService;
        private readonly IMemberLastCodeService memberLastCodeService;
        private readonly ICountryService countryService;
        private readonly ILgVillageService lgVillageService;
        private UserManager<ApplicationUser> userManager;
        private readonly IEmployeeSPService employeeSpService;
        private readonly IAccReportService accReportService;
        private readonly ISavingSummaryService savingSummaryService;
        private readonly ISavingTrxService savingTrxService;
        private readonly IOrganizationService organizationService;
        private static DataTable dtNIDList;



        public SurveyController(IOfficeService officeService, IGeoLocationService geoLocationService, IMemberService memberService, ICenterService centerService, IGroupService groupService, IMemberCategoryService memberCategoryService, IProductService productService, IMemberLastCodeService memberLastCodeService, ICountryService countryService, ILgVillageService lgVillageService, UserManager<ApplicationUser> userManager, IEmployeeSPService ultimateReportService, IAccReportService accReportService, ISavingSummaryService savingSummaryService, ISavingTrxService savingTrxService, IOrganizationService organizationService)
        {
            this.memberService = memberService;
            this.officeService = officeService;
            this.geoLocationService = geoLocationService;
            this.centerService = centerService;
            this.groupService = groupService;
            this.memberCategoryService = memberCategoryService;
            this.productService = productService;
            this.memberLastCodeService = memberLastCodeService;
            this.countryService = countryService;
            this.lgVillageService = lgVillageService;
            this.userManager = userManager;
            this.employeeSpService = ultimateReportService;
            this.accReportService = accReportService;
            this.savingSummaryService = savingSummaryService;
            this.savingTrxService = savingTrxService;
            this.organizationService = organizationService;
        }
        #endregion

        // GET: Survey
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GET_GetNID_StatusList()
        {
            if (dtNIDList == null)
            {
                StringBuilder sb = new StringBuilder();

                var param = new { AndCondition = sb.ToString() };
                var MessageList = employeeSpService.GetNIDVerificationDataWithParameter(param, "GET_GetNIDStatusList");

                dtNIDList = new DataTable();
                dtNIDList = MessageList.Tables[0];

            }
            List<MemberViewModel> List_ViewModel = new List<MemberViewModel>();
            List_ViewModel = dtNIDList.AsEnumerable()
            .Select(row => new MemberViewModel
            {
                NIDStatusID = row.Field<int>("NIDStatusID"),
                NIDText = row.Field<string>("NIDText")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.NIDStatusID.ToString(),
                Text = x.NIDText.ToString(),
                Selected = x.NIDStatusID == 1 ? true : false                                   // Selected = x.choiceId == 1 ? true : false
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Form(string MemberId) //Load Survey Form 
        {
            ViewData["MemberId"] = MemberId;


            var memberInfo = memberService.GetByIdLong(Convert.ToInt64(MemberId));
            ViewData["MemberCode"] = memberInfo.MemberCode;
            ViewData["MemberBirthDate"] = "";
            if (memberInfo.BirthDate != null)
            {
                DateTime dt = Convert.ToDateTime(memberInfo.BirthDate);
                ViewData["MemberBirthDate"] = dt.ToString("dd-MMM-yyyy");
            }
            ViewData["Gender"] = memberInfo.Gender;
            ViewData["MemberNameBN"] = memberInfo.MemberNameBng;
            ViewData["MemberNameEn"] = memberInfo.FirstName + " " + memberInfo.LastName;
            ViewData["MemberCategoryID"] = memberInfo.MemberCategoryID;
            ViewData["AdmissionDate"] = memberInfo.JoinDate.ToString("dd-MMM-yyyy");



            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["comtype"] = items;
            /*
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["AnulipiText"] = items;
            ViewData["DepartmentNameList"] = items;
            ViewData["ZonalOfficeList"] = items;
            ViewData["ZonalAuditList"] = items;
            ViewData["ZOOfficeListBn"] = items;
            ViewData["AOOfficeListBn"] = items;
            ViewData["BOOfficeListBn"] = items;
            */

            return View();
        }// END View


        [HttpGet]
        public JsonResult GetOrganaizationInfo()// GET ORGID FROM SESSION
        {
            try
            {
                var orgInfo = organizationService.GetById((int)LoggedInOrganizationID);
                OrganizationViewModel List_EmployeeViewModel = new OrganizationViewModel();

                List_EmployeeViewModel.OrgID = orgInfo.OrgID;
                List_EmployeeViewModel.OrganizationName = orgInfo.OrganizationName;
                List_EmployeeViewModel.OrganizationCode = orgInfo.OrganizationCode;
                List_EmployeeViewModel.OrgAddress = orgInfo.OrgAddress;
                List_EmployeeViewModel.OrgLOGO = orgInfo.OrgLOGO;

                List_EmployeeViewModel.OrgLogoImage64String = Convert.ToBase64String(orgInfo.OrgLOGO);

                List<OrganizationViewModel> List_ViewModel = new List<OrganizationViewModel>();

                List_ViewModel.Add(List_EmployeeViewModel);
                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }//End of Function

        public JsonResult GetOccupationList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_Occupation");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                OccupationId = row.Field<int>("OccupationId"),
                Occupation = row.Field<string>("Occupation")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OccupationId.ToString(),
                Text = x.Occupation.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 

        public JsonResult GetGenderList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_SurveyGender");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                GenderId = row.Field<int>("GenderId"),
                Gender = row.Field<string>("Gender")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GenderId.ToString(),
                Text = x.Gender.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 

        public JsonResult GetIdentityTypeList()
        {
            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_SurveyIdentityType");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                surveyIdentityTypeId = row.Field<int>("surveyIdentityTypeId"),
                IdentityName = row.Field<string>("IdentityName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.surveyIdentityTypeId.ToString(),
                Text = x.IdentityName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 

        public JsonResult GetIdentityByCountryList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_GetIdentityByCountryList");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                IdentityByCountryId = row.Field<int>("IdentityByCountryId"),
                IdentityByCountry = row.Field<string>("IdentityByCountry")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.IdentityByCountryId.ToString(),
                Text = x.IdentityByCountry.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 

        public JsonResult GET_GetTransactionTypeList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_GetTransactionTypeList");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                transactionTypeId = row.Field<int>("transactionTypeId"),
                TransactionType = row.Field<string>("TransactionType")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.transactionTypeId.ToString(),
                Text = x.TransactionType.ToString()//+ " " + x.OfficeName.ToString()
                                                   // Selected = x.choiceId == 1 ? true : false
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function

        public JsonResult GetServiceUseReasonList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_GetServiceUsereasonList");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                ServiceUseReasonId = row.Field<int>("ServiceUseReasonId"),
                ServiceUseReason = row.Field<string>("ServiceUseReason")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ServiceUseReasonId.ToString(),
                Text = x.ServiceUseReason.ToString()//+ " " + x.OfficeName.ToString()
                                                    // Selected = x.choiceId == 1 ? true : false
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 
        public JsonResult GetOptionList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_GetChoiceList");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                choiceId = row.Field<int>("choiceId"),
                Choice = row.Field<string>("Choice")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.choiceId.ToString(),
                Text = x.Choice.ToString(),//+ " " + x.OfficeName.ToString()
                Selected = x.choiceId == 1 ? true : false
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 

        public JsonResult GetMarriageStatusList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_MarriageStatusList");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                MarriageStatusId = row.Field<int>("MarriageStatusId"),
                MarriageType = row.Field<string>("MarriageType")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MarriageStatusId.ToString(),
                Text = x.MarriageType.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 

        public JsonResult GetEducationStatusList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_SurveyEducationStatusList");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                EducationStatusId = row.Field<int>("EducationStatusId"),
                EducationDegree = row.Field<string>("EducationDegree")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.EducationStatusId.ToString(),
                Text = x.EducationDegree.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 

        public JsonResult GetHouseTypeList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_GetHouseType");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                HouseTypeId = row.Field<int>("HouseTypeId"),
                HouseType = row.Field<string>("HouseType")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.HouseTypeId.ToString(),
                Text = x.HouseType.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 

        public JsonResult GetHouseLocationList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_GetHouseLocation");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                HouseLocationId = row.Field<int>("HouseLocationId"),
                HouseLocation = row.Field<string>("HouseLocation")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.HouseLocationId.ToString(),
                Text = x.HouseLocation.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 

        public JsonResult GetCategoryList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_MemberCategory");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                MemberCategoryID = row.Field<int>("MemberCategoryID"),
                CategoryName = row.Field<string>("CategoryName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MemberCategoryID.ToString(),
                Text = x.CategoryName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function

        public JsonResult CreateMemberProfile(
                          string MemberId = "",
                          string MemberNameBn = "",
                          string MemberNameEn = "",
                          string FatherNameBn = "",
                          string FatherNameEn = "",
                          string MotherNameEn = "",
                          string MotherNameBn = "",
                          string MaritialStatus = "",
                          string SpouseNameBn = "",
                          string SpouseNameEn = "",
                          string MemberBirthDate = "",
                          string MemberOccupation = "",
                          string MemberGender = "",
                          string MemberNationality = "",
                          string MemberIdentityType = "",
                          string IdentityNumber = "",
                          string IdentityValidationDate = "",
                          string IdentityProvidedByCountry = "",
                          string EducationalQualification = "",
                          string ContactNoSecond = "",
                          string ContactNo = "",
                          string ContactNoOnReq = "",
                          string HouseNo = "",
                          string DakGhar = "",
                          string Ward = "",
                          string PostCode = "",
                          string txtUnion = "",
                          string txtThana = "",
                          string txtTown = "",
                          string txtCountry = "",
                          string HouseTypeId = "",
                          string HouseLocationId = "",
                          string NationalID = "",
                          string SmartCard = "",
                          string SurveyDate = "",

                          string PerCountryID = "",
                          string PerDivisionCode = "",
                          string PerDistrictCode = "",
                          string PerUpozillaCode = "",
                          string PerUnionCode = "",
                          string PerVillageCode = "",
                          string PerAddressLine1 = "",


                          string ddlActuPerCountryID    = "",
                          string ddlActuDivisionCode    = "",
                          string ddlActuDistrictCode    = "",
                          string ddlActuUpozillaCode    = "",
                          string ddlActuUnionCode       = "",
                          string ddlActuVillageCode     = "",
                          string txtActuPostCode        = "",
                          string txtActuDakGhar         = "",
                          string ActuAddressLine1       = ""

            ) // End of Parameter KK
        {
            string result = "Data Saved Successfully";
            try
            {
                var param = new
                {
                    MemberId = MemberId.Trim(),
                    MemberNameBn = MemberNameBn.Trim(),
                    MemberNameEn = MemberNameEn.Trim(),
                    FatherNameBn = FatherNameBn.Trim(),
                    FatherNameEn = FatherNameEn.Trim(),
                    MotherNameBn = MotherNameBn.Trim(),
                    MotherNameEn = MotherNameEn.Trim(),
                    MaritialStatus = MaritialStatus.Trim(),
                    SpouseNameBn = SpouseNameBn.Trim(),
                    SpouseNameEn = SpouseNameEn.Trim(),
                    MemberBirthDate = MemberBirthDate.Trim(),
                    MemberOccupation = MemberOccupation.Trim(),
                    MemberGender = MemberGender.Trim(),
                    MemberNationality = MemberNationality.Trim(),
                    MemberIdentityType = MemberIdentityType.Trim(),
                    IdentityNumber = IdentityNumber.Trim(),
                    IdentityValidationDate = IdentityValidationDate.Trim(),
                    IdentityProvidedByCountry = IdentityProvidedByCountry.Trim(),
                    EducationalQualification = EducationalQualification.Trim(),
                    ContactNoSecond = ContactNoSecond.Trim(),
                    ContactNo = ContactNo.Trim(),
                    ContactNoOnReq = ContactNoOnReq.Trim(),
                    HouseNo = HouseNo.Trim(),
                    DakGhar = DakGhar.Trim(),
                    Ward = Ward.Trim(),
                    PostCode = PostCode.Trim(),
                    txtUnion = txtUnion.Trim(),
                    txtThana = txtThana.Trim(),
                    txtTown = txtTown.Trim(),
                    txtCountry = txtCountry.Trim(),
                    HouseTypeId = HouseTypeId.Trim(),
                    HouseLocationId = HouseLocationId.Trim(),
                    NationalID = NationalID.Trim(),
                    SmartCard = SmartCard.Trim(),
                    SurveyDate = SurveyDate.Trim(),

                    PerCountryID = PerCountryID.Trim(),
                    PerDivisionCode = PerDivisionCode.Trim(),
                    PerDistrictCode = PerDistrictCode.Trim(),
                    PerUpozillaCode = PerUpozillaCode.Trim(),
                    PerUnionCode = PerUnionCode.Trim(),
                    PerVillageCode = PerVillageCode.Trim(),
                    PerAddressLine1 = PerAddressLine1.Trim(),

                    ddlActuPerCountryID      = ddlActuPerCountryID.Trim(),
                    ddlActuDivisionCode      = ddlActuDivisionCode.Trim(),
                    ddlActuDistrictCode      = ddlActuDistrictCode.Trim(),
                    ddlActuUpozillaCode      = ddlActuUpozillaCode.Trim(),
                    ddlActuUnionCode         = ddlActuUnionCode.Trim(),
                    ddlActuVillageCode       = ddlActuVillageCode.Trim(),
                    txtActuPostCode          = txtActuPostCode.Trim(),
                    txtActuDakGhar           = txtActuDakGhar.Trim(),
                    ActuAddressLine1         = ActuAddressLine1.Trim()

                };
                var val = employeeSpService.GetDataWithParameter(param, "SP_SET_UpdateEmployeeProfile"); // Was this SP_PR_CreateSALoanDisburseTmp

            }
            catch (Exception ex)
            {
                //Response.StatusCode = 403;
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }//End of Function.




        public JsonResult CreateMustFill2(
                          string MemberId = "",
                          string TIN2 = "",
                          string txtHouseNoRoadNo2 = "",
                          string txtDakGhar2 = "",
                          string txtWard2 = "",
                          string txtPostCode2 = "",
                          string txtUnion2 = "",
                          string txtThanaUpozela2 = "",
                          string txtTown2 = "",
                          string txtCountry2 = "",
                          string txtTaxAmount = "",
                          string txtServiceName = "",
                          string SurveyServiceUseReasonId = "",
                          string SurveyTransactionType = "",
                          string txtMemberrelativeInfo = "",
                          string txtMemberReferencedBy = "",
                          string ddlCountryID = "",
                          string ddlDivisionCode = "",
                          string ddlDistrictCode = "",
                          string ddlUpozillaCode = "",
                          string ddlUnionCode = "",
                          string ddlVillageCode = "",
                          bool ddlChoseFinService = true


            ) // End of Parameter KK
        {
            string result = "Data Saved Successfully";
            try
            {
                var param = new
                {
                    MemberId = MemberId,
                    TIN2 = TIN2,
                    txtHouseNoRoadNo2 = txtHouseNoRoadNo2,
                    txtDakGhar2 = txtDakGhar2,
                    txtWard2 = txtWard2,
                    txtPostCode2 = txtPostCode2,
                    txtUnion2 = txtUnion2,
                    txtThanaUpozela2 = txtThanaUpozela2,
                    txtTown2 = txtTown2,
                    txtCountry2 = txtCountry2,
                    txtTaxAmount = txtTaxAmount,
                    txtServiceName = txtServiceName,
                    SurveyServiceUseReasonId = SurveyServiceUseReasonId,
                    SurveyTransactionType = SurveyTransactionType,
                    txtMemberrelativeInfo = txtMemberrelativeInfo,
                    txtMemberReferencedBy = txtMemberReferencedBy,
                    ddlCountryID = ddlCountryID,
                    ddlDivisionCode = ddlDivisionCode,
                    ddlDistrictCode = ddlDistrictCode,
                    ddlUpozillaCode = ddlUpozillaCode,
                    ddlUnionCode = ddlUnionCode,
                    ddlVillageCode = ddlVillageCode,
                    ddlChoseFinService = ddlChoseFinService



                };
                var val = employeeSpService.GetDataWithParameter(param, "SP_SET_UpdateMustFill2"); // Was this SP_PR_CreateSALoanDisburseTmp

            }
            catch (Exception ex)
            {
                //Response.StatusCode = 403;
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }//End of Function.


        public JsonResult SetSurveyFamilyInfo(string MemberId = "", string MaleFamilyMember = "", string FemaleFamilyMember = "", string TotalFamilyMember = "", string FamilyHeadName = "", string AnotherMFIMember = "", string MFIName = "", string MFIOfficalLoan = "", string UnofficialLoan = "", string MFISavings = "", string CoOperativeSavings = "", string BankSavings = "", string OtherSavings = "", string NumEarningMember = "", string FamilyMonthlyIncome = "", string FamilyMonthlyExpense = "", string HouseRent = "", string FoodCost = "", string EducationMedicalExpense = "", string OtherExpense = "", string TotalExpense = "", string FamilyYearlyIncome = "", string FamilyYearlyExpense = "", string FamilyEarningSource = "", string NumChildrenStudying = "", string ChildrenEducationalInstitute = "", string ChildrenEducationalLevel = "", string ChildrenYearsOfStudy = "", string TotalLand = "", string TotalLandPrice = "", string FurniturePrice = "", string ElectronicsPrice = "", string ShopBusinessPrice = "", string OtherPropertyPrice = "", string OwnPropertyPrice = "", string BuroFamilyMemberName = "", string BuroFamilyMemberId = "", string BuroFamilyMemberRelation = "", string BuroFamilyMemberLoan = "")
        {
            string result = "OK";
            try
            {

                if (UnofficialLoan == "" || UnofficialLoan == null)
                {
                    UnofficialLoan = "0.00";
                }

                if (MFIOfficalLoan == "" || MFIOfficalLoan == null)
                {
                    MFIOfficalLoan = "0.00";
                }

                if (MFISavings == "" || MFISavings == null)
                {
                    MFISavings = "0.00";
                }

                if (CoOperativeSavings == "" || CoOperativeSavings == null)
                {
                    CoOperativeSavings = "0.00";
                }

                if (BankSavings == "" || BankSavings == null)
                {
                    BankSavings = "0.00";
                }

                if (OtherSavings == "" || OtherSavings == null)
                {
                    OtherSavings = "0.00";
                }
                if (FamilyMonthlyIncome == "" || FamilyMonthlyIncome == null)
                {
                    FamilyMonthlyIncome = "0.00";
                }
                if (FamilyMonthlyExpense == "" || FamilyMonthlyExpense == null)
                {
                    FamilyMonthlyExpense = "0.00";
                }
                if (HouseRent == "" || HouseRent == null)
                {
                    HouseRent = "0.00";
                }
                if (FoodCost == "" || FoodCost == null)
                {
                    FoodCost = "0.00";
                }

                if (EducationMedicalExpense == "" || EducationMedicalExpense == null)
                {
                    EducationMedicalExpense = "0.00";
                }
                if (OtherExpense == "" || OtherExpense == null)
                {
                    OtherExpense = "0.00";
                }

                var param = new
                {
                    MemberId = MemberId,
                    MaleFamilyMember = MaleFamilyMember,
                    FemaleFamilyMember = FemaleFamilyMember,
                    TotalFamilyMember = TotalFamilyMember,
                    FamilyHeadName = FamilyHeadName,
                    AnotherMFIMember = AnotherMFIMember,
                    MFIName = MFIName,
                    MFIOfficalLoan = MFIOfficalLoan,
                    UnofficialLoan = UnofficialLoan,
                    MFISavings = MFISavings,
                    CoOperativeSavings = CoOperativeSavings,
                    BankSavings = BankSavings,
                    OtherSavings = OtherSavings,
                    NumEarningMember = NumEarningMember,
                    FamilyMonthlyIncome = FamilyMonthlyIncome,
                    FamilyMonthlyExpense = FamilyMonthlyExpense,
                    HouseRent = HouseRent,
                    FoodCost = FoodCost,
                    EducationMedicalExpense = EducationMedicalExpense,
                    OtherExpense = OtherExpense,
                    TotalExpense = TotalExpense,
                    FamilyYearlyIncome = FamilyYearlyIncome,
                    FamilyYearlyExpense = FamilyYearlyExpense,
                    FamilyEarningSource = FamilyEarningSource,
                    NumChildrenStudying = NumChildrenStudying,
                    TotalLand = TotalLand,


                    TotalLandPrice = TotalLandPrice,
                    FurniturePrice = FurniturePrice,
                    ElectronicsPrice = ElectronicsPrice,

                    ShopBusinessPrice = ShopBusinessPrice,
                    OtherPropertyPrice = OtherPropertyPrice,
                    OwnPropertyPrice = OwnPropertyPrice,

                    BuroFamilyMemberName = BuroFamilyMemberName,
                    BuroFamilyMemberId = BuroFamilyMemberId,
                    BuroFamilyMemberRelation = BuroFamilyMemberRelation,
                    BuroFamilyMemberLoan = BuroFamilyMemberLoan
                };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_SurveyFamily");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SetSurveyHouseOwnerInfo(string MemberId = "", string HouseOwnerName = "", string HouseOwnerPhoneNo = "", string HouseOwnerProfession = "", string HouseRent = "", string HouseRentPayment = "", string HouseRentalDuration = "")
        {
            string result = "OK";
            try
            {
                var param = new
                {
                    MemberId = MemberId,
                    HouseOwnerName = HouseOwnerName,
                    HouseOwnerPhoneNo = HouseOwnerPhoneNo,
                    HouseOwnerProfession = HouseOwnerProfession,
                    HouseRent = HouseRent,
                    HouseRentPayment = HouseRentPayment,
                    HouseRentalDuration = HouseRentalDuration
                };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_SurveyHouseOwner");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetChildrenEducationInfo(
            string MemberId,
            string ChildrenEducationalInstitute,
            string ChildrenEducationalLevel,
            string ChildrenYearsOfStudy,
            string EducationInfoID = "0"


            )
        {
            string result = "OK";
            try
            {
                var param = new
                {
                    MemberId = MemberId,
                    EducationInfoID = EducationInfoID,
                    ChildrenEducationalInstitute = ChildrenEducationalInstitute,
                    ChildrenEducationalLevel = ChildrenEducationalLevel,
                    ChildrenYearsOfStudy = ChildrenYearsOfStudy
                };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_SurveyAddChildrenAcademy");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteChildrenEducationInfo(
           string MemberId,
           string EducationInfoID = "0"


           )
        {
            string result = "OK";
            try
            {
                var param = new
                {
                    MemberId = MemberId,
                    EducationInfoID = EducationInfoID,
                };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_SurveyDeleteChildrenAcademy");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEducationList(string MemberId, string SurveyEducationInfoId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (SurveyEducationInfoId != null) //"0"
                {
                    if (SurveyEducationInfoId != "0")
                        sb.Append(" AND SurveyEducationInfoId =" + SurveyEducationInfoId);

                }
                if (MemberId.Length > 2)
                {
                    sb.Append(" AND MemberId =" + MemberId);

                }



                List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_Get_Education_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new SurveyViewModel
                {
                    rowSl = row.Field<Int64>("rowSl"),
                    SurveyEducationInfoId = row.Field<Int64>("SurveyEducationInfoId"),
                    MemberId = row.Field<Int64>("MemberId"),
                    ChildrenEducationalInstitute = row.Field<string>("ChildrenEducationalInstitute"),
                    ChildrenEducationalLevel = row.Field<string>("ChildrenEducationalLevel"),
                    ChildrenYearsOfStudy = row.Field<string>("ChildrenYearsOfStudy")

                }).ToList();

                if (SurveyEducationInfoId != null && SurveyEducationInfoId != "0")
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }

                //var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                //return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });


                var detail = List_ViewModel.ToList();
                var TotCount = detail.Count();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function


        public JsonResult SetServicePhoneInfo(
       string MemberId,
       string ServiceName,
       string RegisterdPhoneNo,
       string ServicePhoneID

       )
        {
            string result = "OK";
            try
            {
                var param = new
                {
                    MemberId            = MemberId          ,
                    ServiceName         = ServiceName       ,
                    RegisterdPhoneNo    = RegisterdPhoneNo  ,
                    ServicePhoneID      = ServicePhoneID
                };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_SurveyAddServicePhone");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteServicePhoneInfo(
          string MemberId,
          string ServicePhoneInfoId = "0"
          )
        {
            string result = "OK";
            try
            {
                var param = new
                {
                    MemberId = MemberId,
                    ServicePhoneInfoId = ServicePhoneInfoId,
                };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_SurveyDeleteServicePhone");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServicePhoneList(string MemberId, string ServicePhoneInfoId, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (ServicePhoneInfoId != null) //"0"
                {
                    if (ServicePhoneInfoId != "0")
                        sb.Append(" AND ServicePhoneInfoId =" + ServicePhoneInfoId);

                }
                if (MemberId.Length > 2)
                {
                    sb.Append(" AND MemberId =" + MemberId);

                }

                List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_Get_ServicePhoneInfo_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new SurveyViewModel
                {
                    rowSl = row.Field<Int64>("rowSl"),
                    ServicePhoneID = row.Field<Int64>("ServicePhoneID"),
                    MemberId = row.Field<Int64>("MemberId"),
                    ServiceName = row.Field<string>("ServiceName"),
                    RegisterdPhoneNo = row.Field<string>("RegisterdPhoneNo"),
                 
                }).ToList();

                if (ServicePhoneInfoId != null && ServicePhoneInfoId != "0")
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }
                 
                var detail = List_ViewModel.ToList();
                var TotCount = detail.Count();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function



        [HttpGet]
        public JsonResult GetSurveyData(string MemberId)
        {
            try
            {
                List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();

                var param = new { MemberId = MemberId };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_GetSurveyData");

                string searchType = Convert.ToString(empList.Tables[0].Rows[0]["SearchType"]);

                if (searchType == "NewMember")
                {

                    if (empList.Tables[0].Rows.Count > 0)
                    {
                        List_ViewModel = empList.Tables[0].AsEnumerable()
                       .Select(row => new SurveyViewModel
                       {
                           SearchType = row.Field<string>("SearchType"),
                           MemberCode = row.Field<string>("MemberCode"),
                           BirthDate = row.Field<string>("MemberBirthDate"),
                           MemberCategoryID = row.Field<int>("MemberCategoryID"),
                           CenterName = row.Field<string>("CenterName"),
                           AdmissionDate = row.Field<string>("AdmissionDate"),
                           GenderId = row.Field<int>("Gender"),
                           MemberNameBN = row.Field<string>("MemberNameBN"),
                           MemberNameEn = row.Field<string>("MemberNameEn"),
                           
                           ContactNo = row.Field<string>("ContactNo"),
                           ContactNoOnReq = row.Field<string>("ContactNoOnReq"),
                           HouseNo = row.Field<string>("HouseNo"),
                           DakGhar = row.Field<string>("DakGhar"),
                           Ward = row.Field<string>("Ward"),
                           PostCode = row.Field<string>("PostCode"),
                           txtUnion = row.Field<string>("txtUnion"),
                           txtThana = row.Field<string>("txtThana"),
                           txtTown = row.Field<string>("txtTown"),
                           txtCountry = row.Field<string>("txtCountry"),
                           HouseTypeId = row.Field<int>("HouseTypeId"),
                           HouseLocationId = row.Field<int>("HouseLocationId"),
                           FatherNameBn = row.Field<string>("FatherNameBn"),
                           FatherNameEn = row.Field<string>("FatherNameEn"),
                           MotherNameBn = row.Field<string>("MotherNameBn"),
                           MotherNameEn = row.Field<string>("MotherNameEn"),
                           SpouseNameBn = row.Field<string>("SpouseNameBn"),
                           SpouseNameEn = row.Field<string>("SpouseNameEn"),
                           MarriageStatusId = row.Field<int>("MaritialStatus"),

                           MemberIdentityType = row.Field<int>("MemberIdentityType"),
                           IdentityNumber = row.Field<string>("IdentityNumber"),
                           NationalID = row.Field<string>("NationalID"),
                           SmartCard = row.Field<string>("SmartCard"),

                           OccupationId = row.Field<int>("EconomicActivity"),
                           IdentityValidationDate = row.Field<string>("ExpireDate"),
                           EducationalQualification = row.Field<int>("Education"),

                           PerAddressLine1 = row.Field<string>("PerAddressLine1"),
                           PerCountryID = row.Field<int?>("PerCountryID"),
                           PerDivisionCode = row.Field<string>("PerDivisionCode"),
                           PerDistrictCode = row.Field<string>("PerDistrictCode"),
                           PerUpozillaCode = row.Field<string>("PerUpozillaCode"),
                           PerUnionCode = row.Field<string>("PerUnionCode"),
                           PerVillageCode = row.Field<string>("PerVillageCode"),


                       }).ToList();
                    }
                }
                else if (searchType == "ExistMember")
                {
                    if (empList.Tables[0].Rows.Count > 0)
                    {
                        List_ViewModel = empList.Tables[0].AsEnumerable()
                       .Select(row => new SurveyViewModel
                       {
                           SearchType = row.Field<string>("SearchType"),
                           MemberCode = row.Field<string>("MemberCode"),
                           BirthDate = row.Field<string>("MemberBirthDate"),
                           MemberCategoryID = row.Field<int>("MemberCategoryID"),
                           AdmissionDate = row.Field<string>("AdmissionDate"),
                           CenterName = row.Field<string>("CenterName"),
                           MemberNameBN = row.Field<string>("MemberNameBN"),
                           MemberNameEn = row.Field<string>("MemberNameEn"),
                           FatherNameBn = row.Field<string>("FatherNameBn"),
                           FatherNameEn = row.Field<string>("FatherNameEn"),
                           MotherNameBn = row.Field<string>("MotherNameBn"),
                           MotherNameEn = row.Field<string>("MotherNameEn"),
                           MarriageStatusId = row.Field<int>("MaritialStatus"),

                           SpouseNameBn = row.Field<string>("SpouseNameBn"),
                           SpouseNameEn = row.Field<string>("SpouseNameEn"),

                           OccupationId = row.Field<int>("MemberOccupation"),
                           GenderId = row.Field<int>("Gender"),
                           MemberNationality = row.Field<int>("MemberNationality"),
                           MemberIdentityType = row.Field<int>("MemberIdentityType"),
                           IdentityNumber = row.Field<string>("IdentityNumber"),
                           IdentityValidationDate = row.Field<string>("IdentityValidationDate"),
                           IdentityProvidedByCountry = row.Field<int>("IdentityProvidedByCountry"),
                           EducationalQualification = row.Field<int>("EducationalQualification"),
                           ContactNoSecond = row.Field<string>("ContactNoSecond"),
                           ContactNo = row.Field<string>("ContactNo"),
                           ContactNoOnReq = row.Field<string>("ContactNoOnReq"),
                           HouseNo = row.Field<string>("HouseNo"),
                           DakGhar = row.Field<string>("DakGhar"),
                           Ward = row.Field<string>("Ward"),
                           PostCode = row.Field<string>("PostCode"),
                           txtUnion = row.Field<string>("txtUnion"),
                           txtThana = row.Field<string>("txtThana"),
                           txtTown = row.Field<string>("txtTown"),
                           txtCountry = row.Field<string>("txtCountry"),
                           HouseTypeId = row.Field<int>("HouseTypeId"),
                           HouseLocationId = row.Field<int>("HouseLocationId"),
                           SurveyDate = row.Field<string>("SurveyDate"),
                           NationalID = row.Field<string>("NationalID"),
                           SmartCard = row.Field<string>("SmartCard"),

                           PerAddressLine1 = row.Field<string>("PerAddressLine1"),
                           PerCountryID = row.Field<int?>("PerCountryID"),
                           PerDivisionCode = row.Field<string>("PerDivisionCode"),
                           PerDistrictCode = row.Field<string>("PerDistrictCode"),
                           PerUpozillaCode = row.Field<string>("PerUpozillaCode"),
                           PerUnionCode = row.Field<string>("PerUnionCode"),
                           PerVillageCode = row.Field<string>("PerVillageCode"),


                           PerCountryIDActu = row.Field<string>("PerCountryIDActu"),
                           PerDivisionCodeActu = row.Field<string>("PerDivisionCodeActu"),
                           PerDistrictCodeActu = row.Field<string>("PerDistrictCodeActu"),
                           PerUpozillaCodeActu = row.Field<string>("PerUpozillaCodeActu"),
                           PerUnionCodeActu = row.Field<string>("PerUnionCodeActu"),
                           PerVillageCodeActu = row.Field<string>("PerVillageCodeActu"),
                           txtActuPostCode = row.Field<string>("txtActuPostCode"),
                           txtActuDakGhar = row.Field<string>("txtActuDakGhar"),
                           ActuAddressLine1 = row.Field<string>("ActuAddressLine1"),





                       }).ToList();

                    }
                }
                else
                {
                    Response.StatusCode = 403;
                }


                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.ToString(), JsonRequestBehavior.AllowGet); }

        }

        [HttpGet]
        public JsonResult GetMustFill2Data(string MemberId)
        {
            try
            {
                List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();

                var param = new { MemberId = MemberId };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_GetMustFill2");

                if (empList.Tables[0].Rows.Count > 0)
                {
                    List_ViewModel = empList.Tables[0].AsEnumerable()
                   .Select(row => new SurveyViewModel
                   {

                       MemberId = row.Field<Int64>("MemberId"),
                       TIN2 = row.Field<string>("TIN2"),
                       txtHouseNoRoadNo2 = row.Field<string>("txtHouseNoRoadNo2"),
                       txtDakGhar2 = row.Field<string>("txtDakGhar2"),
                       txtWard2 = row.Field<string>("txtWard2"),
                       txtPostCode2 = row.Field<string>("txtPostCode2"),
                       txtUnion2 = row.Field<string>("txtUnion2"),
                       txtThanaUpozela2 = row.Field<string>("txtThanaUpozela2"),
                       txtTown2 = row.Field<string>("txtTown2"),
                       txtCountry2 = row.Field<string>("txtCountry2"),
                       txtTaxAmount = row.Field<string>("txtTaxAmount"),
                       txtServiceName = row.Field<string>("txtServiceName"),
                       SurveyServiceUseReasonId = row.Field<int>("SurveyServiceUseReasonId"),
                       SurveyTransactionType = row.Field<int>("SurveyTransactionType"),
                       txtMemberrelativeInfo = row.Field<string>("txtMemberrelativeInfo"),
                       txtMemberReferencedBy = row.Field<string>("txtMemberReferencedBy"),

                       CountryId = row.Field<int?>("CountryID"),
                       DivisionCode = row.Field<string>("DivisionCode"),
                       DistrictCode = row.Field<string>("DistrictCode"),
                       UpozillaCode = row.Field<string>("UpozillaCode"),
                       UnionCode = row.Field<string>("UnionCode"),
                       VillageCode = row.Field<string>("VillageCode"),

                       PerAddressLine1 = row.Field<string>("PerAddressLine1"),

                       IsAnyFS = row.Field<bool>("IsAnyFS"),




                   }).ToList();
                }
                else
                {
                    // Response.StatusCode = 403;
                }
                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.ToString(), JsonRequestBehavior.AllowGet); }

        }// End Must Fill

        [HttpGet]
        public JsonResult GetFamilyInfo(string MemberId)
        {

            try
            {
                List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();

                var param = new { MemberId = MemberId };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_GET_SurveyFamily");

                if (empList.Tables[0].Rows.Count > 0)
                {
                    List_ViewModel = empList.Tables[0].AsEnumerable()
                   .Select(row => new SurveyViewModel
                   {

                       MaleFamilyMember = row.Field<int>("MaleFamilyMember"),
                       FemaleFamilyMember = row.Field<int>("FemaleFamilyMember"),
                       TotalFamilyMember = row.Field<int>("TotalFamilyMember"),
                       FamilyHeadName = row.Field<string>("FamilyHeadName"),
                       AnotherMFIMember = row.Field<string>("AnotherMFIMember"),
                       MFIName = row.Field<string>("MFIName"),
                       MFIOfficalLoan = row.Field<decimal>("MFIOfficalLoan"),
                       UnofficialLoan = row.Field<decimal>("UnofficialLoan"),
                       MFISavings = row.Field<decimal>("MFISavings"),
                       CoOperativeSavings = row.Field<decimal>("CoOperativeSavings"),
                       BankSavings = row.Field<decimal>("BankSavings"),
                       OtherSavings = row.Field<decimal>("OtherSavings"),
                       NumEarningMember = row.Field<int>("NumEarningMember"),
                       FamilyMonthlyIncome = row.Field<decimal>("FamilyMonthlyIncome"),
                       FamilyMonthlyExpense = row.Field<decimal>("FamilyMonthlyExpense"),
                       HouseRent = row.Field<decimal>("HouseRent"),

                       FoodCost = row.Field<decimal>("FoodCost"),
                       EducationMedicalExpense = row.Field<decimal>("EducationMedicalExpense"),
                       OtherExpense = row.Field<decimal>("OtherExpense"),
                       TotalExpense = row.Field<decimal>("TotalExpense"),
                       FamilyYearlyIncome = row.Field<decimal>("FamilyYearlyIncome"),
                       FamilyYearlyExpense = row.Field<decimal>("FamilyYearlyExpense"),
                       FamilyEarningSource = row.Field<string>("FamilyEarningSource"),
                       NumChildrenStudying = row.Field<int>("NumChildrenStudying"),
                       TotalLand = row.Field<string>("TotalLand"),
                       TotalLandPrice = row.Field<decimal>("TotalLandPrice"),
                       FurniturePrice = row.Field<decimal>("FurniturePrice"),
                       ElectronicsPrice = row.Field<decimal>("ElectronicsPrice"),
                       ShopBusinessPrice = row.Field<decimal>("ShopBusinessPrice"),
                       OtherPropertyPrice = row.Field<decimal>("OtherPropertyPrice"),
                       OwnPropertyPrice = row.Field<decimal>("OwnPropertyPrice"),
                       BuroFamilyMemberName = row.Field<string>("BuroFamilyMemberName"),
                       BuroFamilyMemberId = row.Field<int>("BuroFamilyMemberId"),
                       BuroFamilyMemberRelation = row.Field<string>("BuroFamilyMemberRelation"),
                       BuroFamilyMemberLoan = row.Field<decimal>("BuroFamilyMemberLoan")

                   }).ToList();
                }
                else
                {
                    // Response.StatusCode = 403;
                }
                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.ToString(), JsonRequestBehavior.AllowGet); }


        }

        [HttpGet]
        public JsonResult GetHouseOwnerInfo(string MemberId)
        {

            try
            {
                List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();

                var param = new { MemberId = MemberId };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_GET_SurveyHouseOwnerInfo");

                if (empList.Tables[0].Rows.Count > 0)
                {
                    List_ViewModel = empList.Tables[0].AsEnumerable()
                   .Select(row => new SurveyViewModel
                   {
                       HouseOwnerName = row.Field<string>("HouseOwnerName"),
                       HouseOwnerPhoneNo = row.Field<string>("HouseOwnerPhoneNo"),
                       HouseOwnerProfession = row.Field<string>("HouseOwnerProfession"),
                       HouseRentPayment = row.Field<bool>("HouseRentPayment"),
                       HouseRentalDuration = row.Field<string>("HouseRentalDuration"),
                       HouseRent = row.Field<decimal>("HouseRent")

                   }).ToList();
                }
                else
                {
                    // Response.StatusCode = 403;
                }
                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.ToString(), JsonRequestBehavior.AllowGet); }


        }// END of Function


        // NXT
        public JsonResult SetSurveyProfessionInfo(string MemberId = "", string JobOrganizationName = "", string Designation = "", string JobOrganizationType = "", string MonthlySalary = "", string JobDuration = "", string JobTimeLeft = "", string JobOrgAddress = "", string JobOrgPostOfficeBox = "", string JobOrgWard = "", string JobOrgPostCode = "", string JobOrgUnion = "", string JobOrgThana = "", string JobOrgDistrict = "",
            string JobOrgCountry = "", string JobCoWorkerDetails = "",

           string ddlComCountryID = "",
           string ddlComDivisionCode = "",
           string ddlComDistrictCode = "",
           string ddlComUpozillaCode = "",
           string ddlComUnionCode = "",
           string ddlComVillageCode = "",
           string txtComPostCode = "",
           string txtComDakGhar = "",
           string ComAddressLine1 = ""




            )
        {
            string result = "OK";
            try
            {
                var param = new
                {
                    MemberId = MemberId,
                    JobOrganizationName = JobOrganizationName,
                    Designation = Designation,
                    JobOrganizationType = JobOrganizationType,
                    MonthlySalary = MonthlySalary,
                    JobDuration = JobDuration,
                    JobTimeLeft = JobTimeLeft,
                    JobOrgAddress = JobOrgAddress,
                    JobOrgPostOfficeBox = JobOrgPostOfficeBox,
                    JobOrgWard = JobOrgWard,
                    JobOrgPostCode = JobOrgPostCode,
                    JobOrgUnion = JobOrgUnion,
                    JobOrgThana = JobOrgThana,
                    JobOrgDistrict = JobOrgDistrict,
                    JobOrgCountry = JobOrgCountry,
                    JobCoWorkerDetails = JobCoWorkerDetails,

                    ddlComCountryID = ddlComCountryID,
                    ddlComDivisionCode = ddlComDivisionCode,
                    ddlComDistrictCode = ddlComDistrictCode,
                    ddlComUpozillaCode = ddlComUpozillaCode,
                    ddlComUnionCode = ddlComUnionCode,
                    ddlComVillageCode = ddlComVillageCode,
                    txtComPostCode = txtComPostCode,
                    txtComDakGhar = txtComDakGhar,
                    ComAddressLine1 = ComAddressLine1

                };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_SurveyProfession");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPofessionInfo(string MemberId)
        {
            try
            {
                List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();

                var param = new { MemberId = MemberId };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_GET_SurveyProfessionInfo");

                if (empList.Tables[0].Rows.Count > 0)
                {
                    List_ViewModel = empList.Tables[0].AsEnumerable()
                   .Select(row => new SurveyViewModel
                   {

                       JobOrganizationName = row.Field<string>("JobOrganizationName"),
                       Designation = row.Field<string>("Designation"),
                       JobOrganizationType = row.Field<string>("JobOrganizationType"),
                       MonthlySalary = row.Field<decimal>("MonthlySalary"),
                       JobDuration = row.Field<string>("JobDuration"),
                       JobTimeLeft = row.Field<string>("JobTimeLeft"),
                       JobOrgAddress = row.Field<string>("JobOrgAddress"),
                       JobOrgPostOfficeBox = row.Field<string>("JobOrgPostOfficeBox"),
                       JobOrgWard = row.Field<string>("JobOrgWard"),
                       JobOrgPostCode = row.Field<string>("JobOrgPostCode"),
                       JobOrgUnion = row.Field<string>("JobOrgUnion"),
                       JobOrgThana = row.Field<string>("JobOrgThana"),
                       JobOrgDistrict = row.Field<string>("JobOrgDistrict"),
                       JobOrgCountry = row.Field<string>("JobOrgCountry"),
                       JobCoWorkerDetails = row.Field<string>("JobCoWorkerDetails"),

                       ddlComCountryID = row.Field<int?>("ddlComCountryID"),
                       ddlComDivisionCode = row.Field<string>("ddlComDivisionCode"),
                       ddlComDistrictCode = row.Field<string>("ddlComDistrictCode"),
                       ddlComUpozillaCode = row.Field<string>("ddlComUpozillaCode"),
                       ddlComUnionCode = row.Field<string>("ddlComUnionCode"),
                       ddlComVillageCode = row.Field<string>("ddlComVillageCode"),
                       txtComPostCode = row.Field<string>("txtComPostCode"),
                       txtComDakGhar = row.Field<string>("txtComDakGhar"),
                       ComAddressLine1 = row.Field<string>("ComAddressLine1"),



                   }).ToList();
                }
                else
                {
                    // Response.StatusCode = 403;
                }
                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.ToString(), JsonRequestBehavior.AllowGet); }


        }// END of Function


        public JsonResult SetSurveyInvestmentInfo(string MemberId = "", string BusinessOrgName = "", string BusinessType = "", string BusinessStartDate = "", string BusinessOrgAddress = "", string BusinessOrgPostOfficeBox = "", string BusinessOrgWard = "", string BusinessOrgPostCode = "", string BusinessOrgUnion = "", string BusinessOrgThana = "", string BusinessOrgDistrict = "", string BusinessOrgCountry = "", string BusinessInvestment = "", string BusinessMonthlyIncome = "", string BusinessMonthlyExpense = "", string BusinessMonthlyProfit = "", string NumBusinessEmployee = "", string TradeLicenseNo = "", string TradeLicenseExpireDate = "", string ShopRentContractExpireDate = "", string BankNameAndBranch = "", string AccountHeading = "", string AccountNumber = "", string TIN = "", string CoBusinessWorkerDetails = "", string RelatedBusinessTraining = "",
           string ddlBizCountryID = ""
        , string ddlBizDivisionCode = ""
        , string ddlBizDistrictCode = ""
        , string ddlBizUpozillaCode = ""
        , string ddlBizUnionCode = ""
        , string ddlBizVillageCode = ""
        , string txtBizPostCode = ""
        , string txtBizDakGhar = ""
        , string BizAddressLine1 = ""

            )
        {
            string result = "OK";
            try
            {

                if (BusinessInvestment != null && BusinessInvestment == "")
                {
                    BusinessInvestment = "0.00";
                }

                if (BusinessMonthlyIncome != null && BusinessMonthlyIncome == "")
                {
                    BusinessMonthlyIncome = "0.00";
                }

                if (BusinessMonthlyExpense != null && BusinessMonthlyExpense == "")
                {
                    BusinessMonthlyExpense = "0.00";
                }

                if (BusinessMonthlyProfit != null && BusinessMonthlyProfit == "")
                {
                    BusinessMonthlyProfit = "0.00";
                }



                var param = new
                {
                    MemberId = MemberId,
                    BusinessOrgName = BusinessOrgName,
                    BusinessType = BusinessType,
                    BusinessStartDate = BusinessStartDate,
                    BusinessOrgAddress = BusinessOrgAddress,
                    BusinessOrgPostOfficeBox = BusinessOrgPostOfficeBox,
                    BusinessOrgWard = BusinessOrgWard,
                    BusinessOrgPostCode = BusinessOrgPostCode,
                    BusinessOrgUnion = BusinessOrgUnion,
                    BusinessOrgThana = BusinessOrgThana,
                    BusinessOrgDistrict = BusinessOrgDistrict,
                    BusinessOrgCountry = BusinessOrgCountry,
                    BusinessInvestment = BusinessInvestment,
                    BusinessMonthlyIncome = BusinessMonthlyIncome,
                    BusinessMonthlyExpense = BusinessMonthlyExpense,
                    BusinessMonthlyProfit = BusinessMonthlyProfit,
                    NumBusinessEmployee = NumBusinessEmployee,
                    TradeLicenseNo = TradeLicenseNo,
                    TradeLicenseExpireDate = TradeLicenseExpireDate,
                    ShopRentContractExpireDate = ShopRentContractExpireDate,
                    BankNameAndBranch = BankNameAndBranch,
                    AccountHeading = AccountHeading,
                    AccountNumber = AccountNumber,
                    TIN = TIN,
                    CoBusinessWorkerDetails = CoBusinessWorkerDetails,
                    RelatedBusinessTraining = RelatedBusinessTraining,
                    ddlBizCountryID = ddlBizCountryID,
                    ddlBizDivisionCode = ddlBizDivisionCode,
                    ddlBizDistrictCode = ddlBizDistrictCode,
                    ddlBizUpozillaCode = ddlBizUpozillaCode,
                    ddlBizUnionCode = ddlBizUnionCode,
                    ddlBizVillageCode = ddlBizVillageCode,
                    txtBizPostCode = txtBizPostCode,
                    txtBizDakGhar = txtBizDakGhar,
                    BizAddressLine1 = BizAddressLine1,

                };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_SurveyInvestment");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInvestorInfo(string MemberId)
        {
            try
            {
                List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();

                var param = new { MemberId = MemberId };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_GET_SurveyInvestorInfo");

                if (empList.Tables[0].Rows.Count > 0)
                {
                    List_ViewModel = empList.Tables[0].AsEnumerable()
                   .Select(row => new SurveyViewModel
                   {

                       BusinessOrgName = row.Field<string>("BusinessOrgName"),
                       BusinessType = row.Field<string>("BusinessType"),
                       BusinessStartDatemsg = row.Field<string>("BusinessStartDate"),
                       BusinessOrgAddress = row.Field<string>("BusinessOrgAddress"),
                       BusinessOrgPostOfficeBox = row.Field<string>("BusinessOrgPostOfficeBox"),
                       BusinessOrgWard = row.Field<string>("BusinessOrgWard"),
                       BusinessOrgPostCode = row.Field<string>("BusinessOrgPostCode"),
                       BusinessOrgUnion = row.Field<string>("BusinessOrgUnion"),
                       BusinessOrgThana = row.Field<string>("BusinessOrgThana"),
                       BusinessOrgDistrict = row.Field<string>("BusinessOrgDistrict"),
                       BusinessOrgCountry = row.Field<string>("BusinessOrgCountry"),
                       BusinessInvestment = row.Field<decimal>("BusinessInvestment"),
                       BusinessMonthlyIncome = row.Field<decimal>("BusinessMonthlyIncome"),
                       BusinessMonthlyExpense = row.Field<decimal>("BusinessMonthlyExpense"),
                       JobCoWorkerDetails = row.Field<string>("JobCoWorkerDetails"),
                       BusinessMonthlyProfit = row.Field<decimal>("BusinessMonthlyProfit"),
                       NumBusinessEmployee = row.Field<string>("NumBusinessEmployee"),
                       TradeLicenseNo = row.Field<string>("TradeLicenseNo"),
                       TradeLicenseExpireDatemsg = row.Field<string>("TradeLicenseExpireDate"),
                       ShopRentContractExpireDatemsg = row.Field<string>("ShopRentContractExpireDate"),
                       BankNameAndBranch = row.Field<string>("BankNameAndBranch"),
                       AccountHeading = row.Field<string>("AccountHeading"),
                       AccountNumber = row.Field<string>("AccountNumber"),
                       TIN = row.Field<string>("TIN"),
                       CoBusinessWorkerDetails = row.Field<string>("CoBusinessWorkerDetails"),

                       ddlBizCountryID = row.Field<string>("ddlBizCountryID"),
                       ddlBizDivisionCode = row.Field<string>("ddlBizDivisionCode"),
                       ddlBizDistrictCode = row.Field<string>("ddlBizDistrictCode"),
                       ddlBizUpozillaCode = row.Field<string>("ddlBizUpozillaCode"),
                       ddlBizUnionCode = row.Field<string>("ddlBizUnionCode"),
                       ddlBizVillageCode = row.Field<string>("ddlBizVillageCode"),
                       txtBizPostCode = row.Field<string>("txtBizPostCode"),
                       txtBizDakGhar = row.Field<string>("txtBizDakGhar"),
                       BizAddressLine1 = row.Field<string>("BizAddressLine1"),


                   }).ToList();
                }
                else
                {
                    // Response.StatusCode = 403;
                }
                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.ToString(), JsonRequestBehavior.AllowGet); }


        }// END of Function


        public JsonResult SetSurveyGeneralInfo(string MemberId = "", string AnotherOrgLoan = "", string LoanAmount = "", string AnotherOrgName = "", string BusinessOnInterest = "", string LoanThroughTenant = "", string LoanPaymentRoutine = "", string dofa = "", string dofaloanAmount = "")
        {
            string result = "OK";
            try
            {
                if (AnotherOrgLoan == "" || AnotherOrgLoan == null)
                {
                    AnotherOrgLoan = "0.00";
                }

                if (LoanAmount == "" || LoanAmount == null)
                {
                    LoanAmount = "0.00";
                }

                if (BusinessOnInterest == "" || BusinessOnInterest == null)
                {
                    BusinessOnInterest = "0.00";
                }





                var param = new
                {
                    MemberId = MemberId,
                    AnotherOrgLoan = AnotherOrgLoan,
                    LoanAmount = LoanAmount,
                    AnotherOrgName = AnotherOrgName,
                    BusinessOnInterest = BusinessOnInterest,
                    LoanThroughTenant = LoanThroughTenant,
                    LoanPaymentRoutine = LoanPaymentRoutine,
                    dofa        = dofa,
                    dofaloanAmount = dofaloanAmount
                };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_SurveyGeneral");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetSadharonInfo(string MemberId)
        {
            try
            {
                List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();

                var param = new { MemberId = MemberId };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_GET_SurveyGeneralInfo");

                if (empList.Tables[0].Rows.Count > 0)
                {
                    List_ViewModel = empList.Tables[0].AsEnumerable()
                   .Select(row => new SurveyViewModel
                   {

                       AnotherOrgLoan = row.Field<string>("AnotherOrgLoan"),
                       LoanAmount = row.Field<decimal>("LoanAmount"),
                       AnotherOrgName = row.Field<string>("AnotherOrgName"),
                       BusinessOnInterest = row.Field<string>("BusinessOnInterest"),
                       LoanThroughTenant = row.Field<string>("LoanThroughTenant"),
                       LoanPaymentRoutine = row.Field<string>("LoanPaymentRoutine"),
                       dofa = row.Field<int?>("dofa"),
                       dofaloanAmount = row.Field<decimal?>("dofaloanAmount"),

                   }).ToList();
                }
                else
                {
                    // Response.StatusCode = 403;
                }
                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.ToString(), JsonRequestBehavior.AllowGet); }


        }// END of Function


        [HttpPost]
        public ActionResult AddPHOTO(SurveyNomineeInfoViewModel fm)
        {
            try
            {
                //Write your database insert code / activities  

                var param = new
                {
                    MemberId = fm.CurrentMemberId,

                    NomineePhoto = fm.NomineePhotoBinary,
                    ApplicantSignature = fm.ApplicantSignatureBinary,

                };
               var empList = employeeSpService.GetDataWithParameter(param, "SP_SET_SurveyNomini_Photo");

            }
            catch(Exception ex) {

            }

            return RedirectToAction("Index");
        }

        //[WebMethod(EnableSession = true)]
        //public static void MoveImages(string imageData)
        //{
        //    string fileName = "xyz" + ".png";
        //    string pathstring = @"D:\";
        //    string destFile = Path.Combine(pathstring);
        //    string destFile1 = Path.Combine(destFile, fileName);
        //    //if (File.Exists(destFile1))
        //    //{
        //    //    File.Delete(destFile1);
        //    //}

        //    using (FileStream fs = new FileStream(destFile1, FileMode.Create))
        //    {
        //        using (BinaryWriter bw = new BinaryWriter(fs))
        //        {
        //            byte[] bytes = Convert.FromBase64String(imageData);
        //            bw.Write(bytes, 0, bytes.Length);
        //            bw.Close();
        //        }
        //    }
        //}

        public JsonResult SetSurveyNomineeInfo(string MemberId = "", string NomineeName = "", string NomineeFatherName = "", string NomineeMotherName = "", string NomineeSpouseName = "", string NomineeNID = "", string NomineeRelation = "", string NomineeDateOfBirth = "", string NomineeAddress = "", string NomineePhoto = "", string ApplicantSignature = "", string ApplicationDate = "")
        {
            string result = "OK";
            try
            {
                if (ApplicationDate == "")
                {
                    ApplicationDate = null;
                }
                if (NomineeDateOfBirth == "")
                {
                    NomineeDateOfBirth = null;
                }

                var param = new
                {
                    MemberId = MemberId,
                    NomineeName = NomineeName,
                    NomineeFatherName = NomineeFatherName,
                    NomineeMotherName = NomineeMotherName,
                    NomineeSpouseName = NomineeSpouseName,
                    NomineeNID = NomineeNID,
                    NomineeRelation = NomineeRelation,
                    NomineeDateOfBirth = NomineeDateOfBirth,
                    NomineeAddress = NomineeAddress,
                    NomineePhoto = NomineePhoto,
                    ApplicantSignature = ApplicantSignature,
                    ApplicationDate = ApplicationDate
                };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_SET_SurveyNomini");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetNominiInfo(string MemberId)
        {
            try
            {
                List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();

                var param = new { MemberId = MemberId };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_GET_SurveyNominiInfo");

                string searchType = Convert.ToString(empList.Tables[0].Rows[0]["SearchType"]);


                if (empList.Tables[0].Rows.Count > 0)
                {
                    List_ViewModel = empList.Tables[0].AsEnumerable()
                   .Select(row => new SurveyViewModel
                   {

                       NomineeName = row.Field<string>("NomineeName"),
                       NomineeFatherName = row.Field<string>("NomineeFatherName"),
                       NomineeMotherName = row.Field<string>("NomineeMotherName"),

                       NomineeSpouseName = row.Field<string>("NomineeSpouseName"),
                       NomineeNID = row.Field<string>("NomineeNID"),
                       NomineeRelation = row.Field<string>("NomineeRelation"),

                       NomineeDateOfBirth = row.Field<string>("NomineeDateOfBirth"),
                       NomineeAddress = row.Field<string>("NomineeAddress"),
                       ApplicationDate = row.Field<string>("ApplicationDate"),


                   }).ToList();

                }
                else
                {
                    Response.StatusCode = 403;
                }


                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.ToString(), JsonRequestBehavior.AllowGet); }

        }







        [HttpGet]
        public JsonResult GetNominiImageData(string MemberId)
        {
            try
            {


                var param = new { MemberId = MemberId };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_GET_SurveyNominiPhotoSignature");

                List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
                if (empList.Tables[0].Rows.Count > 0)
                {


                    List_ViewModel = empList.Tables[0].AsEnumerable()
                   .Select(row => new SurveyViewModel
                   {
                       NomineePhotoBase64 = row.Field<string>("NomineePhotoBase64"),
                       ApplicantSignatureBase64 = row.Field<string>("ApplicantSignatureBase64")

                   }).ToList();

                    foreach (var n in List_ViewModel)
                    {
                        if (n.NomineePhotoBase64 == null || n.NomineePhotoBase64 == "")
                        {
                            n.NomineePhotoBase64 = @"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";
                        }
                        if (n.ApplicantSignatureBase64 == null || n.ApplicantSignatureBase64 == "")
                        {
                            n.ApplicantSignatureBase64 = @"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";
                        }

                    }

                }

                return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.ToString(), JsonRequestBehavior.AllowGet); }

        }


        // Upload Nomini Photo
        [HttpPost]
        public ActionResult UploadFiles(string MemberId)
        {
            string path = Server.MapPath("~/CapturedImages/");
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                file.SaveAs(path + file.FileName);

                var length = file.InputStream.Length; //Length: 103050706
                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target); // generates problem in this line
                byte[] data = target.ToArray();

                string base64String = Convert.ToBase64String(data);

                DeleteUploadedFiles(path);


                decimal size = Math.Round(((decimal)file.ContentLength / (decimal)1024), 2);
                if (size > 100)
                {
                    return Json("Files Size Should Be 100KB .!");
                }





                var param = new
                {
                    @MemberId = MemberId,
                    @NomineePhoto = base64String,
                    @NomineePhotoBinary = data

                };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_SET_SurveyNominiPhoto");

                //var v = ultimateReportService.SaveSurveyPhoto(Convert.ToInt64(MemberId), base64String, data);


                //    ultimateReportService.SurveyNominiPhotoSignature(param);

            }
            //return Json(files.Count + " Files Uploaded!");
            return Json("Files Uploaded!");
        }

        // End Nomini Photo

        // Upload Applicant Signature
        [HttpPost]
        public ActionResult UploadFiles2(string MemberId)
        {
            string path = Server.MapPath("~/CapturedImages/");
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                file.SaveAs(path + file.FileName);


                var length = file.InputStream.Length; //Length: 103050706
                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target); // generates problem in this line
                byte[] data = target.ToArray();

                string base64String = Convert.ToBase64String(data);
                DeleteUploadedFiles(path);

                decimal size = Math.Round(((decimal)file.ContentLength / (decimal)1024), 2);
                if (size > 100)
                {
                    return Json("Files Size Should Be 100KB .!");
                }

                var param = new
                {
                    MemberId = MemberId,
                    ApplicantSignature = base64String,
                    ApplicantSignatureBinary = data
                };
                var empList = employeeSpService.GetDataWithParameter(param, "SP_SET_SurveyNominiSignature");

            }
            //return Json(files.Count + " Files Uploaded!");
            return Json("Files Uploaded!");
        }

        private void DeleteUploadedFiles(string path)
        {
            string[] files = Directory.GetFiles(path, ".", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                var FileInfo = new FileInfo(file);
                FileInfo.Delete();
            }

        }

        // END Applicant  Signature

        ///Load Permanent Address
        ///

        public JsonResult GetCountryList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = employeeSpService.GetDataWithParameter(param, "GET_CountryList");

            List<SurveyViewModel> List_ViewModel = new List<SurveyViewModel>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new SurveyViewModel
            {
                CountryId = row.Field<int>("CountryId"),
                CountryName = row.Field<string>("CountryName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CountryId.ToString(),
                Text = x.CountryName.ToString(), //+ " " + x.OfficeName.ToString()
                Selected = x.CountryId == 18 ? true : false
            });


            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 






        //// END Permanent Address










    }// END Class
}//END Namespace
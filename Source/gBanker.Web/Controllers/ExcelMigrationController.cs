
using OfficeOpenXml;
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Dynamic;
using System.Data.Entity.Validation;
using gBanker.Web.ViewModels;
using gBanker.Service.ReportServies;
using gBanker.Service;
using gBanker.Web.Helpers;
using System.Text;
using gBanker.Web.Utility;
using Newtonsoft.Json;

namespace gBanker.Web.Controllers
{
    public class ExcelMigrationController : BaseController
    {

        List<ValidateResultModel> WrongStaffInfoList = new List<ValidateResultModel>();
        List<ValidateResultModel> WrongCenterInfoList = new List<ValidateResultModel>();
        List<ValidateResultModel> WrongCustomerInfoList = new List<ValidateResultModel>();
        // DBDataAccess dbDataAccess = new DBDataAccess();

        private static string MyTillDate;

        private readonly IUltimateExcelService ultimateExcelService;
        private readonly IOfficeService officeService;
        private readonly IAccReportService accreportService;
        private static DataSet empList;//= new DataSet();
        private static string SpName;
        public ExcelMigrationController(IAccReportService accreportService, IUltimateExcelService ultimateExcelService, IOfficeService officeService)
        {
            this.ultimateExcelService = ultimateExcelService;
            this.officeService = officeService;
            this.accreportService = accreportService;
        }
        // GET: ExcelMigration
        public ActionResult Index()
        {

            var model = new BuroCustomerInfoViewModel();
            List<SelectListItem> items3 = new List<SelectListItem>();
            //items3.Add(new SelectListItem
            //{
            //    Text = "Please Select",
            //    Value = "0"
            //});
            ViewData["comtype"] = items3;

            var officeInfo = officeService.GetByIdLong((long)SessionHelper.LoginUserOfficeID);
            ViewData["OfficeName"] = officeInfo.OfficeCode + "-" + officeInfo.OfficeName;

            ViewData["BranchCode"] = officeInfo.OfficeCode;


            model.BranchCode = officeInfo.OfficeCode;
            model.IsSuperAdmin = false;
            model.CheckStaffDataTable = new List<dynamic>();



            var v = model.CheckStaffDataTable.Count;
            return View(model);
        }
        //txtBranchCode: txtBranchCode, ddlSPName : ddlSPName, TillDate : TillDate
        public JsonResult ExecuteSP(string txtBranchCode = "", string ddlSPName = "", string TillDate = "")
        {
            string result = "OK";
            try
            {
                SpName = ddlSPName.Trim().ToString();

                MyTillDate = TillDate;

                if (TillDate != "")
                {
                    var param2 = new
                    {
                        BranchCode = txtBranchCode,
                        dateTo = TillDate
                    };
                    empList = ultimateExcelService.GetDataWithParameter(param2, ddlSPName);
                }
                else
                {
                    var param = new
                    {
                        BranchCode = txtBranchCode
                    };
                    empList = ultimateExcelService.GetDataWithParameter(param, ddlSPName);

                }
                //  public List<dynamic> CheckStaffDataTable { get; set; }

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBranchNameList()
        {
            StringBuilder sb = new StringBuilder();

            var param = new { @UserId = SessionHelper.LoggedInEmployeeID };
            var List = ultimateExcelService.GetDataWithParameter(param, "Get_MappingBranchList");

            List<ReportSpName> List_ViewModel = new List<ReportSpName>();
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new ReportSpName
            {
                OfficeCode = row.Field<string>("OfficeCode"),
                OfficeName = row.Field<string>("OfficeName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeCode.ToString(),
                Text = x.OfficeName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 
        public JsonResult GetSPNameList()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = ultimateExcelService.GetDataWithParameter(param, "SP_Get_SPList");

            List<ReportSpName> List_ViewModel = new List<ReportSpName>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new ReportSpName
            {
                LabelName = row.Field<string>("LabelName"),
                SpName = row.Field<string>("SpName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.SpName.ToString(),
                Text = x.LabelName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 
        public ActionResult DownloadExcelJsonFormat()
        {

            return View();
        }

        //public ActionResult DownloadExcelJsonCon(string spName, string ACCOUNTINGDATE, string txtBranchCode = "")
        //{
        //    try
        //    {

        //        var FileName = "gBankerData_Con";
        //        var contracttDetailListing = new List<MRA_Contract_DetailsViewModel>();
        //        var ContractDetailsParam = new { OrgID = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, ACCOUNTINGDATE = ACCOUNTINGDATE };
        //        var ContractDetails = accreportService.GetAccDataForReport(ContractDetailsParam, "Proc_Get_ContractDetails");

        //        var listContractDetails = ContractDetails.Tables[0].AsEnumerable()
        //        .Select(row => new MRA_Contract_DetailsViewModel
        //        {
        //            DATATYPE = "C",
        //            RECORDNO = row.Field<int>("RECORDNO"),
        //            BRANCH_CODE = row.Field<string>("BRANCH_CODE"),
        //            MEMBERID = row.Field<string>("MEMBERID"),
        //            LOAN_CODE = row.Field<string>("LOAN_CODE"),
        //            LOAN_TYPE = row.Field<string>("LOAN_TYPE"),
        //            LOAN_DISBURSEMENT_DATE = row.Field<string>("LOAN_DISBURSEMENT_DATE"),
        //            END_DATE_CONTRACT = row.Field<string>("END_DATE_CONTRACT"),
        //            LAST_INSTALLMENT_PAID_DATE = row.Field<string>("LAST_INSTALLMENT_PAID_DATE"),
        //            DISBURSED_AMOUNT = row.Field<decimal>("DISBURSED_AMOUNT"),
        //            TOTAL_OUTSTANDING_AMT = row.Field<decimal?>("TOTAL_OUTSTANDING_AMT"),
        //            PERIODICITY_PAYMENT = row.Field<string>("PERIODICITY_PAYMENT"),
        //            TOTAL_NUM_INSTALLMENT = row.Field<int>("TOTAL_NUM_INSTALLMENT"),
        //            INSTALLMENT_AMT = row.Field<decimal?>("INSTALLMENT_AMT"),
        //            NUM_REMAINING_INSTALLMENT = row.Field<int?>("NUM_REMAINING_INSTALLMENT"),
        //            NUM_OVERDUE_INSTALLMENT = row.Field<decimal?>("NUM_OVERDUE_INSTALLMENT"),
        //            OVERDUE_AMT = row.Field<decimal?>("OVERDUE_AMT"),
        //            LOAN_STATUS = row.Field<string>("LOAN_STATUS"),
        //            RESCHEDULE_NO = row.Field<int>("RESCHEDULE_NO"),
        //            LAST_RESCHEDULE_DATE = row.Field<int?>("LAST_RESCHEDULE_DATE"),
        //            WRITE_OFF_AMT = row.Field<decimal>("WRITE_OFF_AMT"),
        //            WRITE_OFF_DATE = row.Field<string>("WRITE_OFF_DATE"),
        //            CONTRACT_PHASE = row.Field<string>("CONTRACT_PHASE"),
        //            LOAN_DURATION = row.Field<int>("LOAN_DURATION"),
        //            ACTUAL_END_DATE_CONTRACT = row.Field<string>("ACTUAL_END_DATE_CONTRACT"),
        //            ECONOMIC_PURPOSE_CODE = row.Field<string>("ECONOMIC_PURPOSE_CODE"),
        //            COMPULSORY_SAVING_AMT = row.Field<decimal>("COMPULSORY_SAVING_AMT"),
        //            VOLUNTARY_SAVING_AMT = row.Field<decimal>("VOLUNTARY_SAVING_AMT"),
        //            TERM_SAVING_AMT = row.Field<decimal>("TERM_SAVING_AMT"),
        //            SUBSIDIZED_CREDIT_FLAG = row.Field<string>("SUBSIDIZED_CREDIT_FLAG"),
        //            SERVICE_CHARGE_RATE = row.Field<decimal?>("SERVICE_CHARGE_RATE"),
        //            PAYMENT_MODE = row.Field<string>("PAYMENT_MODE"),
        //            ADVANCE_PAYMENT_AMT = row.Field<decimal?>("ADVANCE_PAYMENT_AMT"),
        //            LAW_SUIT = row.Field<string>("LAW_SUIT"),
        //            ME = row.Field<string>("ME"),
        //            MEMBER_WELFARE_FUND = row.Field<string>("MEMBER_WELFARE_FUND"),
        //            INSURENCE_COVERAGE = row.Field<string>("INSURENCE_COVERAGE"),

        //        }).ToList();

        //        var MFICODE = ContractDetails.Tables[0].Rows[0]["MFICode"].ToString();  //ListContractDetails.Select(l => l.MFICODE).FirstOrDefault();
        //        FileName = ContractDetails.Tables[0].Rows[0]["FileNameJSON"].ToString();
        //        ACCOUNTINGDATE = ContractDetails.Tables[0].Rows[0]["ACCOUNTINGDATE"].ToString(); //ListContractDetails.Select(l => l.ACCOUNTINGDATE).FirstOrDefault();

        //        var ProductionDate = ContractDetails.Tables[0].Rows[0]["ProductionDate"].ToString();

        //        var header = new
        //        {
        //            DATATYPE = "H",
        //            MFICODE = MFICODE,
        //            ACCOUNTINGDATE = ACCOUNTINGDATE,
        //            PRODUCTIONDATE = ProductionDate
        //        };


        //        var footer = new 
        //        {
        //            DATATYPE = "F",
        //            TOTALRECORD = listContractDetails.Count.ToString()
        //        };

        //        if (listContractDetails.Any())
        //            contracttDetailListing.AddRange(listContractDetails);

        //        //let's serial as json formatted data 
        //        var jsonStringHeader = JsonConvert.SerializeObject(header);
        //        var jsonStringFooter = JsonConvert.SerializeObject(footer);

        //        var jsonStringDeatils = JsonConvert.SerializeObject(contracttDetailListing);
        //            jsonStringDeatils = jsonStringDeatils.Replace("[", "");
        //            jsonStringDeatils = jsonStringDeatils.Replace("]", "");

        //        var jsonStringFinal = "[" + jsonStringHeader + "," + jsonStringDeatils + "," + jsonStringFooter + "]";

        //        byte[] fileBytes = Encoding.ASCII.GetBytes(jsonStringFinal);

        //        string fileName = FileName + ".json";

        //        return File(fileBytes, "application/json", fileName);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult DownloadExcelJsonCon(string spName, string ACCOUNTINGDATE, string txtBranchCode = "")
        {
            try
            {

                var FileName = "gBankerData_Con";
                var contracttDetailListing = new List<MRA_Contract_DetailsViewModel>();
                var ContractDetailsParam = new { OrgID = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, ACCOUNTINGDATE = ACCOUNTINGDATE };
                var ContractDetails = accreportService.GetAccDataForReport(ContractDetailsParam, "Proc_Get_ContractDetails");

                var listContractDetails = ContractDetails.Tables[0].AsEnumerable()
                .Select(row => new MRA_Contract_DetailsViewModel
                {
                    DATATYPE = "C",
                    RECORDNO = row.Field<string>("RECORDNO"),
                    BRANCH_CODE = row.Field<string>("BRANCH_CODE"),
                    MEMBERID = row.Field<string>("MEMBERID"),
                    LOAN_CODE = row.Field<string>("LOAN_CODE"),
                    LOAN_TYPE = row.Field<string>("LOAN_TYPE"),
                    LOAN_DISBURSEMENT_DATE = row.Field<string>("LOAN_DISBURSEMENT_DATE"),
                    END_DATE_CONTRACT = row.Field<string>("END_DATE_CONTRACT"),
                    LAST_INSTALLMENT_PAID_DATE = row.Field<string>("LAST_INSTALLMENT_PAID_DATE"),
                    DISBURSED_AMOUNT = row.Field<string>("DISBURSED_AMOUNT"),
                    TOTAL_OUTSTANDING_AMT = row.Field<string>("TOTAL_OUTSTANDING_AMT"),
                    PERIODICITY_PAYMENT = row.Field<string>("PERIODICITY_PAYMENT"),
                    TOTAL_NUM_INSTALLMENT = row.Field<string>("TOTAL_NUM_INSTALLMENT"),
                    INSTALLMENT_AMT = row.Field<string>("INSTALLMENT_AMT"),
                    NUM_REMAINING_INSTALLMENT = row.Field<string>("NUM_REMAINING_INSTALLMENT"),
                    NUM_OVERDUE_INSTALLMENT = row.Field<string>("NUM_OVERDUE_INSTALLMENT"),
                    OVERDUE_AMT = row.Field<string>("OVERDUE_AMT"),
                    LOAN_STATUS = row.Field<string>("LOAN_STATUS"),
                    RESCHEDULE_NO = row.Field<string>("RESCHEDULE_NO"),
                    LAST_RESCHEDULE_DATE = row.Field<string>("LAST_RESCHEDULE_DATE"),
                    WRITE_OFF_AMT = row.Field<string>("WRITE_OFF_AMT"),
                    WRITE_OFF_DATE = row.Field<string>("WRITE_OFF_DATE"),
                    CONTRACT_PHASE = row.Field<string>("CONTRACT_PHASE"),
                    LOAN_DURATION = row.Field<string>("LOAN_DURATION"),
                    ACTUAL_END_DATE_CONTRACT = row.Field<string>("ACTUAL_END_DATE_CONTRACT"),
                    ECONOMIC_PURPOSE_CODE = row.Field<string>("ECONOMIC_PURPOSE_CODE"),
                    COMPULSORY_SAVING_AMT = row.Field<string>("COMPULSORY_SAVING_AMT"),
                    VOLUNTARY_SAVING_AMT = row.Field<string>("VOLUNTARY_SAVING_AMT"),
                    TERM_SAVING_AMT = row.Field<string>("TERM_SAVING_AMT"),
                    SUBSIDIZED_CREDIT_FLAG = row.Field<string>("SUBSIDIZED_CREDIT_FLAG"),
                    SERVICE_CHARGE_RATE = row.Field<string>("SERVICE_CHARGE_RATE"),
                    PAYMENT_MODE = row.Field<string>("PAYMENT_MODE"),
                    ADVANCE_PAYMENT_AMT = row.Field<string>("ADVANCE_PAYMENT_AMT"),
                    LAW_SUIT = row.Field<string>("LAW_SUIT"),
                    ME = row.Field<string>("ME"),
                    MEMBER_WELFARE_FUND = row.Field<string>("MEMBER_WELFARE_FUND"),
                    INSURENCE_COVERAGE = row.Field<string>("INSURENCE_COVERAGE"),

                }).ToList();

                var MFICODE = "";
                var ProductionDate = "";

                if (listContractDetails.Count > 0)
                {
                    MFICODE = ContractDetails.Tables[0].Rows[0]["MFICode"].ToString();  //ListContractDetails.Select(l => l.MFICODE).FirstOrDefault();
                    FileName = ContractDetails.Tables[0].Rows[0]["FileNameJSON"].ToString();
                    ACCOUNTINGDATE = ContractDetails.Tables[0].Rows[0]["ACCOUNTINGDATE"].ToString(); //ListContractDetails.Select(l => l.ACCOUNTINGDATE).FirstOrDefault();
                    ProductionDate = ContractDetails.Tables[0].Rows[0]["ProductionDate"].ToString();

                }

                var header = new
                {
                    DATATYPE = "H",
                    MFICODE = MFICODE,
                    ACCOUNTINGDATE = ACCOUNTINGDATE,
                    PRODUCTIONDATE = ProductionDate
                };


                var footer = new
                {
                    DATATYPE = "F",
                    TOTALRECORD = listContractDetails.Count.ToString()
                };

                if (listContractDetails.Any())
                    contracttDetailListing.AddRange(listContractDetails);

                //let's serial as json formatted data 
                var jsonStringHeader = JsonConvert.SerializeObject(header);
                var jsonStringFooter = JsonConvert.SerializeObject(footer);

                var jsonStringDeatils = JsonConvert.SerializeObject(contracttDetailListing);
                jsonStringDeatils = jsonStringDeatils.Replace("[", "");
                jsonStringDeatils = jsonStringDeatils.Replace("]", "");

                var jsonStringFinal = "[" + jsonStringHeader + "," + jsonStringDeatils + "," + jsonStringFooter + "]";

                byte[] fileBytes = Encoding.ASCII.GetBytes(jsonStringFinal);

                string fileName = FileName + ".json";

                return File(fileBytes, "application/json", fileName);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult DownloadExcelJsonSub(string spName, string ACCOUNTINGDATE, string txtBranchCode = "")
        //{
        //    try
        //    {
        //        var FileName = "gBankerData_Sub";
        //        var subjectDetailListing = new List<MRA_Subject_DetailsViewModel>();
        //        var SubjectDetailsParam = new { OrgID = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, ACCOUNTINGDATE = ACCOUNTINGDATE };
        //        var SubjectDetails = accreportService.GetAccDataForReport(SubjectDetailsParam, "Proc_Get_SubjectDetails");

        //        var listSubjectDetails = SubjectDetails.Tables[0].AsEnumerable()
        //        .Select(row => new MRA_Subject_DetailsViewModel
        //        {
        //            DATATYPE = "P",
        //            RECORD_NO = row.Field<int>("RECORD_NO"),
        //            BRANCH_CODE = row.Field<string>("BRANCH_CODE"),
        //            MEMBERID = row.Field<string>("MEMBERID"),
        //            NAME = row.Field<string>("Name"),
        //            OCCUPATION = row.Field<int>("Occupation"),
        //            FATHERS_NAME = row.Field<string>("FATHERS_NAME"),
        //            MOTHERS_NAME = row.Field<string>("MOTHERS_NAME"),
        //            MARITAL_STATUS = row.Field<int>("MARITAL_STATUS"),
        //            SPOUSE_NAME = row.Field<string>("SPOUSE_NAME"),
        //            GENDER = row.Field<string>("Gender"),
        //            DOB = row.Field<string>("DOB"),
        //            NID = row.Field<string>("NID"),
        //            SMARTCARD_NO = row.Field<string>("SMARTCARD_NO"),
        //            BIRTH_CERTIFICATE_NO = row.Field<string>("BIRTH_CERTIFICATE_NO"),
        //            TIN = row.Field<string>("TIN"),
        //            OTHER_ID_TYPE = row.Field<string>("Other_ID_Type"),
        //            OTHER_ID_NO = row.Field<string>("OTHER_ID_NO"),
        //            EXPIRY_DATE = row.Field<string>("EXPIRY_DATE"),
        //            ISSUE_COUNTRY = row.Field<string>("ISSUE_COUNTRY"),
        //            CONTACTNO = row.Field<string>("ContactNo"),
        //            P_ADDRESS = row.Field<string>("P_ADDRESS"),
        //            P_THANA = row.Field<string>("P_THANA"),
        //            P_DISTRICT = row.Field<string>("P_DISTRICT"),
        //            P_COUNTRY = row.Field<string>("P_COUNTRY"),
        //            PR_ADDRESS = row.Field<string>("PR_ADDRESS"),
        //            PR_THANA = row.Field<string>("PR_THANA"),

        //            PR_DISTRICT = row.Field<string>("PR_DISTRICT"),
        //            PR_COUNTRY = row.Field<string>("PR_COUNTRY"),
        //            ACADEMIC_QUALIFICATION = row.Field<int>("ACADEMIC_QUALIFICATION"),

        //        }).ToList();

        //        var MFICODE = SubjectDetails.Tables[0].Rows[0]["MFICode"].ToString();//ListSubjectDetails.Select(l => l.MFICODE).FirstOrDefault();
        //        FileName = SubjectDetails.Tables[0].Rows[0]["FileNameJSON"].ToString();
        //        ACCOUNTINGDATE = SubjectDetails.Tables[0].Rows[0]["ACCOUNTINGDATE"].ToString();//ListSubjectDetails.Select(l => l.ACCOUNTINGDATE).FirstOrDefault();

        //        var ProductionDate = SubjectDetails.Tables[0].Rows[0]["ProductionDate"].ToString();

        //        //var Subject = new
        //        //{
        //        //    header = new { DATATYPE = "H", MFICODE = MFICODE, ACCOUNTINGDATE = ACCOUNTINGDATE, PRODUCTIONDATE = ProductionDate },
        //        //    Details = new { values = ListSubjectDetails },
        //        //    footer = new { DATATYPE = "F", TOTALRECORD = ListSubjectDetails.Count },
        //        //};

        //        //var mraCIBData = new { Subject };

        //        var header = new 
        //        {
        //            DATATYPE = "H",
        //            MFICODE = MFICODE,
        //            ACCOUNTINGDATE = ACCOUNTINGDATE,
        //            PRODUCTIONDATE = ProductionDate
        //        };


        //        var footer = new 
        //        {
        //            DATATYPE = "F",
        //            TOTALRECORD = listSubjectDetails.Count.ToString()
        //        };


        //        if (listSubjectDetails.Any())
        //            subjectDetailListing.AddRange(listSubjectDetails);

        //        //let's serial as json formatted data
        //        var jsonStringHeader = JsonConvert.SerializeObject(header);
        //        var jsonStringFooter = JsonConvert.SerializeObject(footer);

        //        var jsonStringDeatils = JsonConvert.SerializeObject(listSubjectDetails);
        //        jsonStringDeatils = jsonStringDeatils.Replace("[", "");
        //        jsonStringDeatils = jsonStringDeatils.Replace("]", "");

        //        var jsonStringFinal = "[" + jsonStringHeader + "," + jsonStringDeatils + "," + jsonStringFooter + "]";

        //        byte[] fileBytes = Encoding.ASCII.GetBytes(jsonStringFinal);
        //        string fileName = FileName + ".json";

        //        return File(fileBytes, "application/json", fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult DownloadExcelJsonSub(string spName, string ACCOUNTINGDATE, string txtBranchCode = "")
        {
            try
            {
                var FileName = "gBankerData_Sub";
                var subjectDetailListing = new List<MRA_Subject_DetailsViewModel>();
                var SubjectDetailsParam = new { OrgID = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, ACCOUNTINGDATE = ACCOUNTINGDATE };
                var SubjectDetails = accreportService.GetAccDataForReport(SubjectDetailsParam, "Proc_Get_SubjectDetails");

                var listSubjectDetails = SubjectDetails.Tables[0].AsEnumerable()
                .Select(row => new MRA_Subject_DetailsViewModel
                {
                    DATATYPE = "P",
                    RECORD_NO = row.Field<string>("RECORD_NO"),
                    BRANCH_CODE = row.Field<string>("BRANCH_CODE"),
                    MEMBERID = row.Field<string>("MEMBERID"),
                    NAME = row.Field<string>("Name"),
                    OCCUPATION = row.Field<string>("Occupation"),
                    FATHERS_NAME = row.Field<string>("FATHERS_NAME"),
                    MOTHERS_NAME = row.Field<string>("MOTHERS_NAME"),
                    MARITAL_STATUS = row.Field<string>("MARITAL_STATUS"),
                    SPOUSE_NAME = row.Field<string>("SPOUSE_NAME"),
                    GENDER = row.Field<string>("Gender"),
                    DOB = row.Field<string>("DOB"),
                    NID = row.Field<string>("NID"),
                    SMARTCARD_NO = row.Field<string>("SMARTCARD_NO"),
                    BIRTH_CERTIFICATE_NO = row.Field<string>("BIRTH_CERTIFICATE_NO"),
                    TIN = row.Field<string>("TIN"),
                    OTHER_ID_TYPE = row.Field<string>("Other_ID_Type"),
                    OTHER_ID_NO = row.Field<string>("OTHER_ID_NO"),
                    EXPIRY_DATE = row.Field<string>("EXPIRY_DATE"),
                    ISSUE_COUNTRY = row.Field<string>("ISSUE_COUNTRY"),
                    CONTACTNO = row.Field<string>("ContactNo"),
                    P_ADDRESS = row.Field<string>("P_ADDRESS"),
                    P_THANA = row.Field<string>("P_THANA"),
                    P_DISTRICT = row.Field<string>("P_DISTRICT"),
                    P_COUNTRY = row.Field<string>("P_COUNTRY"),
                    PR_ADDRESS = row.Field<string>("PR_ADDRESS"),
                    PR_THANA = row.Field<string>("PR_THANA"),

                    PR_DISTRICT = row.Field<string>("PR_DISTRICT"),
                    PR_COUNTRY = row.Field<string>("PR_COUNTRY"),
                    ACADEMIC_QUALIFICATION = row.Field<string>("ACADEMIC_QUALIFICATION"),

                }).ToList();

                var MFICODE = "";
                var ProductionDate = "";

                if (listSubjectDetails.Count > 0)
                {
                    MFICODE = SubjectDetails.Tables[0].Rows[0]["MFICode"].ToString();//ListSubjectDetails.Select(l => l.MFICODE).FirstOrDefault();
                    FileName = SubjectDetails.Tables[0].Rows[0]["FileNameJSON"].ToString();
                    ACCOUNTINGDATE = SubjectDetails.Tables[0].Rows[0]["ACCOUNTINGDATE"].ToString();//ListSubjectDetails.Select(l => l.ACCOUNTINGDATE).FirstOrDefault();

                    ProductionDate = SubjectDetails.Tables[0].Rows[0]["ProductionDate"].ToString();
                }
                //var Subject = new
                //{
                //    header = new { DATATYPE = "H", MFICODE = MFICODE, ACCOUNTINGDATE = ACCOUNTINGDATE, PRODUCTIONDATE = ProductionDate },
                //    Details = new { values = ListSubjectDetails },
                //    footer = new { DATATYPE = "F", TOTALRECORD = ListSubjectDetails.Count },
                //};

                //var mraCIBData = new { Subject };

                var header = new
                {
                    DATATYPE = "H",
                    MFICODE = MFICODE,
                    ACCOUNTINGDATE = ACCOUNTINGDATE,
                    PRODUCTIONDATE = ProductionDate
                };


                var footer = new
                {
                    DATATYPE = "F",
                    TOTALRECORD = listSubjectDetails.Count.ToString()
                };


                if (listSubjectDetails.Any())
                    subjectDetailListing.AddRange(listSubjectDetails);

                //let's serial as json formatted data
                var jsonStringHeader = JsonConvert.SerializeObject(header);
                var jsonStringFooter = JsonConvert.SerializeObject(footer);

                var jsonStringDeatils = JsonConvert.SerializeObject(listSubjectDetails);
                jsonStringDeatils = jsonStringDeatils.Replace("[", "");
                jsonStringDeatils = jsonStringDeatils.Replace("]", "");

                var jsonStringFinal = "[" + jsonStringHeader + "," + jsonStringDeatils + "," + jsonStringFooter + "]";

                byte[] fileBytes = Encoding.ASCII.GetBytes(jsonStringFinal);
                string fileName = FileName + ".json";

                return File(fileBytes, "application/json", fileName);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult DownloadExcel(string DownloadExcel)
        {
            try
            {
                //var param = new { OfficeTypeId = OfficeTypeId };
                //var OverdueMls = employeeSPService.GetDataWithParameter(param, "SP_RPT_BranchWiseNoOfEmployee");
                var reportParam = new Dictionary<string, object>();


                int IsDownloadExcel = Convert.ToInt32(DownloadExcel);
                if (IsDownloadExcel == 1 && empList.Tables[0].Rows.Count > 0)
                {
                    // ExcelPrintWithSubReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources, ReportClass reportClass)
                    ExcelReortHelper.ExcelPrintWithSubReport(SpName, empList.Tables[0]);
                    /////End of Excel Export Khalid

                }
                else
                {


                }


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public ActionResult DownloadExcelDatatable(string spName, string date, string txtBranchCode = "")
        {
            try
            {
                //var UploadDate = FileUploadDate;
                //var param = new { @EmployeeList = MyEmployeeIds, @UploadDate = Convert.ToDateTime(UploadDate).Date, @CreateUser = LoggedInOfficeID };
                //var data = employeeSPService.GetDataWithParameter(param, "SP_BulkEmployeeInfoGetAllData");

                //DateTime MDt = new DateTime();

                var MDt = DateTime.Now;

                string FileName = spName + "_" + Convert.ToString(MDt.Day) + Convert.ToString(MDt.Month) + Convert.ToString(MDt.Year) + ".xls";

                DataTable dt = empList.Tables[0];
                string attachment = "attachment; filename=" + FileName;
                Response.ClearContent();
                Response.AddHeader("content-disposition", attachment);
                Response.ContentType = "application/vnd.ms-excel";
                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        Response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
                Response.End();
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        [HttpPost]
        public ActionResult Index(BuroCustomerInfoViewModel entity)//string Id, string Center
        {
            var list = new List<dynamic>();
            try
            {
                ListToDataTableHelper convertDT = new ListToDataTableHelper();
                DataTable dd = new DataTable();

                dd = empList.Tables[0];
                var resultset = convertDT.ConvertToDictionary(dd);

                var result = new List<dynamic>();

                foreach (var emprow in resultset)
                {
                    var row = (IDictionary<string, object>)new ExpandoObject();
                    Dictionary<string, object> eachRow = (Dictionary<string, object>)emprow;

                    foreach (KeyValuePair<string, object> keyValuePair in eachRow)
                    {
                        row.Add(keyValuePair);
                    }
                    result.Add(row);
                }

                entity.CheckStaffDataTable = result.Count > 0 ? result : new List<dynamic>();

            }
            catch (Exception e)
            {
                entity.Result = 2;
                entity.ReturnMessage = "You provided wrong parameter for this check.";
            }


            List<SelectListItem> items3 = new List<SelectListItem>();
            //items3.Add(new SelectListItem
            //{
            //    Text = "Please Select",
            //    Value = "0"
            //});
            ViewData["comtype"] = items3;

            var officeInfo = officeService.GetByIdLong((long)SessionHelper.LoginUserOfficeID);
            ViewData["OfficeName"] = officeInfo.OfficeCode + "-" + officeInfo.OfficeName;

            ViewData["BranchCode"] = officeInfo.OfficeCode;


            entity.BranchCode = officeInfo.OfficeCode;


            return View(entity);
        }
        #region dr
        public JsonResult GetSPNameListForDR()
        {

            StringBuilder sb = new StringBuilder();

            var param = new { AndCondition = sb.ToString() };
            var MessageList = ultimateExcelService.GetDataWithParameter(param, "SP_Get_SPList_ForDr");

            List<ReportSpName> List_ViewModel = new List<ReportSpName>();
            List_ViewModel = MessageList.Tables[0].AsEnumerable()
            .Select(row => new ReportSpName
            {
                LabelName = row.Field<string>("LabelName"),
                SpName = row.Field<string>("SpName")

            }).ToList();

            var viewData = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.SpName.ToString(),
                Text = x.LabelName.ToString() //+ " " + x.OfficeName.ToString()
            });
            var List_items = new List<SelectListItem>();

            List_items.AddRange(viewData);
            return Json(List_items, JsonRequestBehavior.AllowGet);
        }//  END  Function 
        public ActionResult DRView()
        {

            var model = new BuroCustomerInfoViewModel();
            List<SelectListItem> items3 = new List<SelectListItem>();
            //items3.Add(new SelectListItem
            //{
            //    Text = "Please Select",
            //    Value = "0"
            //});
            ViewData["comtype"] = items3;

            var officeInfo = officeService.GetByIdLong((long)SessionHelper.LoginUserOfficeID);
            ViewData["OfficeName"] = officeInfo.OfficeCode + "-" + officeInfo.OfficeName;

            ViewData["BranchCode"] = officeInfo.OfficeCode;



            model.BranchCode = officeInfo.OfficeCode;
            model.IsSuperAdmin = false;
            model.CheckStaffDataTable = new List<dynamic>();
            var v = model.CheckStaffDataTable.Count;
            return View(model);
        }
        [HttpPost]
        public ActionResult DRView(BuroCustomerInfoViewModel entity)//string Id, string Center
        {
            var list = new List<dynamic>();
            try
            {
                ListToDataTableHelper convertDT = new ListToDataTableHelper();
                DataTable dd = new DataTable();

                dd = empList.Tables[0];
                var resultset = convertDT.ConvertToDictionary(dd);

                var result = new List<dynamic>();

                foreach (var emprow in resultset)
                {
                    var row = (IDictionary<string, object>)new ExpandoObject();
                    Dictionary<string, object> eachRow = (Dictionary<string, object>)emprow;

                    foreach (KeyValuePair<string, object> keyValuePair in eachRow)
                    {
                        row.Add(keyValuePair);
                    }
                    result.Add(row);
                }

                entity.CheckStaffDataTable = result.Count > 0 ? result : new List<dynamic>();

            }
            catch (Exception e)
            {
                entity.Result = 2;
                entity.ReturnMessage = "You provided wrong parameter for this check.";
            }


            List<SelectListItem> items3 = new List<SelectListItem>();
            //items3.Add(new SelectListItem
            //{
            //    Text = "Please Select",
            //    Value = "0"
            //});
            ViewData["comtype"] = items3;

            var officeInfo = officeService.GetByIdLong((long)SessionHelper.LoginUserOfficeID);
            ViewData["OfficeName"] = officeInfo.OfficeCode + "-" + officeInfo.OfficeName;

            ViewData["BranchCode"] = officeInfo.OfficeCode;



            entity.BranchCode = officeInfo.OfficeCode;


            return View(entity);
        }
        public JsonResult ExecuteSPForDr(string txtBranchCode = "", string ddlSPName = "", string TillDate = "")
        {
            string result = "OK";
            try
            {
                SpName = ddlSPName.Trim().ToString();


                if (TillDate != "")
                {
                    var param2 = new
                    {
                        BranchCode = txtBranchCode,
                        dateTo = TillDate
                    };
                    empList = ultimateExcelService.GetDataWithParameter(param2, ddlSPName);
                }
                else
                {
                    var param = new
                    {
                        BranchCode = txtBranchCode
                    };
                    empList = ultimateExcelService.GetDataWithParameter(param, ddlSPName);

                }
                //  public List<dynamic> CheckStaffDataTable { get; set; }

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }// END Class
}// END NamespaceBappa
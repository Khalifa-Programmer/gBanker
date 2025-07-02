//using ExcelMigration.DBModel;
//using ExcelMigration.WEB.DBModel;
//using ExcelMigration.WEB.ViewModel;
using OfficeOpenXml;
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using System.Text.RegularExpressions;
//using ExcelMigration.ViewModel;
//using ExcelMigration.Service.StoredProcedure;
//using ExcelMigration.Helper;
using System.Dynamic;
using System.Data.Entity.Validation;
using gBanker.Web.ViewModels;
using gBanker.Service.ReportServies;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Web.Helpers;

namespace gBanker.Web.Controllers
{
    public class ExcelUploadController : BaseController
    {
        List<ValidateResultModel> WrongStaffInfoList = new List<ValidateResultModel>();
        List<ValidateResultModel> WrongCenterInfoList = new List<ValidateResultModel>();
        List<ValidateResultModel> WrongCustomerInfoList = new List<ValidateResultModel>();
        //UltimateReportService SPQueryService = new UltimateReportService();
        //DBDataAccess dbDataAccess = new DBDataAccess();

        private readonly IUltimateReportService unlimitedReportService;
        public ExcelUploadController(IUltimateReportService unlimitedReportService)

        {

            this.unlimitedReportService = unlimitedReportService;

        }
        public ActionResult Index(int? Id)
        {
            try
            {
                var model = new BuroCustomerInfoViewModel();
                //model.of = Session["BranchCode"].ToString() != null ? Session["BranchCode"].ToString() : null;
                model.OfficeID = Convert.ToInt16(LoginUserOfficeID);
                model.IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"].ToString() != null ? Session["IsSuperAdmin"].ToString() : "false");
                if (Id.HasValue == true && Id > 0)
                {
                    ViewBag.result = "Success";
                }
                else
                {
                    ViewBag.result = null;
                }
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            var retSheet = new BuroCustomerInfoViewModel();
            if (Request != null)
            {


                HttpPostedFileBase file = Request.Files["UploadFile"];
                try
                {
                    var customerList = new List<BuroCustomerInfo>();
                    var centerList = new List<BuroCenterInfo>();
                    var staffList = new List<BuroStaffInfo>();
                    if ((file != null) && (file.ContentLength != 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                        var fileTYpe = Path.GetExtension(file.FileName).GetType();

                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;

                            foreach (var sheet in currentSheet)
                            {
                                var sheetName = sheet.Name;
                                var sheetIndex = sheet.Index;
                                var noOfCol = sheet.Dimension.End.Column;
                                var noOfRow = sheet.Dimension.End.Row;
                                var currentNUCode = "";
                                var previousNUCode = "";

                                if (sheetIndex == 1)//sheetName == "Customer_Info"
                                {
                                    customerList = GenerateCustomerInfoList(sheet, noOfRow);
                                }
                                if (sheetIndex == 2)//sheetName == "Center_Info"
                                {
                                    centerList = GenerateCenterInfoList(sheet, noOfRow);
                                }
                                if (sheetIndex == 3)//sheetName == "Staff_Info"
                                {
                                    staffList = GenerateStaffInfoList(sheet, noOfRow);
                                }
                            }
                        }
                        ///Save to DB
                        ///
                        using (TransactionScope scope = new TransactionScope())
                        {

                            if (WrongStaffInfoList.Count() == 0 && WrongCenterInfoList.Count() == 0 && WrongCustomerInfoList.Count() == 0)
                            {
                                var s_branchCode = staffList.First().BranchCode;
                                var c_branchCode = centerList.First().BranchCode;
                                var cu_branchCode = customerList.First().BranchCode;
                                if ((s_branchCode==c_branchCode) && (c_branchCode==cu_branchCode))
                                {
                                    var param = new { BranchCode = s_branchCode };
                                    unlimitedReportService.GetDataWithParameter(param, "SP_DeleteTableDataByBranchCode");
                                    SaveStaffList(staffList);
                                    SaveCenterList(centerList);
                                    SaveCustomerList(customerList);
                                }
                                
                            }
                            scope.Complete();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;                    
                }
            }
            retSheet.WrongStaffInfoList = WrongStaffInfoList;

            retSheet.WrongCenterInfoList = WrongCenterInfoList;

            retSheet.WrongCustomerInfoList = WrongCustomerInfoList;
            if ((WrongStaffInfoList.Count==0) && (WrongCenterInfoList.Count==0) && (WrongCustomerInfoList.Count==0)) {
                return RedirectToAction("Index",new {Id=1 });
               // ViewBag.result = "Success";
            }
            else {
                return View(retSheet);
            }

            
        }
        [HttpGet]
        public ActionResult VerifyExcelData()
        {
            try
            {
                var model = new BuroCustomerInfoViewModel();
                // model.BranchCode = Session["BranchCode"].ToString() != null ? Session["BranchCode"].ToString() : null;
                //model.IsSuperAdmin = Convert.ToBoolean(Session["IsSuperAdmin"].ToString() != null ? Session["IsSuperAdmin"].ToString() : "false");
                model.OfficeID = Convert.ToInt16(LoginUserOfficeID);
                MapBranchCodeList(model);
                model.CheckStaffDataTable = new List<dynamic>();
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult VerifyExcelData(BuroCustomerInfoViewModel entity)//string Id, string Center
        {
            var list = new List<dynamic>();
            MapBranchCodeList(entity);
            try
            {

                if (entity.BranchCode != "" && entity.SpName != "")
                {
                    if (entity.DateTo != "" && entity.DateTo != null)
                    {
                        list = SpDynamicCalling(entity.BranchCode, entity.DateTo, entity.SpName);
                    }
                    else
                    {
                        list = SpDynamicCalling(entity.BranchCode, entity.SpName);
                    }

                }
                entity.Result = 1;
                entity.ReturnMessage = "";
            }
            catch (Exception e)
            {
                entity.Result = 2;
                entity.ReturnMessage = "You provided wrong parameter for this check.";
            }
            entity.CheckStaffDataTable = list.Count > 0 ? list : new List<dynamic>();
            return View(entity);
        }

        private void MapBranchCodeList(BuroCustomerInfoViewModel model)
        {
            var branchCodes = unlimitedReportService.GetDataWithoutParameter("SP_GetBranchCodeList");
            var branchCodeList = branchCodes.Tables[0].AsEnumerable().Select(p => new SelectListItem()
            {
                Text = p.Field<string>("BranchCode"),
                Value = p.Field<string>("BranchCode")
            }).ToList();
            var listView = new List<SelectListItem>();
            listView.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            listView.AddRange(branchCodeList);
            model.BranchCodeList = listView;

            //model.ReportSpNameList = dbDataAccess.GetReportSpNameList();
        }
        private List<BuroCustomerInfo> GenerateCustomerInfoList(ExcelWorksheet sheet, int noOfRow)
        {
            var customerList = new List<BuroCustomerInfo>();
            var sheetName = sheet.Name;
            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            {
                //if (rowIterator ==511)
                //{
                    try
                    {
                        var customer = new BuroCustomerInfo();

                        var ExcelSl = IfCellHasValue(sheet, rowIterator, 1, "int", sheetName, "Sl", "BuroCustomerInfo");
                        customer.Sl = ExcelSl.Result == true ? Convert.ToInt32(ExcelSl.Value) : 0; //IfCellHasValue(sheet, rowIterator, 2);

                        var BranchCode = IfCellHasValue(sheet, rowIterator, 2, "string", sheetName, "BranchCode", "BuroCustomerInfo");
                        customer.BranchCode = BranchCode.Result == true ? BranchCode.Value : null; //IfCellHasValue(sheet, rowIterator, 2);

                        var CenterType = IfCellHasValue(sheet, rowIterator, 3, "string", sheetName, "CenterType", "BuroCustomerInfo");
                        customer.CenterType = CenterType.Result == true ? CenterType.Value : null; //IfCellHasValue(sheet, rowIterator, 3);

                        var CustomerType = IfCellHasValue(sheet, rowIterator, 4, "string", sheetName, "CustomerType", "BuroCustomerInfo");
                        customer.CustomerType = CustomerType.Result == true ? CustomerType.Value : null; //IfCellHasValue(sheet, rowIterator, 4);

                        var CenterNo = IfCellHasValue(sheet, rowIterator, 5, "string", sheetName, "CenterNo", "BuroCustomerInfo");
                        customer.CenterNo = CenterNo.Result == true ? CenterNo.Value : null; //IfCellHasValue(sheet, rowIterator, 5);

                        var CustomerId = IfCellHasValue(sheet, rowIterator, 6, "string", sheetName, "CustomerId", "BuroCustomerInfo");
                        customer.CustomerId = CustomerId.Result == true ? CustomerId.Value : null; //IfCellHasValue(sheet, rowIterator, 6);

                        var CustomrName = IfCellHasValue(sheet, rowIterator, 7, "string", sheetName, "CustomrName", "BuroCustomerInfo");
                        customer.CustomrName = CustomrName.Result == true ? CustomrName.Value : null; //IfCellHasValue(sheet, rowIterator, 7);

                        var SurveyDate = IfCellHasValue(sheet, rowIterator, 8, "DateTime", sheetName, "SurveyDate", "BuroCustomerInfo");
                        customer.SurveyDate = SurveyDate.Result == true ? Convert.ToDateTime(SurveyDate.Value) : DateTime.Now; //IfCellHasValue(sheet, rowIterator, 8);

                        var AdmissionDate = IfCellHasValue(sheet, rowIterator, 9, "DateTime", sheetName, "AdmissionDate", "BuroCustomerInfo");
                        customer.AdmissionDate = AdmissionDate.Result == true ? Convert.ToDateTime(AdmissionDate.Value) : DateTime.Now; //IfCellHasValue(sheet, rowIterator, 9);

                        var PO = IfCellHasValue(sheet, rowIterator, 10, "int", sheetName, "PO", "BuroCustomerInfo");
                        customer.PO = PO.Result == true ? Convert.ToInt32(PO.Value) : 0; //IfCellHasValue(sheet, rowIterator, 10);

                        var Gender = IfCellHasValue(sheet, rowIterator, 11, "string", sheetName, "Gender", "BuroCustomerInfo");
                        customer.Gender = Gender.Result == true ? Gender.Value : null; //IfCellHasValue(sheet, rowIterator, 11);

                        var Age = IfCellHasValue(sheet, rowIterator, 12, "int", sheetName, "Age", "BuroCustomerInfo");
                        customer.Age = Age.Result == true ? Convert.ToInt32(Age.Value) : 0; //IfCellHasValue(sheet, rowIterator, 12);

                        var FatherName = IfCellHasValue(sheet, rowIterator, 13, "string", sheetName, "FatherName", "BuroCustomerInfo");
                        customer.FatherName = FatherName.Result == true ? FatherName.Value : null; //IfCellHasValue(sheet, rowIterator, 13);

                        var SpouseName = IfCellHasValue(sheet, rowIterator, 14, "string", sheetName, "SpouseName", "BuroCustomerInfo");
                        customer.SpouseName = SpouseName.Result == true ? SpouseName.Value : null; //IfCellHasValue(sheet, rowIterator, 14);

                        var NationalID = IfCellHasValue(sheet, rowIterator, 15, "string", sheetName, "NationalID", "BuroCustomerInfo");
                        customer.NationalID = NationalID.Result == true ? NationalID.Value : null; //IfCellHasValue(sheet, rowIterator, 15);

                        var MobileNo = IfCellHasValue(sheet, rowIterator, 16, "string", sheetName, "MobileNo", "BuroCustomerInfo");
                        customer.MobileNo = MobileNo.Result == true ? MobileNo.Value : null; //IfCellHasValue(sheet, rowIterator, 16);

                        var Village = IfCellHasValue(sheet, rowIterator, 17, "string", sheetName, "Village", "BuroCustomerInfo");
                        customer.Village = Village.Result == true ? Village.Value : null; //IfCellHasValue(sheet, rowIterator, 17);

                        var PostOffice = IfCellHasValue(sheet, rowIterator, 18, "string", sheetName, "PostOffice", "BuroCustomerInfo");
                        customer.PostOffice = PostOffice.Result == true ? PostOffice.Value : null; //IfCellHasValue(sheet, rowIterator, 18);

                        var Union = IfCellHasValue(sheet, rowIterator, 19, "string", sheetName, "Union", "BuroCustomerInfo");
                        customer.Union = Union.Result == true ? Union.Value : null; //IfCellHasValue(sheet, rowIterator, 19);

                        var Thana = IfCellHasValue(sheet, rowIterator, 20, "string", sheetName, "Thana", "BuroCustomerInfo");
                        customer.Thana = Thana.Result == true ? Thana.Value : null; //IfCellHasValue(sheet, rowIterator, 20);

                        var District = IfCellHasValue(sheet, rowIterator, 21, "string", sheetName, "District", "BuroCustomerInfo");
                        customer.District = District.Result == true ? District.Value : null; //IfCellHasValue(sheet, rowIterator, 21);

                        var SurveyStatus = IfCellHasValue(sheet, rowIterator, 22, "string", sheetName, "SurveyStatus", "BuroCustomerInfo");
                        customer.SurveyStatus = SurveyStatus.Result == true ? SurveyStatus.Value : null; //IfCellHasValue(sheet, rowIterator, 22);

                        var Occupation = IfCellHasValue(sheet, rowIterator, 23, "string", sheetName, "Occupation", "BuroCustomerInfo");
                        customer.Occupation = Occupation.Result == true ? Occupation.Value : null; //IfCellHasValue(sheet, rowIterator, 23);

                        var MaritalStatus = IfCellHasValue(sheet, rowIterator, 24, "string", sheetName, "MaritalStatus", "BuroCustomerInfo");
                        customer.MaritalStatus = MaritalStatus.Result == true ? MaritalStatus.Value : null; //IfCellHasValue(sheet, rowIterator, 24);

                        var NoOfMaleFamilyMember = IfCellHasValue(sheet, rowIterator, 25, "int", sheetName, "NoOfMaleFamilyMember", "BuroCustomerInfo");
                        customer.NoOfMaleFamilyMember = NoOfMaleFamilyMember.Result == true ? Convert.ToInt32(NoOfMaleFamilyMember.Value) : 0; //IfCellHasValue(sheet, rowIterator, 25);

                        var NoOfFemaleFamilyMember = IfCellHasValue(sheet, rowIterator, 26, "int", sheetName, "NoOfFemaleFamilyMember", "BuroCustomerInfo");
                        customer.NoOfFemaleFamilyMember = NoOfFemaleFamilyMember.Result == true ? Convert.ToInt32(NoOfFemaleFamilyMember.Value) : 0; //IfCellHasValue(sheet, rowIterator, 26);

                        var NoOfEarningFamilyMember = IfCellHasValue(sheet, rowIterator, 27, "int", sheetName, "NoOfEarningFamilyMember", "BuroCustomerInfo");
                        customer.NoOfEarningFamilyMember = NoOfEarningFamilyMember.Result == true ? Convert.ToInt32(NoOfEarningFamilyMember.Value) : 0; //IfCellHasValue(sheet, rowIterator, 27);

                        var FamilyHeadName = IfCellHasValue(sheet, rowIterator, 28, "string", sheetName, "FamilyHeadName", "BuroCustomerInfo");
                        customer.FamilyHeadName = FamilyHeadName.Result == true ? FamilyHeadName.Value : null; //IfCellHasValue(sheet, rowIterator, 28);

                        var AnnualFamilyIncome = IfCellHasValue(sheet, rowIterator, 29, "decimal", sheetName, "AnnualFamilyIncome", "BuroCustomerInfo");
                        customer.AnnualFamilyIncome = AnnualFamilyIncome.Result == true ? Convert.ToDecimal(AnnualFamilyIncome.Value) : 0; //IfCellHasValue(sheet, rowIterator, 29);

                        var PaidTaxAmount = IfCellHasValue(sheet, rowIterator, 30, "decimal", sheetName, "PaidTaxAmount", "BuroCustomerInfo");
                        customer.PaidTaxAmount = PaidTaxAmount.Result == true ? Convert.ToDecimal(PaidTaxAmount.Value) : 0; //IfCellHasValue(sheet, rowIterator, 30);

                        var FamilyLand = IfCellHasValue(sheet, rowIterator, 31, "int", sheetName, "FamilyLand", "BuroCustomerInfo");
                        customer.FamilyLand = FamilyLand.Result == true ? Convert.ToInt32(FamilyLand.Value) : 0; //IfCellHasValue(sheet, rowIterator, 31);

                        var FamilyAssetAmount = IfCellHasValue(sheet, rowIterator, 32, "decimal", sheetName, "FamilyAssetAmount", "BuroCustomerInfo");
                        customer.FamilyAssetAmount = FamilyAssetAmount.Result == true ? Convert.ToDecimal(FamilyAssetAmount.Value) : 0; //IfCellHasValue(sheet, rowIterator, 32);

                        var GSProductCode = IfCellHasValue(sheet, rowIterator, 33, "string", sheetName, "GSProductCode", "BuroCustomerInfo");
                        customer.GSProductCode = GSProductCode.Result == true ? GSProductCode.Value : null; //IfCellHasValue(sheet, rowIterator, 33);

                        var GSAccountOpeningDate = IfCellHasValue(sheet, rowIterator, 34, "DateTime", sheetName, "GSAccountOpeningDate", "BuroCustomerInfo");
                        customer.GSAccountOpeningDate = GSAccountOpeningDate.Result == true ? Convert.ToDateTime(GSAccountOpeningDate.Value) : DateTime.Now; //IfCellHasValue(sheet, rowIterator, 34);

                        var GSNomineeName = IfCellHasValue(sheet, rowIterator, 35, "string", sheetName, "GSNomineeName", "BuroCustomerInfo");
                        customer.GSNomineeName = GSNomineeName.Result == true ? GSNomineeName.Value : null; //IfCellHasValue(sheet, rowIterator, 35);

                        var GSRelation = IfCellHasValue(sheet, rowIterator, 36, "string", sheetName, "GSRelation", "BuroCustomerInfo");
                        customer.GSRelation = GSRelation.Result == true ? GSRelation.Value : null; //IfCellHasValue(sheet, rowIterator, 36);

                        var GSNomineeAddress = IfCellHasValue(sheet, rowIterator, 37, "string", sheetName, "GSNomineeAddress", "BuroCustomerInfo");
                        customer.GSNomineeAddress = GSNomineeAddress.Result == true ? GSNomineeAddress.Value : null; //IfCellHasValue(sheet, rowIterator, 37);

                        var GSInterest = IfCellHasValue(sheet, rowIterator, 38, "decimal", sheetName, "GSInterest", "BuroCustomerInfo");
                        customer.GSInterest = GSInterest.Result == true ? Convert.ToDecimal(GSInterest.Value) : 0; //IfCellHasValue(sheet, rowIterator, 38);

                        var GSClosingBalnce = IfCellHasValue(sheet, rowIterator, 39, "decimal", sheetName, "GSClosingBalnce", "BuroCustomerInfo");
                        customer.GSClosingBalnce = GSClosingBalnce.Result == true ? Convert.ToDecimal(GSClosingBalnce.Value) : 0; //IfCellHasValue(sheet, rowIterator, 39);

                        var CSProductCode = IfCellHasValue(sheet, rowIterator, 40, "string", sheetName, "CSProductCode", "BuroCustomerInfo");
                        customer.CSProductCode = CSProductCode.Result == true ? CSProductCode.Value : null; //IfCellHasValue(sheet, rowIterator, 40);

                        var CSAccountOpeningDate = IfCellHasValue(sheet, rowIterator, 41, "DateTime", sheetName, "CSAccountOpeningDate", "BuroCustomerInfo");
                        customer.CSAccountOpeningDate = CSAccountOpeningDate.Result == true ? Convert.ToDateTime(CSAccountOpeningDate.Value) : DateTime.Now; //IfCellHasValue(sheet, rowIterator, 41);

                        var CSDepositLife = IfCellHasValue(sheet, rowIterator, 42, "int", sheetName, "CSDepositLife", "BuroCustomerInfo");
                        customer.CSDepositLife = CSDepositLife.Result == true ? Convert.ToInt32(CSDepositLife.Value) : 0; //IfCellHasValue(sheet, rowIterator, 42);

                        var CSScheme = IfCellHasValue(sheet, rowIterator, 43, "int", sheetName, "CSScheme", "BuroCustomerInfo");
                        customer.CSScheme = CSScheme.Result == true ? Convert.ToInt32(CSScheme.Value) : 0; //IfCellHasValue(sheet, rowIterator, 43);

                        var CSNomineeName = IfCellHasValue(sheet, rowIterator, 44, "string", sheetName, "CSNomineeName", "BuroCustomerInfo");
                        customer.CSNomineeName = CSNomineeName.Result == true ? CSNomineeName.Value : null; //IfCellHasValue(sheet, rowIterator, 44);

                        var CSRelation = IfCellHasValue(sheet, rowIterator, 45, "string", sheetName, "CSRelation", "BuroCustomerInfo");
                        customer.CSRelation = CSRelation.Result == true ? CSRelation.Value : null; //IfCellHasValue(sheet, rowIterator, 45);

                        var CSNomineeAddress = IfCellHasValue(sheet, rowIterator, 46, "string", sheetName, "CSNomineeAddress", "BuroCustomerInfo");
                        customer.CSNomineeAddress = CSNomineeAddress.Result == true ? CSNomineeAddress.Value : null; //IfCellHasValue(sheet, rowIterator, 46);

                        var CSRealizableInstNum = IfCellHasValue(sheet, rowIterator, 47, "int", sheetName, "CSRealizableInstNum", "BuroCustomerInfo");
                        customer.CSRealizableInstNum = CSRealizableInstNum.Result == true ? Convert.ToInt32(CSRealizableInstNum.Value) : 0; //IfCellHasValue(sheet, rowIterator, 47);

                        var CSRealizedInstNum = IfCellHasValue(sheet, rowIterator, 48, "int", sheetName, "CSRealizedInstNum", "BuroCustomerInfo");
                        customer.CSRealizedInstNum = CSRealizedInstNum.Result == true ? Convert.ToInt32(CSRealizedInstNum.Value) : 0; //IfCellHasValue(sheet, rowIterator, 48);

                        var CSInterest = IfCellHasValue(sheet, rowIterator, 49, "decimal", sheetName, "CSInterest", "BuroCustomerInfo");
                        customer.CSInterest = CSInterest.Result == true ? Convert.ToDecimal(CSInterest.Value) : 0; //IfCellHasValue(sheet, rowIterator, 49);

                        var CSFine = IfCellHasValue(sheet, rowIterator, 50, "decimal", sheetName, "CSFine", "BuroCustomerInfo");
                        customer.CSFine = CSFine.Result == true ? Convert.ToDecimal(CSFine.Value) : 0; //IfCellHasValue(sheet, rowIterator, 50);

                        var CSClosingBalance = IfCellHasValue(sheet, rowIterator, 51, "decimal", sheetName, "CSClosingBalance", "BuroCustomerInfo");
                        customer.CSClosingBalance = CSClosingBalance.Result == true ? Convert.ToDecimal(CSClosingBalance.Value) : 0; //IfCellHasValue(sheet, rowIterator, 51);

                        var CSProductcode2 = IfCellHasValue(sheet, rowIterator, 52, "string", sheetName, "CSProductcode2", "BuroCustomerInfo");
                        customer.CSProductcode2 = CSProductcode2.Result == true ? CSProductcode2.Value : null; //IfCellHasValue(sheet, rowIterator, 52);

                        var CSAccountOpeningDate2 = IfCellHasValue(sheet, rowIterator, 53, "DateTime", sheetName, "CSAccountOpeningDate2", "BuroCustomerInfo");
                        customer.CSAccountOpeningDate2 = CSAccountOpeningDate2.Result == true ? Convert.ToDateTime(CSAccountOpeningDate2.Value) : DateTime.Now; //IfCellHasValue(sheet, rowIterator, 53);

                        var CSDepositLife2 = IfCellHasValue(sheet, rowIterator, 54, "int", sheetName, "CSDepositLife2", "BuroCustomerInfo");
                        customer.CSDepositLife2 = CSDepositLife2.Result == true ? Convert.ToInt32(CSDepositLife2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 54);

                        var CSScheme2 = IfCellHasValue(sheet, rowIterator, 55, "int", sheetName, "CSScheme2", "BuroCustomerInfo");
                        customer.CSScheme2 = CSScheme2.Result == true ? Convert.ToInt32(CSScheme2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 55);

                        var CSNomineeName2 = IfCellHasValue(sheet, rowIterator, 56, "string", sheetName, "CSNomineeName2", "BuroCustomerInfo");
                        customer.CSNomineeName2 = CSNomineeName2.Result == true ? CSNomineeName2.Value : null; //IfCellHasValue(sheet, rowIterator, 56);

                        var CSRelation2 = IfCellHasValue(sheet, rowIterator, 57, "string", sheetName, "CSRelation2", "BuroCustomerInfo");
                        customer.CSRelation2 = CSRelation2.Result == true ? CSRelation2.Value : null; //IfCellHasValue(sheet, rowIterator, 57);

                        var CSNomineeAddress2 = IfCellHasValue(sheet, rowIterator, 58, "string", sheetName, "CSNomineeAddress2", "BuroCustomerInfo");
                        customer.CSNomineeAddress2 = CSNomineeAddress2.Result == true ? CSNomineeAddress2.Value : null; //IfCellHasValue(sheet, rowIterator, 58);

                        var CSRealizableInstNum2 = IfCellHasValue(sheet, rowIterator, 59, "int", sheetName, "CSRealizableInstNum2", "BuroCustomerInfo");
                        customer.CSRealizableInstNum2 = CSRealizableInstNum2.Result == true ? Convert.ToInt32(CSRealizableInstNum2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 59);

                        var CSRealizedInstNum2 = IfCellHasValue(sheet, rowIterator, 60, "int", sheetName, "CSRealizedInstNum2", "BuroCustomerInfo");
                        customer.CSRealizedInstNum2 = CSRealizedInstNum2.Result == true ? Convert.ToInt32(CSRealizedInstNum2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 60);

                        var CSInterest2 = IfCellHasValue(sheet, rowIterator, 61, "decimal", sheetName, "CSInterest2", "BuroCustomerInfo");
                        customer.CSInterest2 = CSInterest2.Result == true ? Convert.ToDecimal(CSInterest2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 61);

                        var CSFine2 = IfCellHasValue(sheet, rowIterator, 62, "decimal", sheetName, "CSFine2", "BuroCustomerInfo");
                        customer.CSFine2 = CSFine2.Result == true ? Convert.ToDecimal(CSFine2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 62);

                        var CSClosingBalnce2 = IfCellHasValue(sheet, rowIterator, 63, "decimal", sheetName, "CSClosingBalnce2", "BuroCustomerInfo");
                        customer.CSClosingBalnce2 = CSClosingBalnce2.Result == true ? Convert.ToDecimal(CSClosingBalnce2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 63);

                        var CSProductcode3 = IfCellHasValue(sheet, rowIterator, 64, "string", sheetName, "CSProductcode3", "BuroCustomerInfo");
                        customer.CSProductcode3 = CSProductcode3.Result == true ? CSProductcode3.Value : null; //IfCellHasValue(sheet, rowIterator, 64);

                        var CSAccountOpeningDate3 = IfCellHasValue(sheet, rowIterator, 65, "DateTime", sheetName, "CSAccountOpeningDate3", "BuroCustomerInfo");
                        customer.CSAccountOpeningDate3 = CSAccountOpeningDate3.Result == true ? Convert.ToDateTime(CSAccountOpeningDate3.Value) : DateTime.Now; //IfCellHasValue(sheet, rowIterator, 65);

                        var CSDepositLife3 = IfCellHasValue(sheet, rowIterator, 66, "int", sheetName, "CSDepositLife3", "BuroCustomerInfo");
                        customer.CSDepositLife3 = CSDepositLife3.Result == true ? Convert.ToInt32(CSDepositLife3.Value) : 0; //IfCellHasValue(sheet, rowIterator, 66);

                        var CSScheme3 = IfCellHasValue(sheet, rowIterator, 67, "int", sheetName, "CSScheme3", "BuroCustomerInfo");
                        customer.CSScheme3 = CSScheme3.Result == true ? Convert.ToInt32(CSScheme3.Value) : 0; //IfCellHasValue(sheet, rowIterator, 67);

                        var CSNomineeName3 = IfCellHasValue(sheet, rowIterator, 68, "string", sheetName, "CSNomineeName3", "BuroCustomerInfo");
                        customer.CSNomineeName3 = CSNomineeName3.Result == true ? CSNomineeName3.Value : null; //IfCellHasValue(sheet, rowIterator, 68);

                        var CSRelation3 = IfCellHasValue(sheet, rowIterator, 69, "string", sheetName, "CSRelation3", "BuroCustomerInfo");
                        customer.CSRelation3 = CSRelation3.Result == true ? CSRelation3.Value : null; //IfCellHasValue(sheet, rowIterator, 69);

                        var CSNomineeAddress3 = IfCellHasValue(sheet, rowIterator, 70, "string", sheetName, "CSNomineeAddress3", "BuroCustomerInfo");
                        customer.CSNomineeAddress3 = CSNomineeAddress3.Result == true ? CSNomineeAddress3.Value : null; //IfCellHasValue(sheet, rowIterator, 70);

                        var CSRealizableInstNum3 = IfCellHasValue(sheet, rowIterator, 71, "int", sheetName, "CSRealizableInstNum3", "BuroCustomerInfo");
                        customer.CSRealizableInstNum3 = CSRealizableInstNum3.Result == true ? Convert.ToInt32(CSRealizableInstNum3.Value) : 0; //IfCellHasValue(sheet, rowIterator, 71);

                        var CSRealizedInstNum3 = IfCellHasValue(sheet, rowIterator, 72, "int", sheetName, "CSRealizedInstNum3", "BuroCustomerInfo");
                        customer.CSRealizedInstNum3 = CSRealizedInstNum3.Result == true ? Convert.ToInt32(CSRealizedInstNum3.Value) : 0; //IfCellHasValue(sheet, rowIterator, 72);

                        var CSInterest3 = IfCellHasValue(sheet, rowIterator, 73, "decimal", sheetName, "CSInterest3", "BuroCustomerInfo");
                        customer.CSInterest3 = CSInterest3.Result == true ? Convert.ToDecimal(CSInterest3.Value) : 0; //IfCellHasValue(sheet, rowIterator, 73);

                        var CSFine3 = IfCellHasValue(sheet, rowIterator, 74, "decimal", sheetName, "CSFine3", "BuroCustomerInfo");
                        customer.CSFine3 = CSFine3.Result == true ? Convert.ToDecimal(CSFine3.Value) : 0; //IfCellHasValue(sheet, rowIterator, 74);

                        var CSClosingBalnce3 = IfCellHasValue(sheet, rowIterator, 75, "decimal", sheetName, "CSClosingBalnce3", "BuroCustomerInfo");
                        customer.CSClosingBalnce3 = CSClosingBalnce3.Result == true ? Convert.ToDecimal(CSClosingBalnce3.Value) : 0; //IfCellHasValue(sheet, rowIterator, 75);

                        var CSProductcode4 = IfCellHasValue(sheet, rowIterator, 76, "string", sheetName, "CSProductcode4", "BuroCustomerInfo");
                        customer.CSProductcode4 = CSProductcode4.Result == true ? CSProductcode4.Value : null; //IfCellHasValue(sheet, rowIterator, 76);

                        var CSAccountOpeningDate4 = IfCellHasValue(sheet, rowIterator, 77, "DateTime", sheetName, "CSAccountOpeningDate4", "BuroCustomerInfo");
                        customer.CSAccountOpeningDate4 = CSAccountOpeningDate4.Result == true ? Convert.ToDateTime(CSAccountOpeningDate4.Value) : DateTime.Now; //IfCellHasValue(sheet, rowIterator, 77);

                        var CSDepositLife4 = IfCellHasValue(sheet, rowIterator, 78, "int", sheetName, "CSDepositLife4", "BuroCustomerInfo");
                        customer.CSDepositLife4 = CSDepositLife4.Result == true ? Convert.ToInt32(CSDepositLife4.Value) : 0; //IfCellHasValue(sheet, rowIterator, 78);

                        var CSScheme4 = IfCellHasValue(sheet, rowIterator, 79, "int", sheetName, "CSScheme4", "BuroCustomerInfo");
                        customer.CSScheme4 = CSScheme4.Result == true ? Convert.ToInt32(CSScheme4.Value) : 0; //IfCellHasValue(sheet, rowIterator, 79);

                        var CSNomineeName4 = IfCellHasValue(sheet, rowIterator, 80, "string", sheetName, "CSNomineeName4", "BuroCustomerInfo");
                        customer.CSNomineeName4 = CSNomineeName4.Result == true ? CSNomineeName4.Value : null; //IfCellHasValue(sheet, rowIterator, 80);

                        var CSRelation4 = IfCellHasValue(sheet, rowIterator, 81, "string", sheetName, "CSRelation4", "BuroCustomerInfo");
                        customer.CSRelation4 = CSRelation4.Result == true ? CSRelation4.Value : null; //IfCellHasValue(sheet, rowIterator, 81);

                        var CSNomineeAddress4 = IfCellHasValue(sheet, rowIterator, 82, "string", sheetName, "CSNomineeAddress4", "BuroCustomerInfo");
                        customer.CSNomineeAddress4 = CSNomineeAddress4.Result == true ? CSNomineeAddress4.Value : null; //IfCellHasValue(sheet, rowIterator, 82);

                        var CSRealizableInstNum4 = IfCellHasValue(sheet, rowIterator, 83, "int", sheetName, "CSRealizableInstNum4", "BuroCustomerInfo");
                        customer.CSRealizableInstNum4 = CSRealizableInstNum4.Result == true ? Convert.ToInt32(CSRealizableInstNum4.Value) : 0; //IfCellHasValue(sheet, rowIterator, 83);

                        var CSRealizedInstNum4 = IfCellHasValue(sheet, rowIterator, 84, "int", sheetName, "CSRealizedInstNum4", "BuroCustomerInfo");
                        customer.CSRealizedInstNum4 = CSRealizedInstNum4.Result == true ? Convert.ToInt32(CSRealizedInstNum4.Value) : 0; //IfCellHasValue(sheet, rowIterator, 84);

                        var CSInterest4 = IfCellHasValue(sheet, rowIterator, 85, "decimal", sheetName, "CSInterest4", "BuroCustomerInfo");
                        customer.CSInterest4 = CSInterest4.Result == true ? Convert.ToDecimal(CSInterest4.Value) : 0; //IfCellHasValue(sheet, rowIterator, 85);

                        var CSFine4 = IfCellHasValue(sheet, rowIterator, 86, "decimal", sheetName, "CSFine4", "BuroCustomerInfo");
                        customer.CSFine4 = CSFine4.Result == true ? Convert.ToDecimal(CSFine4.Value) : 0; // IfCellHasValue(sheet, rowIterator, 86);

                        var CSClosingBalnce4 = IfCellHasValue(sheet, rowIterator, 87, "decimal", sheetName, "CSClosingBalnce4", "BuroCustomerInfo");
                        customer.CSClosingBalnce4 = CSClosingBalnce4.Result == true ? Convert.ToDecimal(CSClosingBalnce4.Value) : 0; //IfCellHasValue(sheet, rowIterator, 87);

                        var RVSProductCode = IfCellHasValue(sheet, rowIterator, 88, "string", sheetName, "RVSProductCode", "BuroCustomerInfo");
                        customer.RVSProductCode = RVSProductCode.Result == true ? RVSProductCode.Value : null; //IfCellHasValue(sheet, rowIterator, 88);

                        var RVSOpeningDate = IfCellHasValue(sheet, rowIterator, 89, "DateTime", sheetName, "RVSOpeningDate", "BuroCustomerInfo");
                        customer.RVSOpeningDate = RVSOpeningDate.Result == true ? Convert.ToDateTime(RVSOpeningDate.Value) : DateTime.Now; //IfCellHasValue(sheet, rowIterator, 89);

                        var RVSNomineeName = IfCellHasValue(sheet, rowIterator, 90, "string", sheetName, "RVSNomineeName", "BuroCustomerInfo");
                        customer.RVSNomineeName = RVSNomineeName.Result == true ? RVSNomineeName.Value : null; //IfCellHasValue(sheet, rowIterator, 90);

                        var RVSRelation = IfCellHasValue(sheet, rowIterator, 91, "string", sheetName, "RVSRelation", "BuroCustomerInfo");
                        customer.RVSRelation = RVSRelation.Result == true ? RVSRelation.Value : null; //IfCellHasValue(sheet, rowIterator, 91);

                        var RVSNomineeAddress = IfCellHasValue(sheet, rowIterator, 92, "string", sheetName, "RVSNomineeAddress", "BuroCustomerInfo");
                        customer.RVSNomineeAddress = RVSNomineeAddress.Result == true ? RVSNomineeAddress.Value : null; //IfCellHasValue(sheet, rowIterator, 92);

                        var RVSInterest = IfCellHasValue(sheet, rowIterator, 93, "decimal", sheetName, "RVSInterest", "BuroCustomerInfo");
                        customer.RVSInterest = RVSInterest.Result == true ? Convert.ToDecimal(RVSInterest.Value) : 0; //IfCellHasValue(sheet, rowIterator, 93);

                        var RVSClosingBalance = IfCellHasValue(sheet, rowIterator, 94, "decimal", sheetName, "RVSClosingBalance", "BuroCustomerInfo");
                        customer.RVSClosingBalance = RVSClosingBalance.Result == true ? Convert.ToDecimal(RVSClosingBalance.Value) : 0; //IfCellHasValue(sheet, rowIterator, 94);

                        var RVSProductCode2 = IfCellHasValue(sheet, rowIterator, 95, "string", sheetName, "RVSProductCode2", "BuroCustomerInfo");
                        customer.RVSProductCode2 = RVSProductCode2.Result == true ? RVSProductCode2.Value : null; //IfCellHasValue(sheet, rowIterator, 95);

                        var RVSOpeningDate2 = IfCellHasValue(sheet, rowIterator, 96, "DateTime", sheetName, "RVSOpeningDate2", "BuroCustomerInfo");
                        customer.RVSOpeningDate2 = RVSOpeningDate2.Result == true ? Convert.ToDateTime(RVSOpeningDate2.Value) : DateTime.Now; //IfCellHasValue(sheet, rowIterator, 96);

                        var RVSNomineeName2 = IfCellHasValue(sheet, rowIterator, 97, "string", sheetName, "RVSNomineeName2", "BuroCustomerInfo");
                        customer.RVSNomineeName2 = RVSNomineeName2.Result == true ? RVSNomineeName2.Value : null; //IfCellHasValue(sheet, rowIterator, 97);

                        var RVSRelation2 = IfCellHasValue(sheet, rowIterator, 98, "string", sheetName, "RVSRelation2", "BuroCustomerInfo");
                        customer.RVSRelation2 = RVSRelation2.Result == true ? RVSRelation2.Value : null; //IfCellHasValue(sheet, rowIterator, 98);

                        var RVSNomineeAddress2 = IfCellHasValue(sheet, rowIterator, 99, "string", sheetName, "RVSNomineeAddress2", "BuroCustomerInfo");
                        customer.RVSNomineeAddress2 = RVSNomineeAddress2.Result == true ? RVSNomineeAddress2.Value : null; //IfCellHasValue(sheet, rowIterator, 99);

                        var RVSInterest2 = IfCellHasValue(sheet, rowIterator, 100, "decimal", sheetName, "RVSInterest2", "BuroCustomerInfo");
                        customer.RVSInterest2 = RVSInterest2.Result == true ? Convert.ToDecimal(RVSInterest2.Value) : 0; // IfCellHasValue(sheet, rowIterator, 100);




                        var RVSClosingBalance2 = IfCellHasValue(sheet, rowIterator, 101, "decimal", sheetName, "RVSClosingBalance2", "BuroCustomerInfo");
                        customer.RVSClosingBalance2 = RVSClosingBalance2.Result == true ? Convert.ToDecimal(RVSClosingBalance2.Value) : 0; // IfCellHasValue(sheet, rowIterator, 101);

                        var GroupNo = IfCellHasValue(sheet, rowIterator, 102, "int", sheetName, "GroupNo", "BuroCustomerInfo");
                        customer.GroupNo = GroupNo.Result == true ? Convert.ToInt32(GroupNo.Value) : 0; // IfCellHasValue(sheet, rowIterator, 102);

                        var FormationDate = IfCellHasValue(sheet, rowIterator, 103, "DateTime", sheetName, "FormationDate", "BuroCustomerInfo");
                        customer.FormationDate = FormationDate.Result == true ? Convert.ToDateTime(FormationDate.Value) : DateTime.Now; // IfCellHasValue(sheet, rowIterator, 103);

                        var ApprovalDate = IfCellHasValue(sheet, rowIterator, 104, "DateTime", sheetName, "ApprovalDate", "BuroCustomerInfo");
                        customer.ApprovalDate = ApprovalDate.Result == true ? Convert.ToDateTime(ApprovalDate.Value) : DateTime.Now;  //IfCellHasValue(sheet, rowIterator, 104);

                        var LoanProductcode = IfCellHasValue(sheet, rowIterator, 105, "string", sheetName, "LoanProductcode", "BuroCustomerInfo");
                        customer.LoanProductcode = LoanProductcode.Result == true ? LoanProductcode.Value : null; //IfCellHasValue(sheet, rowIterator, 105);

                        var LoanCycle = IfCellHasValue(sheet, rowIterator, 106, "int", sheetName, "LoanCycle", "BuroCustomerInfo");
                        customer.LoanCycle = LoanCycle.Result == true ? Convert.ToInt32(LoanCycle.Value) : 0; //IfCellHasValue(sheet, rowIterator, 106);

                        var LoanSector = IfCellHasValue(sheet, rowIterator, 107, "int", sheetName, "LoanSector", "BuroCustomerInfo");
                        customer.LoanSector = LoanSector.Result == true ? Convert.ToInt32(LoanSector.Value) : 0; //IfCellHasValue(sheet, rowIterator, 107);

                        var LoanSubSector = IfCellHasValue(sheet, rowIterator, 108, "int", sheetName, "LoanSubSector", "BuroCustomerInfo");

                        customer.LoanSubSector = LoanSubSector.Result == true ? Convert.ToInt32(LoanSubSector.Value) : 0; //IfCellHasValue(sheet, rowIterator, 108);

                        var LoanPeriod = IfCellHasValue(sheet, rowIterator, 109, "int", sheetName, "LoanPeriod", "BuroCustomerInfo");
                        customer.LoanPeriod = LoanPeriod.Result == true ? Convert.ToInt32(LoanPeriod.Value) : 0; // IfCellHasValue(sheet, rowIterator, 109);

                        var ProposalDate = IfCellHasValue(sheet, rowIterator, 110, "DateTime", sheetName, "ProposalDate", "BuroCustomerInfo");
                        customer.ProposalDate = ProposalDate.Result == true ? Convert.ToDateTime(ProposalDate.Value) : DateTime.Now; // IfCellHasValue(sheet, rowIterator, 110);

                        var DisbursedDate = IfCellHasValue(sheet, rowIterator, 111, "DateTime", sheetName, "DisbursedDate", "BuroCustomerInfo");
                        customer.DisbursedDate = DisbursedDate.Result == true ? Convert.ToDateTime(DisbursedDate.Value) : DateTime.Now; // IfCellHasValue(sheet, rowIterator, 111);

                        var ProposedAmount = IfCellHasValue(sheet, rowIterator, 112, "decimal", sheetName, "ProposedAmount", "BuroCustomerInfo");
                        customer.ProposedAmount = ProposedAmount.Result == true ? Convert.ToDecimal(ProposedAmount.Value) : 0; // IfCellHasValue(sheet, rowIterator, 112);

                        var ApprovedAmount = IfCellHasValue(sheet, rowIterator, 113, "decimal", sheetName, "ApprovedAmount", "BuroCustomerInfo");
                        customer.ApprovedAmount = ApprovedAmount.Result == true ? Convert.ToDecimal(ApprovedAmount.Value) : 0; // IfCellHasValue(sheet, rowIterator, 113);

                        var DisbursedAmountPrincipal = IfCellHasValue(sheet, rowIterator, 114, "decimal", sheetName, "DisbursedAmountPrincipal", "BuroCustomerInfo");
                        customer.DisbursedAmountPrincipal = DisbursedAmountPrincipal.Result == true ? Convert.ToDecimal(DisbursedAmountPrincipal.Value) : 0; // IfCellHasValue(sheet, rowIterator, 114);

                        var GuaranterName = IfCellHasValue(sheet, rowIterator, 115, "string", sheetName, "GuaranterName", "BuroCustomerInfo");
                        customer.GuaranterName = GuaranterName.Result == true ? GuaranterName.Value : null; // IfCellHasValue(sheet, rowIterator, 115);

                        var GuaranterAge = IfCellHasValue(sheet, rowIterator, 116, "int", sheetName, "GuaranterAge", "BuroCustomerInfo");
                        customer.GuaranterAge = GuaranterAge.Result == true ? Convert.ToInt32(GuaranterAge.Value) : 0; // IfCellHasValue(sheet, rowIterator, 116);

                        var Relation = IfCellHasValue(sheet, rowIterator, 117, "string", sheetName, "Relation", "BuroCustomerInfo");
                        customer.Relation = Relation.Result == true ? Relation.Value : null; // IfCellHasValue(sheet, rowIterator, 117);

                        var RealizableInstNum = IfCellHasValue(sheet, rowIterator, 118, "int", sheetName, "RealizableInstNum", "BuroCustomerInfo");
                        customer.RealizableInstNum = RealizableInstNum.Result == true ? Convert.ToInt32(RealizableInstNum.Value) : 0; // IfCellHasValue(sheet, rowIterator, 118);

                        var RealizedInstNum = IfCellHasValue(sheet, rowIterator, 119, "int", sheetName, "RealizedInstNum", "BuroCustomerInfo");
                        customer.RealizedInstNum = RealizedInstNum.Result == true ? Convert.ToInt32(RealizedInstNum.Value) : 0; // IfCellHasValue(sheet, rowIterator, 119);

                        var Outstanding = IfCellHasValue(sheet, rowIterator, 120, "decimal", sheetName, "Outstanding", "BuroCustomerInfo");
                        customer.Outstanding = Outstanding.Result == true ? Convert.ToDecimal(Outstanding.Value) : 0; //IfCellHasValue(sheet, rowIterator, 120);

                        var LoanProductcode2 = IfCellHasValue(sheet, rowIterator, 121, "string", sheetName, "LoanProductcode2", "BuroCustomerInfo");
                        customer.LoanProductcode2 = LoanProductcode2.Result == true ? LoanProductcode2.Value : null; //IfCellHasValue(sheet, rowIterator, 121);

                        var LoanCycle2 = IfCellHasValue(sheet, rowIterator, 122, "int", sheetName, "LoanCycle2", "BuroCustomerInfo");
                        customer.LoanCycle2 = LoanCycle2.Result == true ? Convert.ToInt32(LoanCycle2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 122);

                        var LoanSector2 = IfCellHasValue(sheet, rowIterator, 123, "int", sheetName, "LoanSector2", "BuroCustomerInfo");
                        customer.LoanSector2 = LoanSector2.Result == true ? Convert.ToInt32(LoanSector2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 123);

                        var LoanSubSector2 = IfCellHasValue(sheet, rowIterator, 124, "int", sheetName, "LoanSubSector2", "BuroCustomerInfo");
                        customer.LoanSubSector2 = LoanSubSector2.Result == true ? Convert.ToInt32(LoanSubSector2.Value) : 0; // IfCellHasValue(sheet, rowIterator, 124);

                        var LoanPeriod2 = IfCellHasValue(sheet, rowIterator, 125, "int", sheetName, "LoanPeriod2", "BuroCustomerInfo");
                        customer.LoanPeriod2 = LoanPeriod2.Result == true ? Convert.ToInt32(LoanPeriod2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 125);

                        var ProposalDate2 = IfCellHasValue(sheet, rowIterator, 126, "DateTime", sheetName, "ProposalDate2", "BuroCustomerInfo");
                        customer.ProposalDate2 = ProposalDate2.Result == true ? Convert.ToDateTime(ProposalDate2.Value) : DateTime.Now; // IfCellHasValue(sheet, rowIterator, 126);

                        var DisbursedDate2 = IfCellHasValue(sheet, rowIterator, 127, "DateTime", sheetName, "DisbursedDate2", "BuroCustomerInfo");
                        customer.DisbursedDate2 = DisbursedDate2.Result == true ? Convert.ToDateTime(DisbursedDate2.Value) : DateTime.Now; //IfCellHasValue(sheet, rowIterator, 127);

                        var ProposedAmount2 = IfCellHasValue(sheet, rowIterator, 128, "decimal", sheetName, "ProposedAmount2", "BuroCustomerInfo");
                        customer.ProposedAmount2 = ProposedAmount2.Result == true ? Convert.ToDecimal(ProposedAmount2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 128);

                        var ApprovedAmount2 = IfCellHasValue(sheet, rowIterator, 129, "decimal", sheetName, "ApprovedAmount2", "BuroCustomerInfo");
                        customer.ApprovedAmount2 = ApprovedAmount2.Result == true ? Convert.ToDecimal(ApprovedAmount2.Value) : 0; //fCellHasValue(sheet, rowIterator, 129);

                        var DisbursedAmountPrincipal2 = IfCellHasValue(sheet, rowIterator, 130, "decimal", sheetName, "DisbursedAmountPrincipal2", "BuroCustomerInfo");
                        customer.DisbursedAmountPrincipal2 = DisbursedAmountPrincipal2.Result == true ? Convert.ToDecimal(DisbursedAmountPrincipal2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 130);

                        var GuaranterName2 = IfCellHasValue(sheet, rowIterator, 131, "string", sheetName, "GuaranterName2", "BuroCustomerInfo");
                        customer.GuaranterName2 = GuaranterName2.Result == true ? GuaranterName2.Value : null; //IfCellHasValue(sheet, rowIterator, 131);

                        var GuaranterAge2 = IfCellHasValue(sheet, rowIterator, 132, "int", sheetName, "GuaranterAge2", "BuroCustomerInfo");
                        customer.GuaranterAge2 = GuaranterAge2.Result == true ? Convert.ToInt32(GuaranterAge2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 132);

                        var Relation2 = IfCellHasValue(sheet, rowIterator, 133, "string", sheetName, "Relation2", "BuroCustomerInfo");
                        customer.Relation2 = Relation2.Result == true ? Relation2.Value : null; //IfCellHasValue(sheet, rowIterator, 133);

                        var RealizableInstNum2 = IfCellHasValue(sheet, rowIterator, 134, "int", sheetName, "RealizableInstNum2", "BuroCustomerInfo");
                        customer.RealizableInstNum2 = RealizableInstNum2.Result == true ? Convert.ToInt32(RealizableInstNum2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 134);

                        var RealizedInstNum2 = IfCellHasValue(sheet, rowIterator, 135, "int", sheetName, "RealizedInstNum2", "BuroCustomerInfo");
                        customer.RealizedInstNum2 = RealizedInstNum2.Result == true ? Convert.ToInt32(RealizedInstNum2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 135);

                        var Outstanding2 = IfCellHasValue(sheet, rowIterator, 136, "decimal", sheetName, "Outstanding2", "BuroCustomerInfo");
                        customer.Outstanding2 = Outstanding2.Result == true ? Convert.ToDecimal(Outstanding2.Value) : 0; //IfCellHasValue(sheet, rowIterator, 136);

                        var BA = IfCellHasValue(sheet, rowIterator, 137, "string", sheetName, "BA", "BuroCustomerInfo");
                        customer.BA = BA.Result == true ? BA.Value : null; //IfCellHasValue(sheet, rowIterator, 137);

                        var BM = IfCellHasValue(sheet, rowIterator, 138, "string", sheetName, "BM", "BuroCustomerInfo");
                        customer.BM = BM.Result == true ? BM.Value : null; // IfCellHasValue(sheet, rowIterator, 138);
                        if (customer.BranchCode != null)
                        {
                            customerList.Add(customer);
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                //}
            }
            return customerList;
        }
        private List<BuroCenterInfo> GenerateCenterInfoList(ExcelWorksheet sheet, int noOfRow)
        {
            var centerList = new List<BuroCenterInfo>();
            var sheetName = sheet.Name;
            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
            {
                try
                {
               
                var center = new BuroCenterInfo();

                var ExcelSl = IfCellHasValue(sheet, rowIterator, 1, "int", sheetName, "Sl", "BuroCenterInfo");
                center.Sl = ExcelSl.Result == true ? Convert.ToInt32(ExcelSl.Value) : 0; //IfCellHasValue(sheet, rowIterator, 2);

                var BranchCode = IfCellHasValue(sheet, rowIterator, 2, "string", sheetName, "BranchCode", "BuroCenterInfo");
                center.BranchCode = BranchCode.Result == true ? BranchCode.Value : null;//IfCellHasValue(sheet, rowIterator, 2);

                var CenterType = IfCellHasValue(sheet, rowIterator, 3, "string", sheetName, "CenterType", "BuroCenterInfo");
                center.CenterType = CenterType.Result == true ? CenterType.Value : null;//IfCellHasValue(sheet, rowIterator, 3);

                var CenterID = IfCellHasValue(sheet, rowIterator, 4, "string", sheetName, "CenterID", "BuroCenterInfo");
                center.CenterID = CenterID.Result == true ? CenterID.Value : null;//IfCellHasValue(sheet, rowIterator, 4);

                var CenterName = IfCellHasValue(sheet, rowIterator, 5, "string", sheetName, "CenterName", "BuroCenterInfo");
                center.CenterName = CenterName.Result == true ? CenterName.Value : null;//IfCellHasValue(sheet, rowIterator, 5);

                var CenterOpeningDate = IfCellHasValue(sheet, rowIterator, 6, "DateTime", sheetName, "CenterOpeningDate", "BuroCenterInfo");
                center.CenterOpeningDate = CenterOpeningDate.Result == true ? Convert.ToDateTime(CenterOpeningDate.Value) : DateTime.Now;//IfCellHasValue(sheet, rowIterator, 6);

                var CenterVillage = IfCellHasValue(sheet, rowIterator, 7, "string", sheetName, "CenterVillage", "BuroCenterInfo");
                center.CenterVillage = CenterVillage.Result == true ? CenterVillage.Value : null;//IfCellHasValue(sheet, rowIterator, 7);

                var CenterPO = IfCellHasValue(sheet, rowIterator, 8, "string", sheetName, "CenterPO", "BuroCenterInfo");
                center.CenterPO = CenterPO.Result == true ? CenterPO.Value : null;//IfCellHasValue(sheet, rowIterator, 8);

                var CenterUnion = IfCellHasValue(sheet, rowIterator, 9, "string", sheetName, "CenterUnion", "BuroCenterInfo");
                center.CenterUnion = CenterUnion.Result == true ? CenterUnion.Value : null;//IfCellHasValue(sheet, rowIterator, 9);

                var CenterThana = IfCellHasValue(sheet, rowIterator, 10, "string", sheetName, "CenterThana", "BuroCenterInfo");
                center.CenterThana = CenterThana.Result == true ? CenterThana.Value : null;//IfCellHasValue(sheet, rowIterator, 10);

                var CenterDistrict = IfCellHasValue(sheet, rowIterator, 11, "string", sheetName, "CenterDistrict", "BuroCenterInfo");
                center.CenterDistrict = CenterDistrict.Result == true ? CenterDistrict.Value : null;//IfCellHasValue(sheet, rowIterator, 11);

                var CenterTime = IfCellHasValue(sheet, rowIterator, 12, "int", sheetName, "CenterTime", "BuroCenterInfo");
                center.CenterTime = CenterTime.Result == true ? Convert.ToInt32(CenterTime.Value) : 0;//IfCellHasValue(sheet, rowIterator, 12);

                var CenterDay = IfCellHasValue(sheet, rowIterator, 13, "string", sheetName, "CenterDay", "BuroCenterInfo");
                center.CenterDay = CenterDay.Result == true ? CenterDay.Value : null;//IfCellHasValue(sheet, rowIterator, 13);

                var PO_APO = IfCellHasValue(sheet, rowIterator, 14, "string", sheetName, "PO_APO", "BuroCenterInfo");
                center.PO_APO = PO_APO.Result == true ? PO_APO.Value : null;//IfCellHasValue(sheet, rowIterator, 14);

                var BA_ABA = IfCellHasValue(sheet, rowIterator, 15, "string", sheetName, "BA_ABA", "BuroCenterInfo");
                center.BA_ABA = BA_ABA.Result == true ? BA_ABA.Value : null;//IfCellHasValue(sheet, rowIterator, 15);

                var BM_ABM = IfCellHasValue(sheet, rowIterator, 16, "string", sheetName, "BM_ABM", "BuroCenterInfo");
                center.BM_ABM = BM_ABM.Result == true ? BM_ABM.Value : null;//IfCellHasValue(sheet, rowIterator, 16);

                var CenterChief = IfCellHasValue(sheet, rowIterator, 17, "string", sheetName, "CenterChief", "BuroCenterInfo");
                center.CenterChief = CenterChief.Result == true ? CenterChief.Value : null;//IfCellHasValue(sheet, rowIterator, 17);

                var AssCenterChief = IfCellHasValue(sheet, rowIterator, 18, "string", sheetName, "AssCenterChief", "BuroCenterInfo");
                center.AssCenterChief = AssCenterChief.Result == true ? AssCenterChief.Value : null;//IfCellHasValue(sheet, rowIterator, 18);

                var CenterLocation = IfCellHasValue(sheet, rowIterator, 19, "string", sheetName, "CenterLocation", "BuroCenterInfo");
                center.CenterLocation = CenterLocation.Result == true ? CenterLocation.Value : null;//IfCellHasValue(sheet, rowIterator, 19);
                    if (center.BranchCode!=null) {
                        centerList.Add(center);
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            return centerList;
        }
        private List<BuroStaffInfo> GenerateStaffInfoList(ExcelWorksheet sheet, int noOfRow)
        {
            var staffList = new List<BuroStaffInfo>();
            var sheetName = sheet.Name;


            for (int rowIterator = 5; rowIterator < noOfRow; rowIterator++)
            {
                try
                {

                    var staff = new BuroStaffInfo();

                    var ExcelSl = IfCellHasValue(sheet, rowIterator, 1, "int", sheetName, "Sl", "BuroStaffInfo");
                    staff.Sl = ExcelSl.Result == true ? Convert.ToInt32(ExcelSl.Value) : 0;//(InsertWrongStaffInfoList(rowIterator, "BranchCode", "string")==""?null:null);

                    var BranchCode = IfCellHasValue(sheet, rowIterator, 2, "string", sheetName, "BranchCode", "BuroStaffInfo");
                    staff.BranchCode = BranchCode.Result == true ? BranchCode.Value : null;//(InsertWrongStaffInfoList(rowIterator, "BranchCode", "string")==""?null:null);

                    var StaffName = IfCellHasValue(sheet, rowIterator, 3, "string", sheetName, "StaffName", "BuroStaffInfo");
                    staff.StaffName = StaffName.Result == true ? StaffName.Value : null; //IfCellHasValue(sheet, rowIterator, 3);

                    var StaffPin = IfCellHasValue(sheet, rowIterator, 4, "string", sheetName, "StaffPin", "BuroStaffInfo");
                    staff.StaffPin = StaffPin.Result == true ? StaffPin.Value : null; //IfCellHasValue(sheet, rowIterator, 4);

                    var Designation = IfCellHasValue(sheet, rowIterator, 5, "string", sheetName, "Designation", "BuroStaffInfo");
                    staff.Designation = Designation.Result == true ? Designation.Value : null;//IfCellHasValue(sheet, rowIterator, 5);

                    var BasicSalary = IfCellHasValue(sheet, rowIterator, 6, "decimal", sheetName, "BasicSalary", "BuroStaffInfo");
                    staff.BasicSalary = BasicSalary.Result == true ? Convert.ToDecimal(BasicSalary.Value) : 0;//IfCellHasValue(sheet, rowIterator, 6);

                    var HouseRent = IfCellHasValue(sheet, rowIterator, 7, "decimal", sheetName, "HouseRent", "BuroStaffInfo");
                    staff.HouseRent = HouseRent.Result == true ? Convert.ToDecimal(HouseRent.Value) : 0;//IfCellHasValue(sheet, rowIterator, 7);

                    var Medical = IfCellHasValue(sheet, rowIterator, 8, "decimal", sheetName, "Medical", "BuroStaffInfo");
                    staff.Medical = Medical.Result == true ? Convert.ToDecimal(Medical.Value) : 0;//IfCellHasValue(sheet, rowIterator, 8);

                    var Comm_Collection = IfCellHasValue(sheet, rowIterator, 9, "decimal", sheetName, "Comm_Collection", "BuroStaffInfo");
                    staff.Comm_Collection = Comm_Collection.Result == true ? Convert.ToDecimal(Comm_Collection.Value) : 0;//IfCellHasValue(sheet, rowIterator, 9);

                    var Comm_Mobile = IfCellHasValue(sheet, rowIterator, 10, "decimal", sheetName, "Comm_Mobile", "BuroStaffInfo");
                    staff.Comm_Mobile = Comm_Mobile.Result == true ? Convert.ToDecimal(Comm_Mobile.Value) : 0;//IfCellHasValue(sheet, rowIterator, 10);

                    var Comm_Convenience = IfCellHasValue(sheet, rowIterator, 11, "decimal", sheetName, "Comm_Convenience", "BuroStaffInfo");
                    staff.Comm_Convenience = Comm_Convenience.Result == true ? Convert.ToDecimal(Comm_Convenience.Value) : 0;//IfCellHasValue(sheet, rowIterator, 11);

                    var TotalSalary = IfCellHasValue(sheet, rowIterator, 12, "decimal", sheetName, "TotalSalary", "BuroStaffInfo");
                    staff.TotalSalary = TotalSalary.Result == true ? Convert.ToDecimal(TotalSalary.Value) : 0;//IfCellHasValue(sheet, rowIterator, 12);

                    var SelfPFContribution = IfCellHasValue(sheet, rowIterator, 13, "decimal", sheetName, "SelfPFContribution", "BuroStaffInfo");
                    staff.SelfPFContribution = SelfPFContribution.Result == true ? Convert.ToDecimal(SelfPFContribution.Value) : 0;//IfCellHasValue(sheet, rowIterator, 13);

                    var StaffBima = IfCellHasValue(sheet, rowIterator, 14, "decimal", sheetName, "StaffBima", "BuroStaffInfo");
                    staff.StaffBima = StaffBima.Result == true ? Convert.ToDecimal(StaffBima.Value) : 0;//IfCellHasValue(sheet, rowIterator, 14);

                    var Health = IfCellHasValue(sheet, rowIterator, 15, "decimal", sheetName, "Health", "BuroStaffInfo");
                    staff.Health = Health.Result == true ? Convert.ToDecimal(Health.Value) : 0;//IfCellHasValue(sheet, rowIterator, 15);

                    var NetSalaryPayment = IfCellHasValue(sheet, rowIterator, 16, "decimal", sheetName, "NetSalaryPayment", "BuroStaffInfo");
                    staff.NetSalaryPayment = NetSalaryPayment.Result == true ? Convert.ToDecimal(NetSalaryPayment.Value) : 0;//IfCellHasValue(sheet, rowIterator, 16);

                    var OfficePF = IfCellHasValue(sheet, rowIterator, 17, "decimal", sheetName, "OfficePF", "BuroStaffInfo");
                    staff.OfficePF = OfficePF.Result == true ? Convert.ToDecimal(OfficePF.Value) : 0;//IfCellHasValue(sheet, rowIterator, 17);

                    var PaymentPFBoard = IfCellHasValue(sheet, rowIterator, 18, "decimal", sheetName, "PaymentPFBoard", "BuroStaffInfo");
                    staff.PaymentPFBoard = PaymentPFBoard.Result == true ? Convert.ToDecimal(PaymentPFBoard.Value) : 0;//IfCellHasValue(sheet, rowIterator, 18);

                    var TotalSalary_OtherAllownce = IfCellHasValue(sheet, rowIterator, 19, "decimal", sheetName, "TotalSalary_OtherAllownce", "BuroStaffInfo");
                    staff.TotalSalary_OtherAllownce = TotalSalary_OtherAllownce.Result == true ? Convert.ToDecimal(TotalSalary_OtherAllownce.Value) : 0;//IfCellHasValue(sheet, rowIterator, 19);
                    if (staff.BranchCode!=null) {
                        staffList.Add(staff);
                    }                    
                }

                catch (Exception e)
                {
                    var a = rowIterator;
                    throw;
                }
            }
            return staffList;
        }
        private ValidateColumn IfCellHasValue(ExcelWorksheet sheet, int rowIterator, int columnValue, string dataType, string sheetName, string cellHeader,string tableName)
        {
            ValidateColumn result = new ValidateColumn();
            var readCellValue = sheet.Cells[rowIterator, columnValue].Value;
            var ExcelSl = sheet.Cells[rowIterator, 1].Value==null?0: Convert.ToInt32(sheet.Cells[rowIterator, 1].Value.ToString());
            var AttrLength = 0;
            
            var CellValue = "";
            try
            {
                if (readCellValue != null && readCellValue.ToString().Trim() != "")
                {
                    CellValue = readCellValue.ToString().Trim();

                    if (dataType == "int")
                    {
                        result.Result = isInt(CellValue);
                    }
                    else if (dataType == "decimal")
                    {
                        result.Result = isDecimal(CellValue);
                    }
                    else if (dataType == "string")
                    {
                        var param = new { TableName = tableName, AttributeName = cellHeader };
                        var AttributeLength = unlimitedReportService.GetDataWithParameter(param, "SP_GetAttribureMaxLength");
                        AttrLength = Convert.ToInt32(AttributeLength.Tables[0].Rows[0][0].ToString());
                        if (CellValue.Length > AttrLength) {
                            result.Result = false;
                        }
                        else
                        {
                            result.Result = isString(CellValue);
                        }

                        
                    }
                    else if (dataType == "DateTime")
                    {
                        result.Result = isDateTime(CellValue);
                    }
                    result.Value = CellValue;
                }
                else
                {
                    result.Value = null;
                    result.Result = false;
                }
                if (result.Result == false && readCellValue != null && readCellValue.ToString().Trim() != "")
                {
                    if (tableName == "BuroCustomerInfo")
                    {
                        InsertWrongCustomerInfoList(ExcelSl, cellHeader, dataType, sheetName, AttrLength);
                        //InsertWrongCustomerInfoList(rowIterator, cellHeader, dataType, sheetName);
                    }
                    if (tableName == "BuroCenterInfo")
                    {
                        InsertWrongCenterInfoList(ExcelSl, cellHeader, dataType, sheetName, AttrLength);
                        //InsertWrongCenterInfoList(rowIterator, cellHeader, dataType, sheetName);
                    }
                    if (tableName == "BuroStaffInfo")
                    {
                        InsertWrongStaffInfoList(ExcelSl, cellHeader, dataType, sheetName, AttrLength);
                        //InsertWrongStaffInfoList(rowIterator, cellHeader, dataType, sheetName);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return result;
        }

        private void InsertWrongCustomerInfoList(int rowIterator, string cellHeader, string dataType, string sheetName,int AttrLength)
        {
            var data_Type = "";
            if (dataType == "int")
            {
                data_Type = "Number";
            }
            else if (dataType == "decimal")
            {
                data_Type = "Decimal Number";
            }
            else if (dataType == "string")
            {
                data_Type = "Text (Max Length "+AttrLength+")";
            }
            else if (dataType == "DateTime")
            {
                data_Type = "Date (ex: 31/12/2015)";
            }
            var wrongCustomer = new ValidateResultModel();
            wrongCustomer.RowSl = rowIterator;
            wrongCustomer.ColumnName = cellHeader;
            wrongCustomer.SheetName = sheetName;
            wrongCustomer.Message = "Data must be " + data_Type;
            WrongCustomerInfoList.Add(wrongCustomer);
        }
        private void InsertWrongCenterInfoList(int rowIterator, string cellHeader, string dataType, string sheetName ,int AttrLength)
        {
            var data_Type = "";
            if (dataType == "int")
            {
                data_Type = "Number";
            }
            else if (dataType == "decimal")
            {
                data_Type = "Decimal Number";
            }
            else if (dataType == "string")
            {
                data_Type = "Text (Max Length " + AttrLength + ")";
            }
            else if (dataType == "DateTime")
            {
                data_Type = "Date (ex: 31/12/2015)";
            }
            var wrongCenter = new ValidateResultModel();
            wrongCenter.RowSl = rowIterator;
            wrongCenter.ColumnName = cellHeader;
            wrongCenter.SheetName = sheetName;
            wrongCenter.Message = "Data must be " + data_Type;
            WrongCenterInfoList.Add(wrongCenter);
        }
        private void InsertWrongStaffInfoList(int rowIterator, string cellHeader, string dataType, string sheetName, int AttrLength)
        {
            var data_Type = "";
            if (dataType == "int")
            {
                data_Type = "Number";
            }
            else if (dataType == "decimal")
            {
                data_Type = "Decimal Number";
            }
            else if (dataType == "string")
            {
                data_Type = "Text (Max Length " + AttrLength + ")";
            }
            else if (dataType == "DateTime")
            {
                data_Type = "Date (ex: 31/12/2015)";
            }
            var wrongStaff = new ValidateResultModel();
            wrongStaff.RowSl = rowIterator;
            wrongStaff.ColumnName = cellHeader;
            wrongStaff.SheetName = sheetName;
            wrongStaff.Message = "Data must be " + data_Type;
            WrongStaffInfoList.Add(wrongStaff);
        }
        public bool isInt(string num)
        {
            bool res = true;
            try
            {
                var intNunber = Convert.ToInt32(num);
                res = true;
            }
            catch (Exception e)
            {
                res = false;
            }
            return res;
        }
        public bool isDecimal(string num)
        {
            bool res = true;
            try
            {
                var decimalNumber = Convert.ToDecimal(num);
                res = true;

            }
            catch (Exception e)
            {
                res = false;
            }
            return res;
        }
        public bool isString(string num)
        {
            bool res = true;
            try
            {
                var stringData = Convert.ToString(num);
                res = true;

            }
            catch (Exception e)
            {
                res = false;
            }
            return res;
        }
        public bool isDateTime(string num)
        {
            bool res = true;
            try
            {
                var dateTimeData = Convert.ToDateTime(num);
                res = true;

            }
            catch (Exception e)
            {
                res = false;
            }
            return res;
        }
        private void SaveStaffList(List<BuroStaffInfo> staffList)
        {
            try
            {
                gBankerDbContext db = new gBankerDbContext();
                db.BuroStaffInfos.AddRange(staffList);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private void SaveCenterList(List<BuroCenterInfo> centerList)
        {
            try
            {
                gBankerDbContext db = new gBankerDbContext();
                db.BuroCenterInfos.AddRange(centerList);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private void SaveCustomerList(List<BuroCustomerInfo> customerList)
        {
            try
            {
                gBankerDbContext db = new gBankerDbContext(); ;
                //db.BuroCustomerInfos.AddRange(customerList);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<dynamic> SpDynamicCalling(string BranchCode, string SpName)
        {
            ListToDataTableHelper convertDT = new ListToDataTableHelper();
            DataTable dd = new DataTable();
            var param = new { BranchCode = BranchCode };
            dd = unlimitedReportService.GetDataWithParameter(param, SpName).Tables[0];


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

            return result;
        }
        public List<dynamic> SpDynamicCalling(string BranchCode, string DateTo, string SpName)
        {
            ListToDataTableHelper convertDT = new ListToDataTableHelper();
            DataTable dd = new DataTable();
            var param = new { BranchCode = BranchCode, DateTo = DateTo };
            dd = unlimitedReportService.GetDataWithParameter(param, SpName).Tables[0];


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

            return result;
        }

    }
}
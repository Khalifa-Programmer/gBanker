//using ExcelMigration.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class BuroCustomerInfoViewModel
    {
        public int? Sl { get; set; }
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }
        public int OfficeID { get; set; }
        public string CenterType { get; set; }
        public string CustomerType { get; set; }
        public string CenterNo { get; set; }
        public string CustomerId { get; set; }
        public string CustomrName { get; set; }
        public DateTime? SurveyDate { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public int? PO { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string FatherName { get; set; }
        public string SpouseName { get; set; }
        public string NationalID { get; set; }
        public string MobileNo { get; set; }
        public string Village { get; set; }
        public string PostOffice { get; set; }
        public string Union { get; set; }
        public string Thana { get; set; }
        public string District { get; set; }
        public string SurveyStatus { get; set; }
        public string Occupation { get; set; }
        public string MaritalStatus { get; set; }
        public int? NoOfMaleFamilyMember { get; set; }
        public int? NoOfFemaleFamilyMember { get; set; }
        public int? NoOfEarningFamilyMember { get; set; }
        public string FamilyHeadName { get; set; }
        public decimal? AnnualFamilyIncome { get; set; }
        public decimal? PaidTaxAmount { get; set; }
        public int? FamilyLand { get; set; }
        public decimal? FamilyAssetAmount { get; set; }
        public string GSProductCode { get; set; }
        public DateTime? GSAccountOpeningDate { get; set; }
        public string GSNomineeName { get; set; }
        public string GSRelation { get; set; }
        public string GSNomineeAddress { get; set; }
        public decimal? GSInterest { get; set; }
        public decimal? GSClosingBaalnce { get; set; }
        public string CSProductCode { get; set; }
        public DateTime? CSAccountOpeningDate { get; set; }
        public int? CSDepositLife { get; set; }
        public int? CSScheme { get; set; }
        public string CSNomineeName { get; set; }
        public string CSRelation { get; set; }
        public string CSNomineeAddress { get; set; }
        public int? CSRealizableInstNum { get; set; }
        public int? CSRealizedInstNum { get; set; }
        public decimal? CSInterest { get; set; }
        public decimal? CSFine { get; set; }
        public decimal? CSClosingBalance { get; set; }
        public string CSProductcode2 { get; set; }
        public DateTime? CSAccountOpeningDate2 { get; set; }
        public int? CSDepositLife2 { get; set; }
        public int? CSScheme2 { get; set; }
        public string CSNomineeName2 { get; set; }
        public string CSRelation2 { get; set; }
        public string CSNomineeAddress2 { get; set; }
        public int? CSRealizableInstNum2 { get; set; }
        public int? CSRealizedInstNum2 { get; set; }
        public decimal? CSInterest2 { get; set; }
        public decimal? CSFine2 { get; set; }
        public decimal? CSClosingBaalnce2 { get; set; }
        public string CSProductcode3 { get; set; }
        public DateTime? CSAccountOpeningDate3 { get; set; }
        public int CSDepositLife3 { get; set; }
        public int CSScheme3 { get; set; }
        public string CSNomineeName3 { get; set; }
        public string CSRelation3 { get; set; }
        public string CSNomineeAddress3 { get; set; }
        public int? CSRealizableInstNum3 { get; set; }
        public int? CSRealizedInstNum3 { get; set; }
        public decimal? CSInterest3 { get; set; }
        public decimal? CSFine3 { get; set; }
        public decimal? CSClosingBaalnce3 { get; set; }
        public string CSProductcode4 { get; set; }
        public DateTime? CSAccountOpeningDate4 { get; set; }
        public int? CSDepositLife4 { get; set; }
        public int? CSScheme4 { get; set; }
        public string CSNomineeName4 { get; set; }
        public string CSRelation4 { get; set; }
        public string CSNomineeAddress4 { get; set; }
        public int? CSRealizableInstNum4 { get; set; }
        public int? CSRealizedInstNum4 { get; set; }
        public decimal? CSInterest4 { get; set; }
        public decimal? CSFine4 { get; set; }
        public decimal? CSClosingBaalnce4 { get; set; }
        public string RVSProductCode { get; set; }
        public DateTime? RVSOpeningDate { get; set; }
        public string RVSNomineeName { get; set; }
        public string RVSRelation { get; set; }
        public string RVSNomineeAddress { get; set; }
        public decimal? RVSInterest { get; set; }
        public decimal? RVSClosingBalance { get; set; }
        public string RVSProductCode2 { get; set; }
        public DateTime? RVSOpeningDate2 { get; set; }
        public string RVSNomineeName2 { get; set; }
        public string RVSRelation2 { get; set; }
        public string RVSNomineeAddress2 { get; set; }
        public decimal? RVSInterest2 { get; set; }
        public decimal? RVSClosingBalance2 { get; set; }
        public int? GroupNo { get; set; }
        public DateTime? FormationDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string LoanProductcode { get; set; }
        public int? LoanCycle { get; set; }
        public int? LoanSector { get; set; }
        public int? LoanSubSector { get; set; }
        public int? LoanPeriod { get; set; }
        public DateTime? ProposalDate { get; set; }
        public DateTime? DisbursedDate { get; set; }
        public decimal? ProposedAmount { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public decimal? DisbursedAmountPrincipal { get; set; }
        public string GuaranterName { get; set; }
        public int? GuaranterAge { get; set; }
        public string Relation { get; set; }
        public int? RealizableInstNum { get; set; }
        public int? RealizedInstNum { get; set; }
        public decimal? Outstanding { get; set; }
        public string LoanProductcode2 { get; set; }
        public int? LoanCycle2 { get; set; }
        public int? LoanSector2 { get; set; }
        public int? LoanSubSector2 { get; set; }
        public int? LoanPeriod2 { get; set; }
        public DateTime? ProposalDate2 { get; set; }
        public DateTime? DisbursedDate2 { get; set; }
        public decimal? ProposedAmount2 { get; set; }
        public decimal? ApprovedAmount2 { get; set; }
        public decimal? DisbursedAmountPrincipal2 { get; set; }
        public string GuaranterName2 { get; set; }
        public int? GuaranterAge2 { get; set; }
        public string Relation2 { get; set; }
        public int? RealizableInstNum2 { get; set; }
        public int? RealizedInstNum2 { get; set; }
        public decimal? Outstanding2 { get; set; }
        public string BA { get; set; }
        public string BM { get; set; }
        [Display(Name = "Checking Type")]
        public string SpName { get; set; }
        [Display(Name = "Date To")]
        public string DateTo { get; set; }

        public string ReturnMessage { get; set; }
        public int Result { get; set; }
        public bool IsSuperAdmin { get; set; }
        [Display(Name = "Upload File")]
        public HttpPostedFileBase UploadFile { get; set; }


        public List<ValidateResultModel> WrongStaffInfoList { get; set; }
        public List<ValidateResultModel> WrongCenterInfoList { get; set; }
        public List<ValidateResultModel> WrongCustomerInfoList { get; set; }
        public List<SelectListItem> BranchCodeList { get; set; }
        public List<SelectListItem> ReportSpNameList { get; set; }


        public List<dynamic> CheckStaffDataTable { get; set; }

    }// END Class

    public class ValidateResultModel
    {
        public string SheetName { get; set; }
        public int RowSl { get; set; }
        public string ColumnName { get; set; }
        public string Message { get; set; }

    }// END Namespace

    public class ValidateColumn
    {
        public string Value { get; set; }
        public bool Result { get; set; }
    }// END CLASS


}// END Namespace
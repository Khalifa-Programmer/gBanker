using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class FixedAssetViewModel
    {

        public int AssetGroupInfoID { get; set; }

        [Display(Name = "Group Id")]
        public int GroupId { get; set; }
        public string GroupCode { get; set; }

        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        [Display(Name = "Account Code")]
        public string AccountCode { get; set; }

        [Display(Name = "Asset Serial")]
        public string AssetSerial { get; set; }

        [Display(Name = "Asset Code")]
        public string AssetCode { get; set; }

        [Display(Name = "Asset Name")]
        public string AssetName { get; set; }

        [Display(Name = "Depriciable")]
        public bool Depriciable { get; set; }

        [Display(Name = "Depriciation Rate")]
        public decimal DepriciationRate { get; set; }
        public IEnumerable<SelectListItem> GroupIdList { get; set; }
        public IEnumerable<SelectListItem> AssetCodeList { get; set; }
        public IEnumerable<SelectListItem> DepriciableList { get; set; }

        /// <summary>
        /// Client
        /// </summary>
        /// 
        [Display(Name = "Client Id")]
        public long AssetClientInfoID { get; set; }

        [Display(Name = "Client Code")]
        public string AssetClientCode { get; set; }

        [Display(Name = "Client Name")]
        public string AssetClientName { get; set; }

        [Display(Name = "Client Address")]
        public string AssetClientAddress { get; set; }

        [Display(Name = "Business License No")]
        public string BusLicNo { get; set; }
        public string ClientType { get; set; }
        public string ClientLevel { get; set; }
        public string FirstLevel { get; set; }
        public string SecondLevel { get; set; }
        public string ThirdLevel { get; set; }
        public DateTime InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }


        /// <summary>
        /// Fixed Asset In
        /// </summary>
        /// 
        public long DailyTransactionId { get; set; }
        public long AssetClientId { get; set; }

        [Display(Name = "Fixed Asset In Date")]
        public DateTime FixedAssetInDate { get; set; }

        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; }
        public int TranType { get; set; }
        public IEnumerable<SelectListItem> TransactionTypeList { get; set; }
        public IEnumerable<SelectListItem> ClientList { get; set; }

        [Display(Name = "Vouchar No")]
        public string VoucherNo { get; set; }

        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Transfer value")]
        public decimal TransferValue { get; set; }

        [Display(Name = "Depriciation")]
        public decimal Depriciation { get; set; }

        [Display(Name = "Purchase Price")]
        public decimal PurchasePrice { get; set; }
        //public int SupplierId { get; set; }

        [Display(Name = "Purchase Date")]
        public string PurchaseDate { get; set; }

        [Display(Name = "Depriciation Date")]
        public DateTime DepriciationDate { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        public string DepriciationDebit { get; set; }
        public string DepriciationCredit { get; set; }
        public long AssetInfoID { get; set; }
        public int selectedOfficeId { get; set; }
        public bool? Depritiable { get; set; }
        public decimal? Deprate { get; set; }
        public string AssetGroupInfoCode { get; set; }
        public IEnumerable<SelectListItem> AccountCodeList { get; set; }

        public IEnumerable<SelectListItem> AssetGroupIdList { get; set; }
        public IEnumerable<SelectListItem> GroupNameList { get; set; }
        public IEnumerable<SelectListItem> AssetSerialList { get; set; }
        public int OrgID { get; set; }
        //TransactionType//
        public int TransactionId { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionName { get; set; }
        public string TransactionTypeInOut { get; set; }
        //TransactionType//

        //Fix Asset updates
        public int FixAssetUpdateID { get; set; }
        public string TransactionDate { get; set; }
        public int AssetQuantity { get; set; }
        public decimal TransactionValue { get; set; }
        public decimal AccumulatedDepri { get; set; }
        public decimal CurrentDepri { get; set; }
        public string ClientId { get; set; }
        public string AssetUser { get; set; }
        public bool? Usable { get; set; }
        public int OfficeID { get; set; }
        public string DepCalcDate { get; set; }
        public string ClientCode { get; set; }
        public IEnumerable<SelectListItem> UsableList { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public DateTime? InsuranceExpDate { get; set; }
        public string InsExpDate { get; set; }
        public decimal? InstallationCost { get; set; }
        public decimal? CarringCost { get; set; }
        public decimal? OtherCost { get; set; }
        public decimal? TotalCost { get; set; }
        public bool? IsCapitalizedAsset { get; set; }
        public bool? IsInstallmentAsset { get; set; }
        public decimal? DownPayment { get; set; }
        public int? InstallmentNo { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public decimal? TotalOpeningBalanceCost { get; set; }
        public decimal? OpeningDepriciationBalance { get; set; }
        public decimal? OpeningBookValue { get; set; }
        public int? ProjectID { get; set; }
        public decimal? InsuranceValue { get; set; }
        public string WarrantyGurantee { get; set; }
        public int? UsefulLifeYear { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime? PurchaseOrderDate { get; set; }
        public string PurOrderDate { get; set; }
        public string OperationDate { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public string AssetSerialMin { get; set; }
        public string AssetSerialMax { get; set; }
        public string AssetDescription { get; set; }
        public string AssetDepartment { get; set; }
        //Fix Asset updates

        //fix asset out date
        public long AssetOutID { get; set; }
        public decimal? OutPrice { get; set; }
        public string FixedAssetOutDate { get; set; }
        public int Year { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public int Month { get; set; }
        public IEnumerable<SelectListItem> MonthList { get; set; }
        public string DepriciationMethod { get; set; }
        public List<SelectListItem> DepriciationMethodList { get; set; }
        public string VATRegistrationNo { get; set; }
        public string CorporateStatus { get; set; }
        public string BusinessExperience { get; set; }
        public string TIN { get; set; }
        public string Phone { get; set; }

        public decimal? BusinessGain { get; set; }
        public decimal? CurrentDepriciation { get; set; }
        public int AssetStatus { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        public int ClientTypeID { get; set; }
        public string ClientCategory { get; set; }
        public string ProjectName { get; set; }
        public string FundingAgency { get; set; }
        public string Description { get; set; }
        public int ValuerID { get; set; }
        public string ValuerName { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }

        // Asset Transfer
        public int TransferID { get; set; }
        public int AssetID { get; set; }
        public int OfficeFrom { get; set; }
        public int TransferOfficeID { get; set; }
        public string OfficeFromName { get; set; }
        public string OfficeToName { get; set; }
        public string EffectiveDate { get; set; }
        public string AuthorisedBy { get; set; }
        public IEnumerable<SelectListItem> AssetList { get; set; }
        public string AssetSerialOld { get; set; }
        public string AssetSerialNew { get; set; }
        public long DailyTransactionIdOld { get; set; }
        public long DailyTransactionIdNew { get; set; }
        public string[] allSerialId { get; set; }
        public string[] allTransactionId { get; set; }

        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StatusDate { get; set; }



        // Asset Revaluation

        public int AssetRevaluationID { get; set; }
        public int AssetGroupID { get; set; }
        public decimal CurrAssetCost { get; set; }
        public int Valuer { get; set; }
        public decimal RevaluatedValue { get; set; }
        public decimal DeprRate { get; set; }
        public IEnumerable<SelectListItem> ValuerList { get; set; }

        // Asset Out

        public decimal? SellingPrice { get; set; }
        public decimal? AssetCost { get; set; }
        public decimal? AccumulatedDepriciation { get; set; }
        public decimal? BookValue { get; set; }
        public decimal? TotalProfitLoss { get; set; }
        public decimal? CapitalGain { get; set; }

        // Asset Partial Out

        public int AssetPartialOutID { get; set; }
        public decimal DisposalAmount { get; set; }
        public decimal NewAddition { get; set; }
        public decimal CurrCostAfterDisposal { get; set; }
        public decimal PreviousBookValue { get; set; }
        public decimal AccuDeprWholeAsset { get; set; }
        public decimal AccuDeprDisposedAsset { get; set; }
        public decimal NewBookValue { get; set; }
        //Asset Overhauling

        public int AssetOverhaulingID { get; set; }
        public decimal CurrTotalCost { get; set; }
        public decimal OverhaulingCost { get; set; }

        // Depriciation Rate Change

        public int DepRateChangeID { get; set; }
        public decimal CurrDepRate { get; set; }
        public decimal NewDepRate { get; set; }

        // Asset User
        public int UserID { get; set; }
        public int DepartmentID { get; set; }
        public int DesignationID { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> DesignationList { get; set; }
        public IEnumerable<SelectListItem> EmployeeList { get; set; }
        public IEnumerable<SelectListItem> ReportType { get; set; }
        public IEnumerable<SelectListItem> AssetGroupFrom { get; set; }
        public IEnumerable<SelectListItem> AssetGroupTo { get; set; }
        public string OfficeName { get; set; }
        public string DepriciationClosingDate { get; set; }
    }
}
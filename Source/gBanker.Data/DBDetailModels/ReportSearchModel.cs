using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class SearchFilterData
    {
        public DateTime? ProcessStartDate { get; set; }
        public DateTime?  ProcessEndDate { get; set; }
        public string  ZoneCode { get; set; }
        public string  ZoneName { get; set; }
        public string  AreaCode { get; set; }
        public string  AreaName { get; set; }
        public int OfficeID { get; set; }
        public string BranchName { get; set; }
        public string StaffID { get; set; }
        public string StaffName { get; set; }
        public int? NoOfCenter { get; set; }
        public int? NoOfMember { get; set; }
        public int? NoOfActiveMember { get; set; }
        public int?     TotalLoanee { get; set; }
        public decimal?     SavingsOutstanding { get; set; }
        public decimal?     CurrentMonthDue { get; set; }
        public decimal?     TotalDueWithoutServiceCharge { get; set; }
        public decimal?     TotalDueWithServiceCharge { get; set; }
        public int?          NoOfOverdueLoanee { get; set; }
        public decimal?         OverdueWithoutServiceCharge { get; set; }
        public decimal?         OverdueWithServiceCharge { get; set; }
        public decimal?         LoanOutstandingWithoutServiceCharge { get; set; }
        public decimal?         LoanOutstandingWithServiceCharge { get; set; }
        public string         OfficeCode { get; set; }
        public decimal?         OTR { get; set; }
        public decimal?         PAR { get; set; }
        public decimal?         Recoverable { get; set; }
        public decimal?         CurRecovery { get; set; }
        public decimal?         DueMembersLoanBalance { get; set; }
        public decimal?         DisbursementAmount { get; set; }
        public int QType { get; set; }
        public int? EmployeeId { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        
    }

    public class Rpt_DailyCollectionReceiptFoWise
    {
        public int? rType { get; set; }
        public string Item { get; set; }
        public string TrxDate { get; set; }
        public string OrganizationName { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string Samity { get; set; }
        public string Employee { get; set; }
        public string Product { get; set; }
        public decimal? Amount { get; set; }
        public byte[]  logo { get; set; }
        public string OrgAddress { get; set; }

    }// END Class


    public class Rpt_DailyCollectionReceiptFoWise_New_tbl 
    {
        public int? rType { get; set; }
        public string Item { get; set; }
        public string TrxDate { get; set; }
        public string OrganizationName { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string Samity { get; set; }
        public string Employee { get; set; }
        public string Product { get; set; }
        public decimal? Amount { get; set; }
        public byte[] logo { get; set; }
        public string OrgAddress { get; set; }
        public int CType { get; set; }
        public decimal? inWordAmount { get; set; }

    }// END CLASS


    public class Rpt_DailyCollectionReceiptFoWise_New_JSON
    {
       public string Employee { get; set; }
       public virtual List<GCollectionC> SavingsCollections { get; set; }
       public virtual List<GCollectionC> LoanCollections { get; set; }
       public virtual List<GCollectionC> ServiceChargeCollections { get; set; }

    }// END CLASS
    public class GCollectionC
    {
        public string ProductCode { get; set; }
        public decimal? Recoverable { get; set; }
        public decimal? Collection { get; set; }

    }// END CLASS

    public class Rpt_Product_tbl
    {
        public string ProductCode { get; set; }
        public byte? ProductType { get; set; } //tinyint
        public Int16 ProductID { get; set; }
        
    }// END CLASS






}

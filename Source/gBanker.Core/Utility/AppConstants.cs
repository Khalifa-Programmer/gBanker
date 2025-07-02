using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Core.Utility
{
    public static class SMSSendRequestTypeConstants
    {
        public const string SINGLE_SMS = "SINGLE_SMS";
        public const string GENERAL_CAMPAIGN = "GENERAL_CAMPAIGN";
        public const string OTP = "OTP";
        public const string MULTIBODY_CAMPAIGN = "MULTIBODY_CAMPAIGN";
    }


    public static class SMSSenderConstants_Rtcom
    {
        public static string ApiKey = "7a41cf050f70e7afc86819c72415da2c57e04d81";  //"KEY-y7vgh30ujryg54a8x20mir2xa4rojbn3";
        public static string SenderId = "8809610935162"; //"j8NtlP3WwMA6BcMd";
        public static string ApiBaseUrl = "https://api.rtcom.xyz/onetomany";
        public static string MessageType = "unicode";
        public static string AccountCode = "30000319";
        public static string TransactionType = "T";

    }

    public static class SMSMessageTypeConstants
    {
        public const string UNICODE = "UNICODE";
        public const string TEXT = "TEXT";
    }

    public static class SMSApiResponseConstants
    {
        public const string SUCCESS = "200";
        public const string FAILED = "400";
    }

    public static class UserRoleConstants
    {
        public const string Administrator = "Administrator";
    }

    public static class ConsolidateReportConstants
    {
        public static string FA = "FA";
        public static string SCP = "SCP";
    }

    public static class MFIReportTypeConstants
    {
        public static string Consolidate = "C";
    }

    public static class SMSSenderConstants
    {
        public static string ApiKey = "KEY-8tot4n9p4c3lrojjcsmo3b64dn6xr6bd";  //"KEY-y7vgh30ujryg54a8x20mir2xa4rojbn3";
        public static string ApiSecret = "CHf0Kohb6gTxRWhE"; //"j8NtlP3WwMA6BcMd";
        public static string ApiUrl = "https://portal.adnsms.com/api";
    }

    public static class AccChartLavelConstants
    {
        public static string FirstLevel = "FirstLevel";
        public static string SecondLevel = "SecondLevel";
        public static string ThirdLevel = "ThirdLevel";

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "First Level", Value = FirstLevel, Selected = false},
                    new ConstantDropdownItem {Text = "Second Level", Value = SecondLevel, Selected = false},
                    new ConstantDropdownItem {Text = "Third Level", Value = ThirdLevel, Selected = false}
                };
            }
        }
    }
    public static class EmploymentTypeConstants
    {
        public static string Employment_up_to_Last_Half_Year = "Employment up to Last Half Year";
        public static string Employment_Retained_From_Last_Half_Year = "Employment Retained From Last Half Year";
        public static string New_Emloyee_In_Current_Half_Year = "New Emloyee In Current Half Year";

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Employment up to Last Half Year", Value = Employment_up_to_Last_Half_Year, Selected = false},
                    new ConstantDropdownItem {Text = "Employment Retained From Last Half Year", Value = Employment_Retained_From_Last_Half_Year, Selected = false},
                    new ConstantDropdownItem {Text = "New Emloyee In Current Half Year", Value = New_Emloyee_In_Current_Half_Year, Selected = false}
                };
            }
        }
    }
    public static class OLRSRelatedConstants
    {
        public static string PRA_MN_RPT_TAB_XL_PD = "PRA_MN_RPT_TAB_XL_PD";

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "PRA_MN_RPT_TAB_XL_PD", Value = PRA_MN_RPT_TAB_XL_PD, Selected = false},

                };
            }
        }
    }
    public static class EmploymentLoanCodeConstants
    {
        public static string _001_Jagoron = "001";
        public static string _002_Agrasor = "002";
        public static string _003_Buniad = "003";
        public static string _004_Others = "004";

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "001-Jagoron", Value = _001_Jagoron, Selected = false},
                    new ConstantDropdownItem {Text = "002-Agrasor", Value = _002_Agrasor, Selected = false},
                    new ConstantDropdownItem {Text = "003-Buniad", Value = _003_Buniad, Selected = false},
                    new ConstantDropdownItem {Text = "004-Others", Value = _004_Others, Selected = false}
                };
            }
        }
    }
    public static class LoanSavingsRateTypeConstants
    {
        public static string Loan_Service_Charge_Rate = "Loan Service Charge Rate";
        public static string Savings_Interest_Rate = "Savings Interest Rate";

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Loan Service Charge Rate", Value = Loan_Service_Charge_Rate, Selected = false},
                    new ConstantDropdownItem {Text = "Savings Interest Rate", Value = Savings_Interest_Rate, Selected = false},
                };
            }
        }
    }

    public static class MaleFemaleFlagConstants
    {
        public static string Male = "M";
        public static string Female = "F";
        public static string Neutral = "N";

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Male", Value = Male, Selected = false},
                    new ConstantDropdownItem {Text = "Female", Value = Female, Selected = false},
                    new ConstantDropdownItem {Text = "Neutral", Value = Neutral, Selected = true},
                };
            }
        }
    }
    public static class EmploymentProductConstants
    {
        public static string Agrosor = "Agrosor";
        public static string Buniad = "Buniad";
        public static string Jagoron = "Jagoron";

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Agrosor", Value = Agrosor, Selected = false},
                    new ConstantDropdownItem {Text = "Buniad", Value = Buniad, Selected = false},
                    new ConstantDropdownItem {Text = "Jagoron", Value = Jagoron, Selected = false},
                };
            }
        }
    }

    public static class OrganiationConstants
    {
        public static string BuroBangladesh = "00288";

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "BURO Bangladesh", Value = BuroBangladesh, Selected = false}
                };
            }
        }
    }
    public static class ProcessTypeConstants
    {
        public static string Process_All = "Process_All";
        public static string Program_Data = "Program_Data";
        public static string Basic_Data = "Basic_Data";
        public static string Financial_Data = "Financial_Data";
        public static string Upazilla_Loan = "Upazilla_Loan";
        public static string Balance_Sheet = "Balance_Sheet";
        public static string Trial_Balance = "Trial_Balance";
        public static string Employment_Data = "Employment_Data";
        public static string Top_Sheet = "Top_Sheet";

        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Process All", Value = Process_All, Selected = false},
                    new ConstantDropdownItem {Text = "Program Data", Value = Program_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Basic Data", Value = Basic_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Upazilla Loan", Value = Upazilla_Loan, Selected = false},
                    new ConstantDropdownItem {Text = "Balance Sheet", Value = Balance_Sheet, Selected = false},
                    new ConstantDropdownItem {Text = "Trial Balance [IE & RP]", Value = Trial_Balance, Selected = false},
                    new ConstantDropdownItem {Text = "Financial Data", Value = Financial_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Employment Data", Value = Employment_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Top Sheet", Value = Top_Sheet, Selected = false},

                };
            }
        }
    }

    public static class SyncToPKSFTypeConstants
    {
        public static string Sync_All = "Sync_All";
        public static string Program_Data = "Program_Data";
        public static string Financial_Data = "Financial_Data";
        public static string Basic_Data = "Basic_Data";
        public static string Upazilla_Loan = "Upazilla_Loan";

        public static string Accounting_BS_IE_RP = "Accounting_BS_IE_RP";

        /*
        public static string Balance_Sheet = "Balance_Sheet";
        public static string Income_Expenditure = "Income_Expenditure";
        public static string Receipt_Payment = "Receipt_Payment";
        */

        public static string ImputedCost_Header_Info = "Imputed_Cost_Header_Info";
        public static string ImputedCost_Loan_Code_Wise_Service_Change = "ImputedCost_Loan_Code_Wise_Service_Change";
        public static string ImputedCost_Savings_Interest_Info = "ImputedCost_Savings_Interest_Info";
        public static string ImputedCost_Inflation_Equity_Info = "ImputedCost_Inflation_Equity_Info";

        public static string Employment_Related_Last_Data = "Employment_Related_Last_Data";
        public static string Employment_Related_Last_Retained_Data = "Employment_Related_Last_Retained_Data";
        public static string Employment_Related_Current_Year_New_Data = "Employment_Related_Current_Year_New_Data";
        public static string Top_Sheet = "Top_Sheet";


        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Sync All", Value = Sync_All, Selected = false},
                    new ConstantDropdownItem {Text = "Program Data", Value = Program_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Financial Data", Value = Financial_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Basic Data", Value = Basic_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Upazilla Loan", Value = Upazilla_Loan, Selected = false},

                    new ConstantDropdownItem {Text = "Accounting BS IE RP", Value = Accounting_BS_IE_RP, Selected = false},

                    /*
                    new ConstantDropdownItem {Text = "Balance Sheet", Value = Balance_Sheet, Selected = false},
                    new ConstantDropdownItem {Text = "Income Expenditure", Value = Income_Expenditure, Selected = false},
                    new ConstantDropdownItem {Text = "Receipt Payment", Value = Receipt_Payment, Selected = false},
                    */

                    new ConstantDropdownItem {Text = "Imputed Cost Header Info", Value = ImputedCost_Header_Info, Selected = false},
                    new ConstantDropdownItem {Text = "Imputed Cost Loan Code Wise Service Change", Value = ImputedCost_Loan_Code_Wise_Service_Change, Selected = false},
                    new ConstantDropdownItem {Text = "Imputed Cost Savings Interest Info", Value = ImputedCost_Savings_Interest_Info, Selected = false},
                    new ConstantDropdownItem {Text = "Imputed Cost Inflation Equity Info", Value = ImputedCost_Inflation_Equity_Info, Selected = false},

                    new ConstantDropdownItem {Text = "Employment Related Last Data", Value = Employment_Related_Last_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Employment Related Last Retained Data", Value = Employment_Related_Last_Retained_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Employment Related Current Year New Data", Value = Employment_Related_Current_Year_New_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Top Sheet", Value = Top_Sheet, Selected = false},

                };
            }
        }
    }

    public static class NotSyncToPKSFTypeConstants
    {
        public static string Not_Sync_All = "Not_Sync_All";
        public static string Program_Data = "Program_Data";
        public static string Financial_Data = "Financial_Data";
        public static string Basic_Data = "Basic_Data";
        public static string Upazilla_Loan = "Upazilla_Loan";

        public static string Accounting_BS_IE_RP = "Accounting_BS_IE_RP";

        /*
        public static string Balance_Sheet = "Balance_Sheet";
        public static string Income_Expenditure = "Income_Expenditure";
        public static string Receipt_Payment = "Receipt_Payment";
        */

        public static string ImputedCost_Header_Info = "Imputed_Cost_Header_Info";
        public static string ImputedCost_Loan_Code_Wise_Service_Change = "ImputedCost_Loan_Code_Wise_Service_Change";
        public static string ImputedCost_Savings_Interest_Info = "ImputedCost_Savings_Interest_Info";
        public static string ImputedCost_Inflation_Equity_Info = "ImputedCost_Inflation_Equity_Info";

        public static string Employment_Related_Last_Data = "Employment_Related_Last_Data";
        public static string Employment_Related_Last_Retained_Data = "Employment_Related_Last_Retained_Data";
        public static string Employment_Related_Current_Year_New_Data = "Employment_Related_Current_Year_New_Data";
        public static string Top_Sheet = "Top_Sheet";


        public static IEnumerable<ConstantDropdownItem> Items
        {
            get
            {
                return new List<ConstantDropdownItem>
                {
                    new ConstantDropdownItem {Text = "Not Sync All", Value = Not_Sync_All, Selected = false},
                    new ConstantDropdownItem {Text = "Program Data", Value = Program_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Financial Data", Value = Financial_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Basic Data", Value = Basic_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Upazilla Loan", Value = Upazilla_Loan, Selected = false},

                    new ConstantDropdownItem {Text = "Accounting BS IE RP", Value = Accounting_BS_IE_RP, Selected = false},

                    /*
                    new ConstantDropdownItem {Text = "Balance Sheet", Value = Balance_Sheet, Selected = false},
                    new ConstantDropdownItem {Text = "Income Expenditure", Value = Income_Expenditure, Selected = false},
                    new ConstantDropdownItem {Text = "Receipt Payment", Value = Receipt_Payment, Selected = false},
                    */

                    new ConstantDropdownItem {Text = "Imputed Cost Header Info", Value = ImputedCost_Header_Info, Selected = false},
                    new ConstantDropdownItem {Text = "Imputed Cost Loan Code Wise Service Change", Value = ImputedCost_Loan_Code_Wise_Service_Change, Selected = false},
                    new ConstantDropdownItem {Text = "Imputed Cost Savings Interest Info", Value = ImputedCost_Savings_Interest_Info, Selected = false},
                    new ConstantDropdownItem {Text = "Imputed Cost Inflation Equity Info", Value = ImputedCost_Inflation_Equity_Info, Selected = false},

                    new ConstantDropdownItem {Text = "Employment Related Last Data", Value = Employment_Related_Last_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Employment Related Last Retained Data", Value = Employment_Related_Last_Retained_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Employment Related Current Year New Data", Value = Employment_Related_Current_Year_New_Data, Selected = false},
                    new ConstantDropdownItem {Text = "Top Sheet", Value = Top_Sheet, Selected = false},

                };
            }
        }
    }

    public static class OLRSAuthConstants
    {
        public static string ClientId = "nObMMomumwvXvlTy6KEfEbjKMbdsO";
        public static string ClientSecret = "jTP3FxMYV3oIosyOFvqLKktfRrbobqnEeKpO2787AY3gVpbOpeshkKop-";

        //live
        public static string ApiBaseUrl = "http://192.192.192.171";

        //local
        //public static string ApiBaseUrl = "http://localhost:1971";
    }

    public static class BulkSMSAuthConstants
    {
        public static string BulkSMSAuthClientKey = "KEY::nObMMomumwvXvlTy6KEfEbjKMbdsO";
        public static string ClientId = "nObMMomumwvXvlTy6KEfEbjKMbdsO";
        public static string ClientSecret = "jTP3FxMYV3oIosyOFvqLKktfRrbobqnEeKpO2787AY3gVpbOpeshkKop-";
        public static string BulkSMSAuthClientValue = $@"{ClientId }@{ClientSecret}";

        //live
        public static string ApiBaseUrl = "http://192.192.192.171";

        //local
        //public static string ApiBaseUrl = "http://localhost:59312";
    }

    public static class MFIConstants
    {
        public static int Society_For_Social_Service_SSS = 126;
    }

    public static class OLRSSyncDataCountConstants
    {
        public static int BASIC_DATA = 15;
        public static int PROGRAM_DATA = 88;
        public static int FINANCIAL_DATA = 11;
    }
}


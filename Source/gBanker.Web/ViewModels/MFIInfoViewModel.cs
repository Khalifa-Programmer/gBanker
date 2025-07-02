using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class MFIInfoViewModel
    {
        public int MFIId { get; set; }
        public string MFIName { get; set; }
        public string LicenseNo { get; set; }
        public string DateTo { get; set; }
        public string HOMailingAddress { get; set; }
        public string HODistrict { get; set; }
        public string HOThana { get; set; }
        public string HOUnion { get; set; }
        public string HOEmail { get; set; }
        public string HOWebAddress { get; set; }
        public string HOPhone { get; set; }
        public string HOMobile { get; set; }
        public string HOFax { get; set; }
        public string LOAddress { get; set; }
        public string LOTelephone { get; set; }
        public string LOMobile { get; set; }
        public string LOFax { get; set; }
        public string LOEmail { get; set; }
        public int? PrimaryRegistrationID { get; set; }
        public int? GBNoOfMaleMember { get; set; }
        public int? GBNoOfFemaleMember { get; set; }
        public int? GBNoOfYearlyMeetingsHeld { get; set; }
        public string GBDateOfLastMeeting { get; set; }
        public int? GBNoOfParticipantsInTheLastMeeting { get; set; }
        public string EBExpiryDate { get; set; }
        public int? EBNoOfMaleMember { get; set; }
        public int? EBNoOfFemaleMember { get; set; }
        public int? EBNoOfYearlyMeetingsHeld { get; set; }
        public string EBDateOfLastMeeting { get; set; }
        public int? EBNoOfParticipantsInTheLastMeeting { get; set; }
        public string ServiceRules { get; set; }
        public string FinancialPolicy { get; set; }
        public string SavingsAndCreditPolicy { get; set; }
        public string NISAndAntiMoneyLaunderingGuideline { get; set; }
        public string CitizenCharter { get; set; }
        public int? MaleMicroCreditProgramBranch { get; set; }
        public int? FemaleMicroCreditProgramBranch { get; set; }
        public int? MaleMicroCreditProgramArea { get; set; }
        public int? FemaleMicroCreditProgramArea { get; set; }
        public int? MaleMicroCreditProgramHO { get; set; }
        public int? FemaleMicroCreditProgramHO { get; set; }
        public int? MaleOrganizationBranch { get; set; }
        public int? FemaleOrganizationBranch { get; set; }
        public int? MaleOrganizationArea { get; set; }
        public int? FemaleOrganizationArea { get; set; }
        public int? MaleOrganizationHO { get; set; }
        public int? FemaleOrganizationHO { get; set; }
        public decimal? HighestMonthlySalary { get; set; }
        public string HighestMonthlySalaryDesignation { get; set; }
        public decimal? LowestMonthlySalary { get; set; }
        public string LowestMonthlySalaryDesignation { get; set; }
        public string OtherInformation { get; set; }
        public List<SelectListItem> PrimaryRegistrationList { get; set; }
        //public List<SelectListItem> TrustActList { get; set; }
        //public List<SelectListItem> CompanyActList { get; set; }
        //public List<SelectListItem> TheVoluntarySocialWelfareAgenciesList { get; set; }
        public List<SelectListItem> ServiceRulesList { get; set; }
        public List<SelectListItem> FinancialPolicyList { get; set; }
        public List<SelectListItem> SavingsAndCreditPolicyList { get; set; }
        public List<SelectListItem> NISAndAntiMoneyLaunderingGuidelineList { get; set; }
        public List<SelectListItem> CitizenCharterList { get; set; }
    }
}
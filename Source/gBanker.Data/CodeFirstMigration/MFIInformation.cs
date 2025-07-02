using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("MFIInformation")]
    public class MFIInformation
    {
        [Key]
        public int MFIId { get; set; }
        public string MFIName { get; set; }
        public string LicenseNo { get; set; }
        public DateTime DateTo { get; set; }
        public string HOMailingAddress { get; set;}
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
        public DateTime? GBDateOfLastMeeting { get; set; }
        public int? GBNoOfParticipantsInTheLastMeeting { get; set; }
        public DateTime? EBExpiryDate { get; set; }
        public int? EBNoOfMaleMember { get; set; }
        public int? EBNoOfFemaleMember { get; set; }
        public int? EBNoOfYearlyMeetingsHeld { get; set; }
        public DateTime? EBDateOfLastMeeting { get; set; }
        public int? EBNoOfParticipantsInTheLastMeeting { get; set; }
        public int? ServiceRules { get; set; }        
        public int? FinancialPolicy { get; set; }
        public int? SavingsAndCreditPolicy { get; set; }        
        public int? NISAndAntiMoneyLaunderingGuideline { get; set; }        
        public int? CitizenCharter { get; set; }        
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
        public bool? IsActive { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}

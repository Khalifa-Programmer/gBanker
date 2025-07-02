using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class NomineeXSavingSummary
    {
        public long SavingSummaryID { get; set; }
        public string NomineeName { get; set; }
        public string NFatherName { get; set; }
        public string NRelationName { get; set; }
        public string NAddressName { get; set; }
        public int NAlocation { get; set; }
        public int MemberNomineeId { get; set; }
    }
    public class SavingsAccountOpeningWithNomineeViewModel
    {
       
        public int OfficeID { get; set; }
       
        public long MemberID { get; set; }
       
        public short ProductID { get; set; }
     
        public int CenterID { get; set; }
 
        public int NoOfAccount { get; set; }
       
        public decimal InterestRate { get; set; }
       
        public decimal SavingInstallment { get; set; }
        
       // public System.DateTime OpeningDate { get; set; }
        
       // public Nullable<System.DateTime> MaturedDate { get; set; }
        //public string SavingAccountNo { get; set; }

        //public int Duration { get; set; }
        //public int InstallmentNo { get; set; }
        public int Ref_EmployeeID { get; set; }
        public string NomineeName { get; set; }
        public string NFatherName { get; set; }
        public string NRelationName { get; set; }
        public string NAddressName { get; set; }
        public int NAlocation { get; set; }


        public int MemberNomineeId { get; set; }
        public long SavingSummaryID { get; set; }
    }
}
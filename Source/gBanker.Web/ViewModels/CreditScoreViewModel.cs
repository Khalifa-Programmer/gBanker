using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class CreditScoreViewModel
    {

        public int? OfficeID { get; set; }

      
        public string CenterID { get; set; }

   
        public string ProductID { get; set; }

     
        public byte LoanTerm { get; set; }

    
        public string MemberID { get; set; }

     
        public string OfficeName { get; set; }

     
        public string EmpName { get; set; }


        public DateTime? JoingDate { get; set; }

      
        public string MemberName { get; set; }

       
        public DateTime? DOB { get; set; }

      
        public string RefereeName { get; set; }

     
        public string MemberAddress { get; set; }

       
        public string SSN { get; set; }

        public decimal PrincipalLoan { get; set; }


        public DateTime? DisburseDate { get; set; }

 
        public decimal LoanPaid { get; set; }

        public decimal LoanBalance { get; set; }

        public decimal InterestPaid { get; set; }

        public decimal LoanInstallment { get; set; }

        public decimal IntInstallment { get; set; }


        public string LoanItem { get; set; }


        public string PurposeName { get; set; }

  
        public DateTime? LastInstallmentDate { get; set; }

     
        public decimal? LoanPaid_ThisMonth { get; set; }

   
        public decimal? IntPaid_ThisMonth { get; set; }

        
        public short LoanDuration { get; set; }


        public short WeekPassed { get; set; }


        public short DropInstallment { get; set; }


        public DateTime? AccountCloseDate { get; set; }


        public DateTime? LastPaymentDate { get; set; }


        public decimal? LastLoanPaid { get; set; }


        public decimal? LastIntPaid { get; set; }


        public DateTime? ReportDate { get; set; }


        public string MemberFirstName { get; set; }

 
        public string MemberMiddleName { get; set; }

        public string MemberLastName { get; set; }

        public string StreetAddress1 { get; set; }

        public string StreetAddress2 { get; set; }


        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }


        public string TypeofID { get; set; }


        public string IDComments { get; set; }

  
        public string Race { get; set; }


        public string Ethnicity { get; set; }
    }
}
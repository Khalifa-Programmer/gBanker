using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class SavingInstallmentViewModel//:BaseModel
    {
        public long ImmatureLTSID { get; set; }
       
        public long SavingSummaryID { get; set; }
        
      
        public decimal? Calcnterest { get; set; }
        public decimal? Deposit { get; set; }

        public decimal? WithDrawal { get; set; }
        public decimal? Interest { get; set; }
        public bool Transffered { get; set; }
        public DateTime? TransDate { get; set; }

        public int ProductID { get; set; }
        public int OfficeID { get; set; }
        public decimal? CurrentInterest { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal? WithdrawalRate { get; set; }
        public DateTime? OpeningDate { get; set; }
        public decimal? SavingInstallment { get; set; }
        public int Duration { get; set; }
        
        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }


        public DateTime? CreateDate { get; set; }
        public int rowSl { get; set; }


    }// END Class
}// END NameSpace
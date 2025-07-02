using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class BatchPostingProcessViewModel :BaseModel
    {
        public long BatchId { get; set; }
        public int RowSl { get; set; }
       [Display(Name = "TransactionDate")]
       public DateTime TransactionDate { get; set; }
    
       public string TransactionDateMsg { get; set; }

       [Display(Name = "BatchFile" )]
        public string BatchFile { get; set; }
        [Display(Name = "UnitCode" )]
        public string OfficeCode { get; set; }
        [Display(Name = "UnitName")]
        public string OfficeName { get; set; }
         [Display(Name = "AccountCode")]
        public string AccountCode { get; set; }
         [Display(Name = "AccountName")]
        public string AccountName { get; set; }
         [Display(Name = "VoucherType")]
        public string VoucherType { get; set; }
         [Display(Name = "Narration")]
        public string Narration { get; set; }
         [Display(Name = "Credit")]
        public decimal Credit { get; set; }
         [Display(Name = "Debit")]
        public decimal Debit { get; set; }
       
        public bool? IsPosted { get; set; }
        public DateTime? PostedDate { get; set; }
        public int? PostedBy { get; set; }

        public int IsError { get; set; }
        public string ErrorMessage { get; set; }
        [Display(Name = "BatchFileNo")]
        public string BatchFileNo { get; set; }
        [Display(Name = "BatchFileDate")]
        public string BatchFileDate { get; set; }

        public decimal totalCredit { get; set; }
        public decimal totalDebit { get; set; }

    }
}
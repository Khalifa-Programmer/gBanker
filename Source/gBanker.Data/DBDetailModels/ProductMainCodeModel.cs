using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public  class ProductMainCodeModel
    { 
        public string MainProductCode { get; set; }      
        public string MainItemName { get; set; }
    }
    public class ProductCodeModel
    {
        public int ProductCode { get; set; }
        public string ProductName { get; set; }
    }
    public class PassbookEntryModel
    {  
        public long MemberWisePassbookBalanceEntryId { get; set; }
        public string OfficeName { get; set; }
        public string CenterName { get; set; }
        public string Member { get; set; }
        public string ProductName { get; set; }
        public decimal Amount { get; set; }
    }
}

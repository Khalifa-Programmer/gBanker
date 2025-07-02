using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class LoanCodeXProductMappingViewModel
    {
        public IEnumerable<POLoanCode> POLoanCodes { get; set; }
        public IEnumerable<POProductMapping> POProductMappings { get; set; }
        public IEnumerable<LOAN_PRODUCT> LoanProducts { get; set; }
        public IEnumerable<ProductViewModel> ProductList { get; set; }

    }

    public class AddOrEditLoanCodeXProductMappingViewModel
    {
        public AddOrEditLoanCodeXProductMappingViewModel()
        {
            this.LoanXProductMappings = new List<LoanXProductMapping>();
        }
        public List<LoanXProductMapping> LoanXProductMappings { get; set; }
    }
    public partial class LoanXProductMapping
    {
        public string LoanCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string AssociatedLoanCode { get; set; }
        public decimal LoanServiceChargeRate { get; set; }
    }
}
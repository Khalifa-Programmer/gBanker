using gBanker.Data.CodeFirstMigration;
using System.Collections.Generic;

namespace gBanker.Web.ViewModels
{
    public class LoanCodeXAccCodeMappingViewModel
    {
        public LoanCodeXAccCodeMappingViewModel()
        {
            this.LoanXAccCodeMappings = new List<LoanXAccCodeMapping>();
        }
        public List<LoanXAccCodeMapping> LoanXAccCodeMappings { get; set; }
    }
    public partial class LoanXAccCodeMapping
    {
        public string LoanCode { get; set; }
        public string FunctionalitiesAndFeatures { get; set; }
        public List<string> AssociatedAccCodeFA { get; set; }
        public List<string> AssociatedAccCodeSCP { get; set; } 
    }
}
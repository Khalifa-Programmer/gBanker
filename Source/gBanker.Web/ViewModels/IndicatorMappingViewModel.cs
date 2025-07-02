using gBanker.Data.CodeFirstMigration;
using System.Collections.Generic;

namespace gBanker.Web.ViewModels
{
    public class IndicatorMappingViewModel
    {
        public IndicatorMappingViewModel()
        {
            this.IndicatorMappings = new List<IndicatorMapping>();
        }
        public List<IndicatorMapping> IndicatorMappings { get; set; }
    }
    public partial class IndicatorMapping
    {
        public string IndicatorCode { get; set; }
        public string IndicatorName { get; set; }
        public List<string> AssociatedAccCodeFD { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace gBanker.Core.Filters
{
    public class AccChartSearchFilter : BaseSearchFilter
    {
        public string AccChartLevel { get; set; }
        public string AccCode { get; set; }
    }
}

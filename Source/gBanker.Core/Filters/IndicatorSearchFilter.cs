using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace gBanker.Core.Filters
{
    public class IndicatorSearchFilter : BaseSearchFilter
    {
        public string AssociatedTable { get; set; }
    }
}

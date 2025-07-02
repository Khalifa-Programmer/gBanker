using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Web.Core.Extensions
{
    public static class StringExtenstion
    {
        public static int ToInt(this string value)
        {
            return Convert.ToInt32(value);
        }        
    }
}

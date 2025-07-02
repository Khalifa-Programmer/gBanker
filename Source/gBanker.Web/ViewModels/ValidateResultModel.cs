using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class ValidateResultModel
    {
        public string SheetName { get; set; }
        public int RowSl { get; set; }
        public string ColumnName { get; set; }
        public string Message { get; set; }

    }

    public class ValidateColumn
    {
        public string Value { get; set; }
        public bool Result { get; set; }
    }

}
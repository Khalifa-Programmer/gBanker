using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class MemberPassBookStockViewModel : BaseModel
    {
        public long MemberPassBookStockID { get; set; }

        public int? OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public long? LotNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        public long? StartingNo { get; set; }

        public long? LastIssue { get; set; }

        public int OrgID { get; set; }

       
        public IEnumerable<SelectListItem> officeListItems { get; set; }
    }
}
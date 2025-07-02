using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DBCategoryTransferDetails
    {
        public long TransferHistoryID { get; set; }


        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public int? TrOfficeID { get; set; }

        public int? CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public int? TrCenterID { get; set; }

        public int? MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public int? TrMemberID { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TransferDate { get; set; }

        public int? MemberCategoryId { get; set; }

        public int? TrMemberCategoryID { get; set; }

        public int? ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int? TrProductID { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Charge { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Principal { get; set; }
    }
}

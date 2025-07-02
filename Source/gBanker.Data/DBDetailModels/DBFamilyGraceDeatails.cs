using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DBFamilyGraceDeatails
    {
        public int FamilyGraceID { get; set; }

        public int? OrgID { get; set; }
        public string OfficeCode { get; set; }
        public string MemberCode { get; set; }

        public string CenterCode { get; set; }
        public int? OfficeID { get; set; }

        public int? CenterID { get; set; }

        public long? MemberID { get; set; }

        public DateTime? GraceStartDate { get; set; }


        public DateTime? GraceEndDate { get; set; }


        public string Description { get; set; }
        public bool? IsActive { get; set; }


        public DateTime? InActiveDate { get; set; }


        public DateTime CreateDate { get; set; }

        public string CreateUser { get; set; }
    }
}

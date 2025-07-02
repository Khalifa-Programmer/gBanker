using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class getFamilyGrace_Result
    {
        public Nullable<int> FamilyGraceID { get; set; }
        public Nullable<int> OfficeID { get; set; }
        public string officeCode { get; set; }
        public Nullable<int> CenterID { get; set; }
        public string CenterCode { get; set; }
        public Nullable<long> MemberID { get; set; }
        public string MemberCode { get; set; }
        public Nullable<System.DateTime> GraceStartDate { get; set; }
        public Nullable<System.DateTime> GraceEndDate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}

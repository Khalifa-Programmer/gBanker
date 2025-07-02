using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class getPassBookRegister_Result
    {
        public long MemberPassBookRegisterID { get; set; }
        public int OfficeID { get; set; }
       
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public long MemberPassBookNO { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public Nullable<System.DateTime> PassBookStartDate { get; set; }
        public Nullable<System.DateTime> PassBookCloseDate { get; set; }
        public int Status { get; set; }
        public long LotNo { get; set; }
        
        public bool? IsActive { get; set; }

       
        public DateTime? InActiveDate { get; set; }

       
        public string CreateUser { get; set; }

      
        public DateTime CreateDate { get; set; }
    }
}

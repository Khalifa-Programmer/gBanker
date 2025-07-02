using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class getCenterMeetingSchedule_Result
    {
        public string centercode { get; set; }
        public string centername { get; set; }
        public string CenterAddress { get; set; }
        public string CollectionDay { get; set; }
        public short EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmpName { get; set; }
    }
}

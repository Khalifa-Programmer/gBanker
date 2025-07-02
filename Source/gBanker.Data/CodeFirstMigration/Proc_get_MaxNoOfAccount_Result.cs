using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class Proc_get_MaxNoOfAccount_Result
    {
        public int officeID { get; set; }
        public Nullable<int> NoOfAccount { get; set; }
    }
}

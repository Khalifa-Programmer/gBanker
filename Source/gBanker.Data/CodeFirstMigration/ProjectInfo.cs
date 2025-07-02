using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.ProjectInfo")]
    public partial class ProjectInfo
    {
        [Key]
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string FundingAgency { get; set; }        
        public string Description { get; set; }
        public int OfficeID { get; set; }
        public int OrgID { get; set; }
        public bool IsActive { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

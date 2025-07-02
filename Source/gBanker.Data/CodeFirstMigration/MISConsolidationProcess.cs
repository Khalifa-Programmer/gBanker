using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("MISConsolidationProcess")]
    public class MISConsolidationProcess
    {        
        [Key]
        public Int16? ConsolidationID { get; set; }        
        public Int16? ConsoQueue { get; set; }        
        public int? OfficeID { get; set; }       
        public DateTime? ProcessDate { get; set; }        
        public string CreateUser { get; set; }        
        public DateTime? CreateDate { get; set; }        
        public bool? IsRunning { get; set; }
    }
}

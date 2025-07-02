using gBankerCodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("ReportSpName")]
    public class ReportSpName
    {
        [Key]
        public int Id { get; set; }
        public string LabelName { get; set; }
        public string SpName { get; set; }
        public bool? IsActive { get; set; }
    }
}

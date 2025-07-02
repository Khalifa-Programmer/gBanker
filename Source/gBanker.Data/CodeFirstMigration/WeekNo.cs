using gBanker.Data.CodeFirstMigration.Db;
using gBankerCodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WeekNo")]
    public partial class WeekNo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long WeekNoID { get; set; }

        public int OrgID { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WeekYear { get; set; }

        [Key]
        [Column("WeekNoSl", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WeekNoSl { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public virtual Organization Organization { get; set; }
    }
}

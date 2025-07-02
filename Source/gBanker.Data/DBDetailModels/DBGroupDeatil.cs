using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public  class DBGroupDeatil
    {
        public short GroupID { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupCode { get; set; }

        public int OfficeID { get; set; }

        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }

        [Column(TypeName = "date")]
        public DateTime FormationDate { get; set; }

        public byte GroupStatus { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }
    }
}

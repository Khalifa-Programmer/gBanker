using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("FamilyMemberSameHousehold")]
    public class FamilyMemberSameHousehold
    {
        [Key]
        public long FamilyMemberId { get; set; }
        public long MemberId { get; set; }
        public string FamilyMemberName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Relation { get; set; }
        public string Education { get; set; }
        public string Occupation { get; set; }
        public string OccupationTime { get; set; }
        public bool IsActive { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

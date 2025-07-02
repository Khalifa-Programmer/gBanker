namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberBlackList")]
    public partial class MemberBlackList
    {
        [Key]
        public Int64 MemberBlackListID { get; set; }
        [NotMapped]
        public int CenterID { get; set; }
        public int? OfficeID { get; set; }
        public Int64? MemberID { get; set; }
        public string MemberCode { get; set; }
        public string NationalID { get; set; }
        public string PhoneNo { get; set; }
        public string SmartCard { get; set; }
        public string OtherIdNo { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

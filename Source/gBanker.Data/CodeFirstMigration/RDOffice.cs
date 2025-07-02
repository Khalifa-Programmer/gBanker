namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RDOffice")]
    public partial class RDOffice
    {
        [Key]
        public int RDID { get; set; }
        public int ParentID { get; set; }
        public string RDCode { get; set; }
        public string RDName { get; set; }
        public string RDAddress { get; set; }
        public string PostCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ActiveDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedIn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedIn { get; set; }
    }
}

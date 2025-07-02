namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AccMappingForFundTransfer")]
    public partial class AccMappingForFundTransfer
    {
        [Key]
        public int ID { get; set; }
        public string HOFundAccCode { get; set; }
        public string BRFundAccCode { get; set; }
        public bool IsActive { get; set; }
       
        public string CreateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? UpdateDate { get; set; }
    }
}

namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dbo.MemberWisePassbookBalanceEntry")]
    public partial class MemberWisePassbookBalanceEntry
    {
        [Key] 
        public Int64 MemberWisePassbookBalanceEntryId { get; set; }

        [Display(Name = "Office Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int OfficeId { get; set; }

        [Display(Name = "Center Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int CenterId { get; set; }

        [Display(Name = "Member Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int MemberId { get; set; }

        [Display(Name = "Product Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int ProductId { get; set; }

        [Display(Name = "Amount")]
        [Required(ErrorMessage = "{0} is Required")]
        public decimal Amount { get; set; }

        [Display(Name = "Is Active")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool IsActive { get; set; }

       
        [Required]
        [StringLength(15)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}

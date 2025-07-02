using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class CumAISViewModel:BaseModel
    {

        [Key]
        public int CumAisID { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime AISDate { get; set; }

        [Required]
        [StringLength(50)]
        public string VoucherNo { get; set; }

        [Required]
        [StringLength(50)]
        public string OfficeID { get; set; }

        [Display(Name = "Office Code")]
        public String OfficeCode { get; set; }

        [Required]
        [StringLength(50)]
        public string AccCode { get; set; }

        [StringLength(200)]
        public string Naration { get; set; }

        [StringLength(50)]
        public string ReconPurposeCode { get; set; }

        [StringLength(50)]
        public string Reference { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Debit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Credit { get; set; }

        [StringLength(50)]
        public string VoucherType { get; set; }

        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> VoucherTypeList { get; set; }
        public IEnumerable<SelectListItem> ReconPurposeList { get; set; }
        public IEnumerable<SelectListItem> AccCodeList { get; set; }
        

     
    }
}
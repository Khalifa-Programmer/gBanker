using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public partial class AccCorrectionViewModel
    {
        public int? AccID { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string AccCode { get; set; }

        [StringLength(100)]
        public string AccName { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Debit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Credit { get; set; }

        public DateTime? TransferTrxDate { get; set; }

        [StringLength(35)]
        public string CreateUser { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string CurrentAccCode { get; set; }

        [StringLength(100)]
        public string CurrentAccName { get; set; }

        public decimal? CurrentCredit { get; set; }

        public decimal? CurrentDebit { get; set; }

        [StringLength(10)]
        public string EmployeeCode { get; set; }

        [StringLength(50)]
        public string EmpName { get; set; }

        public DateTime? CreateDate { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string OfficeCode { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(40)]
        public string OfficeName { get; set; }
    }
}
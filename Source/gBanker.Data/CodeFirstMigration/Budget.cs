namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Budget")]
    public partial class Budget
    {
        public int BudgetID { get; set; }

        public int OrgID { get; set; }

        public int OfficeID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TrxDate { get; set; }

        public int? BudgetYear { get; set; }

        public int AccID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal BudgetAmount { get; set; }

        public int? BudgetType { get; set; }
        public bool IsFinancial { get; set; }
        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}

//namespace gBanker.Data.CodeFirstMigration
//{
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel.DataAnnotations;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Data.Entity.Spatial;

//    [Table("Budget")]
//    public partial class Budget
//    {
//        public int BudgetID { get; set; }

//        public int OrgID { get; set; }

//        public int OfficeID { get; set; }

//        [Column(TypeName = "date")]
//        public DateTime? TrxDate { get; set; }

//        public int? BudgetYear { get; set; }

//        public int AccID { get; set; }

//        [Column(TypeName = "numeric")]
//        public decimal BudgetAmount { get; set; }

//        public int? BudgetType { get; set; }

//        public bool? IsActive { get; set; }

//        [Column(TypeName = "smalldatetime")]
//        public DateTime? InActiveDate { get; set; }

//        [Required]
//        [StringLength(35)]
//        public string CreateUser { get; set; }

//        [Column(TypeName = "smalldatetime")]
//        public DateTime CreateDate { get; set; }
//    }
//}

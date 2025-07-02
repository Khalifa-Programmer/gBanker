namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PNMConfirm")]
    public partial class PNMConfirm
    {
        [Key]
        public int pnm_confirm_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? entry_dt { get; set; }

        public decimal? due_to_site_amount { get; set; }

        [StringLength(10)]
        public string due_to_site_currency { get; set; }

        public decimal? net_payment_amount { get; set; }

        [StringLength(10)]
        public string net_payment_currency { get; set; }

        [StringLength(50)]
        public string order_payee_identifier { get; set; }

        public decimal? payment_amount { get; set; }

        [StringLength(10)]
        public string payment_currency { get; set; }

        [StringLength(50)]
        public string payment_timestamp { get; set; }
        public DateTime? payment_timestamp_dt { get; set; }

        [StringLength(50)]
        public string pnm_order_identifier { get; set; }

        [StringLength(50)]
        public string pnm_payment_identifier { get; set; }

        public decimal? pnm_withheld_amount { get; set; }

        [StringLength(10)]
        public string pnm_withheld_currency { get; set; }

        [StringLength(50)]
        public string signature { get; set; }

        [StringLength(50)]
        public string site_customer_identifier { get; set; }

        [StringLength(50)]
        public string site_identifier { get; set; }

        public bool? standin { get; set; }

        [StringLength(50)]
        public string status { get; set; }

        public bool? test { get; set; }

        [StringLength(50)]
        public string timestamp { get; set; }

        [StringLength(10)]
        public string version { get; set; }
    }
}

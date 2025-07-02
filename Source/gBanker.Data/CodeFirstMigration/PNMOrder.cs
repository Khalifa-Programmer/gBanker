namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PNMOrder")]
    public partial class PNMOrder
    {
        [Key]
        public long pnm_order_id { get; set; }

        public long? loan_disburse_id { get; set; }

        public long? loan_coll_id { get; set; }

        public string pnm_customer_address { get; set; }

        [StringLength(200)]
        public string pnm_customer_identifier { get; set; }

        [StringLength(100)]
        public string pnm_customer_postal_code { get; set; }

        [StringLength(200)]
        public string site_customer_identifier { get; set; }

        [StringLength(500)]
        public string latitude { get; set; }

        [StringLength(500)]
        public string longitude { get; set; }

        public decimal? minimum_payment_amount { get; set; }

        [StringLength(200)]
        public string minimum_payment_currency { get; set; }

        public decimal? order_amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? order_created { get; set; }

        [StringLength(200)]
        public string order_currency { get; set; }

        public bool? order_is_standing { get; set; }

        [StringLength(100)]
        public string order_status { get; set; }

        public string order_tracking_url { get; set; }

        [StringLength(200)]
        public string order_type { get; set; }

        public decimal? pnm_balance_due_amount { get; set; }

        [StringLength(200)]
        public string pnm_balance_due_currency { get; set; }

        [StringLength(100)]
        public string pnm_customer_language { get; set; }

        [StringLength(200)]
        public string pnm_order_crid { get; set; }

        [StringLength(200)]
        public string pnm_order_identifier { get; set; }

        [StringLength(200)]
        public string pnm_order_short_identifier { get; set; }

        public bool? require_auth_tracker { get; set; }

        [StringLength(500)]
        public string retailer_name { get; set; }

        [StringLength(500)]
        public string slip_id { get; set; }

        [StringLength(200)]
        public string site_identifier { get; set; }

        [StringLength(500)]
        public string site_name { get; set; }

        [StringLength(500)]
        public string site_order_key { get; set; }
        public bool is_active { get; set; }

        public DateTime? inactive_date { get; set; }

        public int? create_user { get; set; }

        public DateTime? create_date { get; set; }

        public int? update_user { get; set; }

        public DateTime? update_date { get; set; }
    }
}

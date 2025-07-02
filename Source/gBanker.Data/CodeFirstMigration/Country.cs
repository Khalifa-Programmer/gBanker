namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Country")]
    public partial class Country
    {
        public int CountryId { get; set; }

        [StringLength(50)]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(100)]
        public string CountryName { get; set; }

        [Required]
        [StringLength(10)]
        public string CountryShortCode { get; set; }

        [Required]
        [StringLength(10)]
        public string isoCode3 { get; set; }

        public bool Status { get; set; }
    }
}

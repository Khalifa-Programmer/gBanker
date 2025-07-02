namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductXEmploymentProductMapping")]
    public partial class ProductXEmploymentProductMapping
    {
        [Key]
        public int MappingId { get; set; }
        public string MainProductCode { get; set; }
        public string MainProductName { get; set; }
        public int DisplayOrder { get; set; }
        public string EmploymentProductName { get; set; }
     }
    
}

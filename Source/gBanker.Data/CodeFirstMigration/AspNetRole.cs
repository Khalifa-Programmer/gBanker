namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetRoleModules = new HashSet<AspNetRoleModule>();
            AspNetUsers = new HashSet<AspNetUser>();
        }
        public string Id { get; set; }
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(500)]
        public string DefaultLinkURL { get; set; }
        public int OrgID { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual ICollection<AspNetRoleModule> AspNetRoleModules { get; set; }
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}

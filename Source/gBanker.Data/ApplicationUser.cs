using gBanker.Data.CodeFirstMigration.Db;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            DateCreated = DateTime.Now;
        }
                
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfilePicUrl { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public bool Activated { get; set; }

        public int RoleId { get; set; }

        public Int16? EmployeeID { get; set; }

        public DateTime UpdateDate { get; set; }

        public string DisplayName
        {
            get { return FirstName + " " + LastName; }
        }
       // public virtual Employee Employee { get; set; }
    }
}

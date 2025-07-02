using gBankerCodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class EmployeeDetail
    {
        public short EmployeeID { get; set; }

     
        public string EmployeeCode { get; set; }

        public int OfficeID { get; set; }

        public string EmpName { get; set; }

    
        public string EmpNameBen { get; set; }

     
        public string GuardianName { get; set; }

        public string EmpAddress { get; set; }

     
        public string PhoneNo { get; set; }

    
        public string Email { get; set; }

     
        public string Gender { get; set; }

     
        public DateTime? BirthDate { get; set; }

     
        public string Designation { get; set; }

     
        public DateTime JoiningDate { get; set; }

        public byte EmployeeStatus { get; set; }

     
        public DateTime? ReleaseDate { get; set; }

        public bool? IsActive { get; set; }

  
        public DateTime? InActiveDate { get; set; }

       
        public DateTime CreateDate { get; set; }

      
        public string CreateUser { get; set; }

        public string OfficeCode { get; set; }
        public int OrgID { get; set; }
    }
}

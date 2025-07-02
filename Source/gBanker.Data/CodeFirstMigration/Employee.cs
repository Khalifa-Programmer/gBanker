namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        public short EmployeeID { get; set; }

        [Required]
        [StringLength(10)]
        public string EmployeeCode { get; set; }

        public int OfficeID { get; set; }

        [Required]
        [StringLength(40)]
        public string EmpName { get; set; }

        [StringLength(50)]
        public string EmpNameBen { get; set; }

        [StringLength(40)]
        public string GuardianName { get; set; }

        [Required]
        [StringLength(155)]
        public string EmpAddress { get; set; }

        [StringLength(35)]
        public string PhoneNo { get; set; }

        [StringLength(55)]
        public string Email { get; set; }

        [Required]
        [StringLength(7)]
        public string Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Designation { get; set; }

        [Column(TypeName = "date")]
        public DateTime JoiningDate { get; set; }

        public byte EmployeeStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ReleaseDate { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }
      
        //public string OfficeCode { get; set; }
        public int OrgID { get; set; }
        public virtual Organization Organization { get; set; }

        public virtual Office Office { get; set; }

        public int? DesignationID { get; set; }
    }
}

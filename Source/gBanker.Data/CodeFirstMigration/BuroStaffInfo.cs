using gBankerCodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("BuroStaffInfo")]
    public class BuroStaffInfo
    {
        [Key]
        public int ID { get; set; }
        public int? Sl { get; set; }
        public string BranchCode { get; set; }
        public string StaffName { get; set; }
        public string StaffPin { get; set; }
        public string Designation { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HouseRent { get; set; }
        public decimal? Medical { get; set; }
        public decimal? Comm_Collection { get; set; }
        public decimal? Comm_Mobile { get; set; }
        public decimal? Comm_Convenience { get; set; }
        public decimal? TotalSalary { get; set; }
        public decimal? SelfPFContribution { get; set; }
        public decimal? StaffBima { get; set; }
        public decimal? Health { get; set; }
        public decimal? NetSalaryPayment { get; set; }
        public decimal? OfficePF { get; set; }
        public decimal? PaymentPFBoard { get; set; }
        public decimal? TotalSalary_OtherAllownce { get; set; }


    }
}

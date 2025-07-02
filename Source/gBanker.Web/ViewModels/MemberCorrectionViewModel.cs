using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class MemberCorrectionViewModel
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OfficeID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string OfficeCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(40)]
        public string OfficeName { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string CenterCode { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string CenterName { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string NewCenterCode { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string NewCenterName { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MemberID { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(20)]
        public string MemberCode { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(107)]
        public string MemName { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(107)]
        public string NewMemName { get; set; }

        [StringLength(151)]
        public string Address { get; set; }

        [StringLength(151)]
        public string NewAddress { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(5)]
        public string MemberCategoryCode { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(5)]
        public string NewMemberCategory { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(7)]
        public string Gender { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(7)]
        public string NewGender { get; set; }

        [StringLength(45)]
        public string RefereeName { get; set; }

        [StringLength(45)]
        public string NewRef { get; set; }

        [Key]
        [Column(Order = 15)]
        [StringLength(20)]
        public string NationalID { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(20)]
        public string NewNationalID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NewBirthDate { get; set; }

        [Key]
        [Column(Order = 17, TypeName = "date")]
        public DateTime JoinDate { get; set; }

        [Key]
        [Column(Order = 18, TypeName = "date")]
        public DateTime NewJoinDate { get; set; }

        [StringLength(35)]
        public string PhoneNo { get; set; }

        [StringLength(35)]
        public string NewPhoneNo { get; set; }

        [Key]
        [Column(Order = 19)]
        [StringLength(15)]
        public string CreateUser { get; set; }

        [Key]
        [Column(Order = 20, TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [Key]
        [Column(Order = 21)]
        [StringLength(10)]
        public string EmployeeCode { get; set; }

        [Key]
        [Column(Order = 22)]
        [StringLength(50)]
        public string EmpName { get; set; }
    }
}
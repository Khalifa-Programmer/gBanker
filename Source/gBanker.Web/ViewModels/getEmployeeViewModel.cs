using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class getEmployeeViewModel :BaseModel
    {
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public int OfficeID { get; set; }
        public string OfficeName { get; set; }
        public string EmpName { get; set; }
        public string GuardianName { get; set; }
        public string EmpAddress { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Designation { get; set; }
        public System.DateTime JoiningDate { get; set; }
        public byte EmployeeStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
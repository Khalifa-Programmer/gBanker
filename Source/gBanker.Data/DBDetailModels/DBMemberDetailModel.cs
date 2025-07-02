
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DBMemberDetailModel
    {
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string OldMemberCode { get; set; }
        public int OfficeID { get; set; }
        public int NewOfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public int CenterID { get; set; }
        public int NewCenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public short GroupID { get; set; }
        public short NewGroupID { get; set; }
        public string GroupCode { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string RefereeName { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public System.DateTime JoinDate { get; set; }
        //public String ReleaseDate { get; set; }
        public string Gender { get; set; }
        public string NationalID { get; set; }
        public string Location { get; set; }
        public int GeoLocationID { get; set; }
        public string LocationName { get; set; }
        public byte MemberCategoryID { get; set; }
        public string MemberCategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string CategoryShortName { get; set; }
        public string MemberStatus { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string ZipCode { get; set; }
        public string CountryOfIssue { get; set; }
        public string NIDComments { get; set; }
        public string IDType { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string nsAccountNo { get; set; }
        public string MemCategory { get; set; }
        public string MaritalStatus { get; set; }
        public Nullable<byte> MemberType { get; set; }

        public decimal AdmissionFee { get; set; }
        public decimal PassBookFee { get; set; }
        public decimal LoanFormFee { get; set; }
        public string MemberNameBng { get; set; }
        public string DivisionCode { get; set; }
        //public string CategoryShortName { get; set; }

        public string SmartCard { get; set; }
        public string OtherIdNo { get; set; }
        [NotMapped]
        public string NidSmartOther { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public int? FamilyMember { get; set; }
        public string SpouseName { get; set; }
        public string FamilyContactNo { get; set; }
        public int? EmployeeID { get; set; }

        public string NIDText { get; set; }


    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace gBanker.Data.DBDetailModels
//{
//    public class DBMemberDetailModel
//    {
//        public long MemberID { get; set; }
//        public string MemberCode { get; set; }
//        public string OldMemberCode { get; set; }
//        public int OfficeID { get; set; }
//        public int NewOfficeID { get; set; }
//        public string OfficeCode { get; set; }
//        public string OfficeName { get; set; }
//        public int CenterID { get; set; }
//        public int NewCenterID { get; set; }
//        public string CenterCode { get; set; }
//        public string CenterName { get; set; }
//        public short GroupID { get; set; }
//        public short NewGroupID { get; set; }
//        public string GroupCode { get; set; }
//        public string FullName { get; set; }
//        public string FirstName { get; set; }
//        public string MiddleName { get; set; }
//        public string LastName { get; set; }
//        public string AddressLine1 { get; set; }
//        public string AddressLine2 { get; set; }
//        public string RefereeName { get; set; }
//        public Nullable<System.DateTime> BirthDate { get; set; }
//        public System.DateTime JoinDate { get; set; }
//        //public String ReleaseDate { get; set; }
//        public string Gender { get; set; }
//        public string NationalID { get; set; }
//        public string Location { get; set; }
//        public int GeoLocationID { get; set; }
//        public string LocationName { get; set; }
//        public byte MemberCategoryID { get; set; }
//        public string MemberCategoryCode { get; set; }
//        public string CategoryName { get; set; }
//        public string CategoryShortName { get; set; }
//        public string MemberStatus { get; set; }
//        public Nullable<System.DateTime> ReleaseDate { get; set; }
//        public string City { get; set; }
//        public string StateName { get; set; }
//        public string ZipCode { get; set; }
//        public string CountryOfIssue { get; set; }
//        public string NIDComments { get; set; }
//        public string IDType { get; set; }
//        public string Race { get; set; }
//        public string Ethnicity { get; set; }
//        public string Email { get; set; }
//        public string PhoneNo { get; set; }
//        public string nsAccountNo { get; set; }        
//        public string MemCategory { get; set; }        
//        public string MaritalStatus { get; set; }
//        public Nullable<byte> MemberType { get; set; }

//        public decimal AdmissionFee { get; set; }
//        public decimal PassBookFee { get; set; }
//        public decimal LoanFormFee { get; set; }
//        public string MemberNameBng { get; set; }
//        public string DivisionCode { get; set; }
//        //public string CategoryShortName { get; set; }

//        public string SmartCard { get; set; }
//        public string OtherIdNo { get; set; }

//    }
//}

